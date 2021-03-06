﻿using System;
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
        Form back = null;
        int IDRol = 0;
        public AltaRol()
        {
            InitializeComponent();
        }

        public AltaRol(Form atras, int idrol)
        {
            InitializeComponent();
            back = atras;
            IDRol = idrol +1;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //if (IDRol != 0)
            //{
            //    string ConnStr = ConfigurationManager.AppSettings["stringConexion"];
            //    SqlConnection con = new SqlConnection(ConnStr);
            //    con.Open();
            //    SqlTransaction transaction = con.BeginTransaction();
            //    List<int> cantidad = new List<int>();
            //    bool conError = false;
            //    for (int i = 0; i < idCon.Count; i++)
            //    {
            //        string tipo = idCon[i];
            //        cantidad.Add(cantCon(tipo));
            //        if (cantidad[i] != 0)
            //        {
            //            SqlCommand scCommand = new SqlCommand("CONTROL_ZETA.SP_ABM_ROL", con, transaction);
            //            scCommand.CommandType = CommandType.StoredProcedure;
            //            scCommand.Parameters.Add("@id_hotel", SqlDbType.Int).Value = idHotel;
            //            scCommand.Parameters.Add("@nro_hab", SqlDbType.SmallInt).Value = nroHab;
            //            scCommand.Parameters.Add("@id_con ", SqlDbType.SmallInt).Value = int.Parse(idCon[i]);
            //            scCommand.Parameters.Add("@id_est", SqlDbType.Int).Value = int.Parse(param[0]);
            //            scCommand.Parameters.Add("@cant", SqlDbType.TinyInt).Value = cantidad[i];
            //            scCommand.Parameters.Add("@error", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
            //            if (scCommand.Connection.State == ConnectionState.Closed)
            //            {
            //                scCommand.Connection.Open();
            //            }
            //            scCommand.ExecuteNonQuery();
            //            int result = int.Parse(scCommand.Parameters["@error"].Value.ToString());
            //            if (result != 1)
            //            {
            //                string mensaje = string.Format("Error en la carga, COD: {0}", result);
            //                MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //                conError = true;
            //            }
            //        }
            //    }



            //    int idHotel = buscarHotel(param[0]);
            //    int nroHab = buscarHab(idHotel, param[1]);
            //    string ConnStr = ConfigurationManager.AppSettings["stringConexion"];
            //    SqlConnection con = new SqlConnection(ConnStr);
            //    con.Open();
            //    SqlTransaction transaction = con.BeginTransaction();
            //    List<int> cantidad = new List<int>();
            //    bool conError = false;
            //    for (int i = 0; i < idCon.Count; i++)
            //    {
            //        string tipo = idCon[i];
            //        cantidad.Add(cantCon(tipo));
            //        if (cantidad[i] != 0)
            //        {
            //            SqlCommand scCommand = new SqlCommand("CONTROL_ZETA.SP_REGISTRAR_CONSUMIBLE", con, transaction);
            //            scCommand.CommandType = CommandType.StoredProcedure;
            //            scCommand.Parameters.Add("@id_hotel", SqlDbType.Int).Value = idHotel;
            //            scCommand.Parameters.Add("@nro_hab", SqlDbType.SmallInt).Value = nroHab;
            //            scCommand.Parameters.Add("@id_con ", SqlDbType.SmallInt).Value = int.Parse(idCon[i]);
            //            scCommand.Parameters.Add("@id_est", SqlDbType.Int).Value = int.Parse(param[0]);
            //            scCommand.Parameters.Add("@cant", SqlDbType.TinyInt).Value = cantidad[i];
            //            scCommand.Parameters.Add("@error", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
            //            if (scCommand.Connection.State == ConnectionState.Closed)
            //            {
            //                scCommand.Connection.Open();
            //            }
            //            scCommand.ExecuteNonQuery();
            //            int result = int.Parse(scCommand.Parameters["@error"].Value.ToString());
            //            if (result != 1)
            //            {
            //                string mensaje = string.Format("Error en la carga, COD: {0}", result);
            //                MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //                conError = true;
            //            }
            //        }
            //    }
            //}
        }

        private void AltaRol_Load(object sender, EventArgs e)
        {
            string ConnStr = ConfigurationManager.AppSettings["stringConexion"];
            SqlConnection conn = new SqlConnection(ConnStr);
            string sSel = string.Format(@"SELECT ROL_NOMBRE, ROL_ESTADO FROM [GD2C2014].[Control_zeta].[Rol]  
                where ROL_ID = {0}" , IDRol);
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

            conn = new SqlConnection(ConnStr);
            sSel = string.Format(@"SELECT FUNC_DETALLE FROM [GD2C2014].[Control_zeta].[FUNCIONALIDAD]");
            cmd = new SqlCommand(sSel, conn);
            conn.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                checkedListBox1.Items.Add(reader["FUNC_DETALLE"].ToString());
            }
            reader.Close();
            conn.Close();

            conn = new SqlConnection(ConnStr);
            sSel = string.Format(@"SELECT FUNC_DETALLE FROM [GD2C2014].[Control_zeta].[FUNCIONALIDAD] f,  
                        [GD2C2014].[Control_zeta].[ROL_FUNC] rf 
                        where rf.ROL_ID = {0} and rf.FUNC_ID = f.FUNC_ID", IDRol);
            cmd = new SqlCommand(sSel, conn);
            conn.Open();
            reader = cmd.ExecuteReader();
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
