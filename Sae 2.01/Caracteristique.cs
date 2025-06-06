using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sae_2._01
{
    public class Caracteristique
    {
        private int num_caracteristique;
        private string nom_caracteristique;

        public Caracteristique(int num_caracteristique, string nom_caracteristique)
        {
            Num_caracteristique = num_caracteristique;
            Nom_caracteristique = nom_caracteristique;
        }

        public int Num_caracteristique
        {
            get => num_caracteristique;
            set
            {
                if (value == null)
                    throw new ArgumentNullException("Le numéro de caractéristique ne doit pas être nul");
                num_caracteristique = value;
            }
        }
        public string Nom_caracteristique { get => nom_caracteristique;
            set
            {
                if (value.Length > 30)
                    throw new ArgumentOutOfRangeException("le nom ne doit pas dépasser 30 caractères");
                nom_caracteristique = value;
            }
        }
    }
}
