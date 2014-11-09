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
        string estID = "";
        public Factura(Form atras, string estadia)
        {
            InitializeComponent();
            back = atras;
            estID = estadia;
        }

        private void Factura_Load(object sender, EventArgs e)
        {
            string ConnStr2 = ConfigurationManager.AppSettings["stringConexion"];
            string sSel2 = string.Format(@"SELECT fac.FACTURA_NRO as NroFactura, ITEM_DESCRIPCION as Descripcion, ITEM_FACTURA_CANTIDAD as Cantidad, ITEM_FACTURA_MONTO as Monto, fac.FACTURA_TOTAL as Total
                        FROM [GD2C2014].[CONTROL_ZETA].[FACTURA] fac,
                        [GD2C2014].[CONTROL_ZETA].[ITEM_FACTURA] item            
                        where fac.EST_ID = {0}
                        and fac.FACTURA_NRO = item.FACTURA_NRO
						order by ITEM_FACTURA_MONTO desc", estID);
            SqlDataAdapter da;
            DataTable dt = new DataTable();
            try
            {
                da = new SqlDataAdapter(sSel2, ConnStr2);
                da.Fill(dt);
                this.dataGridView1.DataSource = dt;
                label1.Text = String.Format("Total datos en la tabla: {0}", dt.Rows.Count);
            }
            catch (Exception ex)
            {
                label1.Text = "Error: " + ex.Message;
            }
        }
    }
}
