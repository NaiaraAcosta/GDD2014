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
                //todavia sin hacer
            }
            if (!func[12])//listado estadistico
            {
                this.estadísticasToolStripMenuItem.Visible = false;
            }
            if (!func[0] && !func[2] && !func[3] && !func[4] && !func[5])
            {
                this.qweToolStripMenuItem1.Visible = false;
            }
            if (!func[7] && !func[8])
            {
                this.asdToolStripMenuItem2.Visible = false;
            }
            if (!func[9] && !func[10])
            {
                this.estadíaToolStripMenuItem.Visible = false;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string ConnStr = @"Data Source=localhost\SQLSERVER2008;Initial Catalog=GD2C2014;User ID=gd;Password=gd2014;Trusted_Connection=False;";

            SqlConnection conn = new SqlConnection(ConnStr);
            SqlCommand cmd = new SqlCommand("SELECT TOP 10 [Hotel_Ciudad] FROM [GD2C2014].[gd_esquema].[Maestra]", conn);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                listBox1.Items.Add(reader["Hotel_Ciudad"].ToString());
            }
            reader.Close();
            conn.Close(); 

            string sCnn;
            sCnn = @"data source = Gonzalo-PC\SQLSERVER2008; initial catalog = GD2C2014; user id = gd; password = gd2014";

            string sSel = @"SELECT TOP 1000 [Hotel_Ciudad]
                                 ,[Hotel_Calle]
                                 ,[Hotel_Nro_Calle]
                                 ,[Hotel_CantEstrella]
                                 ,[Hotel_Recarga_Estrella]
                                 ,[Habitacion_Numero]
                                  ,[Habitacion_Piso]
                                    ,[Habitacion_Frente]
                                    ,[Habitacion_Tipo_Codigo]
                                 ,[Habitacion_Tipo_Descripcion]
      ,[Habitacion_Tipo_Porcentual]
      ,[Regimen_Descripcion]
      ,[Regimen_Precio]
      ,[Reserva_Fecha_Inicio]
      ,[Reserva_Codigo]
      ,[Reserva_Cant_Noches]
      ,[Estadia_Fecha_Inicio]
      ,[Estadia_Cant_Noches]
      ,[Consumible_Codigo]
      ,[Consumible_Descripcion]
      ,[Consumible_Precio]
      ,[Item_Factura_Cantidad]
      ,[Item_Factura_Monto]
      ,[Factura_Nro]
      ,[Factura_Fecha]
      ,[Factura_Total]
      ,[Cliente_Pasaporte_Nro]
      ,[Cliente_Apellido]
      ,[Cliente_Nombre]
      ,[Cliente_Fecha_Nac]
      ,[Cliente_Mail]
      ,[Cliente_Dom_Calle]
      ,[Cliente_Nro_Calle]
      ,[Cliente_Piso]
      ,[Cliente_Depto]
      ,[Cliente_Nacionalidad]
  FROM [GD2C2014].[gd_esquema].[Maestra]";

            SqlDataAdapter da;
            DataTable dt = new DataTable();

            try
            {
                da = new SqlDataAdapter(sSel, sCnn);
                da.Fill(dt);

                this.dataGridView1.DataSource = dt;
                //this.dataGridView1.DataBind();
                label1.Text = String.Format("Total datos en la tabla: {0}", dt.Rows.Count);
            }
            catch (Exception ex)
            {
                label1.Text = "Error: " + ex.Message;
            }


            string sCnn2 = @"data source = Gonzalo-PC\SQLSERVER2008; initial catalog = AdventureWorks2008; user id = gd; password = gd2014";
            string sSel2 = @"uspGetAddress";
            SqlDataAdapter da2;
            DataTable dt2 = new DataTable();
            try
            {
                da2 = new SqlDataAdapter(sSel2, sCnn2);
                da2.Fill(dt2);

                this.dataGridView2.DataSource = dt2;
                //this.dataGridView1.DataBind();
                label2.Text = String.Format("Total datos en la tabla: {0}", dt2.Rows.Count);
            }
            catch (Exception ex)
            {
                label2.Text = "Error: " + ex.Message;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form f = new Login.Ingresar(this);
            f.Show();
        }

        private void asdToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            Form f = new Login.SeleccionRol();
            f.Show();
        }

        private void estadísticasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new Listado_Estadistico.Estadisticas();
            f.Show();
            this.Hide();
        }

        private void gfdToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void modificacionBajaDeClienteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new ABM_de_Habitacion.ABMHabitacion();
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
            Form f = new ABM_de_Rol.ABMRol();
            f.Show();
            this.Hide();
        }

        private void aBMDeUsuarioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new ABM_de_Usuario.ABMUsuario();
            f.Show();
            this.Hide();
        }

        private void generarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new Generar_Modificar_Reserva.AltaReserva();
            f.Show();
            this.Hide();
        }

        private void cancelarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new Cancelar_Reserva.BajaReserva();
            f.Show();
            this.Hide();
        }

        private void registarEstadiaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new Registrar_Estadia.RegistrarEstadia();
            f.Show();
            this.Hide();
        }

        private void registrarConsumibleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new Registrar_Consumible.ABMConsumible();
            f.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox2.Text = SHA256Encrypt(textBox1.Text);
        }

        public string SHA256Encrypt(string input)
        {
            SHA256CryptoServiceProvider provider = new SHA256CryptoServiceProvider();

            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hashedBytes = provider.ComputeHash(inputBytes);

            StringBuilder output = new StringBuilder();

            for (int i = 0; i < hashedBytes.Length; i++)
                output.Append(hashedBytes[i].ToString("x2").ToLower());

            return output.ToString();
        }

        

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

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
    }
}
