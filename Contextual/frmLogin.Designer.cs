namespace Contextual
{
    partial class frmLogin : Form
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
            pnlLoginCard = new Panel();
            tblLayout = new TableLayoutPanel();
            lblWelcome = new Label();
            lblSubtitle = new Label();
            pictureBox1 = new PictureBox();
            label1 = new Label();
            lblPasswordLabel = new Label();
            pnlPasswordContainer = new Panel();
            lblPasswordIcon = new Label();
            txtPassword = new TextBox();
            checkBxShowPass = new CheckBox();
            button1 = new Button();
            button2 = new Button();
            pnlTitleBar = new Panel();
            label8 = new Label();
            label7 = new Label();
            lblFooter = new Label();
            pnlMain.SuspendLayout();
            pnlLoginCard.SuspendLayout();
            tblLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            pnlPasswordContainer.SuspendLayout();
            pnlTitleBar.SuspendLayout();
            SuspendLayout();
            // 
            // pnlMain
            // 
            pnlMain.BackColor = Color.FromArgb(79, 70, 229);
            pnlMain.Controls.Add(pnlLoginCard);
            pnlMain.Controls.Add(pnlTitleBar);
            pnlMain.Controls.Add(lblFooter);
            pnlMain.Dock = DockStyle.Fill;
            pnlMain.Location = new Point(0, 0);
            pnlMain.Name = "pnlMain";
            pnlMain.Size = new Size(811, 700);
            pnlMain.TabIndex = 0;
            // 
            // pnlLoginCard
            // 
            pnlLoginCard.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlLoginCard.BackColor = Color.White;
            pnlLoginCard.Controls.Add(tblLayout);
            pnlLoginCard.Location = new Point(30, 55);
            pnlLoginCard.Name = "pnlLoginCard";
            pnlLoginCard.Padding = new Padding(32);
            pnlLoginCard.Size = new Size(751, 590);
            pnlLoginCard.TabIndex = 0;
            pnlLoginCard.Paint += pnlLoginCard_Paint;
            // 
            // tblLayout
            // 
            tblLayout.ColumnCount = 1;
            tblLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblLayout.Controls.Add(lblWelcome, 0, 0);
            tblLayout.Controls.Add(lblSubtitle, 0, 1);
            tblLayout.Controls.Add(pictureBox1, 0, 2);
            tblLayout.Controls.Add(label1, 0, 3);
            tblLayout.Controls.Add(lblPasswordLabel, 0, 4);
            tblLayout.Controls.Add(pnlPasswordContainer, 0, 5);
            tblLayout.Controls.Add(checkBxShowPass, 0, 6);
            tblLayout.Controls.Add(button1, 0, 7);
            tblLayout.Controls.Add(button2, 0, 8);
            tblLayout.Dock = DockStyle.Fill;
            tblLayout.Location = new Point(32, 32);
            tblLayout.Name = "tblLayout";
            tblLayout.RowCount = 9;
            tblLayout.RowStyles.Add(new RowStyle());
            tblLayout.RowStyles.Add(new RowStyle());
            tblLayout.RowStyles.Add(new RowStyle());
            tblLayout.RowStyles.Add(new RowStyle());
            tblLayout.RowStyles.Add(new RowStyle());
            tblLayout.RowStyles.Add(new RowStyle());
            tblLayout.RowStyles.Add(new RowStyle());
            tblLayout.RowStyles.Add(new RowStyle());
            tblLayout.RowStyles.Add(new RowStyle());
            tblLayout.Size = new Size(687, 526);
            tblLayout.TabIndex = 0;
            // 
            // lblWelcome
            // 
            lblWelcome.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            lblWelcome.AutoSize = true;
            lblWelcome.Font = new Font("Segoe UI Semibold", 20F, FontStyle.Bold, GraphicsUnit.Point);
            lblWelcome.ForeColor = Color.FromArgb(15, 23, 42);
            lblWelcome.Location = new Point(3, 0);
            lblWelcome.Name = "lblWelcome";
            lblWelcome.Size = new Size(681, 72);
            lblWelcome.TabIndex = 0;
            lblWelcome.Text = "Welcome Back";
            lblWelcome.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblSubtitle
            // 
            lblSubtitle.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            lblSubtitle.AutoSize = true;
            lblSubtitle.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);
            lblSubtitle.ForeColor = Color.FromArgb(100, 116, 139);
            lblSubtitle.Location = new Point(3, 72);
            lblSubtitle.Name = "lblSubtitle";
            lblSubtitle.Size = new Size(681, 36);
            lblSubtitle.TabIndex = 1;
            lblSubtitle.Text = "Please enter your password to continue";
            lblSubtitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pictureBox1
            // 
            pictureBox1.Anchor = AnchorStyles.None;
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.Image = Properties.Resources.LAUTECH_Logo_1;
            pictureBox1.Location = new Point(293, 111);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(100, 100);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 2;
            pictureBox1.TabStop = false;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 13F, FontStyle.Bold, GraphicsUnit.Point);
            label1.ForeColor = Color.FromArgb(79, 70, 229);
            label1.Location = new Point(3, 214);
            label1.Name = "label1";
            label1.Size = new Size(681, 47);
            label1.TabIndex = 3;
            label1.Text = "CONTEXTUAL";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblPasswordLabel
            // 
            lblPasswordLabel.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            lblPasswordLabel.AutoSize = true;
            lblPasswordLabel.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold, GraphicsUnit.Point);
            lblPasswordLabel.ForeColor = Color.FromArgb(51, 65, 85);
            lblPasswordLabel.Location = new Point(3, 261);
            lblPasswordLabel.Name = "lblPasswordLabel";
            lblPasswordLabel.Size = new Size(681, 36);
            lblPasswordLabel.TabIndex = 8;
            lblPasswordLabel.Text = "Password";
            lblPasswordLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // pnlPasswordContainer
            // 
            pnlPasswordContainer.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            pnlPasswordContainer.BackColor = Color.FromArgb(248, 250, 252);
            pnlPasswordContainer.Controls.Add(lblPasswordIcon);
            pnlPasswordContainer.Controls.Add(txtPassword);
            pnlPasswordContainer.Location = new Point(3, 300);
            pnlPasswordContainer.Name = "pnlPasswordContainer";
            pnlPasswordContainer.Padding = new Padding(12, 0, 12, 0);
            pnlPasswordContainer.Size = new Size(681, 48);
            pnlPasswordContainer.TabIndex = 4;
            // 
            // lblPasswordIcon
            // 
            lblPasswordIcon.Anchor = AnchorStyles.Left;
            lblPasswordIcon.Font = new Font("Segoe UI", 5F, FontStyle.Regular, GraphicsUnit.Point);
            lblPasswordIcon.ForeColor = Color.FromArgb(148, 163, 184);
            lblPasswordIcon.Location = new Point(12, 10);
            lblPasswordIcon.Margin = new Padding(1);
            lblPasswordIcon.Name = "lblPasswordIcon";
            lblPasswordIcon.Size = new Size(28, 28);
            lblPasswordIcon.TabIndex = 0;
            lblPasswordIcon.Text = "🔒";
            lblPasswordIcon.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // txtPassword
            // 
            txtPassword.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtPassword.BackColor = Color.FromArgb(248, 250, 252);
            txtPassword.BorderStyle = BorderStyle.None;
            txtPassword.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            txtPassword.ForeColor = Color.FromArgb(30, 41, 59);
            txtPassword.Location = new Point(44, 5);
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '●';
            txtPassword.Size = new Size(623, 40);
            txtPassword.TabIndex = 1;
            txtPassword.TextChanged += txtPassword_TextChanged;
            txtPassword.KeyUp += txtPassword_KeyUp;
            // 
            // checkBxShowPass
            // 
            checkBxShowPass.Anchor = AnchorStyles.Left;
            checkBxShowPass.Cursor = Cursors.Hand;
            checkBxShowPass.FlatStyle = FlatStyle.Flat;
            checkBxShowPass.Font = new Font("Segoe UI", 7F, FontStyle.Regular, GraphicsUnit.Point);
            checkBxShowPass.ForeColor = Color.FromArgb(100, 116, 139);
            checkBxShowPass.Location = new Point(1, 352);
            checkBxShowPass.Margin = new Padding(1);
            checkBxShowPass.Name = "checkBxShowPass";
            checkBxShowPass.Size = new Size(140, 32);
            checkBxShowPass.TabIndex = 5;
            checkBxShowPass.Text = "Show Password";
            checkBxShowPass.UseVisualStyleBackColor = true;
            checkBxShowPass.CheckedChanged += checkBxShowPass_CheckedChanged;
            // 
            // button1
            // 
            button1.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            button1.BackColor = Color.FromArgb(79, 70, 229);
            button1.Cursor = Cursors.Hand;
            button1.FlatAppearance.BorderSize = 0;
            button1.FlatAppearance.MouseDownBackColor = Color.FromArgb(67, 56, 202);
            button1.FlatAppearance.MouseOverBackColor = Color.FromArgb(99, 102, 241);
            button1.FlatStyle = FlatStyle.Flat;
            button1.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold, GraphicsUnit.Point);
            button1.ForeColor = Color.White;
            button1.Location = new Point(3, 388);
            button1.Name = "button1";
            button1.Size = new Size(681, 48);
            button1.TabIndex = 6;
            button1.Text = "Sign In";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            button2.BackColor = Color.Transparent;
            button2.Cursor = Cursors.Hand;
            button2.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            button2.FlatAppearance.MouseDownBackColor = Color.FromArgb(241, 245, 249);
            button2.FlatAppearance.MouseOverBackColor = Color.FromArgb(248, 250, 252);
            button2.FlatStyle = FlatStyle.Flat;
            button2.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            button2.ForeColor = Color.FromArgb(71, 85, 105);
            button2.Location = new Point(3, 461);
            button2.Name = "button2";
            button2.Size = new Size(681, 42);
            button2.TabIndex = 7;
            button2.Text = "Clear";
            button2.UseVisualStyleBackColor = false;
            button2.Click += button2_Click;
            // 
            // pnlTitleBar
            // 
            pnlTitleBar.BackColor = Color.Transparent;
            pnlTitleBar.Controls.Add(label8);
            pnlTitleBar.Controls.Add(label7);
            pnlTitleBar.Dock = DockStyle.Top;
            pnlTitleBar.Location = new Point(0, 0);
            pnlTitleBar.Name = "pnlTitleBar";
            pnlTitleBar.Size = new Size(811, 36);
            pnlTitleBar.TabIndex = 1;
            // 
            // label8
            // 
            label8.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label8.Cursor = Cursors.Hand;
            label8.Font = new Font("Segoe UI", 8F, FontStyle.Bold, GraphicsUnit.Point);
            label8.ForeColor = Color.FromArgb(224, 231, 255);
            label8.Location = new Point(729, 3);
            label8.Name = "label8";
            label8.Size = new Size(32, 28);
            label8.TabIndex = 0;
            label8.Text = "−";
            label8.TextAlign = ContentAlignment.MiddleCenter;
            label8.Click += label8_Click;
            // 
            // label7
            // 
            label7.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label7.Cursor = Cursors.Hand;
            label7.Font = new Font("Segoe UI", 8F, FontStyle.Bold, GraphicsUnit.Point);
            label7.ForeColor = Color.FromArgb(224, 231, 255);
            label7.Location = new Point(767, 3);
            label7.Name = "label7";
            label7.Size = new Size(32, 28);
            label7.TabIndex = 1;
            label7.Text = "✕";
            label7.TextAlign = ContentAlignment.MiddleCenter;
            label7.Click += label7_Click;
            // 
            // lblFooter
            // 
            lblFooter.Dock = DockStyle.Bottom;
            lblFooter.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point);
            lblFooter.ForeColor = Color.FromArgb(199, 210, 254);
            lblFooter.Location = new Point(0, 672);
            lblFooter.Name = "lblFooter";
            lblFooter.Size = new Size(811, 28);
            lblFooter.TabIndex = 2;
            lblFooter.Text = "© 2025 Contextual. All rights reserved.";
            lblFooter.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // frmLogin
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(79, 70, 229);
            ClientSize = new Size(811, 700);
            ControlBox = false;
            Controls.Add(pnlMain);
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            ForeColor = Color.FromArgb(30, 41, 59);
            MaximizeBox = false;
            MinimizeBox = false;
            MinimumSize = new Size(400, 500);
            Name = "frmLogin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Contextual - Login";
            Load += frmLogin_Load;
            Shown += frmLogin_Shown;
            pnlMain.ResumeLayout(false);
            pnlLoginCard.ResumeLayout(false);
            tblLayout.ResumeLayout(false);
            tblLayout.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            pnlPasswordContainer.ResumeLayout(false);
            pnlPasswordContainer.PerformLayout();
            pnlTitleBar.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlMain;
                private Panel pnlLoginCard;
                private Label lblWelcome;
                private Label lblSubtitle;
                private PictureBox pictureBox1;
                private Label label1;
                private Label lblPasswordLabel;
                private Panel pnlPasswordContainer;
                private Label lblPasswordIcon;
                private TextBox txtPassword;
                private CheckBox checkBxShowPass;
                private Button button1;
                private Button button2;
                private Panel pnlTitleBar;
                private Label label8;
                private Label label7;
                private Label lblFooter;
                private TableLayoutPanel tblLayout;
    }
}