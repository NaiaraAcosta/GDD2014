using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FrbaHotel.ABM_de_Hotel
{
    public partial class AltaHotel : Form
    {
        Form back = null;
        public AltaHotel()
        {
            InitializeComponent();
        }

        public AltaHotel(Form atras)
        {
            InitializeComponent();
            back = atras;
        }

        public AltaHotel(Form atras,
            string nombre,
            string mail,
            string telefono,
            string direccion,
            string cantEstrellas,
            string recargaEstrellas,
            string idCiudad,
            string idPais,
            string fechaCreacion)
        {
            InitializeComponent();
            back = atras;
            textBox1.Text = nombre;
            textBox2.Text = mail;
            textBox3.Text = telefono;
            textBox4.Text = direccion;
            textBox5.Text = cantEstrellas;
            textBox6.Text = recargaEstrellas;
            textBox7.Text = idCiudad;
            textBox8.Text = idPais;
            string[] stringSeparators = new string[] { "/" };
            string[] result = fechaCreacion.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
            result[2] = result[2].Substring(0, 4);
            dateTimePicker1.Value = new DateTime(int.Parse(result[2]), int.Parse(result[1]), int.Parse(result[0]));
        }

       

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
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
                new FrbaHotel.MenuPrincipal().Show();
            }
            this.Hide();
        }
    }
}
