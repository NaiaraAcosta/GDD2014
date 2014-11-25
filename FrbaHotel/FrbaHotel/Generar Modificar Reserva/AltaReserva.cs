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
    public partial class AltaReserva : Form
    {
        Form back = null;
        List<int> idHotel = new List<int>();
        List<int> idHab = new List<int>();
        List<int> idReg = new List<int>();
        List<int> listHab = new List<int>();
        bool verificado;
        bool yaVerificado;
        bool primeraVez = true;
        string idResTemp = "";
        string reserva = "";
        string cliente = "";
        public AltaReserva()
        {
            InitializeComponent();
        }

        public AltaReserva(Form atras)
        {
            InitializeComponent();
            back = atras;
            cargarDatos();
            int año = int.Parse(ConfigurationManager.AppSettings["Año"]);
                int mes = int.Parse(ConfigurationManager.AppSettings["Mes"]);
                int dia = int.Parse(ConfigurationManager.AppSettings["Dia"]);
                dateTimePicker1.MinDate = new DateTime(año, mes, dia);
                dateTimePicker1.Value = new DateTime(año, mes, dia);
            dateTimePicker2.MinDate = new DateTime (año, mes, dia);
        }

        public AltaReserva(Form atras, SqlDataReader reader, SqlConnection conn)
        {
            InitializeComponent();
            back = atras;
            cargarDatos();
           
            //dateTimePicker1.MinDate = DateTime.Today;
            string fechaInicio = "";
            string fechaHasta = "";
            string detalle = "";
            //while (reader.Read())
            //{
                fechaInicio = reader["RESERVA_FECHA_INICIO"].ToString();
                string[] stringSeparators = new string[] { "/"};
                string[] result = fechaInicio.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
                result[2] = result[2].Substring(0, 4);
                dateTimePicker1.Value = new DateTime(int.Parse(result[2]), int.Parse(result[1]), int.Parse(result[0]));

                fechaHasta = reader["RESERVA_FECHA_HASTA"].ToString();
                result = fechaHasta.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
                result[2] = result[2].Substring(0, 4);
                dateTimePicker2.Value = new DateTime(int.Parse(result[2]), int.Parse(result[1]), int.Parse(result[0]));

                comboBox2.Text = reader["REG_DESCRIPCION"].ToString();

                reserva = reader["RESERVA_ID"].ToString();
                cliente = reader["CLIENTE_ID"].ToString();

                detalle = string.Format("{0} - {1} {2}",
                        reader["LOC_DETALLE"].ToString().Trim(),
                        reader["HOTEL_CALLE"].ToString(),
                        reader["HOTEL_NRO_CALLE"].ToString());
                    idHotel.Add(int.Parse(reader["HOTEL_ID"].ToString()));
                    comboBox3.Text = detalle;
                    //comboBox3.Enabled = true;
            //}
            reader.Close();
            conn.Close();
            cargarHabitaciones(reserva);
        }

        private void cargarHabitaciones(string reservaID)
        {
            string ConnStr2 = ConfigurationManager.AppSettings["stringConexion"];
            SqlConnection conn2 = new SqlConnection(ConnStr2);
            string sSel2 = string.Format(@"SELECT * FROM [GD2C2014].[CONTROL_ZETA].[RESERVA_HABITACION] reshab,
                    [GD2C2014].[CONTROL_ZETA].[HABITACION] hab, 
                    [GD2C2014].[CONTROL_ZETA].[TIPO_HAB] tipohab
                    where reshab.RESERVA_ID = '{0}' 
                    and reshab.HAB_ID = hab.HAB_ID
                    and hab.HAB_ID_TIPO = tipohab.TIPO_HAB_ID", reservaID);
            SqlCommand cmd2 = new SqlCommand(sSel2, conn2);
            conn2.Open();
            SqlDataReader reader2 = cmd2.ExecuteReader();
            while (reader2.Read())
            {
                string lectura = reader2["TIPO_HAB_DESCRIPCION"].ToString();
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    int primerUnCheckeado = primerUncheked(i);
                    checkedListBox1.SetSelected(primerUnCheckeado, true);
                    if (checkedListBox1.SelectedItem.ToString() == lectura)
                    {
                        checkedListBox1.SetItemChecked(primerUnCheckeado, true);
                        lectura = "";
                    }
                    checkedListBox1.SetSelected(primerUnCheckeado, false);
                }
            }
            reader2.Close();
            conn2.Close();

            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void cargarDatos()
        {
            string ConnStr2 = ConfigurationManager.AppSettings["stringConexion"];
            SqlConnection conn2 = new SqlConnection(ConnStr2);
            string sSel2 = string.Format(@"SELECT * FROM [GD2C2014].[CONTROL_ZETA].[TIPO_HAB]");
            SqlCommand cmd2 = new SqlCommand(sSel2, conn2);
            conn2.Open();
            SqlDataReader reader2 = cmd2.ExecuteReader();
            while (reader2.Read())
            {
                checkedListBox1.Items.Add(reader2["TIPO_HAB_DESCRIPCION"].ToString());
                idHab.Add(int.Parse(reader2["TIPO_HAB_ID"].ToString()));
                listHab.Add(int.Parse(reader2["TIPO_HAB_ID"].ToString()));
            }
            reader2.Close();
            conn2.Close();

            sSel2 = string.Format(@"select * from [GD2C2014].[CONTROL_ZETA].[REGIMEN] reg, [GD2C2014].[CONTROL_ZETA].[HOTEL_REGIMEN] hotreg
	                where hotreg.HOTEL_ID = '{0}'
	                and hotreg.REG_ID = reg.REG_ID", Login.Class1.hotel);
            cmd2 = new SqlCommand(sSel2, conn2);
            conn2.Open();
            reader2 = cmd2.ExecuteReader();
            while (reader2.Read())
            {
                comboBox2.Items.Add(reader2["REG_DESCRIPCION"].ToString());
                idReg.Add(int.Parse(reader2["REG_ID"].ToString()));
            }
            reader2.Close();
            conn2.Close();
            {
                string ConnStr = ConfigurationManager.AppSettings["stringConexion"];
                SqlConnection conn = new SqlConnection(ConnStr);
                string sSel = string.Format(@"SELECT * FROM [GD2C2014].[CONTROL_ZETA].[HOTEL] hotel, 
                    [GD2C2014].[CONTROL_ZETA].[LOCALIDAD] loc 
                    where hotel.HOTEL_ID_LOC = loc.LOC_ID");
                SqlCommand cmd = new SqlCommand(sSel, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                string detalle = "";
                while (reader.Read())
                {
                    detalle = string.Format("{0} - {1} {2}",
                        reader["LOC_DETALLE"].ToString().Trim(),
                        reader["HOTEL_CALLE"].ToString(),
                        reader["HOTEL_NRO_CALLE"].ToString());
                    idHotel.Add(int.Parse(reader["HOTEL_ID"].ToString()));
                    comboBox3.Items.Add(detalle);
                    comboBox3.Enabled = true;
                }
                if (Login.Class1.hotel != 0)
                {
                    comboBox3.SelectedIndex = Login.Class1.hotel - 1;
                    comboBox3.Text = comboBox3.SelectedItem.ToString();
                    comboBox3.Enabled = false;
                }
                reader.Close();
                conn.Close();
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            dateTimePicker2.MinDate = dateTimePicker1.Value;
            yaVerificado = false;
            informar(verificado, yaVerificado);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            yaVerificado = false;
            informar(verificado, yaVerificado);
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            yaVerificado = false;
            informar(verificado, yaVerificado);
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string ConnStr = ConfigurationManager.AppSettings["stringConexion"];
            SqlConnection con = new SqlConnection(ConnStr);
            con.Open();
            limpiarSiEsNecesario(con);
            con.Close();
            back.Show();
            this.Close();
        }

        private void limpiarSiEsNecesario(SqlConnection con)
        {
            if (!primeraVez)
            {
                SqlCommand scCommand = new SqlCommand("CONTROL_ZETA.LIMPIAR_PEDIDO", con);
                scCommand.CommandType = CommandType.StoredProcedure;
                    if (scCommand.Connection.State == ConnectionState.Closed)
                    {
                        scCommand.Connection.Open();
                    }
                    scCommand.ExecuteNonQuery();
                            }
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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (reserva == "")
            {
                string[] param = new string[7];
                param[0] = idHotel[comboBox3.SelectedIndex].ToString();
                param[1] = dateTimePicker1.Value.ToString();
                param[2] = dateTimePicker2.Value.ToString();
                param[3] = idReg[comboBox2.SelectedIndex].ToString();
                param[4] = ""; //cliente id
                param[5] = Login.Class1.user; //id usr
                int año = int.Parse(ConfigurationManager.AppSettings["Año"]);
                int mes = int.Parse(ConfigurationManager.AppSettings["Mes"]);
                int dia = int.Parse(ConfigurationManager.AppSettings["Dia"]);
                param[6] = new DateTime(año, mes, dia).ToString();
                TipoCliente f = new TipoCliente(this, back, param);
                f.Show();
                this.Hide();
            }
            else
            {
                string[] param = new string[8];
                param[0] = idHotel[comboBox3.SelectedIndex].ToString();
                param[1] = dateTimePicker1.Value.ToString();
                param[2] = dateTimePicker2.Value.ToString();
                param[3] = idReg[comboBox2.SelectedIndex].ToString();
                param[4] = cliente; //cliente id
                param[5] = Login.Class1.user; //id usr
                param[6] = reserva;
                int año = int.Parse(ConfigurationManager.AppSettings["Año"]);
                int mes = int.Parse(ConfigurationManager.AppSettings["Mes"]);
                int dia = int.Parse(ConfigurationManager.AppSettings["Dia"]);
                param[7] = new DateTime(año, mes, dia).ToString();
                ReservaFinalizada f = new ReservaFinalizada(this, back, param);
                f.Show();
                this.Hide();
            }
            
            
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            yaVerificado = false;
            informar(verificado, yaVerificado);
        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked && e.CurrentValue == CheckState.Unchecked)
            {
                checkedListBox1.Items.Insert(e.Index + 1, checkedListBox1.SelectedItem.ToString());
                listHab.Insert(e.Index + 1, listHab[e.Index]);
                
            }
            if (e.NewValue == CheckState.Unchecked && e.CurrentValue == CheckState.Checked)
            {
                if (e.Index == proximoChecked(e.Index))
                {
                    checkedListBox1.Items.RemoveAt(e.Index);
                    listHab.RemoveAt(e.Index);
                }
                else
                {
                    checkedListBox1.Items.RemoveAt(proximoChecked(e.Index));
                    listHab.RemoveAt(proximoChecked(e.Index));
                    e.NewValue = e.CurrentValue;
                }
            }
        }
        private int proximoChecked(int aBorrar)
        {
            if (!checkedListBox1.GetItemChecked(aBorrar + 1))
            {
                return aBorrar;
            }
            else
            {
                return proximoChecked(aBorrar + 1);
            }
        }

        private int primerUncheked(int comienzo)
        {
            if (!checkedListBox1.GetItemChecked(comienzo))
            {
                return comienzo;
            }
            else
            {
                return primerUncheked(comienzo + 1);
            }
        }

        private void checkedListBox1_MouseClick(object sender, MouseEventArgs e)
        {
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (sePuedeVerificar())
            {
                List<int> cantidad = new List<int>();
                List<int> result = new List<int>();
                string ConnStr = ConfigurationManager.AppSettings["stringConexion"];
                SqlConnection con = new SqlConnection(ConnStr);
                con.Open();
                limpiarSiEsNecesario(con);
                primeraVez = false;
                SqlTransaction transaction = con.BeginTransaction();
                //try
                //{
                    for (int i = 0; i < idHab.Count; i++)
                    {
                        int tipo = idHab[i];
                        cantidad.Add(cantHab(tipo));
                        if (cantidad[i] != 0)
                        {
                            SqlCommand scCommand = new SqlCommand("CONTROL_ZETA.SP_VERIFICA_DISPONIBILIDAD", con, transaction);
                            scCommand.CommandType = CommandType.StoredProcedure;
                            if (reserva == "")
                            {
                                scCommand.Parameters.AddWithValue("@id_res", DBNull.Value);
                            }
                            else
                            {
                                scCommand.Parameters.Add("@id_res", SqlDbType.Int).Value = int.Parse(reserva);
                            }
                            scCommand.Parameters.Add("@hotel_id", SqlDbType.Int).Value = idHotel[comboBox3.SelectedIndex];
                            scCommand.Parameters.Add("@fe_desde", SqlDbType.Date).Value = dateTimePicker1.Value;
                            scCommand.Parameters.Add("@fe_hasta ", SqlDbType.Date).Value = dateTimePicker2.Value;
                            scCommand.Parameters.Add("@cant_hab", SqlDbType.TinyInt).Value = cantidad[i];
                            int año = int.Parse(ConfigurationManager.AppSettings["Año"]);
                            int mes = int.Parse(ConfigurationManager.AppSettings["Mes"]);
                            int dia = int.Parse(ConfigurationManager.AppSettings["Dia"]);
                            scCommand.Parameters.Add("@fe_sist", SqlDbType.Date).Value = new DateTime(año, mes, dia);
                            scCommand.Parameters.Add("@id_tipo_hab", SqlDbType.SmallInt).Value = tipo;
                            scCommand.Parameters.Add("@id_regimen", SqlDbType.TinyInt).Value = idReg[comboBox2.SelectedIndex];
                            scCommand.Parameters.Add("@res", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                            scCommand.Parameters.Add("@id_res_new_temp", SqlDbType.Int).Direction = ParameterDirection.Output;
                                if (scCommand.Connection.State == ConnectionState.Closed)
                                {
                                    scCommand.Connection.Open();
                                }
                                scCommand.ExecuteNonQuery();
                                result.Add(int.Parse(scCommand.Parameters["@res"].Value.ToString()));
                                idResTemp = scCommand.Parameters["@id_res_new_temp"].Value.ToString();
                        }
                    }
                    transaction.Commit();
                //}
                //catch (SqlException)
                //{
                //    transaction.Rollback();
                //}


                verificado = true;
                for (int i = 0; i < result.Count; i++)
                {
                    if (result[i] == 0)
                    {
                        verificado = false;
                    }
                }
                yaVerificado = true;
                informar(verificado, yaVerificado);

                if (verificado)
                {
                    SqlCommand Totalf = new SqlCommand("SELECT CONTROL_ZETA.SP_PRECIO_TOTAL(@criterio ,@fe_desde ,@fe_hasta ,@id_res ,@id_hotel)", con);
                    Totalf.Parameters.Add("@criterio", SqlDbType.TinyInt).Value = 0;
                    Totalf.Parameters.Add("@fe_desde", SqlDbType.Date).Value = dateTimePicker1.Value;
                    Totalf.Parameters.Add("@fe_hasta", SqlDbType.Date).Value = dateTimePicker2.Value;
                    Totalf.Parameters.Add("@id_res", SqlDbType.Int).Value = idResTemp;
                    Totalf.Parameters.Add("@id_hotel", SqlDbType.Int).Value = idHotel[comboBox3.SelectedIndex];
                    SqlDataReader reader = Totalf.ExecuteReader();
                    string precioNoche = "";
                    while (reader.Read())
                    {
                        precioNoche = reader[0].ToString();
                    }
                    reader.Close();

                    Totalf = new SqlCommand("SELECT CONTROL_ZETA.SP_PRECIO_TOTAL(@criterio ,@fe_desde ,@fe_hasta ,@id_res ,@id_hotel)", con);
                    Totalf.Parameters.Add("@criterio", SqlDbType.TinyInt).Value = 1;
                    Totalf.Parameters.Add("@fe_desde", SqlDbType.Date).Value = dateTimePicker1.Value;
                    Totalf.Parameters.Add("@fe_hasta", SqlDbType.Date).Value = dateTimePicker2.Value;
                    Totalf.Parameters.Add("@id_res", SqlDbType.Int).Value = idResTemp;
                    Totalf.Parameters.Add("@id_hotel", SqlDbType.Int).Value = idHotel[comboBox3.SelectedIndex];
                    reader = Totalf.ExecuteReader();
                    string precioTotal = "";
                    while (reader.Read())
                    {
                        precioTotal = reader[0].ToString();
                    }

                    label7.Text = "El precio por noche es de: " + precioNoche;
                    label8.Text = "El precio total es de: " + precioTotal;
                }
                con.Close();
            }
            else
            {
                MessageBox.Show("Existen datos invalidos", "No se puede verificar disponibilidad", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void informar(bool verificado, bool yaVerificado)
        {
            if (yaVerificado)
            {
                if (verificado)
                {
                    label6.Text = "Existe disponibilidad para reservar";
                    button1.Enabled = true;
                }
                else
                {
                    label6.Text = "No existe disponibilidad para reservar";
                    label7.Text = "";
                    label8.Text = "";
                    button1.Enabled = false;
                }
            }
            else
            {
                label6.Text = "Se debe verificar la reserva antes de continuar";
                label7.Text = "";
                label8.Text = "";
                button1.Enabled = false;
            }
        }
        private int cantHab(int tipo)
        {
            return listHab.Count(x => x == tipo) - 1;
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            yaVerificado = false;
            informar(verificado, yaVerificado);

            comboBox2.Items.Clear();
            idReg.Clear();

            string ConnStr2 = ConfigurationManager.AppSettings["stringConexion"];
            SqlConnection conn2 = new SqlConnection(ConnStr2);
            string sSel2 = string.Format(@"select * from [GD2C2014].[CONTROL_ZETA].[REGIMEN] reg, [GD2C2014].[CONTROL_ZETA].[HOTEL_REGIMEN] hotreg
	                where hotreg.HOTEL_ID = '{0}'
	                and hotreg.REG_ID = reg.REG_ID", comboBox3.SelectedIndex + 1);
            SqlCommand cmd2 = new SqlCommand(sSel2, conn2);
            conn2.Open();
            SqlDataReader reader2 = cmd2.ExecuteReader();
            while (reader2.Read())
            {
                comboBox2.Items.Add(reader2["REG_DESCRIPCION"].ToString());
                idReg.Add(int.Parse(reader2["REG_ID"].ToString()));
            }
            reader2.Close();
            conn2.Close();
        }

        private bool sePuedeVerificar()
        {
            if (comboBox2.SelectedItem != null &&
                comboBox3.SelectedItem != null &&
                !(dateTimePicker1.Value.Day == dateTimePicker2.Value.Day &&
                dateTimePicker1.Value.Month == dateTimePicker2.Value.Month &&
                dateTimePicker1.Value.Year == dateTimePicker2.Value.Year) &&
                checkedListBox1.CheckedIndices.Count != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
