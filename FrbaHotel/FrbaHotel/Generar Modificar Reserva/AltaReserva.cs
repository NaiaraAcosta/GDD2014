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

        public AltaReserva()
        {
            InitializeComponent();
        }

        public AltaReserva(Form atras)
        {
            InitializeComponent();
            back = atras;
            dateTimePicker1.MinDate = DateTime.Today;
            if (Login.Class1.hotel != 0)
            {
                string ConnStr = @"Data Source=localhost\SQLSERVER2008;Initial Catalog=GD2C2014;User ID=gd;Password=gd2014;Trusted_Connection=False;";

                SqlConnection conn = new SqlConnection(ConnStr);
                string sSel = string.Format(@"SELECT * FROM [GD2C2014].[CONTROL_ZETA].[HOTEL] hotel, 
                    [GD2C2014].[CONTROL_ZETA].[LOCALIDAD] loc 
                    where hotel.HOTEL_ID = '{0}' 
                    and hotel.HOTEL_ID_LOC = loc.LOC_ID", Login.Class1.hotel);
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
                    comboBox3.Text = detalle;
                    comboBox3.Enabled = false;
                }
                reader.Close();
                conn.Close();
            }
            else
            {
                string ConnStr = @"Data Source=localhost\SQLSERVER2008;Initial Catalog=GD2C2014;User ID=gd;Password=gd2014;Trusted_Connection=False;";

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
                reader.Close();
                conn.Close();
            }

            string ConnStr2 = @"Data Source=localhost\SQLSERVER2008;Initial Catalog=GD2C2014;User ID=gd;Password=gd2014;Trusted_Connection=False;";

            SqlConnection conn2 = new SqlConnection(ConnStr2);
            string sSel2 = string.Format(@"SELECT * FROM [GD2C2014].[CONTROL_ZETA].[TIPO_HAB]");
            SqlCommand cmd2 = new SqlCommand(sSel2, conn2);
            conn2.Open();
            SqlDataReader reader2 = cmd2.ExecuteReader();
            while (reader2.Read())
            {
                checkedListBox1.Items.Add(reader2["TIPO_HAB_DESCRIPCION"].ToString());
                idHab.Add(int.Parse(reader2["TIPO_HAB_ID"].ToString()));
            }
            reader2.Close();
            conn2.Close();

            sSel2 = string.Format(@"SELECT * FROM [GD2C2014].[CONTROL_ZETA].[REGIMEN]");
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
        }

        public AltaReserva(Form atras, SqlDataReader reader, SqlConnection conn)
        {
            InitializeComponent();
            back = atras;
            //dateTimePicker1.MinDate = DateTime.Today;
            string fechaInicio = "";
            string fechaHasta = "";
            string detalle = "";
            while (reader.Read())
            {
                fechaInicio = reader["RESERVA_FECHA_INICIO"].ToString();
                string[] stringSeparators = new string[] { "/"};
                string[] result = fechaInicio.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
                result[2] = result[2].Substring(0, 4);
                dateTimePicker1.Value = new DateTime(int.Parse(result[2]), int.Parse(result[1]), int.Parse(result[0]));

                detalle = string.Format("{0} - {1} {2}",

                    reader["LOC_DETALLE"].ToString().Trim(),
                    reader["HOTEL_CALLE"].ToString(),
                    reader["HOTEL_NRO_CALLE"].ToString());
                idHotel.Add(int.Parse(reader["HOTEL_ID"].ToString()));
                comboBox3.Text = detalle;
                comboBox3.Enabled = false;
            }
            reader.Close();
            conn.Close();
        }
                

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            dateTimePicker2.MinDate = dateTimePicker1.Value;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {

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
            back.Show();
            this.Close();
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
            Form f = new TipoCliente(this, back);
            f.Show();
            this.Hide();
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked && e.CurrentValue == CheckState.Unchecked)
            {
                checkedListBox1.Items.Insert(e.Index + 1, checkedListBox1.SelectedItem.ToString());
            }
            if (e.NewValue == CheckState.Unchecked && e.CurrentValue == CheckState.Checked)
            {
                if (e.Index == proximoChecked(e.Index))
                {
                    checkedListBox1.Items.RemoveAt(e.Index);
                }
                else
                {
                    checkedListBox1.Items.RemoveAt(proximoChecked(e.Index));
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

        private void checkedListBox1_MouseClick(object sender, MouseEventArgs e)
        {
           
        }
    }
}
