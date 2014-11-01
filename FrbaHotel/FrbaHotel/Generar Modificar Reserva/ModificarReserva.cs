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
    public partial class ModificarReserva : Form
    {
        Form back = null;
        public ModificarReserva()
        {
            InitializeComponent();
        }

        public ModificarReserva(Form atras)
        {
            InitializeComponent();
            back = atras;
        }

        private void ModificarReserva_Load(object sender, EventArgs e)
        {

        }
    }
}
