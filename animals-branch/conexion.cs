using MySql.Data.MySqlClient;
using System;


namespace Animals
{
    public class Conexion
    {
        private MySqlConnection con;

        public Conexion()
        {

        }

        public MySqlConnection connectBd()
        {
            string connectionString = "Server=localhost;Port=3306;Database=animeaux;Uid=root;Pwd=;";
            con = new MySqlConnection(connectionString);

            try
            {
                con.Open();
                Console.WriteLine("conexión avec succes");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Imposible conexión a la base de dones " + ex.Message);
            }

            return con;
        }
    }
}

