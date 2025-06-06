using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sae_2._01
{
    public class Client
    {
        private int num_client;
        private string nom_client;
        private string adresse_rue_client;
        private string adresse_cp_client;
        private string adresse_ville_client;
        private string telephone_client;
        private string mail_client;

        public Client(int num_client, string nom_client, string adresse_rue_client, string adresse_cp_client, string adresse_ville_client, string telephone_client, string mail_client)
        {
            Num_client = num_client;
            Nom_client = nom_client;
            Adresse_rue_client = adresse_rue_client;
            Adresse_cp_client = adresse_cp_client;
            Adresse_ville_client = adresse_ville_client;
            Telephone_client = telephone_client;
            Mail_client = mail_client;
        }

        public Client(int num_client, string nom_client, string telephone_client, string mail_client)
        {
            Num_client = num_client;
            Nom_client = nom_client;
            Telephone_client = telephone_client;
            Mail_client = mail_client;
        }

        public int Num_client { get => num_client;
            set
            {
                if (value == null)
                    throw new ArgumentNullException("l'id ne doit pas être nul");
                num_client = value;
            }
        }

        public Client(string nom_client, string adresse_rue_client, string adresse_cp_client, string adresse_ville_client, string telephone_client, string mail_client)
        {
            Nom_client = nom_client;
            Adresse_rue_client = adresse_rue_client;
            Adresse_cp_client = adresse_cp_client;
            Adresse_ville_client = adresse_ville_client;
            Telephone_client = telephone_client;
            Mail_client = mail_client;
        }

        public Client()
        {
        }

        public string Nom_client { get => nom_client;
            set
            {
                if (value.Length > 50)
                    throw new ArgumentException("Le nom du client ne doit pas faire plus de 50 caractères");
                nom_client = value;
            }
        }
        public string Adresse_rue_client
        {
            get => adresse_rue_client;
            set 
            {
                if (value.Length > 200)
                    throw new ArgumentException("l'adresse de la rue du client ne doit pas faire plus de 200 caractères");
                adresse_rue_client = value; 
            }
        }
        public string Adresse_cp_client { get => adresse_cp_client;
            set
            {
                if (value.Length != 5)
                    throw new ArgumentException("Un code postale doit contenir 5 chiffres");
                
                adresse_cp_client = value;
            }
        }
        public string Adresse_ville_client { get => adresse_ville_client;
            set
            {
                if (value.Length > 50)
                    throw new ArgumentException("Le nom de la ville ne doit pas faire plus de 50 caractères");
                adresse_ville_client = value;
            }
        }
        public string Telephone_client { get => telephone_client;
            set
            {
                if (value.Length != 10)
                    throw new ArgumentException("Un numéro de téléphone doit contenir 10 chiffres");

                telephone_client = value;
            }
        }
        public string Mail_client { get => mail_client;
            set
            {
                if (value.Length > 150)
                    throw new ArgumentException("L'adresse mail ne doit pas faire plus de 150 caractères");
                mail_client = value;
            }
        }
    }
}
