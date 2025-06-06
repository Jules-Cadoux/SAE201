using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sae_2._01
{
    public class Assurance
    {
        private int num_assurance;
        private string description_assurance;
        private int prix_assurance;

        public Assurance(int num_assurance, string description_assurance, int prix_assurance)
        {
            Num_assurance = num_assurance;
            Description_assurance = description_assurance;
            Prix_assurance = prix_assurance;
        }

        public int Num_assurance
        {
            get => num_assurance;
            set
            {
                if (value == null)
                    throw new ArgumentNullException("le numéro d'assurance ne doit pas être nul");
                num_assurance = value;
            }
        }
        public string Description_assurance
        {
            get => description_assurance;
            set 
            {
                if (value.Length > 30)
                    throw new ArgumentException("La description de l'assurance ne doit pas faire plus de 30 caractères");
                description_assurance = value; 
            }
        }
        public int Prix_assurance { get => prix_assurance; set => prix_assurance = value; }
    }
}
