using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ExcelDataReader;
using GlobalVariable;
using ClosedXML.Excel;

namespace Contextual
{
    /// <summary>
    /// Helper class for bulk uploading data from Excel files
    /// </summary>
    public class BulkUploadHelper
    {
        private readonly SqlConnection _connection;

        public BulkUploadHelper(SqlConnection connection)
        {
            _connection = connection;
        }

        /// <summary>
        /// Result of a bulk upload operation
        /// </summary>
        public class BulkUploadResult
        {
            public int SuccessCount { get; set; }
            public int FailedCount { get; set; }
            public int SkippedCount { get; set; }
            public List<UploadRowResult> RowResults { get; set; } = new List<UploadRowResult>();
        }

        public class UploadRowResult
        {
            public int RowNumber { get; set; }
            public string Identifier { get; set; } = "";
            public string Status { get; set; } = "";
            public string Message { get; set; } = "";
        }

        #region Template Download Methods

        /// <summary>
        /// Downloads a template for student bulk upload
        /// </summary>
        public static void DownloadStudentTemplate()
        {
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Excel Files (*.xlsx)|*.xlsx";
                saveDialog.Title = "Save Student Template";
                saveDialog.FileName = "Student_Upload_Template.xlsx";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (var workbook = new XLWorkbook())
                        {
                            var worksheet = workbook.Worksheets.Add("Students");

                            // Add headers
                            worksheet.Cell(1, 1).Value = "Matric";
                            worksheet.Cell(1, 2).Value = "Surname";
                            worksheet.Cell(1, 3).Value = "Firstname";
                            worksheet.Cell(1, 4).Value = "Middlename";
                            worksheet.Cell(1, 5).Value = "Gender";
                            worksheet.Cell(1, 6).Value = "Session";

                            // Style headers
                            var headerRange = worksheet.Range(1, 1, 1, 6);
                            headerRange.Style.Font.Bold = true;
                            headerRange.Style.Fill.BackgroundColor = XLColor.LightBlue;
                            headerRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                            // Add sample data
                            worksheet.Cell(2, 1).Value = "STU/2023/001";
                            worksheet.Cell(2, 2).Value = "Smith";
                            worksheet.Cell(2, 3).Value = "John";
                            worksheet.Cell(2, 4).Value = "David";
                            worksheet.Cell(2, 5).Value = "Male";
                            worksheet.Cell(2, 6).Value = "2023/2024";

                            worksheet.Cell(3, 1).Value = "STU/2023/002";
                            worksheet.Cell(3, 2).Value = "Johnson";
                            worksheet.Cell(3, 3).Value = "Jane";
                            worksheet.Cell(3, 4).Value = "";
                            worksheet.Cell(3, 5).Value = "Female";
                            worksheet.Cell(3, 6).Value = "2023/2024";

                            // Add instructions sheet
                            var instructionSheet = workbook.Worksheets.Add("Instructions");
                            instructionSheet.Cell(1, 1).Value = "Student Upload Template Instructions";
                            instructionSheet.Cell(1, 1).Style.Font.Bold = true;
                            instructionSheet.Cell(1, 1).Style.Font.FontSize = 14;

                            instructionSheet.Cell(3, 1).Value = "Required Columns:";
                            instructionSheet.Cell(3, 1).Style.Font.Bold = true;
                            instructionSheet.Cell(4, 1).Value = "• Matric - Student matriculation number (required)";
                            instructionSheet.Cell(5, 1).Value = "• Surname/Lastname - Student's surname (required)";
                            instructionSheet.Cell(6, 1).Value = "• Firstname - Student's first name (required)";
                            instructionSheet.Cell(7, 1).Value = "• Middlename - Student's middle name (optional)";
                            instructionSheet.Cell(8, 1).Value = "• Gender - Male or Female (or M/F)";
                            instructionSheet.Cell(9, 1).Value = "• Session - Academic session (e.g., 2023/2024)";

                            instructionSheet.Cell(11, 1).Value = "Notes:";
                            instructionSheet.Cell(11, 1).Style.Font.Bold = true;
                            instructionSheet.Cell(12, 1).Value = "• Delete sample data rows before uploading";
                            instructionSheet.Cell(13, 1).Value = "• Session must exist in the system";
                            instructionSheet.Cell(14, 1).Value = "• Duplicate matric numbers will be skipped";

                            // Auto-fit columns
                            worksheet.Columns().AdjustToContents();
                            instructionSheet.Columns().AdjustToContents();

                            workbook.SaveAs(saveDialog.FileName);
                        }

                        MessageBox.Show("Template downloaded successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error creating template: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        /// <summary>
        /// Downloads a template for lecturer bulk upload
        /// </summary>
        public static void DownloadLecturerTemplate()
        {
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Excel Files (*.xlsx)|*.xlsx";
                saveDialog.Title = "Save Lecturer Template";
                saveDialog.FileName = "Lecturer_Upload_Template.xlsx";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (var workbook = new XLWorkbook())
                        {
                            var worksheet = workbook.Worksheets.Add("Lecturers");

                            // Add headers
                            worksheet.Cell(1, 1).Value = "Staff_id";
                            worksheet.Cell(1, 2).Value = "Name";
                            worksheet.Cell(1, 3).Value = "Gender";
                            worksheet.Cell(1, 4).Value = "Email";

                            // Style headers
                            var headerRange = worksheet.Range(1, 1, 1, 4);
                            headerRange.Style.Font.Bold = true;
                            headerRange.Style.Fill.BackgroundColor = XLColor.LightGreen;
                            headerRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                            // Add sample data
                            worksheet.Cell(2, 1).Value = "LEC001";
                            worksheet.Cell(2, 2).Value = "Dr. John Smith";
                            worksheet.Cell(2, 3).Value = "Male";
                            worksheet.Cell(2, 4).Value = "john.smith@university.edu";

                            worksheet.Cell(3, 1).Value = "LEC002";
                            worksheet.Cell(3, 2).Value = "Prof. Jane Doe";
                            worksheet.Cell(3, 3).Value = "Female";
                            worksheet.Cell(3, 4).Value = "jane.doe@university.edu";

                            // Add instructions sheet
                            var instructionSheet = workbook.Worksheets.Add("Instructions");
                            instructionSheet.Cell(1, 1).Value = "Lecturer Upload Template Instructions";
                            instructionSheet.Cell(1, 1).Style.Font.Bold = true;
                            instructionSheet.Cell(1, 1).Style.Font.FontSize = 14;

                            instructionSheet.Cell(3, 1).Value = "Required Columns:";
                            instructionSheet.Cell(3, 1).Style.Font.Bold = true;
                            instructionSheet.Cell(4, 1).Value = "• Staff_id - Unique staff ID (required)";
                            instructionSheet.Cell(5, 1).Value = "• Name - Full name of lecturer (required)";
                            instructionSheet.Cell(6, 1).Value = "• Gender - Male or Female (or M/F)";
                            instructionSheet.Cell(7, 1).Value = "• Email - Email address (optional)";

                            instructionSheet.Cell(9, 1).Value = "Notes:";
                            instructionSheet.Cell(9, 1).Style.Font.Bold = true;
                            instructionSheet.Cell(10, 1).Value = "• Delete sample data rows before uploading";
                            instructionSheet.Cell(11, 1).Value = "• Duplicate Staff IDs will be skipped";

                            // Auto-fit columns
                            worksheet.Columns().AdjustToContents();
                            instructionSheet.Columns().AdjustToContents();

                            workbook.SaveAs(saveDialog.FileName);
                        }

                        MessageBox.Show("Template downloaded successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error creating template: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        /// <summary>
        /// Downloads a template for course bulk upload
        /// </summary>
        public static void DownloadCourseTemplate()
        {
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Excel Files (*.xlsx)|*.xlsx";
                saveDialog.Title = "Save Course Template";
                saveDialog.FileName = "Course_Upload_Template.xlsx";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (var workbook = new XLWorkbook())
                        {
                            var worksheet = workbook.Worksheets.Add("Courses");

                            // Add headers
                            worksheet.Cell(1, 1).Value = "Code";
                            worksheet.Cell(1, 2).Value = "Name";
                            worksheet.Cell(1, 3).Value = "Level";
                            worksheet.Cell(1, 4).Value = "Semester";
                            worksheet.Cell(1, 5).Value = "Unit";
                            worksheet.Cell(1, 6).Value = "Type";

                            // Style headers
                            var headerRange = worksheet.Range(1, 1, 1, 6);
                            headerRange.Style.Font.Bold = true;
                            headerRange.Style.Fill.BackgroundColor = XLColor.LightYellow;
                            headerRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                            // Add sample data
                            worksheet.Cell(2, 1).Value = "CSC101";
                            worksheet.Cell(2, 2).Value = "Introduction to Computer Science";
                            worksheet.Cell(2, 3).Value = "1";
                            worksheet.Cell(2, 4).Value = "H";
                            worksheet.Cell(2, 5).Value = "3";
                            worksheet.Cell(2, 6).Value = "C";

                            worksheet.Cell(3, 1).Value = "CSC102";
                            worksheet.Cell(3, 2).Value = "Programming Fundamentals";
                            worksheet.Cell(3, 3).Value = "1";
                            worksheet.Cell(3, 4).Value = "R";
                            worksheet.Cell(3, 5).Value = "4";
                            worksheet.Cell(3, 6).Value = "C";

                            worksheet.Cell(4, 1).Value = "CSC201";
                            worksheet.Cell(4, 2).Value = "Data Structures";
                            worksheet.Cell(4, 3).Value = "2";
                            worksheet.Cell(4, 4).Value = "H";
                            worksheet.Cell(4, 5).Value = "3";
                            worksheet.Cell(4, 6).Value = "E";

                            // Add instructions sheet
                            var instructionSheet = workbook.Worksheets.Add("Instructions");
                            instructionSheet.Cell(1, 1).Value = "Course Upload Template Instructions";
                            instructionSheet.Cell(1, 1).Style.Font.Bold = true;
                            instructionSheet.Cell(1, 1).Style.Font.FontSize = 14;

                            instructionSheet.Cell(3, 1).Value = "Required Columns:";
                            instructionSheet.Cell(3, 1).Style.Font.Bold = true;
                            instructionSheet.Cell(4, 1).Value = "• Code - Unique course code (required)";
                            instructionSheet.Cell(5, 1).Value = "• Name - Course title (required)";
                            instructionSheet.Cell(6, 1).Value = "• Level - Year level (1, 2, 3, etc.)";
                            instructionSheet.Cell(7, 1).Value = "• Semester - H (Harmattan/First) or R (Rain/Second)";
                            instructionSheet.Cell(8, 1).Value = "• Unit - Credit units (1, 2, 3, etc.)";
                            instructionSheet.Cell(9, 1).Value = "• Type - C (Compulsory) or E (Elective)";

                            instructionSheet.Cell(11, 1).Value = "Notes:";
                            instructionSheet.Cell(11, 1).Style.Font.Bold = true;
                            instructionSheet.Cell(12, 1).Value = "• Delete sample data rows before uploading";
                            instructionSheet.Cell(13, 1).Value = "• Duplicate course codes in the same program will be skipped";

                            // Auto-fit columns
                            worksheet.Columns().AdjustToContents();
                            instructionSheet.Columns().AdjustToContents();

                            workbook.SaveAs(saveDialog.FileName);
                        }

                        MessageBox.Show("Template downloaded successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error creating template: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        /// <summary>
        /// Downloads a template for result bulk upload
        /// </summary>
        public static void DownloadResultTemplate()
        {
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Excel Files (*.xlsx)|*.xlsx";
                saveDialog.Title = "Save Result Template";
                saveDialog.FileName = "Result_Upload_Template.xlsx";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (var workbook = new XLWorkbook())
                        {
                            var worksheet = workbook.Worksheets.Add("Results");

                            // Add headers
                            worksheet.Cell(1, 1).Value = "Matric";
                            worksheet.Cell(1, 2).Value = "Score";

                            // Style headers
                            var headerRange = worksheet.Range(1, 1, 1, 2);
                            headerRange.Style.Font.Bold = true;
                            headerRange.Style.Fill.BackgroundColor = XLColor.LightCoral;
                            headerRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                            // Add sample data
                            worksheet.Cell(2, 1).Value = "STU/2023/001";
                            worksheet.Cell(2, 2).Value = 75;

                            worksheet.Cell(3, 1).Value = "STU/2023/002";
                            worksheet.Cell(3, 2).Value = 68;

                            worksheet.Cell(4, 1).Value = "STU/2023/003";
                            worksheet.Cell(4, 2).Value = 82;

                            // Add instructions sheet
                            var instructionSheet = workbook.Worksheets.Add("Instructions");
                            instructionSheet.Cell(1, 1).Value = "Result Upload Template Instructions";
                            instructionSheet.Cell(1, 1).Style.Font.Bold = true;
                            instructionSheet.Cell(1, 1).Style.Font.FontSize = 14;

                            instructionSheet.Cell(3, 1).Value = "Required Columns:";
                            instructionSheet.Cell(3, 1).Style.Font.Bold = true;
                            instructionSheet.Cell(4, 1).Value = "• Matric - Student matriculation number (required)";
                            instructionSheet.Cell(5, 1).Value = "• Score/TOTAL - Exam score (required, 0-100)";

                            instructionSheet.Cell(7, 1).Value = "Notes:";
                            instructionSheet.Cell(7, 1).Style.Font.Bold = true;
                            instructionSheet.Cell(8, 1).Value = "• Delete sample data rows before uploading";
                            instructionSheet.Cell(9, 1).Value = "• Select the course and session before uploading";
                            instructionSheet.Cell(10, 1).Value = "• Students must exist in the program";
                            instructionSheet.Cell(11, 1).Value = "• Existing results will be updated with new scores";
                            instructionSheet.Cell(12, 1).Value = "• Column can also be named 'TOTAL' or 'Mark'";

                            // Auto-fit columns
                            worksheet.Columns().AdjustToContents();
                            instructionSheet.Columns().AdjustToContents();

                            workbook.SaveAs(saveDialog.FileName);
                        }

                        MessageBox.Show("Template downloaded successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error creating template: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        #endregion

        /// <summary>
        /// Reads an Excel file and returns a DataTable
        /// </summary>
        public static DataTable ReadExcelFile(string filePath, int headerRowIndex = 0)
        {
            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var result = reader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                        {
                            UseHeaderRow = false
                        }
                    });

                    if (result.Tables.Count == 0 || result.Tables[0].Rows.Count <= headerRowIndex)
                    {
                        throw new Exception("Excel file is empty or does not have enough rows.");
                    }

                    DataTable excelTable = result.Tables[0];

                    // Set column names from header row
                    DataRow headerRow = excelTable.Rows[headerRowIndex];
                    for (int i = 0; i < excelTable.Columns.Count; i++)
                    {
                        string colName = headerRow[i]?.ToString()?.Trim();
                        if (!string.IsNullOrEmpty(colName))
                        {
                            excelTable.Columns[i].ColumnName = colName;
                        }
                    }

                    // Remove header rows
                    for (int i = 0; i <= headerRowIndex; i++)
                    {
                        excelTable.Rows.RemoveAt(0);
                    }

                    return excelTable;
                }
            }
        }

        /// <summary>
        /// Opens a file dialog to select an Excel file
        /// </summary>
        public static string SelectExcelFile()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Excel Files (*.xlsx;*.xls)|*.xlsx;*.xls|All Files (*.*)|*.*";
                openFileDialog.Title = "Select Excel File for Bulk Upload";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    return openFileDialog.FileName;
                }
            }
            return null;
        }

        /// <summary>
        /// Bulk upload students from a DataTable
        /// Expected columns: Matric, Surname, Firstname, Middlename (optional), Gender, Session
        /// </summary>
        public BulkUploadResult BulkUploadStudents(DataTable data, int programId)
        {
            var result = new BulkUploadResult();

            try
            {
                EnsureConnectionOpen();

                int rowNum = 1;
                foreach (DataRow row in data.Rows)
                {
                    if (IsRowEmpty(row)) continue;

                    try
                    {
                        string matric = GetColumnValue(row, "Matric", "MATRIC", "Matric No", "MATRIC NO");
                        string surname = GetColumnValue(row, "Surname", "SURNAME", "Lastname", "LASTNAME", "Last Name");
                        string firstname = GetColumnValue(row, "Firstname", "FIRSTNAME", "First Name", "FIRST NAME");
                        string middlename = GetColumnValue(row, "Middlename", "MIDDLENAME", "Middle Name", "MIDDLE NAME", "Othername", "OTHERNAME") ?? "";
                        string gender = GetColumnValue(row, "Gender", "GENDER", "Sex", "SEX");
                        string sessionName = GetColumnValue(row, "Session", "SESSION", "Year", "YEAR", "Entry Year", "ENTRY YEAR");

                        if (string.IsNullOrEmpty(matric) || string.IsNullOrEmpty(surname) || string.IsNullOrEmpty(firstname))
                        {
                            result.RowResults.Add(new UploadRowResult
                            {
                                RowNumber = rowNum,
                                Identifier = matric ?? "Unknown",
                                Status = "Failed",
                                Message = "Missing required fields (Matric, Surname, or Firstname)"
                            });
                            result.FailedCount++;
                            rowNum++;
                            continue;
                        }

                        // Check if student already exists
                        string checkQuery = "SELECT COUNT(*) FROM [Student] WHERE Matric = @Matric";
                        using (SqlCommand checkCmd = new SqlCommand(checkQuery, _connection))
                        {
                            checkCmd.Parameters.AddWithValue("@Matric", matric);
                            if ((int)checkCmd.ExecuteScalar() > 0)
                            {
                                result.RowResults.Add(new UploadRowResult
                                {
                                    RowNumber = rowNum,
                                    Identifier = matric,
                                    Status = "Skipped",
                                    Message = "Student already exists"
                                });
                                result.SkippedCount++;
                                rowNum++;
                                continue;
                            }
                        }

                        // Get session ID
                        int sessionId = GetSessionIdByName(sessionName);
                        if (sessionId == 0)
                        {
                            result.RowResults.Add(new UploadRowResult
                            {
                                RowNumber = rowNum,
                                Identifier = matric,
                                Status = "Failed",
                                Message = $"Session '{sessionName}' not found"
                            });
                            result.FailedCount++;
                            rowNum++;
                            continue;
                        }

                        // Normalize gender
                        gender = NormalizeGender(gender);

                        // Insert student
                        string insertQuery = @"INSERT INTO Student (Matric, Lastname, Firstname, Middlename, Gender, Program, Year) 
                                              VALUES (@Matric, @Lastname, @Firstname, @Middlename, @Gender, @Program, @Year)";
                        using (SqlCommand insertCmd = new SqlCommand(insertQuery, _connection))
                        {
                            insertCmd.Parameters.AddWithValue("@Matric", matric);
                            insertCmd.Parameters.AddWithValue("@Lastname", surname);
                            insertCmd.Parameters.AddWithValue("@Firstname", firstname);
                            insertCmd.Parameters.AddWithValue("@Middlename", middlename);
                            insertCmd.Parameters.AddWithValue("@Gender", gender);
                            insertCmd.Parameters.AddWithValue("@Program", programId.ToString());
                            insertCmd.Parameters.AddWithValue("@Year", sessionId.ToString());
                            insertCmd.ExecuteNonQuery();
                        }

                        result.RowResults.Add(new UploadRowResult
                        {
                            RowNumber = rowNum,
                            Identifier = matric,
                            Status = "Success",
                            Message = "Student added successfully"
                        });
                        result.SuccessCount++;
                    }
                    catch (Exception ex)
                    {
                        result.RowResults.Add(new UploadRowResult
                        {
                            RowNumber = rowNum,
                            Identifier = "Row " + rowNum,
                            Status = "Failed",
                            Message = ex.Message
                        });
                        result.FailedCount++;
                    }
                    rowNum++;
                }
            }
            finally
            {
                CloseConnectionIfNeeded();
            }

            return result;
        }

        /// <summary>
        /// Bulk upload lecturers from a DataTable
        /// Expected columns: Staff_id, Name, Gender, Email (optional)
        /// </summary>
        public BulkUploadResult BulkUploadLecturers(DataTable data)
        {
            var result = new BulkUploadResult();

            try
            {
                EnsureConnectionOpen();

                int rowNum = 1;
                foreach (DataRow row in data.Rows)
                {
                    if (IsRowEmpty(row)) continue;

                    try
                    {
                        string staffId = GetColumnValue(row, "Staff_id", "STAFF_ID", "Staff ID", "STAFF ID", "StaffId", "STAFFID");
                        string name = GetColumnValue(row, "Name", "NAME", "Full Name", "FULL NAME", "Fullname", "FULLNAME");
                        string gender = GetColumnValue(row, "Gender", "GENDER", "Sex", "SEX");
                        string email = GetColumnValue(row, "Email", "EMAIL", "E-mail", "E-MAIL") ?? "";

                        if (string.IsNullOrEmpty(staffId) || string.IsNullOrEmpty(name))
                        {
                            result.RowResults.Add(new UploadRowResult
                            {
                                RowNumber = rowNum,
                                Identifier = staffId ?? "Unknown",
                                Status = "Failed",
                                Message = "Missing required fields (Staff ID or Name)"
                            });
                            result.FailedCount++;
                            rowNum++;
                            continue;
                        }

                        // Check if lecturer already exists
                        string checkQuery = "SELECT COUNT(*) FROM [Lecturer] WHERE Staff_id = @StaffId";
                        using (SqlCommand checkCmd = new SqlCommand(checkQuery, _connection))
                        {
                            checkCmd.Parameters.AddWithValue("@StaffId", staffId);
                            if ((int)checkCmd.ExecuteScalar() > 0)
                            {
                                result.RowResults.Add(new UploadRowResult
                                {
                                    RowNumber = rowNum,
                                    Identifier = staffId,
                                    Status = "Skipped",
                                    Message = "Lecturer already exists"
                                });
                                result.SkippedCount++;
                                rowNum++;
                                continue;
                            }
                        }

                        gender = NormalizeGender(gender);

                        // Insert lecturer
                        string insertQuery = @"INSERT INTO Lecturer (Staff_id, Name, Gender, Email, Phone) 
                                              VALUES (@StaffId, @Name, @Gender, @Email, '')";
                        using (SqlCommand insertCmd = new SqlCommand(insertQuery, _connection))
                        {
                            insertCmd.Parameters.AddWithValue("@StaffId", staffId);
                            insertCmd.Parameters.AddWithValue("@Name", name);
                            insertCmd.Parameters.AddWithValue("@Gender", gender);
                            insertCmd.Parameters.AddWithValue("@Email", email);
                            insertCmd.ExecuteNonQuery();
                        }

                        result.RowResults.Add(new UploadRowResult
                        {
                            RowNumber = rowNum,
                            Identifier = staffId,
                            Status = "Success",
                            Message = "Lecturer added successfully"
                        });
                        result.SuccessCount++;
                    }
                    catch (Exception ex)
                    {
                        result.RowResults.Add(new UploadRowResult
                        {
                            RowNumber = rowNum,
                            Identifier = "Row " + rowNum,
                            Status = "Failed",
                            Message = ex.Message
                        });
                        result.FailedCount++;
                    }
                    rowNum++;
                }
            }
            finally
            {
                CloseConnectionIfNeeded();
            }

            return result;
        }

        /// <summary>
        /// Bulk upload courses from a DataTable
        /// Expected columns: Code, Name, Level, Semester, Unit, Type
        /// </summary>
        public BulkUploadResult BulkUploadCourses(DataTable data, int programId)
        {
            var result = new BulkUploadResult();

            try
            {
                EnsureConnectionOpen();

                int rowNum = 1;
                foreach (DataRow row in data.Rows)
                {
                    if (IsRowEmpty(row)) continue;

                    try
                    {
                        string code = GetColumnValue(row, "Code", "CODE", "Course Code", "COURSE CODE");
                        string name = GetColumnValue(row, "Name", "NAME", "Title", "TITLE", "Course Title", "COURSE TITLE");
                        string level = GetColumnValue(row, "Level", "LEVEL", "Year", "YEAR");
                        string semester = GetColumnValue(row, "Semester", "SEMESTER", "Sem", "SEM");
                        string unit = GetColumnValue(row, "Unit", "UNIT", "Credit", "CREDIT", "Credit Unit", "CREDIT UNIT");
                        string type = GetColumnValue(row, "Type", "TYPE", "Course Type", "COURSE TYPE") ?? "C";

                        if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(name))
                        {
                            result.RowResults.Add(new UploadRowResult
                            {
                                RowNumber = rowNum,
                                Identifier = code ?? "Unknown",
                                Status = "Failed",
                                Message = "Missing required fields (Code or Name)"
                            });
                            result.FailedCount++;
                            rowNum++;
                            continue;
                        }

                        // Check if course already exists in this program
                        string checkQuery = "SELECT COUNT(*) FROM [Course] WHERE Code = @Code AND Program = @Program";
                        using (SqlCommand checkCmd = new SqlCommand(checkQuery, _connection))
                        {
                            checkCmd.Parameters.AddWithValue("@Code", code);
                            checkCmd.Parameters.AddWithValue("@Program", programId);
                            if ((int)checkCmd.ExecuteScalar() > 0)
                            {
                                result.RowResults.Add(new UploadRowResult
                                {
                                    RowNumber = rowNum,
                                    Identifier = code,
                                    Status = "Skipped",
                                    Message = "Course already exists in this program"
                                });
                                result.SkippedCount++;
                                rowNum++;
                                continue;
                            }
                        }

                        // Normalize semester and type
                        semester = NormalizeSemester(semester);
                        type = NormalizeCourseType(type);

                        // Insert course
                        string insertQuery = @"INSERT INTO Course (Code, Name, Program, Level, Semester, Unit, Type) 
                                              VALUES (@Code, @Name, @Program, @Level, @Semester, @Unit, @Type)";
                        using (SqlCommand insertCmd = new SqlCommand(insertQuery, _connection))
                        {
                            insertCmd.Parameters.AddWithValue("@Code", code);
                            insertCmd.Parameters.AddWithValue("@Name", name);
                            insertCmd.Parameters.AddWithValue("@Program", programId.ToString());
                            insertCmd.Parameters.AddWithValue("@Level", level);
                            insertCmd.Parameters.AddWithValue("@Semester", semester);
                            insertCmd.Parameters.AddWithValue("@Unit", unit);
                            insertCmd.Parameters.AddWithValue("@Type", type);
                            insertCmd.ExecuteNonQuery();
                        }

                        result.RowResults.Add(new UploadRowResult
                        {
                            RowNumber = rowNum,
                            Identifier = code,
                            Status = "Success",
                            Message = "Course added successfully"
                        });
                        result.SuccessCount++;
                    }
                    catch (Exception ex)
                    {
                        result.RowResults.Add(new UploadRowResult
                        {
                            RowNumber = rowNum,
                            Identifier = "Row " + rowNum,
                            Status = "Failed",
                            Message = ex.Message
                        });
                        result.FailedCount++;
                    }
                    rowNum++;
                }
            }
            finally
            {
                CloseConnectionIfNeeded();
            }

            return result;
        }

        /// <summary>
        /// Bulk upload results from a DataTable
        /// Expected columns: Matric, Score (or TOTAL)
        /// </summary>
        public BulkUploadResult BulkUploadResults(DataTable data, int programId, int courseId, int sessionId, int passGrade = 40)
        {
            var result = new BulkUploadResult();

            try
            {
                EnsureConnectionOpen();

                // Get course level
                int courseLevel = 0;
                string getCourseQuery = "SELECT Level FROM [Course] WHERE Id = @CourseId";
                using (SqlCommand cmd = new SqlCommand(getCourseQuery, _connection))
                {
                    cmd.Parameters.AddWithValue("@CourseId", courseId);
                    var levelResult = cmd.ExecuteScalar();
                    if (levelResult != null)
                    {
                        int.TryParse(levelResult.ToString(), out courseLevel);
                    }
                }

                int rowNum = 1;
                foreach (DataRow row in data.Rows)
                {
                    if (IsRowEmpty(row)) continue;

                    try
                    {
                        string matric = GetColumnValue(row, "Matric", "MATRIC", "Matric No", "MATRIC NO", "MATRIC No");
                        string scoreStr = GetColumnValue(row, "Score", "SCORE", "Total", "TOTAL", "Mark", "MARK");

                        if (string.IsNullOrEmpty(matric))
                        {
                            rowNum++;
                            continue;
                        }

                        if (string.IsNullOrEmpty(scoreStr) || !int.TryParse(scoreStr, out int score))
                        {
                            result.RowResults.Add(new UploadRowResult
                            {
                                RowNumber = rowNum,
                                Identifier = matric,
                                Status = "Failed",
                                Message = "Invalid or missing score"
                            });
                            result.FailedCount++;
                            rowNum++;
                            continue;
                        }

                        // Get student ID
                        int studentId = GetStudentIdByMatric(matric, programId);
                        if (studentId == 0)
                        {
                            result.RowResults.Add(new UploadRowResult
                            {
                                RowNumber = rowNum,
                                Identifier = matric,
                                Status = "Failed",
                                Message = "Student not found in this program"
                            });
                            result.FailedCount++;
                            rowNum++;
                            continue;
                        }

                        // Check student level
                        int studentLevel = Globals.GetStudentLevel(studentId, _connection);
                        if (studentLevel < courseLevel)
                        {
                            result.RowResults.Add(new UploadRowResult
                            {
                                RowNumber = rowNum,
                                Identifier = matric,
                                Status = "Failed",
                                Message = $"Student level ({studentLevel}) is below course level ({courseLevel})"
                            });
                            result.FailedCount++;
                            rowNum++;
                            continue;
                        }

                        // Check if result already exists
                        string checkQuery = "SELECT COUNT(*) FROM [Result] WHERE Student = @Student AND Course = @Course AND Session = @Session";
                        using (SqlCommand checkCmd = new SqlCommand(checkQuery, _connection))
                        {
                            checkCmd.Parameters.AddWithValue("@Student", studentId);
                            checkCmd.Parameters.AddWithValue("@Course", courseId);
                            checkCmd.Parameters.AddWithValue("@Session", sessionId);
                            if ((int)checkCmd.ExecuteScalar() > 0)
                            {
                                // Update existing result
                                string updateQuery = "UPDATE [Result] SET Score = @Score WHERE Student = @Student AND Course = @Course AND Session = @Session";
                                using (SqlCommand updateCmd = new SqlCommand(updateQuery, _connection))
                                {
                                    updateCmd.Parameters.AddWithValue("@Score", score);
                                    updateCmd.Parameters.AddWithValue("@Student", studentId);
                                    updateCmd.Parameters.AddWithValue("@Course", courseId);
                                    updateCmd.Parameters.AddWithValue("@Session", sessionId);
                                    updateCmd.ExecuteNonQuery();
                                }

                                result.RowResults.Add(new UploadRowResult
                                {
                                    RowNumber = rowNum,
                                    Identifier = matric,
                                    Status = "Success",
                                    Message = $"Result updated: {score} ({(score >= passGrade ? "Pass" : "Fail")})"
                                });
                                result.SuccessCount++;
                                rowNum++;
                                continue;
                            }
                        }

                        // Insert new result
                        string insertQuery = @"INSERT INTO Result (Student, Course, Session, Score) 
                                              VALUES (@Student, @Course, @Session, @Score)";
                        using (SqlCommand insertCmd = new SqlCommand(insertQuery, _connection))
                        {
                            insertCmd.Parameters.AddWithValue("@Student", studentId);
                            insertCmd.Parameters.AddWithValue("@Course", courseId);
                            insertCmd.Parameters.AddWithValue("@Session", sessionId);
                            insertCmd.Parameters.AddWithValue("@Score", score);
                            insertCmd.ExecuteNonQuery();
                        }

                        result.RowResults.Add(new UploadRowResult
                        {
                            RowNumber = rowNum,
                            Identifier = matric,
                            Status = "Success",
                            Message = $"Result uploaded: {score} ({(score >= passGrade ? "Pass" : "Fail")})"
                        });
                        result.SuccessCount++;
                    }
                    catch (Exception ex)
                    {
                        result.RowResults.Add(new UploadRowResult
                        {
                            RowNumber = rowNum,
                            Identifier = "Row " + rowNum,
                            Status = "Failed",
                            Message = ex.Message
                        });
                        result.FailedCount++;
                    }
                    rowNum++;
                }
            }
            finally
            {
                CloseConnectionIfNeeded();
            }

            return result;
        }

        #region Helper Methods

        private bool _connectionOpenedByHelper = false;

        private void EnsureConnectionOpen()
        {
            if (_connection.State != ConnectionState.Open)
            {
                _connection.Open();
                _connectionOpenedByHelper = true;
            }
            else
            {
                _connectionOpenedByHelper = false;
            }
        }

        private void CloseConnectionIfNeeded()
        {
            if (_connectionOpenedByHelper && _connection.State == ConnectionState.Open)
            {
                _connection.Close();
                _connectionOpenedByHelper = false;
            }
        }

        private bool IsRowEmpty(DataRow row)
        {
            return row.ItemArray.All(field => string.IsNullOrWhiteSpace(field?.ToString()));
        }

        private string GetColumnValue(DataRow row, params string[] possibleColumnNames)
        {
            foreach (var colName in possibleColumnNames)
            {
                if (row.Table.Columns.Contains(colName))
                {
                    var value = row[colName]?.ToString()?.Trim();
                    if (!string.IsNullOrEmpty(value))
                        return value;
                }
            }
            return null;
        }

        private int GetSessionIdByName(string sessionName)
        {
            if (string.IsNullOrEmpty(sessionName)) return 0;

            string query = "SELECT Id FROM [Session] WHERE Name = @Name AND Semester = 'Harmattan'";
            using (SqlCommand cmd = new SqlCommand(query, _connection))
            {
                cmd.Parameters.AddWithValue("@Name", sessionName);
                var result = cmd.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out int id))
                {
                    return id;
                }
            }
            return 0;
        }

        private int GetStudentIdByMatric(string matric, int programId)
        {
            string query = "SELECT Id FROM [Student] WHERE Matric = @Matric AND Program = @Program";
            using (SqlCommand cmd = new SqlCommand(query, _connection))
            {
                cmd.Parameters.AddWithValue("@Matric", matric);
                cmd.Parameters.AddWithValue("@Program", programId.ToString());
                var result = cmd.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out int id))
                {
                    return id;
                }
            }
            return 0;
        }

        private string NormalizeGender(string gender)
        {
            if (string.IsNullOrEmpty(gender)) return "Male";

            gender = gender.Trim().ToUpper();
            if (gender.StartsWith("M")) return "Male";
            if (gender.StartsWith("F")) return "Female";
            return "Male";
        }

        private string NormalizeSemester(string semester)
        {
            if (string.IsNullOrEmpty(semester)) return "H";

            semester = semester.Trim().ToUpper();
            if (semester.StartsWith("H") || semester == "1" || semester.Contains("FIRST") || semester.Contains("HARMATTAN"))
                return "H";
            if (semester.StartsWith("R") || semester == "2" || semester.Contains("SECOND") || semester.Contains("RAIN"))
                return "R";
            return "H";
        }

        private string NormalizeCourseType(string type)
        {
            if (string.IsNullOrEmpty(type)) return "C";

            type = type.Trim().ToUpper();
            if (type.StartsWith("C") || type.Contains("COMPULSORY") || type.Contains("CORE"))
                return "C";
            if (type.StartsWith("E") || type.Contains("ELECTIVE"))
                return "E";
            return "C";
        }

        #endregion
    }
}
