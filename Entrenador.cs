using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProjectePokemon
{
    public class Entrenador
    {
        public int EntrenadorID { get; set; }

        [Required, MaxLength(50)]
        public String Nom { get; set; }

        [Required, MaxLength(100)]
        public String Cognoms { get; set; }

        [DefaultValue(0), Range(0, int.MaxValue)]
        public int Medalles { get; set; }

        [DefaultValue(1500), Range(0, int.MaxValue)]
        public int DinersPokedollars { get; set; }

        public virtual ICollection<Pokemon> Pokemons { get; set; }
        public virtual Combat CombatGuanyat { get; set; }
        public virtual ICollection<Combat> CombatsParticipats { get; set; }

        public Entrenador()
        {
            Pokemons = new HashSet<Pokemon>();
            CombatsParticipats = new HashSet<Combat>();

            Medalles = 0;
            DinersPokedollars = 1500;
        }
    }

}