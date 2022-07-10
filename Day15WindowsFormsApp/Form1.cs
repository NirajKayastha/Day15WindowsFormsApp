using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Day15WindowsFormsApp
{
    public partial class Form1 : Form
    {

        private SqlConnection conObj = null;
        private SqlCommand cmdObj = null;
        private SqlDataReader reader = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            using (conObj = new SqlConnection(ConfigurationManager.ConnectionStrings["Conn"].ConnectionString))
            {
                using (cmdObj = new SqlCommand("Select Distinct cCity from Employee", conObj))
                {

                    if (conObj.State == ConnectionState.Closed)
                    {
                        conObj.Open();

                    }
                    reader = cmdObj.ExecuteReader();
                    if (reader.HasRows)
                    {
                        comboCity.Text = "--Select--";
                        while (reader.Read())
                        {
                            comboCity.Items.Add(reader["cCity"].ToString());

                        }
                    }

                }
            }

        }

        private void comboCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (conObj = new SqlConnection(ConfigurationManager.ConnectionStrings["Conn"].ConnectionString))
            {
                using (cmdObj = new SqlCommand("usp_GetEmployeeAndDepartmentByCity", conObj))
                {
                    cmdObj.CommandType = CommandType.StoredProcedure;
                    cmdObj.Parameters.AddWithValue("@City", comboCity.SelectedItem.ToString());

                    if (conObj.State == ConnectionState.Closed)
                    {
                        conObj.Open();

                    }
                    reader = cmdObj.ExecuteReader();
                    if (reader.HasRows)
                    {

                        DataTable dt = new DataTable();
                        dt.Load(reader);
                        dataGridViewEmployee.DataSource = dt;


                    }

                }
            }
        }
    }
}
