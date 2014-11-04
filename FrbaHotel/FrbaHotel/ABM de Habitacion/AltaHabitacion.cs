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

namespace FrbaHotel.ABM_de_Habitacion
{
    public partial class AltaHabitacion : Form
    {
        Form back = null;
        List<string> tipoHabId = new List<string>();
        public AltaHabitacion()
        {
            InitializeComponent();
        }

        public AltaHabitacion(Form atras)
        {
            InitializeComponent();
            back = atras;
        }

        public AltaHabitacion(Form atras, string id ,string num, string piso, string ubi, string tipo, string comodidades)
        {
            InitializeComponent();
            back = atras;

            string ConnStr = @"Data Source=localhost\SQLSERVER2008;Initial Catalog=GD2C2014;User ID=gd;Password=gd2014;Trusted_Connection=False;";
            SqlConnection conn = new SqlConnection(ConnStr);
            string sSel = string.Format(@"SELECT * FROM [GD2C2014].[CONTROL_ZETA].[HABITACION] hab, 
                [GD2C2014].[CONTROL_ZETA].[TIPO_HAB] tipohab 
                where hab.HAB_ID_TIPO = tipohab.TIPO_HAB_ID");
            SqlCommand cmd = new SqlCommand(sSel, conn);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                comboBox1.Items.Add(reader["TIPO_HAB_DESCRIPCION"].ToString());
                tipoHabId.Add(reader["TIPO_HAB_ID"].ToString());
            }
            reader.Close();
            conn.Close();

            textBox1.Text = num;
            textBox2.Text = piso;
            textBox3.Text = ubi;
            comboBox1.SelectedIndex = tipoHabId.FindIndex(x => x == tipo);
            comboBox1.Enabled = false;
            richTextBox1.Text = comodidades;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (back != null)
            {
                back.Show();
            }
            else
            {
                new ABMHabitacion().Show();
            }
            this.Close();
        }
    }
}
