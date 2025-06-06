using System;
using System.Collections.Generic;
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
