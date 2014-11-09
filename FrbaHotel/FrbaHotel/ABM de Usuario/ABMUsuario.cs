using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace FrbaHotel.ABM_de_Usuario
{
    public partial class ABMUsuario : Form
    {
        public ABMUsuario()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string value1 = string.Empty;

            string ConnStr = ConfigurationManager.AppSettings["stringConexion"];

            SqlConnection conn = new SqlConnection(ConnStr);
            SqlCommand cmd = new SqlCommand("SELECT TOP 1000 [Hotel_Ciudad] FROM [GD2C2014].[gd_esquema].[Maestra]", conn);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                listBox1.Items.Add(reader["Hotel_Ciudad"].ToString());
            }
            reader.Close();
            conn.Close(); 
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void ABMUsuario_Load(object sender, EventArgs e)
        {

        }
    }
}
