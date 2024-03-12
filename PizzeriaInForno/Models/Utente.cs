using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PizzeriaInForno.Models
{
    public class Utente
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [ScaffoldColumn(false)]
        public int IDUtente { get; set; }

        [Required(ErrorMessage = "Il Nome è un campo obbligatorio")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Il Nome non può avere più di 50 caratteri")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Il Cognome è un campo obbligatorio")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Il Cognome non può avere più di 50 caratteri")]
        public string Cognome { get; set; }

        [Required(ErrorMessage = "L' Email è un campo obbligatorio")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "L' Email Nome non può avere più di 50 caratteri")]
        public string Email { get; set; }

        [Required(ErrorMessage = "La Password è un campo obbligatorio")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "La Password non può avere più di 50 caratteri")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Il Ruolo è un campo obbligatorio")]
        [StringLength(20, ErrorMessage = "Il Ruolo non può avere più di 20 caratteri")]
        public string Ruolo { get; set; } = "User";

    }
}