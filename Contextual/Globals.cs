using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Data;
using System.Data.SqlClient;
using static System.Net.WebRequestMethods;
using System.Net.Http;
using System.Text.Json;
using System.IO;
using System.IO.Compression;
using System.Windows.Forms;
using System.Net;
using DocumentFormat.OpenXml.Spreadsheet;
using Contextual;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;

namespace GlobalVariable
{
    public static class Globals
    {


#if DEBUG

        public static string databasePath = @"C:\Users\Apple\source\repos\Contextual\Contextual\concdb.mdf";

#else

        static string machineName = Directory.GetCurrentDirectory();

        //static string machineName = |DataDirectory|;

        public static string databasePath = machineName + @"\concdb.mdf";

#endif
       

        // Get student 

        // Check if student has Courses left to pass
        //public static List<int> CheckStudentExtra(string studentTableId, SqlConnection Con)
        //{
        //    // Query to get the student's program ID based on their student ID
        //    string programIdQuery = "SELECT Program FROM [Student] WHERE Id = @StudentId";
        //    string programId = "";

        //    // Execute the query to retrieve the program ID
        //    using (SqlCommand cmdProgramId = new SqlCommand(programIdQuery, Con))
        //    {
        //        cmdProgramId.Parameters.AddWithValue("@StudentId", studentTableId);
        //        programId = cmdProgramId.ExecuteScalar()?.ToString();
        //    }

        //    // Initialize a list to store the IDs of compulsory courses
        //    List<int> compulsoryCourses = new List<int>();

        //    // Query to get the IDs of compulsory courses for the student's program
        //    string compulsoryCoursesQuery = "SELECT Id FROM [Course] WHERE Program = @ProgramId AND Type = 'C'";

        //    // Execute the query to retrieve the compulsory course IDs
        //    using (SqlCommand cmdCompulsoryCourses = new SqlCommand(compulsoryCoursesQuery, Con))
        //    {
        //        cmdCompulsoryCourses.Parameters.AddWithValue("@ProgramId", programId);
        //        using (SqlDataReader readerCompulsoryCourses = cmdCompulsoryCourses.ExecuteReader())
        //        {
        //            // Add each compulsory course ID to the list
        //            while (readerCompulsoryCourses.Read())
        //            {
        //                compulsoryCourses.Add(readerCompulsoryCourses.GetInt32(readerCompulsoryCourses.GetOrdinal("Id")));
        //            }
        //        }
        //    }

        //    // Query to load the student's results data
        //    string loadResultData = "SELECT * FROM [Result] WHERE Student = @Student";
        //    using (SqlCommand cmdResultData = new SqlCommand(loadResultData, Con))
        //    {
        //        cmdResultData.Parameters.AddWithValue("@Student", studentTableId);
        //        using (SqlDataReader readerResultData = cmdResultData.ExecuteReader())
        //        {
        //            // Process each result record
        //            while (readerResultData.Read())
        //            {
        //                int scores = readerResultData.GetInt32(readerResultData.GetOrdinal("Score"));
        //                int checkId = readerResultData.GetInt32(readerResultData.GetOrdinal("Course"));

        //                // If the score is greater than 39, remove the course ID from the compulsory list
        //                if (scores > 39)
        //                {
        //                    compulsoryCourses.Remove(checkId);
        //                }

        //                // If the score is between 0 and 39, check if the student has passed the course in another attempt
        //                if ((scores < 40) && (scores >= 0))
        //                {
        //                    bool exists = false;

        //                    // Query to check if the student has passed the course in any other attempt
        //                    string checkDataQ = "SELECT COUNT(*) FROM [Result] WHERE Student = @Student AND Course = @Course AND Score > 39";
        //                    using (SqlCommand cmdcheckDataQ = new SqlCommand(checkDataQ, Con))
        //                    {
        //                        cmdcheckDataQ.Parameters.AddWithValue("@Student", studentTableId);
        //                        cmdcheckDataQ.Parameters.AddWithValue("@Course", readerResultData.GetInt32(readerResultData.GetOrdinal("Course")));

        //                        // If the student has passed the course, remove it from the compulsory list
        //                        exists = (int)cmdcheckDataQ.ExecuteScalar() > 0;
        //                    }

        //                    if (exists)
        //                    {
        //                        compulsoryCourses.Remove(checkId);
        //                    }
        //                }

        //                // If the score is negative, perform the same check for passing the course in another attempt
        //                if (scores < 0)
        //                {
        //                    bool exists = false;

        //                    // Query to check if the student has passed the course in any other attempt
        //                    string checkDataQ = "SELECT COUNT(*) FROM [Result] WHERE Student = @Student AND Course = @Course AND Score > 39";
        //                    using (SqlCommand cmdcheckDataQ = new SqlCommand(checkDataQ, Con))
        //                    {
        //                        cmdcheckDataQ.Parameters.AddWithValue("@Student", studentTableId);
        //                        cmdcheckDataQ.Parameters.AddWithValue("@Course", readerResultData.GetInt32(readerResultData.GetOrdinal("Course")));

        //                        // If the student has passed the course, remove it from the compulsory list
        //                        exists = (int)cmdcheckDataQ.ExecuteScalar() > 0;
        //                    }

        //                    if (exists)
        //                    {
        //                        compulsoryCourses.Remove(checkId);
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    // Return the final list of compulsory courses that the student still needs to take
        //    return compulsoryCourses;
        //}

        // Get session start from session table
        public static int getSessionStart(int sessionId, SqlConnection Con)
        {
            string sessionStartQuery = "SELECT Start FROM [Session] WHERE Id = @SessionId";
            int sessionStart = 0;

            using (SqlCommand cmdsessionStart = new SqlCommand(sessionStartQuery, Con))
            {
                cmdsessionStart.Parameters.AddWithValue("@SessionId", sessionId);

                // ExecuteScalar returns the first column of the first row in the result set, or a null reference if the result set is empty.
                var result = cmdsessionStart.ExecuteScalar();

                // Check if the result is null before converting to an integer
                if (result != null && int.TryParse(result.ToString(), out sessionStart))
                {
                    // Successfully parsed sessionStart
                    return sessionStart;
                }
                else
                {
                    // Handle the case where result is null or not an integer
                    throw new InvalidOperationException("An error occured.");
                }
            }
        }

        // Get session name from table
        public static string getSessionName(int sessionId, SqlConnection Con)
        {
            string sessionNameQuery = "SELECT Name FROM [Session] WHERE Id = @SessionId";
            string sessionName = "";

            using (SqlCommand cmdsessionName = new SqlCommand(sessionNameQuery, Con))
            {
                cmdsessionName.Parameters.AddWithValue("@SessionId", sessionId);

                // ExecuteScalar returns the first column of the first row in the result set, or a null reference if the result set is empty.
                var result = cmdsessionName.ExecuteScalar();

                // Successfully parsed sessionStart
                return sessionName;
            }
        }

        // Returns year that student wrote final exam
        public static int GetLastExamYear(int studentId, SqlConnection Con)
        {
            // Query to get session IDs from the Result table for the given student ID
            string sessionIdsQuery = "SELECT Session FROM [Result] WHERE Student = @StudentId";

            // Query to get the start year from the Session table for the given session IDs
            string sessionStartQuery = "SELECT MAX(Start) FROM [Session] WHERE Id IN (@SessionIds)";

            List<int> sessionIds = new List<int>();
            using (SqlCommand cmdSessionIds = new SqlCommand(sessionIdsQuery, Con))
            {
                cmdSessionIds.Parameters.AddWithValue("@StudentId", studentId);
                using (SqlDataReader reader = cmdSessionIds.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        sessionIds.Add(reader.GetInt32(0));
                    }
                }
            }

            if (sessionIds.Count == 0)
            {
                // No sessions found for the student
                // No exam sessions found for the given student ID
                throw new InvalidOperationException("An error occured.");
            }

            // Prepare a parameterized list of session IDs for the IN clause
            string sessionIdsParam = string.Join(",", sessionIds);

            int lastExamYear = 0;
            using (SqlCommand cmdSessionStart = new SqlCommand(sessionStartQuery.Replace("@SessionIds", sessionIdsParam), Con))
            {
                var result = cmdSessionStart.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out lastExamYear))
                {
                    return lastExamYear;
                }
                else
                {
                    //Failed to retrieve the last exam year
                    throw new InvalidOperationException("An error occured.");
                }
            }
        }

        // Returns number of student used years
        public static int CheckStudentGraduateNo(string studentTableId, SqlConnection Con)
        {

            // Get the student's program ID and student year
            string studentQuery = "SELECT Program, Year FROM [Student] WHERE Id = @StudentId";
            string programId = "";
            int studentStartSession = 0;

            using (SqlCommand cmdStudentData = new SqlCommand(studentQuery, Con))
            {
                cmdStudentData.Parameters.AddWithValue("@StudentId", studentTableId);
                using (SqlDataReader readerStudentData = cmdStudentData.ExecuteReader())
                {
                    if (readerStudentData.Read())
                    {
                        programId = readerStudentData["Program"].ToString();
                        
                        //studentStartSession = readerStudentData.GetInt32(readerStudentData.GetOrdinal("Year"));
                        string studentStartSessionStr = readerStudentData["Year"].ToString();
                        if (!int.TryParse(studentStartSessionStr, out studentStartSession))
                        {
                            //The 'Year' field is not a valid integer.
                            throw new InvalidOperationException("An error occured.");
                        }
                    }
                }
            }

            // Get the program maximum and current session
            string programQuery = "SELECT Total, Session FROM [Program] WHERE Id = @ProgramId";
            int programMax = 0;
            int programCurrent = 0;

            using (SqlCommand cmdProgramData = new SqlCommand(programQuery, Con))
            {
                cmdProgramData.Parameters.AddWithValue("@programId", programId);

                using (SqlDataReader readerProgData = cmdProgramData.ExecuteReader())
                {
                    if (readerProgData.Read())
                    {
                        //programMax = readerProgData.GetInt32(readerProgData.GetOrdinal("Total"));
                        string programMaxStr = readerProgData["Total"].ToString();
                        if (!int.TryParse(programMaxStr, out programMax))
                        {
                            //The 'Total' field is not a valid integer.
                            throw new InvalidOperationException("An error occured.");
                        }

                        //programCurrent = readerProgData.GetInt32(readerProgData.GetOrdinal("Session"));
                        string programCurrentStr = readerProgData["Session"].ToString();
                        if (!int.TryParse(programCurrentStr, out programCurrent))
                        {
                            //The 'Session' field is not a valid integer.
                            throw new InvalidOperationException("An error occured.");
                        }
                    }
                }
            }
            // Get the start and current session start years
            int studentStart = getSessionStart(studentStartSession, Con);
            int programCurrentSession = getSessionStart(programCurrent, Con);
            int studentUsed = 0;


            if ((studentStart + ((int)Math.Ceiling((double)programMax / (double)2))) <= programCurrentSession) 
            {
                
                List<int> carryOvers = StudentSessionExtra(studentTableId, programCurrent,  Con);
                if (carryOvers.Count == 0)
                {
                    int lastYearExam = GetLastExamYear(Int32.Parse(studentTableId), Con);
                    if (programCurrentSession < lastYearExam)
                    {
                        studentUsed = (programCurrentSession - studentStart) + 1;
                    }
                    else
                    {
                        studentUsed = (lastYearExam - studentStart) + 1;
                    }
                }
                else
                {
                    studentUsed = (programCurrentSession - studentStart) + 1;
                }
            }
            else
            {
                studentUsed = (programCurrentSession - studentStart) + 1;
                
            }

            return studentUsed;
        }
    
        public static int GetStudentLevel(int studentId, SqlConnection con)
        {
            int studentLevel = 0;
            int programId;
            //int maxSemester;
            int currentSession = 0;
            int startSession;
            //string programSemester;
            int studentStart;


            int currentInt;
            int maxInt = 0;

            string loadStdResult = $"SELECT * FROM [Student] where Id = '{studentId}'";
            SqlCommand cmdStdResult = new SqlCommand(loadStdResult, con);
            SqlDataReader readerStdResult = cmdStdResult.ExecuteReader();

            if (readerStdResult.Read())
            {
                programId = int.Parse(readerStdResult.GetString("Program"));
                studentStart = getSessionStart(int.Parse(readerStdResult.GetString("Year")), con);

                string loadProgData = "SELECT * FROM [Program] WHERE Id = @StudentProgramId";
                SqlCommand cmdProgData = new SqlCommand(loadProgData, con);
                cmdProgData.Parameters.AddWithValue("@StudentProgramId", programId);
                using (SqlDataReader readerProgData = cmdProgData.ExecuteReader())
                {
                    if (readerProgData.Read())
                    {
                        //maxSemester = readerProgData.GetInt32("Total");
                        //currentSession = readerProgData.GetInt32("Session");
                        currentInt = int.Parse(readerProgData.GetString("Session"));
                        maxInt = int.Parse(readerProgData.GetString("Total"));
                    }
                }

                string loadData = "SELECT * FROM [Session] WHERE Id = @currentSession ORDER BY EndSession ASC";
                SqlCommand cmdData = new SqlCommand(loadData, con);
                cmdData.Parameters.AddWithValue("@currentSession", currentSession);
                using (SqlDataReader readerData = cmdData.ExecuteReader())
                {
                    if (readerData.Read())
                    {
                        startSession = readerData.GetInt32(readerData.GetOrdinal("Start"));
                        //programSemester = readerData["Semester"].ToString();
                    }
                }

                //string stdIdConvert = studentTableId.ToString();

                int noOfYearsStudentUsed = Globals.CheckStudentGraduateNo(studentId.ToString(), con);

                int semesterStudentUsed = maxInt % 2 == 0 ? noOfYearsStudentUsed * 2 : (noOfYearsStudentUsed * 2) - 1;
                int maxYear = (int)Math.Ceiling((double)maxInt / 2);

                string loadProg = $"SELECT TOP {semesterStudentUsed} * FROM [Session] WHERE Start >= @studentStart ORDER BY EndSession ASC";
                SqlCommand cmdProg = new SqlCommand(loadProg, con);
                //cmdProg.Parameters.AddWithValue("@semesterStudentUsed", semesterStudentUsed);
                cmdProg.Parameters.AddWithValue("@studentStart", studentStart);

                using (SqlDataReader readerTop = cmdProg.ExecuteReader())
                {
                    int i = 1;
                    int j = i;
                    int repYear = 0;

                    while (readerTop.Read())
                    {
                        j = i % 2 == 0 ? i / 2 : (int)Math.Ceiling((double)i / 2);
                        j = j > maxYear ? maxYear : j;

                        string semester = readerTop["Semester"].ToString();
                        
                        studentLevel = j;

                        i++;
                    }

                }
            }

            return studentLevel;
        }
        
        public static List<int> StudentSessionExtra(string studentTableId, int session, SqlConnection Con)
        {
            string semester = "";
            string programId = "";
            string programMax = "";
            int programMaxDiv = 0;
            string studentStartId = "";
            int studentStart = 0;
            int studentLevel = 0;
            Int32 sessionStart = 0;
            string sessionName = "";

            sessionStart = getSessionStart(session, Con);
            

            // Query session table to get session details
            string loadSummarySession = $"SELECT * FROM [Session] where Id = '{session}'";
            SqlCommand cmdSummarySession = new SqlCommand(loadSummarySession, Con);
            SqlDataReader readerSummarySession = cmdSummarySession.ExecuteReader();
            if (readerSummarySession.Read())
            {
                // Current semester we are working with
                semester = readerSummarySession.GetString("Semester")[0].ToString();
                sessionName = readerSummarySession.GetString("Name");
            }

            // Get all student's compulsory courses up until that semester


            // Query to get the student's program ID and admission year ID based on their student ID and student admission year
            string studentQuery = $"SELECT * FROM [Student] WHERE Id = '{studentTableId}'";
            SqlCommand cmdStudentQuery = new SqlCommand(studentQuery, Con);
            SqlDataReader readerStudentQuery = cmdStudentQuery.ExecuteReader();
            if (readerStudentQuery.Read())
            {
                programId = readerStudentQuery.GetString("Program");
                studentStartId = readerStudentQuery.GetString("Year");
                studentStart = getSessionStart(Int32.Parse(studentStartId), Con);
            }

            // Get maximum semester of Student
            string loadProgData = $"SELECT * FROM [Program] where Id = '{programId}'";
            SqlCommand cmdProgData = new SqlCommand(loadProgData, Con);
            SqlDataReader readerProgData = cmdProgData.ExecuteReader();
            if (readerProgData.Read())
            {
                programMax = readerProgData.GetString("Total");
                programMaxDiv = (int)Math.Ceiling((double)Int32.Parse(programMax) / (double)2);
            }

            // If student admission year is greater than the year we are working on, throw an error exception
            // I dont understad
            if (studentStart > sessionStart)
            {
                throw new InvalidOperationException("An error occured.");
            }

            // Initialize a list to store the IDs of compulsory courses
            List<int> allCompulsoryCourses = new List<int>();

            // Get Student's level
            studentLevel = (sessionStart - studentStart) + 1;

            // If student's level is higher than maximum year of program, then student has graduated
            if (studentLevel > programMaxDiv)
            {
                // Query to get the IDs of compulsory courses for the student's program
                string compulsoryCoursesQuery = "SELECT Id FROM [Course] WHERE Program = @ProgramId AND Type = 'C'";

                // Execute the query to retrieve the compulsory course IDs
                using (SqlCommand cmdCompulsoryCourses = new SqlCommand(compulsoryCoursesQuery, Con))
                {
                    cmdCompulsoryCourses.Parameters.AddWithValue("@ProgramId", programId);
                    using (SqlDataReader readerCompulsoryCourses = cmdCompulsoryCourses.ExecuteReader())
                    {
                        // Add each compulsory course ID to the list
                        while (readerCompulsoryCourses.Read())
                        {
                            allCompulsoryCourses.Add(readerCompulsoryCourses.GetInt32(readerCompulsoryCourses.GetOrdinal("Id")));
                        }
                    }
                }
            }
            else
            {
                
                // Handle logic based on semester type (Harmattan or Rain)
                string compulsoryCoursesQuery = "SELECT Id, Level, Semester FROM [Course] WHERE Program = @ProgramId AND Type = 'C'";

                // Execute the query to retrieve the compulsory course IDs
                using (SqlCommand cmdCompulsoryCourses = new SqlCommand(compulsoryCoursesQuery, Con))
                {
                    
                    cmdCompulsoryCourses.Parameters.AddWithValue("@ProgramId", programId);

                    using (SqlDataReader readerCompulsoryCourses = cmdCompulsoryCourses.ExecuteReader())
                    {
                        while (readerCompulsoryCourses.Read())
                        {
                            string courseLevel = readerCompulsoryCourses.GetString(readerCompulsoryCourses.GetOrdinal("Level"));
                            string courseSemester = readerCompulsoryCourses.GetString(readerCompulsoryCourses.GetOrdinal("Semester"));

                            if (int.Parse(courseLevel[0].ToString()) <= studentLevel)
                            {
                                if (semester == "H")
                                {
                                    // For Harmattan, skip courses in the same level but Rain semester
                                    if (courseLevel[0].ToString() == studentLevel.ToString() && courseSemester == "R")
                                    {
                                        continue; // Skip Rain semester courses of the same level
                                    }
                                }
                                //MessageBox.Show($"{courseLevel[0].ToString()} {studentLevel} {courseSemester} {semester}");
                                allCompulsoryCourses.Add(readerCompulsoryCourses.GetInt32(readerCompulsoryCourses.GetOrdinal("Id")));
                            }

                            // Debug: Check the actual values of Level and Semester being compared

                            //MessageBox.Show($"Course Level: {courseLevel}, Student Level: {studentLevel}, Course Semester: {courseSemester}, Current Semester: {semester}");



                            // Add each compulsory course ID to the list
                            //allCompulsoryCourses.Add(readerCompulsoryCourses.GetInt32(readerCompulsoryCourses.GetOrdinal("Id")));
                            //MessageBox.Show($"{readerCompulsoryCourses.GetString(readerCompulsoryCourses.GetOrdinal("Semester"))} {semester}");
                        }
                    }
                }
            }







            //List<int> compulsoryCourses = new List<int>();
            /*
            int courseId;
            Int32 scores;
            string querySresult = "SELECT * FROM [Result] where Student = '" + studentTableId + "'";
            SqlCommand cmdSresult = new SqlCommand(querySresult, Con);
            SqlDataReader readerSresult = cmdSresult.ExecuteReader();

            while (readerSresult.Read())
            {
                int resultSessionId = readerSresult.GetInt32("Session");

                courseId = readerSresult.GetInt32("Course");

                int sSessionStart = getSessionStart(resultSessionId, Con);
                string sSessionName = getSessionName(resultSessionId, Con);

                if (sessionStart >= sSessionStart)
                {
                        //string unit = "";

                    string loadCourse = "SELECT * FROM [Course] where Id = '" + courseId + "'";
                    SqlCommand cmdCourse = new SqlCommand(loadCourse, Con);
                    SqlDataReader readerCourse = cmdCourse.ExecuteReader();

                    if (readerCourse.Read())
                    {
                        if (semester == "H") //it is first semester
                        {
                            if ((sessionName == sSessionName) && (readerCourse.GetString("Semester") == "R"))
                            {
                                //don't fetch second semester for current session
                            }
                            else
                            {
                                scores = readerSresult.GetInt32("Score");

                                if (scores > 39)
                                {
                                    allCompulsoryCourses.Remove(courseId);
                                }
                                if ((scores < 40) && (scores >= 0))
                                {
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
                                            courseId = readerData.GetInt32(readerData.GetOrdinal("Id"));
                                            if (allCompulsoryCourses.Contains(courseId))
                                            {
                                            }
                                            else
                                            {
                                                // If newEntry2 doesn't exist, add newEntry
                                                if (!allCompulsoryCourses.Contains(courseId))
                                                {
                                                    allCompulsoryCourses.Add(courseId);
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
                                            courseId = readerData2.GetInt32(readerData2.GetOrdinal("Id"));
                                            if (allCompulsoryCourses.Contains(courseId))
                                            {
                                            }
                                            else
                                            {
                                                // If newEntry2 doesn't exist, add newEntry
                                                if (!allCompulsoryCourses.Contains(courseId))
                                                {
                                                    allCompulsoryCourses.Add(courseId);
                                                }
                                            }
                                        }
                                    }
                                }


                            }
                        }
                        else //it is second semester fetch all result
                        {
                            scores = readerSresult.GetInt32("Score");

                            if (scores > 39)
                            {
                                allCompulsoryCourses.Remove(courseId);
                            }

                            if ((scores < 40) && (scores >= 0))
                            {
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
                                        courseId = readerData.GetInt32(readerData.GetOrdinal("Id"));
                                        if (allCompulsoryCourses.Contains(courseId))
                                        {
                                        }
                                        else
                                        {
                                            // If newEntry2 doesn't exist, add newEntry
                                            if (!allCompulsoryCourses.Contains(courseId))
                                            {
                                                allCompulsoryCourses.Add(courseId);
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
                                        courseId = readerData2.GetInt32(readerData2.GetOrdinal("Id"));
                                        if (allCompulsoryCourses.Contains(courseId))
                                        {
                                        }
                                        else
                                        {
                                            // If newEntry2 doesn't exist, add newEntry
                                            if (!allCompulsoryCourses.Contains(courseId))
                                            {
                                                allCompulsoryCourses.Add(courseId);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            */
            
            
            int courseId;
            int scores;

            string querySresult = "SELECT * FROM [Result] WHERE Student = @StudentId";
            SqlCommand cmdSresult = new SqlCommand(querySresult, Con);
            cmdSresult.Parameters.AddWithValue("@StudentId", studentTableId);
            SqlDataReader readerSresult = cmdSresult.ExecuteReader();

            while (readerSresult.Read())
            {
                int resultSessionId = readerSresult.GetInt32(readerSresult.GetOrdinal("Session"));
                courseId = readerSresult.GetInt32(readerSresult.GetOrdinal("Course"));

                int sSessionStart = getSessionStart(resultSessionId, Con);

                string sSessionName = "";

                string loadResultSession = $"SELECT * FROM [Session] where Id = '{resultSessionId}'";
                SqlCommand cmdResultSession = new SqlCommand(loadResultSession, Con);
                SqlDataReader readerResultSession = cmdResultSession.ExecuteReader();
                if (readerResultSession.Read())
                {
                    // Session of result currently being looped
                    sSessionName = readerResultSession.GetString("Name");
                }

                if (sessionStart >= sSessionStart)
                {
                    string loadCourse = "SELECT * FROM [Course] WHERE Id = @CourseId";
                    SqlCommand cmdCourse = new SqlCommand(loadCourse, Con);
                    cmdCourse.Parameters.AddWithValue("@CourseId", courseId);
                    SqlDataReader readerCourse = cmdCourse.ExecuteReader();

                    if (readerCourse.Read())
                    {
                        // Handling Harmattan semester
                        if (semester == "H")
                        {
                            // Skip Rain semester of the same session
                             //MessageBox.Show($"{sessionName}, {sSessionName}, {readerCourse.GetString(readerCourse.GetOrdinal("Semester"))}");
                            if (sessionName == sSessionName && readerCourse.GetString(readerCourse.GetOrdinal("Semester")) == "R")
                            {
                                continue;
                            }
                              

                            scores = readerSresult.GetInt32(readerSresult.GetOrdinal("Score"));

                            // Remove if passed
                            if (scores > 39)
                            {
                                allCompulsoryCourses.Remove(courseId);
                            }
                            else if (scores >= 0 && scores < 40)
                            {
                                if (!CheckIfCoursePassed(readerSresult.GetInt32("Course"), Int32.Parse(studentTableId), Con))
                                {
                                    AddCompulsoryCourseIfNotExists(courseId, allCompulsoryCourses, Con);
                                }
                            }
                            else if (scores < 0)
                            {
                                if (!CheckIfCoursePassed(readerSresult.GetInt32("Course"), Int32.Parse(studentTableId), Con))
                                {
                                    AddCompulsoryCourseIfNotExists(courseId, allCompulsoryCourses, Con);
                                }
                            }
                        }
                        else // Handle Rain semester
                        {
                            scores = readerSresult.GetInt32(readerSresult.GetOrdinal("Score"));

                            if (scores > 39)
                            {
                                allCompulsoryCourses.Remove(courseId);
                            }
                            else if (scores >= 0 && scores < 40)
                            {
                                if (!CheckIfCoursePassed(readerSresult.GetInt32("Course"), Int32.Parse(studentTableId), Con))
                                {
                                    AddCompulsoryCourseIfNotExists(courseId, allCompulsoryCourses, Con);
                                }
                            }
                            else if (scores < 0)
                            {
                                if (!CheckIfCoursePassed(readerSresult.GetInt32("Course"), Int32.Parse(studentTableId), Con))
                                {
                                    AddCompulsoryCourseIfNotExists(courseId, allCompulsoryCourses, Con);
                                }
                            }
                        }
                    }
                }
            }

            //if (allCompulsoryCourses.Count() > 0)
            //{
            //    MessageBox.Show(String.Join(", ", allCompulsoryCourses));
            //}


            return allCompulsoryCourses;
        }

        public static bool CheckIfCoursePassed(int courseId, int studentId, SqlConnection con)
        {
            string checkDataQuery = "SELECT COUNT(*) FROM [Result] WHERE Student = @Student AND Course = @Course AND Score > 39";
            using (SqlCommand cmdCheck = new SqlCommand(checkDataQuery, con))
            {
                cmdCheck.Parameters.AddWithValue("@Student", studentId);
                cmdCheck.Parameters.AddWithValue("@Course", courseId);
                return (int)cmdCheck.ExecuteScalar() > 0;
            }
        }

        public static void AddCompulsoryCourseIfNotExists(int courseId, List<int> allCompulsoryCourses, SqlConnection con)
        {
            if (!allCompulsoryCourses.Contains(courseId))
            {
                allCompulsoryCourses.Add(courseId);
            }
        }


        public static void PrintResult(string studentTableId, List<int> sessions, SqlConnection Con)
        {
        }

    }


}


