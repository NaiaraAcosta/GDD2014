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

namespace FrbaHotel.Generar_Modificar_Reserva
{
    public partial class ClienteRegular : Form
    {
        Form back = null;
        Form back2 = null;
        Form back3 = null;
        List<int> idDoc = new List<int>();
        string[] param;
        public ClienteRegular()
        {
            InitializeComponent();
        }
        public ClienteRegular(Form atras, Form atras2, Form atras3, string[] parametros)
        {
            InitializeComponent();
            back = atras;
            back2 = atras2;
            back3 = atras3;
            param = parametros;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            string ConnStr = ConfigurationManager.AppSettings["stringConexion"];
            SqlConnection conn = new SqlConnection(ConnStr);
            string sSel = string.Format(@"SELECT * FROM [GD2C2014].[CONTROL_ZETA].[TIPO_DOC]");
            SqlCommand cmd = new SqlCommand(sSel, conn);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            string detalle = "";
            while (reader.Read())
            {
                detalle = string.Format("{0}",
                    reader["TIPO_DOC_DETALLE"].ToString().Trim());
                idDoc.Add(int.Parse(reader["TIPO_DOC_ID"].ToString()));
                comboBox1.Items.Add(detalle);
            }
            comboBox1.SelectedIndex = 0;
            reader.Close();
            conn.Close();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            back.Show();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sCnn = ConfigurationManager.AppSettings["stringConexion"];
            string sSel = String.Format("SELECT * FROM [GD2C2014].[CONTROL_ZETA].[CLIENTE] where 1=1");
            if (comboBox1.Text != "")
            {
                sSel = String.Format("{0} and CLIENTE_ID_TIPO_DOC = '{1}'", sSel, idDoc[comboBox1.SelectedIndex] );
            }
            if (textBox3.Text != "")
            {
                sSel = String.Format("{0} and CLIENTE_DOC = '{1}'", sSel, textBox3.Text);
            }
            if (textBox4.Text != "")
            {
                sSel = String.Format("{0} and CLIENTE_MAIL like '{1}%'", sSel, textBox4.Text);
            }
            SqlDataAdapter da;
            DataTable dt = new DataTable();
            try
            {
                da = new SqlDataAdapter(sSel, sCnn);
                da.Fill(dt);
                this.dataGridView1.DataSource = dt;
                groupBox1.Text = String.Format("Total de resultados: {0}", dt.Rows.Count);
            }
            catch (Exception ex)
            {
                groupBox1.Text = "Error: " + ex.Message;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            param[4] = dataGridView1.SelectedCells[0].Value.ToString();
            ReservaFinalizada f = new ReservaFinalizada(this, back, back2, back3, param);
            f.Show();
            this.Hide();
        }
    }
}
