﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FrbaHotel.Login
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (logicaLogueo(textBox1.Text, textBox2.Text))
            {
                Form f = new Login.Form1(textBox1.Text);
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
        }
    }
}
