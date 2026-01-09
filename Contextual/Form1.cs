using System.Windows.Forms;
using System.Data.SqlClient;
using Microsoft.VisualBasic.ApplicationServices;
using GlobalVariable;

namespace Contextual
{
    public partial class RgstFrm : Form
    {
        public RgstFrm()
        {
            InitializeComponent();
        }

        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + GlobalVariable.Globals.databasePath + ";Integrated Security=True;Connect Timeout=30");

        //SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\conceptdb.mdf;Integrated Security=True;Connect Timeout=30");

        private void label6_Click(object sender, EventArgs e)
        {
            frmLogin login = new frmLogin();
            login.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            txtFaculty.Text = "";
            txtDpt.Text = "";
            txtPassword.Text = "";
            txtComPassword.Text = "";
        }

        private void label7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RgstFrm_Load(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {
            //RgstFrm register = new RgstFrm();
            this.WindowState= FormWindowState.Minimized;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if(txtComPassword.Text != txtPassword.Text)
                {
                    MessageBox.Show("Password and confirm password are not the same");
                }
                else if((txtFaculty.Text == "") || (txtDpt.Text == "") || (txtPassword.Text =="") || (txtComPassword.Text == ""))
                {
                    MessageBox.Show("All fields are required");
                }
                else
                {
                    Con.Open();
                    //cmdCheck = new SqlCommand("select top 1 * from UserTbl where StaffId = @username", Con);
                    //cmdCheck.Parameters.AddWithValue("@username", txtUsername.Text);
                    //bool userExists = ProcessCmdKey().ExecuteScalar() != null;1\/


                    String query = "insert into UserTbl (School,Faculty,Department,Pass,Header) values('Ladoke Akintola University of Technology','" + txtFaculty.Text + "','" + txtDpt.Text + "','" + txtPassword.Text + "','')";
                    SqlCommand cmd = new SqlCommand(query, Con);
                    cmd.ExecuteNonQuery();


                    MessageBox.Show("Details Entry successfull");

                    txtFaculty.Text = "";
                    txtDpt.Text = "";
                    txtPassword.Text = "";
                    txtComPassword.Text = "";

                    frmLogin login = new frmLogin();
                    login.Show();
                    this.Hide();

                    Con.Close();
                }
                
            }
            catch(Exception ex)
            {
                Con.Close();
                MessageBox.Show(ex.Message);
            }
        }

        private void checkBxShowPass_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBxShowPass.Checked) 
            {
                txtComPassword.PasswordChar = '\0';
                txtPassword.PasswordChar= '\0';
            }
            else
            {
                txtComPassword.PasswordChar = '•';
                txtPassword.PasswordChar = '•';
            }
        }
    }
}