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
            string sel = string.Format(@"SELECT * FROM [GD2C2014].[CONTROL_ZETA].[ROL] rol,
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
                listBox1.Items.Add(reader[3].ToString());
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

        }
    }
}
