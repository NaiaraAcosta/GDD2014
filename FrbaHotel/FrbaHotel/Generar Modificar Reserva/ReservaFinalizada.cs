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

namespace FrbaHotel.Generar_Modificar_Reserva
{
    public partial class ReservaFinalizada : Form
    {
        Form back = null;
        Form back2 = null;
        Form back3 = null;
        Form back4 = null;
        string[] param;
        public ReservaFinalizada()
        {
            InitializeComponent();
        }
        public ReservaFinalizada(Form atras, Form atras2, Form atras3, Form atras4, string[] parametros)
        {
            InitializeComponent();
            back = atras;
            back2 = atras2;
            back3 = atras3;
            back4 = atras4;
            param = parametros;

            //List<int> cantidad = new List<int>();
            //List<int> result = new List<int>();
            //string ConnStr = @"Data Source=localhost\SQLSERVER2008;Initial Catalog=GD2C2014;User ID=gd;Password=gd2014;Trusted_Connection=False;";
            //SqlConnection con = new SqlConnection(ConnStr);
            //con.Open();
            
            //int tipo = idHab[i];
            //cantidad.Add(cantHab(tipo));
            //if (cantidad[i] != 0)
            //{
            //    SqlCommand scCommand = new SqlCommand("CONTROL_ZETA.SP_VERIFICA_DISPONIBILIDAD", con);
            //    scCommand.CommandType = CommandType.StoredProcedure;
            //    scCommand.Parameters.AddWithValue("@id_res", DBNull.Value);
            //    scCommand.Parameters.Add("@hotel_id", SqlDbType.Int).Value = idHotel[comboBox3.SelectedIndex];
            //    scCommand.Parameters.Add("@fe_desde", SqlDbType.Date).Value = dateTimePicker1.Value;
            //    scCommand.Parameters.Add("@fe_hasta ", SqlDbType.Date).Value = dateTimePicker2.Value;
            //    scCommand.Parameters.Add("@cant_hab", SqlDbType.TinyInt).Value = cantidad[i];
            //    scCommand.Parameters.Add("@fe_sist", SqlDbType.Date).Value = new DateTime(2012, 01, 01);
            //    scCommand.Parameters.Add("@id_tipo_hab", SqlDbType.SmallInt).Value = tipo;
            //    scCommand.Parameters.Add("@res", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
            //    try
            //    {
            //        if (scCommand.Connection.State == ConnectionState.Closed)
            //        {
            //            scCommand.Connection.Open();
            //        }
            //        scCommand.ExecuteNonQuery();
            //        result.Add(int.Parse(scCommand.Parameters["@res"].Value.ToString()));
            //    }
            //    catch (Exception)
            //    {
            //    }
            //}
            
                
            //con.Close();
        }

        private void Form4_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            back.Close();
            back2.Close();
            back3.Close();
            back4.Show();
        }
    }
}
