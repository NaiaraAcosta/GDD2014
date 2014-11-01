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

namespace FrbaHotel.ABM_de_Hotel
{
    public partial class ABMHotel : Form
    {
        Form back = null;
        public ABMHotel()
        {
            InitializeComponent();
        }
        public ABMHotel(Form atras)
        {
            InitializeComponent();
            back = atras;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sCnn;
            sCnn = @"data source = Gonzalo-PC\SQLSERVER2008; initial catalog = GD2C2014; user id = gd; password = gd2014";

            string sSel = String.Format("SELECT * FROM [GD2C2014].[CONTROL_ZETA].[HOTEL] where 1=1");
            if (textBox1.Text != "")
            {
                sSel = String.Format("{0} and HOTEL_NOMBRE like '{1}%'", sSel, textBox1.Text); 
            }
            if (textBox2.Text != "")
            {
                sSel = String.Format("{0} and HOTEL_CALLE like '{1}%'", sSel, textBox2.Text);
            }
            if (textBox3.Text != "")
            {
                sSel = String.Format("{0} and HOTEL_CANT_ESTRELLA = '{1}'", sSel, textBox3.Text);
            }
            if (textBox4.Text != "")
            {
                sSel = String.Format("{0} and HOTEL_ID_LOC = '{1}'", sSel, textBox4.Text);
            }
            if (textBox5.Text != "")
            {
                sSel = String.Format("{0} and HOTEL_PAIS = '{1}'", sSel, textBox5.Text);
            }
            SqlDataAdapter da;
            DataTable dt = new DataTable();
            try
            {
                da = new SqlDataAdapter(sSel, sCnn);
                da.Fill(dt);
                this.dataGridView1.DataSource = dt;
                label6.Text = String.Format("Total datos en la tabla: {0}", dt.Rows.Count);
            }
            catch (Exception ex)
            {
                label6.Text = "Error: " + ex.Message;
            }
        }

        private void button5_Click(object sender, EventArgs e)
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

        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count != 0)
            {
                string direccion = string.Format("{0} {1}", dataGridView1.SelectedCells[4].Value.ToString(),
                    dataGridView1.SelectedCells[5].Value.ToString());
                Form f = new AltaHotel(this,
                    dataGridView1.SelectedCells[1].Value.ToString(), //nombre
                    dataGridView1.SelectedCells[2].Value.ToString(), //mail
                    dataGridView1.SelectedCells[3].Value.ToString(), //telefono
                    direccion, //direccion
                    dataGridView1.SelectedCells[7].Value.ToString(), //cant estrellas
                    dataGridView1.SelectedCells[9].Value.ToString(), //recarga estrellas
                    dataGridView1.SelectedCells[6].Value.ToString(), //id ciudad
                    dataGridView1.SelectedCells[8].Value.ToString(), //id pais
                    dataGridView1.SelectedCells[10].Value.ToString()); //fecha creacion
                f.Show();
            }
            else
            {
                MessageBox.Show("No hay datos que modificar", "No se puede modificar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new AltaHotel(this).Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count != 0)
            {
                new BajaHotel(this,
                    dataGridView1.SelectedCells[1].Value.ToString()).Show(); //hotelID
                this.Hide();
            }
            else
            {
                MessageBox.Show("No hay datos que modificar", "No se puede modificar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
