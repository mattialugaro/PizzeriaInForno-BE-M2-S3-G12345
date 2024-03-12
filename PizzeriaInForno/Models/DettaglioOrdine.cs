using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PizzeriaInForno.Models
{
    public class DettaglioOrdine
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [ScaffoldColumn(false)]
        public int IDDettaglioOrdine { get; set; }

        public Ordine Ordine { get; set; }

        public Articolo Articolo { get; set; }

        [Required(ErrorMessage = "La Quantità è un campo obbligatorio")]
        [Display(Name = "Quantità")]
        [Range(1, int.MaxValue, ErrorMessage = "La Quantità deve essere maggiore di 0")]
        public int Quantita { get; set; }

        [Display(Name = "Prezz Totale del Dettaglio")]
        public decimal PrezzoTotaleDettaglio { get; set; }
    }
}