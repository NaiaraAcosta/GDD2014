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
    public partial class Ingresar : Form
    {
        FrbaHotel.MenuPrincipal back;
        public Ingresar(FrbaHotel.MenuPrincipal atras)
        {
            InitializeComponent();
            back = atras;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (logicaLogueo(textBox1.Text, textBox2.Text))
            {
                Form f = new Login.SeleccionRol(this, back, textBox1.Text);
                f.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Error de logueo, intente nuevamente","Error de logueo",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
        private Boolean logicaLogueo(string user, string pass)
        {
            //string ConnStr = @"Data Source=localhost\SQLSERVER2008;Initial Catalog=GD2C2014;User ID=gd;Password=gd2014;Trusted_Connection=False;";
            //SqlConnection con = new SqlConnection(ConnStr);
            //con.Open();
            //SqlCommand scCommand = new SqlCommand("CONTROL_ZETA.SP_VERIFICA_DISPONIBILIDAD", con);
            //scCommand.CommandType = CommandType.StoredProcedure;
            //scCommand.Parameters.AddWithValue("@id_res", DBNull.Value);
            ////scCommand.Parameters.Add("@hotel_id", SqlDbType.Int).Value = idHotel[comboBox3.SelectedIndex];
            ////scCommand.Parameters.Add("@fe_desde", SqlDbType.Date).Value = dateTimePicker1.Value;
            ////scCommand.Parameters.Add("@fe_hasta ", SqlDbType.Date).Value = dateTimePicker2.Value;
            ////scCommand.Parameters.Add("@cant_hab", SqlDbType.TinyInt).Value = cantidad[i];
            ////scCommand.Parameters.Add("@fe_sist", SqlDbType.Date).Value = new DateTime(2012, 01, 01);
            ////scCommand.Parameters.Add("@id_tipo_hab", SqlDbType.SmallInt).Value = tipo;
            ////scCommand.Parameters.Add("@id_regimen", SqlDbType.TinyInt).Value = idReg[comboBox2.SelectedIndex];
            ////scCommand.Parameters.Add("@res", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
            ////scCommand.Parameters.Add("@id_res_new_temp", SqlDbType.Int).Direction = ParameterDirection.Output;
            ////try
            ////{
            //if (scCommand.Connection.State == ConnectionState.Closed)
            //{
            //    scCommand.Connection.Open();
            //}
            //scCommand.ExecuteNonQuery();
            //result.Add(int.Parse(scCommand.Parameters["@res"].Value.ToString()));
            //idResTemp = scCommand.Parameters["@id_res_new_temp"].Value.ToString();
            //con.Close();
            return true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            back.Show();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
    }
}
