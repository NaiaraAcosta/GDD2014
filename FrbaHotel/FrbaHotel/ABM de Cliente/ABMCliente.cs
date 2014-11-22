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
        List<int> tipoDoc = new List<int>();
        public ABMCliente(Form atras)
        {
            InitializeComponent();
            back = atras;
            string ConnStr = ConfigurationManager.AppSettings["stringConexion"];

            SqlConnection conn = new SqlConnection(ConnStr);
            SqlCommand cmd = new SqlCommand("SELECT * FROM [GD2C2014].[CONTROL_ZETA].[TIPO_DOC]", conn);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                comboBox1.Items.Add(reader["TIPO_DOC_DETALLE"].ToString());
                tipoDoc.Add(int.Parse(reader["TIPO_DOC_ID"].ToString()));
            }
            reader.Close();
            conn.Close(); 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sCnn = ConfigurationManager.AppSettings["stringConexion"];
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
                    sSel = String.Format("{0} where CLIENTE_ID_TIPO_DOC = {1}", sSel, tipoDoc[comboBox1.SelectedIndex]);
                    first = false;
                }
                else
                {
                    sSel = String.Format("{0} and CLIENTE_ID_TIPO_DOC = {1}", sSel, tipoDoc[comboBox1.SelectedIndex]);
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
                string[] param = new string[16];
                param[0] = dataGridView1.SelectedCells[0].Value.ToString(); //id
                param[1] = dataGridView1.SelectedCells[1].Value.ToString(); //nombre
                param[2] = dataGridView1.SelectedCells[2].Value.ToString(); //apellido
                param[3] = dataGridView1.SelectedCells[3].Value.ToString(); //tipo doc
                param[4] = dataGridView1.SelectedCells[4].Value.ToString(); //numero
                param[5] = dataGridView1.SelectedCells[5].Value.ToString(); //mail
                param[6] = dataGridView1.SelectedCells[6].Value.ToString(); //telefono
                param[7] = dataGridView1.SelectedCells[7].Value.ToString(); //localidad
                param[8] = dataGridView1.SelectedCells[8].Value.ToString(); //pais
                param[9] = dataGridView1.SelectedCells[9].Value.ToString(); //calle
                param[10] = dataGridView1.SelectedCells[10].Value.ToString(); //nro calle
                param[11] = dataGridView1.SelectedCells[11].Value.ToString(); //depto
                param[12] = dataGridView1.SelectedCells[12].Value.ToString(); //piso
                param[13] = dataGridView1.SelectedCells[13].Value.ToString(); //nacionalidad
                param[14] = dataGridView1.SelectedCells[14].Value.ToString(); //estado
                param[15] = dataGridView1.SelectedCells[15].Value.ToString(); //nacimiento
                Form f = new ABM_de_Cliente.AltaCliente(this, param, 2, false);
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

        private void button5_Click(object sender, EventArgs e)
        {
            new AltaCliente(this, 1).Show();
            this.Hide();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            AllowNumber(e);
        }

        public static void AllowNumber(KeyPressEventArgs e)
        {
            if (char.IsLetter(e.KeyChar) || //Letras
                char.IsSymbol(e.KeyChar) || //Símbolos
                char.IsWhiteSpace(e.KeyChar) || //Espaço
                char.IsPunctuation(e.KeyChar)) //Pontuação
                e.Handled = true; //Não permitir
            //Com o script acima é possível utilizar Números, 'Del', 'BackSpace'..

            //Abaixo só é permito de 0 a 9
            //if ((e.KeyChar < '0') || (e.KeyChar > '9')) e.Handled = true; //Allow only numbers
        }
    }
}
