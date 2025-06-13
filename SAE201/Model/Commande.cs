using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE201.Model
{
    public class Commande
    {
        private int numCommande;
        private int numEmploye;
        private DateTime dateCommande;
        private bool valider;
        private double prixTotal;

        public string NomFournisseurPrincipal { get; set; }

        public Commande()
        {
            NomFournisseurPrincipal = string.Empty;
        }

        public Commande(int numEmploye, DateTime dateCommande, bool valider, double prixTotal)
        {
            this.NumEmploye = numEmploye;
            this.DateCommande = dateCommande;
            this.Valider = valider;
            this.PrixTotal = prixTotal;
            this.NomFournisseurPrincipal = string.Empty;
        }

        public Commande(int numCommande, int numEmploye, DateTime dateCommande, bool valider, double prixTotal)
        {
            this.NumCommande = numCommande;
            this.NumEmploye = numEmploye;
            this.DateCommande = dateCommande;
            this.Valider = valider;
            this.PrixTotal = prixTotal;
            this.NomFournisseurPrincipal = string.Empty;
        }

        public int NumCommande
        {
            get { return numCommande; }
            set { numCommande = value; }
        }

        public int NumEmploye
        {
            get { return this.numEmploye; }
            set { this.numEmploye = value; }
        }

        public DateTime DateCommande
        {
            get { return dateCommande; }
            set { dateCommande = value; }
        }

        public bool Valider
        {
            get { return valider; }
            set { valider = value; }
        }

        public double PrixTotal
        {
            get { return this.prixTotal; }
            set { this.prixTotal = value; }
        }

        public override bool Equals(object? obj)
        {
            return obj is Commande commande &&
                   this.NumCommande == commande.NumCommande;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.NumCommande);
        }

        public int Create()
        {
            int id = 0;
            using (NpgsqlCommand cmd = new NpgsqlCommand(
                "INSERT INTO commande (numemploye, datecommande, valider, prixtotal) " +
                "VALUES (@numemploye, @datecommande, @valider, @prixtotal) RETURNING numcommande"))
            {
                cmd.Parameters.AddWithValue("numemploye", this.NumEmploye);
                cmd.Parameters.AddWithValue("datecommande", this.DateCommande);
                cmd.Parameters.AddWithValue("valider", this.Valider);
                cmd.Parameters.AddWithValue("prixtotal", this.PrixTotal);
                id = DataAccess.Instance.ExecuteInsert(cmd);
                this.NumCommande = id;
            }
            return id;
        }

        public void Read()
        {
            using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM commande WHERE numcommande = @id"))
            {
                cmd.Parameters.AddWithValue("id", this.NumCommande);
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmd);
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    this.DateCommande = (DateTime)row["datecommande"];
                    this.Valider = (bool)row["valider"];
                    this.PrixTotal = Convert.ToDouble(row["prixtotal"]);
                    this.NumEmploye = (int)row["numemploye"];
                }
            }
        }

        public int Update()
        {
            using (NpgsqlCommand cmd = new NpgsqlCommand(
                "UPDATE commande SET numemploye = @numemploye, datecommande = @datecommande, valider = @valider, prixtotal = @prixtotal " +
                "WHERE numcommande = @numcommande"))
            {
                cmd.Parameters.AddWithValue("numemploye", this.NumEmploye);
                cmd.Parameters.AddWithValue("datecommande", this.DateCommande);
                cmd.Parameters.AddWithValue("valider", this.Valider);
                cmd.Parameters.AddWithValue("prixtotal", this.PrixTotal);
                cmd.Parameters.AddWithValue("numcommande", this.NumCommande);
                return DataAccess.Instance.ExecuteSet(cmd);
            }
        }

        public int Delete()
        {
            using (NpgsqlCommand cmd = new NpgsqlCommand("DELETE FROM commande WHERE numcommande = @numcommande"))
            {
                cmd.Parameters.AddWithValue("numcommande", this.NumCommande);
                return DataAccess.Instance.ExecuteSet(cmd);
            }
        }

        public static List<Commande> FindAll()
        {
            List<Commande> commandes = new List<Commande>();
            string query = @"
                SELECT c.*, f.nomfournisseur
                FROM sae201_nicolas.commande c
                LEFT JOIN (
                    SELECT DISTINCT ON (d.numcommande) d.numcommande, v.numfournisseur
                    FROM sae201_nicolas.demande d
                    JOIN sae201_nicolas.vin v ON d.numvin = v.numvin
                    WHERE d.numcommande IS NOT NULL
                ) AS df ON c.numcommande = df.numcommande
                LEFT JOIN sae201_nicolas.fournisseur f ON df.numfournisseur = f.numfournisseur
                ORDER BY c.datecommande DESC";

            using (NpgsqlCommand cmd = new NpgsqlCommand(query))
            {
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmd);
                foreach (DataRow row in dt.Rows)
                {
                    Commande commande = new Commande
                    {
                        NumCommande = (int)row["numcommande"],
                        NumEmploye = (int)row["numemploye"],
                        DateCommande = (DateTime)row["datecommande"],
                        Valider = (bool)row["valider"],
                        PrixTotal = Convert.ToDouble(row["prixtotal"]),
                        NomFournisseurPrincipal = row["nomfournisseur"] == DBNull.Value ? "N/A" : (string)row["nomfournisseur"]
                    };
                    commandes.Add(commande);
                }
            }
            return commandes;
        }

        public static List<Commande> FindBySelection(string criteres)
        {
            List<Commande> commandes = new List<Commande>();
            using (NpgsqlCommand cmd = new NpgsqlCommand($"SELECT * FROM commande WHERE {criteres}"))
            {
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmd);
                foreach (DataRow row in dt.Rows)
                {
                    int id = (int)row["numcommande"];
                    int numEmploye = (int)row["numemploye"];
                    DateTime date = (DateTime)row["datecommande"];
                    bool valider = (bool)row["valider"];
                    double prix = Convert.ToDouble(row["prixtotal"]);
                    commandes.Add(new Commande(id, numEmploye, date, valider, prix));
                }
            }
            return commandes;
        }
    }
}