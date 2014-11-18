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

namespace FrbaHotel.ABM_de_Hotel
{
    public partial class AltaHotel : Form
    {
        Form back = null;
        string idHotel = "";
        List<bool> listReg = new List<bool>();
        public AltaHotel()
        {
            InitializeComponent();
        }

        public AltaHotel(Form atras)
        {
            InitializeComponent();
            back = atras;
            cargarComboBox();
            cargarRegimenes();
        }

        public AltaHotel(Form atras,
            string idhotel,
            string nombre,
            string mail,
            string telefono,
            string calle,
            string nro,
            string cantEstrellas,
            string recargaEstrellas,
            string idCiudad,
            string idPais,
            string fechaCreacion)
        {
            InitializeComponent();
            
            back = atras;
            idHotel = idhotel;
            cargarComboBox();
            cargarRegimenes();
            textBox1.Text = nombre;
            textBox2.Text = mail;
            textBox3.Text = telefono;
            textBox4.Text = calle;
            textBox9.Text = nro;
            textBox5.Text = cantEstrellas;
            textBox6.Text = recargaEstrellas;
            int idCiudadSeleccionada;
            if (int.TryParse(idCiudad, out idCiudadSeleccionada))
            {
                comboBox1.SelectedIndex = idCiudadSeleccionada - 1;
            }
            int idPaisSeleccionada;
            if (int.TryParse(idPais, out idPaisSeleccionada))
            {
                comboBox2.SelectedIndex = idPaisSeleccionada - 1;
            }
        }

        private void cargarRegimenes()
        {
            listReg.Clear();
            checkedListBox1.Items.Clear();
            string ConnStr2 = ConfigurationManager.AppSettings["stringConexion"];
            SqlConnection conn2 = new SqlConnection(ConnStr2);
            string sSel2 = string.Format(@"SELECT * FROM [GD2C2014].[CONTROL_ZETA].[REGIMEN]");
            SqlCommand cmd2 = new SqlCommand(sSel2, conn2);
            conn2.Open();
            SqlDataReader reader2 = cmd2.ExecuteReader();
            while (reader2.Read())
            {
                checkedListBox1.Items.Add(reader2["REG_DESCRIPCION"].ToString());
                listReg.Add(false);
            }
            reader2.Close();
            conn2.Close();

            if (idHotel != "")
            {
                conn2 = new SqlConnection(ConnStr2);
                sSel2 = string.Format(@"SELECT * FROM [GD2C2014].[CONTROL_ZETA].[HOTEL_REGIMEN]
                        where HOTEL_ID = {0}", idHotel);
                cmd2 = new SqlCommand(sSel2, conn2);
                conn2.Open();
                reader2 = cmd2.ExecuteReader();
                while (reader2.Read())
                {
                    int regID = int.Parse(reader2["REG_ID"].ToString());
                    if (reader2["REG_ESTADO"].ToString() != "I")
                    {
                        listReg[regID - 1] = true;
                        checkedListBox1.SetItemChecked(regID - 1, true);
                    }
                }
                reader2.Close();
                conn2.Close();
            }
        }

        private void cargarComboBox()
        {
            string ConnStr2 = ConfigurationManager.AppSettings["stringConexion"];
            SqlConnection conn2 = new SqlConnection(ConnStr2);
            string sSel2 = string.Format(@"SELECT * FROM [GD2C2014].[CONTROL_ZETA].[LOCALIDAD]");
            SqlCommand cmd2 = new SqlCommand(sSel2, conn2);
            conn2.Open();
            SqlDataReader reader2 = cmd2.ExecuteReader();
            while (reader2.Read())
            {
                comboBox1.Items.Add(reader2["LOC_DETALLE"].ToString());
            }
            reader2.Close();
            conn2.Close();

            conn2 = new SqlConnection(ConnStr2);
            sSel2 = string.Format(@"SELECT * FROM [GD2C2014].[CONTROL_ZETA].[PAIS]");
            cmd2 = new SqlCommand(sSel2, conn2);
            conn2.Open();
            reader2 = cmd2.ExecuteReader();
            while (reader2.Read())
            {
                comboBox2.Items.Add(reader2["PAIS_DETALLE"].ToString());
            }
            reader2.Close();
            conn2.Close();
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            int salida = int.Parse(textBox6.Text);
            if (salida > 10)
            {
                textBox6.Text = "10";
                textBox6.SelectionStart = 2;
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            int salida = int.Parse(textBox5.Text);
            if (salida > 5)
            {
                textBox5.Text = "5";
                textBox5.SelectionStart = 1;
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (textBox4.TextLength > 50)
            {
                textBox4.Text = textBox4.Text.Substring(0, 50);
                textBox4.SelectionStart = 50;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.TextLength > 100)
            {
                textBox1.Text = textBox1.Text.Substring(0, 100);
                textBox1.SelectionStart = 100;
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (textBox3.TextLength > 10)
            {
                textBox3.Text = textBox3.Text.Substring(0, 10);
                textBox3.SelectionStart = 10;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.TextLength > 50)
            {
                textBox2.Text = textBox2.Text.Substring(0, 50);
                textBox2.SelectionStart = 50;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (back != null)
            {
                back.Show();
            }
            else
            {
                new FrbaHotel.MenuPrincipal().Show();
            }
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            string ConnStr = ConfigurationManager.AppSettings["stringConexion"];
            SqlConnection con = new SqlConnection(ConnStr);
            con.Open();
            SqlTransaction transaction = con.BeginTransaction();
            bool conError = false;
            SqlCommand scCommand = new SqlCommand("CONTROL_ZETA.SP_AM_HOTEL", con, transaction);
            scCommand.CommandType = CommandType.StoredProcedure;
            if (idHotel != "")
            {
                scCommand.Parameters.Add("@accion", SqlDbType.TinyInt).Value = 2;
                scCommand.Parameters.Add("@id_hotel", SqlDbType.Int).Value = int.Parse(idHotel);
            }
            else
            {
                scCommand.Parameters.Add("@accion", SqlDbType.TinyInt).Value = 1;
                scCommand.Parameters.AddWithValue("@id_hotel", DBNull.Value);
            }
            if (textBox1.Text != "")
            {
                scCommand.Parameters.Add("@nombre", SqlDbType.VarChar, 100).Value = textBox1.Text;
            }
            else
            {
                scCommand.Parameters.AddWithValue("@nombre", DBNull.Value);
            }
            if (textBox2.Text != "")
            {
            scCommand.Parameters.Add("@mail", SqlDbType.VarChar, 50).Value = textBox2.Text;
            }
            else
            {
                scCommand.Parameters.AddWithValue("@mail", DBNull.Value);
            }
            if (textBox3.Text != "")
            {
                scCommand.Parameters.Add("@tel", SqlDbType.VarChar, 10).Value = textBox3.Text;
            }
            else
            {
                scCommand.Parameters.AddWithValue("@tel", DBNull.Value);
            }

            if (textBox4.Text != "")
            {
                scCommand.Parameters.Add("@calle", SqlDbType.VarChar, 50).Value = textBox4.Text;
            }
            else
            {
                scCommand.Parameters.AddWithValue("@calle", DBNull.Value);
            }

            if (textBox9.Text != "")
            {
                scCommand.Parameters.Add("@nro_calle", SqlDbType.SmallInt).Value = int.Parse(textBox9.Text);
            }
            else
            {
                scCommand.Parameters.AddWithValue("@nro_calle", DBNull.Value);
            }

            if (textBox5.Text != "")
            {
                scCommand.Parameters.Add("@cant_est", SqlDbType.TinyInt).Value = int.Parse(textBox5.Text);
            }
            else
            {
                scCommand.Parameters.AddWithValue("@cant_est", DBNull.Value);
            }

            if (textBox6.Text != "")
            {
                scCommand.Parameters.Add("@rec_estrella", SqlDbType.Int).Value = int.Parse(textBox6.Text);
            }
            else
            {
                scCommand.Parameters.AddWithValue("@rec_estrella", DBNull.Value);
            }

            if (comboBox1.Text != "")
            {
                scCommand.Parameters.Add("@loc", SqlDbType.VarChar, 50).Value = comboBox1.Text;
            }
            else
            {
                scCommand.Parameters.AddWithValue("@loc", DBNull.Value);
            }

            if (comboBox2.Text != "")
            {
                scCommand.Parameters.Add("@pais", SqlDbType.VarChar, 50).Value = comboBox2.Text;
            }
            else
            {
                scCommand.Parameters.AddWithValue("@pais", DBNull.Value);
            }

            scCommand.Parameters.Add("@usr", SqlDbType.VarChar, 50).Value = Login.Class1.user;
            int año = int.Parse(ConfigurationManager.AppSettings["Año"]);
            int mes = int.Parse(ConfigurationManager.AppSettings["Mes"]);
            int dia = int.Parse(ConfigurationManager.AppSettings["Dia"]);
            scCommand.Parameters.Add("@fe_sist", SqlDbType.Date).Value = new DateTime(año, mes, dia);
            scCommand.Parameters.Add("@error", SqlDbType.TinyInt).Direction = ParameterDirection.Output;
            scCommand.Parameters.Add("@id_hotel_new", SqlDbType.Int).Direction = ParameterDirection.Output;
            if (scCommand.Connection.State == ConnectionState.Closed)
            {
                scCommand.Connection.Open();
            }
            scCommand.ExecuteNonQuery();
            int result = int.Parse(scCommand.Parameters["@error"].Value.ToString());
            if (result != 1)
            {
                string mensaje = string.Format("Error en la carga de hotel, COD: {0}", result);
                MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conError = true;
            }
            else
            {
                string idHotelNew = scCommand.Parameters["@id_hotel_new"].Value.ToString();
                if (idHotel != "")
                {
                    spregimen(idHotel, con, transaction, conError);
                }
                else
                {
                    spregimen(idHotelNew, con, transaction, conError);
                }
            }
            if (!conError)
            {
                transaction.Commit();
                MessageBox.Show("Hotel agregado correctamente", "Operacion realizada correctamente", MessageBoxButtons.OK, MessageBoxIcon.None);
            }
            else
            {
                transaction.Rollback();
            }
            con.Close();
        }

        private void spregimen(string Hotel, SqlConnection con, SqlTransaction transaction, bool conError)
        {
            //if (idHotel != "")
            //{
                if (checkedListBox1.CheckedItems.Count != 0)
                {
                    for (int i = 0; i < checkedListBox1.Items.Count; i++)
                    {
                        if (checkedListBox1.GetItemChecked(i) && !listReg[i])
                        {
                            SqlCommand scCommand = new SqlCommand("CONTROL_ZETA.SP_REGIMEN_HOTEL", con, transaction);
                            scCommand.CommandType = CommandType.StoredProcedure;
                            scCommand.Parameters.Add("@accion", SqlDbType.TinyInt).Value = 1;
                            scCommand.Parameters.Add("@id_hotel", SqlDbType.Int).Value = int.Parse(Hotel);
                            scCommand.Parameters.Add("@id_regimen", SqlDbType.TinyInt).Value = i + 1;
                            int año = int.Parse(ConfigurationManager.AppSettings["Año"]);
                            int mes = int.Parse(ConfigurationManager.AppSettings["Mes"]);
                            int dia = int.Parse(ConfigurationManager.AppSettings["Dia"]);
                            scCommand.Parameters.Add("@fe_sist", SqlDbType.Date).Value = new DateTime(año, mes, dia);
                            scCommand.Parameters.Add("@error", SqlDbType.TinyInt).Direction = ParameterDirection.Output;
                            if (scCommand.Connection.State == ConnectionState.Closed)
                            {
                                scCommand.Connection.Open();
                            }
                            scCommand.ExecuteNonQuery();
                            int result = int.Parse(scCommand.Parameters["@error"].Value.ToString());
                            if (result != 1)
                            {
                                string mensaje = string.Format("Error en la alta de regimen, COD: {0}", result);
                                MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                conError = true;
                            }
                        }
                        if (!checkedListBox1.GetItemChecked(i) && listReg[i])
                        {
                            SqlCommand scCommand = new SqlCommand("CONTROL_ZETA.SP_REGIMEN_HOTEL", con, transaction);
                            scCommand.CommandType = CommandType.StoredProcedure;
                            scCommand.Parameters.Add("@accion", SqlDbType.TinyInt).Value = 3;
                            scCommand.Parameters.Add("@id_hotel", SqlDbType.Int).Value = int.Parse(Hotel);
                            scCommand.Parameters.Add("@id_regimen", SqlDbType.TinyInt).Value = i + 1;
                            int año = int.Parse(ConfigurationManager.AppSettings["Año"]);
                            int mes = int.Parse(ConfigurationManager.AppSettings["Mes"]);
                            int dia = int.Parse(ConfigurationManager.AppSettings["Dia"]);
                            scCommand.Parameters.Add("@fe_sist", SqlDbType.Date).Value = new DateTime(año, mes, dia);
                            scCommand.Parameters.Add("@error", SqlDbType.TinyInt).Direction = ParameterDirection.Output;
                            if (scCommand.Connection.State == ConnectionState.Closed)
                            {
                                scCommand.Connection.Open();
                            }
                            scCommand.ExecuteNonQuery();
                            int result = int.Parse(scCommand.Parameters["@error"].Value.ToString());
                            if (result != 1)
                            {
                                string mensaje = string.Format("Error en la baja de regimen, COD: {0}", result);
                                MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                conError = true;
                            }
                        }
                    }
                //}
                //else
                //{

                //}
            }
            else
            {
                MessageBox.Show("Se debe seleccionar al menos un regimen", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            actualizarRegimenes();

        }

        private void actualizarRegimenes()
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                if (checkedListBox1.GetItemChecked(i))
                {
                    listReg[i] = true;
                }
                else
                {
                    listReg[i] = false;
                }
            }
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
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

        private void textBox9_KeyPress(object sender, KeyPressEventArgs e)
        {
            AllowNumber(e);
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            AllowNumber(e);
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            AllowNumber(e);
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
