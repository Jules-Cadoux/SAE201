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
        private string nomVin;
        private double prixVin;
        private string description;
        private int millésime;

        public Vin(int numVin, Fournisseur numFournisseur, string nomVin, double prixVin, string description, int millésime)
        {
            this.NumVin = numVin;
            this.NomVin = nomVin;
            this.PrixVin = prixVin;
            this.Description = description;
            this.Millésime = millésime;
        }

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
                    MessageBox.Show("Le prix en peut pas être <= à la date actuelle", "Erreur création vin", MessageBoxButton.OK, MessageBoxImage.Error);

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
