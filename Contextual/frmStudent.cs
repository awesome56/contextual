using Bunifu.UI.WinForms.Helpers.Transitions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GlobalVariable;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Contextual
{
    public partial class frmStudent : Form
    {
        public frmStudent()
        {
            InitializeComponent();
            Fill_Program();
            Fill_Session();
            LoadTable();
        }

        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + GlobalVariable.Globals.databasePath + ";Integrated Security=True;MultipleActiveResultSets=True;Connect Timeout=30");


        void Fill_Program()
        {
            Con.Open();

            string ProgramQuery = "SELECT * FROM [Program]";
            SqlCommand cmdProg = new SqlCommand(ProgramQuery, Con);
            SqlDataReader reader = cmdProg.ExecuteReader();

            var dtP = new DataTable();

            dtP.Load(reader);

            try
            {

                comboBox3.DataSource = null;
                comboBox3.Items.Clear();



                comboBox3.DisplayMember = "Name";
                comboBox3.ValueMember = "Id";
                comboBox3.DataSource = dtP;


                comboBox3.SelectedItem = null;


                /*while(reader.Read())
                {
                    string programName = reader.GetString("Name");
                    
                    comboBox3.Items.Add(programName);
                }*/
            }
            catch(Exception ex)
            {
                Con.Close();
                MessageBox.Show(ex.Message);
            }
            Con.Close();
        }

        void Fill_Session()
        {
            Con.Open();

            string SessionQuery = "SELECT * FROM [Session] where Semester = 'Harmattan' ORDER BY Name";
            SqlCommand cmdSess = new SqlCommand(SessionQuery, Con);
            SqlDataReader readerSession = cmdSess.ExecuteReader();

            var dt = new DataTable();

            dt.Load(readerSession);

            try
            {
                //string prevName = "";

                //while (readerSession.Read())
                //{
                //string sessionName = readerSession.GetString("Name");
                //if (sessionName != prevName)
                //{
                comboBox2.DataSource = null;
                comboBox2.Items.Clear();

                
                
                comboBox2.DisplayMember = "Name";
                comboBox2.ValueMember = "Id";
                comboBox2.DataSource = dt;


                comboBox2.SelectedItem = null;

                //prevName = sessionName;
                //}
                //}
            }
            catch (Exception ex)
            {
                Con.Close();
                MessageBox.Show(ex.Message);
            }
            Con.Close();
        }


        void LoadTable()
        {
            
            //data table load
            Con.Open();
            string loadProg = "SELECT * FROM [Student]";

            //DataTable dt = new DataTable();

            SqlCommand cmdProg = new SqlCommand(loadProg, Con);
            int val1 = cmdProg.ExecuteNonQuery();


            SqlDataReader reader = cmdProg.ExecuteReader();

            dataStd.Rows.Clear();

            int i = 1;

            while (reader.Read())
            {
                string studentProgram = "";
                string studentYear = "";

                string loadProgData = "SELECT * FROM [Program] where Id = '" + reader.GetString("Program") + "'";
                SqlCommand cmdProgData = new SqlCommand(loadProgData, Con);
                SqlDataReader readerProgData = cmdProgData.ExecuteReader();
                if (readerProgData.Read())
                {
                    studentProgram = readerProgData.GetString("Name");
                }

                string loadYearData = "SELECT * FROM [Session] where Id = '" + reader.GetString("Year") + "'";
                SqlCommand cmdYearData = new SqlCommand(loadYearData, Con);
                SqlDataReader readerYearData = cmdYearData.ExecuteReader();
                if (readerYearData.Read())
                {
                    studentYear = readerYearData.GetString("Name");
                }

                string studentName = reader.GetString("Lastname") + ", " + reader.GetString("Firstname") + " " + reader.GetString("Middlename");
                string studentMatric = reader.GetString("Matric");


                dataStd.Rows.Add(i, studentName, studentMatric, studentProgram, studentYear);

                i++;
            }

            Con.Close();
            label18.Hide();
        }

        public static int parentX, parentY;

        private void button3_Click(object sender, EventArgs e)
        {
            
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button2_Leave(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void frmStudent_Load(object sender, EventArgs e)
        {
            pnlRgStd.Hide();
            comboGender.AutoSize = false;
            comboGender.Size = new System.Drawing.Size(280, 28);

            
        }

        private void label2_Click(object sender, EventArgs e)
        {
            pnlRgStd.Hide();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            //MessageBox.Show(comboBox3.GetItemText(comboBox3.SelectedValue) +" "+ comboBox3.GetItemText(comboBox3.SelectedItem));
            try
            {
                if ((txtMatric.Text == "") || (txtSurname.Text == "") || (textFirstname.Text == "") || (comboGender.SelectedItem == null) || (comboBox2.SelectedItem == null) || (comboBox3.SelectedItem == null))
                {
                    MessageBox.Show("You must fill all required fields");
                }
                else
                {
                    Con.Open();

                    bool exists = false;
                    string chechuser = "SELECT count(*) FROM [Student] where Matric='" + txtMatric.Text + "'";

                    SqlCommand cmdUser = new SqlCommand(chechuser, Con);
                    cmdUser.Parameters.AddWithValue("UserName", txtMatric.Text);
                    exists = (int)cmdUser.ExecuteScalar() > 0;

                    if (exists)
                    {
                        MessageBox.Show("Student Data Already Exists in Database");
                    }
                    else
                    {
                        String queryStd = "insert into Student(Matric,Gender,Program,Year,Firstname,Middlename,Lastname) values('" + txtMatric.Text + "','" + comboGender.Text + "','" + comboBox3.GetItemText(comboBox3.SelectedValue) + "','" + comboBox2.GetItemText(comboBox2.SelectedValue) + "','" + textFirstname.Text + "','" + textOthername.Text + "','" + txtSurname.Text + "')";
                        SqlCommand cmdStd = new SqlCommand(queryStd, Con);
                        cmdStd.ExecuteNonQuery();
                        MessageBox.Show("Student data successfuly Added");

                        

                        txtMatric.Text = "";
                        txtSurname.Text = "";
                        textFirstname.Text = "";
                        textOthername.Text = "";
                        comboGender.SelectedItem = null;
                        comboBox2.SelectedItem = null;
                        comboBox3.SelectedItem = null;
                    }
                    Con.Close();

                    LoadTable();
                }

            }
            catch (Exception ex)
            {
                Con.Close();
                MessageBox.Show(ex.Message);
            }
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void pnlRgStd_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnsearch_Click_1(object sender, EventArgs e)
        {
            //search button load

            if (txtSearch.Text != "")
            {
                label18.Show();
                dataStd.Rows.Clear();

                Con.Open();
                string loadProg = "SELECT * FROM [Student] where Matric like '%" + txtSearch.Text + "%' or Firstname like '%" + txtSearch.Text + "%' or Middlename like '%" + txtSearch.Text + "%' or Lastname like '%" + txtSearch.Text + "%'";

                //DataTable dt = new DataTable();

                SqlCommand cmdProg = new SqlCommand(loadProg, Con);

                //cmdProg.CommandType = CommandType.StoredProcedure;
                SqlDataReader reader = cmdProg.ExecuteReader();

                int i = 1;

                while (reader.Read())
                {
                    string studentProgram = "";
                    string studentYear = "";

                    string loadProgData = "SELECT * FROM [Program] where Id = '" + reader.GetString("Program") + "'";
                    SqlCommand cmdProgData = new SqlCommand(loadProgData, Con);
                    SqlDataReader readerProgData = cmdProgData.ExecuteReader();
                    if (readerProgData.Read())
                    {
                        studentProgram = readerProgData.GetString("Name");
                    }

                    string loadYearData = "SELECT * FROM [Session] where Id = '" + reader.GetString("Year") + "'";
                    SqlCommand cmdYearData = new SqlCommand(loadYearData, Con);
                    SqlDataReader readerYearData = cmdYearData.ExecuteReader();
                    if (readerYearData.Read())
                    {
                        studentYear = readerYearData.GetString("Name");
                    }

                    string studentName = reader.GetString("Lastname") + ", " + reader.GetString("Firstname") + " " + reader.GetString("Middlename");
                    string studentMatric = reader.GetString("Matric");
                    Int32 studId = reader.GetInt32("Id");


                    dataStd.Rows.Add(i, studentName, studentMatric, studentProgram, studentYear, studId);

                    i++;
                }

                // check Program
                string queryHod = "SELECT * FROM [Program] where Name like '%" + txtSearch.Text + "%'";
                SqlCommand cmdHod = new SqlCommand(queryHod, Con);
                SqlDataReader readerHOD = cmdHod.ExecuteReader();

                while (readerHOD.Read())
                {
                    Int32 staffId = readerHOD.GetInt32("Id");

                    string loadProg1 = "SELECT * FROM [Student] where Program = '" + staffId + "'";
                    SqlCommand cmdProg1 = new SqlCommand(loadProg1, Con);
                    SqlDataReader reader1 = cmdProg1.ExecuteReader();

                    while (reader1.Read())
                    {
                        string studentProgram = "";
                        string studentYear = "";

                        string loadProgData = "SELECT * FROM [Program] where Id = '" + reader1.GetString("Program") + "'";
                        SqlCommand cmdProgData = new SqlCommand(loadProgData, Con);
                        SqlDataReader readerProgData = cmdProgData.ExecuteReader();
                        if (readerProgData.Read())
                        {
                            studentProgram = readerProgData.GetString("Name");
                        }

                        string loadYearData = "SELECT * FROM [Session] where Id = '" + reader1.GetString("Year") + "'";
                        SqlCommand cmdYearData = new SqlCommand(loadYearData, Con);
                        SqlDataReader readerYearData = cmdYearData.ExecuteReader();
                        if (readerYearData.Read())
                        {
                            studentYear = readerYearData.GetString("Name");
                        }

                        string studentName = reader1.GetString("Lastname") + ", " + reader1.GetString("Firstname") + " " + reader1.GetString("Middlename");
                        string studentMatric = reader1.GetString("Matric");

                        Int32 studId = reader1.GetInt32("Id");

                        // Get total student table row count so far
                        Int32 count = dataStd.RowCount;
                        // Variable to store if id already exist
                        Int32 counting = 0;
                        // Loop through program table
                        for (Int32 j = 0; j < count; j++)
                        {
                            // Variable to store looping program Id
                            Int32 check = Int32.Parse(dataStd.Rows[j].Cells[5].Value.ToString());
                            // if student Id is already in table
                            if (studId == check)
                            {
                                // Increment variable
                                counting++;
                            }
                        }
                        // If variable is not increamented
                        if (counting == 0)
                        {
                            //update table
                            dataStd.Rows.Add(i, studentName, studentMatric, studentProgram, studentYear, studId);

                            i++;
                        }


                    }
                }

                // check session
                string querySession = "SELECT * FROM [Session] where Name like '%" + txtSearch.Text + "%'";
                SqlCommand cmdSession = new SqlCommand(querySession, Con);
                SqlDataReader readerSession = cmdSession.ExecuteReader();

                while (readerSession.Read())
                {
                    Int32 sessionId = readerSession.GetInt32("Id");

                    string loadProg2 = "SELECT * FROM [Student] where Year = '" + sessionId + "'";
                    SqlCommand cmdProg2 = new SqlCommand(loadProg2, Con);
                    SqlDataReader reader2 = cmdProg2.ExecuteReader();

                    while (reader2.Read())
                    {
                        string studentProgram = "";
                        string studentYear = "";

                        string loadProgData = "SELECT * FROM [Program] where Id = '" + reader2.GetString("Program") + "'";
                        SqlCommand cmdProgData = new SqlCommand(loadProgData, Con);
                        SqlDataReader readerProgData = cmdProgData.ExecuteReader();
                        if (readerProgData.Read())
                        {
                            studentProgram = readerProgData.GetString("Name");
                        }

                        string loadYearData = "SELECT * FROM [Session] where Id = '" + reader2.GetString("Year") + "'";
                        SqlCommand cmdYearData = new SqlCommand(loadYearData, Con);
                        SqlDataReader readerYearData = cmdYearData.ExecuteReader();
                        if (readerYearData.Read())
                        {
                            studentYear = readerYearData.GetString("Name");
                        }

                        string studentName = reader2.GetString("Lastname") + ", " + reader2.GetString("Firstname") + " " + reader2.GetString("Middlename");
                        string studentMatric = reader2.GetString("Matric");

                        Int32 studId = reader2.GetInt32("Id");

                        // Get total student table row count so far
                        Int32 count = dataStd.RowCount;
                        // Variable to store if id already exist
                        Int32 counting = 0;
                        // Loop through program table
                        for (Int32 j = 0; j < count; j++)
                        {
                            // Variable to store looping student Id
                            Int32 check = Int32.Parse(dataStd.Rows[j].Cells[5].Value.ToString());
                            // if program Id is already in table
                            if (studId == check)
                            {
                                // Increment variable
                                counting++;
                            }
                        }
                        // If variable is not increamented
                        if (counting == 0)
                        {
                            //update table
                            dataStd.Rows.Add(i, studentName, studentMatric, studentProgram, studentYear, studId);

                            i++;
                        }


                    }
                }

                Con.Close();
            }
        }

        private void dataStd_DoubleClick(object sender, EventArgs e)
        {
            string nameP = this.dataStd.CurrentRow.Cells[1].Value.ToString();

            frmStudentData programPage = new frmStudentData() { Dock = DockStyle.Fill, TopLevel = false, TopMost = true, FormBorderStyle = FormBorderStyle.None };
            programPage.label17.Text = nameP;
            programPage.studentId = this.dataStd.CurrentRow.Cells[2].Value.ToString();
            //programPage.FormBorderStyle = FormBorderStyle.None;
            contextual parentForm = ApplicationInstance.ContextualForm;
            if (parentForm != null)
            {
                parentForm.formerPage = "frmStudent"; // Track the former page
                parentForm.currentPage = "frmStudentData";
                parentForm.panel1.Controls.Clear(); // Clear existing controls in panel1
                parentForm.panel1.Controls.Add(programPage);
                parentForm.iconButton2.Show();
                parentForm.iconButton1.Show();
                programPage.Show();
            }
        }

        private void label18_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            LoadTable();

        }

        private void dataStd_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataStd.Columns[e.ColumnIndex].Name == "Column7")
            {
                dataStd_DoubleClick(sender, EventArgs.Empty);
            }
        }

        private void txtMatric_TextChanged(object sender, EventArgs e)
        {

        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            pnlRgStd.Show();
        }
    }
}
