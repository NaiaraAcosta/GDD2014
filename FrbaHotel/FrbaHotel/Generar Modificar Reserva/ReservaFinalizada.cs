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
    public partial class ReservaFinalizada : Form
    {
        Form back = null;
        Form back2 = null;
        Form back3 = null;
        Form back4 = null;
        int idCliente = 0;
        public ReservaFinalizada()
        {
            InitializeComponent();
        }
        public ReservaFinalizada(Form atras, Form atras2, Form atras3, Form atras4, int idClien)
        {
            InitializeComponent();
            back = atras;
            back2 = atras2;
            back3 = atras3;
            back4 = atras4;
            idCliente = idClien;
        }

        private void Form4_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            back.Close();
            back2.Close();
            back3.Close();
            back4.Show();
        }
    }
}
