using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Contextual
{
    public partial class contextual : Form
    {
        public contextual()
        {
            InitializeComponent();

            // Subscribe to the FormClosing event
            //this.FormClosing += contextual_FormClosing;
        }

        public string formerPage;
        public string currentPage;

        // To be used in frmStudentData as Program name.
        public string studentDataVariable;

        private void iconButton2_Click(object sender, EventArgs e)
        {
            Home();
        }

        private void contextual_Load(object sender, EventArgs e)
        {
            if (formerPage == "" || formerPage == "frmLogin")
            {
                Home();
            }
        }

        public void Home()
        {
            frmDashboard frmDashboard_page = new frmDashboard() { Dock = DockStyle.Fill, TopLevel = false, TopMost = true, FormBorderStyle = FormBorderStyle.None };
   
      
            contextual parentForm = ApplicationInstance.ContextualForm;
            if (parentForm != null)
            {
                parentForm.panel1.Controls.Clear(); // Clear existing controls in panel1
                parentForm.panel1.Controls.Add(frmDashboard_page);
                parentForm.iconButton2.Hide();
                parentForm.iconButton1.Hide();
                frmDashboard_page.Show();
            }
        }

        // Method to refresh the current content
        public void RefreshContent()
        {
            if (panel1.Controls.Count > 0)
            {
                contextual parentForm = ApplicationInstance.ContextualForm;

                var currentControl = parentForm.panel1.Controls[0];
                parentForm.panel1.Controls.Remove(currentControl);
                parentForm.panel1.Controls.Add(currentControl);
                (currentControl as Form)?.Show();
            }
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            RefreshContent();
        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            if (currentPage == "frmProgramData")
            {
                frmDashboard frmDashboard_page = new frmDashboard() { Dock = DockStyle.Fill, TopLevel = false, TopMost = true, FormBorderStyle = FormBorderStyle.None };

                contextual parentForm = ApplicationInstance.ContextualForm;
                if (parentForm != null)
                {
                    parentForm.panel1.Controls.Clear(); // Clear existing controls in panel1
                    parentForm.panel1.Controls.Add(frmDashboard_page);
                    parentForm.iconButton2.Hide();
                    parentForm.iconButton1.Hide();
                    frmDashboard_page.Show();

                    frmDashboard_page.pnlLoader.Controls.Clear();

                    frmPrograms FrmProgram_page = new frmPrograms() { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
                    FrmProgram_page.FormBorderStyle = FormBorderStyle.None;
                    frmDashboard_page.pnlLoader.Controls.Add(FrmProgram_page);
                    FrmProgram_page.Show();
                }
            }

            if (currentPage == "frmStudentData")
            {
                if (formerPage == "frmStudent")
                {
                    frmDashboard frmDashboard_page = new frmDashboard() { Dock = DockStyle.Fill, TopLevel = false, TopMost = true, FormBorderStyle = FormBorderStyle.None };

                    contextual parentForm = ApplicationInstance.ContextualForm;
                    if (parentForm != null)
                    {
                        parentForm.panel1.Controls.Clear(); // Clear existing controls in panel1
                        parentForm.panel1.Controls.Add(frmDashboard_page);
                        parentForm.iconButton2.Hide();
                        parentForm.iconButton1.Hide();
                        frmDashboard_page.Show();

                        frmDashboard_page.pnlLoader.Controls.Clear();

                        frmStudent FrmProgram_page = new frmStudent() { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
                        FrmProgram_page.FormBorderStyle = FormBorderStyle.None;
                        frmDashboard_page.pnlLoader.Controls.Add(FrmProgram_page);
                        FrmProgram_page.Show();
                    }
                }

                if (formerPage == "frmProgramData")
                {
                    frmStudentData FrmProgram_page = new frmStudentData();
                    FrmProgram_page.backProgram(studentDataVariable);
                }
            }
        }

        private void iconButton3_Click(object sender, EventArgs e)
        {
            contextual parentForm = ApplicationInstance.ContextualForm;
            if (parentForm != null)
            {
                frmLogin login = new frmLogin();
                login.Show();

                parentForm.Hide();
            }
        }

        private void contextual_FormClosing(object sender, FormClosingEventArgs e)
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
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the process
                MessageBox.Show($"Error during form closing process: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
