namespace Contextual
{
    partial class Updateloader
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pnlMain = new Panel();
            pnlHeader = new Panel();
            lblTitle = new Label();
            btnClose = new Label();
            pnlContent = new Panel();
            lblStatus = new Label();
            pnlVersionInfo = new Panel();
            lblCurrentVersionLabel = new Label();
            lbcurr = new Label();
            lblNewVersionLabel = new Label();
            lbnew = new Label();
            lblReleaseDate = new Label();
            pnlProgress = new Panel();
            progressBar = new ProgressBar();
            lblProgressText = new Label();
            lblReleaseNotes = new Label();
            txtReleaseNotes = new TextBox();
            pnlButtons = new Panel();
            btnUpdate = new Button();
            btnCancel = new Button();
            pnlMain.SuspendLayout();
            pnlHeader.SuspendLayout();
            pnlContent.SuspendLayout();
            pnlVersionInfo.SuspendLayout();
            pnlProgress.SuspendLayout();
            pnlButtons.SuspendLayout();
            SuspendLayout();
            // 
            // pnlMain
            // 
            pnlMain.BackColor = Color.White;
            pnlMain.Controls.Add(pnlHeader);
            pnlMain.Controls.Add(pnlContent);
            pnlMain.Dock = DockStyle.Fill;
            pnlMain.Location = new Point(0, 0);
            pnlMain.Margin = new Padding(4, 5, 4, 5);
            pnlMain.Name = "pnlMain";
            pnlMain.Size = new Size(686, 700);
            pnlMain.TabIndex = 0;
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(99, 71, 153);
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(btnClose);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Margin = new Padding(4, 5, 4, 5);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(686, 100);
            pnlHeader.TabIndex = 0;
            pnlHeader.MouseDown += pnlHeader_MouseDown;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold, GraphicsUnit.Point);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(29, 28);
            lblTitle.Margin = new Padding(4, 0, 4, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(233, 38);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Software Update";
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClose.AutoSize = true;
            btnClose.Cursor = Cursors.Hand;
            btnClose.Font = new Font("Segoe UI", 16F, FontStyle.Regular, GraphicsUnit.Point);
            btnClose.ForeColor = Color.White;
            btnClose.Location = new Point(636, 20);
            btnClose.Margin = new Padding(4, 0, 4, 0);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(46, 45);
            btnClose.TabIndex = 1;
            btnClose.Text = "✕";
            btnClose.Click += btnClose_Click;
            // 
            // pnlContent
            // 
            pnlContent.BackColor = Color.FromArgb(250, 250, 252);
            pnlContent.Controls.Add(lblStatus);
            pnlContent.Controls.Add(pnlVersionInfo);
            pnlContent.Controls.Add(pnlProgress);
            pnlContent.Controls.Add(lblReleaseNotes);
            pnlContent.Controls.Add(txtReleaseNotes);
            pnlContent.Controls.Add(pnlButtons);
            pnlContent.Dock = DockStyle.Fill;
            pnlContent.Location = new Point(0, 0);
            pnlContent.Margin = new Padding(4, 5, 4, 5);
            pnlContent.Name = "pnlContent";
            pnlContent.Padding = new Padding(29, 33, 29, 33);
            pnlContent.Size = new Size(686, 700);
            pnlContent.TabIndex = 1;
            pnlContent.Paint += pnlContent_Paint;
            // 
            // lblStatus
            // 
            lblStatus.Dock = DockStyle.Top;
            lblStatus.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            lblStatus.ForeColor = Color.FromArgb(64, 64, 64);
            lblStatus.Location = new Point(29, 33);
            lblStatus.Margin = new Padding(4, 0, 4, 0);
            lblStatus.Name = "lblStatus";
            lblStatus.Padding = new Padding(0, 0, 0, 17);
            lblStatus.Size = new Size(628, 67);
            lblStatus.TabIndex = 0;
            lblStatus.Text = "Checking for updates...";
            lblStatus.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // pnlVersionInfo
            // 
            pnlVersionInfo.BackColor = Color.White;
            pnlVersionInfo.Controls.Add(lblCurrentVersionLabel);
            pnlVersionInfo.Controls.Add(lbcurr);
            pnlVersionInfo.Controls.Add(lblNewVersionLabel);
            pnlVersionInfo.Controls.Add(lbnew);
            pnlVersionInfo.Controls.Add(lblReleaseDate);
            pnlVersionInfo.Location = new Point(29, 108);
            pnlVersionInfo.Margin = new Padding(4, 5, 4, 5);
            pnlVersionInfo.Name = "pnlVersionInfo";
            pnlVersionInfo.Padding = new Padding(21, 25, 21, 25);
            pnlVersionInfo.Size = new Size(629, 150);
            pnlVersionInfo.TabIndex = 1;
            pnlVersionInfo.Visible = false;
            pnlVersionInfo.Paint += pnlVersionInfo_Paint;
            // 
            // lblCurrentVersionLabel
            // 
            lblCurrentVersionLabel.AutoSize = true;
            lblCurrentVersionLabel.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblCurrentVersionLabel.ForeColor = Color.Gray;
            lblCurrentVersionLabel.Location = new Point(21, 25);
            lblCurrentVersionLabel.Margin = new Padding(4, 0, 4, 0);
            lblCurrentVersionLabel.Name = "lblCurrentVersionLabel";
            lblCurrentVersionLabel.Size = new Size(150, 28);
            lblCurrentVersionLabel.TabIndex = 0;
            lblCurrentVersionLabel.Text = "Current Version:";
            // 
            // lbcurr
            // 
            lbcurr.AutoSize = true;
            lbcurr.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point);
            lbcurr.ForeColor = Color.FromArgb(64, 64, 64);
            lbcurr.Location = new Point(186, 25);
            lbcurr.Margin = new Padding(4, 0, 4, 0);
            lbcurr.Name = "lbcurr";
            lbcurr.Size = new Size(68, 28);
            lbcurr.TabIndex = 1;
            lbcurr.Text = "1.0.0.0";
            // 
            // lblNewVersionLabel
            // 
            lblNewVersionLabel.AutoSize = true;
            lblNewVersionLabel.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblNewVersionLabel.ForeColor = Color.Gray;
            lblNewVersionLabel.Location = new Point(21, 67);
            lblNewVersionLabel.Margin = new Padding(4, 0, 4, 0);
            lblNewVersionLabel.Name = "lblNewVersionLabel";
            lblNewVersionLabel.Size = new Size(124, 28);
            lblNewVersionLabel.TabIndex = 2;
            lblNewVersionLabel.Text = "New Version:";
            // 
            // lbnew
            // 
            lbnew.AutoSize = true;
            lbnew.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point);
            lbnew.ForeColor = Color.FromArgb(99, 71, 153);
            lbnew.Location = new Point(186, 67);
            lbnew.Margin = new Padding(4, 0, 4, 0);
            lbnew.Name = "lbnew";
            lbnew.Size = new Size(68, 28);
            lbnew.TabIndex = 3;
            lbnew.Text = "1.0.0.0";
            // 
            // lblReleaseDate
            // 
            lblReleaseDate.AutoSize = true;
            lblReleaseDate.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lblReleaseDate.ForeColor = Color.Gray;
            lblReleaseDate.Location = new Point(21, 108);
            lblReleaseDate.Margin = new Padding(4, 0, 4, 0);
            lblReleaseDate.Name = "lblReleaseDate";
            lblReleaseDate.Size = new Size(0, 25);
            lblReleaseDate.TabIndex = 4;
            // 
            // pnlProgress
            // 
            pnlProgress.BackColor = Color.White;
            pnlProgress.Controls.Add(progressBar);
            pnlProgress.Controls.Add(lblProgressText);
            pnlProgress.Location = new Point(29, 275);
            pnlProgress.Margin = new Padding(4, 5, 4, 5);
            pnlProgress.Name = "pnlProgress";
            pnlProgress.Padding = new Padding(21, 25, 21, 25);
            pnlProgress.Size = new Size(629, 117);
            pnlProgress.TabIndex = 2;
            pnlProgress.Visible = false;
            pnlProgress.Paint += pnlProgress_Paint;
            // 
            // progressBar
            // 
            progressBar.Location = new Point(21, 25);
            progressBar.Margin = new Padding(4, 5, 4, 5);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(586, 42);
            progressBar.Style = ProgressBarStyle.Continuous;
            progressBar.TabIndex = 0;
            // 
            // lblProgressText
            // 
            lblProgressText.AutoSize = true;
            lblProgressText.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lblProgressText.ForeColor = Color.Gray;
            lblProgressText.Location = new Point(21, 75);
            lblProgressText.Margin = new Padding(4, 0, 4, 0);
            lblProgressText.Name = "lblProgressText";
            lblProgressText.Size = new Size(161, 25);
            lblProgressText.TabIndex = 1;
            lblProgressText.Text = "Downloading... 0%";
            // 
            // lblReleaseNotes
            // 
            lblReleaseNotes.AutoSize = true;
            lblReleaseNotes.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point);
            lblReleaseNotes.ForeColor = Color.FromArgb(64, 64, 64);
            lblReleaseNotes.Location = new Point(29, 283);
            lblReleaseNotes.Margin = new Padding(4, 0, 4, 0);
            lblReleaseNotes.Name = "lblReleaseNotes";
            lblReleaseNotes.Size = new Size(145, 28);
            lblReleaseNotes.TabIndex = 4;
            lblReleaseNotes.Text = "Release Notes:";
            lblReleaseNotes.Visible = false;
            // 
            // txtReleaseNotes
            // 
            txtReleaseNotes.BackColor = Color.White;
            txtReleaseNotes.BorderStyle = BorderStyle.FixedSingle;
            txtReleaseNotes.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            txtReleaseNotes.ForeColor = Color.FromArgb(64, 64, 64);
            txtReleaseNotes.Location = new Point(29, 325);
            txtReleaseNotes.Margin = new Padding(4, 5, 4, 5);
            txtReleaseNotes.Multiline = true;
            txtReleaseNotes.Name = "txtReleaseNotes";
            txtReleaseNotes.ReadOnly = true;
            txtReleaseNotes.ScrollBars = ScrollBars.Vertical;
            txtReleaseNotes.Size = new Size(628, 165);
            txtReleaseNotes.TabIndex = 5;
            txtReleaseNotes.Visible = false;
            // 
            // pnlButtons
            // 
            pnlButtons.Controls.Add(btnUpdate);
            pnlButtons.Controls.Add(btnCancel);
            pnlButtons.Dock = DockStyle.Bottom;
            pnlButtons.Location = new Point(29, 575);
            pnlButtons.Margin = new Padding(4, 5, 4, 5);
            pnlButtons.Name = "pnlButtons";
            pnlButtons.Padding = new Padding(0, 17, 0, 0);
            pnlButtons.Size = new Size(628, 92);
            pnlButtons.TabIndex = 3;
            // 
            // btnUpdate
            // 
            btnUpdate.BackColor = Color.FromArgb(99, 71, 153);
            btnUpdate.Cursor = Cursors.Hand;
            btnUpdate.FlatAppearance.BorderSize = 0;
            btnUpdate.FlatStyle = FlatStyle.Flat;
            btnUpdate.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold, GraphicsUnit.Point);
            btnUpdate.ForeColor = Color.White;
            btnUpdate.Location = new Point(0, 17);
            btnUpdate.Margin = new Padding(4, 5, 4, 5);
            btnUpdate.Name = "btnUpdate";
            btnUpdate.Size = new Size(300, 75);
            btnUpdate.TabIndex = 0;
            btnUpdate.Text = "Download && Install";
            btnUpdate.UseVisualStyleBackColor = false;
            btnUpdate.Visible = false;
            btnUpdate.Click += btnUpdate_Click;
            // 
            // btnCancel
            // 
            btnCancel.BackColor = Color.FromArgb(240, 240, 240);
            btnCancel.Cursor = Cursors.Hand;
            btnCancel.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            btnCancel.ForeColor = Color.FromArgb(64, 64, 64);
            btnCancel.Location = new Point(329, 17);
            btnCancel.Margin = new Padding(4, 5, 4, 5);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(300, 75);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += btnCancel_Click;
            // 
            // Updateloader
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(99, 71, 153);
            ClientSize = new Size(686, 700);
            Controls.Add(pnlMain);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(4, 5, 4, 5);
            Name = "Updateloader";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Software Update";
            Load += Updateloader_Load;
            pnlMain.ResumeLayout(false);
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlContent.ResumeLayout(false);
            pnlContent.PerformLayout();
            pnlVersionInfo.ResumeLayout(false);
            pnlVersionInfo.PerformLayout();
            pnlProgress.ResumeLayout(false);
            pnlProgress.PerformLayout();
            pnlButtons.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlMain;
        private Panel pnlHeader;
        private Label lblTitle;
        private Label btnClose;
        private Panel pnlContent;
        private Label lblStatus;
        private Panel pnlVersionInfo;
        private Label lblCurrentVersionLabel;
        private Label lbcurr;
        private Label lblNewVersionLabel;
        private Label lbnew;
        private Label lblReleaseDate;
        private Panel pnlProgress;
        private ProgressBar progressBar;
        private Label lblProgressText;
        private Panel pnlButtons;
        private Button btnUpdate;
        private Button btnCancel;
        private Label lblReleaseNotes;
        private TextBox txtReleaseNotes;
    }
}