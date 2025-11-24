using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProjectePokemon
{
    public class Combat
    {
        public int CombatID { get; set; }

        [DefaultValue(1), Range(1, int.MaxValue)]
        public int Torn { get; set; }

        [Required]
        public DateTime DataInici { get; set; }

        public DateTime? DataFi { get; set; }

        public virtual Entrenador Guanyador { get; set; }

        public virtual ICollection<Entrenador> Participants { get; set; }

        public EstatCombat estatCombat { get; set; }
        public Combat()
        {
            Torn = 1;
            DataInici = DateTime.Now;
            Participants = new HashSet<Entrenador>();
        }
    }

}