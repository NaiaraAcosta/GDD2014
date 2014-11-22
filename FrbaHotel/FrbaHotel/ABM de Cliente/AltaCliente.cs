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
    public partial class AltaCliente : Form
    {
        Form back = null;
        Form back2 = null;
        Form back3 = null;
        string[] param = null;
        int modo = 0;
        string IDClie;
        List<int> tipoDoc = new List<int>();
        bool inconsistente = false;
        public AltaCliente(Form atras,string nombre, string apellido, string tipo, string numero, string mail, string telefono, string calle,
            string localidad, string pais, string nacionalidad, string nacimiento)
        {
            InitializeComponent();
            back = atras;
            textBox1.Text = nombre;
            textBox2.Text = apellido;
            //textBox3.Text = tipo;
            textBox4.Text = numero;
            textBox5.Text = mail;
            textBox6.Text = telefono;
            textBox7.Text = calle;
            //textBox8.Text = localidad;
            //textBox9.Text = pais;
            //textBox10.Text = nacionalidad;
            string[] stringSeparators = new string[] { "/" };
            string[] result = nacimiento.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
            result[2] = result[2].Substring(0, 4);
            dateTimePicker1.Value = new DateTime(int.Parse(result[2]), int.Parse(result[1]), int.Parse(result[0]));
            
        }
        public AltaCliente(Form atras, string[] param, int mod, bool inconsis)
        {
            InitializeComponent();
            cargarCombos();
            back = atras;
            modo = mod;
            inconsistente = inconsis;
            IDClie = param[0];
            textBox1.Text = param[1];
            textBox2.Text = param[2];
            comboBox1.SelectedIndex = int.Parse(param[3]) - 1;
            textBox4.Text = param[4];
            textBox5.Text = param[5];
            textBox6.Text = param[6];
            textBox7.Text = param[9];
            textBox11.Text = param[10];
            textBox3.Text = param[12];
            textBox8.Text = param[11];
            int salida;
            bool valido;
            valido = int.TryParse(param[7], out salida);
            if (valido)
            {
                comboBox2.SelectedIndex = salida - 1;
            }
            valido = int.TryParse(param[8], out salida);
            if (valido)
            {
                comboBox3.SelectedIndex = salida - 1;
            }
            valido = int.TryParse(param[13], out salida);
            if (valido)
            {
                comboBox4.SelectedIndex = salida - 1;
            }
            string[] stringSeparators = new string[] { "/" };
            string[] result = param[15].Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
            result[2] = result[2].Substring(0, 4);
            dateTimePicker1.Value = new DateTime(int.Parse(result[2]), int.Parse(result[1]), int.Parse(result[0]));
            if (param[14] == "H")
            {
                checkBox1.Checked = true;
            }
            else
            {
                checkBox1.Checked = false;
            }
        }

        public AltaCliente()
        {
            InitializeComponent();
        }

        public AltaCliente(Form atras, int mod)
        {
            InitializeComponent();
            cargarCombos();
            back = atras;
            modo = mod;
        }

        public AltaCliente(Form atras, Form atras2, Form atras3, string[] parametros)
        {
            InitializeComponent();
            cargarCombos();
            back = atras;
            back2 = atras2;
            back3 = atras3;
            param = parametros;
        }

        private void cargarCombos()
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

            conn = new SqlConnection(ConnStr);
            cmd = new SqlCommand("SELECT * FROM [GD2C2014].[CONTROL_ZETA].[LOCALIDAD]", conn);
            conn.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                comboBox2.Items.Add(reader["LOC_DETALLE"].ToString());
            }
            reader.Close();
            conn.Close();

            conn = new SqlConnection(ConnStr);
            cmd = new SqlCommand("SELECT * FROM [GD2C2014].[CONTROL_ZETA].[PAIS]", conn);
            conn.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                comboBox3.Items.Add(reader["PAIS_DETALLE"].ToString());
            }
            reader.Close();
            conn.Close();

            conn = new SqlConnection(ConnStr);
            cmd = new SqlCommand("SELECT * FROM [GD2C2014].[CONTROL_ZETA].[NACIONALIDAD]", conn);
            conn.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                comboBox4.Items.Add(reader["NAC_DETALLE"].ToString());
            }
            reader.Close();
            conn.Close(); 
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            /*if (back != null)
            {
                back.Show();
            }
            else
            {
                Form f = new ABM_de_Cliente.Form2(this);
                f.Show();
            }*/
            this.Close(); ;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //validarDatos();
            if (param != null)
            {
                param[4] = ""; //MOCK es el id de usuario!!!!!

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
                scCommand.Parameters.Add("@NOMBRE", SqlDbType.VarChar, 50).Value = textBox1.Text;
                scCommand.Parameters.Add("@APELLIDO", SqlDbType.VarChar, 50).Value = textBox2.Text;
                scCommand.Parameters.Add("@TIPO_IDENT", SqlDbType.TinyInt).Value = buscarTipoIdent();
                scCommand.Parameters.Add("@NRO_IDENT", SqlDbType.VarChar, 15).Value = textBox4.Text;
                scCommand.Parameters.Add("@EMAIL", SqlDbType.VarChar, 50).Value = textBox5.Text;
                scCommand.Parameters.Add("@TEL", SqlDbType.VarChar, 10).Value = textBox6.Text;
                scCommand.Parameters.Add("@NOMBRE_LOC", SqlDbType.VarChar, 50).Value = comboBox2.Text;
                scCommand.Parameters.Add("@NOMBRE_PAIS", SqlDbType.VarChar, 50).Value = comboBox3.Text;
                scCommand.Parameters.Add("@DOM_CALLE", SqlDbType.VarChar, 50).Value = textBox7.Text;
                if (textBox11.Text != "")
                {
                    scCommand.Parameters.Add("@DOM_NRO", SqlDbType.Int).Value = int.Parse(textBox11.Text);
                }
                else
                {
                    scCommand.Parameters.AddWithValue("@DOM_NRO", DBNull.Value);
                }
                scCommand.Parameters.Add("@DEPTO", SqlDbType.VarChar, 2).Value = textBox8.Text;
                scCommand.Parameters.Add("@DOM_PISO", SqlDbType.VarChar, 10).Value = textBox3.Text;
                scCommand.Parameters.Add("@NACIONALIDAD_NOMBRE", SqlDbType.VarChar, 50).Value = comboBox4.Text;
                scCommand.Parameters.Add("@FECHA_NAC", SqlDbType.Date).Value = dateTimePicker1.Value;
                scCommand.Parameters.Add("@CODIGO_ENTRADA", SqlDbType.TinyInt).Value = 1;
                scCommand.Parameters.AddWithValue("@CLIENTE_ID", DBNull.Value);

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
                            param[4] = scCommand.Parameters["@CLIENTE_ID_NEW"].Value.ToString();
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
                    //case 4:
                    //    {
                    //        MessageBox.Show("Se esta modificando un cliente con inconsistencias, debe solucionarse", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //        SqlDataAdapter sda = new SqlDataAdapter(scCommand);
                    //        sda.Fill(ds);
                    //        new InconsistenciasCliente(this, ds, 2).Show();
                    //        this.Hide();
                    //        break;
                    //    }
                    default:
                        {
                            string mensaje = string.Format("Error en la operacion, COD: {0}", result);
                            MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                        }
                }
                con.Close();
                
                Form f = new Generar_Modificar_Reserva.ReservaFinalizada(this, back, back2, back3, param);
                f.Show();
                this.Hide();
            }
            else
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
                scCommand.Parameters.Add("@NOMBRE", SqlDbType.VarChar, 50).Value = textBox1.Text;
                scCommand.Parameters.Add("@APELLIDO", SqlDbType.VarChar, 50).Value = textBox2.Text;
                scCommand.Parameters.Add("@TIPO_IDENT", SqlDbType.TinyInt).Value = buscarTipoIdent();
                scCommand.Parameters.Add("@NRO_IDENT", SqlDbType.VarChar , 15).Value = textBox4.Text;
                scCommand.Parameters.Add("@EMAIL", SqlDbType.VarChar , 50).Value = textBox5.Text;
                scCommand.Parameters.Add("@TEL", SqlDbType.VarChar , 10).Value = textBox6.Text;
                scCommand.Parameters.Add("@NOMBRE_LOC", SqlDbType.VarChar , 50).Value = comboBox2.Text;
                scCommand.Parameters.Add("@NOMBRE_PAIS", SqlDbType.VarChar , 50).Value = comboBox3.Text;
                scCommand.Parameters.Add("@DOM_CALLE", SqlDbType.VarChar , 50).Value = textBox7.Text;
                if (textBox11.Text != "")
                {
                    scCommand.Parameters.Add("@DOM_NRO", SqlDbType.Int).Value = int.Parse(textBox11.Text);
                }
                else
                {
                    scCommand.Parameters.AddWithValue("@DOM_NRO", DBNull.Value);
                }
                scCommand.Parameters.Add("@DEPTO", SqlDbType.VarChar , 2).Value = textBox8.Text;
                scCommand.Parameters.Add("@DOM_PISO", SqlDbType.VarChar , 10).Value = textBox3.Text;
                scCommand.Parameters.Add("@NACIONALIDAD_NOMBRE", SqlDbType.VarChar , 50).Value = comboBox4.Text;
                scCommand.Parameters.Add("@FECHA_NAC", SqlDbType.Date).Value = dateTimePicker1.Value;
                scCommand.Parameters.Add("@CODIGO_ENTRADA", SqlDbType.TinyInt).Value = modo;
                if (modo == 2)
                {
                    scCommand.Parameters.Add("@CLIENTE_ID", SqlDbType.Int).Value = int.Parse(IDClie);
                }
                else
                {
                    scCommand.Parameters.AddWithValue("@CLIENTE_ID", DBNull.Value);
                }
                
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
                            new InconsistenciasCliente(this, ds, 2).Show();
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

        private int buscarTipoIdent()
        {
            return tipoDoc[comboBox1.SelectedIndex];//MOCK!
        }
        private bool validarDatos()
        {
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox5.Text == "")
            {
                MessageBox.Show("Existen datos cuyos valores no pueden dejarse vacios","Error de ingreso de datos",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            return true;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.TextLength > 30)
            {
                textBox1.Text = textBox1.Text.Substring(0, 30);
                textBox1.SelectionStart = 30;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.TextLength > 30)
            {
                textBox2.Text = textBox2.Text.Substring(0, 30);
                textBox2.SelectionStart = 30;
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            //int salida;
            //bool valido = int.TryParse(textBox3.Text, out salida);
            //if (valido && salida > 255)
            //{
            //    textBox3.Text = "255";
            //    textBox3.SelectionStart = 3;
            //}
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            AllowNumber(e);
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (textBox4.TextLength > 15)
            {
                textBox4.Text = textBox4.Text.Substring(0, 15);
                textBox4.SelectionStart = 15;
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            if (textBox5.TextLength > 50)
            {
                textBox5.Text = textBox5.Text.Substring(0, 50);
                textBox5.SelectionStart = 50;
            }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            if (textBox6.TextLength > 10)
            {
                textBox6.Text = textBox6.Text.Substring(0, 10);
                textBox6.SelectionStart = 10;
            }
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            if (textBox7.TextLength > 70)
            {
                textBox7.Text = textBox7.Text.Substring(0, 70);
                textBox7.SelectionStart = 70;
            }
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            //int salida;
            //bool valido = int.TryParse(textBox8.Text, out salida);
            //if (valido && salida > 255)
            //{
            //    textBox8.Text = "255";
            //    textBox8.SelectionStart = 3;
            //}
        }

        private void textBox8_KeyPress(object sender, KeyPressEventArgs e)
        {
            AllowNumber(e);
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            //int salida;
            //bool valido = int.TryParse(textBox9.Text, out salida);
            //if (valido && salida > 255)
            //{
            //    textBox9.Text = "255";
            //    textBox9.SelectionStart = 3;
            //}
        }

        private void textBox9_KeyPress(object sender, KeyPressEventArgs e)
        {
            AllowNumber(e);
        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {
            //int salida;
            //bool valido = int.TryParse(textBox10.Text, out salida);
            //if (valido && salida > 255)
            //{
            //    textBox10.Text = "255";
            //    textBox10.SelectionStart = 3;
            //}
        }
        private void textBox10_KeyPress(object sender, KeyPressEventArgs e)
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

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            AllowNumber(e);
        }

        

    }      
}
