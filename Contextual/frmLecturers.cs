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
using GlobalVariable;

namespace Contextual
{
    public partial class frmLecturers : Form
    {
        public frmLecturers()
        {
            InitializeComponent();
        }

        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + GlobalVariable.Globals.databasePath + ";Integrated Security=True;Connect Timeout=30");
        
        void Load_Lecturer_Table()
        {
            dataLect.Rows.Clear();
            //load table data
            Con.Open();
            string loadLect = "SELECT * FROM [Lecturer]";
            SqlCommand cmdLect = new SqlCommand(loadLect, Con);
            SqlDataReader reader = cmdLect.ExecuteReader();

            int i = 1;

            while (reader.Read())
            {
                string programName = reader.GetString("Name");
                string programStaff_id = reader.GetString("Staff_id");
                string programGender = reader.GetString("Gender");
                string programEmail = reader.GetString("Email");
                dataLect.Rows.Add(i, programName, programStaff_id, programGender, programEmail);

                i++;
            }

            //dt.Load(reader);

            //dataPrg.DataSource= dt;

            Con.Close();
            label18.Hide();
        }
        private void label10_Click(object sender, EventArgs e)
        {
            pnlRgLct.Hide();

            label3.Text = "Register Lecturer";
            txtStaffId.Text = "";
            txtFullname.Text = "";
            comboGender.SelectedItem = null;
            textEmail.Text = "";

            button1.Show();
            updateCourse.Hide();
            deleteCourse.Hide();
        }

        private void frmLecturers_Load(object sender, EventArgs e)
        {
            pnlRgLct.Hide();

            Load_Lecturer_Table();
        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            label3.Text = "Register Lecturer";
            txtStaffId.Text = "";
            txtFullname.Text = "";
            comboGender.SelectedItem = null;
            textEmail.Text = "";

            button1.Show();
            updateCourse.Hide();
            deleteCourse.Hide();

            pnlRgLct.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if ((txtStaffId.Text == "") || (txtFullname.Text == "") || (comboGender.SelectedItem == null))
                {
                    MessageBox.Show("All required fields cannot be empty");
                }
                else
                {
                    Con.Open();

                    bool exists = false;
                    string chechuser = "SELECT count(*) FROM [Lecturer] where Staff_id='" + txtStaffId.Text + "'";

                    SqlCommand cmdUser = new SqlCommand(chechuser, Con);
                    cmdUser.Parameters.AddWithValue("UserName", txtStaffId.Text);
                    exists = (int)cmdUser.ExecuteScalar() > 0;

                    if (exists)
                    {
                        MessageBox.Show("Lecturer's Data Already Exists in Database");
                    }
                    else
                    {
                        String queryLct = "insert into Lecturer(Staff_id,Name,Gender,Email) values('" + txtStaffId.Text + "','" + txtFullname.Text + "','" + comboGender.Text + "','" + textEmail.Text + "')";
                        SqlCommand cmdLct = new SqlCommand(queryLct, Con);
                        cmdLct.ExecuteNonQuery();
                        MessageBox.Show("Lecturer's data successfuly Added");

                        txtStaffId.Text = "";
                        txtFullname.Text = "";
                        comboGender.SelectedItem = null;
                        textEmail.Text = "";
                    }
                    Con.Close();
                }

            }
            catch (Exception ex)
            {
                Con.Close();
                MessageBox.Show(ex.Message);
            }
            Load_Lecturer_Table();
        }

        private void btnsearch_Click(object sender, EventArgs e)
        {
            if(txtSearch.Text != "")
            {
                label18.Show();
                dataLect.Rows.Clear();
                //load table data
                Con.Open();
                string loadLect = "SELECT * FROM [Lecturer] where Staff_id like '%" + txtSearch.Text + "%' or Name like '%" + txtSearch.Text + "%' or Gender like '%" + txtSearch.Text + "%' or Phone like '%" + txtSearch.Text + "%' or Email like '%" + txtSearch.Text + "%'";

                //DataTable dt = new DataTable();

                SqlCommand cmdLect = new SqlCommand(loadLect, Con);

                //cmdProg.CommandType = CommandType.StoredProcedure;
                SqlDataReader reader = cmdLect.ExecuteReader();

                int i = 1;

                while (reader.Read())
                {
                    string programName = reader.GetString("Name");
                    string programStaff_id = reader.GetString("Staff_id");
                    string programGender = reader.GetString("Gender");
                    string programEmail = reader.GetString("Email");
                    dataLect.Rows.Add(i, programName, programStaff_id, programGender, programEmail);

                    i++;
                }

                //dt.Load(reader);

                //dataPrg.DataSource= dt;

                Con.Close();
            }
        }

        private void dataLect_DoubleClick(object sender, EventArgs e)
        {
            label3.Text = "Edit Staff Details";
            txtStaffId.Text = this.dataLect.CurrentRow.Cells[2].Value.ToString();
            txtFullname.Text = this.dataLect.CurrentRow.Cells[1].Value.ToString();
            comboGender.SelectedItem = dataLect.CurrentRow.Cells[3].Value.ToString(); ;
            textEmail.Text = this.dataLect.CurrentRow.Cells[4].Value.ToString();

            selectedLecturerId.Text = this.dataLect.CurrentRow.Cells[2].Value.ToString();

            button1.Hide();
            updateCourse.Show();
            deleteCourse.Show();

            pnlRgLct.Show();
        }

        private void updateCourse_Click(object sender, EventArgs e)
        {
            try
            {
                if ((txtStaffId.Text == "") || (txtFullname.Text == "") || (comboGender.SelectedItem == null))
                {
                    MessageBox.Show("You must fill all required field");
                }
                else
                {
                        Con.Open();

                        if (txtStaffId.Text == selectedLecturerId.Text)
                        {
                            string query = "update Lecturer set Staff_id = '" + txtStaffId.Text + "', Name = '" + txtFullname.Text + "',Gender = '" + comboGender.Text + "',Phone = '',Email = '" + textEmail.Text + "' where Staff_id = '" + txtStaffId.Text + "'";
                            SqlCommand cmd = new SqlCommand(query, Con);
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Staff Details Updated Successfully");
                        }
                        else
                        {
                            bool exists = false;
                            string chechuser = "SELECT count(*) FROM [Lecturer] where Staff_id = '" + txtStaffId.Text + "'";

                            SqlCommand cmdUser = new SqlCommand(chechuser, Con);
                            cmdUser.Parameters.AddWithValue("Staff_id", txtStaffId.Text);
                            exists = (int)cmdUser.ExecuteScalar() > 0;

                            if (exists)
                            {
                                MessageBox.Show("Staff with ID:"+ txtStaffId.Text + " already exist in Database");
                            }
                            else
                            {
                            string query = "update Lecturer set Staff_id = '" + txtStaffId.Text + "', Name = '" + txtFullname.Text + "',Gender = '" + comboGender.Text + "',Phone = '',Email = '" + textEmail.Text + "' where Staff_id = '" + selectedLecturerId.Text + "'";
                            SqlCommand cmd = new SqlCommand(query, Con);
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Staff Details Updated Successfully");

                            }
                        }


                        Con.Close();
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            Load_Lecturer_Table();
        }

        private void deleteCourse_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to delete the lecturer details", "Delete Lecturer", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                Con.Open();
                string queryDel = "DELETE FROM Lecturer WHERE Staff_id = '" + selectedLecturerId.Text + "'";
                SqlCommand cmdDel = new SqlCommand(queryDel, Con);
                cmdDel.ExecuteNonQuery();
                Con.Close();

                MessageBox.Show("Lecturer details deleted successfuly");

                Load_Lecturer_Table();
            }
            else if (result == DialogResult.No)
            {
                result = DialogResult.Cancel;
            }
            
        }

        private void label18_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            Load_Lecturer_Table();
        }

        private void dataLect_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataLect.Columns[e.ColumnIndex].Name == "Column6")
            {
                label3.Text = "Edit Staff Details";
                txtStaffId.Text = this.dataLect.CurrentRow.Cells[2].Value.ToString();
                txtFullname.Text = this.dataLect.CurrentRow.Cells[1].Value.ToString();
                comboGender.SelectedItem = dataLect.CurrentRow.Cells[3].Value.ToString(); ;
                textEmail.Text = this.dataLect.CurrentRow.Cells[4].Value.ToString();

                selectedLecturerId.Text = this.dataLect.CurrentRow.Cells[2].Value.ToString();

                button1.Hide();
                updateCourse.Show();
                deleteCourse.Show();

                pnlRgLct.Show();
            }
        }
    }
    
}
