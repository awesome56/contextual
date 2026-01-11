using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Runtime.InteropServices;

namespace Contextual
{
    public partial class Updateloader : Form
    {
        private CancellationTokenSource? _cancellationTokenSource;
        private GitHubUpdateService? _updateService;
        private GitHubUpdateService.UpdateInfo? _updateInfo;
        private bool _isDownloading = false;

        // GitHub repository information - UPDATE THESE VALUES
        private const string GITHUB_OWNER = "awesome56";
        private const string GITHUB_REPO = "Contextual";

        // For dragging the borderless form
        private bool _dragging = false;
        private Point _dragCursorPoint;
        private Point _dragFormPoint;

        public Updateloader()
        {
            InitializeComponent();
            SetupForm();
        }

        private void SetupForm()
        {
            // Enable double buffering for smoother rendering
            this.DoubleBuffered = true;

            // Add shadow effect
            this.Paint += (s, e) => DrawFormShadow(e.Graphics);
        }

        private void DrawFormShadow(Graphics g)
        {
            // Draw subtle shadow around the form edges
            using (var pen = new Pen(Color.FromArgb(30, 0, 0, 0), 1))
            {
                g.DrawRectangle(pen, 0, 0, this.Width - 1, this.Height - 1);
            }
        }

        private void Updateloader_Load(object sender, EventArgs e)
        {
            // Initialize UI state
            lbcurr.Text = Application.ProductVersion;
            lblStatus.Text = "Checking for updates...";
            pnlVersionInfo.Visible = false;
            pnlProgress.Visible = false;
            btnUpdate.Visible = false;
            lblReleaseNotes.Visible = false;
            txtReleaseNotes.Visible = false;
            btnCancel.Text = "Close";

            // Initialize the GitHub update service
            _updateService = new GitHubUpdateService(GITHUB_OWNER, GITHUB_REPO);

            // Start checking for updates
            CheckForUpdatesAsync();
        }

        private async void CheckForUpdatesAsync()
        {
            try
            {
                lblStatus.Text = "Connecting to GitHub...";

                if (_updateService == null)
                {
                    _updateService = new GitHubUpdateService(GITHUB_OWNER, GITHUB_REPO);
                }

                _updateInfo = await _updateService.CheckForUpdateAsync();

                if (_updateInfo.IsUpdateAvailable)
                {
                    ShowUpdateAvailable(_updateInfo);
                }
                else
                {
                    ShowUpToDate();
                }
            }
            catch (Exception ex)
            {
                ShowError($"Failed to check for updates: {ex.Message}");
            }
        }

        private void ShowUpdateAvailable(GitHubUpdateService.UpdateInfo updateInfo)
        {
            lblStatus.Text = "?? A new version is available!";
            lblStatus.ForeColor = Color.FromArgb(99, 71, 153);

            // Show version info panel
            pnlVersionInfo.Visible = true;
            lbcurr.Text = updateInfo.CurrentVersion;
            lbnew.Text = updateInfo.LatestVersion;

            if (updateInfo.PublishedAt != default)
            {
                lblReleaseDate.Text = $"Released: {updateInfo.PublishedAt:MMMM dd, yyyy}";
            }

            // Show release notes if available
            if (!string.IsNullOrWhiteSpace(updateInfo.ReleaseNotes))
            {
                lblReleaseNotes.Visible = true;
                txtReleaseNotes.Visible = true;
                txtReleaseNotes.Text = updateInfo.ReleaseNotes;

                // Adjust layout for release notes
                lblReleaseNotes.Location = new Point(20, 170);
                txtReleaseNotes.Location = new Point(20, 195);
            }

            // Show update button
            btnUpdate.Visible = true;
            btnUpdate.Text = "Download && Install";
            btnUpdate.Enabled = true;
            btnCancel.Text = "Not Now";
        }

        private void ShowUpToDate()
        {
            lblStatus.Text = "? Your application is up to date!";
            lblStatus.ForeColor = Color.FromArgb(40, 167, 69);

            // Show version info
            pnlVersionInfo.Visible = true;
            lbcurr.Text = Application.ProductVersion;
            lbnew.Text = Application.ProductVersion;
            lblNewVersionLabel.Text = "Latest Version:";
            lblReleaseDate.Text = "";

            btnUpdate.Visible = false;
            btnCancel.Text = "Close";
        }

        private void ShowError(string message)
        {
            lblStatus.Text = $"? {message}";
            lblStatus.ForeColor = Color.FromArgb(220, 53, 69);

            pnlVersionInfo.Visible = false;
            btnUpdate.Visible = false;
            btnCancel.Text = "Close";
        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            if (_updateInfo == null || string.IsNullOrEmpty(_updateInfo.DownloadUrl))
            {
                MessageBox.Show("No download URL available.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (_updateService == null)
            {
                MessageBox.Show("Update service not initialized.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                _isDownloading = true;
                _cancellationTokenSource = new CancellationTokenSource();

                // Update UI for download state
                btnUpdate.Enabled = false;
                btnUpdate.Text = "Downloading...";
                btnCancel.Text = "Cancel Download";
                lblReleaseNotes.Visible = false;
                txtReleaseNotes.Visible = false;

                // Show progress panel
                pnlProgress.Visible = true;
                progressBar.Value = 0;
                lblProgressText.Text = "Starting download...";

                // Subscribe to progress events
                _updateService.DownloadProgressChanged += UpdateService_DownloadProgressChanged;
                _updateService.DownloadCompleted += UpdateService_DownloadCompleted;

                // Start download
                string downloadedFilePath = await _updateService.DownloadUpdateAsync(
                    _updateInfo.DownloadUrl,
                    _updateInfo.AssetName,
                    _cancellationTokenSource.Token);

                // Download completed successfully - install the update
                InstallUpdate(downloadedFilePath);
            }
            catch (OperationCanceledException)
            {
                lblProgressText.Text = "Download cancelled.";
                ResetDownloadState();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Download failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ResetDownloadState();
            }
            finally
            {
                if (_updateService != null)
                {
                    _updateService.DownloadProgressChanged -= UpdateService_DownloadProgressChanged;
                    _updateService.DownloadCompleted -= UpdateService_DownloadCompleted;
                }
            }
        }

        private void UpdateService_DownloadProgressChanged(object? sender, DownloadProgressEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(() => UpdateDownloadProgress(e));
            }
            else
            {
                UpdateDownloadProgress(e);
            }
        }

        private void UpdateDownloadProgress(DownloadProgressEventArgs e)
        {
            progressBar.Value = Math.Min(e.ProgressPercentage, 100);

            string sizeText = FormatBytes(e.BytesReceived);
            string totalText = e.TotalBytesToReceive > 0 ? $" of {FormatBytes(e.TotalBytesToReceive)}" : "";

            lblProgressText.Text = $"Downloading... {e.ProgressPercentage}% ({sizeText}{totalText})";
        }

        private void UpdateService_DownloadCompleted(object? sender, DownloadCompletedEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(() => HandleDownloadCompleted(e));
            }
            else
            {
                HandleDownloadCompleted(e);
            }
        }

        private void HandleDownloadCompleted(DownloadCompletedEventArgs e)
        {
            _isDownloading = false;

            if (e.Success && !string.IsNullOrEmpty(e.FilePath))
            {
                progressBar.Value = 100;
                lblProgressText.Text = "Download complete! Installing...";
            }
            else if (e.Error != null && !(e.Error is OperationCanceledException))
            {
                lblProgressText.Text = $"Download failed: {e.Error.Message}";
                ResetDownloadState();
            }
        }

        private void ResetDownloadState()
        {
            _isDownloading = false;
            btnUpdate.Enabled = true;
            btnUpdate.Text = "Download && Install";
            btnCancel.Text = "Not Now";
            pnlProgress.Visible = false;
        }

        private void InstallUpdate(string installerPath)
        {
            try
            {
                lblProgressText.Text = "Launching installer...";

                // Determine how to run the installer based on file extension
                string extension = Path.GetExtension(installerPath).ToLowerInvariant();

                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = installerPath,
                    UseShellExecute = true,
                    Verb = "runas" // Request admin privileges
                };

                if (extension == ".msi")
                {
                    startInfo.FileName = "msiexec";
                    startInfo.Arguments = $"/i \"{installerPath}\"";
                }

                Process.Start(startInfo);

                // Close the application to allow the installer to run
                Application.Exit();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to launch installer: {ex.Message}\n\nThe update file has been downloaded to:\n{installerPath}",
                    "Installation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ResetDownloadState();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (_isDownloading && _cancellationTokenSource != null)
            {
                var result = MessageBox.Show("Are you sure you want to cancel the download?",
                    "Cancel Download", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    _cancellationTokenSource.Cancel();
                }
            }
            else
            {
                this.Close();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (_isDownloading)
            {
                var result = MessageBox.Show("A download is in progress. Are you sure you want to cancel?",
                    "Cancel Download", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    _cancellationTokenSource?.Cancel();
                    this.Close();
                }
            }
            else
            {
                this.Close();
            }
        }

        private void pnlHeader_MouseDown(object sender, MouseEventArgs e)
        {
            _dragging = true;
            _dragCursorPoint = Cursor.Position;
            _dragFormPoint = this.Location;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (_dragging)
            {
                Point diff = Point.Subtract(Cursor.Position, new Size(_dragCursorPoint));
                this.Location = Point.Add(_dragFormPoint, new Size(diff));
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            _dragging = false;
        }

        private void pnlVersionInfo_Paint(object sender, PaintEventArgs e)
        {
            // Draw rounded border for version info panel
            DrawRoundedBorder(e.Graphics, pnlVersionInfo, Color.FromArgb(230, 230, 235));
        }

        private void pnlProgress_Paint(object sender, PaintEventArgs e)
        {
            // Draw rounded border for progress panel
            DrawRoundedBorder(e.Graphics, pnlProgress, Color.FromArgb(230, 230, 235));
        }

        private void DrawRoundedBorder(Graphics g, Panel panel, Color borderColor)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            using (var pen = new Pen(borderColor, 1))
            {
                var rect = new Rectangle(0, 0, panel.Width - 1, panel.Height - 1);
                int radius = 8;

                using (var path = CreateRoundedRectanglePath(rect, radius))
                {
                    g.DrawPath(pen, path);
                }
            }
        }

        private GraphicsPath CreateRoundedRectanglePath(Rectangle rect, int radius)
        {
            var path = new GraphicsPath();
            path.AddArc(rect.X, rect.Y, radius * 2, radius * 2, 180, 90);
            path.AddArc(rect.Right - radius * 2, rect.Y, radius * 2, radius * 2, 270, 90);
            path.AddArc(rect.Right - radius * 2, rect.Bottom - radius * 2, radius * 2, radius * 2, 0, 90);
            path.AddArc(rect.X, rect.Bottom - radius * 2, radius * 2, radius * 2, 90, 90);
            path.CloseFigure();
            return path;
        }

        private string FormatBytes(long bytes)
        {
            string[] suffixes = { "B", "KB", "MB", "GB" };
            int suffixIndex = 0;
            double value = bytes;

            while (value >= 1024 && suffixIndex < suffixes.Length - 1)
            {
                value /= 1024;
                suffixIndex++;
            }

            return $"{value:0.##} {suffixes[suffixIndex]}";
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            _cancellationTokenSource?.Dispose();
        }

        private void pnlContent_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
