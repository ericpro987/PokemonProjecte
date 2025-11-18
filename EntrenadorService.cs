using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectePokemon
{
    public class EntrenadorService
    {
        private PokemonContext _ctx;

        public EntrenadorService(PokemonContext ctx)
        {
            _ctx = ctx;
        }

        public bool AddPokemon(int idEntrenador, Pokemon Pokemon)
        {
            Entrenador entrenador = _ctx.Entrenadors.Find(idEntrenador);
            if (entrenador.Pokemons.Count >= 6 || entrenador == null)
            {
                Console.WriteLine("L'entrenador ha arribat al màxim de pokemons (6) o l'entrenador no es troba a la BD");
                return false;
            }
            entrenador.Pokemons.Add(Pokemon);

            Console.WriteLine("Pokemon " + Pokemon.Nom + " afegit al entrendor " + entrenador.Nom + ". Llista pokemons de " + entrenador.Nom + ":\n");
            foreach (var p in entrenador.Pokemons)
            {
                Console.WriteLine(p.Nom);
            }

            _ctx.SaveChanges();
            return true;
        }

        public List<Pokemon> EquipActiu(int idEntrenador)
        {
            Entrenador entrenador = _ctx.Entrenadors.Find(idEntrenador);
            if (entrenador == null)
            {
                Console.WriteLine("L'entrenador no es troba a la BD");
                return null;
            }

            List<Pokemon> pokemonsActius = (from p in entrenador.Pokemons
                                            where !p.EstaDebilitat
                                            select p).ToList();

            Console.WriteLine("Pokemons actius de " + entrenador.Nom + ":");
            foreach (Pokemon p in pokemonsActius)
            {
                Console.WriteLine($"{p.Nom}");
            }
            return pokemonsActius;
        }
    }

}
