namespace PizzeriaInForno.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Ordine")]
    public partial class Ordine
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Ordine()
        {
            OrdineArticolo = new HashSet<OrdineArticolo>();
        }

        [Key]
        public int IDOrdine { get; set; }

        [Required]
        [StringLength(50)]
        public string IndirizzoConsegna { get; set; }

        [Required]
        [StringLength(50)]
        public string Note { get; set; }

        public bool Evaso { get; set; }

        public int IDUtente { get; set; }

        public virtual Utente Utente { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrdineArticolo> OrdineArticolo { get; set; }
    }
}
