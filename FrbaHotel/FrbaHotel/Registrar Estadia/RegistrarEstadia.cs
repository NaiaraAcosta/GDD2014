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
        List<int> tipoDoc = new List<int>();
        public RegistrarEstadia()
        {
            InitializeComponent();
        }
        public RegistrarEstadia(Form atras)
        {
            InitializeComponent();
            back = atras;
            cargarCombo();
        }

        private void cargarCombo()
        {
            string ConnStr = ConfigurationManager.AppSettings["stringConexion"];
            SqlConnection conn = new SqlConnection(ConnStr);
            SqlCommand cmd = new SqlCommand("SELECT * FROM [GD2C2014].[CONTROL_ZETA].[TIPO_DOC]", conn);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                comboBox1.Items.Add(reader["TIPO_DOC_DETALLE"].ToString());
                tipoDoc.Add(int.Parse(reader["TIPO_DOC_ID"].ToString()));
            }
            reader.Close();
            conn.Close(); 
        }

        private void button2_Click(object sender, EventArgs e)
        {
            back.Show();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if ((textBox1.Text != "") ||  (dataGridView1.SelectedCells.Count != 0))
            {
                if (textBox1.Text != "")
                {
                    DialogResult result = MessageBox.Show("Se registrara la estadia de la reserva: " + textBox1.Text + " esta seguro?", "Esta seguro?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        registrarEstadia(textBox1.Text);
                    }
                }
                else
                {
                    DialogResult result = MessageBox.Show("Se registrara la estadia de la reserva: " + dataGridView1.SelectedCells[0].Value.ToString() + " esta seguro?", "Esta seguro?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        registrarEstadia(dataGridView1.SelectedCells[0].Value.ToString());
                    }
                }
            }
            if ((textBox1.Text != "") && (dataGridView1.SelectedCells.Count != 0))
            {
                MessageBox.Show("Debe seleccionar colocar, o seleccionar una estadia", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void registrarEstadia(string codReserva)
        {
            string ConnStr = ConfigurationManager.AppSettings["stringConexion"];
            SqlConnection conn2 = new SqlConnection(ConnStr);
            string sSel2 = string.Format(@"SELECT * FROM [GD2C2014].[CONTROL_ZETA].[ESTADIA] est,
                        [GD2C2014].[CONTROL_ZETA].[RESERVA] res 
                        where res.RESERVA_ID = est.EST_RESERVA_ID
                        and res.RESERVA_ID = {0}", codReserva);
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
                    scCommand2.Parameters.Add("@id_reserva", SqlDbType.Int).Value = int.Parse(codReserva);
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
            scCommand.Parameters.Add("@RESERVA_ID", SqlDbType.Int).Value = int.Parse(codReserva);
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
            scCommand.Parameters.Add("@FECHA", SqlDbType.Date).Value = new DateTime(año, mes, dia);
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
                        if (radioButton1.Checked)
                        {
                            cargarCliente cargar = new cargarCliente(this, codReserva);
                            if (cargar.verificarCant(codReserva) != 1)
                            {
                                cargar.Show();
                            }
                        }
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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            dataGridView1.ClearSelection();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != -1)
            {
                if (textBox2.Text != "")
                {
                    string sCnn = ConfigurationManager.AppSettings["stringConexion"];
                    string sSel = String.Format(@"select r.RESERVA_ID, c.CLIENTE_ID,c.CLIENTE_NOMBRE,c.CLIENTE_APELLIDO, c.CLIENTE_FECHA_NAC 
                    from CONTROL_ZETA.CLIENTE c, CONTROL_ZETA.RESERVA r 
                    where c.CLIENTE_ID_TIPO_DOC={0}
                    and c.CLIENTE_DOC={1}
                    and r.CLIENTE_ID=c.CLIENTE_ID", tipoDoc[comboBox1.SelectedIndex], textBox2.Text);
                    SqlDataAdapter da;
                    DataTable dt = new DataTable();
                    try
                    {
                        da = new SqlDataAdapter(sSel, sCnn);
                        da.Fill(dt);
                        this.dataGridView1.DataSource = dt;
                        label3.Text = String.Format("Total datos en la tabla: {0}", dt.Rows.Count);
                    }
                    catch (Exception ex)
                    {
                        label3.Text = "Error: " + ex.Message;
                    }
                    textBox1.Text = "";
                }
                else
                {
                    MessageBox.Show("Para buscar, debe colocar un nro de documento", "Error en la busqueda", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Para buscar, debe colocar un tipo de documento", "Error en la busqueda", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RegistrarEstadia_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            AllowNumber(e);
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

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            AllowNumber(e);
        }
    }
}
