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
    public partial class ABMHabitacion : Form
    {
        string hotel = "1";
        Form back = null;
        public ABMHabitacion()
        {
            InitializeComponent();
        }
        public ABMHabitacion(string idHotel)
        {
            InitializeComponent();
            hotel = idHotel;
        }
        public ABMHabitacion(Form atras)
        {
            InitializeComponent();
            back = atras;
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            string sCnn;
            sCnn = ConfigurationManager.AppSettings["stringConexion"];

            string sSel = String.Format("SELECT * FROM [GD2C2014].[CONTROL_ZETA].[HABITACION] where HAB_ID_HOTEL = '{0}'", Login.Class1.hotel);
            SqlDataAdapter da;
            DataTable dt = new DataTable();
            try
            {
                da = new SqlDataAdapter(sSel, sCnn);
                da.Fill(dt);
                this.dataGridView1.DataSource = dt;
                label1.Text = String.Format("Total datos en la tabla: {0}", dt.Rows.Count);
            }
            catch (Exception ex)
            {
                label1.Text = "Error: " + ex.Message;
            }


            string ConnStr = ConfigurationManager.AppSettings["stringConexion"];
            SqlConnection conn = new SqlConnection(ConnStr);
            sSel = string.Format(@"SELECT * FROM [GD2C2014].[CONTROL_ZETA].[HOTEL] hotel, 
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
                textBox1.Text = detalle;
            }
            reader.Close();
            conn.Close(); 
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count != 0)
            {
                Form f = new ABM_de_Habitacion.AltaHabitacion(this,
                    dataGridView1.SelectedCells[0].Value.ToString(), //id
                    dataGridView1.SelectedCells[1].Value.ToString(), //num
                    dataGridView1.SelectedCells[3].Value.ToString(), //piso
                    dataGridView1.SelectedCells[5].Value.ToString(), //ubi
                    dataGridView1.SelectedCells[4].Value.ToString(), //tipo
                    dataGridView1.SelectedCells[6].Value.ToString());//comodidades
                f.Show();
            }
            else
            {
                MessageBox.Show("No hay datos que modificar", "No se puede modificar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button4_Click(object sender, EventArgs e)
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
    }
}
