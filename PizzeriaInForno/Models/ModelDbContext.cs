using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace PizzeriaInForno.Models
{
    public partial class ModelDbContext : DbContext
    {
        public ModelDbContext()
            : base("name=ModelDbContext")
        {
        }

        public virtual DbSet<Articolo> Articolo { get; set; }
        public virtual DbSet<Ingredient> Ingredient { get; set; }
        public virtual DbSet<ArticoloIngredient> ArticoloIngredient { get; set; }
        public virtual DbSet<Ordine> Ordine { get; set; }
        public virtual DbSet<Utente> Utente { get; set; }
        public virtual DbSet<OrdineArticolo> OrdineArticolo { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            /*modelBuilder.Entity<Articolo>()
                .HasMany(e => e.OrdineArticolo)
                .WithRequired(e => e.Articolo)
                .WillCascadeOnDelete(false);*/

            /*modelBuilder.Entity<Articolo>()
                .HasMany(e => e.Ingrediente)
                .WithMany(e => e.Articolo)
                .Map(m => m.ToTable("ArticoloIngrediente").MapLeftKey("IDArticolo").MapRightKey("IDIngrediente"));*/

           /* modelBuilder.Entity<Ordine>()
                .HasMany(e => e.OrdineArticolo)
                .WithRequired(e => e.Ordine)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Utente>()
                .HasMany(e => e.Ordine)
                .WithRequired(e => e.Utente)
                .WillCascadeOnDelete(false);*/
        }
    }
}
