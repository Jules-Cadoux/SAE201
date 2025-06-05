using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE201.Model
{
    public enum Typedappelation { AOP,AOC,IGP}
    public class Appelation
    {
        private int numAppelation;
        private Typedappelation nomAppelation;

        public int NumAppelation
        {
            get
            {
                return this.numAppelation;
            }

            set
            {
                this.numAppelation = value;
            }
        }

        public Typedappelation NomAppelation
        {
            get
            {
                return this.nomAppelation;
            }

            set
            {
                if (value != Typedappelation.AOP && value != Typedappelation.AOC && value != Typedappelation.IGP)
                    throw new ArgumentException("Le Typedappelation doit être AOP ou AOC ou IGP");
                this.nomAppelation = value;
            }
        }
    }
}
