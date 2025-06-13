using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SAE201.Model
{
    public class DetailCommande
    {
        private int numCommande;
        private int numVin;
        private int quantite;
        private double prix;
        private string etatCommande;

        public DetailCommande(int numCommande, int numVin, int quantite, double prix)
        {
            this.NumCommande = numCommande;
            this.NumVin = numVin;
            this.Quantite = quantite;
            this.Prix = prix;
        }

        public DetailCommande()
        {

        }
        public int NumCommande
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

        public int NumVin
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

        public int Quantite
        {
            get
            {
                return this.quantite;
            }

            set
            {
                this.quantite = value;
            }
        }

        public double Prix
        {
            get
            {
                return this.prix;
            }

            set
            {
                this.prix = value;
            }
        }

        public string EtatCommande
        {
            get
            {
                return this.etatCommande;
            }
            set
            {
                if (value != "Commandé" && value != "En livraison" && value != "Reçu" && value != null)
                    MessageBox.Show("L'état commande doit être Commandé, En livraison ou Reçu", "Erreur etat commande", MessageBoxButton.OK, MessageBoxImage.Error);
                this.etatCommande = value;
            }
        }

        public override bool Equals(object? obj)
        {
            return obj is DetailCommande commande &&
                   this.NumCommande == commande.NumCommande &&
                   this.NumVin == commande.NumVin;
        }


        public int Create()
        {
            using (NpgsqlCommand cmd = new NpgsqlCommand("INSERT INTO sae201_nicolas.detailcommande (numcommande, numvin, quantite, prix) VALUES (@numcommande, @numvin, @quantite, @prix)"))
            {
                cmd.Parameters.AddWithValue("numcommande", this.NumCommande);
                cmd.Parameters.AddWithValue("numvin", this.NumVin);
                cmd.Parameters.AddWithValue("quantite", this.Quantite);
                cmd.Parameters.AddWithValue("prix", this.Prix);
                // Since there is no auto-incrementing ID, we use ExecuteSet.
                return DataAccess.Instance.ExecuteSet(cmd);
            }
        }


        public void Read()
        {
            using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM sae201_nicolas.detailcommande WHERE numcommande = @numcommande AND numvin = @numvin"))
            {
                cmd.Parameters.AddWithValue("numcommande", this.NumCommande);
                cmd.Parameters.AddWithValue("numvin", this.NumVin);
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmd);
                if (dt.Rows.Count > 0)
                {
                    this.Quantite = (int)dt.Rows[0]["quantite"];
                    this.Prix = Convert.ToDouble(dt.Rows[0]["prix"]);
                }
            }
        }


        public int Update()
        {
            using (NpgsqlCommand cmd = new NpgsqlCommand("UPDATE sae201_nicolas.detailcommande SET quantite = @quantite, prix = @prix WHERE numcommande = @numcommande AND numvin = @numvin"))
            {
                cmd.Parameters.AddWithValue("quantite", this.Quantite);
                cmd.Parameters.AddWithValue("prix", this.Prix);
                cmd.Parameters.AddWithValue("numcommande", this.NumCommande);
                cmd.Parameters.AddWithValue("numvin", this.NumVin);
                return DataAccess.Instance.ExecuteSet(cmd);
            }
        }

        public int Delete()
        {
            using (NpgsqlCommand cmd = new NpgsqlCommand("DELETE FROM sae201_nicolas.detailcommande WHERE numcommande = @numcommande AND numvin = @numvin"))
            {
                cmd.Parameters.AddWithValue("numcommande", this.NumCommande);
                cmd.Parameters.AddWithValue("numvin", this.NumVin);
                return DataAccess.Instance.ExecuteSet(cmd);
            }
        }

        public static List<DetailCommande> FindAll()
        {
            List<DetailCommande> lesDetails = new List<DetailCommande>();
            using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM sae201_nicolas.detailcommande"))
            {
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmd);
                foreach (DataRow row in dt.Rows)
                {
                    DetailCommande detail = new DetailCommande
                    {
                        NumCommande = (int)row["numcommande"],
                        NumVin = (int)row["numvin"],
                        Quantite = (int)row["quantite"],
                        Prix = Convert.ToDouble(row["prix"])
                    };
                    lesDetails.Add(detail);
                }
            }
            return lesDetails;
        }
    }
}