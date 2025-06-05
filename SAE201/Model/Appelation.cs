using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE201.Model
{
    public enum Typedappelation { AOP,AOC,IGP}
    class Appelation
    {
        int NumAppelation;
        Typedappelation NomAppelation;

        public int NumAppelation1
        {
            get
            {
                return this.NumAppelation;
            }

            set
            {
                this.NumAppelation = value;
            }
        }

        public Typedappelation NomAppelation1
        {
            get
            {
                return this.NomAppelation;
            }

            set
            {

                if (value != Typedappelation.AOP && value != Typedappelation.AOC && value != Typedappelation.IGP)
                    throw new ArgumentException("Le Typedappelation doit être AOP ou AOC ou IGP");
                this.NomAppelation = value;
            }
        }
    }
}
