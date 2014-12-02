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

namespace FrbaHotel.ABM_de_Habitacion
{
    public partial class ABMHabitacion : Form
    {
        string hotel = "1";
        Form back = null;
        public ABMHabitacion()
        {
            InitializeComponent();
        }
        public ABMHabitacion(string idHotel)
        {
            InitializeComponent();
            hotel = idHotel;
        }
        public ABMHabitacion(Form atras)
        {
            InitializeComponent();
            back = atras;
        }

        public void recargar()
        {
            string sCnn;
            sCnn = ConfigurationManager.AppSettings["stringConexion"];

            string sSel = String.Format("SELECT * FROM [GD2C2014].[CONTROL_ZETA].[HABITACION] where HAB_ID_HOTEL = '{0}'", Login.Class1.hotel);
            SqlDataAdapter da;
            DataTable dt = new DataTable();
            try
            {
                da = new SqlDataAdapter(sSel, sCnn);
                da.Fill(dt);
                this.dataGridView1.DataSource = dt;
                label1.Text = String.Format("Total datos en la tabla: {0}", dt.Rows.Count);
            }
            catch (Exception ex)
            {
                label1.Text = "Error: " + ex.Message;
            }


            string ConnStr = ConfigurationManager.AppSettings["stringConexion"];
            SqlConnection conn = new SqlConnection(ConnStr);
            sSel = string.Format(@"SELECT * FROM [GD2C2014].[CONTROL_ZETA].[HOTEL] hotel, 
                [GD2C2014].[CONTROL_ZETA].[LOCALIDAD] loc 
                where hotel.HOTEL_ID = '{0}' 
                and hotel.HOTEL_ID_LOC = loc.LOC_ID", Login.Class1.hotel);
            SqlCommand cmd = new SqlCommand(sSel, conn);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            string detalle = "";
            while (reader.Read())
            {
                detalle = string.Format("{0} - {1} {2}",
                    reader["LOC_DETALLE"].ToString().Trim(),
                    reader["HOTEL_CALLE"].ToString(),
                    reader["HOTEL_NRO_CALLE"].ToString());
                textBox1.Text = detalle;
            }
            reader.Close();
            conn.Close(); 
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            recargar();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count != 0)
            {
                Form f = new ABM_de_Habitacion.AltaHabitacion(this,
                    dataGridView1.SelectedCells[0].Value.ToString(), //id
                    dataGridView1.SelectedCells[1].Value.ToString(), //num
                    dataGridView1.SelectedCells[3].Value.ToString(), //piso
                    dataGridView1.SelectedCells[5].Value.ToString(), //ubi
                    dataGridView1.SelectedCells[4].Value.ToString(), //tipo
                    dataGridView1.SelectedCells[6].Value.ToString());//comodidades
                f.Show();
            }
            else
            {
                MessageBox.Show("No hay datos que modificar", "No se puede modificar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (back != null)
            {
                back.Show();
            }
            else
            {
                new FrbaHotel.MenuPrincipal().Show();
            }
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new ABM_de_Habitacion.AltaHabitacion(this).Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count != 0)
            {
                if (dataGridView1.SelectedCells[7].Value.ToString() != "I")
                {
                    string ConnStr = ConfigurationManager.AppSettings["stringConexion"];
                    SqlConnection con = new SqlConnection(ConnStr);
                    con.Open();
                    SqlCommand scCommand = new SqlCommand("CONTROL_ZETA.SP_ABM_HABITACION", con);
                    scCommand.CommandType = CommandType.StoredProcedure;
                    scCommand.Parameters.Add("@accion", SqlDbType.TinyInt).Value = 3;
                    scCommand.Parameters.AddWithValue("@nro_hab", DBNull.Value);
                    scCommand.Parameters.Add("@id_hab", SqlDbType.Int).Value = int.Parse(dataGridView1.SelectedCells[0].Value.ToString());
                    scCommand.Parameters.AddWithValue("@hab_piso", DBNull.Value);
                    scCommand.Parameters.AddWithValue("@ubi_hab", DBNull.Value);
                    scCommand.Parameters.AddWithValue("@obs", DBNull.Value);
                    scCommand.Parameters.AddWithValue("@id_hotel", DBNull.Value);
                    scCommand.Parameters.AddWithValue("@id_tipo_hab", DBNull.Value);
                    scCommand.Parameters.Add("@error", SqlDbType.TinyInt).Direction = ParameterDirection.Output;
                    scCommand.Parameters.Add("@id_hab_new", SqlDbType.Int).Direction = ParameterDirection.Output;
                    if (scCommand.Connection.State == ConnectionState.Closed)
                    {
                        scCommand.Connection.Open();
                    }
                    scCommand.ExecuteNonQuery();

                    int result = int.Parse(scCommand.Parameters["@error"].Value.ToString());

                    switch (result)
                    {
                        case 1:
                            {
                                MessageBox.Show("Operacion realizada exitosamente", "Operacion realizada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                break;
                            }
                        case 2:
                            {
                                MessageBox.Show("No existe la habitacion en ese hotel", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;
                            }
                        case 3:
                            {
                                MessageBox.Show("Ya existe la habitacion en ese hotel", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;
                            }
                        default:
                            {
                                string mensaje = string.Format("Error en la operacion, COD: {0}", result);
                                MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;
                            }
                    }
                    con.Close();
                }
                else
                {
                    string ConnStr = ConfigurationManager.AppSettings["stringConexion"];
                    SqlConnection con = new SqlConnection(ConnStr);
                    con.Open();
                    SqlCommand scCommand = new SqlCommand("CONTROL_ZETA.SP_ABM_HABITACION", con);
                    scCommand.CommandType = CommandType.StoredProcedure;
                    scCommand.Parameters.Add("@accion", SqlDbType.TinyInt).Value = 2;
                    scCommand.Parameters.Add("@nro_hab", SqlDbType.SmallInt).Value = int.Parse(dataGridView1.SelectedCells[1].Value.ToString());
                    scCommand.Parameters.Add("@id_hab", SqlDbType.Int).Value = int.Parse(dataGridView1.SelectedCells[0].Value.ToString());
                    scCommand.Parameters.Add("@hab_piso", SqlDbType.SmallInt).Value = int.Parse(dataGridView1.SelectedCells[3].Value.ToString());
                    scCommand.Parameters.Add("@ubi_hab", SqlDbType.VarChar, 70).Value = dataGridView1.SelectedCells[5].Value.ToString();
                    scCommand.Parameters.Add("@obs", SqlDbType.VarChar, 150).Value = dataGridView1.SelectedCells[6].Value.ToString();
                    scCommand.Parameters.Add("@id_hotel", SqlDbType.Int).Value = Login.Class1.hotel;
                    scCommand.Parameters.Add("@id_tipo_hab", SqlDbType.SmallInt).Value = dataGridView1.SelectedCells[4].Value.ToString();
                    scCommand.Parameters.Add("@error", SqlDbType.TinyInt).Direction = ParameterDirection.Output;
                    scCommand.Parameters.Add("@id_hab_new", SqlDbType.Int).Direction = ParameterDirection.Output;
                    if (scCommand.Connection.State == ConnectionState.Closed)
                    {
                        scCommand.Connection.Open();
                    }
                    scCommand.ExecuteNonQuery();

                    int result = int.Parse(scCommand.Parameters["@error"].Value.ToString());

                    switch (result)
                    {
                        case 1:
                            {
                                MessageBox.Show("Operacion realizada exitosamente", "Operacion realizada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                break;
                            }
                        case 2:
                            {
                                MessageBox.Show("No existe la habitacion en ese hotel", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;
                            }
                        case 3:
                            {
                                MessageBox.Show("Ya existe la habitacion en ese hotel", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;
                            }
                        default:
                            {
                                string mensaje = string.Format("Error en la operacion, COD: {0}", result);
                                MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;
                            }
                    }

                    con.Close();
                }
            }
            else
            {
                MessageBox.Show("No hay datos que borrar", "No se puede eliminar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            this.recargar();
        }
    }
}
