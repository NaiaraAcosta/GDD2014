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

namespace FrbaHotel.Cancelar_Reserva
{
    public partial class BajaReserva : Form
    {
        Form back = null;
        public BajaReserva()
        {
            InitializeComponent();
        }

        public BajaReserva(Form atras)
        {
            InitializeComponent();
            back = atras;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            back.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                string ConnStr = ConfigurationManager.AppSettings["stringConexion"];

                SqlConnection conn = new SqlConnection(ConnStr);
                string sSel = string.Format(@"SELECT * FROM [GD2C2014].[CONTROL_ZETA].[RESERVA] res 
                    where res.RESERVA_ID = '{0}'", textBox1.Text);
                SqlCommand cmd = new SqlCommand(sSel, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    DialogResult resultado = MessageBox.Show("Esta seguro que quiere eliminar la reserva?", "Esta seguro?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (resultado == DialogResult.Yes)
                    {
                        darDeBaja();
                    }
                }
                else
                {
                    MessageBox.Show("No existe la reserva especificada", "Error de ingreso de datos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Debe especificar un numero de reserva", "Error de ingreso de datos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void darDeBaja()
        {
            string ConnStr = ConfigurationManager.AppSettings["stringConexion"];
            SqlConnection conn = new SqlConnection(ConnStr);
            conn.Open();
            SqlCommand scCommand = new SqlCommand("CONTROL_ZETA.SP_CANCELAR_RESERVA", conn);
            scCommand.CommandType = CommandType.StoredProcedure;
            scCommand.Parameters.Add("@id_reserva", SqlDbType.Int).Value = int.Parse(textBox1.Text);
            scCommand.Parameters.Add("@motivo", SqlDbType.VarChar, 150).Value = richTextBox1.Text;
            int año = int.Parse(ConfigurationManager.AppSettings["Año"]);
            int mes = int.Parse(ConfigurationManager.AppSettings["Mes"]);
            int dia = int.Parse(ConfigurationManager.AppSettings["Dia"]);
            scCommand.Parameters.Add("@fecha_canc ", SqlDbType.Date).Value = new DateTime(año,mes,dia);
            if (Login.Class1.user != null)
            {
                scCommand.Parameters.Add("@id_usr", SqlDbType.VarChar, 50).Value = Login.Class1.user;
                scCommand.Parameters.Add("@id_est", SqlDbType.VarChar, 4).Value = "RCR";
            }
            else
            {
                scCommand.Parameters.AddWithValue("@id_usr", DBNull.Value);
                scCommand.Parameters.Add("@id_est", SqlDbType.VarChar, 4).Value = "RCC";
            }
            scCommand.Parameters.Add("@error", SqlDbType.TinyInt).Direction = ParameterDirection.Output;
            if (scCommand.Connection.State == ConnectionState.Closed)
            {
                scCommand.Connection.Open();
            }
            scCommand.ExecuteNonQuery();
            int error = int.Parse(scCommand.Parameters["@error"].Value.ToString());

            if (error == 1)
            {
                MessageBox.Show("Reserva cancelada correctamente", "Reserva cancelada", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (error == 5)
            {
                MessageBox.Show("No se puede cancelar una reserva pasada", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (error == 6)
            {
                MessageBox.Show("Reserva ya cancelada", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                string errorDesconocido = string.Format("Error desconocido, codigo: {0}", error);
                MessageBox.Show(errorDesconocido, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void BajaReserva_Load(object sender, EventArgs e)
        {

        }
        public static void AllowNumber(KeyPressEventArgs e)
        {
            if (char.IsLetter(e.KeyChar) || //Letras
                char.IsSymbol(e.KeyChar) || //Símbolos
                char.IsWhiteSpace(e.KeyChar) || //Espaço
                char.IsPunctuation(e.KeyChar)) //Pontuação
                e.Handled = true; //Não permitir
            //Com o script acima é possível utilizar Números, 'Del', 'BackSpace'..

            //Abaixo só é permito de 0 a 9
            //if ((e.KeyChar < '0') || (e.KeyChar > '9')) e.Handled = true; //Allow only numbers
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            AllowNumber(e);
        }
    }
}
