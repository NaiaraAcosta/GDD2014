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
        string[] param;
        public AltaConsumible(Registrar_Consumible.ABMConsumible atras, string[] parametros)
        {
            InitializeComponent();
            back = atras;

            string ConnStr2 = @"Data Source=localhost\SQLSERVER2008;Initial Catalog=GD2C2014;User ID=gd;Password=gd2014;Trusted_Connection=False;";
            SqlConnection conn2 = new SqlConnection(ConnStr2);
            string sSel2 = string.Format(@"SELECT * FROM [GD2C2014].[CONTROL_ZETA].[CONSUMIBLE]");
            SqlCommand cmd2 = new SqlCommand(sSel2, conn2);
            conn2.Open();
            SqlDataReader reader2 = cmd2.ExecuteReader();
            while (reader2.Read())
            {
                checkedListBox1.Items.Add(reader2["CON_DESCRIPCION"].ToString());
                idCon.Add(reader2["CON_ID"].ToString());
                listCon.Add(reader2["CON_ID"].ToString());
            }
            reader2.Close();
            conn2.Close();

            param = parametros;
            cargarCheckedListBox();
        }

        private void cargarCheckedListBox()
        {

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
                if (e.Index == proximoChecked(e.Index))
                {
                    checkedListBox1.Items.RemoveAt(e.Index);
                    listCon.RemoveAt(e.Index);
                }
                else
                {
                    checkedListBox1.Items.RemoveAt(proximoChecked(e.Index));
                    listCon.RemoveAt(proximoChecked(e.Index));
                    e.NewValue = e.CurrentValue;
                }
            }
        }

        private int proximoChecked(int aBorrar)
        {
            if (!checkedListBox1.GetItemChecked(aBorrar + 1))
            {
                return aBorrar;
            }
            else
            {
                return proximoChecked(aBorrar + 1);
            }
        }

        private int primerUncheked(int comienzo)
        {
            if (!checkedListBox1.GetItemChecked(comienzo))
            {
                return comienzo;
            }
            else
            {
                return primerUncheked(comienzo + 1);
            }
        }
    }
}
