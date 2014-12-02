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

            string idReserva = "";
            int error = 10;

            List<int> cantidad = new List<int>();
            List<int> result = new List<int>();
            string ConnStr = ConfigurationManager.AppSettings["stringConexion"];
            SqlConnection con = new SqlConnection(ConnStr);
            con.Open();
            {
                SqlCommand scCommand = new SqlCommand("CONTROL_ZETA.SP_ALTA_RESERVA", con);
                scCommand.CommandType = CommandType.StoredProcedure;
                scCommand.Parameters.Add("@hotel_id", SqlDbType.Int).Value = int.Parse(param[0]);
                scCommand.Parameters.Add("@fe_desde", SqlDbType.Date).Value = DateTime.Parse(param[1]);
                scCommand.Parameters.Add("@fe_hasta ", SqlDbType.Date).Value = DateTime.Parse(param[2]);
                scCommand.Parameters.Add("@tipo_reg_id", SqlDbType.TinyInt).Value = int.Parse(param[3]);
                scCommand.Parameters.Add("@cliente_id", SqlDbType.Int).Value = int.Parse(param[4]);
                scCommand.Parameters.Add("@id_usr", SqlDbType.VarChar, 50).Value = param[5];
                scCommand.Parameters.Add("@fe_sist", SqlDbType.Date).Value = DateTime.Parse(param[6]);
                scCommand.Parameters.Add("@id_reserva", SqlDbType.Int).Direction = ParameterDirection.Output;
                scCommand.Parameters.Add("@error", SqlDbType.TinyInt).Direction = ParameterDirection.Output;
                //try
                //{
                    if (scCommand.Connection.State == ConnectionState.Closed)
                    {
                        scCommand.Connection.Open();
                    }
                    scCommand.ExecuteNonQuery();
                    idReserva = scCommand.Parameters["@id_reserva"].Value.ToString();
                    error = int.Parse(scCommand.Parameters["@error"].Value.ToString());
                //}
                //catch (Exception)
                //{
                //}
            }
            con.Close();

            if (error == 1)
            {
                label2.Text = "Su codigo de reserva es: " + idReserva;
            }
            else
            {
                label2.Text = string.Format("ERROR INESPERADO AL AGREGAR, COD: {0}", error);
            }
        }

        public ReservaFinalizada(Form atras, Form atras2, string[] parametros)
        {
            InitializeComponent();
            back = atras;
            back2 = atras2;
            param = parametros;

            int error = 10;

            List<int> cantidad = new List<int>();
            List<int> result = new List<int>();
            string ConnStr = ConfigurationManager.AppSettings["stringConexion"];
            SqlConnection con = new SqlConnection(ConnStr);
            con.Open();
            {
                SqlCommand scCommand = new SqlCommand("CONTROL_ZETA.SP_MODIF_RESERVA", con);
                scCommand.CommandType = CommandType.StoredProcedure;
                scCommand.Parameters.Add("@hotel_id", SqlDbType.Int).Value = int.Parse(param[0]);
                scCommand.Parameters.Add("@fe_desde", SqlDbType.Date).Value = DateTime.Parse(param[1]);
                scCommand.Parameters.Add("@fe_hasta ", SqlDbType.Date).Value = DateTime.Parse(param[2]);
                scCommand.Parameters.Add("@tipo_reg_id", SqlDbType.TinyInt).Value = int.Parse(param[3]);
                scCommand.Parameters.Add("@cliente_id", SqlDbType.Int).Value = int.Parse(param[4]);
                scCommand.Parameters.Add("@id_usr", SqlDbType.VarChar, 50).Value = param[5];
                scCommand.Parameters.Add("@id_reserva", SqlDbType.Int).Value = int.Parse(param[6]);
                scCommand.Parameters.Add("@fe_sist", SqlDbType.Date).Value = DateTime.Parse(param[7]);
                scCommand.Parameters.Add("@error", SqlDbType.TinyInt).Direction = ParameterDirection.Output;
                //try
                //{
                if (scCommand.Connection.State == ConnectionState.Closed)
                {
                    scCommand.Connection.Open();
                }
                scCommand.ExecuteNonQuery();
                error = int.Parse(scCommand.Parameters["@error"].Value.ToString());
                //}
                //catch (Exception)
                //{
                //}
            }
            con.Close();

            if (error == 1)
            {
                label1.Text = "Reserva modificada satifactoriamente";
                label2.Text = "Su codigo de reserva es: " + param[6];
            }
            else if (error == 5)
            {
                label1.Text = "Reserva no modificada";
                label2.Text = "Su codigo de reserva es: " + param[6];
                label3.Text = "La reserva no se puede modificar con solo un dia de antelacion";
            }
            else
            {
                label2.Text = string.Format("ERROR INESPERADO AL MODIFICAR, COD: {0}", error);
            }
        }

        private void Form4_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (back3 != null && back4 != null)
            {
                back4.Show();
                back3.Close();
                back2.Close();
            }
            else
            {
                back2.Show();
            }
            back.Close();
            this.Close();
        }
    }
}
