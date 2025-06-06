using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SAE201.Model
{
    public class Demande
    {
        private int numDemande;
        private Vin numVin;
        private Employe numEmploye;
        private Commande numCommande;
        private Client numClient;
        private DateTime dateDemande;
        private int quantiteDemande;
        private string accepter;

        public Demande(int numDemande, Vin numVin, Employe numEmploye, Commande numCommande, Client numClient, DateTime dateDemande, int quantiteDemande, string accepter)
        {
            this.NumDemande = numDemande;
            this.NumVin = numVin;
            this.NumEmploye = numEmploye;
            this.NumCommande = numCommande;
            this.NumClient = numClient;
            this.DateDemande = dateDemande;
            this.QuantiteDemande = quantiteDemande;
            this.Accepter = accepter;
        }

        public Demande(int numDemande, Vin numVin, Employe numEmploye, Commande numCommande, Client numClient)
        {
            this.NumDemande = numDemande;
            this.NumVin = numVin;
            this.NumEmploye = numEmploye;
            this.NumCommande = numCommande;
            this.NumClient = numClient;
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
                if (value != DateTime.Today)
                    MessageBox.Show("La date du jour doit = à celle du jour", "Erreur de création demande", MessageBoxButton.OK, MessageBoxImage.Error);
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
                if (value != "Accepter" && value != "En cours" && value != "Refuser")
                    MessageBox.Show("Il faut choisir Accepter, En cours ou Refuser", "Erreur demande", MessageBoxButton.OK, MessageBoxImage.Error);
                this.accepter = value;
            }
        }

        public override bool Equals(object? obj)
        {
            return obj is Demande demande &&
                   this.NumDemande == demande.NumDemande;
        }
    }
}
