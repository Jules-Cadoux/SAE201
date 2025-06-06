using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE201.Model
{
    public class Client
    {
        private int numClient;
        private string nomClient;
        private string prenomClient;
        private string mailClient;

        public Client(int numClient, string nomClient, string prenomClient, string mailClient)
        {
            this.NumClient = numClient;
            this.NomClient = nomClient;
            this.PrenomClient = prenomClient;
            this.MailClient = mailClient;
        }

        public Client(int numClient)
        {
            this.NumClient = numClient;

        }
        public Client(){ }

        public int NumClient
        {
            get
            {
                return numClient;
            }

            set
            {
                numClient = value;
            }
        }

        public string NomClient
        {
            get
            {
                return nomClient;
            }

            set
            {
                nomClient = value;
            }
        }

        public string PrenomClient
        {
            get
            {
                return prenomClient;
            }

            set
            {
                prenomClient = value;
            }
        }

        public string MailClient
        {
            get
            {
                return this.mailClient;
            }

            set
            {
                this.mailClient = value;
            }
        }
    }
}
