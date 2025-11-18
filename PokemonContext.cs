using System.Collections.Generic;
using System.Data.Entity;

namespace ProjectePokemon
{
    public class PokemonContext : DbContext
    {
        public PokemonContext() : base("name=pokemon")
        {
        }
        public DbSet<Entrenador> Entrenadors { get; set; }
        public DbSet<Pokemon> Pokemons { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<PokemonContext>());

            modelBuilder.Entity<Pokemon>()
                .HasOptional(p => p.EvolucionsPrevias)
                .WithMany(p => p.EvolucionsSeguents);

            modelBuilder.Entity<Entrenador>().HasMany(e => e.CombatsParticipats)
                .WithMany(c => c.Participants);

            modelBuilder.Entity<Entrenador>().HasOptional(e => e.CombatGuanyat)
                .WithOptionalPrincipal(c => c.Guanyador);

            base.OnModelCreating(modelBuilder);
        }
    }

}