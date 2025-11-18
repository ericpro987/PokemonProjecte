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

            base.OnModelCreating(modelBuilder);
        }
    }

}