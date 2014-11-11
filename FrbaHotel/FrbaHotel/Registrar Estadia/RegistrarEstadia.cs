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
                SqlConnection conn2 = new SqlConnection(ConnStr);
                string sSel2 = string.Format(@"SELECT * FROM [GD2C2014].[CONTROL_ZETA].[ESTADIA] est,
                        [GD2C2014].[CONTROL_ZETA].[RESERVA] res 
                        where res.RESERVA_ID = est.EST_RESERVA_ID
                        and res.RESERVA_ID = {0}", textBox1.Text);
                SqlCommand cmd2 = new SqlCommand(sSel2, conn2);
                conn2.Open();
                SqlDataReader reader2 = cmd2.ExecuteReader();
                if (reader2.HasRows)
                {
                    reader2.Read();
                    if (reader2["RES_PRECIO_TOTAL"].ToString() == "")
                    {
                        SqlConnection con2 = new SqlConnection(ConnStr);
                        con2.Open();
                        SqlCommand scCommand2 = new SqlCommand("CONTROL_ZETA.SP_ACT_PRECIO_RES", con2);
                        scCommand2.CommandType = CommandType.StoredProcedure;
                        scCommand2.Parameters.Add("@id_reserva", SqlDbType.Int).Value = int.Parse(textBox1.Text);
                        if (scCommand2.Connection.State == ConnectionState.Closed)
                        {
                            scCommand2.Connection.Open();
                        }
                        scCommand2.ExecuteNonQuery();
                        con2.Close();
                    }
                }


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
                    case 1:
                        {
                            MessageBox.Show("Operacion realizada exitosamente", "Operacion realizada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;
                        }
                    case 6:
                        {
                            MessageBox.Show("No existe al reserva indicada", "Error en la reserva", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                        }
                    case 7:
                        {
                            MessageBox.Show("Error en la actualizacion", "Error en la reserva", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                        }
                    default:
                        {
                            string mensaje = string.Format("Error en la operacion, COD: {0}", result);
                            MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                        }
                }
                con.Close();
            }
        }
    }
}
