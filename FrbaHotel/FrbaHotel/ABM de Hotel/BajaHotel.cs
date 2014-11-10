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

namespace FrbaHotel.ABM_de_Hotel
{
    public partial class BajaHotel : Form
    {
        Form back = null;
        string IDHotel;
        public BajaHotel()
        {
            InitializeComponent();
        }

        public BajaHotel(Form atras, string idHotel)
        {
            InitializeComponent();
            back = atras;
            IDHotel = idHotel;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form3_Load(object sender, EventArgs e)
        {
            int año = int.Parse(ConfigurationManager.AppSettings["Año"]);
            int mes = int.Parse(ConfigurationManager.AppSettings["Mes"]);
            int dia = int.Parse(ConfigurationManager.AppSettings["Dia"]);
            dateTimePicker1.Value = new DateTime(año, mes, dia);
            dateTimePicker2.Value = new DateTime(año, mes, dia);
            dateTimePicker1.MinDate = dateTimePicker1.Value;
            dateTimePicker2.MinDate = dateTimePicker2.Value;
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            dateTimePicker1.MaxDate = dateTimePicker2.Value;
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            dateTimePicker2.MinDate = dateTimePicker1.Value;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string ConnStr = ConfigurationManager.AppSettings["stringConexion"];
            SqlConnection con = new SqlConnection(ConnStr);
            con.Open();
            SqlTransaction transaction = con.BeginTransaction();
            bool conError = false;
            SqlCommand scCommand = new SqlCommand("CONTROL_ZETA.SP_CIERRE_HOTEL", con, transaction);
            scCommand.CommandType = CommandType.StoredProcedure;
            scCommand.Parameters.Add("@fe_inicio_cierre", SqlDbType.Date).Value = dateTimePicker1.Value;
            scCommand.Parameters.Add("@fe_fin_cierre", SqlDbType.Date).Value = dateTimePicker2.Value;
            scCommand.Parameters.Add("@id_hotel", SqlDbType.Int).Value = int.Parse(IDHotel);
            scCommand.Parameters.Add("@motivo", SqlDbType.VarChar, 100).Value = richTextBox1.Text;
            scCommand.Parameters.Add("@error", SqlDbType.TinyInt).Direction = ParameterDirection.Output;
            if (scCommand.Connection.State == ConnectionState.Closed)
            {
                scCommand.Connection.Open();
            }
            scCommand.ExecuteNonQuery();
            int result = int.Parse(scCommand.Parameters["@error"].Value.ToString());
            if (result == 1)
            {
                MessageBox.Show("Hotel cerrado correctamente", "Operacion finalizada", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (result == 3)
            {
                MessageBox.Show("El hotel ya tiene planeado un cierre para ese período", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conError = true;
            }
            if (result == 6)
            {
                MessageBox.Show("Existen reservas en el hotel para ese período", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conError = true;
            }
            if (!conError)
            {
                transaction.Commit();
            }
            else
            {
                transaction.Rollback();
            }
            con.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            back.Show();
            this.Close();
        }
    }
}
