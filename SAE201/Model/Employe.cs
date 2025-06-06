using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE201.Model
{
    public class Employe
    {
        private int numEmploye;
        private Role numRole;
        private string nom;
        private string prenom;
        private string login;
        private string mdp;

        public Employe()
        {
        }

        public Employe(int numEmploye, Role numRole, string nom, string prenom, string login, string mdp)
        {
            this.NumEmploye = numEmploye;
            this.NumRole = numRole;
            this.Nom = nom;
            this.Prenom = prenom;
            this.Login = login;
            this.Mdp = mdp;
        }

        public int NumEmploye
        {
            get
            {
                return this.numEmploye;
            }

            set
            {
                this.numEmploye = value;
            }
        }

        public Role NumRole
        {
            get
            {
                return this.numRole;
            }

            set
            {
                this.numRole = value;
            }
        }

        public string Nom
        {
            get
            {
                return nom;
            }

            set
            {
                nom = value;
            }
        }

        public string Prenom
        {
            get
            {
                return prenom;
            }

            set
            {
                prenom = value;
            }
        }

        public string Login
        {
            get
            {
                return login;
            }

            set
            {
                login = value;
            }
        }

        public string Mdp
        {
            get
            {
                return this.mdp;
            }

            set
            {
                this.mdp = value;
            }
        }

        public override bool Equals(object? obj)
        {
            return obj is Employe employe &&
                   this.NumEmploye == employe.NumEmploye;
        }
    }
}
