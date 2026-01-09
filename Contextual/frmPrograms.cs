using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation.Text;
using System.Windows.Forms;
using GlobalVariable;

namespace Contextual
{
    public partial class frmPrograms : Form
    {
        public frmPrograms()
        {
            InitializeComponent();
            Fill_Hod();
            Fill_Dean();
            Fill_Session();
        }

        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + GlobalVariable.Globals.databasePath + ";Integrated Security=True;MultipleActiveResultSets=True;Connect Timeout=30");

        void Load_Table()
        {
            dataPrg.Rows.Clear();
            Con.Open();

            try
            {
                string loadProg = "SELECT * FROM [Program]";

            //DataTable dt = new DataTable();

            SqlCommand cmdProg = new SqlCommand(loadProg, Con);

            //cmdProg.CommandType = CommandType.StoredProcedure;
            SqlDataReader reader = cmdProg.ExecuteReader();

            int i = 1;

            while (reader.Read())
            {
                string programHod = "";
                string programSession = "";

                string loadStaffData = "SELECT * FROM [Lecturer] where Id = '" + reader.GetString("Hod") + "'";
                SqlCommand cmdStaffData = new SqlCommand(loadStaffData, Con);
                SqlDataReader readerStaffData = cmdStaffData.ExecuteReader();
                if (readerStaffData.Read())
                {
                    programHod = readerStaffData.GetString("Name");
                }

                string loadYearData = "SELECT * FROM [Session] where Id = '" + reader.GetString("Session") + "'";
                SqlCommand cmdYearData = new SqlCommand(loadYearData, Con);
                SqlDataReader readerYearData = cmdYearData.ExecuteReader();
                if (readerYearData.Read())
                {
                    programSession = readerYearData.GetString("Name");
                }

                string programName = reader.GetString("Name");
                string programAbbr = reader.GetString("Abbr");
                Int32 progId = reader.GetInt32("Id");

                dataPrg.Rows.Add(i, programName, programAbbr, programHod, programSession, progId);

                i++;
            }

            Con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

                Con.Close();
            }

            label18.Hide();
        }

        void Fill_Session()
        {
            Con.Open();

            string SessionQuery = "SELECT * FROM [Session] where Semester = 'Harmattan'";
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
                comboCurrent.DataSource = null;
                comboCurrent.Items.Clear();



                comboCurrent.DisplayMember = "Name";
                comboCurrent.ValueMember = "Id";
                comboCurrent.DataSource = dt;


                comboCurrent.SelectedItem = null;

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

        void Fill_Hod()
        {
            Con.Open();

            string HodQuery = "SELECT * FROM [Lecturer]";
            SqlCommand cmdHod = new SqlCommand(HodQuery, Con);
            SqlDataReader reader = cmdHod.ExecuteReader();

            var dtHod = new DataTable();

            dtHod.Load(reader);

            try
            {

                selectHod.DataSource = null;
                selectHod.Items.Clear();



                selectHod.DisplayMember = "Name";
                selectHod.ValueMember = "Id";
                selectHod.DataSource = dtHod;


                selectHod.SelectedItem = null;


                /*while(reader.Read())
                {
                    string programName = reader.GetString("Name");
                    
                    comboBox3.Items.Add(programName);
                }*/
            }
            catch (Exception ex)
            {
                Con.Close();
                MessageBox.Show(ex.Message);
            }
            Con.Close();
        }

        void Fill_Dean()
        {
            Con.Open();

            string DeanQuery = "SELECT * FROM [Lecturer]";
            SqlCommand cmdDean = new SqlCommand(DeanQuery, Con);
            SqlDataReader readerDean = cmdDean.ExecuteReader();

            var dtDean = new DataTable();

            dtDean.Load(readerDean);

            try
            {
                //string prevName = "";

                //while (readerSession.Read())
                //{
                //string sessionName = readerSession.GetString("Name");
                //if (sessionName != prevName)
                //{
                selectDean.DataSource = null;
                selectDean.Items.Clear();



                selectDean.DisplayMember = "Name";
                selectDean.ValueMember = "Id";
                selectDean.DataSource = dtDean;


                selectDean.SelectedItem = null;

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

        private void iconButton1_Click(object sender, EventArgs e)
        {
            pnlRgPrg.Show();
        }

        private void label11_Click(object sender, EventArgs e)
        {
            pnlRgPrg.Hide();
        }

        private void frmPrograms_Load(object sender, EventArgs e)
        {
            pnlRgPrg.Hide();

            Load_Table();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if ((txtPrgName.Text == "") || (textTotalSemester.Text == "") || (comboCurrent.SelectedItem == null) || (comboGrading.SelectedItem == null) || (comboCurrSem.SelectedItem == null))
                {
                    MessageBox.Show("All Required field must not be empty");
                }
                else
                {
                    Con.Open();

                    bool exists = false;
                    string chechprg = "SELECT count(*) FROM [Program] where Name like '" + txtPrgName.Text + "'";

                    SqlCommand cmdPrg = new SqlCommand(chechprg, Con);
                    cmdPrg.Parameters.AddWithValue("Name", txtPrgName.Text);
                    exists = (int)cmdPrg.ExecuteScalar() > 0;

                    if (exists)
                    {
                        MessageBox.Show("Program Name Already Exists");
                    }
                    else
                    {
                        bool abbrExists = false;
                        string chechAbbr = "SELECT count(*) FROM [Program] where Abbr like '" + txtPrgAbbr.Text + "'";

                        SqlCommand cmdAbbr = new SqlCommand(chechAbbr, Con);
                        cmdAbbr.Parameters.AddWithValue("Abbr", txtPrgAbbr.Text);
                        abbrExists = (int)cmdAbbr.ExecuteScalar() > 0;

                        if ((txtPrgAbbr.Text != "") & (abbrExists))
                        {
                            MessageBox.Show("Abbriviation Already Exists");
                        }
                        else
                        {
                            long numb = 0;
                            bool canConvert = long.TryParse(textTotalSemester.Text, out numb);
                            if (canConvert)
                            {
                                string sessionId = "";
                                string sessionSemester = "";

                                if (comboCurrSem.Text == "H")
                                {
                                    sessionSemester = "Harmattan";
                                }
                                else
                                {
                                    sessionSemester = "Rain";
                                }

                                string loadStaffData = "SELECT * FROM [Session] where Name = '" + comboCurrent.Text + "' and Semester = '" + sessionSemester + "'";
                                SqlCommand cmdStaffData = new SqlCommand(loadStaffData, Con);
                                SqlDataReader readerStaffData = cmdStaffData.ExecuteReader();
                                if (readerStaffData.Read())
                                {
                                    sessionId = readerStaffData["Id"].ToString();
                                }

                                String query = "insert into Program(Name,Abbr,Hod,Dean,Session,Total,Grading,Lvl_mark,Pass,Examiner) values('" + txtPrgName.Text + "','" + txtPrgAbbr.Text + "','" + selectHod.GetItemText(selectHod.SelectedValue) + "','" + selectDean.GetItemText(selectDean.SelectedValue) + "','" + sessionId + "','" + textTotalSemester.Text + "','" + comboGrading.Text + "','',-1,'')";
                                SqlCommand cmd = new SqlCommand(query, Con);
                                cmd.ExecuteNonQuery();
                                MessageBox.Show("Program successfuly Added");

                                txtPrgName.Text = "";
                                txtPrgAbbr.Text = "";
                                selectHod.SelectedItem = null;
                                selectDean.SelectedItem = null;
                                textTotalSemester.Text = "";
                                comboGrading.SelectedItem = null;
                                comboCurrent.SelectedItem = null;
                                comboCurrSem.SelectedItem = null;
                            }
                            else
                            {
                                MessageBox.Show("Total Semester value must be numeric");
                            }
                        }
                    }
                    Con.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

                Con.Close();
            }

            Load_Table();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void btnsearch_Click(object sender, EventArgs e)
        {
            

            if (txtSearch.Text != "")
            {
                label18.Show();

                dataPrg.Rows.Clear();

                Con.Open();
                string loadProg = "SELECT * FROM [Program] where Name like '%" + txtSearch.Text + "%' or Abbr like '%" + txtSearch.Text + "%'";
                SqlCommand cmdProg = new SqlCommand(loadProg, Con);
                SqlDataReader reader = cmdProg.ExecuteReader();

                int i = 1;

                while (reader.Read())
                {
                    string programHod = "";
                    string programSession = "";

                    string loadStaffData = "SELECT * FROM [Lecturer] where Id = '" + reader.GetString("Hod") + "'";
                    SqlCommand cmdStaffData = new SqlCommand(loadStaffData, Con);
                    SqlDataReader readerStaffData = cmdStaffData.ExecuteReader();
                    if (readerStaffData.Read())
                    {
                        programHod = readerStaffData.GetString("Name");
                    }

                    string loadYearData = "SELECT * FROM [Session] where Id = '" + reader.GetString("Session") + "'";
                    SqlCommand cmdYearData = new SqlCommand(loadYearData, Con);
                    SqlDataReader readerYearData = cmdYearData.ExecuteReader();
                    if (readerYearData.Read())
                    {
                        programSession = readerYearData.GetString("Name");
                    }

                    string programName = reader.GetString("Name");
                    string programAbbr = reader.GetString("Abbr");
                    Int32 progId = reader.GetInt32("Id");

                    dataPrg.Rows.Add(i, programName, programAbbr, programHod, programSession, progId);

                    i++;
                }

                // check Hod
                string queryHod = "SELECT * FROM [Lecturer] where Name like '%" + txtSearch.Text + "%'";
                SqlCommand cmdHod = new SqlCommand(queryHod, Con);
                SqlDataReader readerHOD = cmdHod.ExecuteReader();

                while (readerHOD.Read())
                {
                    Int32 staffId = readerHOD.GetInt32("Id");

                    string loadProg1 = "SELECT * FROM [Program] where Hod = '" + staffId + "'";
                    SqlCommand cmdProg1 = new SqlCommand(loadProg1, Con);
                    SqlDataReader reader1 = cmdProg1.ExecuteReader();

                    while (reader1.Read())
                    {
                        string programHod = "";
                        string programSession = "";

                        string loadStaffData = "SELECT * FROM [Lecturer] where Id = '" + reader1.GetString("Hod") + "'";
                        SqlCommand cmdStaffData = new SqlCommand(loadStaffData, Con);
                        SqlDataReader readerStaffData = cmdStaffData.ExecuteReader();
                        if (readerStaffData.Read())
                        {
                            programHod = readerStaffData.GetString("Name");
                        }

                        string loadYearData = "SELECT * FROM [Session] where Id = '" + reader1.GetString("Session") + "'";
                        SqlCommand cmdYearData = new SqlCommand(loadYearData, Con);
                        SqlDataReader readerYearData = cmdYearData.ExecuteReader();
                        if (readerYearData.Read())
                        {
                            programSession = readerYearData.GetString("Name");
                        }

                        string programName = reader1.GetString("Name");
                        string programAbbr = reader1.GetString("Abbr");
                        Int32 progId = reader1.GetInt32("Id");

                        // Get total program table row count so far
                        Int32 count = dataPrg.RowCount;
                        // Variable to store if id already exist
                        Int32 counting = 0;
                        // Loop through program table
                        for (Int32 j = 0; j < count; j++)
                        {
                            // Variable to store looping program Id
                            Int32 check = Int32.Parse(dataPrg.Rows[j].Cells[5].Value.ToString());
                            // if program Id is already in table
                            if(progId == check)
                            {
                                // Increment variable
                                counting++;
                            }
                        }
                        // If variable is not increamented
                        if (counting == 0)
                        {
                            //update table
                            dataPrg.Rows.Add(i, programName, programAbbr, programHod, programSession, progId);

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

                    string loadProg2 = "SELECT * FROM [Program] where Session = '" + sessionId + "'";
                    SqlCommand cmdProg2 = new SqlCommand(loadProg2, Con);
                    SqlDataReader reader2 = cmdProg2.ExecuteReader();

                    while (reader2.Read())
                    {
                        string programHod = "";
                        string programSession = "";

                        string loadStaffData = "SELECT * FROM [Lecturer] where Id = '" + reader2.GetString("Hod") + "'";
                        SqlCommand cmdStaffData = new SqlCommand(loadStaffData, Con);
                        SqlDataReader readerStaffData = cmdStaffData.ExecuteReader();
                        if (readerStaffData.Read())
                        {
                            programHod = readerStaffData.GetString("Name");
                        }

                        string loadYearData = "SELECT * FROM [Session] where Id = '" + reader2.GetString("Session") + "'";
                        SqlCommand cmdYearData = new SqlCommand(loadYearData, Con);
                        SqlDataReader readerYearData = cmdYearData.ExecuteReader();
                        if (readerYearData.Read())
                        {
                            programSession = readerYearData.GetString("Name");
                        }

                        string programName = reader2.GetString("Name");
                        string programAbbr = reader2.GetString("Abbr");
                        Int32 progId = reader2.GetInt32("Id");

                        // Get total program table row count so far
                        Int32 count = dataPrg.RowCount;
                        // Variable to store if id already exist
                        Int32 counting = 0;
                        // Loop through program table
                        for (Int32 j = 0; j < count; j++)
                        {
                            // Variable to store looping program Id
                            Int32 check = Int32.Parse(dataPrg.Rows[j].Cells[5].Value.ToString());
                            // if program Id is already in table
                            if (progId == check)
                            {
                                // Increment variable
                                counting++;
                            }
                        }
                        // If variable is not increamented
                        if (counting == 0)
                        {
                            //update table
                            dataPrg.Rows.Add(i, programName, programAbbr, programHod, programSession, progId);

                            i++;
                        }


                    }
                }

                //dt.Load(reader);

                //dataPrg.DataSource= dt;

                Con.Close();
            }
            else
            {
                MessageBox.Show("Search text is empty");
            }
            
        }

        private void dataPrg_DoubleClick(object sender, EventArgs e)
        {
            string nameP = this.dataPrg.CurrentRow.Cells[2].Value.ToString() + " (" + this.dataPrg.CurrentRow.Cells[1].Value.ToString() + ")";

            frmProgramData programPage = new frmProgramData() { Dock = DockStyle.Fill, TopLevel = false, TopMost = true, FormBorderStyle = FormBorderStyle.None };
            programPage.label17.Text = nameP;
            programPage.label5.Text = this.dataPrg.CurrentRow.Cells[3].Value.ToString();
            programPage.programName = this.dataPrg.CurrentRow.Cells[1].Value.ToString();
            contextual parentForm = ApplicationInstance.ContextualForm;
            if (parentForm != null)
            {
                parentForm.formerPage = "frmDashboard-studentTable"; // Track the former page
                parentForm.currentPage = "frmProgramData";
                parentForm.panel1.Controls.Clear(); // Clear existing controls in panel1
                parentForm.panel1.Controls.Add(programPage);
                parentForm.iconButton2.Show();
                parentForm.iconButton1.Show();
                programPage.Show();
            }
        }

        private void label18_Click(object sender, EventArgs e)
        {
            Load_Table();
            txtSearch.Text = "";
        }

        private void dataPrg_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataPrg.Columns[e.ColumnIndex].Name == "Column7")
            {
                dataPrg_DoubleClick(sender, EventArgs.Empty);
            }
        }

        private void textTotalSemester_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
