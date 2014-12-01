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

namespace FrbaHotel.ABM_de_Habitacion
{
    public partial class AltaHabitacion : Form
    {
        ABM_de_Habitacion.ABMHabitacion back = null;
        List<string> tipoHabId = new List<string>();
        int modo;
        int idHab;
        public AltaHabitacion()
        {
            InitializeComponent();
        }

        public AltaHabitacion(Form atras)
        {
            InitializeComponent();
            cargarCombo();
            back = (ABM_de_Habitacion.ABMHabitacion) atras;
            modo = 1;
            checkBox1.Enabled = false;
        }

        public AltaHabitacion(Form atras, string id ,string num, string piso, string ubi, string tipo, string comodidades)
        {
            InitializeComponent();
            back = (ABM_de_Habitacion.ABMHabitacion) atras;
            modo = 2;
            cargarCombo();
            idHab = int.Parse(id);
            textBox1.Text = num;
            textBox2.Text = piso;
            textBox3.Text = ubi;
            comboBox1.SelectedIndex = tipoHabId.FindIndex(x => x == tipo);
            comboBox1.Enabled = false;
            richTextBox1.Text = comodidades;
        }

        private void cargarCombo()
        {
            string ConnStr = ConfigurationManager.AppSettings["stringConexion"];
            SqlConnection conn = new SqlConnection(ConnStr);
            string sSel = string.Format(@"SELECT distinct TIPO_HAB_ID, TIPO_HAB_DESCRIPCION FROM [GD2C2014].[CONTROL_ZETA].[HABITACION] hab, 
                [GD2C2014].[CONTROL_ZETA].[TIPO_HAB] tipohab 
                where hab.HAB_ID_TIPO = tipohab.TIPO_HAB_ID");
            SqlCommand cmd = new SqlCommand(sSel, conn);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                comboBox1.Items.Add(reader["TIPO_HAB_DESCRIPCION"].ToString());
                tipoHabId.Add(reader["TIPO_HAB_ID"].ToString());
            }
            reader.Close();
            conn.Close();
        }

        private void label1_Click(object sender, EventArgs e)
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

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            back.Show();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string ConnStr = ConfigurationManager.AppSettings["stringConexion"];
            SqlConnection con = new SqlConnection(ConnStr);
            con.Open();
            SqlCommand scCommand = new SqlCommand("CONTROL_ZETA.SP_ABM_HABITACION", con);
            scCommand.CommandType = CommandType.StoredProcedure;
            scCommand.Parameters.Add("@accion", SqlDbType.TinyInt).Value = modo;
            scCommand.Parameters.Add("@nro_hab", SqlDbType.SmallInt).Value = int.Parse(textBox1.Text);
            scCommand.Parameters.Add("@id_hab", SqlDbType.Int).Value = idHab;
            scCommand.Parameters.Add("@hab_piso", SqlDbType.SmallInt).Value = int.Parse(textBox2.Text);
            scCommand.Parameters.Add("@ubi_hab", SqlDbType.VarChar, 70).Value = textBox3.Text;
            if (checkBox1.Checked)
            {
                scCommand.Parameters.Add("@estado", SqlDbType.VarChar, 1).Value = "H";
            }
            else
            {
                scCommand.Parameters.Add("@estado", SqlDbType.VarChar, 1).Value = "I";
            }
            scCommand.Parameters.Add("@obs", SqlDbType.VarChar, 150).Value = richTextBox1.Text;
            scCommand.Parameters.Add("@id_hotel", SqlDbType.Int).Value = Login.Class1.hotel;
            scCommand.Parameters.Add("@id_tipo_hab", SqlDbType.SmallInt).Value = tipoHabId[comboBox1.SelectedIndex];
            scCommand.Parameters.Add("@error", SqlDbType.TinyInt).Direction = ParameterDirection.Output;
            scCommand.Parameters.Add("@id_hab_new", SqlDbType.Int).Direction = ParameterDirection.Output;
            if (scCommand.Connection.State == ConnectionState.Closed)
            {
                scCommand.Connection.Open();
            }
            scCommand.ExecuteNonQuery();

            int result = int.Parse(scCommand.Parameters["@error"].Value.ToString());

            switch (result)
            {
                case 1:
                    {
                        MessageBox.Show("Operacion realizada exitosamente", "Operacion realizada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        if (modo == 1)
                        {
                            limpiar();
                        }
                        break;
                    }
                case 2:
                    {
                        MessageBox.Show("No existe la habitacion en ese hotel", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }
                case 3:
                    {
                        MessageBox.Show("Ya existe la habitacion en ese hotel", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            back.recargar();
            if (modo == 2)
            {
                back.Show();
                this.Close();
            }
        }

        private void limpiar()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            comboBox1.Text = "";
            richTextBox1.Text = "";
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
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

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            AllowNumber(e);
        }
    }
}
