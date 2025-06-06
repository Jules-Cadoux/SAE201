using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE201.Model
{
    public class Fournisseur
    {
        private int numFournisseur;
        private string nomFournisseur;

        public Fournisseur()
        {
        }

        public Fournisseur(int numFournisseur, string nomFournisseur)
        {
            this.NumFournisseur = numFournisseur;
            this.NomFournisseur = nomFournisseur;
        }

        public int NumFournisseur
        {
            get
            {
                return this.numFournisseur;
            }

            set
            {
                this.numFournisseur = value;
            }
        }

        public string NomFournisseur
        {
            get
            {
                return this.nomFournisseur;
            }

            set
            {
                this.nomFournisseur = value;
            }
        }

        public int Create()
        {
            int id = 0;
            using (NpgsqlCommand cmd = new NpgsqlCommand("INSERT INTO fournisseur (nomfournisseur) VALUES (@nomfournisseur) RETURNING numfournisseur"))
            {
                cmd.Parameters.AddWithValue("nomfournisseur", this.NomFournisseur);
                id = DataAccess.Instance.ExecuteInsert(cmd);
                this.NumFournisseur = id;
            }
            return id;
        }

        public void Read()
        {
            using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM fournisseur WHERE numfournisseur = @id"))
            {
                cmd.Parameters.AddWithValue("id", this.NumFournisseur);
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmd);
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    this.NomFournisseur = (string)row["nomfournisseur"];
                }
            }
        }

        public int Update()
        {
            using (NpgsqlCommand cmd = new NpgsqlCommand("UPDATE fournisseur SET nomfournisseur = @nomfournisseur WHERE numfournisseur = @numfournisseur"))
            {
                cmd.Parameters.AddWithValue("nomfournisseur", this.NomFournisseur);
                cmd.Parameters.AddWithValue("numfournisseur", this.NumFournisseur);
                return DataAccess.Instance.ExecuteSet(cmd);
            }
        }

        public int Delete()
        {
            using (NpgsqlCommand cmd = new NpgsqlCommand("DELETE FROM fournisseur WHERE numfournisseur = @numfournisseur"))
            {
                cmd.Parameters.AddWithValue("numfournisseur", this.NumFournisseur);
                return DataAccess.Instance.ExecuteSet(cmd);
            }
        }

        public static List<Fournisseur> FindAll()
        {
            List<Fournisseur> fournisseurs = new List<Fournisseur>();
            using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM fournisseur"))
            {
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmd);
                foreach (DataRow row in dt.Rows)
                {
                    Fournisseur f = new Fournisseur
                    {
                        NumFournisseur = (int)row["numfournisseur"],
                        NomFournisseur = (string)row["nomfournisseur"]
                    };
                    fournisseurs.Add(f);
                }
            }
            return fournisseurs;
        }

        public static List<Fournisseur> FindBySelection(string criteres)
        {
            List<Fournisseur> fournisseurs = new List<Fournisseur>();
            using (NpgsqlCommand cmd = new NpgsqlCommand($"SELECT * FROM fournisseur WHERE {criteres}"))
            {
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmd);
                foreach (DataRow row in dt.Rows)
                {
                    Fournisseur f = new Fournisseur
                    {
                        NumFournisseur = (int)row["numfournisseur"],
                        NomFournisseur = (string)row["nomfournisseur"]
                    };
                    fournisseurs.Add(f);
                }
            }
            return fournisseurs;
        }


        public override bool Equals(object? obj)
        {
            return obj is Fournisseur fournisseur &&
                   this.NumFournisseur == fournisseur.NumFournisseur;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.NumFournisseur, this.NomFournisseur);
        }
    }
}
