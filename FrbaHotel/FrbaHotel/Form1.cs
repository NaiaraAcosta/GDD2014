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


namespace FrbaHotel
{
    public partial class Form1 : Form
    {
        private SqlDataAdapter dataAdapter = new SqlDataAdapter();
        public Form1()
        {
            
            
            InitializeComponent();

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


        }

        private void Form1_Load(object sender, EventArgs e)
        {
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
            Form f = new Login.Form1();
            f.Show();
        }

        private void asdToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            Form f = new Login.Form1();
            f.Show();
        }

        private void estadísticasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new Listado_Estadistico.Form1();
            f.Show();
            this.Hide();
        }

        private void gfdToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void altaClienteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new ABM_de_Cliente.Form1();
            f.Show();
            this.Hide();
        }

        private void bajaModificacionClienteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new ABM_de_Cliente.Form2();
            f.Show();
            this.Hide();
        }

        private void modificacionBajaDeClienteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new ABM_de_Habitacion.Form2();
            f.Show();
            this.Hide();
        }

        private void aBMDeHotelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new ABM_de_Hotel.Form2();
            f.Show();
            this.Hide();
        }

        private void aBMDeRolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new ABM_de_Rol.Form2();
            f.Show();
            this.Hide();
        }

        private void aBMDeUsuarioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new ABM_de_Usuario.Form2();
            f.Show();
            this.Hide();
        }

        private void generarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new Generar_Modificar_Reserva.Form1();
            f.Show();
            this.Hide();
        }

        private void cancelarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new Cancelar_Reserva.Form1();
            f.Show();
            this.Hide();
        }

        private void registarEstadiaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new Registrar_Estadia.Form1();
            f.Show();
            this.Hide();
        }

        private void registrarConsumibleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new Registrar_Consumible.Form2();
            f.Show();
            this.Hide();
        }
   
    }
}
