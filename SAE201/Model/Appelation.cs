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
        private int numType2;
        private Typedappelation nomAppelation;

        public Appelation()
        {
        }

        public Appelation(int numAppelation)
        {
            this.NumType2 = numAppelation;
        }

        public Appelation(int numAppelation, Typedappelation nomAppelation)
        {
            this.NumType2 = numAppelation;
            this.NomAppelation = nomAppelation;
        }

        public int NumType2
        {
            get
            {
                return this.numType2;
            }

            set
            {
                this.numType2 = value;
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

        public override bool Equals(object? obj)
        {
            return obj is Appelation appelation &&
                   this.NumType2 == appelation.NumType2;
        }
    }
}
