using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE201.Model
{
    public enum TypeDeVin { Rosé,Blanc,Rouge}
    public class TypeVin
    {
        int numType;
        TypeDeVin nomType;

        public int NumType
        {
            get
            {
                return this.numType;
            }

            set
            {
                this.numType = value;
            }
        }

        public TypeDeVin NomType
        {
            get
            {
                return this.nomType;
            }

            set
            {
                if (value != TypeDeVin.Rosé && value != TypeDeVin.Blanc && value != TypeDeVin.Rouge)
                    throw new ArgumentException("Le NomType doit être Rosé ou Blanc ou Rouge");
                this.nomType = value;
            }
        }
    }
}
