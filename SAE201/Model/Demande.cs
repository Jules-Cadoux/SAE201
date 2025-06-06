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
    public class Demande
    {
        private int numDemande;
        private Vin numVin;
        private Employe numEmploye;
        private Commande numCommande;
        private Client numClient;
        private DateTime dateDemande;
        private int quantiteDemande;
        private string accepter;

        public Demande(int numDemande, Vin numVin, Employe numEmploye, Commande numCommande, Client numClient, DateTime dateDemande, int quantiteDemande, string accepter)
        {
            this.NumDemande = numDemande;
            this.NumVin = numVin;
            this.NumEmploye = numEmploye;
            this.NumCommande = numCommande;
            this.NumClient = numClient;
            this.DateDemande = dateDemande;
            this.QuantiteDemande = quantiteDemande;
            this.Accepter = accepter;
        }

        public Demande(int numDemande, Vin numVin, Employe numEmploye, Commande numCommande, Client numClient)
        {
            this.NumDemande = numDemande;
            this.NumVin = numVin;
            this.NumEmploye = numEmploye;
            this.NumCommande = numCommande;
            this.NumClient = numClient;
        }

        public Demande(){ }


        public int NumDemande
        {
            get
            {
                return numDemande;
            }

            set
            {
                numDemande = value;
            }
        }

        public Vin NumVin
        {
            get
            {
                return this.numVin;
            }

            set
            {
                this.numVin = value;
            }
        }

        public Employe NumEmploye
        {
            get
            {
                return this.numEmploye;
            }

            set
            {
                this.numEmploye = value;
            }
        }

        public Commande NumCommande
        {
            get
            {
                return this.numCommande;
            }

            set
            {
                this.numCommande = value;
            }
        }

        public Client NumClient
        {
            get
            {
                return this.numClient;
            }

            set
            {
                this.numClient = value;
            }
        }

        public DateTime DateDemande
        {
            get
            {
                return dateDemande;
            }

            set
            {
                if (value != DateTime.Today)
                    MessageBox.Show("La date du jour doit = à celle du jour", "Erreur de création demande", MessageBoxButton.OK, MessageBoxImage.Error);
                dateDemande = value;
            }
        }

        public int QuantiteDemande
        {
            get
            {
                return quantiteDemande;
            }

            set
            {
                quantiteDemande = value;
            }
        }

        public string Accepter
        {
            get
            {
                return this.accepter;
            }

            set
            {
                if (value != "Accepter" && value != "En cours" && value != "Refuser")
                    MessageBox.Show("Il faut choisir Accepter, En cours ou Refuser", "Erreur demande", MessageBoxButton.OK, MessageBoxImage.Error);
                this.accepter = value;
            }
        }

        public int Create()
        {
            int id = 0;
            using (NpgsqlCommand cmd = new NpgsqlCommand(
                "INSERT INTO demande (numvin, numemploye, numcommande, numclient, datedemande, quantitedemande, accepter) " +
                "VALUES (@numvin, @numemploye, @numcommande, @numclient, @datedemande, @quantitedemande, @accepter) RETURNING numdemande"))
            {
                cmd.Parameters.AddWithValue("numvin", this.NumVin.NumVin);
                cmd.Parameters.AddWithValue("numemploye", this.NumEmploye.NumEmploye);
                cmd.Parameters.AddWithValue("numcommande", this.NumCommande.NumCommande);
                cmd.Parameters.AddWithValue("numclient", this.NumClient.NumClient);
                cmd.Parameters.AddWithValue("datedemande", this.DateDemande);
                cmd.Parameters.AddWithValue("quantitedemande", this.QuantiteDemande);
                cmd.Parameters.AddWithValue("accepter", this.Accepter);
                id = DataAccess.Instance.ExecuteInsert(cmd);
                this.NumDemande = id;
            }
            return id;
        }

        public void Read()
        {
            using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM demande WHERE numdemande = @numdemande"))
            {
                cmd.Parameters.AddWithValue("id", this.NumDemande);
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmd);
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    this.DateDemande = (DateTime)row["datedemande"];
                    this.QuantiteDemande = (int)row["quantitedemande"];
                    this.Accepter = (string)row["accepter"];

                    int idVin = (int)row["numvin"];
                    int idEmploye = (int)row["numemploye"];
                    int idCommande = (int)row["numcommande"];
                    int idClient = (int)row["numclient"];

                    this.NumVin = new Vin { NumVin = idVin }; // à remplacer par vin.Read() si disponible
                    this.NumEmploye = new Employe { NumEmploye = idEmploye }; // idem
                    this.NumCommande = new Commande { NumCommande = idCommande };
                    this.NumClient = new Client { NumClient = idClient };
                }
            }
        }

        public int Update()
        {
            using (NpgsqlCommand cmd = new NpgsqlCommand(
                "UPDATE demande SET numvin = @numvin, numemploye = @numemploye, numcommande = @numcommande, numclient = @numclient, " +
                "datedemande = @datedemande, quantitedemande = @quantitedemande, accepter = @accepter WHERE numdemande = @numdemande"))
            {
                cmd.Parameters.AddWithValue("numvin", this.NumVin.NumVin);
                cmd.Parameters.AddWithValue("numemploye", this.NumEmploye.NumEmploye);
                cmd.Parameters.AddWithValue("numcommande", this.NumCommande.NumCommande);
                cmd.Parameters.AddWithValue("numclient", this.NumClient.NumClient);
                cmd.Parameters.AddWithValue("datedemande", this.DateDemande);
                cmd.Parameters.AddWithValue("quantitedemande", this.QuantiteDemande);
                cmd.Parameters.AddWithValue("accepter", this.Accepter);
                cmd.Parameters.AddWithValue("numdemande", this.NumDemande);
                return DataAccess.Instance.ExecuteSet(cmd);
            }
        }

        public int Delete()
        {
            using (NpgsqlCommand cmd = new NpgsqlCommand("DELETE FROM demande WHERE numdemande = @numdemande"))
            {
                cmd.Parameters.AddWithValue("numdemande", this.NumDemande);
                return DataAccess.Instance.ExecuteSet(cmd);
            }
        }

        public static List<Demande> FindAll()
        {
            List<Demande> demandes = new List<Demande>();
            using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM demande"))
            {
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmd);
                foreach (DataRow row in dt.Rows)
                {
                    Demande d = new Demande();
                    d.NumDemande = (int)row["numdemande"];
                    d.DateDemande = (DateTime)row["datedemande"];
                    d.QuantiteDemande = (int)row["quantitedemande"];
                    d.Accepter = (string)row["accepter"];

                    d.NumVin = new Vin { NumVin = (int)row["numvin"] };
                    d.NumEmploye = new Employe { NumEmploye = (int)row["numemploye"] };
                    d.NumCommande = new Commande { NumCommande = (int)row["numcommande"] };
                    d.NumClient = new Client { NumClient = (int)row["numclient"] };

                    demandes.Add(d);
                }
            }
            return demandes;
        }

        public static List<Demande> FindBySelection(string criteres)
        {
            List<Demande> demandes = new List<Demande>();
            using (NpgsqlCommand cmd = new NpgsqlCommand($"SELECT * FROM demande WHERE {criteres}"))
            {
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmd);
                foreach (DataRow row in dt.Rows)
                {
                    Demande d = new Demande();
                    d.NumDemande = (int)row["numdemande"];
                    d.DateDemande = (DateTime)row["datedemande"];
                    d.QuantiteDemande = (int)row["quantitedemande"];
                    d.Accepter = (string)row["accepter"];

                    d.NumVin = new Vin { NumVin = (int)row["numvin"] };
                    d.NumEmploye = new Employe { NumEmploye = (int)row["numemploye"] };
                    d.NumCommande = new Commande { NumCommande = (int)row["numcommande"] };
                    d.NumClient = new Client { NumClient = (int)row["numclient"] };

                    demandes.Add(d);
                }
            }
            return demandes;
        }

        public override bool Equals(object? obj)
        {
            return obj is Demande demande && this.NumDemande == demande.NumDemande;
        }    
    }
}
