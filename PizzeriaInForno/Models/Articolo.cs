namespace PizzeriaInForno.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;

    [Table("Articolo")]
    public partial class Articolo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Articolo()
        {
            OrdineArticolo = new HashSet<OrdineArticolo>();            
        }

        [Key]
        public int IDArticolo { get; set; }

        [Required]
        [StringLength(50)]
        public string Nome { get; set; }

        [Required]
        [StringLength(50)]
        public string Foto { get; set; }

        public decimal Prezzo { get; set; }

        [Display(Name = "Tempo di Preparazione")]
        public int TempoConsegna { get; set; }

       
        public virtual ICollection<OrdineArticolo> OrdineArticolo { get; set; }

        [NotMapped]
        [Display(Name = "Ingredienti")]
        public virtual List<Ingredient> Ingredient { get; set; } = new List<Ingredient>();

        [NotMapped]
        public List<Ingredient> IngredientiTendina = new List<Ingredient>();

        [NotMapped]
        public List<int> IngredientiSelezionati { get; set; } = new List<int>();

        public bool isIngredienteSelezionato(int idIngrediente)
        {
            return Ingredient.Where(i => i.IDIngredient == idIngrediente).FirstOrDefault() != null;
        }
    }
}
