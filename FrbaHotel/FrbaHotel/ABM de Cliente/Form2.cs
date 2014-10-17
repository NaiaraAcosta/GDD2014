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

namespace FrbaHotel.ABM_de_Cliente
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            string ConnStr = @"Data Source=localhost\SQLSERVER2008;Initial Catalog=GD2C2014;User ID=gd;Password=gd2014;Trusted_Connection=False;";

            SqlConnection conn = new SqlConnection(ConnStr);
            SqlCommand cmd = new SqlCommand("SELECT distinct CLIENTE_ID_TIPO_DOC FROM [GD2C2014].[CONTROL_ZETA].[CLIENTE]", conn);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                comboBox1.Items.Add(reader["CLIENTE_ID_TIPO_DOC"].ToString());
            }
            reader.Close();
            conn.Close(); 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sCnn;
            sCnn = @"data source = Gonzalo-PC\SQLSERVER2008; initial catalog = GD2C2014; user id = gd; password = gd2014";

            string sSel = String.Format("SELECT * FROM [GD2C2014].[CONTROL_ZETA].[CLIENTE] where CLIENTE_DOC = '{0}'", textBox3.Text);

            SqlDataAdapter da;
            DataTable dt = new DataTable();

            try
            {
                da = new SqlDataAdapter(sSel, sCnn);
                da.Fill(dt);

                this.dataGridView1.DataSource = dt;
                //this.dataGridView1.DataBind();
                label6.Text = String.Format("Total datos en la tabla: {0}", dt.Rows.Count);
            }
            catch (Exception ex)
            {
                label6.Text = "Error: " + ex.Message;
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
    }
}
