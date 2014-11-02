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
            sCnn = @"data source = Gonzalo-PC\SQLSERVER2008; initial catalog = GD2C2014; user id = gd; password = gd2014";

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
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count != 0)
            {
                Form f = new ABM_de_Habitacion.AltaHabitacion(this,
                    int.Parse(dataGridView1.SelectedCells[1].Value.ToString()),
                    int.Parse(dataGridView1.SelectedCells[3].Value.ToString()),
                    char.Parse(dataGridView1.SelectedCells[4].Value.ToString()),
                    int.Parse(dataGridView1.SelectedCells[5].Value.ToString()),
                    "");
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
