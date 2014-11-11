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

namespace FrbaHotel.Listado_Estadistico
{
    public partial class Estadisticas : Form
    {
        Form back = null;
        public Estadisticas(Form atras)
        {
            InitializeComponent();
            back = atras;
        }

        private void Estadisticas_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Data.DataTable ds = new DataTable();
            string ConnStr = ConfigurationManager.AppSettings["stringConexion"];
            SqlConnection conn = new SqlConnection(ConnStr);
            System.Data.SqlClient.SqlCommand cmd = new SqlCommand("CONTROL_ZETA.SP_ESTADISTICAS", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            if (radioButton1.Checked)
            {
                cmd.Parameters.Add("@CODIGO_LISTADO", SqlDbType.TinyInt).Value = 1;
                cmd.Parameters.Add("@FECHA", SqlDbType.Date).Value = dateTimePicker1.Value;
                if (cmd.Connection.State == ConnectionState.Closed)
                {
                    cmd.Connection.Open();
                }
                SqlDataAdapter sda = new SqlDataAdapter(cmd);  
                sda.Fill(ds);  
                new Resultado(this, 1, ds, dateTimePicker1.Value).Show();
                
            }
            if (radioButton2.Checked)
            {
                cmd.Parameters.Add("@CODIGO_LISTADO", SqlDbType.TinyInt).Value = 2;
                cmd.Parameters.Add("@FECHA", SqlDbType.Date).Value = dateTimePicker1.Value;
                if (cmd.Connection.State == ConnectionState.Closed)
                {
                    cmd.Connection.Open();
                }
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(ds);
                new Resultado(this, 2, ds, dateTimePicker1.Value).Show();
            }
            if (radioButton3.Checked)
            {
                cmd.Parameters.Add("@CODIGO_LISTADO", SqlDbType.TinyInt).Value = 4;
                cmd.Parameters.Add("@FECHA", SqlDbType.Date).Value = dateTimePicker1.Value;
                if (cmd.Connection.State == ConnectionState.Closed)
                {
                    cmd.Connection.Open();
                }
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(ds);
                new Resultado(this, 4, ds, dateTimePicker1.Value).Show();
            }
            if (radioButton4.Checked)
            {
                cmd.Parameters.Add("@CODIGO_LISTADO", SqlDbType.TinyInt).Value = 5;
                cmd.Parameters.Add("@FECHA", SqlDbType.Date).Value = dateTimePicker1.Value;
                if (cmd.Connection.State == ConnectionState.Closed)
                {
                    cmd.Connection.Open();
                }
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(ds);
                new Resultado(this, 5, ds, dateTimePicker1.Value).Show();
            }
            if (radioButton5.Checked)
            {
                cmd.Parameters.Add("@CODIGO_LISTADO", SqlDbType.TinyInt).Value = 3;
                cmd.Parameters.Add("@FECHA", SqlDbType.Date).Value = dateTimePicker1.Value;
                if (cmd.Connection.State == ConnectionState.Closed)
                {
                    cmd.Connection.Open();
                }
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(ds);
                new Resultado(this, 3, ds, dateTimePicker1.Value).Show();
            }
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            back.Show();
            this.Close();
        }
    }
}
