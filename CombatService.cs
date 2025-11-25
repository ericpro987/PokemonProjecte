using Microsoft.Win32.SafeHandles;
using System.Data.Entity.Core.Mapping;

namespace ProjectePokemon
{
    public class CombatService
    {
        private PokemonContext _ctx;

        public CombatService(PokemonContext ctx)
        {
            _ctx = ctx;
        }

        public Combat IniciarCombat(int idEntrenador1, int idEntrenador2)
        {
            Random rnd = new Random();
            Combat combat = new Combat()
            {
                Participants = new List<Entrenador>()
                {
                    _ctx.Entrenadors.Find(idEntrenador1),
                    _ctx.Entrenadors.Find(idEntrenador2)
                },
                Torn = rnd.Next(1, 3),
                estatCombat = EstatCombat.EN_CURS,
                DataInici = DateTime.Now,
                DataFi = null,
                Guanyador = null
            };
            if (combat.Participants.First().Pokemons != null && combat.Participants.First().Pokemons.Count > 0 && combat.Participants.Last().Pokemons != null && combat.Participants.Last().Pokemons.Count >= 0)
            {
                _ctx.Combats.Add(combat);
                _ctx.SaveChanges();
                return combat;
            }
            else
            {
                return null;
            }
        }
        string UsarMoviment(int idCombat, int idPokemonAtacant, int idPokemonDefensor, int idMoviment)
        {
            MovimentService MovimentService = new MovimentService(_ctx);
            PokemonService pokemonService = new PokemonService(_ctx);
            Combat combat = _ctx.Combats.Find(idCombat);
            Pokemon pokemonAtacant = _ctx.Pokemons.Find(idPokemonAtacant);
            Pokemon pokemonDefensor = _ctx.Pokemons.Find(idPokemonDefensor);
            Moviment mov = _ctx.Moviments.Find(idMoviment);
            if (ComprovarPrecisioMoviment(mov))
            {
                int dany = calcularDany(mov, pokemonAtacant, pokemonDefensor);
                pokemonService.RebreDany(pokemonDefensor.PokemonID, dany);
                ComprovarDebilitament(pokemonDefensor, pokemonAtacant);
                if(MovimentService.CalcularEfectivitat(pokemonAtacant.Tipus, pokemonDefensor.Tipus) == 2.0d)
                    return $"{pokemonAtacant.Nom} ha utilitzat {mov.Nom}! Causa {dany} de dany! Es super efectiu!";
                else
                    return $"{pokemonAtacant.Nom} ha utilitzat {mov.Nom}! Causa {dany} de dany!";
            }
            return "L'atac ha fallat!";
        }

        public bool ComprovarPrecisioMoviment(Moviment moviment)
        {
            Random rnd = new Random();
            int valorRandom = rnd.Next(0, 101);
            if (valorRandom > moviment.Precisio)
            {
                return false;
            }
            return true;
        }
        public int calcularDany(Moviment mov, Pokemon pokemonAtacant, Pokemon pokemonDefensor)
        {
            MovimentService MovimentService = new MovimentService(_ctx);
            int danyBase = (mov.Potencia * pokemonAtacant.Nivell) / 10;
            double efectivitat = MovimentService.CalcularEfectivitat(pokemonAtacant.Tipus, pokemonDefensor.Tipus);
            int danyFinal = (int)(danyBase * efectivitat);
            return danyFinal;
        }
        public void ComprovarDebilitament(Pokemon pokemonDefensor, Pokemon pokemonAtacant)
        {
            if (pokemonDefensor.PuntsVida <= 0)
            {
                Console.WriteLine(pokemonDefensor.Nom + " s'ha debilitat!");
                int expGuanyada = pokemonDefensor.Nivell * 50;
                PokemonService pokemonService = new PokemonService(_ctx);
                pokemonService.GuanyarExperiencia(pokemonAtacant.PokemonID, expGuanyada);
                if (pokemonDefensor.EstaDebilitat == false)
                {
                    pokemonDefensor.EstaDebilitat = true;
                    pokemonDefensor.actiu = false;
                    _ctx.SaveChanges();
                }
                if (pokemonService.PotEvolucionar(pokemonAtacant.PokemonID))
                {
                    Console.WriteLine(pokemonAtacant.Nom + " pot evolucionar!");
                }
            }
        }
        public bool CanviarPokemon(int idCombat, int idEntrenador, int idPokemon)
        {
            Combat combat = _ctx.Combats.Find(idCombat);
            Entrenador entrenador = _ctx.Entrenadors.Find(idEntrenador);
            Pokemon pok = _ctx.Pokemons.Find(idPokemon);
            if (combat.Participants.Contains(entrenador))
            {
                if (entrenador.Pokemons.Contains(pok))
                {
                    if (!pok.EstaDebilitat)
                    {
                        pok.actiu = true;
                        _ctx.SaveChanges();
                        return true;
                    }
                }
            }
            return false;
        }
        bool ComprovarFiCombat(int idCombat)
        {
            bool seSalva = false;
            Combat c = _ctx.Combats.Find(idCombat);
            foreach (Entrenador e in c.Participants)
            {
                foreach (Pokemon pok in e.Pokemons)
                {
                    if (!pok.EstaDebilitat)
                    {
                        seSalva = true;
                    }
                }
                if(seSalva)
                    seSalva = false;
                else
                {
                    foreach(Entrenador e2 in c.Participants)
                    {
                        if(!(e == e2))
                        {
                            AssignarGuanyador(c.CombatID,e2.EntrenadorID);
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        void AssignarGuanyador(int idCombat, int idEntrenador)
        {
            Combat combat = _ctx.Combats.Find(idCombat);
            Entrenador e = _ctx.Entrenadors.Find(idEntrenador);
            combat.Guanyador = e;
            combat.estatCombat = EstatCombat.FINALITZAT;
            combat.DataFi = DateTime.Now;
            e.Medalles+=1;
            foreach(Entrenador e2 in combat.Participants)
            {
                if (e2 != e)
                {
                    e.DinersPokedollars += CalcularPokeDollars(e2.EntrenadorID);
                }
            }
            _ctx.SaveChanges();
        }
        int CalcularPokeDollars(int idEntrenador)
        {
            int mitja = 0;
            Entrenador e = _ctx.Entrenadors.Find(idEntrenador);
            foreach(Pokemon pokemon in e.Pokemons)
            {
                mitja += pokemon.Nivell;
            }
            mitja/=e.Pokemons.Count();
            return mitja;
        }
        void CambiarTorn(int idCombat)
        {
            Combat c = _ctx.Combats.Find(idCombat);
            switch (c.Torn)
            {
                case 1:
                    c.Torn = 2; 
                    break;
                case 2:
                    c.Torn = 1;
                    break;
            }
            _ctx.SaveChanges();
        }
        public Pokemon PokemonActiu(int idEntrenador, int idCombat)
        {
            Entrenador e = _ctx.Entrenadors.Find(idEntrenador);
            foreach(Pokemon pok in e.Pokemons)
            {
                if (pok.actiu)
                    return pok;
            }
            return null;
        }
    }
}
