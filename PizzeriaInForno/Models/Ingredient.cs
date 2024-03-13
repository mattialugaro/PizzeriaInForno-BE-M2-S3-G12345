namespace PizzeriaInForno.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Ingredient")]
    public partial class Ingredient
    {
 
        public Ingredient()
        {
            
        }

        [Key]
        public int IDIngredient { get; set; }

        [Required]
        [StringLength(50)]
        public string Nome { get; set; }
            
        
       
    }
}
