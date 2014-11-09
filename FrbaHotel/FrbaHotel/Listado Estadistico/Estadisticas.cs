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
            if (radioButton1.Checked)
            {
                string ConnStr = ConfigurationManager.AppSettings["stringConexion"];
                SqlConnection conn = new SqlConnection(ConnStr);
                conn.Open();
                SqlCommand scCommand = new SqlCommand("CONTROL_ZETA.SP_ESTADISTICAS", conn);
                scCommand.CommandType = CommandType.StoredProcedure;
                scCommand.Parameters.Add("@CODIGO_LISTADO", SqlDbType.TinyInt).Value = 1;
                scCommand.Parameters.Add("@FECHA", SqlDbType.Date).Value = dateTimePicker1.Value;
                scCommand.Parameters.Add("@P_CURSOR", SqlDbType.Int).Direction = ParameterDirection.Output;
                if (scCommand.Connection.State == ConnectionState.Closed)
                {
                    scCommand.Connection.Open();
                }
                scCommand.ExecuteNonQuery();
                int error = int.Parse(scCommand.Parameters["@error"].Value.ToString());
            }
            if (radioButton2.Checked)
            {
                string ConnStr = ConfigurationManager.AppSettings["stringConexion"];
                SqlConnection conn = new SqlConnection(ConnStr);
                conn.Open();
                SqlCommand scCommand = new SqlCommand("CONTROL_ZETA.SP_ESTADISTICAS", conn);
                scCommand.CommandType = CommandType.StoredProcedure;
                scCommand.Parameters.Add("@CODIGO_LISTADO", SqlDbType.TinyInt).Value = 2;
                scCommand.Parameters.Add("@FECHA", SqlDbType.Date).Value = dateTimePicker1.Value;
                scCommand.Parameters.Add("@P_CURSOR", SqlDbType.Int).Direction = ParameterDirection.Output;
                if (scCommand.Connection.State == ConnectionState.Closed)
                {
                    scCommand.Connection.Open();
                }
                scCommand.ExecuteNonQuery();
                int error = int.Parse(scCommand.Parameters["@error"].Value.ToString());
            }
            if (radioButton3.Checked)
            {
                string ConnStr = ConfigurationManager.AppSettings["stringConexion"];
                SqlConnection conn = new SqlConnection(ConnStr);
                conn.Open();
                SqlCommand scCommand = new SqlCommand("CONTROL_ZETA.SP_ESTADISTICAS", conn);
                scCommand.CommandType = CommandType.StoredProcedure;
                scCommand.Parameters.Add("@CODIGO_LISTADO", SqlDbType.TinyInt).Value = 3;
                scCommand.Parameters.Add("@FECHA", SqlDbType.Date).Value = dateTimePicker1.Value;
                scCommand.Parameters.Add("@P_CURSOR", SqlDbType.Int).Direction = ParameterDirection.Output;
                if (scCommand.Connection.State == ConnectionState.Closed)
                {
                    scCommand.Connection.Open();
                }
                scCommand.ExecuteNonQuery();
                int error = int.Parse(scCommand.Parameters["@error"].Value.ToString());
            }
            if (radioButton4.Checked)
            {
                string ConnStr = ConfigurationManager.AppSettings["stringConexion"];
                SqlConnection conn = new SqlConnection(ConnStr);
                conn.Open();
                SqlCommand scCommand = new SqlCommand("CONTROL_ZETA.SP_ESTADISTICAS", conn);
                scCommand.CommandType = CommandType.StoredProcedure;
                scCommand.Parameters.Add("@CODIGO_LISTADO", SqlDbType.TinyInt).Value = 4;
                scCommand.Parameters.Add("@FECHA", SqlDbType.Date).Value = dateTimePicker1.Value;
                scCommand.Parameters.Add("@P_CURSOR", SqlDbType.Int).Direction = ParameterDirection.Output;
                if (scCommand.Connection.State == ConnectionState.Closed)
                {
                    scCommand.Connection.Open();
                }
                scCommand.ExecuteNonQuery();
                int error = int.Parse(scCommand.Parameters["@error"].Value.ToString());
            }
            if (radioButton5.Checked)
            {
                string ConnStr = ConfigurationManager.AppSettings["stringConexion"];
                SqlConnection conn = new SqlConnection(ConnStr);
                conn.Open();
                SqlCommand scCommand = new SqlCommand("CONTROL_ZETA.SP_ESTADISTICAS", conn);
                scCommand.CommandType = CommandType.StoredProcedure;
                scCommand.Parameters.Add("@CODIGO_LISTADO", SqlDbType.TinyInt).Value = 5;
                scCommand.Parameters.Add("@FECHA", SqlDbType.Date).Value = dateTimePicker1.Value;
                scCommand.Parameters.Add("@P_CURSOR", SqlDbType.Int).Direction = ParameterDirection.Output;
                if (scCommand.Connection.State == ConnectionState.Closed)
                {
                    scCommand.Connection.Open();
                }
                scCommand.ExecuteNonQuery();
                int error = int.Parse(scCommand.Parameters["@error"].Value.ToString());
            }
        }
    }
}
