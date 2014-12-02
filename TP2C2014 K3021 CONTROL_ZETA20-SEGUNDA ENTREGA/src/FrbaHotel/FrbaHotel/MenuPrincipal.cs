using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;
using System.Security.Cryptography;  


namespace FrbaHotel
{
    public partial class MenuPrincipal : Form
    {
        private SqlDataAdapter dataAdapter = new SqlDataAdapter();
        FrbaHotel.MenuPrincipal principal = null;
        public MenuPrincipal()
        {
            InitializeComponent();
            if (Login.Class1.mode == 1)//modo usuario
            { 
                this.qweToolStripMenuItem1.Visible = false;
                this.estadíaToolStripMenuItem.Visible = false;
                this.estadísticasToolStripMenuItem.Visible = false;
            }
        }

        public MenuPrincipal(bool[] func, Form menu)
        {
            InitializeComponent();
            principal = (FrbaHotel.MenuPrincipal) menu;
            deshabilitarFunciones(func);
        }

        public MenuPrincipal(bool[] func)
        {
            InitializeComponent();
            deshabilitarFunciones(func);
        }

        private void deshabilitarFunciones(bool[] func)
        {
            if (!func[0])//abm de rol
            {
                this.aBMDeRolToolStripMenuItem.Visible = false;
            }
            if (!func[1])//login y seguridad
            {
                this.asdToolStripMenuItem3.Visible = false;
                this.button1.Visible = false;
            }
            if (!func[2])//abm de usuario
            {
                this.aBMDeUsuarioToolStripMenuItem.Visible = false;
            }
            if (!func[3])//abm de clientes
            {
                this.altaDeClienteToolStripMenuItem.Visible = false;
            }
            if (!func[4])//abm de hotel
            {
                this.aBMDeHotelToolStripMenuItem.Visible = false;
            }
            if (!func[5])//abm de habitacion
            {
                this.modificacionBajaDeClienteToolStripMenuItem.Visible = false;
            }
            if (!func[6])//abm de regimen de estadias
            {
                //nothing to do here...
            }
            if (!func[7])//generar o modificar reserva
            {
                this.generarToolStripMenuItem.Visible = false;
                this.modificarToolStripMenuItem.Visible = false;
            }
            if (!func[8])//cancelar reserva
            {
                this.cancelarToolStripMenuItem.Visible = false;
            }
            if (!func[9])//registrar estadia
            {
                this.registarEstadiaToolStripMenuItem.Visible = false;
            }
            if (!func[10])//registrar consumibles
            {
                this.registrarConsumibleToolStripMenuItem.Visible = false;
            }
            if (!func[11])//facturar estadia
            {
                this.facturarToolStripMenuItem.Visible = false;
            }
            if (!func[12])//listado estadistico
            {
                this.estadísticasToolStripMenuItem.Visible = false;
            }
            if (!func[13])//reservasInconsistentes
            {
                this.reservasInconsistentesToolStripMenuItem.Visible = false;
            }
            if (!func[0] && !func[2] && !func[3] && !func[4] && !func[5])
            {
                this.qweToolStripMenuItem1.Visible = false;
            }
            if (!func[7] && !func[8] && !func[13])
            {
                this.asdToolStripMenuItem2.Visible = false;
            }
            if (!func[9] && !func[10])
            {
                this.estadíaToolStripMenuItem.Visible = false;
            }
            if (func.All(x => x))
            {
                button2.Visible = true;
            }
            else
            {
                button2.Visible = false;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form f = new Login.Ingresar(this);
            f.Show();
        }


        private void estadísticasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new Listado_Estadistico.Estadisticas(this);
            f.Show();
            this.Hide();
        }

        private void gfdToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void modificacionBajaDeClienteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("La siguiente funcionalidad no esta desarrollada en su totalidad", "Funcionalidad sin terminar", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Form f = new ABM_de_Habitacion.ABMHabitacion(this);
            f.Show();
            this.Hide();
        }

        private void aBMDeHotelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new ABM_de_Hotel.ABMHotel(this);
            f.Show();
            this.Hide();
        }

        private void aBMDeRolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("La siguiente funcionalidad no esta desarrollada en su totalidad", "Funcionalidad sin terminar", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Form f = new ABM_de_Rol.ABMRol(this);
            f.Show();
            this.Hide();
        }

        private void aBMDeUsuarioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("La siguiente funcionalidad no esta desarrollada en su totalidad", "Funcionalidad sin terminar", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Form f = new ABM_de_Usuario.ABMUsuario(this);
            f.Show();
            this.Hide();
        }

        private void generarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new Generar_Modificar_Reserva.AltaReserva(this);
            f.Show();
            this.Hide();
        }

        private void cancelarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new Cancelar_Reserva.BajaReserva(this);
            f.Show();
            this.Hide();
        }

        private void registarEstadiaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new Registrar_Estadia.RegistrarEstadia(this);
            f.Show();
            this.Hide();
        }

        private void registrarConsumibleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new Registrar_Consumible.ABMConsumible(this);
            f.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (principal != null)
            {
                principal.cerrar();
            }
            this.Close();
        }

        public void cerrar()
        {
            if (principal != null)
            {
                principal.cerrar();
            }
            this.Close();
        }

        private void altaDeClienteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("La siguiente funcionalidad no esta desarrollada en su totalidad", "Funcionalidad sin terminar", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Form f = new ABM_de_Cliente.ABMCliente(this);
            f.Show();
            this.Hide();
        }

        private void modificarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new Generar_Modificar_Reserva.ModificarReserva(this);
            f.Show();
            this.Hide();
        }

        private void facturarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new Facturacion.Facturar(this);
            f.Show();
            this.Hide();
        }

        private void reservasInconsistentesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Reservas_Inconsistentes.ReservasInconsistentes(this).Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new desarrollador(this).Show();
            this.Hide();
        }

        private void asdToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            Form f = new Login.Ingresar(this);
            f.Show();
        }
    }
}
