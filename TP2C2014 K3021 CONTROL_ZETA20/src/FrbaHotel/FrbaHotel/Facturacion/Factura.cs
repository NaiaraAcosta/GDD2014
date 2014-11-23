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

namespace FrbaHotel.Facturacion
{
    public partial class Factura : Form
    {
        Form back = null;
        int factura = 0;
        public Factura(Form atras, int nroFactura)
        {
            InitializeComponent();
            back = atras;
            factura = nroFactura;
        }

        private void Factura_Load(object sender, EventArgs e)
        {
            string ConnStr2 = ConfigurationManager.AppSettings["stringConexion"];
            string sSel2 = string.Format(@"SELECT ITEM_DESCRIPCION as Descripcion, ITEM_FACTURA_CANTIDAD as Cantidad, ITEM_FACTURA_MONTO as Monto
                        FROM [GD2C2014].[CONTROL_ZETA].[FACTURA] fac,
                        [GD2C2014].[CONTROL_ZETA].[ITEM_FACTURA] item            
                        where fac.FACTURA_NRO = {0}
                        and fac.FACTURA_NRO = item.FACTURA_NRO
						order by ITEM_FACTURA_MONTO desc", factura);
            SqlDataAdapter da;
            DataTable dt = new DataTable();
            try
            {
                da = new SqlDataAdapter(sSel2, ConnStr2);
                da.Fill(dt);
                this.dataGridView1.DataSource = dt;
                label10.Text = String.Format("Total datos en la tabla: {0}", dt.Rows.Count);
            }
            catch (Exception ex)
            {
                label10.Text = "Error: " + ex.Message;
            }

            string ConnStr = ConfigurationManager.AppSettings["stringConexion"];
            SqlConnection conn = new SqlConnection(ConnStr);
            string sel = string.Format(@"SELECT * from [GD2C2014].[CONTROL_ZETA].[FACTURA] fac
                    where fac.FACTURA_NRO = '{0}'", factura);
            SqlCommand cmd = new SqlCommand(sel, conn);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                textBox1.Text = string.Format("{0}", factura);
                textBox2.Text = reader[1].ToString();
                textBox3.Text = reader[2].ToString();
                textBox4.Text = reader[3].ToString();
                textBox5.Text = reader[4].ToString();
                textBox6.Text = reader[6].ToString();
                textBox7.Text = reader[9].ToString();
                textBox8.Text = reader[5].ToString();
            }
            reader.Close();
            conn.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            back.Show();
            this.Close();
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
