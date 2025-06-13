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
    public class Vin
    {
        private int numVin;
        private Fournisseur numFournisseur;
        private int numType;
        private Appelation numType2;
        private string nomVin;
        private double prixVin;
        private string description;
        private int millesime;
        private string imagePath;

        public Vin(int numVin, Fournisseur numFournisseur, int numType, Appelation numType2, string nomVin, double prixVin, string description, int millesime)
        {
            this.NumVin = numVin;
            this.NumFournisseur = numFournisseur;
            this.NumType = numType;
            this.NumType2 = numType2;
            this.NomVin = nomVin;
            this.PrixVin = prixVin;
            this.Description = description;
            this.Millesime = millesime;
        }

        public Vin(int numVin, Fournisseur numFournisseur, int numType, Appelation numType2)
        {
            this.NumVin = numVin;
            this.NumFournisseur = numFournisseur;
            this.NumType = numType;
            this.NumType2 = numType2;
        }

        public Vin() { }

        public int NumVin
        {
            get
            {
                return numVin;
            }

            set
            {
                numVin = value;
            }
        }

        public Fournisseur NumFournisseur
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

        public int NumType
        {
            get
            {
                return this.numType;
            }

            set
            {
                this.numType = value;
            }
        }

        public Appelation NumType2
        {
            get
            {
                return this.numType2;
            }

            set
            {
                this.numType2 = value;
            }
        }

        public string NomVin
        {
            get
            {
                return nomVin;
            }

            set
            {
                nomVin = value;
            }
        }

        public double PrixVin
        {
            get
            {
                return prixVin;
            }

            set
            {
                if(value <= 0)
                {
                    MessageBox.Show("Le prix en peut pas être <= 0", "Erreur création vin", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                prixVin = value;
            }
        }

        public string Description
        {
            get
            {
                return description;
            }

            set
            {
                description = value;
            }
        }

        public int Millesime
        {
            get
            {
                return this.millesime;
            }

            set
            {
                if(value > DateTime.Today.Year)
                {
                    MessageBox.Show("Le millésime doit être < à la date actuelle", "Erreur création vin",MessageBoxButton.OK,MessageBoxImage.Error);
                }
                this.millesime = value;
            }
        }

        public string ImagePath
        {
            get
            {
                string abdelkader="";
                switch (NumType)
                {
                    case 1:
                        abdelkader = "rouge";
                        break;
                    case 2:
                        abdelkader = "blanc";
                        break;
                    case 3:
                        abdelkader = "rosé";
                        break;
                }
                Console.WriteLine($"/Fichier/Vin{abdelkader}.png");
                return $"/img/vin_{abdelkader}.png";
            }
        }

        public int Create()
        {
            int id = 0;
            using (NpgsqlCommand cmd = new NpgsqlCommand(
                "INSERT INTO vin (numfournisseur, numtype, numtype2, nomvin, prixvin, descriptif, millesime) " +
                "VALUES (@numfournisseur, @numtype, @numtype2, @nomvin, @prixvin, @descriptif, @millesime) RETURNING numvin"))
            {
                cmd.Parameters.AddWithValue("numfournisseur", this.NumFournisseur.NumFournisseur);
                cmd.Parameters.AddWithValue("numtype", this.NumType);
                cmd.Parameters.AddWithValue("numtype2", this.NumType2.NumType2);
                cmd.Parameters.AddWithValue("nomvin", this.NomVin);
                cmd.Parameters.AddWithValue("prixvin", this.PrixVin);
                cmd.Parameters.AddWithValue("descriptif", this.Description);
                cmd.Parameters.AddWithValue("millesime", this.Millesime);
                id = DataAccess.Instance.ExecuteInsert(cmd);
                this.NumVin = id;
            }
            return id;
        }

        public void Read()
        {
            using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM vin WHERE numvin = @id"))
            {
                cmd.Parameters.AddWithValue("id", this.NumVin);
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmd);
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    this.NomVin = (string)row["nomvin"];
                    this.PrixVin = Convert.ToDouble(row["prixvin"]);
                    this.Description = (string)row["descriptif"];
                    this.Millesime = (int)row["millesime"];

                    this.NumFournisseur = new Fournisseur { NumFournisseur = (int)row["numfournisseur"] };
                    this.NumType = (int)row["numtype"];
                    this.NumType2 = new Appelation { NumType2 = (int)row["numtype2"] };
                }
            }
        }

        public int Update()
        {
            using (NpgsqlCommand cmd = new NpgsqlCommand(
                "UPDATE vin SET numfournisseur = @numfournisseur, numtype = @numtype, numtype2 = @numtype2, nomvin = @nomvin, " +
                "prixvin = @prixvin, descriptif = @descriptif, millesime = @millesime WHERE numvin = @numvin"))
            {
                cmd.Parameters.AddWithValue("numfournisseur", this.NumFournisseur.NumFournisseur);
                cmd.Parameters.AddWithValue("numtype", this.NumType);
                cmd.Parameters.AddWithValue("numtype2", this.NumType2.NumType2);
                cmd.Parameters.AddWithValue("nomvin", this.NomVin);
                cmd.Parameters.AddWithValue("prixvin", this.PrixVin);
                cmd.Parameters.AddWithValue("descriptif", this.Description);
                cmd.Parameters.AddWithValue("millesime", this.Millesime);
                cmd.Parameters.AddWithValue("numvin", this.NumVin);
                return DataAccess.Instance.ExecuteSet(cmd);
            }
        }

        public int Delete()
        {
            using (NpgsqlCommand cmd = new NpgsqlCommand("DELETE FROM vin WHERE numvin = @numvin"))
            {
                cmd.Parameters.AddWithValue("numvin", this.NumVin);
                return DataAccess.Instance.ExecuteSet(cmd);
            }
        }

        public static List<Vin> FindAll()
        {
            List<Vin> vins = new List<Vin>();
            using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM vin"))
            {
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmd);
                foreach (DataRow row in dt.Rows)
                {
                    Vin v = new Vin
                    {
                        NumVin = (int)row["numvin"],
                        NomVin = (string)row["nomvin"],
                        PrixVin = Convert.ToDouble(row["prixvin"]),
                        Description = (string)row["descriptif"],
                        Millesime = (int)row["millesime"],
                        NumFournisseur = new Fournisseur { NumFournisseur = (int)row["numfournisseur"] },
                        NumType = (int)row["numtype"],
                        NumType2 = new Appelation { NumType2 = (int)row["numtype2"] }
                    };
                    vins.Add(v);
                }
            }
            return vins;
        }

        public static List<Vin> FindBySelection(string criteres)
        {
            List<Vin> vins = new List<Vin>();
            using (NpgsqlCommand cmd = new NpgsqlCommand($"SELECT * FROM vin WHERE {criteres}"))
            {
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmd);
                foreach (DataRow row in dt.Rows)
                {
                    Vin v = new Vin
                    {
                        NumVin = (int)row["numvin"],
                        NomVin = (string)row["nomvin"],
                        PrixVin = Convert.ToDouble(row["prixvin"]),
                        Description = (string)row["descriptif"],
                        Millesime = (int)row["millesime"],
                        NumFournisseur = new Fournisseur { NumFournisseur = (int)row["numfournisseur"] },
                        NumType = (int)row["numtype"],
                        NumType2 = new Appelation { NumType2 = (int)row["numtype2"] }
                    };
                    vins.Add(v);
                }
            }
            return vins;
        }


        public override bool Equals(object? obj)
        {
            return obj is Vin vin &&
                   this.NumVin == vin.NumVin &&
                   this.NomVin == vin.NomVin &&
                   this.PrixVin == vin.PrixVin &&
                   this.Description == vin.Description &&
                   this.Millesime == vin.Millesime;
        }
        public static bool operator >(Vin? left, Vin? right)
        {
            return (left.PrixVin > right.PrixVin);
        }
        public static bool operator <(Vin? left, Vin? right)
        {
            return (left.PrixVin < right.PrixVin);
        }
    }
}
