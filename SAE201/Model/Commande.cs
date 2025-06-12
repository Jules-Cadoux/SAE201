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
        private string nomFournisseurPrincipal;

        public Commande()
        {
        }
        public Commande(int numEmploye, DateTime dateCommande, bool valider, double prixTotal)
        {

            this.NumEmploye = numEmploye;
            this.DateCommande = dateCommande;
            this.Valider = valider;
            this.PrixTotal = prixTotal;
        }


        public Commande(int numCommande, int numEmploye, DateTime dateCommande, bool valider, double prixTotal)
        {
            this.NumCommande = numCommande;
            this.NumEmploye = numEmploye;
            this.DateCommande = dateCommande;
            this.Valider = valider;
            this.PrixTotal = prixTotal;
        }


        public int NumCommande
        {
            get
            {
                return numCommande;
            }

            set
            {
                numCommande = value;
            }
        }

        public int NumEmploye
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

        public DateTime DateCommande
        {
            get
            {
                return dateCommande;
            }

            set
            {
                dateCommande = value;
            }
        }

        public bool Valider
        {
            get
            {
                return valider;
            }

            set
            {
                valider = value;
            }
        }

        public double PrixTotal
        {
            get
            {
                return this.prixTotal;
            }

            set
            {
                this.prixTotal = value;
            }
        }


        public string NomFournisseurPrincipal
        {
            get
            {
                try
                {
                    // Trouver la première demande liée à cette commande
                    Demande demande = Demande.FindAll().FirstOrDefault(d => d.NumCommande?.NumCommande == this.NumCommande);

                    if (demande != null && demande.NumVin?.NumFournisseur != null)
                    {
                        return demande.NumVin.NumFournisseur.NomFournisseur;
                    }
                    return "N/A";
                }
                catch
                {
                    return "N/A";
                }
            }
        }


        public override bool Equals(object? obj)
        {
            return obj is Commande commande &&
                   this.NumCommande == commande.NumCommande;
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
            using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM commande"))
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
