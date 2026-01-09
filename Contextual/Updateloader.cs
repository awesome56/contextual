using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Data.SqlClient;

namespace Contextual
{
    public partial class Updateloader : Form
    {
        //private CancellationTokenSource _cts;
        private Button cancelButton;
        private CancellationTokenSource cancellationTokenSource;

        public Updateloader()
        {
            InitializeComponent();
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {
        }

        private void Updateloader_Load(object sender, EventArgs e)
        {
            plsWait.Show();
            lbheader.Hide();
            label1.Show();
            label2.Hide();
            lbnew.Hide();
            btnUpdate.Hide();
            lbcurr.Text = Application.ProductVersion.ToString();

            bw_updateChecker.RunWorkerAsync();
        }

        private void checkUpadte()
        {
            var urlVersion = "https://asltech.com.ng/download/contextual/version.txt";
            var newVersion = new WebClient().DownloadString(urlVersion).Trim();
            var currentVersion = Application.ProductVersion;

            var newVersionNumber = int.Parse(newVersion.Replace(".", ""));
            var currentVersionNumber = int.Parse(currentVersion.Replace(".", ""));

            this.Invoke(new Action(() =>
            {
                if (newVersionNumber > currentVersionNumber)
                {
                    plsWait.Hide();

                    lbheader.Text = "A New Version is Available.\rDo you want to Update ?\r\n";
                    lbheader.Show();
                    label2.Show();
                    lbnew.Text = $"{newVersion}";
                    lbnew.Show();
                    btnUpdate.Enabled = true;
                    btnUpdate.Show();

                }
                else
                {
                    plsWait.Hide();

                    lbheader.Text = "The application is up-to-date.";
                    lbheader.Show();
                    btnUpdate.Enabled = false;
                    lbnew.Hide();
                    label2.Hide();
                }
            }));


        }

        private void initScript()
        {
            string path = Application.StartupPath + @"\bat.bat";

            Process p = new Process();
            p.StartInfo.FileName = path;
            p.StartInfo.Arguments = "";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.Verb = "runas";
            p.Start();
            Environment.Exit(1);
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            checkUpadte();
        }

        private void bw_updateChecker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            bw_updateChecker.RunWorkerAsync();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            btnUpdate.Enabled = false;

            btnUpdate.Text = "Downloading...";

            var web = new WebClient();

            string downloadFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Documents", "contextual");

            // Check if the folder exists, and create it if it does not
            if (!Directory.Exists(downloadFolderPath))
            {
                Directory.CreateDirectory(downloadFolderPath);
            }

            string downloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Documents", "contextual", "contextual.msi");


            web.DownloadFileCompleted += Web_DownloadFileCompleted;
            web.DownloadFileAsync(new Uri("https://asltech.com.ng/download/contextual/contextual.msi"), downloadPath);
        }

        private void Web_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            string logFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Documents", "contextual", "update_log.txt");

            try
            {
                //// Detach the database
                //DetachDatabase("concdb");

                //// Short delay to ensure the database detachment completes
                //System.Threading.Thread.Sleep(2000); // 2-second delay

                //string appFolderPath = Application.StartupPath;
                //string sourceFile1 = Path.Combine(appFolderPath, "concdb.mdf");
                //string sourceFile2 = Path.Combine(appFolderPath, "concdb_log.ldf");

                //string downloadFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Documents", "contextual");

                //if (!Directory.Exists(downloadFolderPath))
                //{
                //    Directory.CreateDirectory(downloadFolderPath);
                //}

                //// Copy files with retry
                //bool copyMdfSuccess = CopyFileWithRetry(sourceFile1, Path.Combine(downloadFolderPath, "concdb.mdf"));
                //bool copyLdfSuccess = CopyFileWithRetry(sourceFile2, Path.Combine(downloadFolderPath, "concdb_log.ldf"));

                //if (!copyMdfSuccess || !copyLdfSuccess)
                //{
                //    File.AppendAllText(logFilePath, DateTime.Now + " - Error: Failed to copy database files after retries." + Environment.NewLine);
                //    this.Close();
                //    MessageBox.Show("An error occured while updating application");
                //    return;
                //}

                initScript();
            }
            catch (Exception ex)
            {
                File.AppendAllText(logFilePath, DateTime.Now + " - Error: " + ex.Message + Environment.NewLine);
            }
        }



        public void DetachDatabase(string databaseName)
        {
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;Integrated Security=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = $@"
                        ALTER DATABASE [{databaseName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                        ALTER DATABASE [{databaseName}] SET OFFLINE WITH ROLLBACK IMMEDIATE;
                    ";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error detaching database: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        bool CopyFileWithRetry(string source, string destination, int maxRetries = 5, int delayMilliseconds = 500)
        {
            string logFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Documents", "contextual", "update_log.txt");

            for (int i = 0; i < maxRetries; i++)
            {
                try
                {
                    File.Copy(source, destination, true);
                    return true; // Success
                }
                catch (IOException ex) when (i < maxRetries - 1)
                {
                    // Wait before retrying
                    System.Threading.Thread.Sleep(delayMilliseconds);
                }
                catch (Exception ex)
                {
                    File.AppendAllText(logFilePath, DateTime.Now + " - Error copying file: " + ex.Message + Environment.NewLine);
                    return false; // Log error and exit on failure
                }
            }
            return false;
        }


        private void label7_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
