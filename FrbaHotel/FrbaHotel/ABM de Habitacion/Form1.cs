using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FrbaHotel.ABM_de_Habitacion
{
    public partial class Form1 : Form
    {
        Form back = null;
        public Form1()
        {
            InitializeComponent();
        }

        public Form1(Form atras)
        {
            InitializeComponent();
            back = atras;
        }

        public Form1(Form atras, int num, int piso, char ubi, int tipo, string comodidades)
        {
            InitializeComponent();
            back = atras;
            textBox1.Text = string.Format("{0}",num);
            textBox2.Text = string.Format("{0}", piso);
            textBox3.Text = string.Format("{0}", ubi);
            textBox4.Text = string.Format("{0}", tipo);
            richTextBox1.Text = string.Format("{0}", comodidades);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (back != null)
            {
                back.Show();
            }
            else
            {
                new Form2().Show();
            }
            this.Hide();
        }
    }
}
