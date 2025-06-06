using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE201.Model
{
    public class Commande
    {
        private int numCommande;
        private Employe numEmploye;
        private DateTime dateCommande;
        private bool valider;
        private double prixTotal;

        public Commande()
        {
        }

        public Commande(int numCommande)
        {
            this.NumCommande = numCommande;
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

        public override bool Equals(object? obj)
        {
            return obj is Commande commande &&
                   this.NumCommande == commande.NumCommande;
        }
    }
}
