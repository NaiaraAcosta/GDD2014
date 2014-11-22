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

namespace FrbaHotel.ABM_de_Usuario
{
    public partial class ABMUsuario : Form
    {
        Form back = null;
        public ABMUsuario(Form atras)
        {
            InitializeComponent();
            back = atras;

            string ConnStr = ConfigurationManager.AppSettings["stringConexion"];
            SqlConnection conn = new SqlConnection(ConnStr);
            string sel = string.Format(@"SELECT distinct emple.USR_USERNAME from [GD2C2014].[CONTROL_ZETA].[ROL] rol,
                    [GD2C2014].[CONTROL_ZETA].[EMPLEADO] emple,
                    [GD2C2014].[CONTROL_ZETA].[USR_ROL_HOTEL] usrrol
                    where usrrol.HOTEL_ID = '{0}'
                    and usrrol.USR_USERNAME = emple.USR_USERNAME
                    and usrrol.ROL_ID = rol.ROL_ID", Login.Class1.hotel);
            SqlCommand cmd = new SqlCommand(sel, conn);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                listBox1.Items.Add(reader[0].ToString());
            }
            reader.Close();
            conn.Close(); 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new AltaUsuario(this).Show();
            this.Hide();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                new AltaUsuario(this, listBox1.SelectedItem.ToString()).Show();
                this.Hide();
            }
        }

        private void ABMUsuario_Load(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            back.Show();
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                string ConnStr = ConfigurationManager.AppSettings["stringConexion"];
                SqlConnection con = new SqlConnection(ConnStr);
                con.Open();
                SqlCommand scCommand = new SqlCommand("CONTROL_ZETA.SP_ABM_USUARIO", con);
                scCommand.CommandType = CommandType.StoredProcedure;
                scCommand.Parameters.Add("@ACCION", SqlDbType.SmallInt).Value = 3;
                scCommand.Parameters.Add("@USUARIO", SqlDbType.VarChar, 50).Value = listBox1.SelectedItem.ToString();
                scCommand.Parameters.AddWithValue("@PASS", DBNull.Value);
                scCommand.Parameters.AddWithValue("@NOMBRE", DBNull.Value);
                scCommand.Parameters.AddWithValue("@APELLIDO", DBNull.Value);
                scCommand.Parameters.AddWithValue("@TIPO_DOC", DBNull.Value);
                scCommand.Parameters.AddWithValue("@DOC", DBNull.Value);
                scCommand.Parameters.AddWithValue("@MAIL", DBNull.Value);
                scCommand.Parameters.AddWithValue("@TEL", DBNull.Value);
                scCommand.Parameters.AddWithValue("@DOM", DBNull.Value);
                scCommand.Parameters.AddWithValue("@FECHA_NAC", DBNull.Value);
                scCommand.Parameters.AddWithValue("@ESTADO", DBNull.Value);
                scCommand.Parameters.AddWithValue("@HOTEL_ID", DBNull.Value);
                scCommand.Parameters.AddWithValue("@ERROR", SqlDbType.TinyInt).Direction = ParameterDirection.Output;
                if (scCommand.Connection.State == ConnectionState.Closed)
                {
                    scCommand.Connection.Open();
                }
                scCommand.ExecuteNonQuery();
                int result = int.Parse(scCommand.Parameters["@ERROR"].Value.ToString());
                switch (result)
                {
                    case 1:
                        {
                            MessageBox.Show("Operacion realizada exitosamente", "Operacion realizada", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                con.Close();
            }
        }
    }
}
