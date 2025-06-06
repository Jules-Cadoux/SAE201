using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sae_2._01
{
    class Employe
    {
        private int num_employe;
        private Magasin num_magasin;
        private string login;
        private string mdp;


        public Employe(int num_employe, string login, string mdp, Magasin num_magasin)
        {
            Num_employe = num_employe;
            Num_magasin = num_magasin;
            Login = login;
            Mdp = mdp;
        }

        public int Num_employe
        {
            get => num_employe;
            set
            {
                if (value ==  null)
                    throw new ArgumentNullException("Le nuéro d'employé ne doit pas être nul");
                num_employe = value;
            }
        }
        public string Login
        {
            get => login;
            set
            {
                if (login.Length == 6)
                    throw new ArgumentException("Le login doit être composé de 6 caractères");
                login = value;
            }
        }
        public string Mdp
        {
            get => mdp;
            set
            {
                if (mdp.Length > 10)
                    throw new ArgumentException("Le mot de passe ne doit pas faire plus de 10 caractères");
                mdp = value; 
            }
        }

        public Magasin Num_magasin { get => num_magasin; set => num_magasin = value; }

        public Magasin Magasin
        {
            get => default;
            set
            {
            }
        }
    }
}
