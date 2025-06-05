using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE201.Model
{
    public class Demande
    {
        private int numDemande;
        private DateTime dateDemande;
        private int quantiteDemande;
        private string accepter;

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

        public DateTime DateDemande
        {
            get
            {
                return dateDemande;
            }

            set
            {
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
                this.accepter = value;
            }
        }
    }
}
