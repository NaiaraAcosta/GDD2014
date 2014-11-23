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

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                string ConnStr = ConfigurationManager.AppSettings["stringConexion"];
                SqlConnection conn = new SqlConnection(ConnStr);
                string sSel = string.Format(@"SELECT * FROM [GD2C2014].[CONTROL_ZETA].[RESERVA] res, 
                    [GD2C2014].[CONTROL_ZETA].[HOTEL] hotel,
                    [GD2C2014].[CONTROL_ZETA].[REGIMEN] reg, 
                    [GD2C2014].[CONTROL_ZETA].[LOCALIDAD] loc
                    where res.RESERVA_ID = '{0}'
                    and res.RESERVA_ID_HOTEL = hotel.HOTEL_ID
                    and res.RESERVA_ID_REGIMEN = reg.REG_ID
                    and loc.LOC_ID = hotel.HOTEL_ID_LOC", textBox1.Text);
                SqlCommand cmd = new SqlCommand(sSel, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    if (reader["RES_PRECIO_TOTAL"].ToString() == "")
                    {
                        SqlConnection con = new SqlConnection(ConnStr);
                        con.Open();
                        SqlCommand scCommand = new SqlCommand("CONTROL_ZETA.SP_ACT_PRECIO_RES", con);
                        scCommand.CommandType = CommandType.StoredProcedure;
                        scCommand.Parameters.Add("@id_reserva", SqlDbType.Int).Value = int.Parse(textBox1.Text);
                        if (scCommand.Connection.State == ConnectionState.Closed)
                        {
                            scCommand.Connection.Open();
                        }
                        scCommand.ExecuteNonQuery();
                        con.Close();
                    }
                    Form f = new AltaReserva(this, reader, conn);
                    f.Show();
                    this.Hide();
                }

                    
                else
                {
                    MessageBox.Show("No existe la reserva especificada", "Error de ingreso de datos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            back.Show();
        }
    }
}
