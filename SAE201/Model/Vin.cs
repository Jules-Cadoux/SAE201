using System;
using System.Collections.Generic;
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
        private TypeVin numType;
        private Appelation numType2;
        private string nomVin;
        private double prixVin;
        private string description;
        private int millésime;

        public Vin(int numVin, Fournisseur numFournisseur, TypeVin numType, Appelation numType2, string nomVin, double prixVin, string description, int millésime)
        {
            this.NumVin = numVin;
            this.NumFournisseur = numFournisseur;
            this.NumType = numType;
            this.NumType2 = numType2;
            this.NomVin = nomVin;
            this.PrixVin = prixVin;
            this.Description = description;
            this.Millésime = millésime;
        }

        public Vin(int numVin, Fournisseur numFournisseur, TypeVin numType, Appelation numType2)
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

        public TypeVin NumType
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

        public int Millésime
        {
            get
            {
                return this.millésime;
            }

            set
            {
                if(value > DateTime.Today.Year)
                {
                    MessageBox.Show("Le millésime doit être < à la date actuelle", "Erreur création vin",MessageBoxButton.OK,MessageBoxImage.Error);
                }
                this.millésime = value;
            }
        }



        public override bool Equals(object? obj)
        {
            return obj is Vin vin &&
                   this.NumVin == vin.NumVin &&
                   this.NomVin == vin.NomVin &&
                   this.PrixVin == vin.PrixVin &&
                   this.Description == vin.Description &&
                   this.Millésime == vin.Millésime;
        }
    }
}
