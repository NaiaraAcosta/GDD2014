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

namespace FrbaHotel.Reservas_Inconsistentes
{
    public partial class ReservasInconsistentes : Form
    {
        Form back = null;
        public ReservasInconsistentes(Form atras)
        {
            InitializeComponent();
            back = atras;
        }

        private void ReservasInconsistentes_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                string sCnn = ConfigurationManager.AppSettings["stringConexion"];
                string sSel = String.Format(@"select R.RESERVA_ID, R.RESERVA_FECHA_INICIO, R.RESERVA_FECHA_HASTA,R.CLIENTE_ID, F.FACTURA_NRO, C.CLIENTE_NOMBRE,C.CLIENTE_APELLIDO 
                                            from CONTROL_ZETA.RESERVA R, CONTROL_ZETA.FACTURA F, CONTROL_ZETA.ESTADIA E, CONTROL_ZETA.CLIENTE C
                                            WHERE R.RESERVA_ID=E.EST_RESERVA_ID
                        AND F.EST_ID=E.EST_ID
                        AND C.CLIENTE_ID=R.CLIENTE_ID
                        AND R.RESERVA_ESTADO='RINC'");
                SqlDataAdapter da;
                DataTable dt = new DataTable();
                try
                {
                    da = new SqlDataAdapter(sSel, sCnn);
                    da.Fill(dt);
                    this.dataGridView1.DataSource = dt;
                    label2.Text = String.Format("Total de resultados: {0}", dt.Rows.Count);
                }
                catch (Exception ex)
                {
                    label2.Text = "Error: " + ex.Message;
                }
            }
            else
            {
                string sCnn = ConfigurationManager.AppSettings["stringConexion"];
                string sSel = String.Format(@"select  R.RESERVA_ID, R.RESERVA_FECHA_INICIO, R.RESERVA_FECHA_HASTA,R.CLIENTE_ID, C.CLIENTE_NOMBRE,C.CLIENTE_APELLIDO 
                    from CONTROL_ZETA.RESERVA R,  CONTROL_ZETA.CLIENTE C
                    WHERE C.CLIENTE_ID=R.CLIENTE_ID
                    AND R.RESERVA_ESTADO='RSF' 
                    group by R.RESERVA_ID, R.RESERVA_FECHA_INICIO, R.RESERVA_FECHA_HASTA,R.CLIENTE_ID, C.CLIENTE_NOMBRE,C.CLIENTE_APELLIDO ");
                SqlDataAdapter da;
                DataTable dt = new DataTable();
                try
                {
                    da = new SqlDataAdapter(sSel, sCnn);
                    da.Fill(dt);
                    this.dataGridView1.DataSource = dt;
                    label2.Text = String.Format("Total de resultados: {0}", dt.Rows.Count);
                }
                catch (Exception ex)
                {
                    label2.Text = "Error: " + ex.Message;
                }
            }
        }
    }
}
