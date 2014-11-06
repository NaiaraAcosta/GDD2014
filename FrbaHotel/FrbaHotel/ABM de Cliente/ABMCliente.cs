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
    public partial class ABMCliente : Form
    {
        Form back = null;
        public ABMCliente(Form atras)
        {
            InitializeComponent();
            back = atras;
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
            string sCnn = @"data source = localhost\SQLSERVER2008; initial catalog = GD2C2014; user id = gd; password = gd2014";
            string sSel = String.Format("SELECT * FROM [GD2C2014].[CONTROL_ZETA].[CLIENTE]");
            bool first = true;

            if (textBox1.Text != "")
            {
                if (first)
                {
                    sSel = String.Format("{0} where CLIENTE_NOMBRE like '{1}%'", sSel, textBox1.Text);
                    first = false;
                }
                else
                {
                    sSel = String.Format("{0} and CLIENTE_NOMBRE like '{1}%'", sSel, textBox1.Text);
                }
            }
            
            if (textBox2.Text != "")
            {
                if (first)
                {
                    sSel = String.Format("{0} where CLIENTE_APELLIDO like '{1}%'", sSel, textBox2.Text);
                    first = false;
                }
                else
                {
                    sSel = String.Format("{0} and CLIENTE_APELLIDO like '{1}%'", sSel, textBox2.Text);
                }
            }

            if (textBox3.Text != "")
            {
                if (first)
                {
                    sSel = String.Format("{0} where CLIENTE_DOC = '{1}'", sSel, textBox3.Text);
                    first = false;
                }
                else
                {
                    sSel = String.Format("{0} and CLIENTE_DOC = '{1}'", sSel, textBox3.Text);
                }
            }

            if (textBox4.Text != "")
            {
                if (first)
                {
                    sSel = String.Format("{0} where CLIENTE_MAIL like '{1}%'", sSel, textBox4.Text);
                    first = false;
                }
                else
                {
                    sSel = String.Format("{0} and CLIENTE_MAIL like '{1}%'", sSel, textBox4.Text);
                }
            }

            if (comboBox1.Text != "")
            {
                if (first)
                {
                    sSel = String.Format("{0} where CLIENTE_ID_TIPO_DOC = {1}", sSel, comboBox1.Text);
                    first = false;
                }
                else
                {
                    sSel = String.Format("{0} and CLIENTE_ID_TIPO_DOC = {1}", sSel, comboBox1.Text);
                }
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

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (back != null)
            {
                back.Show();
            }
            else
            {
                Form f = new FrbaHotel.MenuPrincipal();
                f.Show();
            }
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            if (dataGridView1.SelectedCells.Count != 0)
            {
                //string[] result = dataGridView1.SelectedCells[1].Value.ToString().Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
                Form f = new ABM_de_Cliente.AltaCliente(this,
                    dataGridView1.SelectedCells[1].Value.ToString(), //nombre
                    dataGridView1.SelectedCells[2].Value.ToString(), //apellido
                    dataGridView1.SelectedCells[3].Value.ToString(), //tipo doc
                    dataGridView1.SelectedCells[4].Value.ToString(), //numero
                    dataGridView1.SelectedCells[5].Value.ToString(), //mail
                    dataGridView1.SelectedCells[6].Value.ToString(), //telefono
                    dataGridView1.SelectedCells[9].Value.ToString(), //calle
                    dataGridView1.SelectedCells[7].Value.ToString(), //localidad
                    dataGridView1.SelectedCells[8].Value.ToString(), //pais
                    dataGridView1.SelectedCells[13].Value.ToString(), //nacionalidad
                    dataGridView1.SelectedCells[15].Value.ToString()); //nacimiento
                f.Show();
            }
            else
            {
                MessageBox.Show("No hay datos que modificar", "No se puede modificar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
        }
    }
}
