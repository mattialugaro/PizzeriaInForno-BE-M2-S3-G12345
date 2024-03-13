namespace PizzeriaInForno.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.DynamicData;

    [Table("ArticoloIngrediente")]
    public partial class ArticoloIngredient
    {      
        public ArticoloIngredient()
        {
            
        }

        [Key]
        [Column(Order = 0)]
        public int IDArticolo { get; set; }

        [Key]
        [Column(Order = 1)]        
        public int IDIngrediente { get; set; }
                

    }
}
