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

            string sSel = String.Format("SELECT * FROM [GD2C2014].[CONTROL_ZETA].[CLIENTE]");
            bool first = true;

            if (textBox1.Text != "")
            {
                if (first)
                {
                    sSel = String.Format("{0} where CLIENTE_NOMBRE like '%, {1}%'", sSel, textBox1.Text);
                    first = false;
                }
                else
                {
                    sSel = String.Format("{0} and CLIENTE_NOMBRE like '%, {1}%'", sSel, textBox1.Text);
                }
            }
            
            if (textBox2.Text != "")
            {
                if (first)
                {
                    sSel = String.Format("{0} where CLIENTE_NOMBRE like '{1}%, %'", sSel, textBox2.Text);
                    first = false;
                }
                else
                {
                    sSel = String.Format("{0} and CLIENTE_NOMBRE like '{1}%, %'", sSel, textBox2.Text);
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

        private void button4_Click(object sender, EventArgs e)
        {
            Form f = new FrbaHotel.Form1();
            f.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string[] stringSeparators = new string[] { ", " };
            if (dataGridView1.SelectedCells.Count != 0)
            {
                string[] result = dataGridView1.SelectedCells[1].Value.ToString().Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
                Form f = new ABM_de_Cliente.Form1(result[1], result[0],
                    dataGridView1.SelectedCells[2].Value.ToString(),
                    dataGridView1.SelectedCells[3].Value.ToString(),
                    dataGridView1.SelectedCells[4].Value.ToString(),
                    dataGridView1.SelectedCells[5].Value.ToString(),
                    dataGridView1.SelectedCells[8].Value.ToString(),
                    dataGridView1.SelectedCells[6].Value.ToString(),
                    dataGridView1.SelectedCells[7].Value.ToString(),
                    dataGridView1.SelectedCells[12].Value.ToString(),
                    dataGridView1.SelectedCells[14].Value.ToString());
                f.Show();
            }
            else
            {
                Form f = new ABM_de_Cliente.Form1();
                f.Show();
            }
            
            //this.Hide();
            
        }
    }
}
