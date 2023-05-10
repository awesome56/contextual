﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using GlobalVariable;
using System.IO;

namespace Contextual
{
    public partial class frmSettings : Form
    {
        public frmSettings()
        {
            InitializeComponent();
        }

        static string instituteName;
        static string facultyName;
        static string departmentName;
        static string pass;
        static string headers;

        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + GlobalVariable.Globals.databasePath + ";Integrated Security=True;MultipleActiveResultSets=True;Connect Timeout=30");

        void Load_Setthings_Details()
        {
            label5.Show();
            label7.Show();
            txtHeader.Hide();
            label8.Hide();
            label9.Hide();

            Con.Open();
            string loadProg = "SELECT * FROM [UserTbl]";
            SqlCommand cmdProg = new SqlCommand(loadProg, Con);
            SqlDataReader reader = cmdProg.ExecuteReader();

            if (reader.Read())
            {
                instituteName = reader.GetString("School");
                facultyName = reader.GetString("Faculty");
                departmentName = reader.GetString("Department");
                pass = reader.GetString("Pass");
                headers = reader.GetString("Header");

                txtInstName.Text = instituteName;
                txtFaculty.Text = facultyName;
                txtDpt.Text = departmentName;
                txtPassword.Text = pass;

                if ((headers == null) || (headers == ""))
                {
                    txtHeader.Text = instituteName + "\nFaculty of " + facultyName + "\nDepartment of " + departmentName;
                    label7.Text = instituteName + "\nFaculty of " + facultyName + "\nDepartment of " + departmentName;
                }
                else
                {
                    txtHeader.Text = headers;
                    label7.Text = headers;
                }
            }
            Con.Close();
        }
        private void frmSettings_Load(object sender, EventArgs e)
        {
            Load_Setthings_Details();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            txtHeader.Show();
            label7.Hide();
            label5.Hide();
            label8.Show();
            label9.Show();
        }

        private void label8_Click(object sender, EventArgs e)
        {
            txtHeader.Hide();
            label7.Show();
            label5.Show();
            label8.Hide();
            label9.Hide();
        }

        private void label9_Click(object sender, EventArgs e)
        {
            txtHeader.Text = instituteName + "\nFaculty of " + facultyName + "\nDepartment of " + departmentName;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                Con.Open();

                if ((txtInstName.Text == "") || (txtFaculty.Text == "") || (txtDpt.Text == "") || (txtPassword.Text == ""))
                {
                    MessageBox.Show("You must fill all required field");
                }
                else
                {
                    string queryStudent = "UPDATE UserTbl set School = '" + txtInstName.Text + "', Faculty = '" + txtFaculty.Text + "', Department = '" + txtDpt.Text + "', Pass = '" + txtPassword.Text + "',Header = '" + txtHeader.Text + "' where School = '" + instituteName + "'";
                    SqlCommand cmdStudent = new SqlCommand(queryStudent, Con);
                    cmdStudent.ExecuteNonQuery();
                    MessageBox.Show("Settings Saved");
                   
                }
                Con.Close();

                Load_Setthings_Details();
            }
            catch (Exception ex)
            {
                Con.Close();
                MessageBox.Show(ex.Message);
            }
        }

        private void checkBxShowPass_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBxShowPass.Checked)
            {
                txtPassword.PasswordChar = '\0';
            }
            else
            {
                txtPassword.PasswordChar = '•';
            }
        }
    }
}
