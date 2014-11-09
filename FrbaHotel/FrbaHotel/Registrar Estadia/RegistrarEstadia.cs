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

namespace FrbaHotel.Registrar_Estadia
{
    public partial class RegistrarEstadia : Form
    {
        Form back = null;
        public RegistrarEstadia()
        {
            InitializeComponent();
        }
        public RegistrarEstadia(Form atras)
        {
            InitializeComponent();
            back = atras;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            back.Show();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                string ConnStr = ConfigurationManager.AppSettings["stringConexion"];
                SqlConnection con = new SqlConnection(ConnStr);
                con.Open();
                SqlCommand scCommand = new SqlCommand("CONTROL_ZETA.SP_REGISTRAR_ESTADIA", con);
                scCommand.CommandType = CommandType.StoredProcedure;
                scCommand.Parameters.Add("@RESERVA_ID", SqlDbType.Int).Value = int.Parse(textBox1.Text);
                scCommand.Parameters.Add("@USUARIO", SqlDbType.VarChar, 50).Value = Login.Class1.user;
                if (radioButton1.Checked)
                {
                    scCommand.Parameters.Add("@CODIGO_IN_OUT", SqlDbType.TinyInt).Value = 1;
                }
                else
                {
                    scCommand.Parameters.Add("@CODIGO_IN_OUT", SqlDbType.TinyInt).Value = 2;
                }
                int año = int.Parse(ConfigurationManager.AppSettings["Año"]);
                int mes = int.Parse(ConfigurationManager.AppSettings["Mes"]);
                int dia = int.Parse(ConfigurationManager.AppSettings["Dia"]);
                scCommand.Parameters.Add("@FECHA", SqlDbType.Date).Value = new DateTime(año,mes,dia);
                scCommand.Parameters.Add("@CODIGO", SqlDbType.Int).Direction = ParameterDirection.Output;
                if (scCommand.Connection.State == ConnectionState.Closed)
                {
                    scCommand.Connection.Open();
                }
                scCommand.ExecuteNonQuery();
                int result = int.Parse(scCommand.Parameters["@CODIGO"].Value.ToString());

                switch (result)
                {
                    case 5:
                        {
                            MessageBox.Show("Reserva fuera de tiempo", "Error en la reserva", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                        }
                    default:
                        {
                            MessageBox.Show("Operacion realizada exitosamente", "Operacion realizada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;
                        }
                }
                con.Close();
            }
        }
    }
}
