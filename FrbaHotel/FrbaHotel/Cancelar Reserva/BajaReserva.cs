using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FrbaHotel.Cancelar_Reserva
{
    public partial class BajaReserva : Form
    {
        Form back = null;
        public BajaReserva()
        {
            InitializeComponent();
        }

        public BajaReserva(Form atras)
        {
            InitializeComponent();
            back = atras;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            back.Show();
        }
    }
}
