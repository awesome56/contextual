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
using Contextual.Properties;
using System.Security.Cryptography;

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
        string programGrading = "";
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

        string printRMK = "";

        private PrintDocument printDocument1 = new PrintDocument();
        private int currentPageIndex = 0;
        private bool isPrinting = false;
        private int maxPageIndex = 0;
        private float totalContentHeight; // Add this variable to keep track of the total content height
        private float currentYPosition;  // Add this variable to keep track of the current vertical position


        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + GlobalVariable.Globals.databasePath + ";Integrated Security=True;MultipleActiveResultSets=True;Connect Timeout=30");

        //Extra Courses
        void LoadResultData()
        {
            Int32 noOfExtra = 0;

            CTU = 0;
            CTP = 0;
            TotalGrading = 0;
            label52.Text = "";
            string carryOvers = "";
            Int32 scores;
            if (Con.State == ConnectionState.Open)
            {
                Con.Close();
            }
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

                    Int32 gradePoint = 0;

                    if ((scores >= 70) && (scores <= 100))
                    {
                        gradePoint = 5;
                    }
                    else if ((scores >= 60) && (scores < 70))
                    {
                        gradePoint = 4;
                    }
                    else if ((scores >= 50) && (scores < 60))
                    {
                        gradePoint = 3;
                    }
                    else if ((scores >= 45) && (scores < 50))
                    {
                        gradePoint = 2;
                    }
                    else if ((scores >= 40) && (scores < 45))
                    {
                        gradePoint = 1;
                    }
                    else
                    {
                        gradePoint = 0;
                    }


                    CTU = Int32.Parse(unit) + CTU;

                    if (programGrading == "Percentage Grading")
                    {
                        CTP = (Int32.Parse(unit) * scores) + CTP;
                    }
                    else
                    {
                        CTP = (Int32.Parse(unit) * gradePoint) + CTP;
                    }
                }

                if ((scores < 40) && (scores >= 0))
                {
                    bool exists = false;

                    string checkDataQ = "SELECT COUNT(*) FROM [Result] WHERE student = @Student AND course = @Course AND score > 39";

                    using (SqlCommand cmdcheckDataQ = new SqlCommand(checkDataQ, Con))
                    {
                        cmdcheckDataQ.Parameters.AddWithValue("@Student", studentTableId);
                        cmdcheckDataQ.Parameters.AddWithValue("@Course", readerProgData.GetInt32("Course"));

                        exists = (int)cmdcheckDataQ.ExecuteScalar() > 0;
                    }

                    if (exists)
                    {

                    }
                    else
                    {
                        string loadData = "SELECT * FROM [Course] where Id = '" + readerProgData.GetInt32("Course") + "'";
                        SqlCommand cmdData = new SqlCommand(loadData, Con);
                        SqlDataReader readerData = cmdData.ExecuteReader();
                        if (readerData.Read())
                        {
                            string newEntry = readerData.GetString("Code") + " (" + readerData.GetString("Unit") + ") REP";

                            string newEntry2 = readerData.GetString("Code") + " (" + readerData.GetString("Unit") + ") A.R";

                            // Check if newEntry2 exists in label52.Text
                            if (label52.Text.Contains(newEntry2))
                            {
                                // Replace newEntry2 with newEntry
                                label52.Text = label52.Text.Replace(newEntry2, newEntry);
                            }
                            else
                            {
                                // If newEntry2 doesn't exist, add newEntry
                                if (!label52.Text.Contains(newEntry))
                                {
                                    passAll = passAll + 1;

                                    label52.Text = label52.Text + "     " + newEntry;
                                }
                            }
                        }
                    }
                    
                }

                if (scores < 0)
                {
                    bool exists = false;

                    string checkDataQ = "SELECT COUNT(*) FROM [Result] WHERE student = @Student AND course = @Course AND score > 39";

                    using (SqlCommand cmdcheckDataQ = new SqlCommand(checkDataQ, Con))
                    {
                        cmdcheckDataQ.Parameters.AddWithValue("@Student", studentTableId);
                        cmdcheckDataQ.Parameters.AddWithValue("@Course", readerProgData.GetInt32("Course"));

                        exists = (int)cmdcheckDataQ.ExecuteScalar() > 0;
                    }

                    if (exists)
                    {

                    }
                    else
                    {
                        string loadData2 = "SELECT * FROM [Course] where Id = '" + readerProgData.GetInt32("Course") + "'";
                        SqlCommand cmdData2 = new SqlCommand(loadData2, Con);
                        SqlDataReader readerData2 = cmdData2.ExecuteReader();
                        if (readerData2.Read())
                        {
                            string newEntry = readerData2.GetString("Code") + " (" + readerData2.GetString("Unit") + ") A.R";

                            string newEntry2 = readerData2.GetString("Code") + " (" + readerData2.GetString("Unit") + ") REP";

                            // Check if newEntry2 exists in label52.Text
                            if (label52.Text.Contains(newEntry2))
                            {
                                // Replace newEntry2 with newEntry
                                label52.Text = label52.Text.Replace(newEntry2, newEntry);
                            }
                            else
                            {
                                // If newEntry2 doesn't exist, add newEntry
                                if (!label52.Text.Contains(newEntry))
                                {
                                    passAll = passAll + 1;

                                    label52.Text = label52.Text + "     " + newEntry;
                                }
                            }
                        }
                    }
                }
            }
                Con.Close();

            label44.Text = "Cumulative Total Units =";
            label46.Text = "Cumulative Total Points =";

            if (programGrading == "Percentage Grading")
            {
                label48.Text = "Cumulative Percentage Total =";
            }
            else
            {
                label48.Text = "Cumulative G.P.A =";
            }


            txtTU.Text = "";
            txtTP.Text = "";
            txtTotal.Text = "";

            //if ((CTU != 0) && (CTP != 0))
            //{
                //Decimal stp = (Decimal) CTP;
                //Decimal stu = (Decimal) CTU;
                //TotalGrading = Math.Round((Decimal)(stp / stu), 2);



                //txtTU.Text = CTU.ToString();
                //txtTP.Text = CTP.ToString();
                //txtTotal.Text = String.Format("{0:0.00}",TotalGrading.ToString()) + "%";
                //if (programGrading == "Percentage Grading")
                //{
                    //txtTotal.Text = TotalGrading.ToString() + "%";
                //}
                //else
                //{
                    //txtTotal.Text = TotalGrading.ToString();
                //}

            //}
            if ((CTU != 0) && (CTP != 0))
            {
                Decimal stp = (Decimal)CTP;
                Decimal stu = (Decimal)CTU;
                TotalGrading = Math.Round((Decimal)(stp / stu), 2);

                txtTU.Text = CTU.ToString();
                txtTP.Text = CTP.ToString();

                if (programGrading == "Percentage Grading")
                {
                    txtTotal.Text = TotalGrading.ToString() + "%";
                }
                else
                {
                    txtTotal.Text = TotalGrading.ToString("N2");
                }

            }
            else if (CTP != 0)
            {
                Decimal stp = 0;
                Decimal stu = (Decimal)semesterCTU;
                TotalGrading = 0;

                txtTU.Text = CTU.ToString();
                txtTP.Text = CTP.ToString();

                if (programGrading == "Percentage Grading")
                {
                    txtTotal.Text = TotalGrading.ToString() + "%";
                }
                else
                {
                    txtTotal.Text = TotalGrading.ToString("N2");
                }
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
            string getSessionName = "";

            string queryStartsession = "SELECT * FROM [Session] where Id = '" + getSession + "'";
            SqlCommand cmdStartsession = new SqlCommand(queryStartsession, Con);
            SqlDataReader readerStartsession = cmdStartsession.ExecuteReader();

            if (readerStartsession.Read())
            {
                getSessionStart = readerStartsession.GetInt32("Start");
                getSessionName = readerStartsession.GetString("Name");
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
                                string sSessionName = "";
                                string loadResultSession = $"SELECT * FROM [Session] where Id = '{readerSresult.GetInt32("Session")}'";
                                SqlCommand cmdResultSession = new SqlCommand(loadResultSession, Con);
                                SqlDataReader readerResultSession = cmdResultSession.ExecuteReader();
                                if (readerResultSession.Read())
                                {
                                    // Session of result currently being looped
                                    sSessionName = readerResultSession.GetString("Name");
                                }

                                if ((getSessionName == sSessionName) && (readerCourse.GetString("Semester") == "R"))
                                {
                                    //don't fetch second semester for current session
                                }
                                else
                                {
                                    unit = readerCourse.GetString("Unit");
                                    scores = readerSresult.GetInt32("Score");

                                    if (scores >= 0)
                                    {
                                        Int32 gradePoint = 0;

                                        if ((scores >= 70) && (scores <= 100))
                                        {
                                            gradePoint = 5;
                                        }
                                        else if ((scores >= 60) && (scores < 70))
                                        {
                                            gradePoint = 4;
                                        }
                                        else if ((scores >= 50) && (scores < 60))
                                        {
                                            gradePoint = 3;
                                        }
                                        else if ((scores >= 45) && (scores < 50))
                                        {
                                            gradePoint = 2;
                                        }
                                        else if ((scores >= 40) && (scores < 45))
                                        {
                                            gradePoint = 1;
                                        }
                                        else
                                        {
                                            gradePoint = 0;
                                        }


                                        sCTU = Int32.Parse(unit) + sCTU;

                                        if (programGrading == "Percentage Grading")
                                        {
                                            sCTP = (Int32.Parse(unit) * scores) + sCTP;
                                        }
                                        else
                                        {
                                            sCTP = (Int32.Parse(unit) * gradePoint) + sCTP;
                                        }
                                    }

                                    if ((scores < 40) && (scores >= 0))
                                    {
                                        int exists = 0;

                                        string checkDataQ = "SELECT Session FROM [Result] WHERE student = @Student AND course = @Course AND score > 39";

                                        using (SqlCommand cmdcheckDataQ = new SqlCommand(checkDataQ, Con))
                                        {
                                            cmdcheckDataQ.Parameters.AddWithValue("@Student", studentTableId);
                                            cmdcheckDataQ.Parameters.AddWithValue("@Course", readerSresult.GetInt32("Course"));
                                            SqlDataReader readercheckDataQ = cmdcheckDataQ.ExecuteReader();
                                            while (readercheckDataQ.Read())
                                            {
                                                int dataQ = readercheckDataQ.GetInt32("Session");
                                                if (getSessionStart >= GlobalVariable.Globals.getSessionStart(dataQ, Con))
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
                                            cmdcheckDataQ.Parameters.AddWithValue("@Student", studentTableId);
                                            cmdcheckDataQ.Parameters.AddWithValue("@Course", readerSresult.GetInt32("Course"));
                                            SqlDataReader readercheckDataQ = cmdcheckDataQ.ExecuteReader();
                                            while (readercheckDataQ.Read())
                                            {
                                                int dataQ = readercheckDataQ.GetInt32("Session");
                                                if (getSessionStart >= GlobalVariable.Globals.getSessionStart(dataQ, Con))
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

                                if (scores >= 0)
                                {
                                    Int32 gradePoint = 0;

                                    if ((scores >= 70) && (scores <= 100))
                                    {
                                        gradePoint = 5;
                                    }
                                    else if ((scores >= 60) && (scores < 70))
                                    {
                                        gradePoint = 4;
                                    }
                                    else if ((scores >= 50) && (scores < 60))
                                    {
                                        gradePoint = 3;
                                    }
                                    else if ((scores >= 45) && (scores < 50))
                                    {
                                        gradePoint = 2;
                                    }
                                    else if ((scores >= 40) && (scores < 45))
                                    {
                                        gradePoint = 1;
                                    }
                                    else
                                    {
                                        gradePoint = 0;
                                    }


                                    sCTU = Int32.Parse(unit) + sCTU;

                                    if (programGrading == "Percentage Grading")
                                    {
                                        sCTP = (Int32.Parse(unit) * scores) + sCTP;
                                    }
                                    else
                                    {
                                        sCTP = (Int32.Parse(unit) * gradePoint) + sCTP;
                                    }
                                    
                                    
                                }

                                if ((scores < 40) && (scores >= 0))
                                {
                                    //carryOvers = readerCourse.GetString("Code") + " (" + readerCourse.GetString("Unit") + ") REP";

                                    //sPassAll = sPassAll + 1;


                                    //sCarryOvers = sCarryOvers + "     " + carryOvers;
                                    bool exists = false;

                                    string checkDataQ = "SELECT COUNT(*) FROM [Result] WHERE student = @Student AND course = @Course AND score > 39";

                                    using (SqlCommand cmdcheckDataQ = new SqlCommand(checkDataQ, Con))
                                    {
                                        cmdcheckDataQ.Parameters.AddWithValue("@Student", studentTableId);
                                        cmdcheckDataQ.Parameters.AddWithValue("@Course", readerSresult.GetInt32("Course"));

                                        exists = (int)cmdcheckDataQ.ExecuteScalar() > 0;
                                    }

                                    if (exists)
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
                                    //carryOvers = readerCourse.GetString("Code") + " (" + readerCourse.GetString("Unit") + ") A.R";

                                    //sPassAll = sPassAll + 1;


                                    //sCarryOvers = sCarryOvers + "     " + carryOvers;

                                    bool exists = false;

                                    string checkDataQ = "SELECT COUNT(*) FROM [Result] WHERE student = @Student AND course = @Course AND score > 39";

                                    using (SqlCommand cmdcheckDataQ = new SqlCommand(checkDataQ, Con))
                                    {
                                        cmdcheckDataQ.Parameters.AddWithValue("@Student", studentTableId);
                                        cmdcheckDataQ.Parameters.AddWithValue("@Course", readerSresult.GetInt32("Course"));

                                        exists = (int)cmdcheckDataQ.ExecuteScalar() > 0;
                                    }

                                    if (exists)
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

            //if ((sCTU != 0) && (sCTP != 0))
            //{
                //Decimal stp = (Decimal)sCTP;
                //Decimal stu = (Decimal)sCTU;
                //sTotalGrading = Math.Round((Decimal)(stp / stu), 2);



                //label34.Text = sCTU.ToString();
                //label35.Text = sCTP.ToString();
                //txtTotal.Text = String.Format("{0:0.00}",TotalGrading.ToString()) + "%";
                //if (programGrading == "Percentage Grading")
                //{
                   // label36.Text = sTotalGrading.ToString() + "%";
                //}
                //else
                //{
                   // label36.Text = sTotalGrading.ToString();
                //}

            //}

            if ((sCTU != 0) && (sCTP != 0))
            {
                Decimal stp = (Decimal)sCTP;
                Decimal stu = (Decimal)sCTU;
                sTotalGrading = Math.Round((Decimal)(stp / stu), 2);

                label34.Text = sCTU.ToString();
                label35.Text = sCTP.ToString();

                if (programGrading == "Percentage Grading")
                {
                    label36.Text = sTotalGrading.ToString() + "%";
                }
                else
                {
                    label36.Text = sTotalGrading.ToString("N2");

                }

            }
            else if (sCTP != 0)
            {
                Decimal stp = 0;
                Decimal stu = (Decimal)sCTU;
                TotalGrading = 0;

                label34.Text = sCTU.ToString();
                label35.Text = sCTP.ToString();

                if (programGrading == "Percentage Grading")
                {
                    label36.Text = sTotalGrading.ToString() + "%";
                }
                else
                {
                    label36.Text = sTotalGrading.ToString("N2");
                }
            }


        }
       
        void LoadStdTable()
        {
            try
            {
                dataStd.Rows.Clear();
                if (Con.State != System.Data.ConnectionState.Open)
                {
                    Con.Open();
                }

                Int32 currentInt = -1;


                string loadProgData = "SELECT * FROM [Program] WHERE Id = @StudentProgramId";
                SqlCommand cmdProgData = new SqlCommand(loadProgData, Con);
                cmdProgData.Parameters.AddWithValue("@StudentProgramId", StudentProgramId);
                using (SqlDataReader readerProgData = cmdProgData.ExecuteReader())
                {
                    if (readerProgData.Read())
                    {
                        maxSemester = readerProgData["Total"].ToString();
                        currentSession = readerProgData["Session"].ToString();
                        currentInt = int.Parse(currentSession);
                        maxInt = int.Parse(maxSemester);
                        lvlMark = string.IsNullOrEmpty(readerProgData["Lvl_mark"].ToString()) ? "Masters" : readerProgData["Lvl_mark"].ToString();
                    }
                }

                string loadData = "SELECT * FROM [Session] WHERE Id = @currentSession ORDER BY EndSession ASC";
                SqlCommand cmdData = new SqlCommand(loadData, Con);
                cmdData.Parameters.AddWithValue("@currentSession", currentSession);
                using (SqlDataReader readerData = cmdData.ExecuteReader())
                {
                    if (readerData.Read())
                    {
                        programSession = readerData["Name"].ToString();
                        startSession = readerData.GetInt32(readerData.GetOrdinal("Start"));
                        programSemester = readerData["Semester"].ToString();
                    }
                }

                string stdIdConvert = studentTableId.ToString();

                int noOfYearsStudentUsed = Globals.CheckStudentGraduateNo(stdIdConvert, Con);

                int semesterStudentUsed = maxInt % 2 == 0 ? noOfYearsStudentUsed * 2 : (noOfYearsStudentUsed * 2) - 1;
                int maxYear = (int)Math.Ceiling((double)maxInt / 2);

                // This logic works if programs only run on Harmattan and Rain
                // Since both semester session has the same start year
                // The noOfYearsStudentUsed function only returns the year used by student and not semester
                // The bellow if statement is to get the number of semester used by student
                // If the maxInt(maximum number of semester of a program) is divisible by 2, it means the final year 2 semesters else the final year is 1 semester

                // I used the maxInt(Total semester of program) to get all sessions starting from studentStart(Entry year of student) to startSession(Current active year of program.

                //string loadProg = "SELECT TOP "+ maxInt +" * FROM Session where Start >= " + studentStart +" and Start <= "+ startSession + " ORDER BY START ASC";

                //string loadProg = "SELECT TOP @semesterStudentUsed * FROM [Session] WHERE Start >= @studentStart ORDER BY Start ASC";
                string loadProg = $"SELECT TOP {semesterStudentUsed} * FROM [Session] WHERE Start >= @studentStart ORDER BY EndSession ASC";
                SqlCommand cmdProg = new SqlCommand(loadProg, Con);
                //cmdProg.Parameters.AddWithValue("@semesterStudentUsed", semesterStudentUsed);
                cmdProg.Parameters.AddWithValue("@studentStart", studentStart);

                using (SqlDataReader reader = cmdProg.ExecuteReader())
                {
                    int i = 1;
                    int j = i;
                    int repYear = 0;

                    while (reader.Read())
                    {
                        j = i % 2 == 0 ? i / 2 : (int)Math.Ceiling((double)i / 2);
                        j = j > maxYear ? maxYear : j;

                        string session = reader["Name"].ToString();
                        string semester = reader["Semester"].ToString();
                        string jLevel = "";
                        int sessionId = Convert.ToInt32(reader["Id"]);

                        if (programSemester == "Harmattan" && (session != programSession || semester != "Rain"))
                        {
                            jLevel = GetLevelString(lvlMark, j);

                            if (repYear > 0)
                            {
                                dataStd.Rows.Add(false,session, jLevel + " Repeated " + repYear, semester, sessionId);
                            }
                            else
                            {
                                dataStd.Rows.Add(false,session, jLevel, semester, sessionId);
                            }
                        }
                        else if (programSemester != "Harmattan")
                        {
                            jLevel = GetLevelString(lvlMark, j);

                            if (repYear > 0)
                            {
                                dataStd.Rows.Add(false, session, jLevel + " Repeated " + repYear, semester, sessionId);
                            }
                            else
                            {
                                dataStd.Rows.Add(false, session, jLevel, semester, sessionId);
                            }
                        }
                        studentLevel = j.ToString();
                        UpdateLabel14(lvlMark, studentLevel);

                        i++;
                    }
                }

                Con.Close();

               // dataStd.Sort(dataStd.Columns[1], System.ComponentModel.ListSortDirection.Descending);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private string GetLevelString(string lvlMark, int j)
        {
            return lvlMark switch
            {
                "Undergraduate" => j + "00 LEVEL",
                "Post Graduate" => (j + 6) + "00 LEVEL",
                "Masters" => "Year " + j,
                "PHD" => "Year " + j,
                "ND" => "ND " + j,
                "HND" => "HND " + j,
                "Year" => "Year " + j,
                _ => "Unknown Level"
            };
        }

        private void UpdateLabel14(string lvlMark, string studentLevel)
        {
            label14.Text = lvlMark switch
            {
                "Undergraduate" => studentLevel + "00 LEVEL",
                "Post Graduate" => (int.Parse(studentLevel) + 6) + "00 LEVEL",
                "Masters" => "Year " + studentLevel,
                "PHD" => "Year " + studentLevel,
                "ND" => "ND " + studentLevel,
                "HND" => "HND " + studentLevel,
                "Year" => "Year " + studentLevel,
                _ => "Unknown Level"
            };
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

                    //here there

                    string gradeScore = "";
                    Int32 gradePoint;

                    if ((scoreMark >= 70) && (scoreMark <= 100))
                    {
                        gradeScore = "A";
                        gradePoint = 5;
                    }
                    else if ((scoreMark >= 60) && (scoreMark < 70))
                    {
                        gradeScore = "B";
                        gradePoint = 4;
                    }
                    else if ((scoreMark >= 50) && (scoreMark < 60))
                    {
                        gradeScore = "C";
                        gradePoint = 3;
                    }
                    else if ((scoreMark >= 45) && (scoreMark < 50))
                    {
                        gradeScore = "D";
                        gradePoint = 2;
                    }
                    else if ((scoreMark >= 40) && (scoreMark < 45))
                    {
                        gradeScore = "E";
                        gradePoint = 1;
                    }
                    else
                    {
                        gradeScore = "F";
                        gradePoint = 0;
                    }

                    if (scoreMark < 0)
                    {
                        gradeScore = "AR";
                    }

                    if (scoreMark >= 0)
                    {

                        semesterCTU = Int32.Parse(unit) + semesterCTU;

                        if (programGrading == "Percentage Grading")
                        {
                            semesterCTP = (Int32.Parse(unit) * scoreMark) + semesterCTP;
                        }
                        else
                        {
                            semesterCTP = (Int32.Parse(unit) * gradePoint) + semesterCTP;
                        }
                    }

                    dataCourses.Rows.Add(true, code, name, unit, scoreMark, gradeScore, cId);
                }
            }
            Con.Close();

            label44.Text = "Semester Total Units =";
            label46.Text = "Semester Total Points =";
            // label48.Text = "Semester Percentage Total =";

            if (programGrading == "Percentage Grading")
            {
                label48.Text = "Semester Percentage Total =";
            }
            else
            {
                label48.Text = "Semester G.P.A =";
                label33.Text = "Cumulative G.P.A =";
            }

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

                if (programGrading == "Percentage Grading")
                {
                    txtTotal.Text = semesterTotalGrading.ToString() + "%";
                }
                else
                {
                    txtTotal.Text = semesterTotalGrading.ToString("N2");
                }

            }
            else if (semesterCTP == 0)
            {
                Decimal tp = 0;
                Decimal tu = (Decimal)semesterCTU;
                semesterTotalGrading = 0;

                txtTU.Text = semesterCTU.ToString();
                txtTP.Text = semesterCTP.ToString();

                if (programGrading == "Percentage Grading")
                {
                    txtTotal.Text = semesterTotalGrading.ToString() + "%";
                }
                else
                {
                    txtTotal.Text = semesterTotalGrading.ToString("N2");
                }
            }
            
        }


        void LoadExtraCourse(string semester, string currentlevel)
        {
            dataExtraCourse.Rows.Clear();

            
            if (Con.State != System.Data.ConnectionState.Open)
            {
                Con.Open();
            }

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

                    string chechuser = "SELECT count(*) FROM [Result] where Student = '" + studentTableId + "'and Course = '" + cId + "' and (Session = '" + txtSelectedSession.Text + "' or Score > 39)";

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
            if (Con.State != System.Data.ConnectionState.Open)
            {
                Con.Open();
            }

            string SessionQuery = "SELECT * FROM [Session] where Semester = 'Harmattan' ORDER BY NAME";
            SqlCommand cmdSess = new SqlCommand(SessionQuery, Con);
            SqlDataReader readerSession = cmdSess.ExecuteReader();

            var dt = new DataTable();

            dt.Load(readerSession);

            try
            {
                comboBox2.DataSource = null;
                comboBox5.DataSource = null;
                comboBox2.Items.Clear();
                comboBox5.Items.Clear();



                comboBox2.DisplayMember = "Name";
                comboBox2.ValueMember = "Id";
                comboBox2.DataSource = dt;

                comboBox5.DisplayMember = "Name";
                comboBox5.ValueMember = "Id";
                comboBox5.DataSource = dt;


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
                    programGrading = readerstudentProgramData.GetString("Grading");
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
            printButtonStd.Hide();
            txtSelectedSession.Hide();
            comboBox4.Hide();
            txtSelectedLvl.Hide();
            comboBox5.Hide();
            txtSelectedSemester.Hide();
            comboBox1.Hide();

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
            //mistake
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

            printButtonStd.Hide();

            dataStd.Show();
            dataCourses.Hide();
            btnSave.Hide();
            btnPrtRes.Hide();
            bunifuCards4.Hide();

            LoadStdTable();
            LoadResultData();
        }

        private void dataStd_DoubleClick(object sender, EventArgs e)
        {
            string level = "";

            //comboBox3.SelectedItem = this.dataStd.CurrentRow.Cells[0].Value.ToString();
            //comboBox4.SelectedItem = this.dataStd.CurrentRow.Cells[1].Value.ToString().Substring(5, 1);

            string checkLevel = this.dataStd.CurrentRow.Cells[2].Value.ToString();
            if (lvlMark == "Undergraduate")
            {
                //jLevel = j + "00 LEVEL";
                level = checkLevel.Substring(0, 1);
                //comboBox4.SelectedItem = checkLevel.Substring(0, 1);
                txtSelectedLvl.Text = checkLevel.Substring(0, 1);
            }
            else if (lvlMark == "Post Graduate") 
            { 
                //jLevel = j + 6 + "00 LEVEL";
                Int32 pGcheckLevel = Int32.Parse(checkLevel.Substring(0, 1)) - 6;
                level = pGcheckLevel.ToString();
                //comboBox4.SelectedItem = pGcheckLevel.ToString();
                txtSelectedLvl.Text = pGcheckLevel.ToString();
            }
            else if(lvlMark == "Masters")
            {
                //jLevel = "Year " + j;
                level = checkLevel.Substring(5, 1);
                //comboBox4.SelectedItem = checkLevel.Substring(5, 1);
                txtSelectedLvl.Text = checkLevel.Substring(5, 1);
            }
            else if(lvlMark == "PHD")
            {
                //jLevel = "Year " + j;
                level = checkLevel.Substring(5, 1);
                //comboBox4.SelectedItem = checkLevel.Substring(5, 1);
                txtSelectedLvl.Text = checkLevel.Substring(5, 1);
            }
            else if(lvlMark == "ND")
            {
                //jLevel = "ND " + j;
                level = checkLevel.Substring(3, 1);
                //comboBox4.SelectedItem = checkLevel.Substring(3, 1);
                txtSelectedLvl.Text = checkLevel.Substring(3, 1);
            }
            else if (lvlMark == "HND")
            {
                //jLevel = "HND " + j;
                level = checkLevel.Substring(4, 1);
                //comboBox4.SelectedItem = checkLevel.Substring(4, 1);
                txtSelectedLvl.Text = checkLevel.Substring(4, 1);
            }
            else if (lvlMark == "Year")
            {
                //jLevel = "Year " + j;
                level = checkLevel.Substring(5, 1);
                //comboBox4.SelectedItem = checkLevel.Substring(5, 1);
                txtSelectedLvl.Text = checkLevel.Substring(5, 1);
            }

            //comboBox5.SelectedItem = this.dataStd.CurrentRow.Cells[3].Value.ToString().Substring(0, 1);
            txtSelectedSemester.Text = this.dataStd.CurrentRow.Cells[3].Value.ToString().Substring(0, 1);

            /*
            Con.Open();
            string selectSessionQuery = "SELECT * FROM [Session] where Name = '"+ this.dataStd.CurrentRow.Cells[1].Value.ToString() + "'";
            SqlCommand cmdselectSess = new SqlCommand(selectSessionQuery, Con);
            SqlDataReader readerselectSession = cmdselectSess.ExecuteReader();

            if (readerselectSession.Read())
            {
                txtSelectedSession.Text = readerselectSession.GetInt32("Id").ToString();
                comboBox5.SelectedValue = readerselectSession.GetInt32("Id");
            }
            Con.Close();
            */

            txtSelectedSession.Text = this.dataStd.CurrentRow.Cells[4].Value.ToString();
            //comboBox5.SelectedValue = readerselectSession.GetInt32("Id");



            dataCourses.Rows.Clear();

            printButtonStd.Hide();

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

            // string level = this.dataStd.CurrentRow.Cells[1].Value.ToString().Substring(5, 1); 
            string semester = this.dataStd.CurrentRow.Cells[3].Value.ToString().Substring(0, 1);
            Int32 session = Int32.Parse(txtSelectedSession.Text);




            LoadCourseTable(session, semester);

            LoadSemesterTotal(session, semester, StudentProgramId);

            Con.Open();

            //List<int> sessionExtra = GlobalVariable.Globals.StudentSessionExtra(StudentProgramId, session, semester, Con);

            //MessageBox.Show(sessionExtra.Count().ToString());

            Con.Close();

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
                bool hasPassingScore = false;

                for (Int32 i = 0; i < rowCount; i++)
                {
                    string checkR = dataCourses.Rows[i].Cells[0].Value?.ToString();
                    if (checkR == "True")
                    {
                        Int32 rowCourseId = Int32.Parse(dataCourses.Rows[i].Cells[6].Value.ToString());
                        Int32 rowCourseScore;

                        if (dataCourses.Rows[i].Cells[4].Value == null)
                        {
                            rowCourseScore = -1;
                        }
                        else
                        {
                            rowCourseScore = Int32.Parse(dataCourses.Rows[i].Cells[4].Value.ToString());
                        }
                        

                        if (rowCourseScore > 39)
                        {
                            hasPassingScore = true;
                        }
                    }
                }

                if (hasPassingScore)
                {
                    DialogResult result = MessageBox.Show(
                        "A pass score is about to be entered. This will delete any future session data that has been entered for this course.\nAre you sure you want to proceed?",
                        "Confirmation",
                        MessageBoxButtons.YesNo);

                    if (result == DialogResult.No)
                    {
                        return; // Terminate the process if user does not accept
                    }
                }

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


                        // Check for invalid score
                        if (rowCourseScore > 100 || rowCourseScore < -1)
                        {
                            throw new Exception("Invalid score entered. Score must be between -1 and 100.");
                        }

                        // Check for passing score and prompt user
                        if (rowCourseScore > 39)
                        {
                            hasPassingScore = true;
                          
                                // Update logic with additional checks
                                bool exists = CheckRecordExists(studentTableId.ToString(), rowCourseId);

                                if (exists)
                                {
                                    // Logic to get existing session data and IDs
                                    List<int> existingSessionValues = GetExistingSessionValues(studentTableId.ToString(), rowCourseId);
                                    List<int> existingResultIDs = GetExistingResultIDs(studentTableId.ToString(), rowCourseId);

                                    int selectedSessionStartValue = GetStartValueFromSessionTable(txtSelectedSession.Text);

                                    // Loop through existing sessions and delete if future session
                                    for (int j = 0; j < existingSessionValues.Count; j++)
                                    {
                                        int existingSessionStartValue = GetStartValueFromSessionTable(existingSessionValues[j].ToString());
                                        if (selectedSessionStartValue < existingSessionStartValue)
                                        {
                                            //MessageBox.Show(selectedSessionStartValue + " < " + existingSessionStartValue);
                                            
                                            using (SqlCommand cmd = new SqlCommand("DELETE FROM Result WHERE Id = @ResultID", Con))
                                            {
                                                cmd.Parameters.AddWithValue("@ResultID", existingResultIDs[j]);
                                                cmd.ExecuteNonQuery();
                                            }
                                            
                                        }
                                    }

                                    string queryStudent = "UPDATE Result set Score = '" + rowCourseScore + "' where Student = '" + studentTableId + "'and Course = '" + rowCourseId + "' and session = '" + txtSelectedSession.Text + "'";
                                    SqlCommand cmdStudent = new SqlCommand(queryStudent, Con);
                                    cmdStudent.ExecuteNonQuery();

                                    countUpdate++;
                                }
                                else
                                {
                                    // Insert logic if record doesn't exist (not modified in this example)
                                }
                        }
                        else
                        {
                            string queryStudent = "UPDATE Result set Score = '" + rowCourseScore + "' where Student = '" + studentTableId + "'and Course = '" + rowCourseId + "' and session = '" + txtSelectedSession.Text + "'";
                            SqlCommand cmdStudent = new SqlCommand(queryStudent, Con);
                            cmdStudent.ExecuteNonQuery();

                            countUpdate++;
                        }

                        

                        
                    }

                }

                Con.Close();

                string message = hasPassingScore ? " (Some passing scores were entered)" : "";
                MessageBox.Show(countUpdate.ToString() + " Result was Updated" + message);

            }
            catch (Exception ex)
            {
                Con.Close();
                MessageBox.Show(ex.Message);
            }

            LoadResultData();

            LoadSemesterTotal(Int32.Parse(txtSelectedSession.Text), txtSelectedSemester.Text, StudentProgramId);

            LoadCourseTable(Int32.Parse(txtSelectedSession.Text), txtSelectedSemester.Text);
            
        }

        // Helper functions with actual SQL queries
        private bool CheckRecordExists(string studentId, int courseId)
        {
            bool exists = false;
            using (SqlCommand cmd = new SqlCommand("SELECT count(*) FROM Result WHERE Student = @StudentId AND Course = @CourseId", Con))
            {
                cmd.Parameters.AddWithValue("@StudentId", studentId);
                cmd.Parameters.AddWithValue("@CourseId", courseId);

                exists = (int)cmd.ExecuteScalar() > 0;
            }
            return exists;
        }

        private List<int> GetExistingSessionValues(string studentId, int courseId)
        {
            List<int> sessionValues = new List<int>();
            using (SqlCommand cmd = new SqlCommand("SELECT session FROM Result WHERE Student = @StudentId AND Course = @CourseId", Con))
            {
                cmd.Parameters.AddWithValue("@StudentId", studentId);
                cmd.Parameters.AddWithValue("@CourseId", courseId);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        sessionValues.Add(Convert.ToInt32(reader["session"]));
                    }
                }
            }
            return sessionValues;
        }

        private List<int> GetExistingResultIDs(string studentId, int courseId)
        {
            List<int> resultIDs = new List<int>();
            using (SqlCommand cmd = new SqlCommand("SELECT Id FROM Result WHERE Student = @StudentId AND Course = @CourseId", Con))
            {
                cmd.Parameters.AddWithValue("@StudentId", studentId);
                cmd.Parameters.AddWithValue("@CourseId", courseId);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        resultIDs.Add(Convert.ToInt32(reader["Id"])); // Assuming "ResultID" is the unique identifier column
                    }
                }
            }
            return resultIDs;
        }

        private void label39_Click(object sender, EventArgs e)
        {
            backProgram(StudentProgram);
        }

        public void backProgram(string program)
        {
            //string nameP = program;

            frmProgramData programPage = new frmProgramData() { Dock = DockStyle.Fill, TopLevel = false, TopMost = true, FormBorderStyle = FormBorderStyle.None };
            programPage.label17.Text = program;
            programPage.programName = program;
            contextual parentForm = ApplicationInstance.ContextualForm;
            if (parentForm != null)
            {
                parentForm.formerPage = "frmProgram"; // Track the former page
                parentForm.currentPage = "frmProgramData";
                parentForm.studentDataVariable = program;
                parentForm.panel1.Controls.Clear(); // Clear existing controls in panel1
                parentForm.panel1.Controls.Add(programPage);
                parentForm.iconButton2.Show();
                parentForm.iconButton1.Show();
                programPage.Show();
            }
        }

        private void pnlStd_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnPrtRes_Click(object sender, EventArgs e)
        {

            // Reset the page index and start printing
            currentPageIndex = 0;
            isPrinting = true;

            // Show the print dialog
            ShowPrintDialog();

            //printDocument1.PrintPage += new PrintPageEventHandler(PrintPage);
            //printDocument1.Print();

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
            //resultPage.Show();


        }


        // Button click event to show the print dialog
        private void ShowPrintDialog()
        {
            PrintPreviewDialog printPreviewDialog = new PrintPreviewDialog();
            printPreviewDialog.Document = printDocument1;

            // Attach the PrintPage event handler here
            printDocument1.PrintPage += new PrintPageEventHandler(PrintPage);

            // Display the print preview dialog
            printPreviewDialog.ShowDialog();
        }


        private void PrintPage(object sender, PrintPageEventArgs e)
        {

            // Calculate the remaining space on the current page
            float availableHeight = e.MarginBounds.Bottom - currentYPosition;

            // Initialize currentYPosition to the top margin
            //currentYPosition = e.MarginBounds.Top;

            // Reset totalContentHeight at the beginning of each page
            //totalContentHeight = 0;

            Con.Open();

            string header = "";
            string resultFontValue = "8.5";

            string selectSessionQuery = "SELECT * FROM [UserTbl]";
            SqlCommand cmdselectSess = new SqlCommand(selectSessionQuery, Con);
            SqlDataReader readerselectSession = cmdselectSess.ExecuteReader();

            if (readerselectSession.Read())
            {
                header = readerselectSession.GetString("Header").ToUpper() + "\n(" + StudentProgram.ToUpper() + ")" + "\n" + "STUDENTS SEMESTER RESULT";

                resultFontValue = readerselectSession.GetString("Result_font");
            }

            // Define your logo image and header text
            Image logo = Resources.lautech_letter_head;
            DateTime currentTime = DateTime.Now;

            string time = "Time :   " + currentTime.ToString("hh:mm:ss tt");
            string date = "Date :   " + currentTime.ToString("dd/MM/yyyy");

            // Set up the font and margins
            Font headerFont = new Font("Arial", 10.8f, FontStyle.Bold);
            Font dateTimeFont = new Font("Georgia", 7.2f, FontStyle.Italic);
            Font infoLabelFont = new Font("Arial", float.Parse(resultFontValue));
            float y = 10;  // logo vertical position
            float yHeader = 25;  // header vertical
            float yDate = (headerFont.GetHeight() * 7);  // date vertical
            float yTime = yDate + 16;  // time vertical
            float lineHeight = headerFont.GetHeight();
            float logoWidth = 150;
            float logoHeight = 150;

            // Create StringFormat for text alignment
            StringFormat headerFormat = new StringFormat();
            headerFormat.Alignment = StringAlignment.Center;
            headerFormat.LineAlignment = StringAlignment.Center;

            StringFormat dateTimeFormat = new StringFormat();
            dateTimeFormat.Alignment = StringAlignment.Far; // Align to the right

            StringFormat infoLabelFormat = new StringFormat();
            infoLabelFormat.Alignment = StringAlignment.Near; // Align to the left

            StringFormat resultLabelFormat = new StringFormat();
            resultLabelFormat.Alignment = StringAlignment.Near; // Align to the left

            // Draw the logo and header
            e.Graphics.DrawImage(logo, 10, y, logoWidth, logoHeight);
            e.Graphics.DrawString(header, headerFont, Brushes.Black, new RectangleF(logoWidth + 30, yHeader, e.PageBounds.Width - (logoWidth + 20) * 2, headerFont.GetHeight() * 5), headerFormat);

            // Draw the date and time on the far right
            e.Graphics.DrawString(date, dateTimeFont, Brushes.Black, new RectangleF(0, yDate, e.PageBounds.Width - 20, dateTimeFont.GetHeight()), dateTimeFormat);
            e.Graphics.DrawString(time, dateTimeFont, Brushes.Black, new RectangleF(0, yTime, e.PageBounds.Width - 20, dateTimeFont.GetHeight()), dateTimeFormat);

            // Add the information labels (NAME, REGISTRATION NUMBER, etc.)
            float xLabels = 40; // Horizontal position for labels
            float yLabels = yTime + 20; // Vertical position for labels
            float labelSpacing = infoLabelFont.GetHeight() + 5; // Spacing between labels

            string selectQuery = "SELECT * FROM [Session] where Id = '" + txtSelectedSession.Text + "'";
            SqlCommand cmdselect = new SqlCommand(selectQuery, Con);
            SqlDataReader readerselect = cmdselect.ExecuteReader();

            string sessionPrint = "";
            string levelPrint = "";

            if (readerselect.Read())
            {
                sessionPrint = readerselect.GetString("Name");
            }

            if (lvlMark == "Undergraduate")
            {
                levelPrint = txtSelectedLvl.Text + "00 LEVEL";
            }

            if (lvlMark == "Post Graduate")
            {
                levelPrint = Int32.Parse(txtSelectedLvl.Text) + 6 + "00 LEVEL";
            }

            if (lvlMark == "Masters")
            {
                levelPrint = "Year " + txtSelectedLvl.Text;
            }

            if (lvlMark == "PHD")
            {
                levelPrint = "Year " + txtSelectedLvl.Text;
            }

            if (lvlMark == "ND")
            {
                levelPrint = "ND " + txtSelectedLvl.Text;
            }

            if (lvlMark == "HND")
            {
                levelPrint = "HND " + txtSelectedLvl.Text;
            }

            if (lvlMark == "Year")
            {
                levelPrint = "Year " + txtSelectedLvl.Text;
            }

            if (sPassAll > 0)
            {
                printRMK = "REQUIRED COURSE (S) OUTSTANDING";
                /*
                resultPage.labelOS.Text = sCarryOvers;

                resultPage.labelOS.Show();
                resultPage.label13.Show();
                resultPage.label15.Show();
                */
            }
            else
            {
                printRMK = "ALL REQUIRED COURSES PASSED";
                /*
                resultPage.labelOS.Hide();
                resultPage.label13.Hide();
                resultPage.label15.Hide();
                */
            }

            // Define the labels and corresponding data (replace with actual data)
            string[] infoLabels = { "NAME", "REGISTRATION NUMBER", "SESSION", "SEMESTER", "LEVEL", "PROGRAM", "REMARK" };
            string[] infoData = { StudentName, studentId, sessionPrint, txtSelectedSemester.Text, levelPrint, StudentProgram, printRMK };

            // Draw the information labels and data
            for (int i = 0; i < infoLabels.Length; i++)
            {
                e.Graphics.DrawString(infoLabels[i], infoLabelFont, Brushes.Black, xLabels, yLabels + i * labelSpacing, infoLabelFormat);
                e.Graphics.DrawString(":", infoLabelFont, Brushes.Black, xLabels + infoLabelFont.Size * infoLabels[1].Length + 5, yLabels + i * labelSpacing, infoLabelFormat);
                e.Graphics.DrawString(infoData[i], infoLabelFont, Brushes.Black, xLabels + infoLabelFont.Size * infoLabels[1].Length + 25, yLabels + i * labelSpacing, infoLabelFormat);
            }

            // Draw the result table labels (COURSE CODE, COURSE TITLE, UNIT, SCORE, GRADE)
            float xResultLabels = xLabels; // Horizontal position for result labels
            float yResultLabels = yLabels + labelSpacing * infoLabels.Length + 20; // Vertical position for result labels
            float resultLabelSpacing = infoLabelFont.GetHeight() + 5; // Spacing between result labels

            string[] resultLabels = { "COURSE CODE", "COURSE TITLE", "UNIT", "SCORE", "GRADE" };

            // Define the custom column widths
            float courseCodeWidth = e.Graphics.MeasureString(resultLabels[0], infoLabelFont).Width + 10; // Width for "COURSE CODE" column
            float courseTitleWidth = 450; // Width for "COURSE TITLE" column (wider)
            float unitWidth = e.Graphics.MeasureString(resultLabels[2], infoLabelFont).Width + 10; // Width for "UNIT" column
            float scoreWidth = e.Graphics.MeasureString(resultLabels[3], infoLabelFont).Width + 10; // Width for "SCORE" column
            float gradeWidth = e.Graphics.MeasureString(resultLabels[4], infoLabelFont).Width + 10; // Width for "GRADE" column

            // Calculate the x-positions for each column based on the custom widths
            float[] columnXPositions = new float[resultLabels.Length];
            columnXPositions[0] = xResultLabels;
            columnXPositions[1] = columnXPositions[0] + courseCodeWidth;
            columnXPositions[2] = columnXPositions[1] + courseTitleWidth;
            columnXPositions[3] = columnXPositions[2] + unitWidth;
            columnXPositions[4] = columnXPositions[3] + scoreWidth;

            Int32 rowCount = Int32.Parse(dataCourses.RowCount.ToString());

            // Draw horizontal grid line at the top of the table
            e.Graphics.DrawLine(Pens.Black, xResultLabels, yResultLabels, xResultLabels + courseCodeWidth + courseTitleWidth + unitWidth + scoreWidth + gradeWidth, yResultLabels);

            // Draw the result table labels at the top of each column with grid lines
            for (int i = 0; i < resultLabels.Length; i++)
            {
                // Draw horizontal grid line for the top of the table
                e.Graphics.DrawLine(Pens.Black, columnXPositions[i], yResultLabels, columnXPositions[i], yResultLabels + resultLabelSpacing);

                e.Graphics.DrawString(resultLabels[i], infoLabelFont, Brushes.Black, columnXPositions[i] + 5, yResultLabels, resultLabelFormat);

                // Draw vertical grid line to the left of each column
                e.Graphics.DrawLine(Pens.Black, columnXPositions[i], yResultLabels, columnXPositions[i], yResultLabels + resultLabelSpacing);

                // Draw horizontal grid line for the bottom of the table
                e.Graphics.DrawLine(Pens.Black, columnXPositions[i], yResultLabels + resultLabelSpacing, columnXPositions[i], yResultLabels + (rowCount + 1) * resultLabelSpacing);
            }

            for (Int32 i = 0; i < rowCount; i++)
            {
                bool exists = false;

                Int32 rowCourseId = Int32.Parse(dataCourses.Rows[i].Cells[6].Value.ToString());
                string checkR = dataCourses.Rows[i].Cells[0].Value.ToString();

                string chechuser = "SELECT count(*) FROM [Result] where Student = '" + studentTableId + "' and Course = '" + rowCourseId + "'";

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

                    // Draw horizontal grid line for the bottom of each row
                    e.Graphics.DrawLine(Pens.Black, xResultLabels, yResultLabels + (i + 1) * resultLabelSpacing, xResultLabels + courseCodeWidth + courseTitleWidth + unitWidth + scoreWidth + gradeWidth, yResultLabels + (i + 1) * resultLabelSpacing);

                    // Draw vertical grid lines to the left of each column
                    for (int j = 0; j < resultLabels.Length; j++)
                    {
                        e.Graphics.DrawLine(Pens.Black, columnXPositions[j], yResultLabels + i * resultLabelSpacing, columnXPositions[j], yResultLabels + (i + 1) * resultLabelSpacing);
                    }

                    const int maxCourseTitleLength = 60;

                    string truncatedCourseTitle = (courseTitle.Length > maxCourseTitleLength)
                        ? courseTitle.Substring(0, maxCourseTitleLength) + "..."
                        : courseTitle;

                    e.Graphics.DrawString(courseCode, infoLabelFont, Brushes.Black, columnXPositions[0] + 5, yResultLabels + (i + 1) * resultLabelSpacing, resultLabelFormat);
                    e.Graphics.DrawString(truncatedCourseTitle, infoLabelFont, Brushes.Black, columnXPositions[1] + 5, yResultLabels + (i + 1) * resultLabelSpacing, resultLabelFormat);
                    e.Graphics.DrawString(courseUnit, infoLabelFont, Brushes.Black, columnXPositions[2] + 5, yResultLabels + (i + 1) * resultLabelSpacing, resultLabelFormat);
                    e.Graphics.DrawString(courseScore, infoLabelFont, Brushes.Black, columnXPositions[3] + 5, yResultLabels + (i + 1) * resultLabelSpacing, resultLabelFormat);
                    e.Graphics.DrawString(courseGrade, infoLabelFont, Brushes.Black, columnXPositions[4] + 5, yResultLabels + (i + 1) * resultLabelSpacing, resultLabelFormat);

                }
            }

            // Draw vertical grid line to the right of the last column (GRADE)
            e.Graphics.DrawLine(Pens.Black, columnXPositions[4] + gradeWidth, yResultLabels, columnXPositions[4] + gradeWidth, yResultLabels + (rowCount + 1) * resultLabelSpacing);

            // Draw horizontal line at the bottom of the last column
            e.Graphics.DrawLine(Pens.Black, columnXPositions[0], yResultLabels + (rowCount + 1) * resultLabelSpacing, columnXPositions[4] + gradeWidth, yResultLabels + (rowCount + 1) * resultLabelSpacing);
            
            string perc_gpa = "";
            string perc = "";

            if (programGrading == "Percentage Grading")
            {
                perc_gpa = "Percentage";
                perc = "%";
            }
            else
            {
                perc_gpa = "G.P.A";
            }
            // Define labels and data for the new section
            string[] additionalInfoLabels = {"Semester Total Unit", "Semester Total Points", "Semester " + perc_gpa, "Cumulative Total Unit", "Cumulative Total Points", "Cumulative " + perc_gpa};
 
            string[] additionalInfoData = { semesterCTU.ToString(), semesterCTP.ToString(), semesterTotalGrading.ToString("N2") + perc, sCTU.ToString(), sCTP.ToString(), sTotalGrading.ToString("N2") + perc};

            // Define the font and spacing for the new section
            Font additionalInfoFont = new Font("Arial", float.Parse(resultFontValue));
            float xAdditionalInfoLabels = xLabels; // Horizontal position for labels
            float yAdditionalInfoLabels = yResultLabels + (rowCount + 1) * resultLabelSpacing + 20; // Vertical position for labels
            float additionalInfoLabelSpacing = additionalInfoFont.GetHeight() + 5; // Spacing between labels

            // Draw the additional information labels and data
            for (int i = 0; i < additionalInfoLabels.Length; i++)
            {
                e.Graphics.DrawString(additionalInfoLabels[i], additionalInfoFont, Brushes.Black, xAdditionalInfoLabels, yAdditionalInfoLabels + i * additionalInfoLabelSpacing, infoLabelFormat);
                e.Graphics.DrawString("=", additionalInfoFont, Brushes.Black, xAdditionalInfoLabels + additionalInfoFont.Size * additionalInfoLabels[1].Length + 5, yAdditionalInfoLabels + i * additionalInfoLabelSpacing, infoLabelFormat);
                e.Graphics.DrawString(additionalInfoData[i], additionalInfoFont, Brushes.Black, xAdditionalInfoLabels + additionalInfoFont.Size * additionalInfoLabels[1].Length + 25, yAdditionalInfoLabels + i * additionalInfoLabelSpacing, infoLabelFormat);
            }

            // Define the font and spacing for the conditional section
            Font conditionalInfoFont = new Font("Arial", float.Parse(resultFontValue));
            float xConditionalInfoLabels = xLabels; // Horizontal position for labels
            float yConditionalInfoLabels = yAdditionalInfoLabels + additionalInfoLabels.Length * additionalInfoLabelSpacing + 20; // Vertical position for labels
            float conditionalInfoLabelSpacing = conditionalInfoFont.GetHeight() + 5; // Spacing between labels

            if (sPassAll > 0)
            {
                // Courses still outstanding
                string outstandingLabel = "COURSES STILL OUTSTANDING";

                e.Graphics.DrawString(outstandingLabel, conditionalInfoFont, Brushes.Black, xConditionalInfoLabels, yConditionalInfoLabels, infoLabelFormat);
                yConditionalInfoLabels += conditionalInfoFont.GetHeight() + 5; // Move down after the label

                // Add the H.O.D's Signature line
                float lineY = yConditionalInfoLabels; // Position the line below the text
                e.Graphics.DrawLine(Pens.Black, xConditionalInfoLabels, lineY, e.PageBounds.Width - xConditionalInfoLabels * 2, lineY);
                yConditionalInfoLabels += 5; // Move down after the line

                // Horizontal list of outstanding courses
                string outstandingCourses = sCarryOvers;

                e.Graphics.DrawString(outstandingCourses, conditionalInfoFont, Brushes.Black, xConditionalInfoLabels, yConditionalInfoLabels, infoLabelFormat);
                yConditionalInfoLabels += conditionalInfoFont.GetHeight() + 5; // Move down after each course


                // Add "Examiner's Signature" text
                string examinerSignatureText = "----------------------------------\n    Examiner's Signature";
                float examinerSignatureTextWidth = e.Graphics.MeasureString(examinerSignatureText, conditionalInfoFont).Width;
                float examinerSignatureTextX = 140; // Position under line
                e.Graphics.DrawString(examinerSignatureText, conditionalInfoFont, Brushes.Black, examinerSignatureTextX, yConditionalInfoLabels + 120, infoLabelFormat);


                // Add "H.O.D's Signature" text
                string hodSignatureText = "-------------------------------\n    H.O.D's Signature";
                float hodSignatureTextWidth = e.Graphics.MeasureString(hodSignatureText, conditionalInfoFont).Width;
                float hodSignatureTextX = e.PageBounds.Width - xConditionalInfoLabels - 240; // Position under line
                e.Graphics.DrawString(hodSignatureText, conditionalInfoFont, Brushes.Black, hodSignatureTextX, yConditionalInfoLabels + 120, infoLabelFormat);

                currentYPosition = yConditionalInfoLabels + 125;
                totalContentHeight = currentYPosition;
            }
            else
            {

                // Add "Examiner's Signature" text
                string examinerSignatureText = "----------------------------------\n    Examiner's Signature";
                float examinerSignatureTextWidth = e.Graphics.MeasureString(examinerSignatureText, conditionalInfoFont).Width;
                float examinerSignatureTextX = 140; // Position under line
                yConditionalInfoLabels += conditionalInfoFont.GetHeight() + 5; // Move down after the H.O.D's Signature line
                e.Graphics.DrawString(examinerSignatureText, conditionalInfoFont, Brushes.Black, examinerSignatureTextX, yConditionalInfoLabels + 120, infoLabelFormat);

                // Add the H.O.D's Signature text
                string hodSignatureText = "-------------------------------\n   H.O.D's Signature";
                float hodSignatureTextWidth = e.Graphics.MeasureString(hodSignatureText, conditionalInfoFont).Width;
                float hodSignatureTextX = e.PageBounds.Width - xConditionalInfoLabels - 240; // Position under line
                e.Graphics.DrawString(hodSignatureText, conditionalInfoFont, Brushes.Black, hodSignatureTextX, yConditionalInfoLabels + 120, infoLabelFormat);

                currentYPosition = yConditionalInfoLabels + 125;
                totalContentHeight = currentYPosition;
            }

            Con.Close();

            // Calculate the remaining content height
            float remainingContentHeight = totalContentHeight - currentYPosition;

            // Check if there is more content to print
            //if (moreContentToPrint())
            //{
                // Determine if there's spill-over content
                if (remainingContentHeight > availableHeight)
                {
                    e.HasMorePages = true;

                    // Update currentYPosition to the top of the next page
                    currentYPosition = e.MarginBounds.Top;
                }
                else
                {
                    // The content fits on this page
                    e.HasMorePages = false;
                }

                currentPageIndex++;
            //}
            //else
            //{
                // Reset the flag to stop printing after the current page
                //isPrinting = false;
            //}


        }

        // Function to determine if there is more content to print
        private bool moreContentToPrint()
        {
            // Implement your logic to determine if there is more content to print
            // For example, check if there are more rows to print
            // Return true to continue printing or false to stop
            // In this example, we'll assume that there are more pages for demonstration purposes
            return isPrinting && currentPageIndex < maxPageIndex; // Define maxPageIndex as needed
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

                    // Check if student and course exists in the Result table
                    //current updating 
                    string checkOthersQuery = "SELECT * FROM Result WHERE Student = @Student AND Course = @Course";
                    using (SqlCommand cmdCheckOthers = new SqlCommand(checkOthersQuery, Con))
                    {
                        cmdCheckOthers.Parameters.AddWithValue("@Student", studentTableId);
                        cmdCheckOthers.Parameters.AddWithValue("@Course", courseRowId);

                        using (SqlDataReader readerCheckOthers = cmdCheckOthers.ExecuteReader())
                        {
                            if (readerCheckOthers.HasRows)
                            {
                                List<int> sessionIds = new List<int>();

                                //int clickedSessionId = Int32.Parse(txtSelectedSession.Text);
                                //sessionIds.Add(clickedSessionId);

                                // Get original start value from Session table based on clicked session ID
                                int originalStartValue = GetStartValueFromSessionTable(txtSelectedSession.Text);

                                // Loop through any additional results for the student and course (optional)
                                while (readerCheckOthers.Read())
                                {
                                    int currentSessionId = readerCheckOthers.GetInt32("Session");
                                    sessionIds.Add(currentSessionId);
                                }

                                // Check if the original result has the smallest start value
                                bool canDelete = IsSmallestStartValue(originalStartValue, sessionIds);

                                if (canDelete)
                                {
                                    // Delete the result(s) with the smallest start value (original or others)
                                    DeleteResult(studentTableId.ToString(), courseRowId, txtSelectedSession.Text);
                                    MessageBox.Show("Result deleted successfully");
                                    LoadResultData();
                                    LoadCourseTable(Int32.Parse(txtSelectedSession.Text), txtSelectedSemester.Text);
                                }
                                else
                                {
                                    MessageBox.Show("Result cannot be deleted. Result exists in future session and must be deleted first.");
                                }
                            }
                            else
                            {
                                // Delete the result(s) with the smallest start value (original or others)
                                DeleteResult(studentTableId.ToString(), courseRowId, txtSelectedSession.Text);
                                MessageBox.Show("Result deleted successfully");
                                LoadResultData();
                                LoadCourseTable(Int32.Parse(txtSelectedSession.Text), txtSelectedSemester.Text);
                            }
                        }
                    }
                    Con.Close();
                }
                else if (result == DialogResult.No)
                {
                    result = DialogResult.Cancel;
                }
            }
        }

        private int GetStartValueFromSessionTable(string sessionId)
        {
            // Implement logic to retrieve the 'start' value from the Session table based on sessionId
            // This function should return the integer value of the 'start' field
            // Here's an example assuming a column named 'start' in the Session table:
            string getStartValueQuery = "SELECT Start FROM Session WHERE Id = @SessionId";
            using (SqlCommand cmdGetStart = new SqlCommand(getStartValueQuery, Con))
            {
                cmdGetStart.Parameters.AddWithValue("@SessionId", sessionId); // Corrected parameter name
                using (SqlDataReader readerStart = cmdGetStart.ExecuteReader())
                {
                    if (readerStart.Read())
                    {
                        return readerStart.GetInt32("Start");
                    }
                    else
                    {
                        // Handle case where 'start' value is not found in Session table (e.g., throw exception)
                        throw new Exception("An error occored while deleting reslt");
                    }
                }
            }
        }

        private bool IsSmallestStartValue(int originalStart, List<int> sessionIds)
        {
            foreach (Int32 sessionId in sessionIds)
            {
                int currentStartValue = GetStartValueFromSessionTable(sessionId.ToString());
                if ( originalStart < currentStartValue)
                {
                    return false; // Another result has a smaller start value
                }
            }
            return true; // Original result has the smallest start value or is the only one
        }

        private void DeleteResult(string studentId, string courseId, string sessionId)
        {
            try
            {
                string deleteQuery = "DELETE FROM Result WHERE Student = @Student AND Course = @Course AND Session = @Session";
                using (SqlCommand cmdDelete = new SqlCommand(deleteQuery, Con))
                {
                    cmdDelete.Parameters.AddWithValue("@Student", studentId);
                    cmdDelete.Parameters.AddWithValue("@Course", courseId);
                    cmdDelete.Parameters.AddWithValue("@Session", sessionId);
                    cmdDelete.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting result: " + ex.Message);
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
            button2.Hide();
            btnPrtRes.Hide();
            btnSave.Hide();
            //MessageBox.Show(txtSelectedLvl.Text + " " + txtSelectedSemester.Text);
            LoadExtraCourse(txtSelectedSemester.Text, txtSelectedLvl.Text);
        }

        private void label53_Click(object sender, EventArgs e)
        {
            bunifuCards4.Hide();
            button2.Show();
            btnPrtRes.Show();
            btnSave.Show();
        }

        private void label54_Click(object sender, EventArgs e)
        {
            //try
            //{
                //Int32 rowCountJ = Int32.Parse(dataExtraCourse.RowCount.ToString());
                //Int32 countInsert = 0;
                //MessageBox.Show(rowCountJ.ToString());
                if (Con.State != System.Data.ConnectionState.Open)
                {
                    Con.Open();
                }
                
                // Get the max_unit value from the program table
                string getMaxUnitQuery = "SELECT max_unit FROM program WHERE Id = @StudentProgramId";
                int maxUnit = -1;

                using (SqlCommand cmdMaxUnit = new SqlCommand(getMaxUnitQuery, Con))
                {
                    cmdMaxUnit.Parameters.AddWithValue("@StudentProgramId", StudentProgramId);
                    object result = cmdMaxUnit.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        maxUnit = Convert.ToInt32(result);

                       // MessageBox.Show("Max unit: "+maxUnit.ToString());
                    }
                }

                if (maxUnit != -1)
                {
                    // Sum units of courses already registered by the student in the selected session
                    string getCourseIdsQuery = "SELECT Course FROM [Result] WHERE Student = @Student AND Session = @Session";
                    List<int> registeredCourseIds = new List<int>();

                    using (SqlCommand cmdCourseIds = new SqlCommand(getCourseIdsQuery, Con))
                    {
                        cmdCourseIds.Parameters.AddWithValue("@Student", studentTableId);
                        cmdCourseIds.Parameters.AddWithValue("@Session", txtSelectedSession.Text);

                        using (SqlDataReader reader = cmdCourseIds.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                registeredCourseIds.Add(reader.GetInt32(0));
                            }
                        }
                    }

                int registeredUnits = 0;

                    if (registeredCourseIds.Count > 0)
                    {
                        string getUnitsQuery = "SELECT SUM(CAST(unit AS INT)) FROM Course WHERE Id IN (" + string.Join(",", registeredCourseIds) + ")";
                        using (SqlCommand cmdUnits = new SqlCommand(getUnitsQuery, Con))
                        {
                            object unitResult = cmdUnits.ExecuteScalar();
                            if (unitResult != null && unitResult != DBNull.Value)
                            {
                                registeredUnits = Convert.ToInt32(unitResult);
                            }
                        }
                    }

                    // Sum units of courses checked in dataExtraCourse
                    int extraUnits = 0;
                    int rowCountJ = dataExtraCourse.RowCount;

                    for (int j = 0; j < rowCountJ; j++)
                    {
                        string checkR = dataExtraCourse.Rows[j].Cells[0].Value.ToString();
                        if (checkR == "True")
                        {
                            int extraUnit = Int32.Parse(dataExtraCourse.Rows[j].Cells[3].Value.ToString());
                            extraUnits += extraUnit;
                        }
                    }

                // Check if the total units exceed the max_unit
                int totalUnits = registeredUnits + extraUnits;

                    if (totalUnits > maxUnit)
                    {
                        MessageBox.Show("The courses have exceeded the program semester unit maximum.");
                        return;
                    }
                }

                int countInsert = 0;

                //for (Int32 j = 0; j < rowCountJ; j++)
                for (Int32 j = 0; j < dataExtraCourse.RowCount; j++) 
                {
                    //string rowShow = dataCourses.Rows[i].Cells[6].Value.ToString();
                    string checkR = dataExtraCourse.Rows[j].Cells[0].Value.ToString();
                    //MessageBox.Show(checkR);
                    if (checkR == "True")
                    {
                        
                        Int32 rowCourseId = Int32.Parse(dataExtraCourse.Rows[j].Cells[5].Value.ToString());

                        bool exists = false;

                        string chechuser = "SELECT count(*) FROM [Result] where Student = '" + studentTableId + "'and Course = '" + rowCourseId + "' and session = '"+ txtSelectedSession.Text+"' ";

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

            //}
            //catch (Exception ex)
            //{
                Con.Close();
               // MessageBox.Show(ex.Message);
            //}

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

        private void label48_Click(object sender, EventArgs e)
        {

        }

        private void label33_Click(object sender, EventArgs e)
        {

        }

        private void dataStd_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        
        }

        private void dataStd_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (dataStd.Columns[e.ColumnIndex].Name == "action")
            {
                dataStd_DoubleClick(sender, EventArgs.Empty);
            }
            // Ensure the clicked cell is in the checkbox column and is within valid row bounds
            if (e.ColumnIndex == 0 && e.RowIndex >= 0)
            {
                DataGridViewCheckBoxCell chkCell = (DataGridViewCheckBoxCell)dataStd.Rows[e.RowIndex].Cells[0];

                // Toggle checkbox state
                bool isChecked = !Convert.ToBoolean(chkCell.Value);
                chkCell.Value = isChecked;

                // Perform actions based on the checkbox state
                if (isChecked)
                {
                    // Checkbox is checked, show the print button
                    printButtonStd.Show();
                }
                else
                {
                    // Checkbox is unchecked, check if any other checkboxes are still checked
                    bool anyChecked = false;

                    foreach (DataGridViewRow row in dataStd.Rows)
                    {
                        if (Convert.ToBoolean(row.Cells[0].Value))
                        {
                            anyChecked = true;
                            break;
                        }
                    }

                    // If no checkboxes are checked, hide the print button
                    if (!anyChecked)
                    {
                        printButtonStd.Hide();
                    }
                }
            }
        }

        private void printButtonStd_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Option to print multiple result is under development.");
        }
    }
}
