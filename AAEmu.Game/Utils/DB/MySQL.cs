using System;
using AAEmu.Game.Models;
using MySql.Data.MySqlClient;
using NLog;

namespace AAEmu.Game.Utils.DB
{
    public static class MySQL
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private static readonly string ConnectionString;

        static MySQL()
        {
            var config = AppConfiguration.Instance.Connections.MySQLProvider;
            ConnectionString =
                $"server={config.Host};port={config.Port};user={config.User};password={config.Password};database={config.Database};Pooling=true;Min Pool Size=0;Max Pool Size=10;Connection Lifetime=600;charset=utf8;Allow Zero Datetime=true;Convert Zero Datetime=true;default command timeout=180;SslMode=Preferred";
        }

        public static MySqlConnection CreateConnection()
        {
            var connection = new MySqlConnection(ConnectionString);
            try
            {
                connection.Open();
            }
            catch (Exception e)
            {
                Log.Fatal(e, "Error on DB connect\n");
                return null;
            }

            return connection;
        }

        public static void Close(MySqlConnection connection)
        {
            connection.Close();
        }

        public static object GetValue(this MySqlDataReader reader, string column)
            => reader.GetValue(reader.GetOrdinal(column));
    }
}
