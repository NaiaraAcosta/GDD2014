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

namespace FrbaHotel.Registrar_Estadia
{
    public partial class cargarCliente : Form
    {
        Form back = null;
        string codReserva;
        int cant = 1;
        int cantMax;
        public cargarCliente(Form atras, string cod)
        {
            InitializeComponent();
            back = atras;
            codReserva = cod;
            cantMax = verificarCant(codReserva);
            if (cant == cantMax)
            {
                this.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new ABM_de_Cliente.ABMCliente(this, 4).Show();
        }

        public void cargarClientes(string idCliente)
        {
            string ConnStr = ConfigurationManager.AppSettings["stringConexion"];
            SqlConnection con = new SqlConnection(ConnStr);
            con.Open();
            SqlCommand scCommand = new SqlCommand("CONTROL_ZETA.SP_AGREGAR_CLIENTE_ESTADIA", con);
            scCommand.CommandType = CommandType.StoredProcedure;
            scCommand.Parameters.Add("@ID_CLIENTE", SqlDbType.Int).Value = int.Parse(idCliente);
            scCommand.Parameters.Add("@ID_RESERVA", SqlDbType.Int).Value = int.Parse(codReserva);
            scCommand.Parameters.Add("@ERROR", SqlDbType.TinyInt).Direction = ParameterDirection.Output;
            if (scCommand.Connection.State == ConnectionState.Closed)
            {
                scCommand.Connection.Open();
            }
            scCommand.ExecuteNonQuery();
            con.Close();
            int result = int.Parse(scCommand.Parameters["@ERROR"].Value.ToString());
            if (result == 1)
            {
                cant++;
                MessageBox.Show("Cliente agregado correctamente", "Operacion correcta", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Cliente ya cargado anteriormente", "Operacion incorrecta", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (cant == cantMax)
            {
                MessageBox.Show("Carga de clientes completa", "Carga completa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
        }

        public int verificarCant(string codReser)
        {
            string ConnStr = ConfigurationManager.AppSettings["stringConexion"];
            SqlConnection con = new SqlConnection(ConnStr);
            con.Open();
            SqlCommand scCommand = new SqlCommand("CONTROL_ZETA.FN_VALIDAD_CANTIDAD_HABITACION", con);
            scCommand.CommandType = CommandType.StoredProcedure;
            scCommand.Parameters.Add("@ID_RESERVA", SqlDbType.Int).Value = int.Parse(codReser);
            scCommand.Parameters.Add("@RETURN_VALUE", SqlDbType.VarChar, 1).Direction = ParameterDirection.ReturnValue;
            if (scCommand.Connection.State == ConnectionState.Closed)
            {
                scCommand.Connection.Open();
            }
            scCommand.ExecuteNonQuery();
            string result = scCommand.Parameters["@RETURN_VALUE"].Value.ToString();
            con.Close();
            return int.Parse(result);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Esta seguro que quiere terminar de cargar clientes en la reserva?", "Esta seguro?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new ABM_de_Cliente.AltaCliente(this, 4).Show();
        }
    }
}
 