using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SAE201.Model
{
    public class Client : INotifyPropertyChanged
    {
        private int numClient;
        private string nomClient;
        private string prenomClient;
        private string mailClient;

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string nomPropriete)
        {
            PropertyChangedEventHandler? handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(nomPropriete));
            }
        }

        public Client(int numClient, string nomClient, string prenomClient, string mailClient)
        {
            this.NumClient = numClient;
            this.NomClient = nomClient;
            this.PrenomClient = prenomClient;
            this.MailClient = mailClient;
        }
        public Client(string nomClient, string prenomClient, string mailClient)
        {
            this.NomClient = nomClient;
            this.PrenomClient = prenomClient;
            this.MailClient = mailClient;
        }

        public Client(int numClient)
        {
            this.NumClient = numClient;

        }
        public Client(){ }

        public int NumClient
        {
            get
            {
                return numClient;
            }

            set
            {
                numClient = value;
            }
        }

        public string NomClient
        {
            get
            {
                return nomClient;
            }

            set
            {
                nomClient = value;
                this.nomClient = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value.ToLower());
                OnPropertyChanged(nameof(NomClient));
            }
        }

        public string PrenomClient
        {
            get
            {
                return prenomClient;
            }

            set
            {
                prenomClient = value;
                this.prenomClient = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value.ToLower());
                OnPropertyChanged(nameof(PrenomClient));

            }
        }

        public string MailClient
        {
            get
            {
                return this.mailClient;
            }

            set
            {
                if (!Regex.IsMatch(value, @"^[a-zA-Z0-9._-]+@gmail\.com$") && !Regex.IsMatch(value, @"^[a-zA-Z0-9._-]+@email\.com$"))
                    throw new FormatException("Mail correspond pas !");
                this.mailClient = value;
                OnPropertyChanged(nameof(MailClient));
            }
        }

        public int Create()
        {
            int nb = 0;
            using (NpgsqlCommand cmdInsert = new NpgsqlCommand("INSERT INTO sae201_nicolas.client (nomclient,prenomclient, mailclient) VALUES (@nomclient,@prenomclient, @mailclient) RETURNING numclient"))
            {
                cmdInsert.Parameters.AddWithValue("nomclient", this.NomClient);
                cmdInsert.Parameters.AddWithValue("prenomclient", this.PrenomClient);
                cmdInsert.Parameters.AddWithValue("mailclient", this.mailClient);
                nb = DataAccess.Instance.ExecuteInsert(cmdInsert);
            }
            this.NumClient = nb;
            return nb;
        }

        public void Read()
        {
            using (NpgsqlCommand cmdSelect = new NpgsqlCommand("SELECT * FROM sae201_nicolas.client WHERE numclient = @numclient"))
            {
                cmdSelect.Parameters.AddWithValue("numclient", this.numClient);
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                this.NumClient = (int)dt.Rows[0]["numclient"];
                this.NomClient = (string)dt.Rows[0]["nomclient"];
                this.PrenomClient = (string)dt.Rows[0]["prenomclient"];
                this.MailClient = (string)dt.Rows[0]["mailclient"];

            }
        }

        public int Update()
        {
            using (NpgsqlCommand cmdUpdate = new NpgsqlCommand("UPDATE sae201_nicolas.client SET nomclient = @nomclient, prenomclient = @prenomclient, mailclient = @mailclient WHERE numclient = @numclient"))
            {
                cmdUpdate.Parameters.AddWithValue("nomclient", this.NomClient);
                cmdUpdate.Parameters.AddWithValue("prenomclient", this.PrenomClient);
                cmdUpdate.Parameters.AddWithValue("mailclient", this.MailClient);
                cmdUpdate.Parameters.AddWithValue("numclient", this.NumClient);
                return DataAccess.Instance.ExecuteSet(cmdUpdate);
            }
        }

        public int Delete()
        {
            using (NpgsqlCommand cmdDelete = new NpgsqlCommand("DELETE FROM sae201_nicolas.client WHERE numclient = @numclient"))
            {
                cmdDelete.Parameters.AddWithValue("numclient", this.NumClient);
                return DataAccess.Instance.ExecuteSet(cmdDelete);
            }
        }


        public List<Client> FindAll()
        {
            List<Client> lesClients = new List<Client>();
            using (NpgsqlCommand cmdSelect = new NpgsqlCommand("select * from sae201_nicolas.client ;"))
            {
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                foreach (DataRow dr in dt.Rows)
                {
                    int numclient = (int)dr["numclient"];
                    string nomclient = (string)dr["nomclient"];
                    string prenomclient = (string)dr["prenomclient"];
                    string mailclient = (string)dr["mailclient"];
                    Client client = new Client(numclient, nomclient, prenomclient, mailclient);
                    lesClients.Add(client);
                }
                    
            }
            return lesClients;
        }

        public List<Client> FindBySelection(string criteres)
        {
            throw new NotImplementedException();
        }
    }
}
