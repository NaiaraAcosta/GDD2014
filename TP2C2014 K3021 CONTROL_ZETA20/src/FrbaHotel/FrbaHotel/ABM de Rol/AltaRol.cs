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
    public partial class AltaRol : Form
    {
        ABM_de_Rol.ABMRol back = null;
        int IDRol = 0;
        int modo;
        public AltaRol(Form atras)
        {
            InitializeComponent();
            back = (ABM_de_Rol.ABMRol) atras;
            modo = 1;
            cargarFunc();
        }

        public AltaRol(Form atras, int idrol)
        {
            InitializeComponent();
            back = (ABM_de_Rol.ABMRol) atras;
            IDRol = idrol +1;
            modo = 2;
            cargarDatos();
            cargarFunc();
            actualizarDetalle();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
                string ConnStr = ConfigurationManager.AppSettings["stringConexion"];
                SqlConnection con = new SqlConnection(ConnStr);
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();
                bool conError = false;
                SqlCommand scCommand = new SqlCommand("CONTROL_ZETA.SP_ABM_ROL", con, transaction);
                scCommand.CommandType = CommandType.StoredProcedure;
                scCommand.Parameters.Add("@accion", SqlDbType.SmallInt).Value = modo;
                if (modo == 2)
                {
                    scCommand.Parameters.Add("@id_rol", SqlDbType.TinyInt).Value = IDRol;
                }
                else
                {
                    scCommand.Parameters.AddWithValue("@id_rol", DBNull.Value);
                }
                scCommand.Parameters.Add("@nombre", SqlDbType.VarChar, 20).Value = textBox1.Text;
                if (checkBox1.Checked)
                {
                    scCommand.Parameters.Add("@estado", SqlDbType.VarChar, 1).Value = "H";
                }
                else
                {
                    scCommand.Parameters.Add("@estado", SqlDbType.VarChar, 1).Value = "I";
                }
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
                    default:
                        {
                            if (modo == 1)
                            {
                                IDRol = int.Parse(scCommand.Parameters["@id_rol_new"].Value.ToString());
                            }
                            break;
                        }
                }


                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    if (checkedListBox1.GetItemChecked(i))
                    {
                        scCommand = new SqlCommand("CONTROL_ZETA.SP_ROL_FUNC", con, transaction);
                        scCommand.CommandType = CommandType.StoredProcedure;
                        scCommand.Parameters.Add("@rol_id", SqlDbType.Int).Value = IDRol;
                        scCommand.Parameters.Add("@func_id", SqlDbType.SmallInt).Value = i + 1;
                        scCommand.Parameters.Add("@error", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                        if (scCommand.Connection.State == ConnectionState.Closed)
                        {
                            scCommand.Connection.Open();
                        }
                        scCommand.ExecuteNonQuery();
                        result = int.Parse(scCommand.Parameters["@error"].Value.ToString());
                        if (result == 2)
                        {
                            string mensaje = string.Format("Ya existe ese func({0}) para ese rol({1})", IDRol, i + 1);
                            MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            conError = true;
                        }
                    }
                }
                if (!conError)
                {
                    transaction.Commit();
                    MessageBox.Show("Operacion realizada exitosamente", "Operacion realizada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    transaction.Rollback();
                }
            
            back.refrescar();
        }

        private void cargarDatos()
        {
            string ConnStr = ConfigurationManager.AppSettings["stringConexion"];
            SqlConnection conn = new SqlConnection(ConnStr);
            string sSel = string.Format(@"SELECT ROL_NOMBRE, ROL_ESTADO FROM [GD2C2014].[Control_zeta].[Rol]  
                where ROL_ID = {0}", IDRol);
            SqlCommand cmd = new SqlCommand(sSel, conn);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            textBox1.Text = reader["ROL_NOMBRE"].ToString();
            if (reader["ROL_ESTADO"].ToString() == "H")
            {
                checkBox1.Checked = true;
            }
            else
            {
                checkBox1.Checked = false;
            }
            reader.Close();
            conn.Close();
        }

        private void cargarFunc()
        {
            string ConnStr = ConfigurationManager.AppSettings["stringConexion"];
            SqlConnection conn = new SqlConnection(ConnStr);
            string sSel = string.Format(@"SELECT FUNC_DETALLE FROM [GD2C2014].[Control_zeta].[FUNCIONALIDAD]");
            SqlCommand cmd = new SqlCommand(sSel, conn);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                checkedListBox1.Items.Add(reader["FUNC_DETALLE"].ToString());
            }
            reader.Close();
            conn.Close();
        }

        private void AltaRol_Load(object sender, EventArgs e)
        {

        }

        private void actualizarDetalle()
        {
            string ConnStr = ConfigurationManager.AppSettings["stringConexion"];
            SqlConnection conn = new SqlConnection(ConnStr);
            string sSel = string.Format(@"SELECT FUNC_DETALLE FROM [GD2C2014].[Control_zeta].[FUNCIONALIDAD] f,  
                        [GD2C2014].[Control_zeta].[ROL_FUNC] rf 
                        where rf.ROL_ID = {0} and rf.FUNC_ID = f.FUNC_ID", IDRol);
            SqlCommand cmd = new SqlCommand(sSel, conn);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    if (checkedListBox1.Items[i].ToString() == reader["FUNC_DETALLE"].ToString())
                    {
                        checkedListBox1.SetItemChecked(i, true);
                    }
                }
            }
            reader.Close();
            conn.Close();
        }

        private void button1_Click(object sender, EventArgs e)
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
    }
}
