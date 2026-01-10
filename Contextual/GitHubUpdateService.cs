using Octokit;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Contextual
{
    /// <summary>
    /// Service for checking and downloading updates from GitHub releases
    /// </summary>
    public class GitHubUpdateService
    {
        private readonly string _owner;
        private readonly string _repository;
        private readonly GitHubClient _client;
        private readonly HttpClient _httpClient;

        public event EventHandler<DownloadProgressEventArgs>? DownloadProgressChanged;
        public event EventHandler<DownloadCompletedEventArgs>? DownloadCompleted;

        public GitHubUpdateService(string owner, string repository)
        {
            _owner = owner;
            _repository = repository;
            _client = new GitHubClient(new ProductHeaderValue("Contextual-Updater"));
            _httpClient = new HttpClient();
        }

        /// <summary>
        /// Represents update information from GitHub
        /// </summary>
        public class UpdateInfo
        {
            public bool IsUpdateAvailable { get; set; }
            public string CurrentVersion { get; set; } = string.Empty;
            public string LatestVersion { get; set; } = string.Empty;
            public string ReleaseNotes { get; set; } = string.Empty;
            public string DownloadUrl { get; set; } = string.Empty;
            public string AssetName { get; set; } = string.Empty;
            public long AssetSize { get; set; }
            public DateTime PublishedAt { get; set; }
            public string ReleaseName { get; set; } = string.Empty;
        }

        /// <summary>
        /// Check for updates from GitHub releases
        /// </summary>
        public async Task<UpdateInfo> CheckForUpdateAsync()
        {
            var updateInfo = new UpdateInfo
            {
                CurrentVersion = System.Windows.Forms.Application.ProductVersion
            };

            try
            {
                // Get the latest release from GitHub
                var releases = await _client.Repository.Release.GetAll(_owner, _repository);
                var latestRelease = releases.FirstOrDefault(r => !r.Prerelease && !r.Draft);

                if (latestRelease == null)
                {
                    // No releases found, try to get any release
                    latestRelease = releases.FirstOrDefault();
                }

                if (latestRelease == null)
                {
                    return updateInfo;
                }

                // Clean version string (remove 'v' prefix if present)
                string latestVersionString = latestRelease.TagName.TrimStart('v', 'V');
                updateInfo.LatestVersion = latestVersionString;
                updateInfo.ReleaseNotes = latestRelease.Body ?? string.Empty;
                updateInfo.ReleaseName = latestRelease.Name ?? latestVersionString;
                updateInfo.PublishedAt = latestRelease.PublishedAt?.DateTime ?? DateTime.Now;

                // Find installer asset (prefer .msi, then .exe, then .zip)
                var installerAsset = latestRelease.Assets
                    .FirstOrDefault(a => a.Name.EndsWith(".msi", StringComparison.OrdinalIgnoreCase))
                    ?? latestRelease.Assets
                    .FirstOrDefault(a => a.Name.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
                    ?? latestRelease.Assets
                    .FirstOrDefault(a => a.Name.EndsWith(".zip", StringComparison.OrdinalIgnoreCase));

                if (installerAsset != null)
                {
                    updateInfo.DownloadUrl = installerAsset.BrowserDownloadUrl;
                    updateInfo.AssetName = installerAsset.Name;
                    updateInfo.AssetSize = installerAsset.Size;
                }

                // Compare versions
                updateInfo.IsUpdateAvailable = CompareVersions(updateInfo.CurrentVersion, latestVersionString) < 0;

                return updateInfo;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error checking for updates: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Download the update file with progress reporting
        /// </summary>
        public async Task<string> DownloadUpdateAsync(string downloadUrl, string assetName, CancellationToken cancellationToken = default)
        {
            string downloadFolderPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                "Documents",
                "contextual");

            if (!Directory.Exists(downloadFolderPath))
            {
                Directory.CreateDirectory(downloadFolderPath);
            }

            string downloadPath = Path.Combine(downloadFolderPath, assetName);

            try
            {
                using var response = await _httpClient.GetAsync(downloadUrl, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
                response.EnsureSuccessStatusCode();

                var totalBytes = response.Content.Headers.ContentLength ?? -1;
                var totalBytesRead = 0L;
                var buffer = new byte[8192];

                using var contentStream = await response.Content.ReadAsStreamAsync(cancellationToken);
                using var fileStream = new FileStream(downloadPath, System.IO.FileMode.Create, FileAccess.Write, FileShare.None, 8192, true);

                while (true)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length, cancellationToken);
                    if (bytesRead == 0)
                        break;

                    await fileStream.WriteAsync(buffer, 0, bytesRead, cancellationToken);
                    totalBytesRead += bytesRead;

                    var progressPercentage = totalBytes > 0 ? (int)((totalBytesRead * 100) / totalBytes) : -1;
                    OnDownloadProgressChanged(new DownloadProgressEventArgs(totalBytesRead, totalBytes, progressPercentage));
                }

                OnDownloadCompleted(new DownloadCompletedEventArgs(true, downloadPath, null));
                return downloadPath;
            }
            catch (OperationCanceledException)
            {
                // Clean up partial download
                if (File.Exists(downloadPath))
                {
                    try { File.Delete(downloadPath); } catch { }
                }
                OnDownloadCompleted(new DownloadCompletedEventArgs(false, null, new OperationCanceledException("Download cancelled")));
                throw;
            }
            catch (Exception ex)
            {
                OnDownloadCompleted(new DownloadCompletedEventArgs(false, null, ex));
                throw;
            }
        }

        /// <summary>
        /// Compare two version strings
        /// </summary>
        private int CompareVersions(string currentVersion, string latestVersion)
        {
            try
            {
                // Parse versions, handling both "x.x.x" and "x.x.x.x" formats
                var current = ParseVersion(currentVersion);
                var latest = ParseVersion(latestVersion);

                return current.CompareTo(latest);
            }
            catch
            {
                // Fallback to string comparison
                return string.Compare(currentVersion, latestVersion, StringComparison.OrdinalIgnoreCase);
            }
        }

        private Version ParseVersion(string versionString)
        {
            // Remove any non-numeric prefix/suffix
            var cleanVersion = new string(versionString.Where(c => char.IsDigit(c) || c == '.').ToArray());
            
            // Ensure we have at least 2 parts
            var parts = cleanVersion.Split('.');
            while (parts.Length < 2)
            {
                cleanVersion += ".0";
                parts = cleanVersion.Split('.');
            }

            return Version.Parse(cleanVersion);
        }

        protected virtual void OnDownloadProgressChanged(DownloadProgressEventArgs e)
        {
            DownloadProgressChanged?.Invoke(this, e);
        }

        protected virtual void OnDownloadCompleted(DownloadCompletedEventArgs e)
        {
            DownloadCompleted?.Invoke(this, e);
        }
    }

    /// <summary>
    /// Event args for download progress
    /// </summary>
    public class DownloadProgressEventArgs : EventArgs
    {
        public long BytesReceived { get; }
        public long TotalBytesToReceive { get; }
        public int ProgressPercentage { get; }

        public DownloadProgressEventArgs(long bytesReceived, long totalBytesToReceive, int progressPercentage)
        {
            BytesReceived = bytesReceived;
            TotalBytesToReceive = totalBytesToReceive;
            ProgressPercentage = progressPercentage;
        }
    }

    /// <summary>
    /// Event args for download completed
    /// </summary>
    public class DownloadCompletedEventArgs : EventArgs
    {
        public bool Success { get; }
        public string? FilePath { get; }
        public Exception? Error { get; }

        public DownloadCompletedEventArgs(bool success, string? filePath, Exception? error)
        {
            Success = success;
            FilePath = filePath;
            Error = error;
        }
    }
}
