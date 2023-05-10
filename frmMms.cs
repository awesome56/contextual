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
        public string sCarryOvers;
        public string sCourses;
        public string lvlMark;
        public string gradeType;
        public Int32 passMark;

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
                    if(gradeType == "Percentage Grading")
                    {
                        passMark = 10;
                    }
                    else
                    {
                        passMark = 1;
                    }
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

            string Query = "SELECT TOP " + divString + " * FROM Session where Start <= " + sessionStart + "and Semester = 'Harmattan' ORDER BY NAME DESC";
            SqlCommand cmd = new SqlCommand(Query, Con);
            SqlDataReader reader = cmd.ExecuteReader();
            Int32 count = 1;
            while (reader.Read())
            {

                if (count == Int32.Parse(level))
                {
                    levelSessionId = reader.GetInt32("Id");

                    //MessageBox.Show(levelSessionId.ToString());
                }
                count++;
            }

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

                sCarryOvers = "";

                sCourses = "";

                

                string carryOvers = "";

                Int32 scores;

                string querySresult = "SELECT * FROM [Result] where Student = '" + readerSession.GetInt32("Id") + "'";
                SqlCommand cmdSresult = new SqlCommand(querySresult, Con);
                SqlDataReader readerSresult = cmdSresult.ExecuteReader();

                while (readerSresult.Read())
                {
                    Int32 sessSelectId = readerSresult.GetInt32("Session");

                    string querySsession = "SELECT * FROM [Session] where Id = '" + sessSelectId + "'";
                    SqlCommand cmdSsession = new SqlCommand(querySsession, Con);
                    SqlDataReader readerSsession = cmdSsession.ExecuteReader();

                    if (readerSsession.Read())
                    {
                        Int32 sSessionStart = readerSsession.GetInt32("Start");

                        if (sessionStart >= sSessionStart)
                        {
                            string unit = "";

                            string loadCourse = "SELECT * FROM [Course] where Id = '" + readerSresult.GetInt32("Course") + "'";
                            SqlCommand cmdCourse = new SqlCommand(loadCourse, Con);
                            SqlDataReader readerCourse = cmdCourse.ExecuteReader();

                            if (readerCourse.Read())
                            {
                                if (semester == "H") //it is first semester
                                {
                                    if ((sessionId == readerSresult.GetInt32("Session")) && (readerCourse.GetString("Semester") == "R"))
                                    {
                                        //don't fetch second semester for current session
                                    }
                                    else
                                    {
                                        unit = readerCourse.GetString("Unit");
                                        scores = readerSresult.GetInt32("Score");

                                        if ((session == readerSsession.GetString("Name")) && (semester == readerCourse.GetString("Semester")))
                                        {
                                            string aCourse = "";
                                            //MessageBox.Show(readerCourse.GetString("Code"));
                                            if (scores >= 0)
                                            {
                                                aCourse = readerCourse.GetString("Code") + " (" + readerCourse.GetString("Unit") + ")" + scores + Grading(scores);
                                                semesterCTU = Int32.Parse(unit) + semesterCTU;
                                                semesterCTP = (Int32.Parse(unit) * scores) + semesterCTP;
                                                if (sCourses == "")
                                                {
                                                    sCourses = aCourse;
                                                }
                                                else
                                                {
                                                    sCourses = sCourses + "     " + aCourse;
                                                }
                                                
                                            }

                                            if ((scores < 40) && (scores >= 0))
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

                                            if (scores < 0)
                                            {
                                                aCourse = readerCourse.GetString("Code") + " (" + readerCourse.GetString("Unit") + ")-AR";
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
                                            sCTP = (Int32.Parse(unit) * scores) + sCTP;
                                        }

                                        if ((scores < 40) && (scores >= 0))
                                        {
                                            carryOvers = readerCourse.GetString("Code") + " (" + readerCourse.GetString("Unit") + ")REP";
                                            sPassAll = sPassAll + 1;
                                            if (sCarryOvers == "")
                                            {
                                                sCarryOvers = carryOvers;
                                            }
                                            else
                                            {
                                                sCarryOvers = sCarryOvers + "     " + carryOvers;
                                            }
                                            
                                        }

                                        if (scores < 0)
                                        {
                                            carryOvers = readerCourse.GetString("Code") + " (" + readerCourse.GetString("Unit") + ")-AR";
                                            sPassAll = sPassAll + 1;
                                            if (sCarryOvers == "")
                                            {
                                                sCarryOvers = carryOvers;
                                            }
                                            else
                                            {
                                                sCarryOvers = sCarryOvers + "     " + carryOvers;
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
                                            aCourse = readerCourse.GetString("Code") + " (" + readerCourse.GetString("Unit") + ")" + scores + Grading(scores) ;
                                            semesterCTU = Int32.Parse(unit) + semesterCTU;
                                            semesterCTP = (Int32.Parse(unit) * scores) + semesterCTP;
                                            if (sCourses == "")
                                            {
                                                sCourses = aCourse;
                                            }
                                            else
                                            {
                                                sCourses = sCourses + "     " + aCourse;
                                            }
                                        }

                                        if ((scores < 40) && (scores >= 0))
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

                                        if (scores < 0)
                                        {
                                            aCourse = readerCourse.GetString("Code") + " (" + readerCourse.GetString("Unit") + ")-AR";
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
                                        sCTP = (Int32.Parse(unit) * scores) + sCTP;
                                    }

                                    if ((scores < 40) && (scores >= 0))
                                    {
                                        carryOvers = readerCourse.GetString("Code") + " (" + readerCourse.GetString("Unit") + ")REP";
                                        sPassAll = sPassAll + 1;
                                        if (sCarryOvers == "")
                                        {
                                            sCarryOvers = carryOvers;
                                        }
                                        else
                                        {
                                            sCarryOvers = sCarryOvers + "     " + carryOvers;
                                        }
                                    }

                                    if (scores < 0)
                                    {
                                        carryOvers = readerCourse.GetString("Code") + " (" + readerCourse.GetString("Unit") + ")-AR";
                                        sPassAll = sPassAll + 1;
                                        if (sCarryOvers == "")
                                        {
                                            sCarryOvers = carryOvers;
                                        }
                                        else
                                        {
                                            sCarryOvers = sCarryOvers + "     " + carryOvers;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                /*
                for (Int32 i = 1; i <= Int32.Parse(level); i++)
                {

                    string unit = "";

                    string loadCourse = "SELECT * FROM [Course] where Program = '" + programId + "' and Level = '" + i + "'";
                    SqlCommand cmdCourse = new SqlCommand(loadCourse, Con);
                    SqlDataReader readerCourse = cmdCourse.ExecuteReader();

                    while (readerCourse.Read())
                    {

                        if (semester == "H")
                        {
                            if ((i == Int32.Parse(level)) && (readerCourse.GetString("Semester") == "R"))
                            {
                            }
                            else
                            {
                                unit = readerCourse.GetString("Unit");

                                string loadProgData = "SELECT * FROM [Result] where Student = '" + readerSession.GetInt32("Id").ToString() + "' and Course = '" + readerCourse.GetInt32("Id") + "'";
                                SqlCommand cmdProgData = new SqlCommand(loadProgData, Con);
                                SqlDataReader readerProgData = cmdProgData.ExecuteReader();
                                while (readerProgData.Read())
                                {
                                    scores = readerProgData.GetInt32("Score");

                                    if ((i == Int32.Parse(level)) && (semester == readerCourse.GetString("Semester")))
                                    {
                                        if (scores >= 0)
                                        {
                                            aCourse = readerCourse.GetString("Code") + " (" + readerCourse.GetString("Unit") + ") "+ scores;

                                            semesterCTU = Int32.Parse(unit) + semesterCTU;
                                            semesterCTP = (Int32.Parse(unit) * scores) + semesterCTP;

                                            sCourses = aCourse + "     " + aCourse;
                                        }

                                        if ((scores < 40) && (scores >= 0))
                                        {
                                            aCourse = readerCourse.GetString("Code") + " (" + readerCourse.GetString("Unit") + ") " + scores;
                                            sCourses = aCourse + "     " + aCourse;
                                            // sPassAll = sPassAll + 1;
                                        }

                                        if (scores < 0)
                                        {
                                            aCourse = readerCourse.GetString("Code") + " (" + readerCourse.GetString("Unit") + ") AR";
                                            sCourses = aCourse + "     " + aCourse;
                                            // sPassAll = sPassAll + 1;
                                        }
                                    }

                                    if (scores >= 0)
                                    {
                                        sCTU = Int32.Parse(unit) + sCTU;
                                        sCTP = (Int32.Parse(unit) * scores) + sCTP;
                                    }

                                    if ((scores < 40) && (scores >= 0))
                                    {
                                        carryOvers = readerCourse.GetString("Code") + " (" + readerCourse.GetString("Unit") + ") REP";

                                        sPassAll = sPassAll + 1;


                                        sCarryOvers = sCarryOvers + "     " + carryOvers;
                                    }

                                    if (scores < 0)
                                    {
                                        carryOvers = readerCourse.GetString("Code") + " (" + readerCourse.GetString("Unit") + ") A.R";

                                        sPassAll = sPassAll + 1;


                                        sCarryOvers = sCarryOvers + "     " + carryOvers;
                                    }
                                }



                            }
                        }
                        else
                        {
                            unit = readerCourse.GetString("Unit");

                            string loadProgData = "SELECT * FROM [Result] where Student = '" + readerSession.GetInt32("Id").ToString() + "' and Course = '" + readerCourse.GetInt32("Id") + "'";
                            SqlCommand cmdProgData = new SqlCommand(loadProgData, Con);
                            SqlDataReader readerProgData = cmdProgData.ExecuteReader();
                            while (readerProgData.Read())
                            {
                                scores = readerProgData.GetInt32("Score");

                                if ((i == Int32.Parse(level)) && (semester == readerCourse.GetString("Semester")))
                                {
                                    if (scores >= 0)
                                    {
                                        aCourse = readerCourse.GetString("Code") + " (" + readerCourse.GetString("Unit") + ") " + scores;
                                        semesterCTU = Int32.Parse(unit) + semesterCTU;
                                        semesterCTP = (Int32.Parse(unit) * scores) + semesterCTP;
                                        sCourses = aCourse + "     " + aCourse;
                                    }

                                    if ((scores < 40) && (scores >= 0))
                                    {
                                        aCourse = readerCourse.GetString("Code") + " (" + readerCourse.GetString("Unit") + ") " + scores;
                                        sCourses = aCourse + "     " + aCourse;
                                        // sPassAll = sPassAll + 1;
                                    }

                                    if (scores < 0)
                                    {
                                        aCourse = readerCourse.GetString("Code") + " (" + readerCourse.GetString("Unit") + ") AR";
                                        sCourses = aCourse + "     " + aCourse;
                                        // sPassAll = sPassAll + 1;
                                    }
                                }

                                if (scores >= 0)
                                {


                                    sCTU = Int32.Parse(unit) + sCTU;
                                    sCTP = (Int32.Parse(unit) * scores) + sCTP;
                                }

                                if ((scores < 40) && (scores >= 0))
                                {
                                    carryOvers = readerCourse.GetString("Code") + " (" + readerCourse.GetString("Unit") + ") REP";

                                    sPassAll = sPassAll + 1;


                                    sCarryOvers = sCarryOvers + "     " + carryOvers;
                                }

                                if (scores < 0)
                                {
                                    carryOvers = readerCourse.GetString("Code") + " (" + readerCourse.GetString("Unit") + ") A.R";

                                    sPassAll = sPassAll + 1;


                                    sCarryOvers = sCarryOvers + "     " + carryOvers;
                                }
                            }
                        }
                    }
                }
                */
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




                    //txtTotal.Text = String.Format("{0:0.00}",TotalGrading.ToString()) + "%";
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

                if ((sPassAll == 0) && (sTotalGrading >= passMark))
                {
                    dataGridView1.Rows.Add(sn, matric, sCourses, "",prevTP,prevTU, prevTotalGrading + "%", semesterCTP,semesterCTU, semesterTotalGrading + "%", sCTP,sCTU, sTotalGrading + "%", "PASS");
                    sn++;
                }

                if ((sPassAll > 0) && (sTotalGrading >= passMark))
                {
                    dataGridView2.Rows.Add(sn2, matric, sCourses, sCarryOvers,prevTP, prevTU, prevTotalGrading + "%", semesterCTP, semesterCTU, semesterTotalGrading + "%", sCTP, sCTU, sTotalGrading + "%", "CSO");
                    sn2++;
                }

                if (sTotalGrading < passMark)
                {
                    dataGridView3.Rows.Add(sn3, matric, sCourses, sCarryOvers,prevTP, prevTU, prevTotalGrading + "%", semesterCTP, semesterCTU, semesterTotalGrading + "%", sCTP, sCTU, sTotalGrading + "%", "PROBATION");
                    sn3++;
                }



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

            dataGridView5.Rows.Add("Examination Officer", "Head of Department", "Post Graduate Coodinator");
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
    }
}
