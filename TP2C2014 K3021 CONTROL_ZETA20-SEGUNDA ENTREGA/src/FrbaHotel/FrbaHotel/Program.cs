using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace FrbaHotel
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        

        static bool[] logicaModo()
        {
            string ConnStr = ConfigurationManager.AppSettings["stringConexion"];
            SqlConnection conn = new SqlConnection(ConnStr);
            string sSel = string.Format(@"SELECT * FROM [GD2C2014].[CONTROL_ZETA].[ROL] rol, 
            [GD2C2014].[CONTROL_ZETA].[FUNCIONALIDAD] fun,
            [GD2C2014].[CONTROL_ZETA].[ROL_FUNC] rolfun
            where rol.ROL_NOMBRE = 'GUEST' 
            and rol.ROL_ID = rolfun.ROL_ID
            and rolfun.FUNC_ID = fun.FUNC_ID");
            SqlCommand cmd = new SqlCommand(sSel, conn);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            int i = 0;
            while (reader.Read())
            {
                i++;
            }
            reader.Close();
            reader = cmd.ExecuteReader();
            bool[] func = new bool[14];
            while (reader.Read())
            {
                func[int.Parse(reader["FUNC_ID"].ToString()) - 1] = true;
            }
            reader.Close();
            conn.Close();
            return func;
        }

        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


            Application.Run(new MenuPrincipal(logicaModo()));
        }
    }
}
