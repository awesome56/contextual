using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using System.Runtime.InteropServices;

namespace Contextual
{
    public partial class frmDashboard : Form
    {

        //[DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]

       /*private static extern IntPtr CreateRoundRectRgn
        (
        int nLeftRect,
        int TopRect,
        int nRightRect,
        int nBottomRect,
        int nWidthEllipse,
        int nHeightEllipse
        );*/
        public frmDashboard()
        {
            InitializeComponent();
            //Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 25, 25));
            pnlNav.Height = btnDashboard.Height;
            pnlNav.Top = btnDashboard.Top;
            pnlNav.Left = btnDashboard.Left;
            //btnDashboard.BackColor = Color.FromArgb(46, 51, 73);

            this.pnlLoader.Controls.Clear();

            frmHome FrmHome_page = new frmHome() { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
            FrmHome_page.FormBorderStyle = FormBorderStyle.None;
            this.pnlLoader.Controls.Add(FrmHome_page);
            FrmHome_page.Show();
        }

        static string instituteName;
        static string facultyName;
        static string departmentName;
        static string pass;
        static string headers;

        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + GlobalVariable.Globals.databasePath + ";Integrated Security=True;MultipleActiveResultSets=True;Connect Timeout=30");


        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void frmDashboard_Load(object sender, EventArgs e)
        {
            Con.Open();
            string loadProg = "SELECT * FROM [UserTbl]";
            SqlCommand cmdProg = new SqlCommand(loadProg, Con);
            SqlDataReader reader = cmdProg.ExecuteReader();

            if (reader.Read())
            {
                instituteName = reader.GetString("School");
                facultyName = reader.GetString("Faculty");
                departmentName = reader.GetString("Department");
                pass = reader.GetString("Pass");
                headers = reader.GetString("Header");

                label1.Text = facultyName;
                label2.Text = departmentName;
            }
            Con.Close();
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            pnlNav.Height = btnDashboard.Height;
            pnlNav.Top = btnDashboard.Top;
            pnlNav.Left = btnDashboard.Left;
            //btnDashboard.BackColor = Color.FromArgb(46, 51, 73);

            this.pnlLoader.Controls.Clear();

            frmHome FrmHome_page = new frmHome() { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
            FrmHome_page.FormBorderStyle= FormBorderStyle.None;
            this.pnlLoader.Controls.Add(FrmHome_page);
            FrmHome_page.Show();
        }

        private void btnStudents_Click(object sender, EventArgs e)
        {
            pnlNav.Height = btnStudents.Height;
            pnlNav.Top = btnStudents.Top;
            pnlNav.Left = btnStudents.Left;
            //btnStudents.BackColor = Color.FromArgb(46, 51, 73);

            this.pnlLoader.Controls.Clear();

            frmStudent FrmStudent_page = new frmStudent() { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
            FrmStudent_page.FormBorderStyle = FormBorderStyle.None;
            this.pnlLoader.Controls.Add(FrmStudent_page);
            FrmStudent_page.Show();
        }

        private void btnLecturers_Click(object sender, EventArgs e)
        {
            pnlNav.Height = btnLecturers.Height;
            pnlNav.Top = btnLecturers.Top;
            pnlNav.Left = btnLecturers.Left;
            //btnLecturers.BackColor = Color.FromArgb(46, 51, 73);

            this.pnlLoader.Controls.Clear();

            frmLecturers FrmLecturer_page = new frmLecturers() { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
            FrmLecturer_page.FormBorderStyle = FormBorderStyle.None;
            this.pnlLoader.Controls.Add(FrmLecturer_page);
            FrmLecturer_page.Show();
        }

        private void btnPrograms_Click(object sender, EventArgs e)
        {
            pnlNav.Height = btnPrograms.Height;
            pnlNav.Top = btnPrograms.Top;
            pnlNav.Left = btnPrograms.Left;
            //btnPrograms.BackColor = Color.FromArgb(46, 51, 73);

            this.pnlLoader.Controls.Clear();

            frmPrograms FrmProgram_page = new frmPrograms() { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
            FrmProgram_page.FormBorderStyle = FormBorderStyle.None;
            this.pnlLoader.Controls.Add(FrmProgram_page);
            FrmProgram_page.Show();
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            pnlNav.Height = btnSettings.Height;
            pnlNav.Top = btnSettings.Top;
            pnlNav.Left = btnSettings.Left;
            //btnSettings.BackColor = Color.FromArgb(46, 51, 73);

            this.pnlLoader.Controls.Clear();

            frmSettings FrmSettings_page = new frmSettings() { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
            FrmSettings_page.FormBorderStyle = FormBorderStyle.None;
            this.pnlLoader.Controls.Add(FrmSettings_page);
            FrmSettings_page.Show();
        }

        private void btnDashboard_Leave(object sender, EventArgs e)
        {
            btnDashboard.BackColor = Color.FromArgb(116, 86, 174);
        }

        private void btnStudents_Leave(object sender, EventArgs e)
        {
            btnStudents.BackColor = Color.FromArgb(116, 86, 174);
        }

        private void btnLecturers_Leave_1(object sender, EventArgs e)
        {
            btnLecturers.BackColor = Color.FromArgb(116, 86, 174);
        }

        private void btnPrograms_Leave(object sender, EventArgs e)
        {
            btnPrograms.BackColor = Color.FromArgb(116, 86, 174);
        }

        private void btnSettings_Leave(object sender, EventArgs e)
        {
            btnSettings.BackColor = Color.FromArgb(116, 86, 174);
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void iconPictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void iconPictureBox4_Click(object sender, EventArgs e)
        {

        }

        private void iconPictureBox1_Click_1(object sender, EventArgs e)
        {

        }

        private void panelDesktop_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
