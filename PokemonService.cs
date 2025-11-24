using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
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
        public bool AprendreMoviment(int idPokemon, int idMoviment)
        {
            Pokemon pok = _ctx.Pokemons.Find(idPokemon);
            Moviment mov = _ctx.Moviments.Find(idMoviment);
            if (!pok.Moviments.Contains(mov) && pok.Moviments.Count < 4 && mov != null)
            {
                pok.Moviments.Add(mov);
                mov.Pokemons.Add(pok);
                _ctx.SaveChanges();
                return true;
            }
            return false;
        }

        public bool OblidarMoviment(int idPokemon, int idMoviment)
        {
            Pokemon pok = _ctx.Pokemons.Find(idPokemon);
            Moviment mov = _ctx.Moviments.Find (idMoviment);
            if (pok.Moviments.Contains(mov))
            {
                pok.Moviments.Remove(mov);
                mov.Pokemons.Remove(pok);
                _ctx.SaveChanges();
                return true;
            }
            return false;
        }
        public List<Moviment> LlistarMovimentsDisponibles(int idPokemon)
        {
            return _ctx.Pokemons
                .Include(m => m.Moviments)
                .Where(m =>  m.PokemonID == idPokemon)
                .SelectMany(m =>m.Moviments)
                .ToList();
        }
        public bool PotEvolucionar(int idPokemon)
        {
            Pokemon pok = _ctx.Pokemons.Find (idPokemon);
            if(pok.Nivell >= pok.NivellEvolucio && pok.EvolucionsSeguents != null && pok.EvolucionsSeguents.Count > 0)
            {
                return true;
            }
            return false;
        }
        public Pokemon Evolucionar(int idPokemon)
        {
            Pokemon pok = _ctx.Pokemons .Find (idPokemon);
            if (PotEvolucionar(idPokemon))
            {
                Pokemon evolucio = pok.EvolucionsSeguents.First();
                Pokemon newPokemon = new Pokemon()
                {
                    Nom = evolucio.Nom,
                    Entrenador = pok.Entrenador,
                    EvolucionsPrevias = pok,
                    EvolucionsSeguents = evolucio.EvolucionsSeguents,
                    NivellEvolucio = evolucio.NivellEvolucio,
                    EstaDebilitat = false,
                    Experiencia = pok.Experiencia,
                    Nivell = pok.Nivell,
                    PuntsVidaMaxim = (int)(pok.PuntsVidaMaxim * 1.5f),
                    PuntsVida = (int)(pok.PuntsVidaMaxim * 1.5f),
                    Tipus = evolucio.Tipus
                };
                foreach (Moviment m in pok.Moviments)
                {
                    if (m.Tipus == evolucio.Tipus || m.Tipus == Tipus.NORMAL)
                    {
                        newPokemon.Moviments.Add(m);
                    }
                }
                _ctx.Pokemons.Add(newPokemon);
                _ctx.Pokemons.Remove(pok);
                _ctx.SaveChanges();
                return newPokemon;
            }
            else
            {
                Console.WriteLine($"{pok.Nom} no pot evolucionar");
                return null;
            }
        }

        public List<Pokemon> CadenaEvolutiva(int idPokemon)
        {
            List<Pokemon> cadena = new List<Pokemon>();
            Pokemon pok = _ctx.Pokemons.Find(idPokemon);
            TrobarPrimer(pok, cadena);
            TrobarUltim(pok, cadena);
            return cadena;
        }

        Pokemon TrobarPrimer(Pokemon pok, List<Pokemon> list)
        {
            if (!list.Contains(pok))
            {
                list.Add(pok);
            }
            if (pok.EvolucionsPrevias != null)
            {
                return TrobarPrimer(pok.EvolucionsPrevias, list);
            }
            else
            {
                return pok;
            }
        }
        Pokemon TrobarUltim(Pokemon pok, List<Pokemon> list)
        {
            if (!list.Contains(pok))
            {
                list.Add (pok);
            }
            if(pok.EvolucionsSeguents != null && pok.EvolucionsSeguents.Count == 1)
            {
                return TrobarUltim(pok.EvolucionsSeguents.First(), list);
            }else if (pok.EvolucionsSeguents.Count > 1)
            {
                foreach(Pokemon p in pok.EvolucionsSeguents)
                {
                    list.Add (p);

                }
                TrobarUltim(pok.EvolucionsSeguents.First(), list);
            }
            else
            {
                return pok;
            }
            return pok;
        }
    }

}
