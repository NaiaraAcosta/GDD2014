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

namespace FrbaHotel.Listado_Estadistico
{
    public partial class Resultado : Form
    {
        Form back = null;
        DataTable ds;

        public Resultado(Form atras, int tipo, DataTable data, DateTime date)
        {
            InitializeComponent();
            back = atras;
            ds = data;
            dataGridView1.DataSource = ds;
            this.dateTimePicker1.Value = date;
            colocarTipo(tipo);
        }

        public void colocarTipo(int tipo)
        {
            switch (tipo)
            {
                case 1:
                    {
                        groupBox1.Text = "Busqueda por: Hoteles con mayor cantidad de reservas canceladas";
                        break;
                    }
                case 2:
                    {
                        groupBox1.Text = "Busqueda por: Hoteles con mayor cantidad de consumibles facturados";
                        break;
                    }
                case 3:
                    {
                        groupBox1.Text = "Busqueda por: Hoteles con mayor cantidad de días fuera de servicio";
                        break;
                    }
                case 4:
                    {
                        groupBox1.Text = "Busqueda por: Habitaciones con mayor cantidad de días y veces que fueron ocupadas";
                        break;
                    }
                case 5:
                    {
                        groupBox1.Text = "Busqueda por: Cliente con mayor cantidad de puntos";
                        break;
                    }
            }
        }
        private void Resultado_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            back.Show();
            this.Close();
        }
    }
}
