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
using System.Xml.Linq;
using GlobalVariable;

namespace Contextual
{
    public partial class frmHome : Form
    {
        public frmHome()
        {
            InitializeComponent();
            Fill_Session();
        }

        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + GlobalVariable.Globals.databasePath + ";Integrated Security=True;MultipleActiveResultSets=True;Connect Timeout=30");

        void Fill_Session()
        {
            Con.Open();

            string ChangeQuery = "IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Session' AND COLUMN_NAME = 'End') BEGIN EXEC sp_rename 'Session.[End]', 'EndSession', 'COLUMN' END";
            SqlCommand cmdChange = new SqlCommand(ChangeQuery, Con);
            cmdChange.ExecuteNonQuery();

            Con.Close();

            Con.Open();

            string ProgramQuery = "SELECT * FROM [Session] ORDER BY NAME";
            SqlCommand cmdProg = new SqlCommand(ProgramQuery, Con);
            SqlDataReader reader = cmdProg.ExecuteReader();

            string checPrg = "SELECT count(*) FROM [Program]";
            SqlCommand cmdPrg = new SqlCommand(checPrg, Con);
            int programsNo = (int)cmdPrg.ExecuteScalar();

            string checLec = "SELECT count(*) FROM [Lecturer]";
            SqlCommand cmdLec = new SqlCommand(checLec, Con);
            int lecturerNo = (int)cmdLec.ExecuteScalar();

            string checStud = "SELECT count(*) FROM [Student]";
            SqlCommand cmdStud = new SqlCommand(checStud, Con);
            int studentsNo = (int)cmdStud.ExecuteScalar();

            try
            {
                while (reader.Read())
                {
                    string programName = reader.GetString("Name");
                    string currentSession = reader.GetString("curr");
                    string currentSemester = reader.GetString("Semester");
                    comboBox3.Items.Add(programName + " (" + currentSemester.Substring(0,1) + ")");

                    if (currentSession == "1")
                    {
                        comboBox3.SelectedItem = programName + " (" + currentSemester.Substring(0, 1) + ")";

                        label9.Text = programName;
                        label10.Text = currentSemester;
                        noOfProgram.Text = programsNo.ToString();
                        label14.Text = lecturerNo.ToString();
                        label16.Text = studentsNo.ToString();
                    }
                }
                Con.Close();
            }
            catch (Exception ex)
            {
                Con.Close();
                MessageBox.Show(ex.Message);
            }

        }

        private void panel15_Paint(object sender, PaintEventArgs e)
        {

        }

        private void iconButton2_Click(object sender, EventArgs e)
        {
            pnlRgSession.Show();    
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Int32 currentSession = 0;
            try
            {
                if ((sessionStart.Text == "") || (sessionEnd.Text == ""))
                {
                    MessageBox.Show("Session Start and Session Year cannot be empty");
                }
                else
                {
                    Con.Open();

                    string loadCurrData = "SELECT * FROM [Session] where Curr = '1'";
                    SqlCommand cmdCurrData = new SqlCommand(loadCurrData, Con);
                    SqlDataReader readerCurrData = cmdCurrData.ExecuteReader();

                    if (readerCurrData.Read())
                    {
                        currentSession = readerCurrData.GetInt32("Id");
                    }

                    string addSession = sessionStart.Text + " / " + sessionEnd.Text;

                    bool exists = false;
                    string chechuser = "SELECT count(*) FROM [Session] where Name='" + addSession + "'";

                    SqlCommand cmdUser = new SqlCommand(chechuser, Con);
                    cmdUser.Parameters.AddWithValue("Name", addSession);
                    exists = (int)cmdUser.ExecuteScalar() > 0;

                    if (exists)
                    {
                        MessageBox.Show("Session already exist in database");
                    }
                    else
                    {
                        string curry = "0";

                        if (checkBox1.Checked)
                        {
                            curry = "1";

                            if (selectSemester.Text != "")
                            {
                                if (selectSemester.Text == "Harmattan")
                                {

                                //String hquery = "insert into Session(Name,Start,End,Semester,Curr) values('" + addSession + "','" + sessionStart.Text + "','" + sessionEnd.Text + "', '" + selectSemester.Text + "', '" + curry + "')";
                                //SqlCommand hcmd = new SqlCommand(hquery, Con);
                                //hcmd.ExecuteNonQuery();

                                    string hquery = "insert into Session(Name,Start,EndSession,Semester,Curr) values(@name, @start, @end, @semester, @curry)";
                                    SqlCommand hcmd = new SqlCommand(hquery, Con);
                                    hcmd.Parameters.AddWithValue("@name", addSession);
                                    hcmd.Parameters.AddWithValue("@start", sessionStart.Text);
                                    hcmd.Parameters.AddWithValue("@end", sessionEnd.Text);
                                    hcmd.Parameters.AddWithValue("@semester", selectSemester.Text);
                                    hcmd.Parameters.AddWithValue("@curry", curry);
                                    hcmd.ExecuteNonQuery();

                                    string unselectSemester = "Rain";

                                    String rquery = "insert into Session(Name,Start,EndSession,Semester) values('" + addSession + "','"+ sessionStart.Text + "','"+ sessionEnd.Text + "', '" + unselectSemester + "')";
                                    SqlCommand rcmd = new SqlCommand(rquery, Con);
                                    rcmd.ExecuteNonQuery();

                                    string updateQuery = "update Session set Curr = '0' where Id = '" + currentSession + "'";
                                    SqlCommand cmdUpdate = new SqlCommand(updateQuery, Con);
                                    cmdUpdate.ExecuteNonQuery();

                                    Fill_Session();

                                    MessageBox.Show("Session successfuly Added");

                                }else if (selectSemester.Text == "Rain")
                                {
                                    string unselectSemester = "Harmattan";

                                    String hquery = "insert into Session(Name,Start,EndSession,Semester) values('" + addSession + "','" + sessionStart.Text + "','" + sessionEnd.Text + "', '" + unselectSemester + "')";
                                    SqlCommand hcmd = new SqlCommand(hquery, Con);
                                    hcmd.ExecuteNonQuery();

                                    String rquery = "insert into Session(Name,Start,EndSession,Semester,Curr) values('" + addSession + "','"+ sessionStart.Text + "','"+ sessionEnd.Text + "', '" + selectSemester.Text + "', '" + curry + "')";
                                    SqlCommand rcmd = new SqlCommand(rquery, Con);
                                    rcmd.ExecuteNonQuery();

                                    string updateQuery = "update Session set Curr = '0' where Id = '" + currentSession + "'";
                                    SqlCommand cmdUpdate = new SqlCommand(updateQuery, Con);
                                    cmdUpdate.ExecuteNonQuery();

                                    Fill_Session();

                                    MessageBox.Show("Session successfuly Added");
                                }
                                else
                                {
                                    MessageBox.Show("You have signified the input as current session but no semester is selected");
                                }
                            }
                        }
                        else
                        {
                            string unselectSemester = "Harmattan";

                            String hquery = "insert into Session(Name,Start,EndSession,Semester) values('" + addSession + "', '"+ sessionStart.Text + "','"+ sessionEnd.Text + "','" + unselectSemester + "')";
                            SqlCommand hcmd = new SqlCommand(hquery, Con);
                            hcmd.ExecuteNonQuery();

                            unselectSemester = "Rain";

                            String rquery = "insert into Session(Name,Start,EndSession,Semester) values('" + addSession + "','" + sessionStart.Text + "','" + sessionEnd.Text + "', '" + unselectSemester + "')";
                            SqlCommand rcmd = new SqlCommand(rquery, Con);
                            rcmd.ExecuteNonQuery();

                            MessageBox.Show("Session successfuly Added");
                        }

                    }
                    Con.Close();
                }

            }
            catch (Exception ex)
            {
                Con.Close();
                MessageBox.Show(ex.Message);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                labelSemester.Show();
                selectSemester.Show();
            }
            else
            {
                labelSemester.Hide();
                selectSemester.Hide();
            }
        }

        private void frmHome_Load(object sender, EventArgs e)
        {
            //Con.Open();
            //string checPrg = "SELECT count(*) FROM [Program]";

            //SqlCommand cmdPrg = new SqlCommand(checPrg, Con);
            //cmdPrg.Parameters.AddWithValue("Name", addSession);
            //int programsNo = (int)cmdPrg.ExecuteScalar();


            pnlRgSession.Hide();
            labelSemester.Hide();
            selectSemester.Hide();

            //noOfProgram.Text = programsNo;

            //Con.Close();
        }

        private void label21_Click(object sender, EventArgs e)
        {
            pnlRgSession.Hide();
            
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void comboBox3_SelectedValueChanged(object sender, EventArgs e)
        {
           
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            DialogResult saveSession = MessageBox.Show("Do you want to make the selected session as current session?", "Save Session", MessageBoxButtons.YesNo);

            if (saveSession == DialogResult.Yes)
            {

            }
        }
    }
}
