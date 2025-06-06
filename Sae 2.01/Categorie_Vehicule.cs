using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sae_2._01
{
    public class Categorie_Vehicule
    {
        private string nom_categorie;

        public Categorie_Vehicule(string nom_categorie)
        {
            Nom_categorie = nom_categorie;
        }

        public string Nom_categorie { get => nom_categorie;
            set
            {
                if (value == null)
                    throw new ArgumentNullException("La catégorie de véhicule ne doit pas être nul");
                if (value.Length > 30)
                    throw new ArgumentException("Le nom de catégprie ne doit pas faire plus de 30 caractères");
                nom_categorie = value;
            }
        }
    }
}
