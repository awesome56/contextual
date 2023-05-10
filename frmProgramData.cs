using FontAwesome.Sharp;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;
using GlobalVariable;
using System.Windows.Media;

namespace Contextual
{
    public partial class frmProgramData : Form
    {
        public frmProgramData()
        {
            InitializeComponent();
            iconButton4.Hide();


        }
        public string programName = "";
        Int32 programId;
        string programAbbr = "";
        string programHod = "";
        string programHodId = "";
        string programExaminer = "";
        string programExaminerId = "";
        string programSession = "";
        Int32 programSessionStart;
        string programSessionId = "";
        string programDean = "";
        string programDeanId = "";
        String programTotal = "";
        string programGrading = "";
        Int32 passGrade = 0;
        string lvlMark = "";

        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + GlobalVariable.Globals.databasePath + ";Integrated Security=True;MultipleActiveResultSets=True;Connect Timeout=30");

        //dataStd ****

        void LoadStdTable()
        {
            dataStd.Rows.Clear();
            Con.Open();
            string loadProg = "SELECT * FROM [Student] where Program = '"+ programId + "'";

            //DataTable dt = new DataTable();

            SqlCommand cmdProg = new SqlCommand(loadProg, Con);
            int val1 = cmdProg.ExecuteNonQuery();


            SqlDataReader reader = cmdProg.ExecuteReader();


            int i = 0;

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

                dataStd.Rows.Add(i + 1, studentName, studentMatric, studentYear, studId);

                i++;
            }

            Con.Close();
        }

        void LoadCourseTable()
        {
            dataCourses.Rows.Clear();
            Con.Open();
            string loadProg = "SELECT * FROM [Course] where Program = '" + programId + "'";

            //DataTable dt = new DataTable();

            SqlCommand cmdProg = new SqlCommand(loadProg, Con);
            int val1 = cmdProg.ExecuteNonQuery();


            SqlDataReader reader = cmdProg.ExecuteReader();


            int i = 1;

            while (reader.Read())
            {
                string courseTitle = reader.GetString("Name");
                string courseCode = reader.GetString("Code");
                string courseLevel = reader.GetString("Level");
                string courseSemester = reader.GetString("Semester");
                string courseUnit = reader.GetString("Unit");
                string courseId = reader.GetInt32("ID").ToString();


                dataCourses.Rows.Add(i, courseTitle, courseCode, courseLevel, courseSemester, courseUnit, courseId);

                i++;
            }

            

            Con.Close();
        }

        void LoadProgramDetails(string programName)
        {
            // Get program data
            if (programName != "")
            {
                Con.Open();
                string loadProg = "SELECT * FROM [Program] where Name = '" + programName + "'";

                SqlCommand cmdProg = new SqlCommand(loadProg, Con);

                SqlDataReader reader = cmdProg.ExecuteReader();
                int i = 0;

                if (reader.Read())
                {
                    try
                    {
                        if (reader.GetString("Examiner") == null)
                        {
                            programExaminerId = "";

                            programExaminer = "";
                        }
                        else
                        {
                            programExaminerId = reader.GetString("Examiner");

                            string loadExaminerData = "SELECT * FROM [Lecturer] where Id = '" + reader.GetString("Examiner") + "'";
                            SqlCommand cmdExaminerData = new SqlCommand(loadExaminerData, Con);
                            SqlDataReader readerExaminerData = cmdExaminerData.ExecuteReader();
                            if (readerExaminerData.Read())
                            {
                                programExaminer = readerExaminerData.GetString("Name");
                            }
                        }
                    }
                    catch(Exception ex)
                    {

                    }
                    
                    

                    programHodId = reader.GetString("Hod");

                    string loadStaffData = "SELECT * FROM [Lecturer] where Id = '" + reader.GetString("Hod") + "'";
                    SqlCommand cmdStaffData = new SqlCommand(loadStaffData, Con);
                    SqlDataReader readerStaffData = cmdStaffData.ExecuteReader();
                    if (readerStaffData.Read())
                    {
                        programHod = readerStaffData.GetString("Name");
                    }

                    programDeanId = reader.GetString("Dean");

                    string loadDeanData = "SELECT * FROM [Lecturer] where Id = '" + reader.GetString("Dean") + "'";
                    SqlCommand cmdDeanData = new SqlCommand(loadDeanData, Con);
                    SqlDataReader readerDeanData = cmdDeanData.ExecuteReader();
                    if (readerDeanData.Read())
                    {
                        programDean = readerDeanData.GetString("Name");
                    }

                    programSessionId = reader.GetString("Session");

                    string loadYearData = "SELECT * FROM [Session] where Id = '" + reader.GetString("Session") + "'";
                    SqlCommand cmdYearData = new SqlCommand(loadYearData, Con);
                    SqlDataReader readerYearData = cmdYearData.ExecuteReader();
                    string programSemester = "";
                    if (readerYearData.Read())
                    {
                        programSession = readerYearData.GetString("Name");
                        programSemester = readerYearData.GetString("Semester");
                        programSessionStart = readerYearData.GetInt32("Start");
                    }

                    programId = reader.GetInt32("Id");
                    programAbbr = reader.GetString("Abbr");

                    if (reader.GetString("Grading") != null) 
                    {
                        programGrading = reader.GetString("Grading");
                    }

                    if (reader.GetString("Total") != null)
                    {
                        programTotal = reader.GetString("Total");
                    }

                    if (reader.GetInt32("Pass") != -1)
                    {
                        passGrade = reader.GetInt32("Pass");

                    }
                    else
                    {
                        if (programGrading == "Percentage Grading")
                        {
                            passGrade = 10;
                        }
                        else
                        {
                            passGrade = 1;
                        }
                    }

                    if (reader.GetString("Lvl_mark") == "")
                    {
                        lvlMark = "Masters";

                    }
                    else
                    {
                        lvlMark = reader.GetString("Lvl_mark");
                    }


                    label17.Text = programAbbr + " (" + programName + ")";
                    label5.Text = programHod;
                    label6.Text = programDean;
                    label14.Text = programSession +" / "+ programSemester;
                    label47.Text = programTotal;
                    label50.Text = programGrading;

                    txtPrgName.Text = programName;
                    txtPrgAbbr.Text = programAbbr;
                    textTotalSemester.Text = programTotal;

                    txtPass.Text = passGrade.ToString();

                    combolvl.SelectedItem = lvlMark;

                    comboGrading.SelectedItem = programGrading;

                    if (programSemester == "Harmattan")
                    {
                        comboCurrSem.SelectedItem = "H";
                    }else if (programSemester == "Rain")
                    {
                        comboCurrSem.SelectedItem = "R";
                    }
                    else
                    {
                        comboCurrSem.SelectedItem = null;
                    }

                    //comboCurrSem.SelectedItem = programSemester.Substring(0, 1);
                }

                //dt.Load(reader);

                //dataPrg.DataSource= dt;

                Con.Close();
            }
            else
            {
                MessageBox.Show("Selected program does not exist");
            }
        }

        void Fill_Report_Session()
        {
            Con.Open();

            string SessionQuery = "SELECT * FROM [Session] where Start <= '"+ programSessionStart +"' and Semester = 'Harmattan' ORDER BY NAME";
            SqlCommand cmdSess = new SqlCommand(SessionQuery, Con);
            SqlDataReader readerSession = cmdSess.ExecuteReader();

            var dt = new DataTable();

            dt.Load(readerSession);

            try
            {
                comboBox3.DataSource = null;
                comboBox3.Items.Clear();
                comboBox3.DisplayMember = "Name";
                comboBox3.ValueMember = "Id";
                comboBox3.DataSource = dt;

                string SQuery = "SELECT * FROM [Session] where Name = '" + programSession + "' and Semester = 'Harmattan'";
                SqlCommand cmdS = new SqlCommand(SQuery, Con);
                SqlDataReader readerS = cmdS.ExecuteReader();

                string idsess = "";

                if (readerS.Read())
                {
                    comboBox3.SelectedValue = readerS["Id"].ToString();
                }
            }
            catch (Exception ex)
            {
                Con.Close();
                MessageBox.Show(ex.Message);
            }
            Con.Close();
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

        void Fill_Level()
        {
            comboBox4.Items.Clear();
            comboBoxLevel.Items.Clear();
            Int32 divString = (int)Math.Ceiling((double)Int32.Parse(programTotal) / (double)2);
            for (int i = 1; i <= divString; i++)
            {
                comboBoxLevel.Items.Add(i);
                comboBox4.Items.Add(i);
            }
            
        }

        void Fill_Program_Session()
        {
            try
            {
                Con.Open();

                comboBox1.Items.Clear();

                string SQuery = "SELECT * FROM [Session] where Name = '"+programSession+"' and Semester = 'Harmattan'";
                SqlCommand cmdS = new SqlCommand(SQuery, Con);
                SqlDataReader readerS = cmdS.ExecuteReader();

                string idsess = "";

                if (readerS.Read())
                {
                    idsess = readerS["Id"].ToString();
                }

                string SessionQuery = "SELECT * FROM [Session] where Semester = 'Harmattan' ORDER BY NAME";
                SqlCommand cmdSess = new SqlCommand(SessionQuery, Con);
                SqlDataReader readerSession = cmdSess.ExecuteReader();
                
                

                var dt = new DataTable();

                dt.Load(readerSession);

            

                comboBox1.SelectedItem= null;
                /*
                foreach (DataRow dr in dt.Rows)
                {
                    if (programSessionId == dr["Id"].ToString())
                    {
                        comboBox1.Items.Add(dr["Name"].ToString());
                        comboBox1.SelectedItem = dr["Name"].ToString();
                        comboBox1.ValueMember = dr["Id"].ToString();
                    }
                    else
                    {
                        comboBox1.Items.Add(dr["Name"].ToString());
                        comboBox1.ValueMember = dr["Id"].ToString();
                    }

                }
                */
                
                comboBox1.DataSource = null;
                comboBox1.Items.Clear();

                comboBox1.DisplayMember = "Name";
                comboBox1.ValueMember = "Id";
                comboBox1.DataSource = dt;

                comboBox1.SelectedItem = null;

                if ((programSessionId != "") || (programSessionId != null))
                {
                    comboBox1.SelectedValue = idsess;
                }
            }
            catch (Exception ex)
            {
                Con.Close();
                MessageBox.Show(ex.Message);
            }
            Con.Close();
        }

        void Fill_Examiner()
        {
            Con.Open();

            comboExamOff.Items.Clear();

            string HodQuery = "SELECT * FROM [Lecturer]";
            SqlCommand cmdHod = new SqlCommand(HodQuery, Con);
            SqlDataReader reader = cmdHod.ExecuteReader();

            //List<string> pp = new List<string>();

            //pp.Add(new )

            var dtHod = new DataTable();

            dtHod.Load(reader);

            try
            {

                comboExamOff.DataSource = null;
                /*
                foreach (DataRow dr in dtHod.Rows)
                {
                    if (programHodId == dr["Id"].ToString())
                    {
                        selectHod.Items.Add(dr["Name"].ToString() +" (" + dr["Staff_id"].ToString()+")");
                        selectHod.SelectedItem = dr["Name"].ToString() + " (" + dr["Staff_id"].ToString() + ")";
                        selectHod.ValueMember = dr["Id"].ToString();
                    }
                    else
                    {
                        selectHod.Items.Add(dr["Name"].ToString() + " (" + dr["Staff_id"].ToString() + ")");
                        selectHod.ValueMember = dr["Id"].ToString();
                    }
                    
                }
                */

                comboExamOff.Items.Clear();

                comboExamOff.DisplayMember = "Name";
                comboExamOff.ValueMember = "Id";
                comboExamOff.DataSource = dtHod;

                comboExamOff.SelectedItem = null;

                if ((programExaminerId != "") || (programExaminerId != null))
                {
                    comboExamOff.SelectedValue = programExaminerId;
                }
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

            selectHod.Items.Clear();

            string HodQuery = "SELECT * FROM [Lecturer]";
            SqlCommand cmdHod = new SqlCommand(HodQuery, Con);
            SqlDataReader reader = cmdHod.ExecuteReader();

            //List<string> pp = new List<string>();

            //pp.Add(new )

            var dtHod = new DataTable();

            dtHod.Load(reader);
            
            try
            {

                selectHod.DataSource = null;
                /*
                foreach (DataRow dr in dtHod.Rows)
                {
                    if (programHodId == dr["Id"].ToString())
                    {
                        selectHod.Items.Add(dr["Name"].ToString() +" (" + dr["Staff_id"].ToString()+")");
                        selectHod.SelectedItem = dr["Name"].ToString() + " (" + dr["Staff_id"].ToString() + ")";
                        selectHod.ValueMember = dr["Id"].ToString();
                    }
                    else
                    {
                        selectHod.Items.Add(dr["Name"].ToString() + " (" + dr["Staff_id"].ToString() + ")");
                        selectHod.ValueMember = dr["Id"].ToString();
                    }
                    
                }
                */

                selectHod.Items.Clear();

                selectHod.DisplayMember = "Name";
                selectHod.ValueMember = "Id";
                selectHod.DataSource = dtHod;

                selectHod.SelectedItem = null;

                if ((programHodId != "") || (programHodId != null))
                {
                    selectHod.SelectedValue = programHodId;
                }
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

            selectDean.Items.Clear();

            string DeanQuery = "SELECT * FROM [Lecturer]";
            SqlCommand cmdDean = new SqlCommand(DeanQuery, Con);
            SqlDataReader readerDean = cmdDean.ExecuteReader();

            var dtDean = new DataTable();

            dtDean.Load(readerDean);

            try
            {
                //selectDean.DataSource = null;
                /*
                foreach (DataRow dr in dtDean.Rows)
                {
                    if (programDeanId == dr["Id"].ToString())
                    {
                        selectDean.Items.Add(dr["Name"].ToString() + " (" + dr["Staff_id"].ToString() + ")");
                        selectDean.SelectedItem = dr["Name"].ToString() + " (" + dr["Staff_id"].ToString() + ")";
                        selectDean.ValueMember = dr["Id"].ToString();
                    }
                    else
                    {
                        selectDean.Items.Add(dr["Name"].ToString() + " (" + dr["Staff_id"].ToString() + ")");
                        selectDean.ValueMember = dr["Id"].ToString();
                    }

                }
                */
                
                selectDean.DataSource = null;
                selectDean.Items.Clear();



                selectDean.DisplayMember = "Name";
                selectDean.ValueMember = "Id";
                selectDean.DataSource = dtDean;

                selectDean.SelectedItem = null;

                if ((programDeanId != "") || (programDeanId != null))
                {
                    selectDean.SelectedValue = programDeanId;
                }

            }
            catch (Exception ex)
            {
                Con.Close();
                MessageBox.Show(ex.Message);
            }
            Con.Close();
        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void iconButton2_Click(object sender, EventArgs e)
        {
            pnlAddCourses.Hide();
            pnlStd.Hide();
            panel2.Show();
        }

        private void dataStd_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void frmProgramData_Load(object sender, EventArgs e)
        {
            textSearchCourse.Hide();
            label58.Hide();
            searchCourseBtn.Hide();

            label57.Hide();
            btnAddCourses.Hide();
            btnAddStd.Show();
            pnlStd.Hide();
            panel2.Hide();
            dataStd.Show();
            dataCourses.Hide();


            LoadProgramDetails(programName);

            LoadStdTable();

            LoadCourseTable();

            Fill_Program_Session();
            Fill_Level();
            Fill_Session();
            Fill_Examiner();
            Fill_Hod();
            Fill_Dean();

            Fill_Report_Session();
        }

        private void label11_Click(object sender, EventArgs e)
        {
            panel2.Hide();
        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            LoadCourseTable();

            txtSearch.Hide();
            label57.Hide();
            btnsearch.Hide();

            textSearchCourse.Show();
            label58.Hide();
            searchCourseBtn.Show();

            btnAddCourses.Show();
            btnAddStd.Hide();
            dataStd.Hide();
            dataCourses.Show();
        }

        private void iconButton3_Click(object sender, EventArgs e)
        {
            textSearchCourse.Hide();
            label58.Hide();
            searchCourseBtn.Hide();

            LoadStdTable();
            txtSearch.Text = "";
            txtSearch.Show();
            label57.Hide();
            btnsearch.Show();
            btnAddCourses.Hide();
            dataCourses.Hide();
            dataStd.Show();
            btnAddStd.Show();
        }

        private void btnAddStd_Click(object sender, EventArgs e)
        {
            pnlAddCourses.Hide();
            btnAddCourses.Hide();
            pnlStd.Show();
            panel2.Show();
            btnAddStd.Hide();
        }

        private void label24_Click(object sender, EventArgs e)
        {
            panel2.Hide();
            pnlStd.Hide();
            btnAddStd.Show();
        }

        private void btnAddCourses_Click(object sender, EventArgs e)
        {
            button2.Show();
            label28.Text = "Add Course";
            textCourseTitle.Text = "";
            textCourseCode.Text = "";
            textUnit.Text = "";
            comboBoxLevel.SelectedItem = null;
            comboBoxSemester.SelectedItem = null;
            updateCourse.Hide();
            deleteCourse.Hide();

            pnlAddCourses.Show();
            pnlStd.Show();
            panel2.Show();
            btnAddCourses.Hide();
        }

        private void label30_Click(object sender, EventArgs e)
        {
            pnlAddCourses.Hide();
            pnlStd.Hide();
            panel2.Hide();
            btnAddCourses.Show();

            label28.Text = "Add Course";
            textCourseTitle.Text = "";
            textCourseCode.Text = "";
            textUnit.Text = "";
            comboBoxLevel.SelectedValue = null;
            comboBoxSemester.SelectedValue = null;
            updateCourse.Hide();
            deleteCourse.Hide();

            button2.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if ((textCourseTitle.Text == "") || (textCourseCode.Text == "") || (textUnit.Text == "") || (comboBoxLevel.SelectedItem == null) || (comboBoxSemester.SelectedItem == null))
                {
                    MessageBox.Show("All fields are required");
                }
                else
                {
                    Con.Open();

                    bool existsCode = false;
                    string chechCode = "SELECT count(*) FROM [Course] where Code='" + textCourseCode.Text + "' and Program ='"+ programId+"'";

                    SqlCommand cmdCode = new SqlCommand(chechCode, Con);
                    cmdCode.Parameters.AddWithValue("Code", textCourseCode.Text);
                    existsCode = (int)cmdCode.ExecuteScalar() > 0;

                    if (existsCode)
                    {
                        MessageBox.Show("Course Code Already Exists in this Program");
                    }
                    else
                    {
                        String queryCourse = "insert into Course(Name,Code,Program,Level,Semester,Unit) values('" + textCourseTitle.Text + "','" + textCourseCode.Text + "','" + programId + "','" + comboBoxLevel.Text + "','" + comboBoxSemester.Text.Substring(0, 1) + "', '"+ textUnit.Text + "')";
                        SqlCommand cmdCourse = new SqlCommand(queryCourse, Con);
                        cmdCourse.ExecuteNonQuery();
                        MessageBox.Show("Course data successfuly Added");

                        textCourseTitle.Text = "";
                        textCourseCode.Text = "";
                        textUnit.Text = "";
                        comboBoxLevel.SelectedItem = null;
                        comboBoxSemester.SelectedItem = null;
                    }
                    Con.Close();
                }

            }
            catch (Exception ex)
            {
                Con.Close();
                MessageBox.Show(ex.Message);
            }
            LoadCourseTable();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if ((txtMatric.Text == "") || (txtSurname.Text == "") || (textFirstname.Text == "") || (comboGender.SelectedItem == null) || (comboBox2.SelectedItem == null))
                {
                    MessageBox.Show("All fields are required");
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
                        String queryStd = "insert into Student(Matric,Gender,Program,Year, Firstname, Middlename, Lastname) values('" + txtMatric.Text + "','" + comboGender.Text + "','" + programId + "','" + comboBox2.GetItemText(comboBox2.SelectedValue) + "','" + textFirstname.Text + "','" + textOthername.Text + "','" + txtSurname.Text + "')";
                        SqlCommand cmdStd = new SqlCommand(queryStd, Con);
                        cmdStd.ExecuteNonQuery();
                        MessageBox.Show("Student data successfuly Added");

                        txtMatric.Text = "";
                        txtSurname.Text = "";
                        comboGender.SelectedItem = null;
                        comboBox2.SelectedItem = null;
                    }
                    Con.Close();
                }

            }
            catch (Exception ex)
            {
                Con.Close();
                MessageBox.Show(ex.Message);
            }
            LoadStdTable();
        }

        private void label28_Click(object sender, EventArgs e)
        {

        }

        private void dataStd_DoubleClick(object sender, EventArgs e)
        {
            string nameP = this.dataStd.CurrentRow.Cells[1].Value.ToString();
            frmStudentData programPage = new frmStudentData();
            programPage.label17.Text = nameP;

            programPage.studentId = this.dataStd.CurrentRow.Cells[2].Value.ToString();

            programPage.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if ((txtPrgName.Text == "") || (comboBox1.SelectedItem == null) || (textTotalSemester.Text == "") || (comboGrading.SelectedItem == null) || (comboCurrSem.SelectedItem == null) || (combolvl.SelectedItem == null) || (txtPass.Text == ""))
                {
                    MessageBox.Show("You must fill all required field");
                }
                else
                {
                    long numb = 0;
                    bool canConvert = long.TryParse(textTotalSemester.Text, out numb);
                    if (canConvert)
                    {
                        bool canConvertPass = long.TryParse(txtPass.Text, out numb);
                        if ((canConvertPass) && (Int32.Parse(txtPass.Text) <= 100) && (Int32.Parse(txtPass.Text) >= 0))
                        {
                            Con.Open();

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

                            string loadStaffData = "SELECT * FROM [Session] where Name like '" + comboBox1.Text + "' and Semester = '" + sessionSemester + "'";
                            SqlCommand cmdStaffData = new SqlCommand(loadStaffData, Con);
                            SqlDataReader readerStaffData = cmdStaffData.ExecuteReader();
                            if (readerStaffData.Read())
                            {
                                sessionId = readerStaffData["Id"].ToString();
                            }

                            if (txtPrgName.Text == programName)
                            {
                                string query = "update Program set Name = '" + txtPrgName.Text + "', Abbr = '" + txtPrgAbbr.Text + "',Hod = '" + selectHod.GetItemText(selectHod.SelectedValue) + "',Dean = '" + selectDean.GetItemText(selectDean.SelectedValue) + "',Session = '" + sessionId + "',Total='" + textTotalSemester.Text + "', Grading = '" + comboGrading.Text + "', Lvl_mark = '" + combolvl.Text + "',Pass = '" + txtPass.Text + "', Examiner = '"+ comboExamOff.GetItemText(comboExamOff.SelectedValue) + "' where Id = '" + programId + "'";
                                SqlCommand cmd = new SqlCommand(query, Con);
                                cmd.ExecuteNonQuery();
                                MessageBox.Show("Program Details Updated Successfully");

                                programName = txtPrgName.Text;
                            }
                            else
                            {
                                bool exists = false;
                                string chechuser = "SELECT count(*) FROM [Program] where Name like '" + txtPrgName.Text + "'";

                                SqlCommand cmdUser = new SqlCommand(chechuser, Con);
                                cmdUser.Parameters.AddWithValue("Code", txtMatric.Text);
                                exists = (int)cmdUser.ExecuteScalar() > 0;

                                if (exists)
                                {
                                    MessageBox.Show("Program Name Already Exists in Database");
                                }
                                else
                                {
                                    string query = "update Program set Name = '" + txtPrgName.Text + "', Abbr = '" + txtPrgAbbr.Text + "',Hod = '" + selectHod.GetItemText(selectHod.SelectedValue) + "',Dean = '" + selectDean.GetItemText(selectDean.SelectedValue) + "',Session = '" + sessionId + "',Total='" + textTotalSemester.Text + "', Grading = '" + comboGrading.Text + "', Lvl_mark = '" + combolvl.Text + "',Pass = '" + txtPass.Text + "', Examiner = '"+ comboExamOff.GetItemText(comboExamOff.SelectedValue) + "' where Id = '" + programId + "'";
                                    SqlCommand cmd = new SqlCommand(query, Con);
                                    cmd.ExecuteNonQuery();
                                    MessageBox.Show("Program Details Updated Successfully");

                                    programName = txtPrgName.Text;
                                }
                            }

                            Con.Close();

                            LoadProgramDetails(programName);
                            Fill_Level();
                            Fill_Report_Session();
                        }
                        else
                        {
                            MessageBox.Show("Invalid value inserted as pass mark");
                        }
                        
                    }
                    else
                    {
                        MessageBox.Show("Total Semester value must be numeric");
                    }
                    
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataCourses_DoubleClick(object sender, EventArgs e)
        {
            button2.Hide();
            label28.Text = "Edit Course";
            textCourseTitle.Text = this.dataCourses.CurrentRow.Cells[1].Value.ToString();
            textCourseCode.Text = this.dataCourses.CurrentRow.Cells[2].Value.ToString();
            textUnit.Text = this.dataCourses.CurrentRow.Cells[5].Value.ToString();
            updateCourse.Show();
            deleteCourse.Show();
            courseCodeSelected.Text = this.dataCourses.CurrentRow.Cells[2].Value.ToString();
            courseIdSelected.Text = this.dataCourses.CurrentRow.Cells[6].Value.ToString();


            //comboBoxLevel.SelectedValue = this.dataCourses.CurrentRow.Cells[3].Value.ToString();
            //comboBoxLevel.Items.Clear();

            Int32 amtCourseLevel = Int32.Parse(this.dataCourses.CurrentRow.Cells[3].Value.ToString());
            Int32 amtprogramLevel = (int)Math.Ceiling((double)Int32.Parse(programTotal) / (double)2);

            if (amtCourseLevel > amtprogramLevel)
            {
                comboBoxLevel.Items.Add(amtCourseLevel);
                comboBoxLevel.SelectedItem = amtCourseLevel;
            }
            else
            {
                comboBoxLevel.SelectedItem = amtCourseLevel;
            }
            /*
            if (this.dataCourses.CurrentRow.Cells[3].Value.ToString() == "Year 1") 
            {
                comboBoxLevel.Items.Add("Year 1");
                comboBoxLevel.SelectedItem = "Year 1";
                comboBoxLevel.Items.Add("Year 2");
            }
            else
            {
                comboBoxLevel.Items.Add("Year 1");
                comboBoxLevel.Items.Add("Year 2");
                comboBoxLevel.SelectedItem = "Year 2";
            }
            */

            //comboBoxSemester.SelectedValue = this.dataCourses.CurrentRow.Cells[4].Value.ToString();
            comboBoxSemester.Items.Clear();
            if (this.dataCourses.CurrentRow.Cells[4].Value.ToString() == "H")
            {
                comboBoxSemester.Items.Add("Harmattan");
                comboBoxSemester.SelectedItem = "Harmattan";
                comboBoxSemester.Items.Add("Rain");
            }
            else
            {
                comboBoxSemester.Items.Add("Harmattan");
                comboBoxSemester.Items.Add("Rain");
                comboBoxSemester.SelectedItem = "Rain";
            }

            pnlAddCourses.Show();
            pnlStd.Show();
            panel2.Show();
            btnAddCourses.Hide();
        }

        private void updateCourse_Click(object sender, EventArgs e)
        {
            try
            {
                if ((textCourseTitle.Text == "") || (textCourseCode.Text == "") || (textUnit.Text == "") || (comboBoxLevel.SelectedItem == null) || (comboBoxSemester.SelectedItem == null))
                {
                    MessageBox.Show("You must fill all required field");
                }
                else
                {
                    long numb = 0;
                    bool canConvert = long.TryParse(textUnit.Text, out numb);
                    if (canConvert)
                    {
                        Con.Open();

                        if (textCourseCode.Text == courseCodeSelected.Text)
                        {
                            string query = "update Course set Name = '" + textCourseTitle.Text + "', Code = '" + textCourseCode.Text + "',Program = '" + programId + "',Level = '" + comboBoxLevel.Text + "',Semester = '" + comboBoxSemester.Text.Substring(0, 1) + "',Unit='" + textUnit.Text + "' where Code = '" + textCourseCode.Text + "' and Program = '" + programId + "'";
                            SqlCommand cmd = new SqlCommand(query, Con);
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Course Details Updated Successfully");
                        }
                        else
                        {
                            bool exists = false;
                            string chechuser = "SELECT count(*) FROM [Course] where Code='" + textCourseCode.Text + "' AND Program ='"+ programId +"'";

                            SqlCommand cmdUser = new SqlCommand(chechuser, Con);
                            cmdUser.Parameters.AddWithValue("Code", txtMatric.Text);
                            exists = (int)cmdUser.ExecuteScalar() > 0;

                            if (exists)
                            {
                                MessageBox.Show("Course code already exist in Program");
                            }
                            else
                            {
                                string query = "update Course set Name = '" + textCourseTitle.Text + "', Code = '" + textCourseCode.Text + "',Program = '" + programId + "',Level = '" + comboBoxLevel.Text + "',Semester = '" + comboBoxSemester.Text.Substring(0, 1) + "',Unit='" + textUnit.Text + "' where Code = '" + courseCodeSelected.Text + "' and Program = '" + programId + "'";
                                SqlCommand cmd = new SqlCommand(query, Con);
                                cmd.ExecuteNonQuery();
                                MessageBox.Show("Course Details Updated Successfully");

                            }
                        }

                        
                        Con.Close();
                    }
                    else
                    {
                        MessageBox.Show("Unit value must be numeric");
                    }
                }
                

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            LoadCourseTable();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void comboBoxLevel_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void iconButton5_Click(object sender, EventArgs e)
        {
            if ((comboBox3.SelectedItem == null) || (comboBox4.SelectedItem == null) || (comboBox5.SelectedItem == null))
            {
                MessageBox.Show("You must fill all required field");
            }
            else
            {
                string session = comboBox3.GetItemText(comboBox3.SelectedItem);
                Int32 sessionId = Int32.Parse(comboBox3.GetItemText(comboBox3.SelectedValue));
                string level = comboBox4.GetItemText(comboBox4.SelectedItem);

                string semester = comboBox5.Text;

                frmSummary summary = new frmSummary();
                frmMms MMS = new frmMms();

                summary.session = session;
                summary.sessionId = sessionId;
                summary.semester = semester;
                summary.level = level;
                summary.program = programName;
                summary.programId = programId;
                summary.programStart = programSessionStart;

                summary.Show();

                

                MMS.session = session;
                MMS.sessionId = sessionId;
                MMS.semester = semester;
                MMS.level = level;
                MMS.program = programName;
                MMS.programId = programId;
                MMS.programStart = programSessionStart;

                MMS.Show();
            }
            
        }

        private void deleteCourse_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("By deleting this course, all the students result of this course will be deleted", "Delete Course", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                Con.Open();
                string queryDel = "DELETE FROM Course WHERE Code = '" + courseCodeSelected.Text + "' AND Program = '" + programId + "'";
                SqlCommand cmdDel = new SqlCommand(queryDel, Con);
                cmdDel.ExecuteNonQuery();

                string queryDel2 = "DELETE FROM Result WHERE Course = '" + courseIdSelected.Text + "'";
                SqlCommand cmdDel2 = new SqlCommand(queryDel2, Con);
                cmdDel2.ExecuteNonQuery();

                Con.Close();

                MessageBox.Show("Course Successfuly deleted");
            }
            else if (result == DialogResult.No)
            {
                result = DialogResult.Cancel;
            }

            LoadCourseTable();
        }

        private void dataCourses_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //program delete

            try
            {
                DialogResult result = MessageBox.Show("Are you sure you want to delete this program record? \nBy deleting the Program record, all the students, courses and results associated with the Program will also be deleted", "Delete Program record", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    Con.Open();
                    string queryDel = "DELETE FROM Student WHERE Program = '" + programId + "'";
                    SqlCommand cmdDel = new SqlCommand(queryDel, Con);
                    cmdDel.ExecuteNonQuery();

                    string loadData = "SELECT * FROM [Course] where Program = '" + programId + "'";
                    SqlCommand cmdData = new SqlCommand(loadData, Con);
                    SqlDataReader readerData = cmdData.ExecuteReader();
                    while (readerData.Read())
                    {
                        string queryDel3 = "DELETE FROM Result WHERE Course = '" + readerData.GetInt32("Id") + "'";
                        SqlCommand cmdDel3 = new SqlCommand(queryDel3, Con);
                        cmdDel3.ExecuteNonQuery();
                    }

                    string queryDel2 = "DELETE FROM Course WHERE Program = '" + programId + "'";
                    SqlCommand cmdDel2 = new SqlCommand(queryDel2, Con);
                    cmdDel2.ExecuteNonQuery();

                    string queryDel4 = "DELETE FROM Program WHERE Id = '" + programId + "'";
                    SqlCommand cmdDel4 = new SqlCommand(queryDel4, Con);
                    cmdDel4.ExecuteNonQuery();

                    Con.Close();

                    MessageBox.Show("Program record successfuly deleted");

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

        private void btnsearch_Click(object sender, EventArgs e)
        {
            if(txtSearch.Text != "")
            {
                label57.Show();
                dataStd.Rows.Clear();
                Con.Open();
                string loadProg = "SELECT * FROM [Student] where Program = '" + programId + "' and Matric like '%" + txtSearch.Text + "%' or Firstname like '%" + txtSearch.Text + "%' or Middlename like '%" + txtSearch.Text + "%' or Lastname like '%" + txtSearch.Text + "%'";

                //DataTable dt = new DataTable();

                SqlCommand cmdProg = new SqlCommand(loadProg, Con);
                int val1 = cmdProg.ExecuteNonQuery();


                SqlDataReader reader = cmdProg.ExecuteReader();


                int i = 0;

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

                    dataStd.Rows.Add(i, studentName, studentMatric, studentYear, studId);

                    i++;
                }

                // check session
                string querySession = "SELECT * FROM [Session] where Name like '%" + txtSearch.Text + "%'";
                SqlCommand cmdSession = new SqlCommand(querySession, Con);
                SqlDataReader readerSession = cmdSession.ExecuteReader();

                while (readerSession.Read())
                {
                    Int32 sessionId = readerSession.GetInt32("Id");

                    string loadProg2 = "SELECT * FROM [Student] where Program = '" + programId + "' and Year = '" + sessionId + "'";
                    SqlCommand cmdProg2 = new SqlCommand(loadProg2, Con);
                    SqlDataReader reader2 = cmdProg2.ExecuteReader();

                    while (reader2.Read())
                    {
                        string studentYear = "";

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
                            Int32 check = Int32.Parse(dataStd.Rows[j].Cells[4].Value.ToString());
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
                            dataStd.Rows.Add(i, studentName, studentMatric,studentYear, studId);

                            i++;
                        }


                    }
                }

                Con.Close();
            }
        }

        private void label57_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            LoadStdTable();
        }

        private void label58_Click(object sender, EventArgs e)
        {
            textSearchCourse.Text = "";
            LoadCourseTable();
        }

        private void searchCourseBtn_Click(object sender, EventArgs e)
        {
            if(textSearchCourse.Text != "")
            {
                label58.Show();
                dataCourses.Rows.Clear();
                Con.Open();
                string loadProg = "SELECT * FROM [Course] where Program = '" + programId + "' and Name like '%" + textSearchCourse.Text + "%' or Code like '%" + textSearchCourse.Text + "%' or Level like '%" + textSearchCourse.Text + "%' or Semester  like '%" + textSearchCourse.Text + "%' or Unit  like '%" + textSearchCourse.Text + "%'";

                //DataTable dt = new DataTable();

                SqlCommand cmdProg = new SqlCommand(loadProg, Con);
                int val1 = cmdProg.ExecuteNonQuery();


                SqlDataReader reader = cmdProg.ExecuteReader();


                int i = 1;

                while (reader.Read())
                {
                    string courseTitle = reader.GetString("Name");
                    string courseCode = reader.GetString("Code");
                    string courseLevel = reader.GetString("Level");
                    string courseSemester = reader.GetString("Semester");
                    string courseUnit = reader.GetString("Unit");
                    string courseId = reader.GetInt32("ID").ToString();


                    dataCourses.Rows.Add(i, courseTitle, courseCode, courseLevel, courseSemester, courseUnit, courseId);

                    i++;
                }



                Con.Close();
            }
        }
    }
}
