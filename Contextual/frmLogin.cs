using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;
using System.Windows.Markup;
using GlobalVariable;

namespace Contextual
{
    public partial class frmLogin : Form
    {
        bool exists = false;
        static string connectionString = string.Format(@"Server=(LocalDB)\MSSQLLocalDB;AttachDbFilename={0};Integrated Security=True;Connect Timeout=30", GlobalVariable.Globals.databasePath);

        // For dragging the borderless form
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        public frmLogin()
        {
            InitializeComponent();

            // Apply modern styling
            ApplyModernStyling();

            try
            {
                SqlHelper helper = new SqlHelper(connectionString);
                if (helper.IsConnection)
                {
                    AppSetting setting = new AppSetting();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void ApplyModernStyling()
        {
            // Enable double buffering for smooth rendering
            this.DoubleBuffered = true;

            // Apply rounded corners to the login card
            pnlLoginCard.Paint += (s, e) =>
            {
                using (GraphicsPath path = GetRoundedRect(pnlLoginCard.ClientRectangle, 20))
                {
                    pnlLoginCard.Region = new Region(path);
                }
            };

            // Apply rounded corners to password container
            pnlPasswordContainer.Paint += (s, e) =>
            {
                using (GraphicsPath path = GetRoundedRect(pnlPasswordContainer.ClientRectangle, 10))
                {
                    pnlPasswordContainer.Region = new Region(path);
                }
            };

            // Apply rounded corners to buttons
            button1.Paint += (s, e) =>
            {
                using (GraphicsPath path = GetRoundedRect(button1.ClientRectangle, 10))
                {
                    button1.Region = new Region(path);
                }
            };

            button2.Paint += (s, e) =>
            {
                using (GraphicsPath path = GetRoundedRect(button2.ClientRectangle, 10))
                {
                    button2.Region = new Region(path);
                }
            };

            // Add hover effects to window control buttons
            label7.MouseEnter += (s, e) => label7.ForeColor = Color.FromArgb(252, 165, 165);
            label7.MouseLeave += (s, e) => label7.ForeColor = Color.White;
            label8.MouseEnter += (s, e) => label8.ForeColor = Color.FromArgb(199, 210, 254);
            label8.MouseLeave += (s, e) => label8.ForeColor = Color.White;

            // Make title bar draggable
            pnlTitleBar.MouseDown += (s, e) =>
            {
                dragging = true;
                dragCursorPoint = Cursor.Position;
                dragFormPoint = this.Location;
            };
            pnlTitleBar.MouseMove += (s, e) =>
            {
                if (dragging)
                {
                    Point diff = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                    this.Location = Point.Add(dragFormPoint, new Size(diff));
                }
            };
            pnlTitleBar.MouseUp += (s, e) => dragging = false;

            // Also make the main panel draggable (the purple area)
            pnlMain.MouseDown += (s, e) =>
            {
                dragging = true;
                dragCursorPoint = Cursor.Position;
                dragFormPoint = this.Location;
            };
            pnlMain.MouseMove += (s, e) =>
            {
                if (dragging)
                {
                    Point diff = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                    this.Location = Point.Add(dragFormPoint, new Size(diff));
                }
            };
            pnlMain.MouseUp += (s, e) => dragging = false;

            // Focus effect on password field
            txtPassword.Enter += (s, e) =>
            {
                pnlPasswordContainer.BackColor = Color.FromArgb(241, 245, 249);
                txtPassword.BackColor = Color.FromArgb(241, 245, 249);
            };
            txtPassword.Leave += (s, e) =>
            {
                pnlPasswordContainer.BackColor = Color.FromArgb(248, 250, 252);
                txtPassword.BackColor = Color.FromArgb(248, 250, 252);
            };
        }

        private GraphicsPath GetRoundedRect(Rectangle bounds, int radius)
        {
            int diameter = radius * 2;
            Size size = new Size(diameter, diameter);
            Rectangle arc = new Rectangle(bounds.Location, size);
            GraphicsPath path = new GraphicsPath();

            // Top left arc
            path.AddArc(arc, 180, 90);

            // Top right arc
            arc.X = bounds.Right - diameter;
            path.AddArc(arc, 270, 90);

            // Bottom right arc
            arc.Y = bounds.Bottom - diameter;
            path.AddArc(arc, 0, 90);

            // Bottom left arc
            arc.X = bounds.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
            return path;
        }

        SqlConnection Con = new SqlConnection(connectionString);

        private void button2_Click(object sender, EventArgs e)
        {
            txtPassword.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {


            if (txtPassword.Text == "")
            {
                MessageBox.Show("Password cannot be empty");
            }
            else
            {
                Con.Open();

                bool pass_exist = false;
                string chech_pass = "SELECT count(*) FROM [UserTbl] where Pass='" + txtPassword.Text + "'";
                SqlCommand cmdPass = new SqlCommand(chech_pass, Con);
                pass_exist = (int)cmdPass.ExecuteScalar() > 0;

                if (pass_exist)
                {
                    txtPassword.Text = "";

                    // If login is successful
                    contextual parentForm = ApplicationInstance.ContextualForm;
                    if (parentForm != null)
                    {
                        parentForm.formerPage = "frmLogin";
                        parentForm.Show(); // Show the contextual form again
                    }

                    //this.Close(); // Close the login form
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Invalid password");
                }
                Con.Close();

            }
        }

        private void label6_Click(object sender, EventArgs e)
        {
            RgstFrm register = new RgstFrm();
            register.Show();
            this.Hide();
        }

        private void label7_Click(object sender, EventArgs e)
        {
            try
            {
                // Create a list to store the forms to be closed
                List<Form> formsToClose = new List<Form>();

                // Iterate through all open forms and add them to the list
                foreach (Form openForm in Application.OpenForms)
                {
                    if (openForm != this) // Exclude the current form if necessary
                    {
                        formsToClose.Add(openForm);
                    }
                }

                // Close all forms in the list
                foreach (Form form in formsToClose)
                {
                    try
                    {
                        form.Close();
                    }
                    catch (Exception ex)
                    {
                        // Handle any exceptions that occur while closing a form
                        MessageBox.Show($"Error closing form: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                // Close the current form
                this.Close();
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the process
                MessageBox.Show($"Error during form closing process: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void label8_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void checkBxShowPass_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBxShowPass.Checked)
            {
                txtPassword.PasswordChar = '\0';
            }
            else
            {
                txtPassword.PasswordChar = '●';
            }
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            Con.Open();

            //Add Examiner to program table
            //string checkTableProg = "IF NOT EXISTS (SELECT * FROM Sys.columns WHERE Object_ID = Object_ID(N'[db].[Program]') AND Name = 'Examiner') BEGIN ALTER TABLE Program ADD Examiner varchar(MAX) END"; 
            string checkTableProg = "IF NOT EXISTS (SELECT * FROM Sys.columns WHERE Object_ID = Object_ID(N'Program') AND Name = 'Examiner') BEGIN ALTER TABLE Program ADD Examiner varchar(MAX) END";

            SqlCommand command = new SqlCommand(checkTableProg, Con);
            var v = command.ExecuteNonQuery();

            // Check if the column "result_font" exists in the "UserTbl" table
            string checkColumnQuery = "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'UserTbl' AND COLUMN_NAME = 'result_font') BEGIN " + "ALTER TABLE UserTbl ADD Result_font VARCHAR(MAX) NOT NULL DEFAULT '8.5f' END";

            SqlCommand checkColumnCommand = new SqlCommand(checkColumnQuery, Con);
            checkColumnCommand.ExecuteNonQuery();

            // Add Type column to Course table if it does not exist
            string checkCourseTableQuery = @"
            IF NOT EXISTS (SELECT * FROM Sys.columns 
                           WHERE Object_ID = Object_ID(N'Course') 
                           AND Name = 'Type') 
            BEGIN 
                ALTER TABLE Course ADD Type VARCHAR(MAX) 
                NOT NULL DEFAULT 'E' 
            END"
            ;

            SqlCommand checkCourseCommand = new SqlCommand(checkCourseTableQuery, Con);
            checkCourseCommand.ExecuteNonQuery();

            // Add max_unit to Program table if it does not exist
            string checkMaxUnitQuery = @"
                IF NOT EXISTS (
                SELECT * 
            FROM Sys.columns 
            WHERE Object_ID = Object_ID(N'Program') AND Name = 'Max_unit'
            )
            BEGIN 
                ALTER TABLE Program ADD max_unit INT NOT NULL DEFAULT -1
            END";

            SqlCommand checkMaxUnitCommand = new SqlCommand(checkMaxUnitQuery, Con);
            checkMaxUnitCommand.ExecuteNonQuery();

            // Check if the table "compulsory" exists, and create it if it doesn't
            string checkTableQuery = @"
                IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'compulsory')
                BEGIN
                    CREATE TABLE compulsory (
                        id INT PRIMARY KEY IDENTITY(1,1),
                        course_id INT NOT NULL,
                        session_id INT NOT NULL,
                        lecturer_id INT NULL
                    )
                END";

            SqlCommand checkTableCommand = new SqlCommand(checkTableQuery, Con);
            checkTableCommand.ExecuteNonQuery();

            // Check table Session, colomn semester. any where semester is "Hammattan" change it to "Harmattan"

            string updateSemesterQuery = @"IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Session' AND COLUMN_NAME = 'semester')
                BEGIN
                    UPDATE Session
                        SET semester = 'Harmattan'
                        WHERE semester = 'Hammattan';
                END";

            SqlCommand updateSemesterCommand = new SqlCommand(updateSemesterQuery, Con);
            updateSemesterCommand.ExecuteNonQuery();


            // SQL query to update session in Result table
            string updateSessionQuery = @"
                UPDATE Result
                    SET Result.session = Result.session + 1
                    FROM Result
                    INNER JOIN Course ON Result.course = Course.Id
                    INNER JOIN Session ON Result.session = Session.Id
                WHERE Course.semester = 'R'
                AND Session.semester = 'Harmattan';
            ";

            SqlCommand updateSessionCommand = new SqlCommand(updateSessionQuery, Con);

            try
            {
                int rowsAffected = updateSessionCommand.ExecuteNonQuery();
                // MessageBox.Show($"{rowsAffected} rows updated successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating session values: {ex.Message}");
            }


            // Step 1: Update Start and EndSession columns if Start is NULL and name is not NULL
            string updateSessionStartQuery = @"
                UPDATE Session
                    SET Start = LEFT(name, 4),
                    EndSession = RIGHT(name, 4)
                WHERE Start IS NULL 
                AND name IS NOT NULL;
            ";

            // Step 2: Delete rows where name is NULL
            string deleteSessionQuery = @"
                DELETE FROM Session
                WHERE name IS null;
            ";

            SqlCommand updateSessionStartCommand = new SqlCommand(updateSessionStartQuery, Con);
            SqlCommand deleteSessionStartCommand = new SqlCommand(deleteSessionQuery, Con);

            try
            {
                // Execute the update query
                int rowsUpdated = updateSessionStartCommand.ExecuteNonQuery();
                // MessageBox.Show($"{rowsUpdated} rows updated successfully.");

                // Execute the delete query
                int rowsDeleted = deleteSessionStartCommand.ExecuteNonQuery();
                //MessageBox.Show($"{rowsDeleted} rows deleted successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }


            try
            {


                string chech = "SELECT count(*) FROM [UserTbl]";
                SqlCommand cmd = new SqlCommand(chech, Con);
                exists = (int)cmd.ExecuteScalar() > 0;



                Con.Close();
            }
            catch (Exception ex)
            {
                Con.Close();
                MessageBox.Show(ex.Message);
            }
        }

        private void frmLogin_Shown(object sender, EventArgs e)
        {
            if (exists)
            {
            }
            else
            {
                RgstFrm registerForm = new RgstFrm();
                registerForm.Show();
                this.Hide();
            }
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtPassword_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // Trigger the button click event
                button1.PerformClick();
                // Optionally, you can suppress the 'ding' sound
                e.SuppressKeyPress = true;
            }
        }

        private void pnlLoginCard_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lblPasswordIcon_Click(object sender, EventArgs e)
        {

        }
    }
}
