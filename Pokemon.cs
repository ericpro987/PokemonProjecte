using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectePokemon
{
    public class Pokemon
    {
        public int PokemonID { get; set; }

        [Range(1, 100)]
        public int? NivellEvolucio { get; set; }

        public String Nom { get; set; }

        [Required]
        public Tipus Tipus { get; set; }

        [DefaultValue(5), Range(1, 100)]
        public int Nivell { get; set; }

        [Required, Range(0, int.MaxValue)]
        public int PuntsVida { get; set; }

        [Required, Range(1, int.MaxValue)]
        public int PuntsVidaMaxim { get; set; }

        [DefaultValue(0), Range(0, int.MaxValue)]
        public int Experiencia { get; set; }

        [DefaultValue(false)]
        public bool EstaDebilitat { get; set; }

        public virtual Entrenador Entrenador { get; set; }
        public virtual Pokemon EvolucionsPrevias { get; set; }
        public virtual ICollection<Pokemon> EvolucionsSeguents { get; set; }
        public virtual ICollection<Moviment> Moviments { get; set; }

        public Pokemon()
        {
            Nivell = 5;
            Experiencia = 0;
            EstaDebilitat = false;

            EvolucionsSeguents = new HashSet<Pokemon>();
            Moviments = new HashSet<Moviment>();
        }


    }

}
