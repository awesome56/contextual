using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Contextual.Properties;
using DGVPrinterHelper;
using GlobalVariable;
using ClosedXML.Excel;
using static System.Net.WebRequestMethods;
using System.Diagnostics.Metrics;

namespace Contextual
{
    public partial class frmMms : Form
    {
        public frmMms()
        {
            InitializeComponent();
        }

        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + GlobalVariable.Globals.databasePath + ";Integrated Security=True;MultipleActiveResultSets=True;Connect Timeout=30");

        public string session;
        public Int32 sessionId;
        public string level;
        public string semester;
        public string program;
        public string programAbbr;
        public string programHod;
        public string programDean;
        public string programExaminer;
        public Int32 programMax;
        public Int32 programId;
        public Int32 programStart;
        public Int32 levelSessionId;
        // public string sCarryOvers;
        //public string sCourses;
        public string lvlMark;
        public string gradeType;
        public string suffix;
        public string programPointName;
        public string programSemesterPointName;
        public Int32 passMark;

        public String DecimalPlaces(Decimal scoreMark) 
        {
            string stringValue = scoreMark.ToString();
            string[] parts = stringValue.Split('.');

            if (parts.Length == 2 && parts[1].Length == 1)
            {
                return scoreMark.ToString("N2");
            }
            else if (parts.Length == 2 && parts[1].Length == 2)
            {
                return scoreMark.ToString();
            }
            else
            {
                return scoreMark.ToString("N2");
            }

        }
        public string Grading(Int32 scoreMark)
        {
            string gradeScore = "";

            if ((scoreMark >= 70) && (scoreMark <= 100))
            {
                return gradeScore = "A";
            }
            else if ((scoreMark >= 60) && (scoreMark < 70))
            {
                return gradeScore = "B";
            }
            else if ((scoreMark >= 50) && (scoreMark < 60))
            {
                return gradeScore = "C";
            }
            else if ((scoreMark >= 45) && (scoreMark < 50))
            {
                return gradeScore = "D";
            }
            else if ((scoreMark >= 40) && (scoreMark < 45))
            {
                return gradeScore = "E";
            }
            else
            {
                return gradeScore = "F";
            }

            if (scoreMark < 0)
            {
                return gradeScore = "AR";
            }
        }

        public Int32 GradePoint(Int32 scoreMark)
        {
            Int32 gradePoint = 0;

            if ((scoreMark >= 70) && (scoreMark <= 100))
            {
                return gradePoint = 5;
            }
            else if ((scoreMark >= 60) && (scoreMark < 70))
            {
                return gradePoint = 4;
            }
            else if ((scoreMark >= 50) && (scoreMark < 60))
            {
                return gradePoint = 3;
            }
            else if ((scoreMark >= 45) && (scoreMark < 50))
            {
                return gradePoint = 2;
            }
            else if ((scoreMark >= 40) && (scoreMark < 45))
            {
                return gradePoint = 1;
            }
            else
            {
                return gradePoint = 0;
            }
            /*
            if (scoreMark < 0)
            {
                return gradeScore = "AR";
            }
            */
        }

        private void frmMms_Load(object sender, EventArgs e)
        {
            Con.Open();
            string ProgramQuery = "SELECT * FROM Program where Id = '" + programId + "'";
            SqlCommand cmdProg = new SqlCommand(ProgramQuery, Con);
            SqlDataReader readerProgram = cmdProg.ExecuteReader();

            if (readerProgram.Read())
            {
                program = readerProgram.GetString("Name");
                programMax = Int32.Parse(readerProgram.GetString("Total"));
                programAbbr = readerProgram.GetString("Abbr");

                gradeType = readerProgram.GetString("Grading");

                if (readerProgram.GetString("Grading") == "Percentage Grading")
                {
                    suffix = "%";
                    programPointName = "CPT%";
                    programSemesterPointName = "PT%";
                }
                else
                {
                    suffix = "";
                    programPointName = "CGPA";
                    programSemesterPointName = "GPA";

                    // Change column headers for non-percentage grading
                    dataGridView1.Columns[6].HeaderText = "PREV\n"+programPointName;
                    dataGridView1.Columns[9].HeaderText = "PRES\n" + programSemesterPointName;
                    dataGridView1.Columns[12].HeaderText = "PRES\n" + programPointName;

                    dataGridView2.Columns[6].HeaderText = "PREV\n"+programPointName;
                    dataGridView2.Columns[9].HeaderText = "PRES\n" + programSemesterPointName;
                    dataGridView2.Columns[12].HeaderText = "PRES\n"+programPointName;

                    dataGridView3.Columns[6].HeaderText = "PREV\n"+programPointName;
                    dataGridView3.Columns[9].HeaderText = "PRES\n" + programSemesterPointName;
                    dataGridView3.Columns[12].HeaderText = "PRES\n"+programPointName;
                }

                if (readerProgram.GetString("Lvl_mark") == "")
                {
                    lvlMark = "Masters";
                }
                else
                {
                    lvlMark = readerProgram.GetString("Lvl_mark");
                }

                if (readerProgram.GetInt32("Pass") == -1)
                {
                    passMark = 40;
                }
                else
                {
                    passMark = readerProgram.GetInt32("Pass");
                }

                string ExaminerQuery = "SELECT * FROM Lecturer where Id = '" + readerProgram.GetString("Examiner") + "'";
                SqlCommand cmdExaminer = new SqlCommand(ExaminerQuery, Con);
                SqlDataReader readerExaminer = cmdExaminer.ExecuteReader();

                if (readerExaminer.Read())
                {
                    programExaminer = readerExaminer.GetString("Name");
                }

                string HODQuery = "SELECT * FROM Lecturer where Id = '" + readerProgram.GetString("Hod") + "'";
                SqlCommand cmdHOD = new SqlCommand(HODQuery, Con);
                SqlDataReader readerHOD = cmdHOD.ExecuteReader();

                if (readerHOD.Read())
                {
                    programHod = readerHOD.GetString("Name");
                }

                string DeanQuery = "SELECT * FROM Lecturer where Id = '" + readerProgram.GetString("Dean") + "'";
                SqlCommand cmdDean = new SqlCommand(DeanQuery, Con);
                SqlDataReader readerDean = cmdDean.ExecuteReader();

                if (readerDean.Read())
                {
                    programDean = readerDean.GetString("Name");
                }
            }
            label15.Text = program;
            label7.Text = session;

            if (lvlMark == "Undergraduate")
            {
                label9.Text = level +  "00 LEVEL";
            }

            if (lvlMark == "Post Graduate")
            {
                label9.Text = Int32.Parse(level) + 6 + "00 LEVEL";
            }

            if (lvlMark == "Masters")
            {
                label9.Text = "Year " + level;
            }

            if (lvlMark == "PHD")
            {
                label9.Text = "Year " + level;
            }

            if (lvlMark == "ND")
            {
                label9.Text = "ND " + level;
            }

            if (lvlMark == "HND")
            {
                label9.Text = "HND " + level;
            }

            if (lvlMark == "Year")
            {
                label9.Text = "Year " + level;
            }
            

            if (semester == "H")
            {
                label11.Text = "Harmattan Semester";
            }
            else
            {
                label11.Text = "Rain Semester";
            }

            DateTime currentTime = DateTime.Now;

            string time = currentTime.ToString("hh:mm tt");

            DateTime currentDate = DateTime.Today;

            string date = currentDate.ToString("dd/MM/yyyy");

            label13.Text = date + " " + time;

            Int32 sessionStart = Int32.Parse(session.Substring(0, 4));

            Int32 divString = (int)Math.Ceiling((double)programMax / (double)2);

            // Query to get all session that falls within the frame of level 1 - max.
            string Query = "SELECT TOP " + divString + " * FROM Session where Start <= " + sessionStart + "and Semester = 'Harmattan' ORDER BY NAME DESC";
            SqlCommand cmd = new SqlCommand(Query, Con);
            SqlDataReader reader = cmd.ExecuteReader();
            Int32 count = 1;
            // Level session start is the year of admission of the year of students of the level to query
            int levelSessionStart = 0;
            while (reader.Read())
            {

                if (count == Int32.Parse(level))
                {
                    //if count is equal to the level we want to print its report. Get the session ID
                    levelSessionId = reader.GetInt32("Id");

                    // Get start year of the session Id of above.
                    levelSessionStart = GlobalVariable.Globals.getSessionStart(levelSessionId, Con);

                    //MessageBox.Show(levelSessionId.ToString());
                }
                count++;
            }

            // Check if summary requst is for final year students
            if (int.Parse(level) == divString)
            {
                // Query student table to get student list
                string SessionQuery = "SELECT * FROM [Student] where Program = '" + programId + "' ORDER BY Matric";
                SqlCommand cmdSess = new SqlCommand(SessionQuery, Con);
                SqlDataReader readerSession = cmdSess.ExecuteReader();
                Int32 sn = 1;
                Int32 sn2 = 1;
                Int32 sn3 = 1;
                while (readerSession.Read())
                {
                    int studentId = readerSession.GetInt32("Id");
                    // string studentMatric = readerSession.GetString("Matric");
                    int studentEntryYearId = int.Parse(readerSession.GetString("Year"));

                    // Student must be in 500L or suppose to be in graduate year
                    int studentStartYear = GlobalVariable.Globals.getSessionStart(studentEntryYearId, Con);

                    if (studentStartYear <= levelSessionStart)
                    {
                        
                        // Check if Student has Extra courses not yet passed
                        // List<int> studentExtraCourses = new List<int>();
                        //studentExtraCourses = GlobalVariable.Globals.CheckStudentExtra(readerSession.GetInt32("Id").ToString(), Con);
                        //studentExtraCourses = GlobalVariable.Globals.StudentSessionExtra(readerSession.GetInt32("Id").ToString(), sessionId, Con);

                        List<int> studentSessionEx = new List<int>();
                        studentSessionEx = GlobalVariable.Globals.StudentSessionExtra(studentId.ToString(), sessionId, Con);

                        //MessageBox.Show($"{studentMatric} used {GlobalVariable.Globals.CheckStudentGraduateNo(studentId.ToString(), Con)}: {string.Join(", ", studentSessionEx)}");

                        // if Student has Extra courses not yet passed
                        if (studentSessionEx.Count() > 0)
                        {

                            int sPassAll = 0;
                            int sCTU = 0;
                            int sCTP = 0;
                            Decimal sTotalGrading = 0;

                            int semesterCTU = 0;
                            int semesterCTP = 0;
                            Decimal semesterTotalGrading = 0;

                            string sCarryOvers = "";

                            string sCourses = "";

                            Int32 scores;

                            //StringBuilder courseValues = new StringBuilder(); // To store the courses
                            foreach (int courseExtra in studentSessionEx)
                            {
                                // Append each course to the StringBuilder, separated by commas
                                // courseValues.Append(course + ", ");
                                string loadCourse = "SELECT * FROM [Course] where Id = '" + courseExtra + "'";
                                SqlCommand cmdCourse = new SqlCommand(loadCourse, Con);
                                SqlDataReader readerCourse = cmdCourse.ExecuteReader();

                                if (readerCourse.Read())
                                {
                                    string aCourse = "";

                        
                                    aCourse = readerCourse.GetString("Code") + " (" + readerCourse.GetString("Unit") + ") A.R";
                                    if (sCarryOvers == "")
                                    {
                                        sCarryOvers = aCourse;
                                    }
                                    else
                                    {
                                        sCarryOvers = sCarryOvers + "     " + aCourse;
                                    }

                                    sPassAll = sPassAll + 1;
                                }
                            }

                            // Get all the result of exams each student have ever written
                            string querySresult = "SELECT * FROM [Result] where Student = '" + studentId + "'";
                            SqlCommand cmdSresult = new SqlCommand(querySresult, Con);
                            SqlDataReader readerSresult = cmdSresult.ExecuteReader();

                            while (readerSresult.Read())
                            {
                                // Get the session ID of the session of the result of exam we are working on.
                                int sSessionID = readerSresult.GetInt32("Session");
                                // Get the session start of the session of the result of exam we are working on.
                                int sSessionStart = GlobalVariable.Globals.getSessionStart(sSessionID, Con);
                                // Get the session name of the session of the result of exam we are working on.
                                // The global function is not working
                                // string sSessionName = GlobalVariable.Globals.getSessionName(sSessionID, Con);
                                string sSessionName = "";
                                string loadResultSession = $"SELECT * FROM [Session] where Id = '{sSessionID}'";
                                SqlCommand cmdResultSession = new SqlCommand(loadResultSession, Con);
                                SqlDataReader readerResultSession = cmdResultSession.ExecuteReader();
                                if (readerResultSession.Read())
                                {
                                    // Session of result currently being looped
                                    sSessionName = readerResultSession.GetString("Name");
                                }

                                string unit = "";

                                // The session of when the result was written must not be greater than the session we are currently querying
                                if (sessionStart >= sSessionStart)
                                {

                                    string loadCourse = "SELECT * FROM [Course] where Id = '" + readerSresult.GetInt32("Course") + "'";
                                    SqlCommand cmdCourse = new SqlCommand(loadCourse, Con);
                                    SqlDataReader readerCourse = cmdCourse.ExecuteReader();

                                    if (readerCourse.Read())
                                    {
                                        int courseId = readerCourse.GetInt32("Id");
                                        if (semester == "H") //it is first semester
                                        {
                                            //MessageBox.Show($"session query:{session} Result session:{sSessionName} {readerCourse.GetString("Semester")} R {studentSessionEx.Count()}");
                                            if ((session == sSessionName) && (readerCourse.GetString("Semester") == "R"))
                                            {
                                                //don't fetch second semester for current session
                                            }
                                            else
                                            {
                                                unit = readerCourse.GetString("Unit");
                                                scores = readerSresult.GetInt32("Score");

                                                // This section is to calculate and collate the result current semester we are dealing with
                                                if ((sessionId == readerSresult.GetInt32("Session")) && (semester == readerCourse.GetString("Semester")))
                                                {
                                                    // variable of printed details of each course
                                                    string aCourse = "";

                                                    // If scores is greater than or equal to zero it means student was scored for the exam
                                                    if (scores >= 0)
                                                    {
                                                        // write out the printed details into the variable
                                                        aCourse = readerCourse.GetString("Code") + " (" + readerCourse.GetString("Unit") + ")" + scores + Grading(scores);
                                                        semesterCTU = Int32.Parse(unit) + semesterCTU;
                                                        if (gradeType == "Percentage Grading")
                                                        {
                                                            semesterCTP = (Int32.Parse(unit) * scores) + semesterCTP;
                                                        }
                                                        else
                                                        {
                                                            semesterCTP = (Int32.Parse(unit) * GradePoint(scores)) + semesterCTP;
                                                        }

                                                        if (sCourses == "")
                                                        {
                                                            sCourses = aCourse;
                                                        }
                                                        else
                                                        {
                                                            sCourses = sCourses + "     " + aCourse;
                                                        }

                                                    }

                                                    if ((scores < passMark) && (scores >= 0))
                                                    {
                                                        //aCourse = readerCourse.GetString("Code") + " (" + readerCourse.GetString("Unit") + ")" + scores + Grading(scores);
                                                        //if (sCourses == "")
                                                        //{
                                                            //sCourses = aCourse;
                                                        //}
                                                        //else
                                                        //{
                                                            //sCourses = sCourses + "     " + aCourse;
                                                        //}

                                                    }

                                                    if (scores < 0)
                                                    {
                                                        aCourse = readerCourse.GetString("Code") + " (" + readerCourse.GetString("Unit") + ") A.R";
                                                        if (sCourses == "")
                                                        {
                                                            sCourses = aCourse;
                                                        }
                                                        else
                                                        {
                                                            sCourses = sCourses + "     " + aCourse;
                                                        }

                                                    }

                                                }
                                                
                                                // This section is to calculate and collate all the result of student up until that semester
                                                if (scores >= 0)
                                                {
                                                    sCTU = Int32.Parse(unit) + sCTU;
                                                    if (gradeType == "Percentage Grading")
                                                    {
                                                        sCTP = (Int32.Parse(unit) * scores) + sCTP;
                                                    }
                                                    else
                                                    {
                                                        sCTP = (Int32.Parse(unit) * GradePoint(scores)) + sCTP;
                                                    }
                                                }
                                                if ((scores < passMark) && (scores >= 0))
                                                {
                                                    int exists = 0;
                                                    string checkDataQ = "SELECT Session FROM [Result] WHERE student = @Student AND course = @Course AND score > 39";

                                                    using (SqlCommand cmdcheckDataQ = new SqlCommand(checkDataQ, Con))
                                                    {
                                                        cmdcheckDataQ.Parameters.AddWithValue("@Student", studentId);
                                                        cmdcheckDataQ.Parameters.AddWithValue("@Course", readerSresult.GetInt32("Course"));
                                                        SqlDataReader readercheckDataQ = cmdcheckDataQ.ExecuteReader();
                                                        while (readercheckDataQ.Read())
                                                        {
                                                            int dataQ = readercheckDataQ.GetInt32("Session");
                                                            if (sessionStart >= GlobalVariable.Globals.getSessionStart(dataQ, Con))
                                                            {
                                                                exists++;
                                                            }
                                                        }

                                                        //exists = (int)cmdcheckDataQ.ExecuteScalar() > 0;
                                                    }
                                                    if (exists > 0)
                                                    {

                                                    }
                                                    else
                                                    {
                                                        string loadData = "SELECT * FROM [Course] where Id = '" + readerSresult.GetInt32("Course") + "'";
                                                        SqlCommand cmdData = new SqlCommand(loadData, Con);
                                                        SqlDataReader readerData = cmdData.ExecuteReader();
                                                        if (readerData.Read())
                                                        {
                                                            string newEntry = readerData.GetString("Code") + " (" + readerData.GetString("Unit") + ") REP";

                                                            string newEntry2 = readerData.GetString("Code") + " (" + readerData.GetString("Unit") + ") A.R";
                                                            
                                                            //readerCourse.GetString("Code") + " (" + readerCourse.GetString("Unit") + ") A.R";

                                                            // Check if newEntry2 exists in label52.Text
                                                            if (sCarryOvers.Contains(newEntry2))
                                                            {
                                                                // Replace newEntry2 with newEntry
                                                                sCarryOvers = sCarryOvers.Replace(newEntry2, newEntry);
                                                            }
                                                            else
                                                            {
                                                                // If newEntry2 doesn't exist, add newEntry
                                                                if (!sCarryOvers.Contains(newEntry))
                                                                {
                                                                    sPassAll = sPassAll + 1;

                                                                    sCarryOvers = sCarryOvers + "     " + newEntry;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }

                                                if (scores < 0)
                                                {
                                                    int exists = 0;

                                                    string checkDataQ = "SELECT Session FROM [Result] WHERE student = @Student AND course = @Course AND score > 39";

                                                    using (SqlCommand cmdcheckDataQ = new SqlCommand(checkDataQ, Con))
                                                    {
                                                        cmdcheckDataQ.Parameters.AddWithValue("@Student", studentId);
                                                        cmdcheckDataQ.Parameters.AddWithValue("@Course", readerSresult.GetInt32("Course"));
                                                        SqlDataReader readercheckDataQ = cmdcheckDataQ.ExecuteReader();
                                                        while (readercheckDataQ.Read())
                                                        {
                                                            int dataQ = readercheckDataQ.GetInt32("Session");
                                                            if (sessionStart >= GlobalVariable.Globals.getSessionStart(dataQ, Con))
                                                            {
                                                                exists++;
                                                            }
                                                        }
                                                    }

                                                    if (exists > 0)
                                                    {

                                                    }
                                                    else
                                                    {
                                                        string loadData2 = "SELECT * FROM [Course] where Id = '" + readerSresult.GetInt32("Course") + "'";
                                                        SqlCommand cmdData2 = new SqlCommand(loadData2, Con);
                                                        SqlDataReader readerData2 = cmdData2.ExecuteReader();
                                                        if (readerData2.Read())
                                                        {
                                                            string newEntry = readerData2.GetString("Code") + " (" + readerData2.GetString("Unit") + ") A.R";

                                                            string newEntry2 = readerData2.GetString("Code") + " (" + readerData2.GetString("Unit") + ") REP";

                                                            // Check if newEntry2 exists in label52.Text
                                                            if (sCarryOvers.Contains(newEntry2))
                                                            {
                                                                // Replace newEntry2 with newEntry
                                                                sCarryOvers = sCarryOvers.Replace(newEntry2, newEntry);
                                                            }
                                                            else
                                                            {
                                                                // If newEntry2 doesn't exist, add newEntry
                                                                if (!sCarryOvers.Contains(newEntry))
                                                                {
                                                                    sPassAll = sPassAll + 1;

                                                                    sCarryOvers = sCarryOvers + "     " + newEntry;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else //it is second semester fetch all result
                                        {
                                            unit = readerCourse.GetString("Unit");
                                            scores = readerSresult.GetInt32("Score");

                                            if ((sessionId == readerSresult.GetInt32("Session")) && (semester == readerCourse.GetString("Semester")))
                                            {
                                                string aCourse = "";

                                                if (scores >= 0)
                                                {
                                                    aCourse = readerCourse.GetString("Code") + " (" + readerCourse.GetString("Unit") + ")" + scores + Grading(scores);
                                                    semesterCTU = Int32.Parse(unit) + semesterCTU;
                                                    if (gradeType == "Percentage Grading")
                                                    {
                                                        semesterCTP = (Int32.Parse(unit) * scores) + semesterCTP;
                                                    }
                                                    else
                                                    {
                                                        semesterCTP = (Int32.Parse(unit) * GradePoint(scores)) + semesterCTP;
                                                    }

                                                    if (sCourses == "")
                                                    {
                                                        sCourses = aCourse;
                                                    }
                                                    else
                                                    {
                                                        sCourses = sCourses + "     " + aCourse;
                                                    }

                                                }
                                              
                                                if (scores < 0)
                                                {
                                                    aCourse = readerCourse.GetString("Code") + " (" + readerCourse.GetString("Unit") + ") A.R";
                                                    if (sCourses == "")
                                                    {
                                                        sCourses = aCourse;
                                                    }
                                                    else
                                                    {
                                                        sCourses = sCourses + "     " + aCourse;
                                                    }

                                                }

                                            }
                                            if (scores >= 0)
                                            {
                                                sCTU = Int32.Parse(unit) + sCTU;
                                                if (gradeType == "Percentage Grading")
                                                {
                                                    sCTP = (Int32.Parse(unit) * scores) + sCTP;
                                                }
                                                else
                                                {
                                                    sCTP = (Int32.Parse(unit) * GradePoint(scores)) + sCTP;
                                                }
                                            }

                                            if ((scores < passMark) && (scores >= 0))
                                            {
                                                int exists = 0;

                                                string checkDataQ = "SELECT Session FROM [Result] WHERE student = @Student AND course = @Course AND score > 39";

                                                using (SqlCommand cmdcheckDataQ = new SqlCommand(checkDataQ, Con))
                                                {
                                                    cmdcheckDataQ.Parameters.AddWithValue("@Student", studentId);
                                                    cmdcheckDataQ.Parameters.AddWithValue("@Course", readerSresult.GetInt32("Course"));
                                                    SqlDataReader readercheckDataQ = cmdcheckDataQ.ExecuteReader();
                                                    while (readercheckDataQ.Read())
                                                    {
                                                        int dataQ = readercheckDataQ.GetInt32("Session");
                                                        if (sessionStart >= GlobalVariable.Globals.getSessionStart(dataQ, Con))
                                                        {
                                                            exists++;
                                                        }
                                                    }
                                                }

                                                if (exists > 0)
                                                {

                                                }
                                                else
                                                {
                                                    string loadData = "SELECT * FROM [Course] where Id = '" + readerSresult.GetInt32("Course") + "'";
                                                    SqlCommand cmdData = new SqlCommand(loadData, Con);
                                                    SqlDataReader readerData = cmdData.ExecuteReader();
                                                    if (readerData.Read())
                                                    {
                                                        string newEntry = readerData.GetString("Code") + " (" + readerData.GetString("Unit") + ") REP";

                                                        string newEntry2 = readerData.GetString("Code") + " (" + readerData.GetString("Unit") + ") A.R";

                                                        // Check if newEntry2 exists in label52.Text
                                                        if (sCarryOvers.Contains(newEntry2))
                                                        {
                                                            // Replace newEntry2 with newEntry
                                                            sCarryOvers = sCarryOvers.Replace(newEntry2, newEntry);
                                                        }
                                                        else
                                                        {
                                                            // If newEntry2 doesn't exist, add newEntry
                                                            if (!sCarryOvers.Contains(newEntry))
                                                            {
                                                                sPassAll = sPassAll + 1;

                                                                sCarryOvers = sCarryOvers + "     " + newEntry;
                                                            }
                                                        }
                                                    }
                                                }
                                            }

                                            if (scores < 0)
                                            {
                                                int exists = 0;

                                                string checkDataQ = "SELECT Session FROM [Result] WHERE student = @Student AND course = @Course AND score > 39";

                                                using (SqlCommand cmdcheckDataQ = new SqlCommand(checkDataQ, Con))
                                                {
                                                    cmdcheckDataQ.Parameters.AddWithValue("@Student", studentId);
                                                    cmdcheckDataQ.Parameters.AddWithValue("@Course", readerSresult.GetInt32("Course"));
                                                    SqlDataReader readercheckDataQ = cmdcheckDataQ.ExecuteReader();
                                                    while (readercheckDataQ.Read())
                                                    {
                                                        int dataQ = readercheckDataQ.GetInt32("Session");
                                                        if (sessionStart >= GlobalVariable.Globals.getSessionStart(dataQ, Con))
                                                        {
                                                            exists++;
                                                        }
                                                    }
                                                }

                                                if (exists > 0)
                                                {

                                                }
                                                else
                                                {
                                                    string loadData2 = "SELECT * FROM [Course] where Id = '" + readerSresult.GetInt32("Course") + "'";
                                                    SqlCommand cmdData2 = new SqlCommand(loadData2, Con);
                                                    SqlDataReader readerData2 = cmdData2.ExecuteReader();
                                                    if (readerData2.Read())
                                                    {
                                                        string newEntry = readerData2.GetString("Code") + " (" + readerData2.GetString("Unit") + ") A.R";

                                                        string newEntry2 = readerData2.GetString("Code") + " (" + readerData2.GetString("Unit") + ") REP";

                                                        // Check if newEntry2 exists in label52.Text
                                                        if (sCarryOvers.Contains(newEntry2))
                                                        {
                                                            // Replace newEntry2 with newEntry
                                                            sCarryOvers = sCarryOvers.Replace(newEntry2, newEntry);
                                                        }
                                                        else
                                                        {
                                                            // If newEntry2 doesn't exist, add newEntry
                                                            if (!sCarryOvers.Contains(newEntry))
                                                            {
                                                                sPassAll = sPassAll + 1;

                                                                sCarryOvers = sCarryOvers + "     " + newEntry;
                                                            }
                                                        }
                                                    }
                                                }
                                            }

                                        }
                                    }
                                }
                            }

                            if ((semesterCTU != 0) && (semesterCTP != 0))
                            {
                                Decimal stp1 = (Decimal)semesterCTP;
                                Decimal stu1 = (Decimal)semesterCTU;
                                semesterTotalGrading = Math.Round((Decimal)(stp1 / stu1), 2);
                            }

                            if ((sCTU != 0) && (sCTP != 0))
                            {
                                Decimal stp2 = (Decimal)sCTP;
                                Decimal stu2 = (Decimal)sCTU;
                                sTotalGrading = Math.Round((Decimal)(stp2 / stu2), 2);
                            }

                            //Former CGPA

                            Int32 prevTP = sCTP - semesterCTP;
                            Int32 prevTU = sCTU - semesterCTU;


                            Decimal prevTotalGrading = 0;

                            if ((prevTU != 0) && (prevTP != 0))
                            {
                                Decimal stp3 = (Decimal)prevTP;
                                Decimal stu3 = (Decimal)prevTU;
                                prevTotalGrading = Math.Round((Decimal)(stp3 / stu3), 2);
                            }

                            string matric = readerSession.GetString("Matric");
                            string name = readerSession.GetString("Lastname") + ", " + readerSession.GetString("Firstname") + " " + readerSession.GetString("Middlename");
                            string sex = readerSession.GetString("Gender");

                            if (gradeType == "Percentage Grading")
                            {
                                if ((sPassAll == 0) && (sTotalGrading >= passMark))
                                {

                                    dataGridView1.Rows.Add(sn, matric, sCourses, "", prevTP, prevTU, DecimalPlaces(prevTotalGrading) + suffix, semesterCTP, semesterCTU, DecimalPlaces(semesterTotalGrading) + suffix, sCTP, sCTU, DecimalPlaces(sTotalGrading) + suffix, "PASS");
                                    sn++;
                                }

                                if ((sPassAll > 0) && (sTotalGrading >= passMark))
                                {

                                    dataGridView2.Rows.Add(sn2, matric, sCourses, sCarryOvers, prevTP, prevTU, DecimalPlaces(prevTotalGrading) + suffix, semesterCTP, semesterCTU, DecimalPlaces(semesterTotalGrading) + suffix, sCTP, sCTU, DecimalPlaces(sTotalGrading) + suffix, "CSO");
                                    sn2++;
                                }

                                if (sTotalGrading < passMark)
                                {
                                    dataGridView3.Rows.Add(sn3, matric, sCourses, sCarryOvers, prevTP, prevTU, DecimalPlaces(prevTotalGrading) + suffix, semesterCTP, semesterCTU, DecimalPlaces(semesterTotalGrading) + suffix, sCTP, sCTU, DecimalPlaces(sTotalGrading) + suffix, "PROBATION");
                                    sn3++;
                                }
                            }
                            else
                            {
                                if (sPassAll == 0)
                                {

                                    dataGridView1.Rows.Add(sn, matric, sCourses, "", prevTP, prevTU, DecimalPlaces(prevTotalGrading) + suffix, semesterCTP, semesterCTU, DecimalPlaces(semesterTotalGrading) + suffix, sCTP, sCTU, DecimalPlaces(sTotalGrading) + suffix, "PASS");
                                    sn++;
                                }

                                if ((sPassAll > 0) && (sTotalGrading >= 1))
                                {

                                    dataGridView2.Rows.Add(sn2, matric, sCourses, sCarryOvers, prevTP, prevTU, DecimalPlaces(prevTotalGrading) + suffix, semesterCTP, semesterCTU, DecimalPlaces(semesterTotalGrading) + suffix, sCTP, sCTU, DecimalPlaces(sTotalGrading) + suffix, "CSO");
                                    sn2++;
                                }

                                if (sTotalGrading < 1)
                                {
                                    dataGridView3.Rows.Add(sn3, matric, sCourses, sCarryOvers, prevTP, prevTU, DecimalPlaces(prevTotalGrading) + suffix, semesterCTP, semesterCTU, DecimalPlaces(semesterTotalGrading) + suffix, sCTP, sCTU, DecimalPlaces(sTotalGrading) + suffix, "PROBATION");
                                    sn3++;
                                }
                            }
                        }
                        else
                        {
                            // Student have passed all exams, check the last year he wrote an exam.
                            // If the last year he wrote an exam is greater or equal to the year we want to query, the student falls in the category
                            int studentLastExamYear = GlobalVariable.Globals.GetLastExamYear(readerSession.GetInt32("Id"), Con);
                            if (studentLastExamYear >= sessionStart)
                            {
                                //MessageBox.Show("Exam Last: " + studentLastExamYear.ToString() + "\n Year petition: " + levelSessionStart);

                                int sPassAll = 0;
                                int sCTU = 0;
                                int sCTP = 0;
                                Decimal sTotalGrading = 0;

                                int semesterCTU = 0;
                                int semesterCTP = 0;
                                Decimal semesterTotalGrading = 0;

                                string sCourses = "";

                                string carryOvers = "";

                                string sCarryOvers = "";

                                Int32 scores;

                                string querySresult = "SELECT * FROM [Result] where Student = '" + readerSession.GetInt32("Id") + "'";
                                SqlCommand cmdSresult = new SqlCommand(querySresult, Con);
                                SqlDataReader readerSresult = cmdSresult.ExecuteReader();

                                while (readerSresult.Read())
                                {
                                    int sSessionID = readerSresult.GetInt32("Session");
                                    int sSessionStart = GlobalVariable.Globals.getSessionStart(sSessionID, Con);
                                    // Get the session name of the session of the result of exam we are working on.
                                    // The global function is not working
                                    // string sSessionName = GlobalVariable.Globals.getSessionName(sSessionID, Con);
                                    string sSessionName = "";
                                    string loadResultSession = $"SELECT * FROM [Session] where Id = '{sSessionID}'";
                                    SqlCommand cmdResultSession = new SqlCommand(loadResultSession, Con);
                                    SqlDataReader readerResultSession = cmdResultSession.ExecuteReader();
                                    if (readerResultSession.Read())
                                    {
                                        // Session of result currently being looped
                                        sSessionName = readerResultSession.GetString("Name");
                                    }

                                    string unit = "";
                                    if (sessionStart >= sSessionStart)
                                    {
                                        string loadCourse = "SELECT * FROM [Course] where Id = '" + readerSresult.GetInt32("Course") + "'";
                                        SqlCommand cmdCourse = new SqlCommand(loadCourse, Con);
                                        SqlDataReader readerCourse = cmdCourse.ExecuteReader();

                                        if (readerCourse.Read())
                                        {
                                            int courseId = readerCourse.GetInt32("Id");
                                            if (semester == "H") //it is first semester
                                            {
                                                if ((session == sSessionName) && (readerCourse.GetString("Semester") == "R"))
                                                {
                                                    //don't fetch second semester for current session
                                                }
                                                else
                                                {
                                                    unit = readerCourse.GetString("Unit");
                                                    scores = readerSresult.GetInt32("Score");

                                                    if ((sessionId == readerSresult.GetInt32("Session")) && (semester == readerCourse.GetString("Semester")))
                                                    {
                                                        string aCourse = "";

                                                        if (scores >= 0)
                                                        {
                                                            aCourse = readerCourse.GetString("Code") + " (" + readerCourse.GetString("Unit") + ")" + scores + Grading(scores);
                                                            semesterCTU = Int32.Parse(unit) + semesterCTU;
                                                            if (gradeType == "Percentage Grading")
                                                            {
                                                                semesterCTP = (Int32.Parse(unit) * scores) + semesterCTP;
                                                            }
                                                            else
                                                            {
                                                                semesterCTP = (Int32.Parse(unit) * GradePoint(scores)) + semesterCTP;
                                                            }

                                                            if (sCourses == "")
                                                            {
                                                                sCourses = aCourse;
                                                            }
                                                            else
                                                            {
                                                                sCourses = sCourses + "     " + aCourse;
                                                            }

                                                        }
                                                        /*
                                                        if ((scores < passMark) && (scores >= 0))
                                                        {
                                                            aCourse = readerCourse.GetString("Code") + " (" + readerCourse.GetString("Unit") + ")" + scores + Grading(scores);
                                                            if (sCourses == "")
                                                            {
                                                                sCourses = aCourse;
                                                            }
                                                            else
                                                            {
                                                                sCourses = sCourses + "     " + aCourse;
                                                            }

                                                        }
                                                        */
                                                        if (scores < 0)
                                                        {
                                                            aCourse = readerCourse.GetString("Code") + " (" + readerCourse.GetString("Unit") + ") A.R";
                                                            if (sCourses == "")
                                                            {
                                                                sCourses = aCourse;
                                                            }
                                                            else
                                                            {
                                                                sCourses = sCourses + "     " + aCourse;
                                                            }

                                                        }

                                                    }
                                                    if (scores >= 0)
                                                    {
                                                        sCTU = Int32.Parse(unit) + sCTU;
                                                        if (gradeType == "Percentage Grading")
                                                        {
                                                            sCTP = (Int32.Parse(unit) * scores) + sCTP;
                                                        }
                                                        else
                                                        {
                                                            sCTP = (Int32.Parse(unit) * GradePoint(scores)) + sCTP;
                                                        }
                                                    }

                                                    if ((scores < passMark) && (scores >= 0))
                                                    {
                                                        int exists = 0;
                                                        string checkDataQ = "SELECT Session FROM [Result] WHERE student = @Student AND course = @Course AND score > 39";

                                                        using (SqlCommand cmdcheckDataQ = new SqlCommand(checkDataQ, Con))
                                                        {
                                                            cmdcheckDataQ.Parameters.AddWithValue("@Student", studentId);
                                                            cmdcheckDataQ.Parameters.AddWithValue("@Course", readerSresult.GetInt32("Course"));
                                                            SqlDataReader readercheckDataQ = cmdcheckDataQ.ExecuteReader();
                                                            while (readercheckDataQ.Read())
                                                            {
                                                                int dataQ = readercheckDataQ.GetInt32("Session");
                                                                if (sessionStart >= GlobalVariable.Globals.getSessionStart(dataQ, Con))
                                                                {
                                                                    exists++;
                                                                }
                                                            }
                                                        }
                                                        if (exists > 0)
                                                        {

                                                        }
                                                        else
                                                        {
                                                            string loadData = "SELECT * FROM [Course] where Id = '" + readerSresult.GetInt32("Course") + "'";
                                                            SqlCommand cmdData = new SqlCommand(loadData, Con);
                                                            SqlDataReader readerData = cmdData.ExecuteReader();
                                                            if (readerData.Read())
                                                            {
                                                                string newEntry = readerData.GetString("Code") + " (" + readerData.GetString("Unit") + ") REP";

                                                                string newEntry2 = readerData.GetString("Code") + " (" + readerData.GetString("Unit") + ") A.R";

                                                                // Check if newEntry2 exists in label52.Text
                                                                if (sCarryOvers.Contains(newEntry2))
                                                                {
                                                                    // Replace newEntry2 with newEntry
                                                                    sCarryOvers = sCarryOvers.Replace(newEntry2, newEntry);
                                                                }
                                                                else
                                                                {
                                                                    // If newEntry2 doesn't exist, add newEntry
                                                                    if (!sCarryOvers.Contains(newEntry))
                                                                    {
                                                                        sPassAll = sPassAll + 1;

                                                                        sCarryOvers = sCarryOvers + "     " + newEntry;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }

                                                    if (scores < 0)
                                                    {
                                                        int exists = 0;

                                                        string checkDataQ = "SELECT Session FROM [Result] WHERE student = @Student AND course = @Course AND score > 39";

                                                        using (SqlCommand cmdcheckDataQ = new SqlCommand(checkDataQ, Con))
                                                        {
                                                            cmdcheckDataQ.Parameters.AddWithValue("@Student", studentId);
                                                            cmdcheckDataQ.Parameters.AddWithValue("@Course", readerSresult.GetInt32("Course"));
                                                            SqlDataReader readercheckDataQ = cmdcheckDataQ.ExecuteReader();
                                                            while (readercheckDataQ.Read())
                                                            {
                                                                int dataQ = readercheckDataQ.GetInt32("Session");
                                                                if (sessionStart >= GlobalVariable.Globals.getSessionStart(dataQ, Con))
                                                                {
                                                                    exists++;
                                                                }
                                                            }
                                                        }

                                                        if (exists > 0)
                                                        {

                                                        }
                                                        else
                                                        {
                                                            string loadData2 = "SELECT * FROM [Course] where Id = '" + readerSresult.GetInt32("Course") + "'";
                                                            SqlCommand cmdData2 = new SqlCommand(loadData2, Con);
                                                            SqlDataReader readerData2 = cmdData2.ExecuteReader();
                                                            if (readerData2.Read())
                                                            {
                                                                string newEntry = readerData2.GetString("Code") + " (" + readerData2.GetString("Unit") + ") A.R";

                                                                string newEntry2 = readerData2.GetString("Code") + " (" + readerData2.GetString("Unit") + ") REP";

                                                                // Check if newEntry2 exists in label52.Text
                                                                if (sCarryOvers.Contains(newEntry2))
                                                                {
                                                                    // Replace newEntry2 with newEntry
                                                                    sCarryOvers = sCarryOvers.Replace(newEntry2, newEntry);
                                                                }
                                                                else
                                                                {
                                                                    // If newEntry2 doesn't exist, add newEntry
                                                                    if (!sCarryOvers.Contains(newEntry))
                                                                    {
                                                                        sPassAll = sPassAll + 1;

                                                                        sCarryOvers = sCarryOvers + "     " + newEntry;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }

                                                }
                                            }
                                            else //it is second semester fetch all result
                                            {
                                                unit = readerCourse.GetString("Unit");
                                                scores = readerSresult.GetInt32("Score");

                                                if ((sessionId == readerSresult.GetInt32("Session")) && (semester == readerCourse.GetString("Semester")))
                                                {
                                                    string aCourse = "";

                                                    if (scores >= 0)
                                                    {
                                                        aCourse = readerCourse.GetString("Code") + " (" + readerCourse.GetString("Unit") + ")" + scores + Grading(scores);
                                                        semesterCTU = Int32.Parse(unit) + semesterCTU;
                                                        if (gradeType == "Percentage Grading")
                                                        {
                                                            semesterCTP = (Int32.Parse(unit) * scores) + semesterCTP;
                                                        }
                                                        else
                                                        {
                                                            semesterCTP = (Int32.Parse(unit) * GradePoint(scores)) + semesterCTP;
                                                        }

                                                        if (sCourses == "")
                                                        {
                                                            sCourses = aCourse;
                                                        }
                                                        else
                                                        {
                                                            sCourses = sCourses + "     " + aCourse;
                                                        }

                                                    }
                                                    if (scores < 0)
                                                    {
                                                        aCourse = readerCourse.GetString("Code") + " (" + readerCourse.GetString("Unit") + ") A.R";
                                                        if (sCourses == "")
                                                        {
                                                            sCourses = aCourse;
                                                        }
                                                        else
                                                        {
                                                            sCourses = sCourses + "     " + aCourse;
                                                        }

                                                    }

                                                }

                                                if (scores >= 0)
                                                {
                                                    sCTU = Int32.Parse(unit) + sCTU;
                                                    if (gradeType == "Percentage Grading")
                                                    {
                                                        sCTP = (Int32.Parse(unit) * scores) + sCTP;
                                                    }
                                                    else
                                                    {
                                                        sCTP = (Int32.Parse(unit) * GradePoint(scores)) + sCTP;
                                                    }
                                                }

                                                if ((scores < passMark) && (scores >= 0))
                                                {
                                                    int exists = 0;

                                                    string checkDataQ = "SELECT Session FROM [Result] WHERE student = @Student AND course = @Course AND score > 39";

                                                    using (SqlCommand cmdcheckDataQ = new SqlCommand(checkDataQ, Con))
                                                    {
                                                        cmdcheckDataQ.Parameters.AddWithValue("@Student", studentId);
                                                        cmdcheckDataQ.Parameters.AddWithValue("@Course", readerSresult.GetInt32("Course"));
                                                        SqlDataReader readercheckDataQ = cmdcheckDataQ.ExecuteReader();
                                                        while (readercheckDataQ.Read())
                                                        {
                                                            int dataQ = readercheckDataQ.GetInt32("Session");
                                                            if (sessionStart >= GlobalVariable.Globals.getSessionStart(dataQ, Con))
                                                            {
                                                                exists++;
                                                            }
                                                        }
                                                    }

                                                    if (exists > 0)
                                                    {

                                                    }
                                                    else
                                                    {
                                                        string loadData = "SELECT * FROM [Course] where Id = '" + readerSresult.GetInt32("Course") + "'";
                                                        SqlCommand cmdData = new SqlCommand(loadData, Con);
                                                        SqlDataReader readerData = cmdData.ExecuteReader();
                                                        if (readerData.Read())
                                                        {
                                                            string newEntry = readerData.GetString("Code") + " (" + readerData.GetString("Unit") + ") REP";

                                                            string newEntry2 = readerData.GetString("Code") + " (" + readerData.GetString("Unit") + ") A.R";

                                                            // Check if newEntry2 exists in label52.Text
                                                            if (sCarryOvers.Contains(newEntry2))
                                                            {
                                                                // Replace newEntry2 with newEntry
                                                                sCarryOvers = sCarryOvers.Replace(newEntry2, newEntry);
                                                            }
                                                            else
                                                            {
                                                                // If newEntry2 doesn't exist, add newEntry
                                                                if (!sCarryOvers.Contains(newEntry))
                                                                {
                                                                    sPassAll = sPassAll + 1;

                                                                    sCarryOvers = sCarryOvers + "     " + newEntry;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }

                                                if (scores < 0)
                                                {
                                                    int exists = 0;

                                                    string checkDataQ = "SELECT Session FROM [Result] WHERE student = @Student AND course = @Course AND score > 39";

                                                    using (SqlCommand cmdcheckDataQ = new SqlCommand(checkDataQ, Con))
                                                    {
                                                        cmdcheckDataQ.Parameters.AddWithValue("@Student", studentId);
                                                        cmdcheckDataQ.Parameters.AddWithValue("@Course", readerSresult.GetInt32("Course"));
                                                        SqlDataReader readercheckDataQ = cmdcheckDataQ.ExecuteReader();
                                                        while (readercheckDataQ.Read())
                                                        {
                                                            int dataQ = readercheckDataQ.GetInt32("Session");
                                                            if (sessionStart >= GlobalVariable.Globals.getSessionStart(dataQ, Con))
                                                            {
                                                                exists++;
                                                            }
                                                        }
                                                    }

                                                    if (exists > 0)
                                                    {

                                                    }
                                                    else
                                                    {
                                                        string loadData2 = "SELECT * FROM [Course] where Id = '" + readerSresult.GetInt32("Course") + "'";
                                                        SqlCommand cmdData2 = new SqlCommand(loadData2, Con);
                                                        SqlDataReader readerData2 = cmdData2.ExecuteReader();
                                                        if (readerData2.Read())
                                                        {
                                                            string newEntry = readerData2.GetString("Code") + " (" + readerData2.GetString("Unit") + ") A.R";

                                                            string newEntry2 = readerData2.GetString("Code") + " (" + readerData2.GetString("Unit") + ") REP";

                                                            // Check if newEntry2 exists in label52.Text
                                                            if (sCarryOvers.Contains(newEntry2))
                                                            {
                                                                // Replace newEntry2 with newEntry
                                                                sCarryOvers = sCarryOvers.Replace(newEntry2, newEntry);
                                                            }
                                                            else
                                                            {
                                                                // If newEntry2 doesn't exist, add newEntry
                                                                if (!sCarryOvers.Contains(newEntry))
                                                                {
                                                                    sPassAll = sPassAll + 1;

                                                                    sCarryOvers = sCarryOvers + "     " + newEntry;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }

                                            }
                                        }
                                    }
                                }

                                if ((semesterCTU != 0) && (semesterCTP != 0))
                                {
                                    Decimal stp1 = (Decimal)semesterCTP;
                                    Decimal stu1 = (Decimal)semesterCTU;
                                    semesterTotalGrading = Math.Round((Decimal)(stp1 / stu1), 2);
                                }

                                if ((sCTU != 0) && (sCTP != 0))
                                {
                                    Decimal stp2 = (Decimal)sCTP;
                                    Decimal stu2 = (Decimal)sCTU;
                                    sTotalGrading = Math.Round((Decimal)(stp2 / stu2), 2);
                                }

                                //Former CGPA

                                Int32 prevTP = sCTP - semesterCTP;
                                Int32 prevTU = sCTU - semesterCTU;


                                Decimal prevTotalGrading = 0;

                                if ((prevTU != 0) && (prevTP != 0))
                                {
                                    Decimal stp3 = (Decimal)prevTP;
                                    Decimal stu3 = (Decimal)prevTU;
                                    prevTotalGrading = Math.Round((Decimal)(stp3 / stu3), 2);
                                }

                                string matric = readerSession.GetString("Matric");
                                string name = readerSession.GetString("Lastname") + ", " + readerSession.GetString("Firstname") + " " + readerSession.GetString("Middlename");
                                string sex = readerSession.GetString("Gender");

                                if (gradeType == "Percentage Grading")
                                {
                                    if ((sPassAll == 0) && (sTotalGrading >= passMark))
                                    {

                                        dataGridView1.Rows.Add(sn, matric, sCourses, "", prevTP, prevTU, DecimalPlaces(prevTotalGrading) + suffix, semesterCTP, semesterCTU, DecimalPlaces(semesterTotalGrading) + suffix, sCTP, sCTU, DecimalPlaces(sTotalGrading) + suffix, "PASS");
                                        sn++;
                                    }

                                    if ((sPassAll > 0) && (sTotalGrading >= passMark))
                                    {

                                        dataGridView2.Rows.Add(sn2, matric, sCourses, sCarryOvers, prevTP, prevTU, DecimalPlaces(prevTotalGrading) + suffix, semesterCTP, semesterCTU, DecimalPlaces(semesterTotalGrading) + suffix, sCTP, sCTU, DecimalPlaces(sTotalGrading) + suffix, "CSO");
                                        sn2++;
                                    }

                                    if (sTotalGrading < passMark)
                                    {
                                        dataGridView3.Rows.Add(sn3, matric, sCourses, sCarryOvers, prevTP, prevTU, DecimalPlaces(prevTotalGrading) + suffix, semesterCTP, semesterCTU, DecimalPlaces(semesterTotalGrading) + suffix, sCTP, sCTU, DecimalPlaces(sTotalGrading) + suffix, "PROBATION");
                                        sn3++;
                                    }
                                }
                                else
                                {
                                    if (sPassAll == 0)
                                    {

                                        dataGridView1.Rows.Add(sn, matric, sCourses, "", prevTP, prevTU, DecimalPlaces(prevTotalGrading) + suffix, semesterCTP, semesterCTU, DecimalPlaces(semesterTotalGrading) + suffix, sCTP, sCTU, DecimalPlaces(sTotalGrading) + suffix, "PASS");
                                        sn++;
                                    }

                                    if ((sPassAll > 0) && (sTotalGrading >= 1))
                                    {

                                        dataGridView2.Rows.Add(sn2, matric, sCourses, sCarryOvers, prevTP, prevTU, DecimalPlaces(prevTotalGrading) + suffix, semesterCTP, semesterCTU, DecimalPlaces(semesterTotalGrading) + suffix, sCTP, sCTU, DecimalPlaces(sTotalGrading) + suffix, "CSO");
                                        sn2++;
                                    }

                                    if (sTotalGrading < 1)
                                    {
                                        dataGridView3.Rows.Add(sn3, matric, sCourses, sCarryOvers, prevTP, prevTU, DecimalPlaces(prevTotalGrading) + suffix, semesterCTP, semesterCTU, DecimalPlaces(semesterTotalGrading) + suffix, sCTP, sCTU, DecimalPlaces(sTotalGrading) + suffix, "PROBATION");
                                        sn3++;
                                    }
                                }

                                
                            }
                        }
                    }
                }
            }
            else
            {
                // Here next
                string SessionQuery = "SELECT * FROM [Student] where Program = '" + programId + "' and Year ='" + levelSessionId + "' ORDER BY Matric";
                SqlCommand cmdSess = new SqlCommand(SessionQuery, Con);
                SqlDataReader readerSession = cmdSess.ExecuteReader();
                Int32 sn = 1;
                Int32 sn2 = 1;
                Int32 sn3 = 1;
                while (readerSession.Read())
                {
                    Int32 sPassAll = 0;
                    Int32 sCTU = 0;
                    Int32 sCTP = 0;
                    Decimal sTotalGrading = 0;

                    Int32 semesterCTU = 0;
                    Int32 semesterCTP = 0;
                    Decimal semesterTotalGrading = 0;

                    string sCarryOvers = "";

                    string sCourses = "";



                    // string carryOvers = "";

                    // Check if Student has Extra courses not yet passed
                    List<int> studentSessionEx = new List<int>();
                    //studentSessionEx = GlobalVariable.Globals.CheckStudentExtra(readerSession.GetInt32("Id").ToString(), Con);
                    studentSessionEx = GlobalVariable.Globals.StudentSessionExtra(readerSession.GetInt32("Id").ToString(), sessionId, Con);

                    foreach (int courseExtra in studentSessionEx)
                    {
                        // Append each course to the StringBuilder, separated by commas
                        // courseValues.Append(course + ", ");
                        string loadCourse = "SELECT * FROM [Course] where Id = '" + courseExtra + "'";
                        SqlCommand cmdCourse = new SqlCommand(loadCourse, Con);
                        SqlDataReader readerCourse = cmdCourse.ExecuteReader();

                        if (readerCourse.Read())
                        {
                            string aCourse = "";


                            aCourse = readerCourse.GetString("Code") + " (" + readerCourse.GetString("Unit") + ") A.R";
                            if (sCarryOvers == "")
                            {
                                sCarryOvers = aCourse;
                            }
                            else
                            {
                                sCarryOvers = sCarryOvers + "     " + aCourse;
                            }

                            sPassAll = sPassAll + 1;
                        }
                    }


                    Int32 scores;
                    int studentId = readerSession.GetInt32("Id");

                    // Loop through all student's result
                    string querySresult = "SELECT * FROM [Result] where Student = '" + studentId + "'";

                    SqlCommand cmdSresult = new SqlCommand(querySresult, Con);
                    SqlDataReader readerSresult = cmdSresult.ExecuteReader();

                    while (readerSresult.Read())
                    {
                        // Session of each looped result
                        Int32 sessSelectId = readerSresult.GetInt32("Session");

                        // Get the session start year of each looped result colomn
                        string querySsession = "SELECT * FROM [Session] where Id = '" + sessSelectId + "'";
                        SqlCommand cmdSsession = new SqlCommand(querySsession, Con);
                        SqlDataReader readerSsession = cmdSsession.ExecuteReader();

                        if (readerSsession.Read())
                        {
                            // Session start year of each looped result
                            Int32 sSessionStart = readerSsession.GetInt32("Start");
                            string sSessionName = readerSsession.GetString("Name");

                            // Check if each looped result is less than or equal to queried session. we are dealing with session that is not up to or equal to the session we want all result from.
                            if (sessionStart >= sSessionStart)
                            {
                                string unit = "";

                                // Get details of each course
                                string loadCourse = "SELECT * FROM [Course] where Id = '" + readerSresult.GetInt32("Course") + "'";
                                SqlCommand cmdCourse = new SqlCommand(loadCourse, Con);
                                SqlDataReader readerCourse = cmdCourse.ExecuteReader();
                                
                                if (readerCourse.Read())
                                {
                                    if (semester == "H") //it is first semester
                                    {
                                        if ((session == sSessionName) && (readerCourse.GetString("Semester") == "R"))
                                        {
                                            //don't fetch second semester for current session
                                        }
                                        else
                                        {

                                            unit = readerCourse.GetString("Unit");
                                            scores = readerSresult.GetInt32("Score");

                                            if ((sessionId == readerSsession.GetInt32("Id")) && (semester == readerCourse.GetString("Semester")))
                                            {
                                                string aCourse = "";
                                                //MessageBox.Show(readerCourse.GetString("Code"));
                                                if (scores >= 0)
                                                {
                                                    aCourse = readerCourse.GetString("Code") + " (" + readerCourse.GetString("Unit") + ")" + scores + Grading(scores);
                                                    semesterCTU = Int32.Parse(unit) + semesterCTU;
                                                    if (gradeType == "Percentage Grading")
                                                    {
                                                        semesterCTP = (Int32.Parse(unit) * scores) + semesterCTP;
                                                    }
                                                    else
                                                    {
                                                        semesterCTP = (Int32.Parse(unit) * GradePoint(scores)) + semesterCTP;
                                                    }
                                                    if (sCourses == "")
                                                    {
                                                        sCourses = aCourse;
                                                    }
                                                    else
                                                    {
                                                        sCourses = sCourses + "     " + aCourse;
                                                    }

                                                }
                                                /*
                                                if ((scores < passMark) && (scores >= 0))
                                                {
                                                    aCourse = readerCourse.GetString("Code") + " (" + readerCourse.GetString("Unit") + ")" + scores + Grading(scores);
                                                    if (sCourses == "")
                                                    {
                                                        sCourses = aCourse;
                                                    }
                                                    else
                                                    {
                                                        sCourses = sCourses + "     " + aCourse;
                                                    }

                                                }
                                                */
                                                if (scores < 0)
                                                {
                                                    aCourse = readerCourse.GetString("Code") + " (" + readerCourse.GetString("Unit") + ") A.R";
                                                    if (sCourses == "")
                                                    {
                                                        sCourses = aCourse;
                                                    }
                                                    else
                                                    {
                                                        sCourses = sCourses + "     " + aCourse;
                                                    }

                                                }
                                            }


                                            // This session is for all results up to the session and semester of written result
                                            if (scores >= 0)
                                            {
                                                sCTU = Int32.Parse(unit) + sCTU;
                                                if (gradeType == "Percentage Grading")
                                                {
                                                    sCTP = (Int32.Parse(unit) * scores) + sCTP;
                                                }
                                                else
                                                {
                                                    sCTP = (Int32.Parse(unit) * GradePoint(scores)) + sCTP;
                                                }
                                            }

                                            if ((scores < passMark) && (scores >= 0))
                                            {
                                                int exists = 0;

                                                string checkDataQ = "SELECT Session FROM [Result] WHERE student = @Student AND course = @Course AND score > 39";

                                                using (SqlCommand cmdcheckDataQ = new SqlCommand(checkDataQ, Con))
                                                {
                                                    cmdcheckDataQ.Parameters.AddWithValue("@Student", studentId);
                                                    cmdcheckDataQ.Parameters.AddWithValue("@Course", readerSresult.GetInt32("Course"));
                                                    SqlDataReader readercheckDataQ = cmdcheckDataQ.ExecuteReader();
                                                    while (readercheckDataQ.Read())
                                                    {
                                                        int dataQ = readercheckDataQ.GetInt32("Session");
                                                        if (sessionStart >= GlobalVariable.Globals.getSessionStart(dataQ, Con))
                                                        {
                                                            exists++;
                                                        }
                                                    }
                                                }

                                                if (exists > 0)
                                                {

                                                }
                                                else
                                                {
                                                    string loadData = "SELECT * FROM [Course] where Id = '" + readerSresult.GetInt32("Course") + "'";
                                                    SqlCommand cmdData = new SqlCommand(loadData, Con);
                                                    SqlDataReader readerData = cmdData.ExecuteReader();
                                                    if (readerData.Read())
                                                    {
                                                        string newEntry = readerData.GetString("Code") + " (" + readerData.GetString("Unit") + ") REP";

                                                        string newEntry2 = readerData.GetString("Code") + " (" + readerData.GetString("Unit") + ") A.R";

                                                        // Check if newEntry2 exists in label52.Text
                                                        if (sCarryOvers.Contains(newEntry2))
                                                        {
                                                            // Replace newEntry2 with newEntry
                                                            sCarryOvers = sCarryOvers.Replace(newEntry2, newEntry);
                                                        }
                                                        else
                                                        {
                                                            // If newEntry2 doesn't exist, add newEntry
                                                            if (!sCarryOvers.Contains(newEntry))
                                                            {
                                                                sPassAll = sPassAll + 1;

                                                                sCarryOvers = sCarryOvers + "     " + newEntry;
                                                            }
                                                        }
                                                    }
                                                }
                                            }

                                            if (scores < 0)
                                            {
                                                int exists = 0;

                                                string checkDataQ = "SELECT Session FROM [Result] WHERE student = @Student AND course = @Course AND score > 39";

                                                using (SqlCommand cmdcheckDataQ = new SqlCommand(checkDataQ, Con))
                                                {
                                                    cmdcheckDataQ.Parameters.AddWithValue("@Student", studentId);
                                                    cmdcheckDataQ.Parameters.AddWithValue("@Course", readerSresult.GetInt32("Course"));
                                                    SqlDataReader readercheckDataQ = cmdcheckDataQ.ExecuteReader();
                                                    while (readercheckDataQ.Read())
                                                    {
                                                        int dataQ = readercheckDataQ.GetInt32("Session");
                                                        if (sessionStart >= GlobalVariable.Globals.getSessionStart(dataQ, Con))
                                                        {
                                                            exists++;
                                                        }
                                                    }
                                                }

                                                if (exists > 0)
                                                {

                                                }
                                                else
                                                {
                                                    string loadData2 = "SELECT * FROM [Course] where Id = '" + readerSresult.GetInt32("Course") + "'";
                                                    SqlCommand cmdData2 = new SqlCommand(loadData2, Con);
                                                    SqlDataReader readerData2 = cmdData2.ExecuteReader();
                                                    if (readerData2.Read())
                                                    {
                                                        string newEntry = readerData2.GetString("Code") + " (" + readerData2.GetString("Unit") + ") A.R";

                                                        string newEntry2 = readerData2.GetString("Code") + " (" + readerData2.GetString("Unit") + ") REP";

                                                        // Check if newEntry2 exists in label52.Text
                                                        if (sCarryOvers.Contains(newEntry2))
                                                        {
                                                            // Replace newEntry2 with newEntry
                                                            sCarryOvers = sCarryOvers.Replace(newEntry2, newEntry);
                                                        }
                                                        else
                                                        {
                                                            // If newEntry2 doesn't exist, add newEntry
                                                            if (!sCarryOvers.Contains(newEntry))
                                                            {
                                                                sPassAll = sPassAll + 1;

                                                                sCarryOvers = sCarryOvers + "     " + newEntry;
                                                            }
                                                        }
                                                    }
                                                }
                                            }

                                        }
                                        
                                    }
                                    else //it is second semester fetch all result
                                    {
                                        unit = readerCourse.GetString("Unit");
                                        scores = readerSresult.GetInt32("Score");

                                        if ((sessionId == readerSresult.GetInt32("Session")) && (semester == readerCourse.GetString("Semester")))
                                        {
                                            string aCourse = "";
                                            if (scores >= 0)
                                            {
                                                aCourse = readerCourse.GetString("Code") + " (" + readerCourse.GetString("Unit") + ")" + scores + Grading(scores);
                                                semesterCTU = Int32.Parse(unit) + semesterCTU;
                                                if (gradeType == "Percentage Grading")
                                                {
                                                    semesterCTP = (Int32.Parse(unit) * scores) + semesterCTP;
                                                }
                                                else
                                                {
                                                    semesterCTP = (Int32.Parse(unit) * GradePoint(scores)) + semesterCTP;
                                                }
                                                if (sCourses == "")
                                                {
                                                    sCourses = aCourse;
                                                }
                                                else
                                                {
                                                    sCourses = sCourses + "     " + aCourse;
                                                }
                                            }

                                            if ((scores < passMark) && (scores >= 0))
                                            {
                                                /*
                                                aCourse = readerCourse.GetString("Code") + " (" + readerCourse.GetString("Unit") + ")" + scores + Grading(scores);
                                                if (sCourses == "")
                                                {
                                                    sCourses = aCourse;
                                                }
                                                else
                                                {
                                                    sCourses = sCourses + "     " + aCourse;
                                                }
                                                */
                                            }

                                            if (scores < 0)
                                            {
                                                aCourse = readerCourse.GetString("Code") + " (" + readerCourse.GetString("Unit") + ") A.R";
                                                if (sCourses == "")
                                                {
                                                    sCourses = aCourse;
                                                }
                                                else
                                                {
                                                    sCourses = sCourses + "     " + aCourse;
                                                }
                                            }
                                        }


                                        if (scores >= 0)
                                        {
                                            sCTU = Int32.Parse(unit) + sCTU;
                                            if (gradeType == "Percentage Grading")
                                            {
                                                sCTP = (Int32.Parse(unit) * scores) + sCTP;
                                            }
                                            else
                                            {
                                                sCTP = (Int32.Parse(unit) * GradePoint(scores)) + sCTP;
                                            }
                                        }

                                        if ((scores < passMark) && (scores >= 0))
                                        {
                                            /*
                                            aCourse = readerCourse.GetString("Code") + " (" + readerCourse.GetString("Unit") + ")REP";
                                            sPassAll = sPassAll + 1;
                                            if (sCarryOvers == "")
                                            {
                                                sCarryOvers = carryOvers;
                                            }
                                            else
                                            {
                                                sCarryOvers = sCarryOvers + "     " + carryOvers;
                                            }
                                            */
                                            int exists = 0;

                                            string checkDataQ = "SELECT Session FROM [Result] WHERE student = @Student AND course = @Course AND score > 39";

                                            using (SqlCommand cmdcheckDataQ = new SqlCommand(checkDataQ, Con))
                                            {
                                                cmdcheckDataQ.Parameters.AddWithValue("@Student", studentId);
                                                cmdcheckDataQ.Parameters.AddWithValue("@Course", readerSresult.GetInt32("Course"));
                                                SqlDataReader readercheckDataQ = cmdcheckDataQ.ExecuteReader();
                                                while (readercheckDataQ.Read())
                                                {
                                                    int dataQ = readercheckDataQ.GetInt32("Session");
                                                    if (sessionStart >= GlobalVariable.Globals.getSessionStart(dataQ, Con))
                                                    {
                                                        exists++;
                                                    }
                                                }
                                            }

                                            if (exists > 0)
                                            {

                                            }
                                            else
                                            {
                                                string loadData = "SELECT * FROM [Course] where Id = '" + readerSresult.GetInt32("Course") + "'";
                                                SqlCommand cmdData = new SqlCommand(loadData, Con);
                                                SqlDataReader readerData = cmdData.ExecuteReader();
                                                if (readerData.Read())
                                                {
                                                    string newEntry = readerData.GetString("Code") + " (" + readerData.GetString("Unit") + ") REP";

                                                    string newEntry2 = readerData.GetString("Code") + " (" + readerData.GetString("Unit") + ") A.R";

                                                    // Check if newEntry2 exists in label52.Text
                                                    if (sCarryOvers.Contains(newEntry2))
                                                    {
                                                        // Replace newEntry2 with newEntry
                                                        sCarryOvers = sCarryOvers.Replace(newEntry2, newEntry);
                                                    }
                                                    else
                                                    {
                                                        // If newEntry2 doesn't exist, add newEntry
                                                        if (!sCarryOvers.Contains(newEntry))
                                                        {
                                                            sPassAll = sPassAll + 1;

                                                            sCarryOvers = sCarryOvers + "     " + newEntry;
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        if (scores < 0)
                                        {
                                            /*
                                            carryOvers = readerCourse.GetString("Code") + " (" + readerCourse.GetString("Unit") + ") A.R";
                                            sPassAll = sPassAll + 1;
                                            if (sCarryOvers == "")
                                            {
                                                sCarryOvers = carryOvers;
                                            }
                                            else
                                            {
                                                sCarryOvers = sCarryOvers + "     " + carryOvers;
                                            }
                                            */
                                            int exists = 0;

                                            string checkDataQ = "SELECT Session FROM [Result] WHERE student = @Student AND course = @Course AND score > 39";

                                            using (SqlCommand cmdcheckDataQ = new SqlCommand(checkDataQ, Con))
                                            {
                                                cmdcheckDataQ.Parameters.AddWithValue("@Student", studentId);
                                                cmdcheckDataQ.Parameters.AddWithValue("@Course", readerSresult.GetInt32("Course"));
                                                SqlDataReader readercheckDataQ = cmdcheckDataQ.ExecuteReader();
                                                while (readercheckDataQ.Read())
                                                {
                                                    int dataQ = readercheckDataQ.GetInt32("Session");
                                                    if (sessionStart >= GlobalVariable.Globals.getSessionStart(dataQ, Con))
                                                    {
                                                        exists++;
                                                    }
                                                }
                                            }

                                            if (exists > 0)
                                            {

                                            }
                                            else
                                            {
                                                string loadData2 = "SELECT * FROM [Course] where Id = '" + readerSresult.GetInt32("Course") + "'";
                                                SqlCommand cmdData2 = new SqlCommand(loadData2, Con);
                                                SqlDataReader readerData2 = cmdData2.ExecuteReader();
                                                if (readerData2.Read())
                                                {
                                                    string newEntry = readerData2.GetString("Code") + " (" + readerData2.GetString("Unit") + ") A.R";

                                                    string newEntry2 = readerData2.GetString("Code") + " (" + readerData2.GetString("Unit") + ") REP";

                                                    // Check if newEntry2 exists in label52.Text
                                                    if (sCarryOvers.Contains(newEntry2))
                                                    {
                                                        // Replace newEntry2 with newEntry
                                                        sCarryOvers = sCarryOvers.Replace(newEntry2, newEntry);
                                                    }
                                                    else
                                                    {
                                                        // If newEntry2 doesn't exist, add newEntry
                                                        if (!sCarryOvers.Contains(newEntry))
                                                        {
                                                            sPassAll = sPassAll + 1;

                                                            sCarryOvers = sCarryOvers + "     " + newEntry;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    
                                }
                            }
                        }
                    }
                   
                    if ((semesterCTU != 0) && (semesterCTP != 0))
                    {
                        Decimal stp1 = (Decimal)semesterCTP;
                        Decimal stu1 = (Decimal)semesterCTU;
                        semesterTotalGrading = Math.Round((Decimal)(stp1 / stu1), 2);
                        //txtTotal.Text = String.Format("{0:0.00}",TotalGrading.ToString()) + "%";
                    }

                    if ((sCTU != 0) && (sCTP != 0))
                    {
                        Decimal stp2 = (Decimal)sCTP;
                        Decimal stu2 = (Decimal)sCTU;
                        sTotalGrading = Math.Round((Decimal)(stp2 / stu2), 2);
                    }

                    //Former CGPA

                    Int32 prevTP = sCTP - semesterCTP;
                    Int32 prevTU = sCTU - semesterCTU;


                    Decimal prevTotalGrading = 0;

                    if ((prevTU != 0) && (prevTP != 0))
                    {
                        Decimal stp3 = (Decimal)prevTP;
                        Decimal stu3 = (Decimal)prevTU;
                        prevTotalGrading = Math.Round((Decimal)(stp3 / stu3), 2);
                    }

                    string matric = readerSession.GetString("Matric");
                    string name = readerSession.GetString("Lastname") + ", " + readerSession.GetString("Firstname") + " " + readerSession.GetString("Middlename");
                    string sex = readerSession.GetString("Gender");

                    if (gradeType == "Percentage Grading")
                    {
                        if ((sPassAll == 0) && (sTotalGrading >= passMark))
                        {

                            dataGridView1.Rows.Add(sn, matric, sCourses, "", prevTP, prevTU, DecimalPlaces(prevTotalGrading) + suffix, semesterCTP, semesterCTU, DecimalPlaces(semesterTotalGrading) + suffix, sCTP, sCTU, DecimalPlaces(sTotalGrading) + suffix, "PASS");
                            sn++;
                        }

                        if ((sPassAll > 0) && (sTotalGrading >= passMark))
                        {

                            dataGridView2.Rows.Add(sn2, matric, sCourses, sCarryOvers, prevTP, prevTU, DecimalPlaces(prevTotalGrading) + suffix, semesterCTP, semesterCTU, DecimalPlaces(semesterTotalGrading) + suffix, sCTP, sCTU, DecimalPlaces(sTotalGrading) + suffix, "CSO");
                            sn2++;
                        }

                        if (sTotalGrading < passMark)
                        {
                            dataGridView3.Rows.Add(sn3, matric, sCourses, sCarryOvers, prevTP, prevTU, DecimalPlaces(prevTotalGrading) + suffix, semesterCTP, semesterCTU, DecimalPlaces(semesterTotalGrading) + suffix, sCTP, sCTU, DecimalPlaces(sTotalGrading) + suffix, "PROBATION");
                            sn3++;
                        }
                    }
                    else
                    {
                        if (sPassAll == 0)
                        {

                            dataGridView1.Rows.Add(sn, matric, sCourses, "", prevTP, prevTU, DecimalPlaces(prevTotalGrading) + suffix, semesterCTP, semesterCTU, DecimalPlaces(semesterTotalGrading) + suffix, sCTP, sCTU, DecimalPlaces(sTotalGrading) + suffix, "PASS");
                            sn++;
                        }

                        if ((sPassAll > 0) && (sTotalGrading >= 1))
                        {

                            dataGridView2.Rows.Add(sn2, matric, sCourses, sCarryOvers, prevTP, prevTU, DecimalPlaces(prevTotalGrading) + suffix, semesterCTP, semesterCTU, DecimalPlaces(semesterTotalGrading) + suffix, sCTP, sCTU, DecimalPlaces(sTotalGrading) + suffix, "CSO");
                            sn2++;
                        }

                        if (sTotalGrading < 1)
                        {
                            dataGridView3.Rows.Add(sn3, matric, sCourses, sCarryOvers, prevTP, prevTU, DecimalPlaces(prevTotalGrading) + suffix, semesterCTP, semesterCTU, DecimalPlaces(semesterTotalGrading) + suffix, sCTP, sCTU, DecimalPlaces(sTotalGrading) + suffix, "PROBATION");
                            sn3++;
                        }
                    }

                }
               
                // To Here

            }


            Int32 row1 = Int32.Parse(dataGridView1.RowCount.ToString());
            Int32 row2 = Int32.Parse(dataGridView2.RowCount.ToString());
            Int32 row3 = Int32.Parse(dataGridView3.RowCount.ToString());

            dataGridView4.Rows.Add(1, "PASS", dataGridView1.RowCount.ToString());
            dataGridView4.Rows.Add(2, "CSO", dataGridView2.RowCount.ToString());
            dataGridView4.Rows.Add(3, "PROBATION", dataGridView3.RowCount.ToString());
            dataGridView4.Rows.Add(4, "SICK", "");
            dataGridView4.Rows.Add(5, "ABSENT", "");
            dataGridView4.Rows.Add(6, "SUSPENDED", "");
            dataGridView4.Rows.Add(7, "WITHDRAWAL", "");
            dataGridView4.Rows.Add("", "TOTAL", (row1 + row2 + row3).ToString());

            dataGridView1.AutoSize = true;
            dataGridView1.Refresh();

            dataGridView3.AutoSize = true;
            dataGridView3.Refresh();

            dataGridView2.AutoSize = true;
            dataGridView2.Refresh();

            dataGridView4.AutoSize = true;
            dataGridView4.Refresh();

            dataGridView5.Rows.Add("Examination Officer", "Head of Department", "Program Coordinator");
            dataGridView5.Rows.Add(programExaminer, programHod, programDean);

            dataGridView5.AutoSize = true;
            dataGridView5.Refresh();
            Con.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        public void PrintPage(string type, DataGridView table)
        {
            Con.Open();

            string header = "";
            string department = "";

            string query = "SELECT * FROM UserTbl";
            SqlCommand cmd = new SqlCommand(query, Con);
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                header = reader.GetString("Header");
                department = reader.GetString("Department");
            }

            Con.Close();

            Image image = Resources.lautech_letter_head;

            DateTime currentTime = DateTime.Now;

            string time = currentTime.ToString("hh:mm tt");

            DateTime currentDate = DateTime.Today;

            string date = currentDate.ToString("dd/MM/yyyy");

            DGVPrinter printer = new DGVPrinter();
            //printer.HeaderImage = yourImage;
            printer.PageSettings.Margins = new Margins(10, 10, 10, 10);
            printer.TitleFont = new Font("Arial", 12, FontStyle.Bold);
            printer.SubTitleFont = new Font("Arial", 11, FontStyle.Bold);
            printer.Title = header + "\n(" + program.ToUpper() + ")\n";
            printer.SubTitle = "MASTER MARKSHEET \n" +
                "---------------------------------------------------------------------------------------------------------------------------------------------------\n" +
                "| SESSION: " + session + "   | LEVEL: " + label9.Text + "   | SEMESTER: " + semester + "   | PRINTED: " + date + " " + time + "   | " + type + " |\n" +
                "---------------------------------------------------------------------------------------------------------------------------------------------------\n" +
                type;
            printer.SubTitleFormatFlags = StringFormatFlags.LineLimit |

                                  StringFormatFlags.NoClip;

            printer.PageNumbers = false;

            printer.PageNumberInHeader = false;

            printer.PorportionalColumns = true;

            printer.HeaderCellAlignment = StringAlignment.Near;

            printer.Footer = "Printed using contextual Software by asltech.net. Any Alteration on this Result Renders it Invalid";

            printer.FooterFont = new Font(new FontFamily("Arial"), 7, FontStyle.Italic);

            printer.FooterSpacing = 15;

            printer.printDocument.DefaultPageSettings.Landscape = true;

            printer.PrintDataGridView(table);
        }

        private void frmMms_Shown(object sender, EventArgs e)
        {
            /*
            PrintPage("PASS LIST", dataGridView1);
            PrintPage("CSO LIST", dataGridView2);
            PrintPage("PROBATION LIST", dataGridView3);

            PrintDialog pd = new PrintDialog();
            PrintDocument doc = new PrintDocument();
            doc.DefaultPageSettings.Landscape = true;
            doc.PrintPage += myPrintPage;
            pd.Document = doc;
            if (pd.ShowDialog() == DialogResult.OK)
            {
                doc.Print();
            }
            */
        }

        private void myPrintPage(object sender, PrintPageEventArgs e)
        {

            Con.Open();

            string header = "";
            string department = "";

            string query = "SELECT * FROM UserTbl";
            SqlCommand cmd = new SqlCommand(query, Con);
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                header = reader.GetString("Header");
                department = reader.GetString("Department");
            }

            Con.Close();

            Image image = Resources.lautech_letter_head;

            e.Graphics.DrawImage(image, 0, 0, 125, 119);

            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;

            //e.Graphics.DrawString(header.ToUpper(), new Font("Arial", 12, FontStyle.Bold), Brushes.Black, new Point(230, 30), stringFormat);
            e.Graphics.DrawString(header.ToUpper(), new Font("Arial", 12, FontStyle.Bold), Brushes.Black, new RectangleF(550, 30, 0, 0), stringFormat);


            Rectangle rect = new Rectangle(550, 90, 0, 0);
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;

            //e.Graphics.DrawString("B.TECH (" + program.ToUpper() + ")", new Font("Arial", 11, FontStyle.Bold), Brushes.Black, new Point(350, 90), format);
            e.Graphics.DrawString("(" + program.ToUpper() + ")", new Font("Arial", 11, FontStyle.Bold), Brushes.Black, rect, format);

            e.Graphics.DrawString("MASTER MARKSHEET", new Font("Arial", 12, FontStyle.Bold | FontStyle.Underline), Brushes.Black, new Point(475, 120));

            Bitmap bm0 = new Bitmap(tableLayoutPanel1.Width, tableLayoutPanel1.Height);
            tableLayoutPanel1.DrawToBitmap(bm0, new Rectangle(0, 0, tableLayoutPanel1.Width, tableLayoutPanel1.Height));
            e.Graphics.DrawImage(bm0, 95, 145);

            Rectangle recto = new Rectangle(550, 190, 0, 0);
            StringFormat formato = new StringFormat();
            formato.Alignment = StringAlignment.Center;

            e.Graphics.DrawString("SUMMARY", new Font("Arial", 12, FontStyle.Bold | FontStyle.Underline), Brushes.Black, recto, formato);

            Bitmap bm = new Bitmap(dataGridView4.Width, dataGridView4.Height);
            dataGridView4.DrawToBitmap(bm, new Rectangle(0, 0, dataGridView4.Width, dataGridView4.Height));
            e.Graphics.DrawImage(bm, 340, 240);

            Bitmap bm1 = new Bitmap(dataGridView5.Width, dataGridView5.Height);
            dataGridView5.DrawToBitmap(bm1, new Rectangle(0, 0, dataGridView5.Width, dataGridView5.Height));
            e.Graphics.DrawImage(bm1, 95, 550);
        }

        private void printButton_Click(object sender, EventArgs e)
        {
            PrintPage("PASS LIST", dataGridView1);
            PrintPage("CSO LIST", dataGridView2);
            PrintPage("PROBATION LIST", dataGridView3);

            PrintDialog pd = new PrintDialog();
            PrintDocument doc = new PrintDocument();
            doc.DefaultPageSettings.Landscape = true;
            doc.PrintPage += myPrintPage;
            pd.Document = doc;
            if (pd.ShowDialog() == DialogResult.OK)
            {
                doc.Print();
            }
        }

        private void exelButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Create a new Excel workbook
                using (var workbook = new XLWorkbook())
                {
                    // Add worksheets for each DataGridView
                    ExportDataGridViewToWorksheet(workbook, dataGridView1, "PASS LIST");
                    ExportDataGridViewToWorksheet(workbook, dataGridView2, "CSO LIST");
                    ExportDataGridViewToWorksheet(workbook, dataGridView3, "PROBATION LIST");

                    // Save the workbook to a file
                    workbook.SaveAs("ExportedData.xlsx");
                }
                MessageBox.Show("Export Successful");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while exporting data: {ex.Message}");
            }
        }

        private void ExportDataGridViewToWorksheet(XLWorkbook workbook, DataGridView dataGridView, string sheetName)
        {
            // Create a new worksheet
            var worksheet = workbook.Worksheets.Add(sheetName);

            // Write DataGridView headers to Excel
            for (int i = 1; i < dataGridView.Columns.Count + 1; i++)
            {
                worksheet.Cell(1, i).Value = dataGridView.Columns[i - 1].HeaderText;
            }

            // Write DataGridView rows to Excel
            for (int i = 0; i < dataGridView.Rows.Count; i++)
            {
                for (int j = 0; j < dataGridView.Columns.Count; j++)
                {
                    worksheet.Cell(i + 2, j + 1).Value = dataGridView.Rows[i].Cells[j].Value?.ToString();
                }
            }

            // Show save file dialog to user
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Excel files (*.xlsx)|*.xlsx";
            saveDialog.FilterIndex = 2;

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                workbook.SaveAs(saveDialog.FileName);

            }
        }
    }
}
