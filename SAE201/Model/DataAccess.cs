using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SAE201.Model
{
    public class DataAccess
    {
        private static DataAccess instance;
        public static string connectionString;
        private NpgsqlConnection connection;

        public static DataAccess Instance
        {
            get
            {
                return instance;
            }
        }

        public DataAccess(string user, string password)
        {
            connectionString = $"Host=srv-peda-new;Port=5433;Username={user};Password={password};Database=sae201_Nicolas;Options='-c search_path=sae201_nicolas'";
            try
            {
                connection = new NpgsqlConnection(connectionString);
                instance = this;
                GetConnection();
            }
            catch (Exception ex)
            {
                LogError.Log(ex, "Pb de connexion GetConnection \n" + connectionString);
                throw;
            }
        }

        public NpgsqlConnection GetConnection()
        {
            if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
            {
                try
                {
                    connection.Open();
                }
                catch
                {
                    MessageBox.Show("Mauvais mot de passe ou nom d'utilisateur", "Probleme de connection");
                }
            }
            return connection;
        }

        public DataTable ExecuteSelect(NpgsqlCommand cmd)
        {
            DataTable dataTable = new DataTable();
            try
            {
                cmd.Connection = GetConnection();
                using (var adapter = new NpgsqlDataAdapter(cmd))
                {
                    adapter.Fill(dataTable);
                }
            }
            catch (Exception ex)
            {
                LogError.Log(ex, "Erreur SQL");
                throw;
            }
            return dataTable;
        }

        public int ExecuteInsert(NpgsqlCommand cmd)
        {
            int nb = 0;
            try
            {
                cmd.Connection = GetConnection();
                nb = (int)cmd.ExecuteScalar();

            }
            catch (Exception ex)
            {
                LogError.Log(ex, "Pb avec une requete insert " + cmd.CommandText);
                throw;
            }
            return nb;
        }

        public int ExecuteSet(NpgsqlCommand cmd)
        {
            int nb = 0;
            try
            {
                cmd.Connection = GetConnection();
                nb = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                LogError.Log(ex, "Pb avec une requete set " + cmd.CommandText);
                throw;
            }
            return nb;

        }

        public object ExecuteSelectUneValeur(NpgsqlCommand cmd)
        {
            object res = null;
            try
            {
                cmd.Connection = GetConnection();
                res = cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                LogError.Log(ex, "Pb avec une requete select " + cmd.CommandText);
                throw;
            }
            return res;

        }

        public void CloseConnection()
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }
    }
}

