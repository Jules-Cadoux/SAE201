using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sae_2._01
{
    public class Adresse
    {
        private string adresse_rue;
        private string adresse_cp;
        private string adresse_ville;

        public Adresse(string adresse_rue, string adresse_cp, string adresse_ville)
        {
            Adresse_rue = adresse_rue;
            Adresse_cp = adresse_cp;
            Adresse_ville = adresse_ville;
        }

        public string Adresse_rue
        {
            get => adresse_rue;
            set
            {
                if (value.Length > 200)
                    throw new ArgumentException("l'adresse de la rue ne doit pas faire plus de 200 caractères");
                adresse_rue = value;
            }
        }

        public string Adresse_cp
        {
            get => adresse_cp;
            set
            {
                if (value.Length != 5)
                    throw new ArgumentException("Un code postale doit contenir 5 chiffres");
                foreach (var item in value)
                {
                    if (item.GetType() != typeof(int))
                        throw new ArgumentException("Le cpde postal dot être constitué de chiffre");
                }
                adresse_cp = value;
            }
        }

        public string Adresse_ville
        {
            get => adresse_ville;
            set
            {
                if (value.Length > 50)
                    throw new ArgumentException("Le nom de la ville ne doit pas faire plus de 50 caractères");
                adresse_ville = value;
            }
        }

    }
}
