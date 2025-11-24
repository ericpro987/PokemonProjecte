using ProjectePokemon;

public class Program
{
    static PokemonContext ctx;

    static EntrenadorService entrenadorService;
    static PokemonService pokemonService;

    static void Main(string[] args)
    {
        using (ctx = new PokemonContext())
        {
            entrenadorService = new EntrenadorService(ctx);
            pokemonService = new PokemonService(ctx);

            Entrenador eloi = new Entrenador() { Nom = "Eloi", Cognoms = "Albareda" };
            Entrenador marc = new Entrenador() { Nom = "Marc", Cognoms = "Vazquez" };

            ctx.Entrenadors.Add(eloi);
            ctx.Entrenadors.Add(marc);

            Pokemon pikachu = new Pokemon() { Nom = "Pikachu", Tipus = Tipus.ELECTRIC, Nivell = 5, PuntsVida = 50, PuntsVidaMaxim = 50, Experiencia = 50, Entrenador = eloi };
            Pokemon charmander = new Pokemon() { Nom = "Charmander", Tipus = Tipus.FOC, Nivell = 5, PuntsVida = 50, PuntsVidaMaxim = 50, Experiencia = 50, Entrenador = marc };
            Pokemon charmeleon = new Pokemon() { Nom = "Charmeleon", Tipus = Tipus.FOC, Nivell = 5, PuntsVida = 150, PuntsVidaMaxim = 150, Experiencia = 50, Entrenador = marc };
            Pokemon charizard = new Pokemon() { Nom = "Charizard", Tipus = Tipus.FOC, Nivell = 16, PuntsVida = 250, PuntsVidaMaxim = 250, Experiencia = 50, Entrenador = marc };
            Pokemon megacharizardx = new Pokemon() { Nom = "Mega Charizard X", Tipus = Tipus.FOC, Nivell = 16, PuntsVida = 450, PuntsVidaMaxim = 450, Experiencia = 50, Entrenador = marc };
            Pokemon megacharizardy = new Pokemon() { Nom = "Mega Charizard Y", Tipus = Tipus.FOC, Nivell = 16, PuntsVida = 350, PuntsVidaMaxim = 350, Experiencia = 50, Entrenador = marc };
            charmander.EvolucionsSeguents.Add(charmeleon);
            charmeleon.EvolucionsPrevias = charmander;
            charmeleon.EvolucionsSeguents.Add(charizard);
            charizard.EvolucionsPrevias = charmeleon;
            charizard.EvolucionsSeguents.Add(megacharizardx);
            charizard.EvolucionsSeguents.Add(megacharizardy);
            megacharizardx.EvolucionsPrevias = charizard;
            megacharizardy.EvolucionsPrevias = charizard;
            Pokemon pidgeon = new Pokemon() { Nom = "Pidgeon", Tipus = Tipus.NORMAL, Nivell = 5, PuntsVida = 50, PuntsVidaMaxim = 50, Experiencia = 50, Entrenador = eloi };
            Pokemon squirtle = new Pokemon() { Nom = "Squirtle", Tipus = Tipus.AIGUA, Nivell = 5, PuntsVida = 50, PuntsVidaMaxim = 50, Experiencia = 50, Entrenador = marc };
            Pokemon wartortle = new Pokemon() { Nom = "Wartortle", Tipus = Tipus.AIGUA, Nivell = 5, PuntsVida = 50, PuntsVidaMaxim = 50, Experiencia = 50, Entrenador = marc };
            Pokemon blastoise = new Pokemon() { Nom = "Blastoise", Tipus = Tipus.AIGUA, Nivell = 5, PuntsVida = 50, PuntsVidaMaxim = 50, Experiencia = 50, Entrenador = marc };
            squirtle.EvolucionsSeguents.Add(wartortle);
            wartortle.EvolucionsPrevias = squirtle;
            wartortle.EvolucionsSeguents.Add(blastoise);
            blastoise.EvolucionsPrevias = wartortle;
            Pokemon bulbasaur = new Pokemon() { Nom = "Bulbasaur", Tipus = Tipus.PLANTA, Nivell = 5, PuntsVida = 50, PuntsVidaMaxim = 50, Experiencia = 50, Entrenador = eloi };
            Pokemon ivysaur = new Pokemon() { Nom = "Ivysaur", Tipus = Tipus.PLANTA, Nivell = 5, PuntsVida = 50, PuntsVidaMaxim = 50, Experiencia = 50, Entrenador = eloi };
            Pokemon venosaur = new Pokemon() { Nom = "Venosaur", Tipus = Tipus.PLANTA, Nivell = 5, PuntsVida = 50, PuntsVidaMaxim = 50, Experiencia = 50, Entrenador = eloi };
            bulbasaur.EvolucionsSeguents.Add(ivysaur);
            ivysaur.EvolucionsPrevias = bulbasaur;
            ivysaur.EvolucionsSeguents.Add(venosaur);
            venosaur.EvolucionsPrevias = ivysaur;
            Pokemon cyndaquil = new Pokemon() { Nom = "Cyndaquil", Tipus = Tipus.FOC, Nivell = 5, PuntsVida = 50, PuntsVidaMaxim = 50, Experiencia = 50, Entrenador = marc };

            ctx.Pokemons.Add(pikachu);
            ctx.Pokemons.Add(charmander);
            ctx.Pokemons.Add(charmeleon);
            ctx.Pokemons.Add(charizard);
            ctx.Pokemons.Add(megacharizardx);
            ctx.Pokemons.Add(megacharizardy);
            ctx.Pokemons.Add(pidgeon);
            ctx.Pokemons.Add(squirtle);
            ctx.Pokemons.Add(wartortle);
            ctx.Pokemons.Add(blastoise);
            ctx.Pokemons.Add(bulbasaur);
            ctx.Pokemons.Add(ivysaur);
            ctx.Pokemons.Add(venosaur);
            ctx.Pokemons.Add(cyndaquil);

            ctx.SaveChanges();

            // Mostrar els entrenadors i els seus Pokémon inicials
            MostrarDadesBD();

            // Afegir un nou Pokémon a un entrenador
            Pokemon nouPokemon = new Pokemon() { Nom = "Totodile", Tipus = Tipus.AIGUA, Nivell = 5, PuntsVida = 50, PuntsVidaMaxim = 50 };
            ctx.Pokemons.Add(nouPokemon);
            entrenadorService.AddPokemon(1, nouPokemon);

            // Intentar afegir un 7è Pokémon (ha de fallar)
            Afegir7Pokemon(1);

            // Fer que un Pokémon guanyi experiència i pugi de nivell
            GuanyarExperiencia(1, 150);

            // Fer que un Pokémon quedi debilitat(PuntsVida a 0)
            PokemonDebilitat(1, 1000000);

            // Mostrar l'equip actiu d'un entrenador
            MostrarEquipActiu(1);

            // Curar un Pokémon debilitat
            CurarDebilitat(1);

            // Tornar a mostrar l'equip actiu
            MostrarEquipActiu(1);

            Console.WriteLine("FUNCION RECURSIVA EVOLUCIONES");

            MostrarEvolucions2(charmeleon.PokemonID);



            Console.WriteLine("\n=== Evolucions===");
            Console.WriteLine("Evolucions de Charmander:");
            MostarEvolucions(charmander);
            Console.WriteLine("\nEvolucions de Charmeleon:");
            MostarEvolucions(charmeleon);
            Console.WriteLine("\nEvolucions de Charizard:");
            MostarEvolucions(charizard);
            Console.WriteLine("\nEvolucions de Mega Charizard X:");
            MostarEvolucions(megacharizardx);
            Console.WriteLine("\nEvolucions de Mega Charizard Y:");
            MostarEvolucions(megacharizardy);
            Console.WriteLine("Evolucions de Squirtle:");
            MostarEvolucions(squirtle);
            Console.WriteLine("\nEvolucions de Wartortle:");
            MostarEvolucions(wartortle);
            Console.WriteLine("\nEvolucions de Blastoise:");
            MostarEvolucions(blastoise);
            Console.WriteLine("\nEvolucions de Bulbasaur:");
            MostarEvolucions(bulbasaur);
            Console.WriteLine("\nEvolucions de Ivysaur:");
            MostarEvolucions(ivysaur);
            Console.WriteLine("\nEvolucions de Venosaur:");
            MostarEvolucions(venosaur);
        }
    }
    public static void MostrarDadesBD()
    {
        Console.WriteLine("=== ENTRENADORS ===");
        foreach (var entrenador in ctx.Entrenadors.ToList())
        {
            Console.WriteLine($"{entrenador.EntrenadorID} - {entrenador.Nom} {entrenador.Cognoms}");
        }

        Console.WriteLine("\n=== POKEMONS ===");
        foreach (var pokemon in ctx.Pokemons.ToList())
        {
            Console.WriteLine($"{pokemon.PokemonID} - {pokemon.Nom} ({pokemon.Tipus}) - Entrenador: {pokemon.Entrenador.Nom}");
        }
    }

    public static void Afegir7Pokemon(int idEntrenador)
    {
        Console.WriteLine("\n=== Afegir un nou Pokémon a un entrenador ===");

        Pokemon nouPokemon1 = new Pokemon() { Nom = "Pokemon 5", Tipus = Tipus.AIGUA, Nivell = 5, PuntsVida = 50, PuntsVidaMaxim = 50 };
        Pokemon nouPokemon2 = new Pokemon() { Nom = "Pokemon 6", Tipus = Tipus.AIGUA, Nivell = 5, PuntsVida = 50, PuntsVidaMaxim = 50 };
        Pokemon nouPokemon3 = new Pokemon() { Nom = "Pokemon 7", Tipus = Tipus.AIGUA, Nivell = 5, PuntsVida = 50, PuntsVidaMaxim = 50 };

        entrenadorService.AddPokemon(idEntrenador, nouPokemon1);
        entrenadorService.AddPokemon(idEntrenador, nouPokemon2);
        entrenadorService.AddPokemon(idEntrenador, nouPokemon3);
    }

    public static void GuanyarExperiencia(int idPokemon, int exp)
    {
        Console.WriteLine("\n=== Fer que un Pokémon guanyi experiència i pugi de nivell ===");

        pokemonService.GuanyarExperiencia(idPokemon, exp);
    }

    public static void PokemonDebilitat(int idPokemon, int dany)
    {
        Console.WriteLine("\n=== Fer que un Pokémon quedi debilitat(PuntsVida a 0) ===");

        pokemonService.RebreDany(idPokemon, dany);
    }

    public static void MostrarEquipActiu(int idEntrenador)
    {
        Console.WriteLine("\n=== Mostrar l'equip actiu d'un entrenador ===");

        entrenadorService.EquipActiu(idEntrenador);
    }

    public static void CurarDebilitat(int idPokemon)
    {
        Console.WriteLine("\n=== Curar un Pokémon debilitat ===");

        pokemonService.Curar(idPokemon);
    }

    public static void MostrarEvolucions2(int idPokemon)
    {
        List<Pokemon> list = pokemonService.CadenaEvolutiva(idPokemon);

        foreach(Pokemon p in list)
        {
            Console.WriteLine($"- {p.Nom}".ToUpper());
        }
    }

    public static void MostarEvolucions(Pokemon pokemon)
    {
        Console.WriteLine($"Evolucions Previes de {pokemon.Nom}");
        if (pokemon.EvolucionsPrevias != null)
        {
            Console.WriteLine($"- {pokemon.EvolucionsPrevias.Nom}");
        }
        else
        {
            Console.WriteLine("- Cap");
        }
        Console.WriteLine($"Evolucions Seguents de {pokemon.Nom}:");
        if (pokemon.EvolucionsSeguents.Count > 0)
        {
            foreach (var evo in pokemon.EvolucionsSeguents)
            {
                Console.WriteLine($"- {evo.Nom}");
            }
        }
        else
        {
            Console.WriteLine("- Cap");
        }
    }
}
