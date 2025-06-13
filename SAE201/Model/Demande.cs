using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SAE201.Model
{
    public class Demande : INotifyPropertyChanged
    {
        private int numDemande;
        private Vin numVin;
        private Employe numEmploye;
        private Commande numCommande;
        private Client numClient;
        private DateTime dateDemande;
        private int quantiteDemande;
        private string accepter;
        private double prixLigne;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string nomPropriete)
        {
            PropertyChangedEventHandler? handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(nomPropriete));
            }
        }
        public Vin Vin { get;set; }
        public Employe Employe { get; set; }

        public Demande(Vin numVin, Employe numEmploye,Client numClient, DateTime dateDemande, int quantiteDemande, string accepter)
        {
            this.NumVin = numVin;
            this.NumEmploye = numEmploye;
            this.NumClient = numClient;
            this.DateDemande = dateDemande;
            this.QuantiteDemande = quantiteDemande;
            this.Accepter = accepter;
        }

        public Demande(Vin numVin, Employe numEmploye, Client numClient)
        {
            this.NumVin = numVin;
            this.NumEmploye = numEmploye;
            this.NumClient = numClient;
        }

        public Demande(string accepter)
        {
            this.Accepter = accepter;
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
                if (value != "Accepter" && value != "En Attente" && value != "Refuser")
                {
                    MessageBox.Show("Il faut choisir Accepter, En Attente ou Refuser", "Erreur demande", MessageBoxButton.OK, MessageBoxImage.Error);
                    //throw new ArgumentException("Le statut doit être Accepter, En Attente ou Refuser.");
                } 
                this.accepter = value;
                value = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value.ToLower());
                OnPropertyChanged(nameof(Accepter));
            }
        }

        public double PrixLigne
        {
            get
            {
                if (this.NumVin != null)
                {
                    return this.NumVin.PrixVin * this.QuantiteDemande;
                }
                return 0;
            }
        }

        public int Create()
        {
            int id = 0;
            using (NpgsqlCommand cmd = new NpgsqlCommand(
                "INSERT INTO sae201_nicolas.demande (numvin, numemploye, numcommande, numclient, datedemande, quantitedemande, accepter) " +
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
            using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM sae201_nicolas.demande WHERE numdemande = @numdemande"))
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

                    this.NumVin = new Vin { NumVin = idVin }; 
                    this.NumEmploye = new Employe { NumEmploye = idEmploye }; 
                    this.NumCommande = new Commande { NumCommande = idCommande };
                    this.NumClient = new Client { NumClient = idClient };
                }
            }
        }

        public int Update()
        {
            using (NpgsqlCommand cmd = new NpgsqlCommand("UPDATE sae201_nicolas.demande SET accepter = @accepter WHERE numdemande = @numdemande"))
            {
                cmd.Parameters.AddWithValue("accepter", this.Accepter);
                cmd.Parameters.AddWithValue("numdemande", this.NumDemande);
                return DataAccess.Instance.ExecuteSet(cmd);
            }
        }

        public int Delete()
        {
            using (NpgsqlCommand cmd = new NpgsqlCommand("DELETE FROM sae201_nicolas.demande WHERE numdemande = @numdemande"))
            {
                cmd.Parameters.AddWithValue("numdemande", this.NumDemande);
                return DataAccess.Instance.ExecuteSet(cmd);
            }
        }


        public static List<Demande> FindAll()
        {
            List<Demande> demandes = new List<Demande>();

            using (NpgsqlCommand cmd = new NpgsqlCommand(@"
        SELECT 
            d.numdemande, d.datedemande, d.quantitedemande, d.accepter, 
            d.numvin, v.nomvin, v.numfournisseur, v.prixvin,
            d.numemploye, e.nom AS nomemploye,
            d.numcommande,
            d.numclient,
            f.nomfournisseur
        FROM sae201_nicolas.demande d
        JOIN sae201_nicolas.vin v ON v.numvin = d.numvin
        JOIN sae201_nicolas.fournisseur f ON f.numfournisseur = v.numfournisseur
        JOIN sae201_nicolas.employe e ON e.numemploye = d.numemploye
        Order by d.datedemande DESC;
    "))
            {
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmd);
                foreach (DataRow row in dt.Rows)
                {
                    Demande d = new Demande
                    {
                        NumDemande = (int)row["numdemande"],
                        DateDemande = (DateTime)row["datedemande"],
                        QuantiteDemande = (int)row["quantitedemande"],
                        Accepter = (string)row["accepter"],

                        NumVin = new Vin
                        {
                            NumVin = (int)row["numvin"],
                            NomVin = (string)row["nomvin"],
                            PrixVin = Convert.ToDouble(row["prixvin"]),  
                            NumFournisseur = new Fournisseur
                            {
                                NumFournisseur = (int)row["numfournisseur"],
                                NomFournisseur = (string)row["nomfournisseur"]
                            }
                        },

                        NumEmploye = new Employe
                        {
                            NumEmploye = (int)row["numemploye"],
                            Nom = (string)row["nomemploye"]
                        },

                        NumCommande = row["numcommande"] == DBNull.Value
                            ? null
                            : new Commande { NumCommande = (int)row["numcommande"] },

                        NumClient = new Client
                        {
                            NumClient = (int)row["numclient"]
                        }
                    };

                    demandes.Add(d);
                }
            }

            return demandes;
        }





        public static List<Demande> FindBySelection(string criteres)
        {
            List<Demande> demandes = new List<Demande>();
            using (NpgsqlCommand cmd = new NpgsqlCommand($"SELECT * FROM sae201_nicolas.demande WHERE {criteres}"))
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
        public int UpdateCommande()
        {
            using (NpgsqlCommand cmd = new NpgsqlCommand("UPDATE sae201_nicolas.demande SET numcommande = @numcommande WHERE numdemande = @numdemande"))
            {
                if (this.NumCommande != null)
                {
                    cmd.Parameters.AddWithValue("numcommande", this.NumCommande.NumCommande);
                }
                else
                {
                    cmd.Parameters.AddWithValue("numcommande", DBNull.Value);
                }
                cmd.Parameters.AddWithValue("numdemande", this.NumDemande);
                return DataAccess.Instance.ExecuteSet(cmd);
            }
        }

        public override bool Equals(object? obj)    
        {
            return obj is Demande demande && this.NumDemande == demande.NumDemande;
        }    
    }
}
