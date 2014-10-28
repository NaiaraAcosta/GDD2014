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
    }
}
