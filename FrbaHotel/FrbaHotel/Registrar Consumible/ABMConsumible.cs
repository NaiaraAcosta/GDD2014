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

namespace FrbaHotel.Registrar_Consumible
{
    public partial class ABMConsumible : Form
    {
        Form back = null;
        public ABMConsumible(Form atras)
        {
            InitializeComponent();
            back = atras;
        }

        private void ABMConsumible_Load(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
            back.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            refrescar(textBox1.Text, textBox2.Text, textBox3.Text);
        }

        private void refrescar(string habID, string estadiaID, string clienteID)
        {
            string sCnn = @"data source = localhost\SQLSERVER2008; initial catalog = GD2C2014; user id = gd; password = gd2014";
            string sSel = String.Format(@"SELECT CLIENTE_ID, HAB_ID, esta.EST_ID, reserhab.RESERVA_ID, EST_FECHA_DESDE 
                FROM [GD2C2014].[CONTROL_ZETA].[ESTADIA_CLIENTE] estaclie,
                [GD2C2014].[CONTROL_ZETA].[ESTADIA] esta,
		        [GD2C2014].[CONTROL_ZETA].[RESERVA_HABITACION] reserhab
                where esta.EST_FECHA_HASTA is null
                and esta.EST_ID = estaclie.EST_ID
                and reserhab.RESERVA_ID = esta.EST_RESERVA_ID");

            if (habID != "")
            {
                sSel = String.Format("{0} and reserhab.HAB_ID = {1}", sSel, habID);
            }
            if (estadiaID != "")
            {
                sSel = String.Format("{0} and estaclie.EST_ID = {1}", sSel, estadiaID);
            }
            if (clienteID != "")
            {
                sSel = String.Format("{0} and estaclie.CLIENTE_ID = {1}", sSel, clienteID);
            }
            SqlDataAdapter da;
            DataTable dt = new DataTable();
            try
            {
                da = new SqlDataAdapter(sSel, sCnn);
                da.Fill(dt);
                this.dataGridView1.DataSource = dt;
                label4.Text = String.Format("Total datos en la tabla: {0}", dt.Rows.Count);
            }
            catch (Exception ex)
            {
                label4.Text = "Error: " + ex.Message;
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
        
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string[] param = new string[2];
            param[0] = dataGridView1.SelectedCells[2].Value.ToString();
            param[1] = dataGridView1.SelectedCells[1].Value.ToString();
            AltaConsumible f = new AltaConsumible(this, param);
            f.Show();
            this.Hide();
        }

        
    }
}
