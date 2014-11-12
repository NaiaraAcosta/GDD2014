using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FrbaHotel.Generar_Modificar_Reserva
{
    public partial class TipoCliente : Form
    {
        Form back = null;
        Form back2 = null;
        string[] param;
        public TipoCliente()
        {
            InitializeComponent();
        }
        public TipoCliente(Form atras, Form atras2, string[] parametros)
        {
            InitializeComponent();
            back = atras;
            back2 = atras2;
            param = parametros;
        }

        private void TipoCliente_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            back.Show();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Funcionalidad no desarrollada", "Funcionalidad no desarrollada", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //ABM_de_Cliente.AltaCliente f = new ABM_de_Cliente.AltaCliente(this, back, back2, param);
            //f.Show();
            //this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ClienteRegular f = new ClienteRegular(this, back, back2, param);
            f.Show();
            this.Hide();
        }
    }
}
