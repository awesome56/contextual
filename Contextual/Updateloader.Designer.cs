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
            label1 = new Label();
            plsWait = new Label();
            label7 = new Label();
            btnUpdate = new Button();
            label2 = new Label();
            lbcurr = new Label();
            lbnew = new Label();
            lbheader = new Label();
            bw_updateChecker = new System.ComponentModel.BackgroundWorker();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Constantia", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(40, 131);
            label1.Name = "label1";
            label1.Size = new Size(124, 19);
            label1.TabIndex = 0;
            label1.Text = "Current Version:";
            // 
            // plsWait
            // 
            plsWait.AutoSize = true;
            plsWait.Font = new Font("Microsoft Sans Serif", 18F, FontStyle.Bold, GraphicsUnit.Point);
            plsWait.Location = new Point(22, 9);
            plsWait.Name = "plsWait";
            plsWait.Size = new Size(148, 29);
            plsWait.TabIndex = 3;
            plsWait.Text = "Please wait";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Cursor = Cursors.Hand;
            label7.ForeColor = Color.FromArgb(116, 86, 174);
            label7.Location = new Point(341, 9);
            label7.Name = "label7";
            label7.Size = new Size(14, 15);
            label7.TabIndex = 7;
            label7.Text = "X";
            label7.Click += label7_Click;
            // 
            // btnUpdate
            // 
            btnUpdate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnUpdate.BackColor = Color.Transparent;
            btnUpdate.Cursor = Cursors.Hand;
            btnUpdate.FlatAppearance.BorderSize = 0;
            btnUpdate.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point);
            btnUpdate.ForeColor = Color.DimGray;
            btnUpdate.Location = new Point(40, 207);
            btnUpdate.Margin = new Padding(3, 2, 3, 2);
            btnUpdate.Name = "btnUpdate";
            btnUpdate.Size = new Size(290, 45);
            btnUpdate.TabIndex = 27;
            btnUpdate.Text = "UPDATE";
            btnUpdate.UseVisualStyleBackColor = false;
            btnUpdate.Click += btnUpdate_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Constantia", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label2.Location = new Point(40, 167);
            label2.Name = "label2";
            label2.Size = new Size(172, 19);
            label2.TabIndex = 28;
            label2.Text = "New Version Available:";
            // 
            // lbcurr
            // 
            lbcurr.AutoSize = true;
            lbcurr.Font = new Font("Constantia", 12F, FontStyle.Bold, GraphicsUnit.Point);
            lbcurr.Location = new Point(277, 131);
            lbcurr.Name = "lbcurr";
            lbcurr.Size = new Size(57, 19);
            lbcurr.TabIndex = 29;
            lbcurr.Text = "1.0.0.0";
            // 
            // lbnew
            // 
            lbnew.AutoSize = true;
            lbnew.Font = new Font("Constantia", 12F, FontStyle.Bold, GraphicsUnit.Point);
            lbnew.Location = new Point(277, 167);
            lbnew.Name = "lbnew";
            lbnew.Size = new Size(57, 19);
            lbnew.TabIndex = 30;
            lbnew.Text = "1.0.0.0";
            // 
            // lbheader
            // 
            lbheader.Font = new Font("Constantia", 17.25F, FontStyle.Regular, GraphicsUnit.Point);
            lbheader.Location = new Point(40, 53);
            lbheader.Name = "lbheader";
            lbheader.Size = new Size(290, 65);
            lbheader.TabIndex = 31;
            lbheader.Text = "A New Version is Available. Do you want to Update ?";
            // 
            // bw_updateChecker
            // 
            bw_updateChecker.DoWork += backgroundWorker1_DoWork;
            bw_updateChecker.RunWorkerCompleted += bw_updateChecker_RunWorkerCompleted;
            // 
            // Updateloader
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(367, 272);
            Controls.Add(lbheader);
            Controls.Add(lbnew);
            Controls.Add(lbcurr);
            Controls.Add(label2);
            Controls.Add(btnUpdate);
            Controls.Add(label7);
            Controls.Add(plsWait);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "Updateloader";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Updateloader";
            Load += Updateloader_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label plsWait;
        private Label label7;
        private Button btnUpdate;
        private Label label2;
        private Label lbcurr;
        private Label lbnew;
        private Label lbheader;
        private System.ComponentModel.BackgroundWorker bw_updateChecker;
    }
}