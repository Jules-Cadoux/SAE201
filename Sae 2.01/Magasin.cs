using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sae_2._01
{
    public class Magasin
    {
        private int num_magasin;
        private string nom_magasin;
        private string adresse_rue_magasin;
        private string adresse_cp_magasin;
        private string adresse_ville_magasin;
        private string horaire_magasin;

        public Magasin(int num_magasin, string nom_magasin, string adresse_rue_magasin, string adresse_cp_magasin, string adresse_ville_magasin, string horaire_magasin)
        {
            Num_magasin = num_magasin;
            Nom_magasin = nom_magasin;
            Adresse_rue_magasin = adresse_rue_magasin;
            Adresse_cp_magasin = adresse_cp_magasin;
            Adresse_ville_magasin = adresse_ville_magasin;
            Horaire_magasin = horaire_magasin;
        }

        public int Num_magasin
        {
            get => num_magasin; set
            { 
                if (value == null)
                    throw new ArgumentNullException("le numéro du magasin ne doit pas être nul");
                num_magasin = value; 
            }
        }

        public string Nom_magasin { get => nom_magasin;
            set
            {
                if (value.Length > 50 || value == null)
                    throw new ArgumentException("Le nom du magasin ne doit pas être nul et faire moins de 50 caractères");
                nom_magasin = value;
            }
        }


        public string Adresse_rue_magasin
        {
            get => adresse_rue_magasin;
            set
            {
                if (value.Length > 200)
                    throw new ArgumentException("l'adresse de la rue du magasin ne doit pas faire plus de 200 caractères");
                adresse_rue_magasin = value;
            }
        }

        public string Adresse_cp_magasin
        {
            get => adresse_cp_magasin;
            set
            {
                if (value.Length != 5)
                    throw new ArgumentException("Un code postale doit contenir 5 chiffres");
                
                adresse_cp_magasin = value;
            }
        }

        public string Adresse_ville_magasin
        {
            get => adresse_ville_magasin;
            set
            {
                if (value.Length > 50)
                    throw new ArgumentException("Le nom de la ville ne doit pas faire plus de 50 caractères");
                adresse_ville_magasin = value;
            }
        }
        public string Horaire_magasin
        {
            get => horaire_magasin;
            set
            {
                if (value.Length > 20)
                    throw new ArgumentException("L'horaire du magasin ne doit pas faire plus de 20 caractères");
                horaire_magasin = value;
            }
        }
    }
}
