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

namespace FrbaHotel.ABM_de_Rol
{
    public partial class ABMRol : Form
    {
        Form back = null;
        public ABMRol()
        {
            InitializeComponent();
        }

        public ABMRol(Form atras)
        {
            InitializeComponent();
            back = atras;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void ABMRol_Load(object sender, EventArgs e)
        {
            string ConnStr = @"Data Source=localhost\SQLSERVER2008;Initial Catalog=GD2C2014;User ID=gd;Password=gd2014;Trusted_Connection=False;";

            SqlConnection conn = new SqlConnection(ConnStr);
            SqlCommand cmd = new SqlCommand("SELECT * FROM [GD2C2014].[Control_Zeta].[Rol]", conn);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            string Rol;
            while (reader.Read())
            {
                Rol = string.Format("{0} - {1}", reader["Rol_Estado"].ToString(), reader["Rol_Nombre"].ToString());
                listBox1.Items.Add(Rol);
            }
            reader.Close();
            conn.Close(); 
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (back != null)
            {
                back.Show();
            }
            else
            {
                new FrbaHotel.MenuPrincipal().Show();
            }
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                Form f = new AltaRol(this, listBox1.SelectedIndex);
                f.Text = "Modificar Rol";
                f.Show();
            }
            else
            {
                MessageBox.Show("No hay datos que modificar", "No se puede modificar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
