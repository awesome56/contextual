using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Markup;
using GlobalVariable;

namespace Contextual
{
    public partial class frmLogin : Form
    {
        bool exists = false;
        static string connectionString = string.Format(@"Server=(LocalDB)\MSSQLLocalDB;AttachDbFilename={0};Integrated Security=True;Connect Timeout=30", GlobalVariable.Globals.databasePath);
        //static string connectionString = string.Format(@"Data Source={0}\(LocalDB)\MSSQLLocalDB;Initial Catalog=concdb.mdf;Integrated Security=True;Connect Timeout=30", Environment.MachineName);

        public frmLogin()
        {
            InitializeComponent();

            //string machineName = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            //connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" +GlobalVariable.Globals.databasePath + ";Integrated Security=True;Connect Timeout=30";

            //connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\conceptdb.mdf;Integrated Security=True;Connect Timeout=30";

            //connectionString = string.Format(@"Data Source={0}\(LocalDB)\MSSQLLocalDB;Initial Catalog=concdb.mdf;Integrated Security=True;Connect Timeout=30", Environment.MachineName);
            
            try
            {
                SqlHelper helper = new SqlHelper(connectionString);
                if (helper.IsConnection)
                {
                    AppSetting setting = new AppSetting();
                    //setting.SaveConnectionString("Con", connectionString);
                    //MessageBox.Show("Test connection succeeded", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                    //MessageBox.Show("Test connection succeeded", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        SqlConnection Con = new SqlConnection(connectionString);
        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            txtPassword.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            

            if(txtPassword.Text == "")
            {
                MessageBox.Show("Password cannot be empty");
            }
            else
            {
                Con.Open();

                    bool pass_exist = false;
                    string chech_pass = "SELECT count(*) FROM [UserTbl] where Pass='" + txtPassword.Text + "'";
                    SqlCommand cmdPass = new SqlCommand(chech_pass, Con);
                    pass_exist = (int)cmdPass.ExecuteScalar() > 0;

                    if (pass_exist)
                    {
                        //txtUsername.Text = "";
                        txtPassword.Text = "";

                        frmDashboard dashboard = new frmDashboard();
                        dashboard.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Invalid password");
                    }
                Con.Close();
                
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {
            RgstFrm register = new RgstFrm();
            register.Show();
            this.Hide();
        }

        private void label7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label8_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void checkBxShowPass_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBxShowPass.Checked)
            {
                txtPassword.PasswordChar = '\0';
            }
            else
            {
                txtPassword.PasswordChar = '•';
            }
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            //MessageBox.Show(GlobalVariable.Globals.databasePath);
            Con.Open();

            //Add Examiner to program table
            //string checkTableProg = "IF NOT EXISTS (SELECT * FROM Sys.columns WHERE Object_ID = Object_ID(N'[db].[Program]') AND Name = 'Examiner') BEGIN ALTER TABLE Program ADD Examiner varchar(MAX) END"; 
            string checkTableProg = "IF NOT EXISTS (SELECT * FROM Sys.columns WHERE Object_ID = Object_ID(N'Program') AND Name = 'Examiner') BEGIN ALTER TABLE Program ADD Examiner varchar(MAX) END";

            SqlCommand command = new SqlCommand(checkTableProg, Con);
            var v = command.ExecuteNonQuery();

            try 
            {
                

                string chech = "SELECT count(*) FROM [UserTbl]";
                SqlCommand cmd = new SqlCommand(chech, Con);
                exists = (int)cmd.ExecuteScalar() > 0;

                

                Con.Close();
            }
            catch (Exception ex)
            {
                Con.Close();
                MessageBox.Show(ex.Message);
            }
        }

        private void frmLogin_Shown(object sender, EventArgs e)
        {
            if (exists)
            {
            }
            else
            {
                RgstFrm registerForm = new RgstFrm();
                registerForm.Show();
                this.Hide();
            }
        }
    }
}
