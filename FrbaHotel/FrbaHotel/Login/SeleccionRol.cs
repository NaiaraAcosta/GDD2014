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

namespace FrbaHotel.Login
{
    public partial class SeleccionRol : Form
    {
        string username;
        Form back;
        public SeleccionRol()
        {
            InitializeComponent();
        }

        public SeleccionRol(Form atras, string user)
        {
            InitializeComponent();
            username = user;
            back = atras;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string ConnStr = @"Data Source=localhost\SQLSERVER2008;Initial Catalog=GD2C2014;User ID=gd;Password=gd2014;Trusted_Connection=False;";

            SqlConnection conn = new SqlConnection(ConnStr);
            string sSel = string.Format("SELECT [ROL_ID], [HOTEL_ID] FROM [GD2C2014].[CONTROL_ZETA].[USR_ROL_HOTEL] where USR_USERNAME = '{0}'", username);
            SqlCommand cmd = new SqlCommand(sSel, conn);
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
            back.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            logicaModo();
            Form f = new FrbaHotel.MenuPrincipal();
            f.Show();
            this.Hide();
        }
        private void logicaModo()
        {
            label1.Text = listBox1.SelectedItems.ToString();
            Login.Class1.mode = 1;
        }
    }
}
