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

namespace FrbaHotel.Facturacion
{
    public partial class Facturar : Form
    {
        Form back = null;
        string reservaID = "";
        string estID = "";
        public Facturar()
        {
            InitializeComponent();
        }

        public Facturar(Form atras)
        {
            InitializeComponent();
            back = atras;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton4.Checked)
            {
                textBox6.Enabled = true;
            }
            else
            {
                textBox6.Enabled = false;
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked)
            {
                textBox1.Enabled = true;
            }
            else
            {
                textBox1.Enabled = false;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                textBox2.Enabled = false;
                textBox3.Enabled = false;
                dateTimePicker1.Enabled = false;
                textBox5.Enabled = false;
            }
            else
            {
                textBox2.Enabled = true;
                textBox3.Enabled = true;
                dateTimePicker1.Enabled = true;
                textBox5.Enabled = true;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                textBox2.Enabled = true;
                textBox3.Enabled = true;
                dateTimePicker1.Enabled = true;
                textBox5.Enabled = true;
            }
            else
            {
                textBox2.Enabled = false;
                textBox3.Enabled = false;
                dateTimePicker1.Enabled = false;
                textBox5.Enabled = false;
            }
        }

        private void Facturar_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (back != null)
            {
                back.Show();
            }
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string ConnStr2 = ConfigurationManager.AppSettings["stringConexion"];
            SqlConnection conn2 = new SqlConnection(ConnStr2);
            string sSel2 = string.Format(@"SELECT * FROM [GD2C2014].[CONTROL_ZETA].[ESTADIA] est,
                        [GD2C2014].[CONTROL_ZETA].[RESERVA] res 
                        where res.RESERVA_ID = est.EST_RESERVA_ID");
            if (radioButton3.Checked)
            {
                sSel2 = string.Format(" {0} and res.RESERVA_ID = {1}", sSel2, textBox1.Text);
            }
            else
            {
                sSel2 = string.Format(" {0} and est.EST_ID = {1}", sSel2, textBox6.Text);
            }
            SqlCommand cmd2 = new SqlCommand(sSel2, conn2);
            conn2.Open();
            SqlDataReader reader2 = cmd2.ExecuteReader();
            string precio = "";
            if (reader2.HasRows)
            {
                while (reader2.Read())
                {
                    precio = reader2["RES_PRECIO_TOTAL"].ToString();
                    label1.Text = "El valor a facturar es de: " + precio;
                    reservaID = reader2["RESERVA_ID"].ToString();
                    estID = reader2["EST_ID"].ToString();
                }
                if (precio == "")
                {
                    SqlConnection con = new SqlConnection(ConnStr2);
                    con.Open();
                    SqlCommand scCommand = new SqlCommand("CONTROL_ZETA.SP_ACT_PRECIO_RES", con);
                    scCommand.CommandType = CommandType.StoredProcedure;
                    scCommand.Parameters.Add("@id_reserva", SqlDbType.Int).Value = int.Parse(reservaID);
                    if (scCommand.Connection.State == ConnectionState.Closed)
                    {
                        scCommand.Connection.Open();
                    }
                    scCommand.ExecuteNonQuery();
                    con.Close();
                }
            
                reader2.Close();
                conn2.Close();
                
                
                
                SqlConnection con2 = new SqlConnection(ConnStr2);
                con2.Open();
                SqlCommand scCommand2 = new SqlCommand("CONTROL_ZETA.SP_REALIZAR_FACTURACION", con2);
                scCommand2.CommandType = CommandType.StoredProcedure;
                scCommand2.Parameters.Add("@RESERVA_ID", SqlDbType.Int).Value = int.Parse(reservaID);
                
                if (radioButton1.Checked)
                {
                    scCommand2.Parameters.Add("@FORMAPAGO", SqlDbType.VarChar, 2).Value = "E";
                    scCommand2.Parameters.AddWithValue("@NROTARJETA", DBNull.Value);
                    scCommand2.Parameters.AddWithValue("@COD_VERIF", DBNull.Value);
                    scCommand2.Parameters.AddWithValue("@FECHA_VENC", DBNull.Value);
                    scCommand2.Parameters.AddWithValue("@NRO_CUOTAS", DBNull.Value);
                }
                else
                {
                    scCommand2.Parameters.Add("@FORMAPAGO", SqlDbType.VarChar, 2).Value = "T";
                    scCommand2.Parameters.Add("@NROTARJETA", SqlDbType.Int).Value = int.Parse(textBox2.Text);
                    scCommand2.Parameters.Add("@COD_VERIF", SqlDbType.Int).Value = int.Parse(textBox3.Text);
                    scCommand2.Parameters.Add("@FECHA_VENC", SqlDbType.Date).Value = dateTimePicker1.Value;
                    scCommand2.Parameters.Add("@NRO_CUOTAS", SqlDbType.TinyInt).Value = int.Parse(textBox5.Text);
                }
                scCommand2.Parameters.Add("@CODIGO", SqlDbType.Int).Direction = ParameterDirection.Output;
                scCommand2.Parameters.Add("@FACTURA_NRO", SqlDbType.Int).Direction = ParameterDirection.Output;
                if (scCommand2.Connection.State == ConnectionState.Closed)
                {
                    scCommand2.Connection.Open();
                }
                scCommand2.ExecuteNonQuery();
                int result = int.Parse(scCommand2.Parameters["@CODIGO"].Value.ToString());
                
                //if (result != 1)
                //{
                //    string mensaje = string.Format("Error en la facturacion, COD: {0}", result);
                //    MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //}
                //else
                //{
                    int factura = int.Parse(scCommand2.Parameters["@FACTURA_NRO"].Value.ToString());
                    mostrarFactura(factura);
                //}
            }
            else
            {
                MessageBox.Show("No existe la reserva, o no se ha realizado el check-in de la misma", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void mostrarFactura(int factura)
        {
            Form f = new Facturacion.Factura(this, factura);
            f.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
            
            
            string ConnStr2 = ConfigurationManager.AppSettings["stringConexion"];
            SqlConnection conn2 = new SqlConnection(ConnStr2);
            string sSel2 = string.Format(@"SELECT * FROM [GD2C2014].[CONTROL_ZETA].[ESTADIA] est,
                        [GD2C2014].[CONTROL_ZETA].[RESERVA] res 
                        where res.RESERVA_ID = est.EST_RESERVA_ID");
            if (radioButton3.Checked)
            {
                sSel2 = string.Format(" {0} and res.RESERVA_ID = {1}", sSel2, textBox1.Text);
            }
            else
            {
                sSel2 = string.Format(" {0} and est.EST_ID = {1}", sSel2, textBox6.Text);
            }
            SqlCommand cmd2 = new SqlCommand(sSel2, conn2);
            conn2.Open();
            SqlDataReader reader2 = cmd2.ExecuteReader();
            while (reader2.Read())
            {
                string precio = reader2["RES_PRECIO_TOTAL"].ToString();
                label1.Text = "El valor a facturar es de: " + precio;
                reservaID = reader2["RESERVA_ID"].ToString();
                estID = reader2["EST_ID"].ToString();
            }
            reader2.Close();
            conn2.Close();
        }
    }
}
