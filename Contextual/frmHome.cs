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
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using DocumentFormat.OpenXml.Wordprocessing;
using GlobalVariable;

namespace Contextual
{
    public partial class frmHome : Form
    {
        public frmHome()
        {
            InitializeComponent();
            sessionIdText.Hide();
            Fill_Session();
        }

        string oldStartSession = "";
        string oldEndSession = "";

        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + GlobalVariable.Globals.databasePath + ";Integrated Security=True;MultipleActiveResultSets=True;Connect Timeout=30");

        void Fill_Session()
        {
            dataSession.Rows.Clear();

            if (Con.State != System.Data.ConnectionState.Open)
            {
                Con.Open();
            }

            string ChangeQuery = "IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Session' AND COLUMN_NAME = 'End') BEGIN EXEC sp_rename 'Session.[End]', 'EndSession', 'COLUMN' END";
            SqlCommand cmdChange = new SqlCommand(ChangeQuery, Con);
            cmdChange.ExecuteNonQuery();

            //string ProgramQuery = "SELECT * FROM [Session] ORDER BY NAME";
            string ProgramQuery = "SELECT * FROM [Session]  where Semester ='Harmattan' ORDER BY EndSession ASC";
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

            noOfProgram.Text = programsNo.ToString();
            label14.Text = lecturerNo.ToString();
            label16.Text = studentsNo.ToString();

            try
            {
                int i = 1;
                while (reader.Read())
                {
                    string sessionName = reader.GetString("Name");
                    int sessionStart = reader.GetInt32("Start");
                    int sessionEnd = reader.GetInt32("EndSession");
                    int sessionId = reader.GetInt32("Id");

                    dataSession.Rows.Add(i, sessionName, sessionStart, sessionEnd, sessionId);
                    dataSession.Sort(dataSession.Columns[3], System.ComponentModel.ListSortDirection.Ascending);
                    i++;
                }

                label9.Text = (i-1).ToString();


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
            sessionStart.Text = "";
            sessionEnd.Text = "";
            pnlRgSession.Show();
            button1.Show();
            buttonUpdate.Hide();
            buttonDelete.Hide();
            label23.Text = "Add Session";
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
                    if (Con.State != System.Data.ConnectionState.Open)
                    {
                        Con.Open();
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

                        return;
                    }

                    if (int.Parse(sessionEnd.Text) < int.Parse(sessionStart.Text))
                    {
                        MessageBox.Show("Invalid input\nSession End is greater than session start.");

                        return;
                    }

                    string semester = "Harmattan";

                    String hquery = "insert into Session(Name,Start,EndSession,Semester) values('" + addSession + "', '"+ sessionStart.Text + "','"+ sessionEnd.Text + "','" + semester + "')";
                    SqlCommand hcmd = new SqlCommand(hquery, Con);
                    hcmd.ExecuteNonQuery();

                    semester = "Rain";

                    String rquery = "insert into Session(Name,Start,EndSession,Semester) values('" + addSession + "','" + sessionStart.Text + "','" + sessionEnd.Text + "', '" + semester + "')";
                    SqlCommand rcmd = new SqlCommand(rquery, Con);
                    rcmd.ExecuteNonQuery();

                    MessageBox.Show("Session successfuly Added");
                    Con.Close();
                }

                Fill_Session();

            }
            catch (Exception ex)
            {
                Con.Close();
                MessageBox.Show(ex.Message);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            /*
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
            */
        }

        private void frmHome_Load(object sender, EventArgs e)
        {
            textBox1.Hide();
            //Con.Open();
            //string checPrg = "SELECT count(*) FROM [Program]";

            //SqlCommand cmdPrg = new SqlCommand(checPrg, Con);
            //cmdPrg.Parameters.AddWithValue("Name", addSession);
            //int programsNo = (int)cmdPrg.ExecuteScalar();


            pnlRgSession.Hide();
            /*
            labelSemester.Hide();
            selectSemester.Hide();
            */

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
            //DialogResult saveSession = MessageBox.Show("Do you want to make the selected session as current session?", "Save Session", MessageBoxButtons.YesNo);

            //if (saveSession == DialogResult.Yes)
            //{

            //}
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void dataSession_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataSession.Columns[e.ColumnIndex].Name == "action")
            {
                // Retrieve the values from the DataGridView cells
                var sessionStartValue = this.dataSession.CurrentRow.Cells[2].Value;
                var sessionEndValue = this.dataSession.CurrentRow.Cells[3].Value;

                string sessionStartText = sessionStartValue.ToString();
                string sessionEndText = sessionEndValue.ToString();

                if (sessionStartValue != null && sessionEndValue != null)
                {
                    sessionStartText = sessionStartValue.ToString();
                    sessionEndText = sessionEndValue.ToString();
                
                    sessionStart.Text = sessionStartValue + "-01-01"; // Display the first year from sessionStart
                    sessionEnd.Text = sessionEndValue + "-01-01"; // Display the first year from sessionEnd
                }
                else
                {
                    // Handle the invalid year scenario
                    MessageBox.Show("The year format is invalid. Please check the input.", "Invalid Year", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                label23.Text = "Edit Session";
                button1.Hide();
                buttonDelete.Show();
                buttonUpdate.Show();
                pnlRgSession.Show();

                //sessionSelectId.Text = this.dataSession.CurrentRow.Cells[3].Value.ToString();

                oldStartSession = sessionStartText;
                oldEndSession = sessionEndText;

                sessionIdText.Text = this.dataSession.CurrentRow.Cells[4].Value.ToString();
            }
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if ((sessionStart.Text == oldStartSession) && (sessionEnd.Text == oldEndSession))
                {
                    return; 
                }
                if ((sessionStart.Text == "") || (sessionEnd.Text == ""))
                {
                    MessageBox.Show("Session Start and Session Year cannot be empty");
                }
                else
                {
                    if (Con.State != System.Data.ConnectionState.Open)
                    {
                        Con.Open();
                    }

                    string addSession = sessionStart.Text + " / " + sessionEnd.Text;
                    string oldSession = oldStartSession + " / " + oldEndSession;

                    bool exists = false;
                    string chechuser = "SELECT count(*) FROM [Session] where Name='" + addSession + "'";

                    SqlCommand cmdUser = new SqlCommand(chechuser, Con);
                    cmdUser.Parameters.AddWithValue("Name", addSession);
                    exists = (int)cmdUser.ExecuteScalar() > 0;

                    if (exists)
                    {
                        MessageBox.Show("Session already exist in database");

                        return;
                    }

                    if (int.Parse(sessionEnd.Text) < int.Parse(sessionStart.Text))
                    {
                        MessageBox.Show("Invalid input\nSession End is greater than session start.");

                        return;
                    }

                    string semesterHarmattan = "Harmattan";
                    string semesterRain = "Rain";

                    // Update query for Harmattan semester
                    String hquery = "UPDATE Session SET Name = @addSession, Start = @sessionStart, EndSession = @sessionEnd, Semester = @semesterHarmattan WHERE Name = @oldSession AND Semester = @semesterHarmattan";
                    SqlCommand hcmd = new SqlCommand(hquery, Con);
                    hcmd.Parameters.AddWithValue("@addSession", addSession);
                    hcmd.Parameters.AddWithValue("@sessionStart", sessionStart.Text);
                    hcmd.Parameters.AddWithValue("@sessionEnd", sessionEnd.Text);
                    hcmd.Parameters.AddWithValue("@semesterHarmattan", semesterHarmattan);
                    hcmd.Parameters.AddWithValue("@oldSession", oldSession);
                    hcmd.ExecuteNonQuery();

                    // Update query for Rain semester
                    String rquery = "UPDATE Session SET Name = @addSession, Start = @sessionStart, EndSession = @sessionEnd, Semester = @semesterRain WHERE Name = @oldSession AND Semester = @semesterRain";
                    SqlCommand rcmd = new SqlCommand(rquery, Con);
                    rcmd.Parameters.AddWithValue("@addSession", addSession);
                    rcmd.Parameters.AddWithValue("@sessionStart", sessionStart.Text);
                    rcmd.Parameters.AddWithValue("@sessionEnd", sessionEnd.Text);
                    rcmd.Parameters.AddWithValue("@semesterRain", semesterRain);
                    rcmd.Parameters.AddWithValue("@oldSession", oldSession);
                    rcmd.ExecuteNonQuery();

                    MessageBox.Show("Session successfuly Added");
                    Con.Close();
                }
                Fill_Session();

                pnlRgSession.Hide();
                label23.Text = "Add Session";
                button1.Show();
                buttonDelete.Hide();
                buttonDelete.Hide();
                

            }
            catch (Exception ex)
            {
                Con.Close();
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (Con.State != System.Data.ConnectionState.Open)
            {
                Con.Open();
            }

            string oldSession = oldStartSession + " / " + oldEndSession;

            int sessionIdSelected = int.Parse(sessionIdText.Text);

            // Prompt the user for confirmation
            DialogResult result = MessageBox.Show("Are you sure you want to delete the session? Clicking yes will also delete all the Students and Results record stored under the Session's data.", "Delete Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                // Start a transaction to ensure all operations are completed or none at all
                SqlTransaction transaction = Con.BeginTransaction();

                try
                {
                    // Get the IDs of the sessions (Harmattan and Rain) for the old session
                    List<int> sessionIds = new List<int>();

                    string query = "SELECT Id FROM Session WHERE Name = @oldSession";
                    SqlCommand cmd = new SqlCommand(query, Con, transaction);
                    cmd.Parameters.AddWithValue("@oldSession", oldSession);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            sessionIds.Add(reader.GetInt32(0));
                        }
                    }

                    // If no IDs found, abort the operation
                    if (sessionIds.Count == 0)
                    {
                        MessageBox.Show("No sessions found with the specified name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        transaction.Rollback();
                        return;
                    }

                    // Delete from Student table where Year is in the session IDs
                    query = "DELETE FROM Student WHERE Year IN (" + string.Join(",", sessionIds) + ")";
                    cmd = new SqlCommand(query, Con, transaction);
                    cmd.ExecuteNonQuery();

                    // Delete from Result table where Session is in the session IDs
                    query = "DELETE FROM Result WHERE Session IN (" + string.Join(",", sessionIds) + ")";
                    cmd = new SqlCommand(query, Con, transaction);
                    cmd.ExecuteNonQuery();

                    // Delete from Session table where Id is in the session IDs
                    query = "DELETE FROM Session WHERE Id IN (" + string.Join(",", sessionIds) + ")";
                    cmd = new SqlCommand(query, Con, transaction);
                    cmd.ExecuteNonQuery();

                    // Get the nearest session ID less than sessionIdSelected
                    int? nearestSessionId = null;
                    query = "SELECT TOP 1 Id FROM Session WHERE Id < @sessionIdSelected ORDER BY Id DESC";
                    cmd = new SqlCommand(query, Con, transaction);
                    cmd.Parameters.AddWithValue("@sessionIdSelected", sessionIdSelected);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            nearestSessionId = reader.GetInt32(0);
                        }
                    }

                    // Update the Program table where Session is in the old session IDs
                    query = "UPDATE Program SET Session = @nearestSessionId WHERE Session IN (" + string.Join(",", sessionIds) + ")";
                    cmd = new SqlCommand(query, Con, transaction);
                    if (nearestSessionId.HasValue)
                    {
                        cmd.Parameters.AddWithValue("@nearestSessionId", nearestSessionId.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@nearestSessionId", DBNull.Value);
                    }
                    cmd.ExecuteNonQuery();

                    // Commit the transaction
                    transaction.Commit();
                    MessageBox.Show("Session deleted and records updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    // Rollback the transaction on error
                    transaction.Rollback();
                    MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Deletion canceled.", "Canceled", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            Fill_Session();

            pnlRgSession.Hide();

            Con.Close();

        }
    }
}
