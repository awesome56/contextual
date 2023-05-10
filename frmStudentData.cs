using FontAwesome.Sharp;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using System.Drawing.Printing;
using GlobalVariable;
using Bunifu.Framework.UI;

namespace Contextual
{
    public partial class frmStudentData : Form
    {
        public frmStudentData()
        {
            InitializeComponent();
            
        }

        public string studentId = "";
        Int32 studentTableId;
        string StudentName = "";
        string StudentGender = "";
        string StudentYear = "";
        string StudentYearId = "";
        string StudentProgram = "";
        string StudentProgramId = "";
        string currentSession = "";
        string studentLevel = "";
        string maxSemester = "";
        string programSession = "";
        string programSemester = "";
        string lvlMark = "";
        Int32 studentStart;
        Int32 startSession;
        Int32 maxInt;

        Int32 semesterCTU = 0;
        Int32 semesterCTP = 0;
        Decimal semesterTotalGrading;

        Int32 sCTU;
        Int32 sCTP;
        Decimal sTotalGrading;
        string sCarryOvers = "";

        Int32 CTU;
        Int32 CTP;
        Decimal TotalGrading;

        Int32 passAll = 0;

        Int32 sPassAll = 0;

        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + GlobalVariable.Globals.databasePath + ";Integrated Security=True;MultipleActiveResultSets=True;Connect Timeout=30");

        void LoadResultData()
        {
            CTU = 0;
            CTP = 0;
            TotalGrading = 0;
            label52.Text = "";
            string carryOvers = "";
            Int32 scores;
            Con.Open();
            string loadProgData = "SELECT * FROM [Result] where Student = '" + studentTableId + "'";
            SqlCommand cmdProgData = new SqlCommand(loadProgData, Con);
            SqlDataReader readerProgData = cmdProgData.ExecuteReader();
            while (readerProgData.Read())
            {
                scores = readerProgData.GetInt32("Score");

                if (scores >= 0)
                {
                    string unit = "";
                    string loadCourse = "SELECT * FROM [Course] where Id = '" + readerProgData.GetInt32("Course") + "'";
                    SqlCommand cmdCourse = new SqlCommand(loadCourse, Con);
                    SqlDataReader readerCourse = cmdCourse.ExecuteReader();
                    if (readerCourse.Read())
                    {
                        unit = readerCourse.GetString("Unit");
                    }

                    CTU = Int32.Parse(unit) + CTU;
                    CTP = (Int32.Parse(unit) * scores) + CTP;
                }

                if ((scores < 40) && (scores >= 0))
                {
                    string loadData = "SELECT * FROM [Course] where Id = '" + readerProgData.GetInt32("Course") + "'";
                    SqlCommand cmdData = new SqlCommand(loadData, Con);
                    SqlDataReader readerData = cmdData.ExecuteReader();
                    if (readerData.Read())
                    {
                        carryOvers = readerData.GetString("Code") + " (" + readerData.GetString("Unit") + ") REP";

                        passAll = passAll + 1;
                    }

                    label52.Text = label52.Text + "     " + carryOvers;
                }

                if (scores < 0)
                {
                    string loadData2 = "SELECT * FROM [Course] where Id = '" + readerProgData.GetInt32("Course") + "'";
                    SqlCommand cmdData2 = new SqlCommand(loadData2, Con);
                    SqlDataReader readerData2 = cmdData2.ExecuteReader();
                    if (readerData2.Read())
                    {
                        carryOvers = readerData2.GetString("Code") + " (" + readerData2.GetString("Unit") + ") A.R";

                        passAll = passAll + 1;
                    }

                    label52.Text = label52.Text + "     " + carryOvers;
                }
            }
            Con.Close();

            label44.Text = "Cumulative Total Units =";
            label46.Text = "Cumulative Total Points =";
            label48.Text = "Cumulative Percentage Total =";

            txtTU.Text = "";
            txtTP.Text = "";
            txtTotal.Text = "";

            if ((CTU != 0) && (CTP != 0))
            {
                Decimal stp = (Decimal) CTP;
                Decimal stu = (Decimal) CTU;
                TotalGrading = Math.Round((Decimal)(stp / stu), 2);



                txtTU.Text = CTU.ToString();
                txtTP.Text = CTP.ToString();
                //txtTotal.Text = String.Format("{0:0.00}",TotalGrading.ToString()) + "%";
                txtTotal.Text = TotalGrading.ToString() + "%";
            }
        }

        void LoadSemesterTotal(Int32 getSession, string getSesmster, string getProgram)
        {

            Con.Open();


            sCTU = 0;
            sCTP = 0;
            sTotalGrading = 0;
            sCarryOvers = "";

            sPassAll = 0;

            string carryOvers = "";
            Int32 scores;
            Int32 getSessionStart = 0;

            string queryStartsession = "SELECT * FROM [Session] where Id = '" + getSession + "'";
            SqlCommand cmdStartsession = new SqlCommand(queryStartsession, Con);
            SqlDataReader readerStartsession = cmdStartsession.ExecuteReader();

            if (readerStartsession.Read())
            {
                getSessionStart = readerStartsession.GetInt32("Start");
            }

            string querySresult = "SELECT * FROM [Result] where Student = '" + studentTableId + "'";
            SqlCommand cmdSresult = new SqlCommand(querySresult, Con);
            SqlDataReader readerSresult = cmdSresult.ExecuteReader();

            while (readerSresult.Read())
            {
                Int32 progSelectId = readerSresult.GetInt32("Session");

                string querySsession = "SELECT * FROM [Session] where Id = '" + progSelectId + "'";
                SqlCommand cmdSsession = new SqlCommand(querySsession, Con);
                SqlDataReader readerSsession = cmdSsession.ExecuteReader();

                if (readerSsession.Read())
                {
                    Int32 sSessionStart = readerSsession.GetInt32("Start");

                    if (getSessionStart >= sSessionStart)
                    {
                        string unit = "";

                        string loadCourse = "SELECT * FROM [Course] where Id = '" + readerSresult.GetInt32("Course") + "'";
                        SqlCommand cmdCourse = new SqlCommand(loadCourse, Con);
                        SqlDataReader readerCourse = cmdCourse.ExecuteReader();

                        if(readerCourse.Read())
                        {
                            if(getSesmster == "H") //it is first semester
                            {
                                if((getSession == readerSresult.GetInt32("Session")) && (readerCourse.GetString("Semester") == "R"))
                                {
                                    //don't fetch second semester for current session
                                }
                                else
                                {
                                    unit = readerCourse.GetString("Unit");
                                    scores = readerSresult.GetInt32("Score");

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
                            else //it is second semester fetch all result
                            {
                                unit = readerCourse.GetString("Unit");
                                scores = readerSresult.GetInt32("Score");

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
            }
                /*
                for (Int32 i = 1; i <= getLevel; i++)
                {
                    string unit = "";
                    string loadCourse = "SELECT * FROM [Course] where Program = '" + getProgram + "' and Level = '" + i + "'";
                    SqlCommand cmdCourse = new SqlCommand(loadCourse, Con);
                    SqlDataReader readerCourse = cmdCourse.ExecuteReader();

                    while (readerCourse.Read())
                    {

                        if (getSesmster == "H")
                        {
                        if ((i == getLevel) && (readerCourse.GetString("Semester") == "R"))
                        {
                        }
                        else
                        {
                                unit = readerCourse.GetString("Unit");

                                string loadProgData = "SELECT * FROM [Result] where Student = '" + studentTableId + "' and Course = '" + readerCourse.GetInt32("Id") + "'";
                                SqlCommand cmdProgData = new SqlCommand(loadProgData, Con);
                                SqlDataReader readerProgData = cmdProgData.ExecuteReader();
                                while (readerProgData.Read())
                                {
                                    scores = readerProgData.GetInt32("Score");

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

                            string loadProgData = "SELECT * FROM [Result] where Student = '" + studentTableId + "' and Course = '" + readerCourse.GetInt32("Id") + "'";
                            SqlCommand cmdProgData = new SqlCommand(loadProgData, Con);
                            SqlDataReader readerProgData = cmdProgData.ExecuteReader();
                            while (readerProgData.Read())
                            {
                                scores = readerProgData.GetInt32("Score");

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
                Con.Close();

            label34.Text = "";
            label35.Text = "";
            label36.Text = "";

            if ((sCTU != 0) && (sCTP != 0))
            {
                Decimal stp = (Decimal)sCTP;
                Decimal stu = (Decimal)sCTU;
                sTotalGrading = Math.Round((Decimal)(stp / stu), 2);



                label34.Text = sCTU.ToString();
                label35.Text = sCTP.ToString();
                //txtTotal.Text = String.Format("{0:0.00}",TotalGrading.ToString()) + "%";
                label36.Text = sTotalGrading.ToString() + "%";
            }


        }
        void LoadStdTable()
        {
            try 
            { 
            dataStd.Rows.Clear();
            Con.Open();

            Int32 currentInt = -1;
            
            


            string loadProgData = "SELECT * FROM [Program] where Id = '"+ StudentProgramId + "'";
            SqlCommand cmdProgData = new SqlCommand(loadProgData, Con);
            SqlDataReader readerProgData = cmdProgData.ExecuteReader();
            if (readerProgData.Read())
            {
                maxSemester = readerProgData.GetString("Total");
                currentSession = readerProgData.GetString("Session");
                currentInt = Int32.Parse(currentSession);
                maxInt = Int32.Parse(maxSemester);
                    if (readerProgData.GetString("Lvl_mark") == "")
                    {
                        lvlMark = "Masters";
                    }
                    else
                    {
                        lvlMark = readerProgData.GetString("Lvl_mark");
                    }
                lvlMark = readerProgData.GetString("Lvl_mark");

                    string loadData = "SELECT * FROM [Session] where Id = '" + currentSession + "'";
                SqlCommand cmdData = new SqlCommand(loadData, Con);
                SqlDataReader readerData = cmdData.ExecuteReader();
                if (readerData.Read())
                {
                    programSession = readerData.GetString("Name");
                    startSession = readerData.GetInt32("Start");
                    programSemester = readerData.GetString("Semester");
                }
            }

            string loadProg = "SELECT TOP "+ maxInt +" * FROM Session where Start >= " + studentStart +" and Start <= "+ startSession + " ORDER BY START ASC";
            SqlCommand cmdProg = new SqlCommand(loadProg, Con);
            SqlDataReader reader = cmdProg.ExecuteReader();

            Int32 i = 1;
            Int32 j = i;

            while (reader.Read())
            {
                if (i % 2 == 0)
                {
                    j = i / 2;
                }
                else
                {
                    j = (int)Math.Ceiling((double)i / (double)2);
                }

                string session = reader.GetString("Name");
                string semester = reader.GetString("Semester");
                string jLevel = "";

                //MessageBox.Show(programSession);

                if (programSemester == "Harmattan")
                {
                    if ((session == programSession) && (semester == "Rain"))
                    {
                    }
                    else
                    {
                            if (lvlMark == "Undergraduate")
                            {
                                jLevel = j + "00 LEVEL";
                            }

                            if (lvlMark == "Post Graduate")
                            {
                                jLevel = j + 6 + "00 LEVEL";
                            }

                            if (lvlMark == "Masters")
                            {
                                jLevel = "Year " + j;
                            }

                            if (lvlMark == "PHD")
                            {
                                jLevel = "Year " + j;
                            }

                            if (lvlMark == "ND")
                            {
                                jLevel = "ND " + j;
                            }

                            if (lvlMark == "HND")
                            {
                                jLevel = "HND " + j;
                            }

                            if (lvlMark == "Year")
                            {
                                jLevel = "Year " + j;
                            }

                            dataStd.Rows.Add(session, jLevel, semester);
                    }
                }
                else
                {
                        if (lvlMark == "Undergraduate")
                        {
                            jLevel = j + "00 LEVEL";
                        }

                        if (lvlMark == "Post Graduate")
                        {
                            jLevel = j + 6 + "00 LEVEL";
                        }

                        if (lvlMark == "Masters")
                        {
                            jLevel = "Year " + j;
                        }

                        if (lvlMark == "PHD")
                        {
                            jLevel = "Year " + j;
                        }

                        if (lvlMark == "ND")
                        {
                            jLevel = "ND " + j;
                        }

                        if (lvlMark == "HND")
                        {
                            jLevel = "HND " + j;
                        }

                        if (lvlMark == "Year")
                        {
                            jLevel = "Year " + j;
                        }

                        dataStd.Rows.Add(session, jLevel, semester);
                }



                studentLevel = j.ToString();

                i = 1 + i;
                j = i;
            }

            Con.Close();

            comboBox4.Items.Clear();
            
            for (Int32 x = 1; x <= Int32.Parse(studentLevel); x++)
            {
                comboBox4.Items.Add(x.ToString());
            }
            
            
            if ((studentStart + ((int)Math.Ceiling((double)maxInt / (double)2))) < startSession)
            {
                label14.Text = "Grad";
            }
            else
            {
                    if (lvlMark == "Undergraduate")
                    {
                        label14.Text = studentLevel + "00 LEVEL";
                    }

                    if (lvlMark == "Post Graduate")
                    {
                        label14.Text = Int32.Parse(studentLevel) + 6 + "00 LEVEL";
                    }

                    if (lvlMark == "Masters")
                    {
                        label14.Text = "Year " + studentLevel;
                    }

                    if (lvlMark == "PHD")
                    {
                        label14.Text = "Year " + studentLevel;
                    }

                    if (lvlMark == "ND")
                    {
                        label14.Text = "ND " + studentLevel;
                    }

                    if (lvlMark == "HND")
                    {
                        label14.Text = "HND " + studentLevel;
                    }

                    if (lvlMark == "Year")
                    {
                        label14.Text = "Year " + studentLevel;
                    }
                    //label14.Text = studentLevel;
            }
        }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

}

        void LoadCourseTable(Int32 session, string semester)
        {
            semesterCTU = 0;
            semesterCTP = 0;
            semesterTotalGrading = 0;

            Con.Open();
            dataCourses.Rows.Clear();
            string loadProgData = "SELECT * FROM [Course] where Program = '" + StudentProgramId + "' and Semester = '" + semester + "'";
            SqlCommand cmdProgData = new SqlCommand(loadProgData, Con);
            SqlDataReader readerProgData = cmdProgData.ExecuteReader();
            while (readerProgData.Read())
            {
                string code = readerProgData.GetString("Code");
                string name = readerProgData.GetString("Name");
                string unit = readerProgData.GetString("Unit");
                string cId = readerProgData.GetInt32("Id").ToString();

                bool exists = false;

                string chechuser = "SELECT count(*) FROM [Result] where Student = '" + studentTableId + "'and Course = '" + cId + "' and Session ='" + session + "'";

                SqlCommand cmdUser = new SqlCommand(chechuser, Con);
                cmdUser.Parameters.AddWithValue("Result", cId);
                exists = (int)cmdUser.ExecuteScalar() > 0;

                if (exists)
                {
                    string checkScore = "SELECT * FROM [Result] where Student = '" + studentTableId + "'and Course = '" + cId + "' and Session ='" + session + "'";
                    SqlCommand cmdMark = new SqlCommand(checkScore, Con);
                    SqlDataReader readerScore = cmdMark.ExecuteReader();

                    Int32 scoreMark = -1;

                    if (readerScore.Read())
                    {
                        scoreMark = readerScore.GetInt32("Score");
                    }

                    if (scoreMark >= 0)
                    {
                        semesterCTU = Int32.Parse(unit) + semesterCTU;
                        semesterCTP = (Int32.Parse(unit) * scoreMark) + semesterCTP;
                    }

                    string gradeScore = "";

                    if ((scoreMark >= 70) && (scoreMark <= 100))
                    {
                        gradeScore = "A";
                    }
                    else if ((scoreMark >= 60) && (scoreMark < 70))
                    {
                        gradeScore = "B";
                    }
                    else if ((scoreMark >= 50) && (scoreMark < 60))
                    {
                        gradeScore = "C";
                    }
                    else if ((scoreMark >= 45) && (scoreMark < 50))
                    {
                        gradeScore = "D";
                    }
                    else if ((scoreMark >= 40) && (scoreMark < 45))
                    {
                        gradeScore = "E";
                    }
                    else
                    {
                        gradeScore = "F";
                    }

                    if (scoreMark < 0)
                    {
                        gradeScore = "AR";
                    }

                    dataCourses.Rows.Add(true, code, name, unit, scoreMark, gradeScore, cId);
                }
            }
            Con.Close();

            label44.Text = "Semester Total Units =";
            label46.Text = "Semester Total Points =";
            label48.Text = "Semester Percentage Total =";

            txtTU.Text = "";
            txtTP.Text = "";
            txtTotal.Text = "";

            if ((semesterCTU != 0) && (semesterCTP != 0))
            {
                Decimal tp = (Decimal)semesterCTP;
                Decimal tu = (Decimal)semesterCTU;
                semesterTotalGrading = Math.Round((Decimal)(tp / tu), 2);

                

                txtTU.Text = semesterCTU.ToString();
                txtTP.Text = semesterCTP.ToString();
                txtTotal.Text = semesterTotalGrading.ToString() + "%";
            }
            
        }


        void LoadExtraCourse(string semester, string currentlevel)
        {
            dataExtraCourse.Rows.Clear();

            Con.Open();
            
            string loadExtraData = "SELECT * FROM [Course] where Program = '" + StudentProgramId + "' and Semester = '" + semester + "'";
            SqlCommand cmdExtraData = new SqlCommand(loadExtraData, Con);
            SqlDataReader readerExtraData = cmdExtraData.ExecuteReader();
            while (readerExtraData.Read())
            {
                string code = readerExtraData.GetString("Code");
                string name = readerExtraData.GetString("Name");
                string unit = readerExtraData.GetString("Unit");
                string cId = readerExtraData.GetInt32("Id").ToString();

                string level = readerExtraData.GetString("Level");
                /*
                if (lvlMark == "Undergraduate")
                {
                    level = readerExtraData.GetString("Level") + "00 LEVEL";
                }

                if (lvlMark == "Post Graduate")
                {
                    level = Int32.Parse(readerExtraData.GetString("Level")) + 6 + "00 LEVEL";
                }

                if (lvlMark == "Masters")
                {
                    level = "Year " + readerExtraData.GetString("Level");
                }

                if (lvlMark == "PHD")
                {
                    level = "Year " + readerExtraData.GetString("Level");
                }

                if (lvlMark == "ND")
                {
                    level = "ND " + readerExtraData.GetString("Level");
                }

                if (lvlMark == "HND")
                {
                    level = "HND " + readerExtraData.GetString("Level");
                }

                if (lvlMark == "Year")
                {
                    level = "Year " + readerExtraData.GetString("Level");
                }
                */
                if (Int32.Parse(currentlevel)  >= Int32.Parse(level))
                {
                    bool exists = false;

                    string chechuser = "SELECT count(*) FROM [Result] where Student = '" + studentTableId + "'and Course = '" + cId + "'";

                    SqlCommand cmdUser = new SqlCommand(chechuser, Con);
                    cmdUser.Parameters.AddWithValue("Result", cId);
                    exists = (int)cmdUser.ExecuteScalar() > 0;

                
                    if (exists)
                    {
                        
                    }
                    else
                    {
                        dataExtraCourse.Rows.Add(false, name, code, unit, level, cId);
                    }
                }
                
            }
            Con.Close();

            //dataExtraCourse.SelectionMode = DataGridViewSelectionMode.None;

        }

        void Fill_Session()
        {
            Con.Open();

            string SessionQuery = "SELECT * FROM [Session] where Semester = 'Harmattan' ORDER BY NAME";
            SqlCommand cmdSess = new SqlCommand(SessionQuery, Con);
            SqlDataReader readerSession = cmdSess.ExecuteReader();

            var dt = new DataTable();

            dt.Load(readerSession);

            try
            {
                comboBox2.DataSource = null;
                comboBox3.DataSource = null;
                comboBox2.Items.Clear();
                comboBox3.Items.Clear();



                comboBox2.DisplayMember = "Name";
                comboBox2.ValueMember = "Id";
                comboBox2.DataSource = dt;

                comboBox3.DisplayMember = "Name";
                comboBox3.ValueMember = "Id";
                comboBox3.DataSource = dt;


                comboBox2.SelectedValue = StudentYearId;
            }
            catch (Exception ex)
            {
                Con.Close();
                MessageBox.Show(ex.Message);
            }
            Con.Close();
        }

        void Fill_Program()
        {


            Con.Open();

            string SessionQuery = "SELECT * FROM [Program]";
            SqlCommand cmdSess = new SqlCommand(SessionQuery, Con);
            SqlDataReader readerSession = cmdSess.ExecuteReader();

            var dt = new DataTable();

            dt.Load(readerSession);

            try
            {
                comboBox6.DataSource = null;
                comboBox6.Items.Clear();



                comboBox6.DisplayMember = "Name";
                comboBox6.ValueMember = "Id";
                comboBox6.DataSource = dt;


                comboBox6.SelectedValue = StudentProgramId;
            }
            catch (Exception ex)
            {
                Con.Close();
                MessageBox.Show(ex.Message);
            }
            Con.Close();
        }

        void Load_Student_Details(string studentId)
        {
            Con.Open();

            string studentData = "SELECT * FROM [Student] where Matric = '" + studentId + "'";
            SqlCommand cmdstudentData = new SqlCommand(studentData, Con);
            SqlDataReader readerstudentData = cmdstudentData.ExecuteReader();
            if (readerstudentData.Read())
            {
                studentTableId = readerstudentData.GetInt32("Id");
                StudentName = readerstudentData.GetString("Lastname") + ", " + readerstudentData.GetString("Firstname") + " " + readerstudentData.GetString("Middlename");
                txtMatric.Text = studentId;
                label17.Text = StudentName;
                txtSurname.Text = readerstudentData.GetString("Lastname");
                textFirstname.Text = readerstudentData.GetString("Firstname");
                textOthername.Text = readerstudentData.GetString("Middlename");

                label5.Text = readerstudentData.GetString("Matric");


                StudentGender = readerstudentData.GetString("Gender");
                label6.Text = StudentGender;
                comboGender.Items.Clear();
                if (StudentGender == "Male")
                {
                    comboGender.Items.Add("Male");
                    comboGender.SelectedItem = "Male";
                    comboGender.Items.Add("Female");
                }
                else if (StudentGender == "Female")
                {
                    comboGender.Items.Add("Male");
                    comboGender.Items.Add("Female");
                    comboGender.SelectedItem = "Female";
                }
                else
                {
                    comboGender.Items.Add("Male");
                    comboGender.Items.Add("Female");
                    comboGender.SelectedItem = null;
                }

                StudentYearId = readerstudentData.GetString("Year");
                string studentYearData = "SELECT * FROM [Session] where Id = '" + readerstudentData.GetString("Year") + "'";
                SqlCommand cmdstudentYearData = new SqlCommand(studentYearData, Con);
                SqlDataReader readerstudentYearData = cmdstudentYearData.ExecuteReader();
                if (readerstudentYearData.Read())
                {
                    studentStart = readerstudentYearData.GetInt32("Start");
                    StudentYear = readerstudentYearData.GetString("Name");
                    label37.Text = StudentYear;
                }

                StudentProgramId = readerstudentData.GetString("Program");
                string studentProgramData = "SELECT * FROM [Program] where Id = '" + readerstudentData.GetString("Program") + "'";
                SqlCommand cmdstudentProgramData = new SqlCommand(studentProgramData, Con);
                SqlDataReader readerstudentProgramData = cmdstudentProgramData.ExecuteReader();
                if (readerstudentProgramData.Read())
                {
                    StudentProgram = readerstudentProgramData.GetString("Name");
                    label39.Text = StudentProgram;
                }


            }

            Con.Close();
        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void btnEditStd_Click(object sender, EventArgs e)
        {
            panel2.Show();
            pnlStd.Show();
        }

        private void frmStudentData_Load(object sender, EventArgs e)
        {
            label30.Hide();
            label34.Hide();
            label31.Hide();
            label35.Hide();
            label33.Hide();
            label36.Hide();
            button2.Hide();
            bunifuCards4.Hide();

            iconButton3.Hide();
            btnAddCourses.Hide();
            btnAddStd.Hide();
            button1.Hide();
            btnSave.Hide();
            btnPrtRes.Hide();

            Load_Student_Details(studentId);

            LoadStdTable();

            Fill_Session();
            Fill_Program();

            LoadResultData();

            panel2.Hide();
            pnlStd.Hide();
            dataCourses.Hide();
            iconButton3.Hide();
        }

        private void label25_Click(object sender, EventArgs e)
        {
            panel2.Hide();
            pnlStd.Hide();
        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            label30.Hide();
            label34.Hide();
            label31.Hide();
            label35.Hide();
            label33.Hide();
            label36.Hide();
            button2.Hide();

            dataStd.Show();
            dataCourses.Hide();
            btnSave.Hide();
            btnPrtRes.Hide();
            bunifuCards4.Hide();

            LoadResultData();
        }

        private void dataStd_DoubleClick(object sender, EventArgs e)
        {

            //comboBox3.SelectedItem = this.dataStd.CurrentRow.Cells[0].Value.ToString();

            comboBox4.SelectedItem = this.dataStd.CurrentRow.Cells[1].Value.ToString().Substring(5, 1);
            txtSelectedLvl.Text = this.dataStd.CurrentRow.Cells[1].Value.ToString().Substring(5, 1);

            comboBox5.SelectedItem = this.dataStd.CurrentRow.Cells[2].Value.ToString().Substring(0, 1);
            txtSelectedSemester.Text = this.dataStd.CurrentRow.Cells[2].Value.ToString().Substring(0, 1);

            Con.Open();
            string selectSessionQuery = "SELECT * FROM [Session] where Name = '"+ this.dataStd.CurrentRow.Cells[0].Value.ToString() + "'";
            SqlCommand cmdselectSess = new SqlCommand(selectSessionQuery, Con);
            SqlDataReader readerselectSession = cmdselectSess.ExecuteReader();

            if (readerselectSession.Read())
            {
                txtSelectedSession.Text = readerselectSession.GetInt32("Id").ToString();
                comboBox3.SelectedValue = readerselectSession.GetInt32("Id");
            }
            Con.Close();

            

            dataCourses.Rows.Clear();

            label30.Show();
            label34.Show();
            label31.Show();
            label35.Show();
            label33.Show();
            label36.Show();

            dataStd.Hide();
            dataCourses.Show();
            btnSave.Show();
            btnPrtRes.Show();
            button2.Show();

            string level = this.dataStd.CurrentRow.Cells[1].Value.ToString().Substring(5, 1);
            string semester = this.dataStd.CurrentRow.Cells[2].Value.ToString().Substring(0, 1);
            Int32 session = Int32.Parse(txtSelectedSession.Text);




            LoadCourseTable(session, semester);

            LoadSemesterTotal(session, semester, StudentProgramId);

            label52.Text = sCarryOvers;
        }

        private void iconButton4_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                Con.Open();

                if ((txtMatric.Text == "") || (txtSurname.Text == "") || (textFirstname.Text == "") || (comboGender.SelectedItem == null) || (comboBox2.SelectedItem == null) || (comboBox6.SelectedItem == null))
                {
                    MessageBox.Show("You must fill all required field");
                }
                else
                {
                    if (studentId == txtMatric.Text)
                    {
                        string queryStudent = "UPDATE Student set matric = '" + txtMatric.Text + "', Lastname = '" + txtSurname.Text + "', Firstname = '" + textFirstname.Text + "', Middlename = '" + textOthername.Text + "',Gender = '" + comboGender.Text + "',Program = '" + comboBox6.GetItemText(comboBox6.SelectedValue) + "',Year = '" + comboBox2.GetItemText(comboBox2.SelectedValue) + "' where Matric = '" + studentId + "'";
                        SqlCommand cmdStudent = new SqlCommand(queryStudent, Con);
                        cmdStudent.ExecuteNonQuery();
                        MessageBox.Show("Student Details Updated Successfully");

                        studentId = txtMatric.Text;
                    }
                    else
                    {
                        bool exists = false;
                        string chechuser = "SELECT count(*) FROM [Student] where Matric='" + txtMatric.Text + "'";

                        SqlCommand cmdUser = new SqlCommand(chechuser, Con);
                        cmdUser.Parameters.AddWithValue("Matric", txtMatric.Text);
                        exists = (int)cmdUser.ExecuteScalar() > 0;

                        if (exists)
                        {
                            MessageBox.Show("Student ID Already Assigned to a Student");
                        }
                        else
                        {
                            string queryStudent = "UPDATE Student set matric = '" + txtMatric.Text + "', Lastname = '" + txtSurname.Text + "', Firstname = '" + textFirstname.Text + "', Middlename = '" + textOthername.Text + "',Gender = '" + comboGender.Text + "',Program = '" + comboBox6.GetItemText(comboBox6.SelectedValue) + "',Year = '" + comboBox2.GetItemText(comboBox2.SelectedValue) + "' where Matric = '" + studentId + "'";
                            SqlCommand cmdStudent = new SqlCommand(queryStudent, Con);
                            cmdStudent.ExecuteNonQuery();
                            MessageBox.Show("Student Details Updated Successfully");

                            studentId = txtMatric.Text;

                        }
                    }
                    

                    
                }
                Con.Close();

                Load_Student_Details(studentId);
                LoadStdTable();
            }
            catch (Exception ex)
            {
                Con.Close();
                MessageBox.Show(ex.Message);
            }
        }

        private void label32_Click(object sender, EventArgs e)
        {

        }

        private void dataCourses_DoubleClick(object sender, EventArgs e)
        {
            //dataCourses.Hide();
        }

        private void dataCourses_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                Int32 rowScore;
                string gradeScore = "";

                if (dataCourses.CurrentRow != null)
                {
                    if (dataCourses.CurrentCell.Value == null)
                    {
                        gradeScore = "AR";
                    }
                    else
                    {
                        if (dataCourses.CurrentRow.Cells[4].Value.ToString() != "")
                        {
                            rowScore = Int32.Parse(dataCourses.CurrentRow.Cells[4].Value.ToString());
                            gradeScore = "";

                            if ((rowScore >= 70) && (rowScore <= 100))
                            {
                                gradeScore = "A";
                            }
                            else if ((rowScore >= 60) && (rowScore < 70))
                            {
                                gradeScore = "B";
                            }
                            else if ((rowScore >= 50) && (rowScore < 60))
                            {
                                gradeScore = "C";
                            }
                            else if ((rowScore >= 45) && (rowScore < 50))
                            {
                                gradeScore = "D";
                            }
                            else if ((rowScore >= 40) && (rowScore < 45))
                            {
                                gradeScore = "E";
                            }
                            else
                            {
                                gradeScore = "F";
                            }

                            if (rowScore < 0)
                            {
                                gradeScore = "AR";
                            }
                        }
                    }

                    dataCourses.CurrentRow.Cells[5].Value = gradeScore;


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Int32 rowCount = Int32.Parse(dataCourses.RowCount.ToString());
                Int32 countInsert = 0;
                Int32 countUpdate = 0;

                Con.Open();
                for (Int32 i = 0; i < rowCount; i++)
                {
                    //string rowShow = dataCourses.Rows[i].Cells[6].Value.ToString();
                    string checkR = dataCourses.Rows[i].Cells[0].Value.ToString();
                    if (checkR == "True")
                    {
                        Int32 rowCourseId = Int32.Parse(dataCourses.Rows[i].Cells[6].Value.ToString());
                        Int32 rowCourseScore;

                        if ((dataCourses.Rows[i].Cells[4].Value == null) || (dataCourses.Rows[i].Cells[4].Value == ""))
                        {
                            rowCourseScore = -1;
                        }
                        else
                        {
                            rowCourseScore = Int32.Parse(dataCourses.Rows[i].Cells[4].Value.ToString());
                        }
                        

                        bool exists = false;

                        string chechuser = "SELECT count(*) FROM [Result] where Student = '" + studentTableId + "'and Course = '" + rowCourseId + "'";

                        SqlCommand cmdUser = new SqlCommand(chechuser, Con);
                        cmdUser.Parameters.AddWithValue("Result", rowCourseId);
                        exists = (int)cmdUser.ExecuteScalar() > 0;



                        if (exists)
                        {

                            string queryStudent = "UPDATE Result set Score = '" + rowCourseScore + "' where Student = '" + studentTableId + "'and Course = '" + rowCourseId + "'";
                            SqlCommand cmdStudent = new SqlCommand(queryStudent, Con);
                            cmdStudent.ExecuteNonQuery();

                            countUpdate++;
                        }
                        else
                        {
                            String queryCourse = "insert into Result(Student,Course,Score) values('" + studentTableId + "','" + rowCourseId + "','" + rowCourseScore + "')";
                            SqlCommand cmdCourse = new SqlCommand(queryCourse, Con);
                            cmdCourse.ExecuteNonQuery();

                            countInsert++;
                        }


                    }

                }

                Con.Close();



                MessageBox.Show(countInsert.ToString() + " Result was Inserted and " + countUpdate.ToString() + " Result was Updated");

            }
            catch (Exception ex)
            {
                Con.Close();
                MessageBox.Show(ex.Message);
            }

            LoadResultData();

            LoadCourseTable(Int32.Parse(txtSelectedSession.Text), txtSelectedSemester.Text);

            
        }

        private void label39_Click(object sender, EventArgs e)
        {
            string nameP = StudentProgram;
            frmProgramData programPage = new frmProgramData();
            programPage.label17.Text = nameP;

            programPage.programName = StudentProgram;

            programPage.Show();
        }

        private void pnlStd_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnPrtRes_Click(object sender, EventArgs e)
        {
            Con.Open();
            frmResult resultPage = new frmResult();

            string selectSessionQuery = "SELECT * FROM [UserTbl]";
            SqlCommand cmdselectSess = new SqlCommand(selectSessionQuery, Con);
            SqlDataReader readerselectSession = cmdselectSess.ExecuteReader();

            if (readerselectSession.Read())
            {
                resultPage.txtDpt.Text = readerselectSession.GetString("Header").ToUpper() + "\n(" + StudentProgram.ToUpper()+")";
            }

            
            resultPage.labelName.Text = StudentName;
            resultPage.labelMatric.Text = studentId;
            
            string selectQuery = "SELECT * FROM [Session] where Id = '" + txtSelectedSession.Text + "'";
            SqlCommand cmdselect = new SqlCommand(selectQuery, Con);
            SqlDataReader readerselect = cmdselect.ExecuteReader();

            if (readerselect.Read())
            {
                resultPage.labelSession.Text = readerselect.GetString("Name");
            }

            resultPage.labelSemester.Text = txtSelectedSemester.Text;

            if (lvlMark == "Undergraduate")
            {
                resultPage.labelLevel.Text = txtSelectedLvl.Text + "00 LEVEL";
            }

            if (lvlMark == "Post Graduate")
            {
                resultPage.labelLevel.Text = Int32.Parse(txtSelectedLvl.Text) + 6 + "00 LEVEL";
            }

            if (lvlMark == "Masters")
            {
                resultPage.labelLevel.Text = "Year " + txtSelectedLvl.Text;
            }

            if (lvlMark == "PHD")
            {
                resultPage.labelLevel.Text = "Year " + txtSelectedLvl.Text;
            }

            if (lvlMark == "ND")
            {
                resultPage.labelLevel.Text = "ND " + txtSelectedLvl.Text;
            }

            if (lvlMark == "HND")
            {
                resultPage.labelLevel.Text = "HND " + txtSelectedLvl.Text;
            }

            if (lvlMark == "Year")
            {
                resultPage.labelLevel.Text = "Year " + txtSelectedLvl.Text;
            }


            resultPage.labelProgram.Text = StudentProgram;
            

            resultPage.labelSemesterTU.Text = semesterCTU.ToString();
            resultPage.labelSesesterTP.Text = semesterCTP.ToString();
            resultPage.labelSemesterTotaling.Text = semesterTotalGrading.ToString() + "%";
            resultPage.labelCTU.Text = sCTU.ToString();
            resultPage.labelCTP.Text = sCTP.ToString();
            resultPage.labelCTotaling.Text = sTotalGrading.ToString() +"%";

            DateTime currentTime = DateTime.Now;

            resultPage.labelTime.Text = currentTime.ToString("hh:mm:ss tt");

            DateTime currentDate = DateTime.Today;

            resultPage.labelDate.Text = currentDate.ToString("dd/MM/yyyy");


            Int32 rowCount = Int32.Parse(dataCourses.RowCount.ToString());
            
            for (Int32 i = 0; i < rowCount; i++)
            {
                bool exists = false;

                Int32 rowCourseId = Int32.Parse(dataCourses.Rows[i].Cells[6].Value.ToString());
                string checkR = dataCourses.Rows[i].Cells[0].Value.ToString();

                
                    string chechuser = "SELECT count(*) FROM [Result] where Student = '" + studentTableId + "'and Course = '" + rowCourseId + "'";

                    SqlCommand cmdUser = new SqlCommand(chechuser, Con);
                    cmdUser.Parameters.AddWithValue("Result", rowCourseId);
                    exists = (int)cmdUser.ExecuteScalar() > 0;

                    if (exists)
                    {
                    string courseCode = dataCourses.Rows[i].Cells[1].Value.ToString();
                    string courseTitle = dataCourses.Rows[i].Cells[2].Value.ToString();
                    string courseUnit = dataCourses.Rows[i].Cells[3].Value.ToString();
                    string courseScore = dataCourses.Rows[i].Cells[4].Value.ToString();
                    string courseGrade = dataCourses.Rows[i].Cells[5].Value.ToString();

                    resultPage.dataGridResult.Rows.Add(courseCode, courseTitle, courseUnit, courseScore, courseGrade);
                    }
                
                //resultPage.dataGridResult.Rows.Add("", "", "", "", "");
            }
            Con.Close();
            if (sPassAll > 0)
            {
                resultPage.labelRmk.Text = "REQUIRED COURSE (S) OUTSTANDING";

                resultPage.labelOS.Text = sCarryOvers;

                resultPage.labelOS.Show();
                resultPage.label13.Show();
                resultPage.label15.Show();
            }
            else
            {
                resultPage.labelRmk.Text = "ALL REQUIRED COURSES PASSED";

                resultPage.labelOS.Hide();
                resultPage.label13.Hide();
                resultPage.label15.Hide();
            }
            
            resultPage.dataGridResult.AutoSize = true;
            resultPage.dataGridResult.Refresh();
            resultPage.Show();

            /*
            DGVPrinter printer = new DGVPrinter();

            printer.Title = "STUDENT RESULT PRINTER";
            printer.SubTitle = StudentName;
            printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
            printer.PageNumbers = false;
            printer.PageNumberInHeader = false;
            printer.PorportionalColumns = false;
            printer.HeaderCellAlignment = StringAlignment.Near;
           

            printer.Footer = "Asl Tech";
            printer.FooterSpacing = 15;
            printer.PrintPreviewDataGridView(dataCourses);
            */
            //printer.PrintDataGridView(dataCourses);


        }

        private void dataCourses_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataCourses.Columns[e.ColumnIndex].Name == "Column7")
            {
                DialogResult result = MessageBox.Show("Are you sure you want to delete the Result", "Delete Result", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    string courseRowId = this.dataCourses.CurrentRow.Cells[6].Value.ToString();
                   
                    Con.Open();
                    string queryDel = "DELETE FROM Result  WHERE Student = '" + studentTableId + "' AND Course = '" + courseRowId + "'";
                    SqlCommand cmdDel = new SqlCommand(queryDel, Con);
                    cmdDel.ExecuteNonQuery();
                    Con.Close();
                    

                    MessageBox.Show("Result deleted successfuly");
                    //MessageBox.Show(courseRowId);

                    LoadResultData();

                    LoadCourseTable(Int32.Parse(txtSelectedSession.Text), txtSelectedSemester.Text);
                }
                else if (result == DialogResult.No)
                {
                    result = DialogResult.Cancel;
                }
            }
        }

        private void deleteCourse_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show("Are you sure you want to delete this student record? \n By deleting the Student record, all the result belonging to the student will also be deleted", "Delete Student record", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    Con.Open();
                    string queryDel = "DELETE FROM Student WHERE Id = '" + studentTableId + "'";
                    SqlCommand cmdDel = new SqlCommand(queryDel, Con);
                    cmdDel.ExecuteNonQuery();

                    string queryDel2 = "DELETE FROM Result WHERE Student = '" + studentTableId + "'";
                    SqlCommand cmdDel2 = new SqlCommand(queryDel2, Con);
                    cmdDel2.ExecuteNonQuery();

                    Con.Close();

                    MessageBox.Show("Student record successfuly deleted");

                    this.Hide();
                }
                else if (result == DialogResult.No)
                {
                    result = DialogResult.Cancel;
                }

                
            }
            catch (Exception ex)
            {
                Con.Close();
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bunifuCards4.Show();
            //MessageBox.Show(txtSelectedLvl.Text + " " + txtSelectedSemester.Text);
            LoadExtraCourse(txtSelectedSemester.Text, txtSelectedLvl.Text);
        }

        private void label53_Click(object sender, EventArgs e)
        {
            bunifuCards4.Hide();
        }

        private void label54_Click(object sender, EventArgs e)
        {
            try
            {
                Int32 rowCountJ = Int32.Parse(dataExtraCourse.RowCount.ToString());
                Int32 countInsert = 0;
                //MessageBox.Show(rowCountJ.ToString());
                Con.Open();
                for (Int32 j = 0; j < rowCountJ; j++)
                {
                    //string rowShow = dataCourses.Rows[i].Cells[6].Value.ToString();
                    string checkR = dataExtraCourse.Rows[j].Cells[0].Value.ToString();
                    //MessageBox.Show(checkR);
                    if (checkR == "True")
                    {
                        
                        Int32 rowCourseId = Int32.Parse(dataExtraCourse.Rows[j].Cells[5].Value.ToString());

                        bool exists = false;

                        string chechuser = "SELECT count(*) FROM [Result] where Student = '" + studentTableId + "'and Course = '" + rowCourseId + "'";

                        SqlCommand cmdUser = new SqlCommand(chechuser, Con);
                        cmdUser.Parameters.AddWithValue("Result", rowCourseId);
                        exists = (int)cmdUser.ExecuteScalar() > 0;

                        if (exists)
                        {
                        }
                        else
                        {
                            String queryCourse = "insert into Result(Student,Course,Score,Session) values('" + studentTableId + "','" + rowCourseId + "','-1', '"+ txtSelectedSession.Text+"')";
                            SqlCommand cmdCourse = new SqlCommand(queryCourse, Con);
                            cmdCourse.ExecuteNonQuery();

                            countInsert++;
                        }


                    }
                }

                Con.Close();



                MessageBox.Show(countInsert.ToString() + " Result was Added ");

            }
            catch (Exception ex)
            {
                Con.Close();
                MessageBox.Show(ex.Message);
            }

            LoadResultData();

            LoadCourseTable(Int32.Parse(txtSelectedSession.Text), txtSelectedSemester.Text);

            LoadExtraCourse(txtSelectedSemester.Text, txtSelectedLvl.Text);
        }

        private void dataExtraCourse_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataExtraCourse_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataExtraCourse_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (dataExtraCourse.Columns[e.ColumnIndex].Name == "dataGridViewCheckBoxColumn1")
            {
                if (this.dataExtraCourse.CurrentRow.Cells[0].Value.ToString() == "False")
                {
                    this.dataExtraCourse.CurrentRow.Cells[0].Value = true;
                }
                else
                {
                    this.dataExtraCourse.CurrentRow.Cells[0].Value = false;
                }
            }
        }
    }
}
