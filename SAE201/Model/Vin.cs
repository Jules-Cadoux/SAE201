using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE201.Model
{
    public class Vin
    {
        private int numVin;
        private string nomVin;
        private double prixVin;
        private string description;
        private int millésime;

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
                this.millésime = value;
            }
        }
    }
}
