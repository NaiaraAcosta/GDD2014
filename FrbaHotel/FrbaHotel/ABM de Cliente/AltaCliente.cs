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
    public partial class AltaCliente : Form
    {
        Form back = null;
        public AltaCliente(Form atras,string nombre, string apellido, string tipo, string numero, string mail, string telefono, string calle,
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
            string[] stringSeparators = new string[] { "/" };
            string[] result = nacimiento.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
            result[2] = result[2].Substring(0, 4);
            dateTimePicker1.Value = new DateTime(int.Parse(result[2]), int.Parse(result[1]), int.Parse(result[0]));
            
        }
        public AltaCliente()
        {
            InitializeComponent();
        }
        public AltaCliente(Form atras)
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
            this.Close(); ;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            validarDatos();
        }
        private bool validarDatos()
        {
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox5.Text == "")
            {
                MessageBox.Show("Existen datos cuyos valores no pueden dejarse vacios","Error de ingreso de datos",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            return true;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.TextLength > 30)
            {
                textBox1.Text = textBox1.Text.Substring(0, 30);
                textBox1.SelectionStart = 30;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.TextLength > 30)
            {
                textBox2.Text = textBox2.Text.Substring(0, 30);
                textBox2.SelectionStart = 30;
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            int salida;
            bool valido = int.TryParse(textBox3.Text, out salida);
            if (valido && salida > 255)
            {
                textBox3.Text = "255";
                textBox3.SelectionStart = 3;
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            AllowNumber(e);
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (textBox4.TextLength > 15)
            {
                textBox4.Text = textBox4.Text.Substring(0, 15);
                textBox4.SelectionStart = 15;
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            if (textBox5.TextLength > 50)
            {
                textBox5.Text = textBox5.Text.Substring(0, 50);
                textBox5.SelectionStart = 50;
            }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            if (textBox6.TextLength > 10)
            {
                textBox6.Text = textBox6.Text.Substring(0, 10);
                textBox6.SelectionStart = 10;
            }
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            if (textBox7.TextLength > 70)
            {
                textBox7.Text = textBox7.Text.Substring(0, 17);
                textBox7.SelectionStart = 70;
            }
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            int salida;
            bool valido = int.TryParse(textBox8.Text, out salida);
            if (valido && salida > 255)
            {
                textBox8.Text = "255";
                textBox8.SelectionStart = 3;
            }
        }

        private void textBox8_KeyPress(object sender, KeyPressEventArgs e)
        {
            AllowNumber(e);
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            int salida;
            bool valido = int.TryParse(textBox9.Text, out salida);
            if (valido && salida > 255)
            {
                textBox9.Text = "255";
                textBox9.SelectionStart = 3;
            }
        }

        private void textBox9_KeyPress(object sender, KeyPressEventArgs e)
        {
            AllowNumber(e);
        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {
            int salida;
            bool valido = int.TryParse(textBox10.Text, out salida);
            if (valido && salida > 255)
            {
                textBox10.Text = "255";
                textBox10.SelectionStart = 3;
            }
        }
        private void textBox10_KeyPress(object sender, KeyPressEventArgs e)
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
