using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FrbaHotel.ABM_de_Cliente
{
    public partial class Form1 : Form
    {
        Form back = null;
        public Form1(Form atras,string nombre, string apellido, string tipo, string numero, string mail, string telefono, string calle,
            string localidad, string pais, string nacionalidad, string nacimiento)
        {
            InitializeComponent();
            back = atras;
            textBox1.Text = nombre;
            textBox2.Text = apellido;
            textBox3.Text = tipo;
            textBox4.Text = numero;
            textBox5.Text = mail;
            textBox6.Text = telefono;
            textBox7.Text = calle;
            textBox8.Text = localidad;
            textBox9.Text = pais;
            textBox10.Text = nacionalidad;
            textBox11.Text = nacimiento;
        }
        public Form1()
        {
            InitializeComponent();
        }
        public Form1(Form atras)
        {
            InitializeComponent();
            back = atras;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            /*if (back != null)
            {
                back.Show();
            }
            else
            {
                Form f = new ABM_de_Cliente.Form2(this);
                f.Show();
            }*/
            this.Hide();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
