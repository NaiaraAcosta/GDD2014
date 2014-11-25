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

namespace FrbaHotel.ABM_de_Rol
{
    public partial class ABMRol : Form
    {
        Form back = null;
        public ABMRol()
        {
            InitializeComponent();
        }

        public ABMRol(Form atras)
        {
            InitializeComponent();
            back = atras;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form f = new AltaRol(this);
            f.Text = "Agregar Rol";
            f.Show();
        }

        

        private void ABMRol_Load(object sender, EventArgs e)
        {
            refrescar(); 
        }

        public void refrescar()
        {
            string ConnStr = ConfigurationManager.AppSettings["stringConexion"];
            SqlConnection conn = new SqlConnection(ConnStr);
            SqlCommand cmd = new SqlCommand("SELECT * FROM [GD2C2014].[Control_Zeta].[Rol]", conn);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            string Rol;
            listBox1.Items.Clear();
            while (reader.Read())
            {
                Rol = string.Format("{0} - {1}", reader["Rol_Estado"].ToString(), reader["Rol_Nombre"].ToString());
                listBox1.Items.Add(Rol);
            }
            reader.Close();
            conn.Close();
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

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                Form f = new AltaRol(this, listBox1.SelectedIndex);
                f.Text = "Modificar Rol";
                f.Show();
            }
            else
            {
                MessageBox.Show("No hay datos que modificar", "No se puede modificar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                string ConnStr = ConfigurationManager.AppSettings["stringConexion"];
                SqlConnection con = new SqlConnection(ConnStr);
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();
                bool conError = false;
                SqlCommand scCommand = new SqlCommand("CONTROL_ZETA.SP_ABM_ROL", con, transaction);
                scCommand.CommandType = CommandType.StoredProcedure;
                scCommand.Parameters.Add("@accion", SqlDbType.SmallInt).Value = 3;
                scCommand.Parameters.Add("@id_rol", SqlDbType.TinyInt).Value = listBox1.SelectedIndex + 1;
                scCommand.Parameters.AddWithValue("@nombre", DBNull.Value);
                scCommand.Parameters.AddWithValue("@estado", DBNull.Value);
                scCommand.Parameters.Add("@id_rol_new", SqlDbType.TinyInt).Direction = ParameterDirection.Output;
                scCommand.Parameters.Add("@error", SqlDbType.TinyInt).Direction = ParameterDirection.Output;
                if (scCommand.Connection.State == ConnectionState.Closed)
                {
                    scCommand.Connection.Open();
                }
                scCommand.ExecuteNonQuery();
                int result = int.Parse(scCommand.Parameters["@error"].Value.ToString());
                switch (result)
                {
                    case 2:
                        {
                            MessageBox.Show("No se encontro el Rol", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            conError = true;
                            break;
                        }
                    case 3:
                        {
                            MessageBox.Show("Ya existe el Rol", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            conError = true;
                            break;
                        }
                }

                if (!conError)
                {
                    DialogResult resultado = MessageBox.Show("Esta seguro que quiere deshabilitar el Rol?", "Esta seguro?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (resultado == DialogResult.Yes)
                    {
                        transaction.Commit();
                        MessageBox.Show("Operacion realizada exitosamente", "Operacion realizada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        transaction.Rollback();
                    }
                }
                else
                {
                    transaction.Rollback();
                }
            }
            else
            {
                MessageBox.Show("No hay datos que modificar", "No se puede modificar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            this.refrescar();
        }
    }
}
