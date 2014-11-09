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

namespace FrbaHotel.Login
{
    public partial class SeleccionRol : Form
    {
        string username;
        Form back;
        Form menu;
        int[] idHotel;
        bool[] func;
        public SeleccionRol()
        {
            InitializeComponent();
        }

        public SeleccionRol(Form atras, FrbaHotel.MenuPrincipal menuPrin, string user)
        {
            InitializeComponent();
            username = user;
            back = atras;
            menu = menuPrin;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string ConnStr = ConfigurationManager.AppSettings["stringConexion"];
            SqlConnection conn = new SqlConnection(ConnStr);
            string sSel = string.Format(@"SELECT * FROM [GD2C2014].[CONTROL_ZETA].[USR_ROL_HOTEL] usr,
                [GD2C2014].[CONTROL_ZETA].[HOTEL] hotel, [GD2C2014].[CONTROL_ZETA].[ROL] rol, 
                [GD2C2014].[CONTROL_ZETA].[LOCALIDAD] loc 
                where usr.USR_USERNAME = '{0}' and usr.HOTEL_ID = hotel.HOTEL_ID 
                and usr.ROL_ID = rol.ROL_ID and hotel.HOTEL_ID_LOC = loc.LOC_ID", username);
            SqlCommand cmd = new SqlCommand(sSel, conn);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            string detalle = "";
            int i = 0;
            while (reader.Read())
            {
                i++;
            }
            reader.Close();
            reader = cmd.ExecuteReader();
            idHotel = new int[i];
            i = 0;
            while (reader.Read())
            {
                detalle = string.Format("{0} - {1} - {2} {3}",
                    reader["ROL_NOMBRE"].ToString(),
                    reader["LOC_DETALLE"].ToString().Trim(),
                    reader["HOTEL_CALLE"].ToString(),
                    reader["HOTEL_NRO_CALLE"].ToString());
                idHotel[i] = int.Parse(reader["HOTEL_ID"].ToString());
                i++;
                listBox1.Items.Add(detalle);
            }
            reader.Close();
            conn.Close(); 
        }

        private void button2_Click(object sender, EventArgs e)
        {
            back.Show();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            logicaModo();
            Form f = new FrbaHotel.MenuPrincipal(func, menu);
            f.Show();
            this.Close();
            menu.Hide();
            //menu.Dispose();
        }
        private void logicaModo()
        {
            if (listBox1.SelectedItem != null)
            {
                string seleccion = listBox1.SelectedItem.ToString();
                //Login.Class1.mode = 1;

                string[] stringSeparators = new string[] { "-" };
                string[] result = seleccion.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);

                string ConnStr = ConfigurationManager.AppSettings["stringConexion"];
                SqlConnection conn = new SqlConnection(ConnStr);
                string sSel = string.Format(@"SELECT * FROM [GD2C2014].[CONTROL_ZETA].[USR_ROL_HOTEL] usr,
                [GD2C2014].[CONTROL_ZETA].[ROL] rol, 
                [GD2C2014].[CONTROL_ZETA].[FUNCIONALIDAD] fun,
                [GD2C2014].[CONTROL_ZETA].[ROL_FUNC] rolfun
                where usr.USR_USERNAME = '{0}' 
                and usr.HOTEL_ID = '{1}'
                and rol.ROL_NOMBRE = '{2}' 
                and usr.ROL_ID = rol.ROL_ID
                and usr.ROL_ID = rolfun.ROL_ID
                and rolfun.FUNC_ID = fun.FUNC_ID",
                                                     username,
                                                     idHotel[listBox1.SelectedIndex],
                                                     result[0]);
                SqlCommand cmd = new SqlCommand(sSel, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                int i = 0;
                while (reader.Read())
                {
                    i++;
                }
                reader.Close();
                reader = cmd.ExecuteReader();
                func = new bool[i];
                while (reader.Read())
                {
                    func[int.Parse(reader["FUNC_ID"].ToString()) - 1] = true;
                }
                reader.Close();
                conn.Close();
                Login.Class1.hotel = idHotel[listBox1.SelectedIndex];
                Login.Class1.user = username;
            }
            else
            {
                func = new bool[13];
                Login.Class1.hotel = 0;
            }
        }
    }
}
