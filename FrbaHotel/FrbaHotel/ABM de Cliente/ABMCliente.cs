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

namespace FrbaHotel.ABM_de_Cliente
{
    public partial class ABMCliente : Form
    {
        Form back = null;
        List<int> tipoDoc = new List<int>();
        bool inconsistente = false;
        bool modoEstadia = false;
        Registrar_Estadia.cargarCliente carga = null;
        public ABMCliente(Form atras)
        {
            InitializeComponent();
            back = atras;
            cargarTipoDoc();
        }

        private void cargarTipoDoc()
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
        public ABMCliente(Form atras, int tipo)
        {
            InitializeComponent();
            back = atras;
            carga = (Registrar_Estadia.cargarCliente)atras;
            cargarTipoDoc();
            if (tipo == 4)
            {
                modoEstadia = true;
                button2.Visible = false;
                button3.Visible = false;
                button5.Text = "Seleccionar";
                button4.Text = "Terminar";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sCnn = ConfigurationManager.AppSettings["stringConexion"];
            string sSel = String.Format("SELECT * FROM [GD2C2014].[CONTROL_ZETA].[CLIENTE]");
            bool first = true;

            if (textBox1.Text != "")
            {
                if (first)
                {
                    sSel = String.Format("{0} where CLIENTE_NOMBRE like '{1}%'", sSel, textBox1.Text);
                    first = false;
                }
                else
                {
                    sSel = String.Format("{0} and CLIENTE_NOMBRE like '{1}%'", sSel, textBox1.Text);
                }
            }
            
            if (textBox2.Text != "")
            {
                if (first)
                {
                    sSel = String.Format("{0} where CLIENTE_APELLIDO like '{1}%'", sSel, textBox2.Text);
                    first = false;
                }
                else
                {
                    sSel = String.Format("{0} and CLIENTE_APELLIDO like '{1}%'", sSel, textBox2.Text);
                }
            }

            if (textBox3.Text != "")
            {
                if (first)
                {
                    sSel = String.Format("{0} where CLIENTE_DOC = '{1}'", sSel, textBox3.Text);
                    first = false;
                }
                else
                {
                    sSel = String.Format("{0} and CLIENTE_DOC = '{1}'", sSel, textBox3.Text);
                }
            }

            if (textBox4.Text != "")
            {
                if (first)
                {
                    sSel = String.Format("{0} where CLIENTE_MAIL like '{1}%'", sSel, textBox4.Text);
                    first = false;
                }
                else
                {
                    sSel = String.Format("{0} and CLIENTE_MAIL like '{1}%'", sSel, textBox4.Text);
                }
            }

            if (comboBox1.Text != "")
            {
                if (first)
                {
                    sSel = String.Format("{0} where CLIENTE_ID_TIPO_DOC = {1}", sSel, tipoDoc[comboBox1.SelectedIndex]);
                    first = false;
                }
                else
                {
                    sSel = String.Format("{0} and CLIENTE_ID_TIPO_DOC = {1}", sSel, tipoDoc[comboBox1.SelectedIndex]);
                }
            }


            SqlDataAdapter da;
            DataTable dt = new DataTable();
            try
            {
                da = new SqlDataAdapter(sSel, sCnn);
                da.Fill(dt);
                this.dataGridView1.DataSource = dt;
                label6.Text = String.Format("Total datos en la tabla: {0}", dt.Rows.Count);
            }
            catch (Exception ex)
            {
                label6.Text = "Error: " + ex.Message;
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (!modoEstadia)
            {
                back.Show();
                this.Close();
            }
            else
            {
                this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            if (dataGridView1.SelectedCells.Count != 0)
            {
                //string[] result = dataGridView1.SelectedCells[1].Value.ToString().Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
                string[] param = new string[16];
                param[0] = dataGridView1.SelectedCells[0].Value.ToString(); //id
                param[1] = dataGridView1.SelectedCells[1].Value.ToString(); //nombre
                param[2] = dataGridView1.SelectedCells[2].Value.ToString(); //apellido
                param[3] = dataGridView1.SelectedCells[3].Value.ToString(); //tipo doc
                param[4] = dataGridView1.SelectedCells[4].Value.ToString(); //numero
                param[5] = dataGridView1.SelectedCells[5].Value.ToString(); //mail
                param[6] = dataGridView1.SelectedCells[6].Value.ToString(); //telefono
                param[7] = dataGridView1.SelectedCells[7].Value.ToString(); //localidad
                param[8] = dataGridView1.SelectedCells[8].Value.ToString(); //pais
                param[9] = dataGridView1.SelectedCells[9].Value.ToString(); //calle
                param[10] = dataGridView1.SelectedCells[10].Value.ToString(); //nro calle
                param[11] = dataGridView1.SelectedCells[11].Value.ToString(); //depto
                param[12] = dataGridView1.SelectedCells[12].Value.ToString(); //piso
                param[13] = dataGridView1.SelectedCells[13].Value.ToString(); //nacionalidad
                param[14] = dataGridView1.SelectedCells[14].Value.ToString(); //estado
                param[15] = dataGridView1.SelectedCells[15].Value.ToString(); //nacimiento
                Form f = new ABM_de_Cliente.AltaCliente(this, param, 2, false);
                f.Show();
            }
            else
            {
                MessageBox.Show("No hay datos que modificar", "No se puede modificar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count != 0)
            {
                System.Data.DataTable ds = new DataTable();
                string ConnStr = ConfigurationManager.AppSettings["stringConexion"];
                SqlConnection con = new SqlConnection(ConnStr);
                con.Open();
                SqlCommand scCommand;
                if (!inconsistente)
                {
                    scCommand = new SqlCommand("CONTROL_ZETA.ABM_CLIENTE", con);
                }
                else
                {
                    scCommand = new SqlCommand("CONTROL_ZETA.MB_CLIENTE", con);
                }
                scCommand.CommandType = CommandType.StoredProcedure;
                scCommand.Parameters.AddWithValue("@NOMBRE", DBNull.Value);
                scCommand.Parameters.AddWithValue("@APELLIDO", DBNull.Value);
                scCommand.Parameters.Add("@TIPO_IDENT", SqlDbType.TinyInt).Value = int.Parse(dataGridView1.SelectedCells[3].Value.ToString());
                scCommand.Parameters.Add("@NRO_IDENT", SqlDbType.VarChar, 15).Value = dataGridView1.SelectedCells[4].Value.ToString();
                scCommand.Parameters.Add("@EMAIL", SqlDbType.VarChar, 50).Value = dataGridView1.SelectedCells[5].Value.ToString();
                scCommand.Parameters.AddWithValue("@TEL", DBNull.Value);
                scCommand.Parameters.AddWithValue("@NOMBRE_LOC", DBNull.Value);
                scCommand.Parameters.AddWithValue("@NOMBRE_PAIS", DBNull.Value);
                scCommand.Parameters.AddWithValue("@DOM_CALLE", DBNull.Value);
                scCommand.Parameters.AddWithValue("@DOM_NRO", DBNull.Value);
                scCommand.Parameters.AddWithValue("@DEPTO", DBNull.Value);
                scCommand.Parameters.AddWithValue("@DOM_PISO", DBNull.Value);
                scCommand.Parameters.AddWithValue("@NACIONALIDAD_NOMBRE", DBNull.Value);
                scCommand.Parameters.AddWithValue("@FECHA_NAC", DBNull.Value);
                scCommand.Parameters.AddWithValue("@CODIGO_ENTRADA", SqlDbType.TinyInt).Value = 3;
                scCommand.Parameters.Add("@CLIENTE_ID", SqlDbType.Int).Value = int.Parse(dataGridView1.SelectedCells[0].Value.ToString());
                scCommand.Parameters.Add("@CODIGO", SqlDbType.TinyInt).Direction = ParameterDirection.Output;
                scCommand.Parameters.Add("@CLIENTE_ID_NEW", SqlDbType.Int).Direction = ParameterDirection.Output;
                if (scCommand.Connection.State == ConnectionState.Closed)
                {
                    scCommand.Connection.Open();
                }
                scCommand.ExecuteNonQuery();

                int result = int.Parse(scCommand.Parameters["@CODIGO"].Value.ToString());

                switch (result)
                {
                    case 1:
                        {
                            MessageBox.Show("Operacion realizada exitosamente", "Operacion realizada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;
                        }
                    case 2:
                        {
                            MessageBox.Show("Modificacion/Baja de usuario inexistente", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                        }
                    case 3:
                        {
                            MessageBox.Show("Alta de usuario existente", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                        }
                    case 4:
                        {
                            MessageBox.Show("Se esta modificando un cliente con inconsistencias, debe solucionarse", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            SqlDataAdapter sda = new SqlDataAdapter(scCommand);
                            sda.Fill(ds);
                            new InconsistenciasCliente(this, ds, 3).Show();
                            this.Hide();
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

        private void button5_Click(object sender, EventArgs e)
        {
            if (!modoEstadia)
            {
                new AltaCliente(this, 1).Show();
                this.Hide();
            }
            else
            {
                if (dataGridView1.SelectedCells.Count != 0)
                {
                    DialogResult result = MessageBox.Show("Esta seguro que quiere cargar a este cliente en la reserva?", "Esta seguro?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        carga.cargarClientes(dataGridView1.SelectedCells[0].Value.ToString());
                        this.Close();
                    }
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
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
    }
}
