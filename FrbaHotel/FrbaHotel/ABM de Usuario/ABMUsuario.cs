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
        List<string> usuario = new List<string>();
        public ABMUsuario(Form atras)
        {
            InitializeComponent();
            back = atras;

            refrescar();
        }

        public void refrescar()
        {
            listBox1.Items.Clear();
            usuario.Clear();
            string ConnStr = ConfigurationManager.AppSettings["stringConexion"];
            SqlConnection conn = new SqlConnection(ConnStr);
            string sel = string.Format(@"SELECT distinct usuario.USR_USERNAME from [GD2C2014].[CONTROL_ZETA].[ROL] rol,
                    [GD2C2014].[CONTROL_ZETA].[EMPLEADO] emple,
                    [GD2C2014].[CONTROL_ZETA].[USR_ROL_HOTEL] usrrol,
                    [GD2C2014].[CONTROL_ZETA].[USUARIO] usuario
                    where usrrol.HOTEL_ID = '{0}'
                    and usrrol.USR_USERNAME = emple.USR_USERNAME
                    and usrrol.ROL_ID = rol.ROL_ID
                    and usuario.USR_USERNAME = usrrol.USR_USERNAME", Login.Class1.hotel);
            SqlCommand cmd = new SqlCommand(sel, conn);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                SqlConnection con = new SqlConnection(ConnStr);
                con.Open();
                SqlCommand scCommand = new SqlCommand("CONTROL_ZETA.FN_USUARIO_HABILITADO", con);
                scCommand.CommandType = CommandType.StoredProcedure;
                scCommand.Parameters.Add("@USUARIO", SqlDbType.VarChar, 50).Value = reader[0].ToString();
                scCommand.Parameters.Add("@HOTEL_ID", SqlDbType.Int).Value = Login.Class1.hotel;
                scCommand.Parameters.Add("@RETURN_VALUE", SqlDbType.VarChar, 1).Direction = ParameterDirection.ReturnValue;

                if (scCommand.Connection.State == ConnectionState.Closed)
                {
                    scCommand.Connection.Open();
                }
                scCommand.ExecuteNonQuery();
                string result = scCommand.Parameters["@RETURN_VALUE"].Value.ToString();
                con.Close();

                string detalle = string.Format("{0} - {1}", result, reader[0].ToString());
                listBox1.Items.Add(detalle);
                usuario.Add(reader[0].ToString());
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
                new AltaUsuario(this, usuario[listBox1.SelectedIndex]).Show();
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
                DialogResult result = MessageBox.Show("Esta seguro de deshabilitar el usuario para este hotel?", "Esta seguro?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    string ConnStr = ConfigurationManager.AppSettings["stringConexion"];
                    SqlConnection con = new SqlConnection(ConnStr);
                    con.Open();
                    SqlCommand scCommand = new SqlCommand("CONTROL_ZETA.SP_DES_HAB_USUARIO", con);
                    scCommand.CommandType = CommandType.StoredProcedure;
                    scCommand.Parameters.Add("@USUARIO", SqlDbType.VarChar, 50).Value = usuario[listBox1.SelectedIndex];
                    scCommand.Parameters.Add("@HOTEL_ID", SqlDbType.Int).Value = Login.Class1.hotel;
                    scCommand.Parameters.Add("@HAB", SqlDbType.TinyInt).Value = 0;
                    if (scCommand.Connection.State == ConnectionState.Closed)
                    {
                        scCommand.Connection.Open();
                    }
                    scCommand.ExecuteNonQuery();
                    con.Close();
                }
            }
            refrescar();
        }
    }
}
