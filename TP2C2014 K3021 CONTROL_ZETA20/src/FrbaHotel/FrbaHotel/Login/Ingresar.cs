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
using System.Security.Cryptography;  


namespace FrbaHotel.Login
{
    public partial class Ingresar : Form
    {
        FrbaHotel.MenuPrincipal back;
        public Ingresar(FrbaHotel.MenuPrincipal atras)
        {
            InitializeComponent();
            back = atras;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (logicaLogueo(textBox1.Text, textBox2.Text))
            {
                Form f = new Login.SeleccionRol(this, back, textBox1.Text);
                f.Show();
                this.Hide();
            }
        }
        private Boolean logicaLogueo(string user, string pass)
        {
            string contra = SHA256Encrypt(pass);
            string ConnStr = ConfigurationManager.AppSettings["stringConexion"];
            SqlConnection con = new SqlConnection(ConnStr);
            con.Open();
            SqlCommand scCommand = new SqlCommand("CONTROL_ZETA.LOGIN_USR", con);
            scCommand.CommandType = CommandType.StoredProcedure;
            scCommand.Parameters.Add("@usr", SqlDbType.VarChar, 50).Value = user;
            scCommand.Parameters.Add("@pass", SqlDbType.VarChar, 100).Value = contra;
            scCommand.Parameters.Add("@error", SqlDbType.TinyInt).Direction = ParameterDirection.Output;
            if (scCommand.Connection.State == ConnectionState.Closed)
            {
                scCommand.Connection.Open();
            }
            scCommand.ExecuteNonQuery();
            int result = int.Parse(scCommand.Parameters["@error"].Value.ToString());
            con.Close();
            bool valor = false;
            switch (result)
            {
                case 1:
                    {
                        valor = true;
                        break;
                    }
                case 2:
                    {
                        string mensaje = string.Format("No existe el usuario");
                        MessageBox.Show(mensaje, "Usuario inexistente", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }
                case 4:
                    {
                        string mensaje = string.Format("Usuario inhabilitado por el administrador");
                        MessageBox.Show(mensaje, "Usuario inhabilitado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }
                case 5:
                    {
                        string mensaje = string.Format("Numero de intentos fallidos mayor a 3");
                        MessageBox.Show(mensaje, "Demasiados intentos fallidos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }
                case 6:
                    {
                        int cantidadRestante = 3 - intentos(user);
                        string mensaje = string.Format("Contraseña incorrecta, intente nuevamente, le quedan {0} intentos", cantidadRestante);
                        MessageBox.Show(mensaje, "Contraseña incorrecta", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }
                default:
                    {
                        break;
                    }

            }
            return valor;
        }

        private int intentos(string user)
        {
            string ConnStr2 = ConfigurationManager.AppSettings["stringConexion"];
            SqlConnection conn2 = new SqlConnection(ConnStr2);
            string sSel2 = string.Format(@"SELECT * FROM [GD2C2014].[CONTROL_ZETA].[USUARIO]  
                    where USR_USERNAME = '{0}'", user);
            SqlCommand cmd2 = new SqlCommand(sSel2, conn2);
            conn2.Open();
            SqlDataReader reader2 = cmd2.ExecuteReader();
            int cantidad = 0;
            while (reader2.Read())
            {
                cantidad = int.Parse(reader2["USR_INTENTOS"].ToString());
            }
            reader2.Close();
            conn2.Close();
            return cantidad;
        }

        public string SHA256Encrypt(string input)
        {
            SHA256CryptoServiceProvider provider = new SHA256CryptoServiceProvider();

            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hashedBytes = provider.ComputeHash(inputBytes);

            StringBuilder output = new StringBuilder();

            for (int i = 0; i < hashedBytes.Length; i++)
                output.Append(hashedBytes[i].ToString("x2").ToLower());

            return output.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            back.Show();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            this.AcceptButton = button1;
        }
    }
}
