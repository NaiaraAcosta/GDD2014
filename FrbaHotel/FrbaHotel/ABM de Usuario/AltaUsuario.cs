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
using System.Security.Cryptography;  

namespace FrbaHotel.ABM_de_Usuario
{
    public partial class AltaUsuario : Form
    {
        Form back = null;
        List<int> codRol = new List<int>();
        List<int> tipoDoc = new List<int>();
        int modo;
        public AltaUsuario()
        {
            InitializeComponent();
        }
        public AltaUsuario(Form atras, string user)
        {
            InitializeComponent();
            back = atras;
            llenarChecked();
            actualizarChecked(user);
            llenarCombo();
            llenarText(user);

            modo = 2;
        }

        public AltaUsuario(Form atras)
        {
            InitializeComponent();
            back = atras;
            llenarChecked();
            llenarCombo();
            modo = 1;
        }

        private void actualizarChecked(string user)
        {
            string ConnStr = ConfigurationManager.AppSettings["stringConexion"];
            SqlConnection conn = new SqlConnection(ConnStr);
            string comm = string.Format(@"SELECT * FROM [GD2C2014].[CONTROL_ZETA].[USR_ROL_HOTEL] usrrol
                    where usrrol.HOTEL_ID = '{0}'
                    and usrrol.USR_USERNAME = '{1}'",Login.Class1.hotel, user);
            SqlCommand cmd = new SqlCommand(comm, conn);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                checkedListBox1.SetItemChecked(int.Parse(reader["ROL_ID"].ToString()) - 1, true);
            }
            reader.Close();
            conn.Close();
        }

        private void llenarCombo()
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

        private void llenarChecked()
        {
            string ConnStr = ConfigurationManager.AppSettings["stringConexion"];
            SqlConnection conn = new SqlConnection(ConnStr);
            string comm = string.Format(@"SELECT * FROM [GD2C2014].[CONTROL_ZETA].[ROL] rol");
            SqlCommand cmd = new SqlCommand(comm, conn);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                checkedListBox1.Items.Add(reader["ROL_NOMBRE"].ToString());
                codRol.Add(int.Parse(reader["ROL_ID"].ToString()));
            }
            reader.Close();
            conn.Close();
        }

        private void llenarText(string user)
        {
            string ConnStr = ConfigurationManager.AppSettings["stringConexion"];
            SqlConnection conn = new SqlConnection(ConnStr);
            string comm = string.Format(@"SELECT * FROM [GD2C2014].[CONTROL_ZETA].[EMPLEADO]
                    where USR_USERNAME = '{0}'", user);
            SqlCommand cmd = new SqlCommand(comm, conn);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                textBox1.Text = reader["USR_USERNAME"].ToString();
                textBox3.Text = reader["EMP_NOMBRE"].ToString();
                textBox4.Text = reader["EMP_APELLIDO"].ToString();
                comboBox1.SelectedIndex = int.Parse(reader["EMP_ID_TIPO_DOC"].ToString()) - 1;
                textBox6.Text = reader["EMP_DOC"].ToString();
                textBox7.Text = reader["EMP_MAIL"].ToString();
                textBox8.Text = reader["EMP_TEL"].ToString();
                textBox9.Text = reader["EMP_DOM"].ToString();
                string[] stringSeparators = new string[] { "/" };
                string[] result = reader["EMP_FECHA_NAC"].ToString().Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
                result[2] = result[2].Substring(0, 4);
                dateTimePicker1.Value = new DateTime(int.Parse(result[2]), int.Parse(result[1]), int.Parse(result[0]));
            }
            reader.Close();
            conn.Close();

            conn = new SqlConnection(ConnStr);
            string sSel = string.Format(@"SELECT * FROM [GD2C2014].[CONTROL_ZETA].[HOTEL] hotel, 
				[GD2C2014].[CONTROL_ZETA].[LOCALIDAD] loc
                where  hotel.HOTEL_ID = '{0}'
                and loc.LOC_ID = hotel.HOTEL_ID_LOC", Login.Class1.hotel);
            cmd = new SqlCommand(sSel, conn);
            conn.Open();
            reader = cmd.ExecuteReader();
            string detalle = "";
            while (reader.Read())
            {
                if (reader["HOTEL_NOMBRE"].ToString() == "")
                {
                    detalle = string.Format("{0} - {1} {2}",
                        reader["LOC_DETALLE"].ToString().Trim(),
                        reader["HOTEL_CALLE"].ToString(),
                        reader["HOTEL_NRO_CALLE"].ToString());
                    textBox11.Text = detalle;
                }
                else
                {
                    detalle = string.Format("{0}",
                        reader["HOTEL_NOMBRE"].ToString().Trim());
                    textBox11.Text = detalle;
                }
            }
            reader.Close();
            conn.Close(); 
        }

        private void AltaUsuario_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string ConnStr = ConfigurationManager.AppSettings["stringConexion"];
            SqlConnection con = new SqlConnection(ConnStr);
            con.Open();
            SqlTransaction transaction = con.BeginTransaction();
            SqlCommand scCommand = new SqlCommand("CONTROL_ZETA.SP_ABM_USUARIO", con, transaction);
            scCommand.CommandType = CommandType.StoredProcedure;
            scCommand.Parameters.Add("@ACCION", SqlDbType.SmallInt).Value = modo;
            scCommand.Parameters.Add("@USUARIO", SqlDbType.VarChar, 50).Value = textBox1.Text;
            scCommand.Parameters.Add("@PASS", SqlDbType.VarChar).Value = encriptarPass();
            scCommand.Parameters.Add("@NOMBRE", SqlDbType.VarChar, 50).Value = textBox3.Text;
            scCommand.Parameters.Add("@APELLIDO", SqlDbType.VarChar, 50).Value = textBox4.Text;
            scCommand.Parameters.Add("@TIPO_DOC", SqlDbType.TinyInt).Value = tipoDoc[comboBox1.SelectedIndex];
            scCommand.Parameters.Add("@DOC", SqlDbType.VarChar, 15).Value = textBox6.Text;
            scCommand.Parameters.Add("@MAIL", SqlDbType.VarChar, 50).Value = textBox7.Text;
            scCommand.Parameters.Add("@TEL", SqlDbType.VarChar, 10).Value = textBox8.Text;
            scCommand.Parameters.Add("@DOM", SqlDbType.VarChar, 50).Value = textBox9.Text;
            scCommand.Parameters.Add("@FECHA_NAC", SqlDbType.Date).Value = dateTimePicker1.Value;
            scCommand.Parameters.Add("@ESTADO", SqlDbType.VarChar, 1).Value = "H";
            scCommand.Parameters.Add("@ERROR", SqlDbType.TinyInt).Direction = ParameterDirection.Output;
            if (scCommand.Connection.State == ConnectionState.Closed)
            {
                scCommand.Connection.Open();
            }
            scCommand.ExecuteNonQuery();
            int result = int.Parse(scCommand.Parameters["@ERROR"].Value.ToString());
            bool conError = true;
            switch (result)
            {
                case 1:
                    {
                        MessageBox.Show("Operacion realizada exitosamente", "Operacion realizada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        conError = false;
                        break;
                    }
                case 2:
                    {
                        MessageBox.Show("Error 2", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }
                case 4:
                    {
                        MessageBox.Show("Error 4", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }
                default:
                    {
                        string mensaje = string.Format("Error en la operacion, COD: {0}", result);
                        MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }
            }
            
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                if (checkedListBox1.GetItemChecked(i))
                {
                    scCommand = new SqlCommand("CONTROL_ZETA.SP_USR_ROL_HOTEL", con, transaction);
                    scCommand.CommandType = CommandType.StoredProcedure;
                    scCommand.Parameters.Add("@USUARIO", SqlDbType.VarChar, 50).Value = textBox1.Text;
                    scCommand.Parameters.Add("@ROL_ID", SqlDbType.TinyInt).Value = i+1;
                    scCommand.Parameters.Add("@HOTEL_ID", SqlDbType.Int).Value = Login.Class1.hotel;
                    if (scCommand.Connection.State == ConnectionState.Closed)
                    {
                        scCommand.Connection.Open();
                    }
                    scCommand.ExecuteNonQuery();
                    //result = int.Parse(scCommand.Parameters["@ERROR"].Value.ToString());
                    //if (result == 1)
                    //{
                    //    conError = false;
                    //}
                }
            }
            
            if (conError)
            {
                transaction.Rollback();
            }
            else
            {
                transaction.Commit();
            }
            con.Close();
        }
        private string encriptarPass()
        {
            return SHA256Encrypt(textBox2.Text);
        }

        public string SHA256Encrypt(string input)
        {
            SHA256CryptoServiceProvider provider = new SHA256CryptoServiceProvider();

            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hashedBytes = provider.ComputeHash(inputBytes);

            StringBuilder output = new StringBuilder();

            for (int i = 0; i < hashedBytes.Length; i++)
                output.Append(hashedBytes[i].ToString("x2").ToLower());

            return output.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            back.Show();
            this.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
