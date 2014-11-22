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

namespace FrbaHotel.Registrar_Consumible
{
    public partial class AltaConsumible : Form
    {
        Registrar_Consumible.ABMConsumible back = null;
        List<string> idCon = new List<string>();
        List<string> listCon = new List<string>();
        List<string> idCon2 = new List<string>();
        List<string> listCon2 = new List<string>();
        List<bool> checkeadoForever = new List<bool>();
        string[] param;
        bool bloqueado = true;
        public AltaConsumible(Registrar_Consumible.ABMConsumible atras, string[] parametros)
        {
            InitializeComponent();
            back = atras;

            string ConnStr2 = ConfigurationManager.AppSettings["stringConexion"];
            SqlConnection conn2 = new SqlConnection(ConnStr2);
            string sSel2 = string.Format(@"SELECT * FROM [GD2C2014].[CONTROL_ZETA].[CONSUMIBLE]");
            SqlCommand cmd2 = new SqlCommand(sSel2, conn2);
            conn2.Open();
            SqlDataReader reader2 = cmd2.ExecuteReader();
            bloqueado = false;
            while (reader2.Read())
            {
                checkedListBox1.Items.Add(reader2["CON_DESCRIPCION"].ToString());
                idCon.Add(reader2["CON_ID"].ToString());
                listCon.Add(reader2["CON_ID"].ToString());

                
                checkedListBox2.Items.Add(reader2["CON_DESCRIPCION"].ToString());
                idCon2.Add(reader2["CON_ID"].ToString());
                listCon2.Add(reader2["CON_ID"].ToString());
                
            }
            bloqueado = true;
            reader2.Close();
            conn2.Close();

            param = parametros;
            cargarCheckedListBox();
        }

        private void refrescarCheckedListBox()
        {
            idCon2 = new List<string>();
            listCon2 = new List<string>();
            checkedListBox2.Items.Clear();
            string ConnStr2 = ConfigurationManager.AppSettings["stringConexion"];
            SqlConnection conn2 = new SqlConnection(ConnStr2);
            string sSel2 = string.Format(@"SELECT * FROM [GD2C2014].[CONTROL_ZETA].[CONSUMIBLE]");
            SqlCommand cmd2 = new SqlCommand(sSel2, conn2);
            conn2.Open();
            SqlDataReader reader2 = cmd2.ExecuteReader();
            bloqueado = false;
            while (reader2.Read())
            {
                checkedListBox2.Items.Add(reader2["CON_DESCRIPCION"].ToString());
                idCon2.Add(reader2["CON_ID"].ToString());
                listCon2.Add(reader2["CON_ID"].ToString());
            }
            bloqueado = true;
            reader2.Close();
            conn2.Close();

            cargarCheckedListBox();
        }

        private void cargarCheckedListBox()
        {
            string ConnStr2 = ConfigurationManager.AppSettings["stringConexion"];
            SqlConnection conn2 = new SqlConnection(ConnStr2);
            string sSel2 = string.Format(@"SELECT * FROM [GD2C2014].[CONTROL_ZETA].[ESTADIA_HAB_CON] estahabcon,
	                    [GD2C2014].[CONTROL_ZETA].[HABITACION] hab
                    where estahabcon.EST_ID = '{0}' 
                    and hab.HAB_NRO = '{1}'
                    and hab.HAB_ID_HOTEL = '{2}'
                    and hab.HAB_ID = estahabcon.HAB_ID", param[0], param[1], Login.Class1.hotel);
            SqlCommand cmd2 = new SqlCommand(sSel2, conn2);
            conn2.Open();
            SqlDataReader reader2 = cmd2.ExecuteReader();
            bloqueado = false;
            while (reader2.Read())
            {
                string lectura = reader2["CON_ID"].ToString();
                for (int i = 0; i < checkedListBox2.Items.Count; i++)
                {
                    int primerUnCheckeado = primerUncheked(i, checkedListBox2);
                    checkedListBox2.SetSelected(primerUnCheckeado, true);
                    if (listCon2[primerUnCheckeado] == lectura)
                    {
                        checkedListBox2.SetItemChecked(primerUnCheckeado, true);
                        lectura = "";
                    }
                    checkedListBox2.SetSelected(primerUnCheckeado, false);
                }
                
            }
            bloqueado = true;
            reader2.Close();
            conn2.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked && e.CurrentValue == CheckState.Unchecked)
            {
                checkedListBox1.Items.Insert(e.Index + 1, checkedListBox1.SelectedItem.ToString());
                listCon.Insert(e.Index + 1, listCon[e.Index]);
            }
            if (e.NewValue == CheckState.Unchecked && e.CurrentValue == CheckState.Checked)
            {
                    if (e.Index == proximoChecked(e.Index, checkedListBox1))
                    {
                        checkedListBox1.Items.RemoveAt(e.Index);
                        listCon.RemoveAt(e.Index);
                    }
                    else
                    {
                        checkedListBox1.Items.RemoveAt(proximoChecked(e.Index, checkedListBox1));
                        listCon.RemoveAt(proximoChecked(e.Index, checkedListBox1));
                        e.NewValue = e.CurrentValue;
                    }
            }
        }

        private int proximoChecked(int aBorrar, CheckedListBox listaCheck)
        {
            if (!listaCheck.GetItemChecked(aBorrar + 1))
            {
                return aBorrar;
            }
            else
            {
                return proximoChecked(aBorrar + 1, listaCheck);
            }
        }

        private int primerUncheked(int comienzo, CheckedListBox listaCheck)
        {
            if (!listaCheck.GetItemChecked(comienzo))
            {
                return comienzo;
            }
            else
            {
                return primerUncheked(comienzo + 1, listaCheck);
            }
        }

        private int cantCon(string tipo)
        {
            return listCon.Count(x => x == tipo) - 1;
        }

        private int buscarHotel(string estadia)
        {
            string ConnStr2 = ConfigurationManager.AppSettings["stringConexion"];
            SqlConnection conn2 = new SqlConnection(ConnStr2);
            string sSel2 = string.Format(@"SELECT * FROM [GD2C2014].[CONTROL_ZETA].[RESERVA] res, [GD2C2014].[CONTROL_ZETA].[ESTADIA] est
                    where est.EST_RESERVA_ID = res.RESERVA_ID
                    and est.EST_ID = {0}", estadia);
            SqlCommand cmd2 = new SqlCommand(sSel2, conn2);
            conn2.Open();
            SqlDataReader reader2 = cmd2.ExecuteReader();
            int salida = 0;
            while (reader2.Read())
            {
                salida = int.Parse(reader2["RESERVA_ID_HOTEL"].ToString());
            }
            reader2.Close();
            conn2.Close();
            return salida;
        }
        private int buscarHab(int idHotel, string idHab)
        {
            string ConnStr2 = ConfigurationManager.AppSettings["stringConexion"];
            SqlConnection conn2 = new SqlConnection(ConnStr2);
            string sSel2 = string.Format(@"SELECT * FROM [GD2C2014].[CONTROL_ZETA].[HABITACION]
                    where HAB_ID_HOTEL = {0}
                    and HAB_ID = {1}", idHotel, idHab);
            SqlCommand cmd2 = new SqlCommand(sSel2, conn2);
            conn2.Open();
            SqlDataReader reader2 = cmd2.ExecuteReader();
            int salida = 0;
            while (reader2.Read())
            {
                salida = int.Parse(reader2["HAB_NRO"].ToString());
            }
            reader2.Close();
            conn2.Close();
            return salida;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int idHotel = buscarHotel(param[0]);
            int nroHab = int.Parse(param[1]);
            string ConnStr = ConfigurationManager.AppSettings["stringConexion"];
            SqlConnection con = new SqlConnection(ConnStr);
            con.Open();
            SqlTransaction transaction = con.BeginTransaction();
            List<int> cantidad = new List<int>();
            bool conError = false;
            for (int i = 0; i < idCon.Count; i++)
            {
                string tipo = idCon[i];
                cantidad.Add(cantCon(tipo));
                if (cantidad[i] != 0)
                {
                    SqlCommand scCommand = new SqlCommand("CONTROL_ZETA.SP_REGISTRAR_CONSUMIBLE", con, transaction);
                    scCommand.CommandType = CommandType.StoredProcedure;
                    scCommand.Parameters.Add("@id_hotel", SqlDbType.Int).Value = idHotel;
                    scCommand.Parameters.Add("@nro_hab", SqlDbType.SmallInt).Value = nroHab;
                    scCommand.Parameters.Add("@id_con ", SqlDbType.SmallInt).Value = int.Parse(idCon[i]);
                    scCommand.Parameters.Add("@id_est", SqlDbType.Int).Value = int.Parse(param[0]);
                    scCommand.Parameters.Add("@cant", SqlDbType.TinyInt).Value = cantidad[i];
                    scCommand.Parameters.Add("@error", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                    if (scCommand.Connection.State == ConnectionState.Closed)
                    {
                        scCommand.Connection.Open();
                    }
                    scCommand.ExecuteNonQuery();
                    int result = int.Parse(scCommand.Parameters["@error"].Value.ToString());
                    if (result != 1)
                    {
                        string mensaje = string.Format("Error en la carga, COD: {0}", result);
                        MessageBox.Show(mensaje , "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        conError = true;
                    }
                }
            }
            if (!conError)
            {
                DialogResult seguro = MessageBox.Show("Esta seguro de agregar los consumibles?", "Esta seguro?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (seguro == DialogResult.Yes)
                {
                    transaction.Commit();
                    MessageBox.Show("Consumibles agregados correctamente", "Operacion realizada correctamente", MessageBoxButtons.OK, MessageBoxIcon.None);
                }
                else
                {
                    transaction.Rollback();
                }
            }
            refrescarCheckedListBox();
            con.Close();

            limpiarCheckeados();
        }

        private void limpiarCheckeados()
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                if (checkedListBox1.GetItemChecked(i))
                {
                    checkedListBox1.SetItemChecked(i, false);
                }
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            back.Show();
            this.Close();
        }

        private void checkedListBox2_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (!bloqueado)
            {
                if (e.NewValue == CheckState.Checked && e.CurrentValue == CheckState.Unchecked)
                {
                    checkedListBox2.Items.Insert(e.Index + 1, checkedListBox2.SelectedItem.ToString());
                    listCon2.Insert(e.Index + 1, listCon2[e.Index]);
                }
                if (e.NewValue == CheckState.Unchecked && e.CurrentValue == CheckState.Checked)
                {
                    if (e.Index == proximoChecked(e.Index, checkedListBox2))
                    {
                        checkedListBox2.Items.RemoveAt(e.Index);
                        listCon2.RemoveAt(e.Index);
                    }
                    else
                    {
                        checkedListBox2.Items.RemoveAt(proximoChecked(e.Index, checkedListBox2));
                        listCon2.RemoveAt(proximoChecked(e.Index, checkedListBox2));
                        e.NewValue = e.CurrentValue;
                    }
                }
            }
            else
            {
                e.NewValue = e.CurrentValue;
            }
        }

        private void checkedListBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
