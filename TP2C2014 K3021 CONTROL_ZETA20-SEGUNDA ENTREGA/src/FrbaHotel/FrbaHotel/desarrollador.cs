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

namespace FrbaHotel
{
    public partial class desarrollador : Form
    {
        Form back = null;
        public desarrollador(Form atras)
        {
            InitializeComponent();
            back = atras;
        }

        private void desarrollador_Load(object sender, EventArgs e)
        {
            actualizar();
        }

        private void actualizar()
        {
            int año = int.Parse(ConfigurationManager.AppSettings["Año"]);
            int mes = int.Parse(ConfigurationManager.AppSettings["Mes"]);
            int dia = int.Parse(ConfigurationManager.AppSettings["Dia"]);
            dateTimePicker1.Value = new DateTime(año, mes, dia);
            dateTimePicker2.Value = new DateTime(año, mes, dia);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ConfigurationManager.AppSettings.Set("Año", dateTimePicker2.Value.Year.ToString());
            ConfigurationManager.AppSettings.Set("Mes", dateTimePicker2.Value.Month.ToString());
            ConfigurationManager.AppSettings.Set("Dia", dateTimePicker2.Value.Day.ToString());
            actualizar();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            back.Show();
            this.Close();
        }
    }
}
