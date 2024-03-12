using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PizzeriaInForno.Models
{
    public class Ordine
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [ScaffoldColumn(false)]
        public int IDOrdine { get; set; }

        public List<DettaglioOrdine> DettagliOrdine { get; set; }

        [Required(ErrorMessage = "L' Indirizzo di Consegna è un campo obbligatorio")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Il Nome non può avere più di 50 caratteri")]
        [Display(Name = "Indirizzo di Consegna")]
        public string IndirizzoConsegna { get; set; }

        [StringLength(50, MinimumLength = 2, ErrorMessage = "Note o Allergie non può avere più di 50 caratteri")]
        [Display(Name = "Note o Allergie")]
        public string Note { get; set; }

        [Required(ErrorMessage = "Evaso è un campo obbligatorio")]
        public bool Evaso {  get; set; }

        [Display(Name = "Prezzo Totale dell' Ordine")]
        public decimal PrezzoTotaleOrdine {  get; set; }

        [ForeignKey("Utente")]
        public int IDUtente { get; set; }
        public Utente Utente {  get; set; }

    }  
}