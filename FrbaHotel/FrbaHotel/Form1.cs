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
          
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form f = new Login.Form1();
            f.Show();
        }
   
    }
}
