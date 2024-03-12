using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PizzeriaInForno.Models
{
    public class Articolo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [ScaffoldColumn(false)]
        public int IDArticolo { get; set; }

        [Required(ErrorMessage = "Il Nome è un campo obbligatorio")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Il Nome non può avere più di 50 caratteri")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "La Foto è un campo obbligatorio")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "La Foto non può avere più di 50 caratteri")]
        public string Foto { get; set; }

        [Required(ErrorMessage = "Il Prezzo è un campo obbligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "Il Prezzo deve essere maggiore di 0")]
        public decimal Prezzo { get; set; }

        [Required(ErrorMessage = "Il Tempo di Consegna è un campo obbligatorio")]
        [Display(Name = "Tempo di Consegna")]
        [Range(1, int.MaxValue, ErrorMessage = "Il Tempo di Consegna deve essere maggiore di 0")]
        public int TempoConsegna {  get; set; }

        public List<Ingrediente> Ingredienti { get; set; }

    }
}