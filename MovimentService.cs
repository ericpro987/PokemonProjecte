using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectePokemon
{
    public class MovimentService
    {
        private PokemonContext _ctx;

        public MovimentService(PokemonContext ctx)
        {
            _ctx = ctx;
        }

       public double CalcularEfectivitat(Tipus tipusAtacant, Tipus tipusDefensor)
        {
            switch(tipusAtacant)
            {
                case Tipus.FOC:
                    if (tipusDefensor == Tipus.PLANTA) return 2.0d;
                    if (tipusDefensor == Tipus.AIGUA) return 0.5d;
                    break;
                case Tipus.AIGUA:
                    if (tipusDefensor == Tipus.FOC) return 2.0d;
                    if (tipusDefensor == Tipus.PLANTA) return 0.5d;
                    break;
                case Tipus.PLANTA:
                    if (tipusDefensor == Tipus.AIGUA) return 2.0d;
                    if (tipusDefensor == Tipus.FOC) return 0.5d;
                    break;
                case Tipus.ELECTRIC:
                    if (tipusDefensor == Tipus.AIGUA) return 2.0d;
                    if (tipusDefensor == Tipus.PLANTA || tipusDefensor == Tipus.ELECTRIC) return 0.5d;
                    break;
            }
            return 1.0d;
        }

    }
}
