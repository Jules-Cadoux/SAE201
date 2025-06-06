using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE201.Model
{
    public enum differentRole { Responsable, Vendeur}
    public class Role
    {
        private int numRole;
        private differentRole nomRole;

        public Role()
        {
        }

        public Role(int numRole, differentRole nomRole)
        {
            this.NumRole = numRole;
            this.NomRole = nomRole;
        }

        public int NumRole
        {
            get
            {
                return numRole;
            }

            set
            {
                numRole = value;
            }
        }

        public differentRole NomRole
        {
            get
            {
                return this.nomRole;
            }

            set
            {
                this.nomRole = value;
            }
        }

        public override bool Equals(object? obj)
        {
            return obj is Role role &&
                   this.NumRole == role.NumRole;
        }
    }
}
