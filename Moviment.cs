using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectePokemon
{
    public class Moviment
    {
        public int MovimentID { get; set; }

        [Required, MaxLength(50)]
        public String Nom { get; set; }

        [Required]
        public Tipus Tipus { get; set; }

        [Range(10, 150)]
        public int Potencia { get; set; }

        [Range(50, 100)]
        public int Precisio { get; set; }

        public virtual ICollection<Pokemon> Pokemons { get; set; }

        public Moviment()
        {
            Potencia = 50;
            Precisio = 100;

            Pokemons = new HashSet<Pokemon>();
        }
    }

}
