using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FrbaHotel.Login
{
    public partial class Ingresar : Form
    {
        FrbaHotel.MenuPrincipal back;
        public Ingresar(FrbaHotel.MenuPrincipal atras)
        {
            InitializeComponent();
            back = atras;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (logicaLogueo(textBox1.Text, textBox2.Text))
            {
                Form f = new Login.SeleccionRol(this, back, textBox1.Text);
                f.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Error de logueo, intente nuevamente","Error de logueo",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
        private Boolean logicaLogueo(string user, string pass)
        {
            return true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            back.Show();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
    }
}
