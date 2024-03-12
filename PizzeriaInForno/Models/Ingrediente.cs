using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PizzeriaInForno.Models
{
    public class Ingrediente
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [ScaffoldColumn(false)]
        public int IDIngrediente { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Il Nome Ingrediente non può avere più di 50 caratteri")]
        public string Nome { get; set; }

    }
}