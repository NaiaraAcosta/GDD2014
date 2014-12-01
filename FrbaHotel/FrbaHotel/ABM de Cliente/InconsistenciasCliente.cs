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

namespace FrbaHotel.ABM_de_Cliente
{
    public partial class InconsistenciasCliente : Form
    {
        Form back = null;
        int modo;
        public InconsistenciasCliente(Form atras, DataTable ds, int mod)
        {
            InitializeComponent();
            back = atras;
            modo = mod;
            dataGridView1.DataSource = ds;
        }

        private void InconsistenciasCliente_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            back.Show();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if ((dataGridView1.SelectedCells.Count != 0) && (modo == 2))
            {
                //string[] result = dataGridView1.SelectedCells[1].Value.ToString().Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
                string[] param = new string[16];
                param[0] = dataGridView1.SelectedCells[0].Value.ToString(); //id
                param[1] = dataGridView1.SelectedCells[1].Value.ToString(); //nombre
                param[2] = dataGridView1.SelectedCells[2].Value.ToString(); //apellido
                param[3] = dataGridView1.SelectedCells[3].Value.ToString(); //tipo doc
                param[4] = dataGridView1.SelectedCells[4].Value.ToString(); //numero
                param[5] = dataGridView1.SelectedCells[5].Value.ToString(); //mail
                param[6] = dataGridView1.SelectedCells[6].Value.ToString(); //telefono
                param[7] = dataGridView1.SelectedCells[7].Value.ToString(); //localidad
                param[8] = dataGridView1.SelectedCells[8].Value.ToString(); //pais
                param[9] = dataGridView1.SelectedCells[9].Value.ToString(); //calle
                param[10] = dataGridView1.SelectedCells[10].Value.ToString(); //nro calle
                param[11] = dataGridView1.SelectedCells[11].Value.ToString(); //depto
                param[12] = dataGridView1.SelectedCells[12].Value.ToString(); //piso
                param[13] = dataGridView1.SelectedCells[13].Value.ToString(); //nacionalidad
                param[14] = dataGridView1.SelectedCells[14].Value.ToString(); //estado
                param[15] = dataGridView1.SelectedCells[15].Value.ToString(); //nacimiento
                Form f = new ABM_de_Cliente.AltaCliente(this, param, 2, true);
                f.Show();
            }
            else if ((dataGridView1.SelectedCells.Count != 0) && (modo == 3))
            {
                string ConnStr = ConfigurationManager.AppSettings["stringConexion"];
                SqlConnection con = new SqlConnection(ConnStr);
                con.Open();
                SqlCommand scCommand;
                scCommand = new SqlCommand("CONTROL_ZETA.MB_CLIENTE", con);
                scCommand.CommandType = CommandType.StoredProcedure;
                scCommand.Parameters.AddWithValue("@NOMBRE", DBNull.Value);
                scCommand.Parameters.AddWithValue("@APELLIDO", DBNull.Value);
                scCommand.Parameters.Add("@TIPO_IDENT", SqlDbType.TinyInt).Value = int.Parse(dataGridView1.SelectedCells[3].Value.ToString());
                scCommand.Parameters.Add("@NRO_IDENT", SqlDbType.VarChar, 15).Value = dataGridView1.SelectedCells[4].Value.ToString();
                scCommand.Parameters.Add("@EMAIL", SqlDbType.VarChar, 50).Value = dataGridView1.SelectedCells[5].Value.ToString();
                scCommand.Parameters.AddWithValue("@TEL", DBNull.Value);
                scCommand.Parameters.AddWithValue("@NOMBRE_LOC", DBNull.Value);
                scCommand.Parameters.AddWithValue("@NOMBRE_PAIS", DBNull.Value);
                scCommand.Parameters.AddWithValue("@DOM_CALLE", DBNull.Value);
                scCommand.Parameters.AddWithValue("@DOM_NRO", DBNull.Value);
                scCommand.Parameters.AddWithValue("@DEPTO", DBNull.Value);
                scCommand.Parameters.AddWithValue("@DOM_PISO", DBNull.Value);
                scCommand.Parameters.AddWithValue("@NACIONALIDAD_NOMBRE", DBNull.Value);
                scCommand.Parameters.AddWithValue("@FECHA_NAC", DBNull.Value);
                scCommand.Parameters.AddWithValue("@CODIGO_ENTRADA", SqlDbType.TinyInt).Value = 3;
                scCommand.Parameters.Add("@CLIENTE_ID", SqlDbType.Int).Value = int.Parse(dataGridView1.SelectedCells[0].Value.ToString());
                scCommand.Parameters.Add("@CODIGO", SqlDbType.TinyInt).Direction = ParameterDirection.Output;
                scCommand.Parameters.Add("@CLIENTE_ID_NEW", SqlDbType.Int).Direction = ParameterDirection.Output;
                if (scCommand.Connection.State == ConnectionState.Closed)
                {
                    scCommand.Connection.Open();
                }
                scCommand.ExecuteNonQuery();

                int result = int.Parse(scCommand.Parameters["@CODIGO"].Value.ToString());

                switch (result)
                {
                    case 1:
                        {
                            MessageBox.Show("Operacion realizada exitosamente", "Operacion realizada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;
                        }
                    case 2:
                        {
                            MessageBox.Show("Modificacion/Baja de usuario inexistente", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                        }
                    case 3:
                        {
                            MessageBox.Show("Alta de usuario existente", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                back.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("No hay datos que modificar", "No se puede modificar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
