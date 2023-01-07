using System;
using System.Data.SqlClient;
using Npgsql;

namespace ChatZorro.Helpers
{
    internal class DataBaseConnection : Services.Configs
    {        
        static NpgsqlConnection Connection { get; set; }
        internal DataBaseConnection()
        {
            base.Load();
            Connection = new NpgsqlConnection();
            NewConnection();
        }

        private void NewConnection()
        {
            Connection = new NpgsqlConnection(base.GetConnectionString());
        }

        internal void CloseConnection()
        {
            if (Connection != null)
                Connection.Close();
        }

        internal NpgsqlCommand NewCommand(string queryString)
        {
            if (Connection == null)
            {
                throw new Exception("Connection does not exists.");
            }

            NpgsqlCommand command = new NpgsqlCommand(queryString, Connection);
            command.Connection.Open();
            return command;
        }

        internal int ExecuteCommand(string queryString)
        {
            try
            {
                NpgsqlCommand command = NewCommand(queryString);
                int state = command.ExecuteNonQuery();
                return state;

                //SqlDataReader reader = command.ExecuteReader();
                //while (reader.Read())
                //{
                //    Console.WriteLine(String.Format("{0}", reader[0]));
                //}

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (Connection != null)
                    Connection.Close();
            }
        }
    }
}
