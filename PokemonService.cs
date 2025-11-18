using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectePokemon
{
    public class PokemonService
    {
        private PokemonContext _ctx;

        public PokemonService(PokemonContext ctx)
        {
            _ctx = ctx;
        }

        public void GuanyarExperiencia(int idPokemon, int exp)
        {
            Pokemon pokemon = _ctx.Pokemons.Find(idPokemon);
            if (pokemon == null)
            {
                Console.WriteLine("El pokemon no es troba a la BD");
                return;
            }

            pokemon.Experiencia += exp;

            int vegadesQuePujaNivell = (int)pokemon.Experiencia / 100;
            if (vegadesQuePujaNivell > 0)
            {
                pokemon.Nivell += vegadesQuePujaNivell;
                pokemon.PuntsVidaMaxim += (vegadesQuePujaNivell * 10);
                pokemon.PuntsVida = pokemon.PuntsVidaMaxim;
                pokemon.Experiencia %= 100;
            }

            Console.WriteLine(pokemon.Nom + " ha guanyat " + exp + " d'experiencia");
            Console.WriteLine(pokemon.Nom + " ara es nivell " + pokemon.Nivell + " i té " + pokemon.Experiencia + " d'experiencia");

            _ctx.SaveChanges();
        }

        public void RebreDany(int idPokemon, int dany)
        {
            Pokemon pokemon = _ctx.Pokemons.Find(idPokemon);
            if (pokemon == null)
            {
                Console.WriteLine("El pokemon no es troba a la BD");
                return;
            }
            if (dany < 0)
            {
                Console.WriteLine("No es pot aplicar dany negatiu");
                return;
            }
            pokemon.PuntsVida -= dany;
            Console.WriteLine(pokemon.Nom + " ha rebut " + dany + " de dany");
            Console.WriteLine("Vida: " + pokemon.PuntsVida + " de " + pokemon.PuntsVidaMaxim);

            if (pokemon.PuntsVida <= 0)
            {
                pokemon.PuntsVida = 0;
                pokemon.EstaDebilitat = true;
                Console.WriteLine(pokemon.Nom + " té " + pokemon.PuntsVida + " de vida, per tant, eliminat");
            }

            _ctx.SaveChanges();
        }

        public void Curar(int idPokemon)
        {
            Pokemon pokemon = _ctx.Pokemons.Find(idPokemon);
            if (pokemon == null)
            {
                Console.WriteLine("El pokemon no es troba a la BD");
                return;
            }

            Console.WriteLine(pokemon.Nom + " abans de curar-se: " + pokemon.PuntsVida + ". Debilitat: " + pokemon.EstaDebilitat);
            pokemon.PuntsVida = pokemon.PuntsVidaMaxim;
            pokemon.EstaDebilitat = false;

            Console.WriteLine(pokemon.Nom + " s' ha curat, ara té vida màxima: " + pokemon.PuntsVida + ". Debilitat: " + pokemon.EstaDebilitat);
            _ctx.SaveChanges();
        }
    }

}
