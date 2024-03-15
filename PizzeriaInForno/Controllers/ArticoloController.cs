using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PizzeriaInForno.Models;

namespace PizzeriaInForno.Controllers
{
    [Authorize]
    public class ArticoloController : Controller
    {
        private ModelDbContext db = new ModelDbContext();

        // GET: Articolo
        [Authorize(Roles = "admin")]
        public ActionResult Index()
        {                                    
            return View(getArticoliConIngredienti());
        }

        // GET: Articolo/Details/5
        [Authorize(Roles = "admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Articolo articolo = db.Articolo.Find(id);
            if (articolo == null)
            {
                return HttpNotFound();
            }

            // carico ingredienti
            var ingredienti = db.ArticoloIngredient.Where(a => a.IDArticolo == articolo.IDArticolo).ToList();
            foreach (var ingrediente in ingredienti)
            {
                var dettagliIngrediente = db.Ingredient.Where(i => i.IDIngredient == ingrediente.IDIngrediente).First();
                articolo.Ingredient.Add(dettagliIngrediente);
            }

            return View(articolo);
        }

        //GET: Articolo/Create
        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {                 
            Articolo articolo = new Articolo();            
            articolo.IngredientiTendina = db.Ingredient.ToList();
            return View(articolo);
        }

        // POST: Articolo/Create
        // Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Nome,Foto,Prezzo,TempoConsegna,IngredientiSelezionati")] Articolo articolo)
        {
            if (ModelState.IsValid)
            {
                // salvataggio foto
                var foto = Request.Files[0];
                if (foto != null && foto.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(foto.FileName);
                    var path = Path.Combine("~/Content/img", fileName);
                    var absolutePath = Server.MapPath(path);
                    foto.SaveAs(absolutePath);

                    articolo.Foto = fileName;
                }                

                // creao articolo
                db.Articolo.Add(articolo);
                db.SaveChanges();

                // aggiungo ingredienti per articolo creato
                foreach (var ingrediente in articolo.IngredientiSelezionati)
                {
                    var articoloIngrediente = new ArticoloIngredient();
                    articoloIngrediente.IDArticolo = articolo.IDArticolo;
                    articoloIngrediente.IDIngrediente = ingrediente;
                    db.ArticoloIngredient.Add(articoloIngrediente);
                }
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(articolo);
        }

        // GET: Articolo/Edit/5
        [Authorize(Roles = "admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Articolo articolo = db.Articolo.Find(id);
            if (articolo == null)
            {
                return HttpNotFound();
            }

            // carico ingredienti dell'articolo
            var ingredienti = db.ArticoloIngredient.Where(a => a.IDArticolo == articolo.IDArticolo).ToList();
            foreach (var ingrediente in ingredienti)
            {
                var dettagliIngrediente = db.Ingredient.Where(i => i.IDIngredient == ingrediente.IDIngrediente).First();
                articolo.Ingredient.Add(dettagliIngrediente);
            }

            // carico tutti ingredienti disponibili
            articolo.IngredientiTendina = db.Ingredient.ToList();

            return View(articolo);
        }   

        // POST: Articolo/Edit/5
        // Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDArticolo,Nome,Foto,Prezzo,TempoConsegna,IngredientiSelezionati")] Articolo articolo)
        {
            if (ModelState.IsValid)
            {
                // salvataggio foto
                var foto = Request.Files[0];
                if (foto != null && foto.ContentLength > 0)
                {
                    // elimino possibile foto vecchia
                    var articoloSalvato = db.Articolo.Where(a => a.IDArticolo == articolo.IDArticolo).First();
                    if (!string.IsNullOrEmpty(articoloSalvato.Foto))
                    {
                        var pathToDelete = Path.Combine("~/Content/img", articoloSalvato.Foto);
                        var absolutePathToDelete = Server.MapPath(pathToDelete);
                        System.IO.File.Delete(absolutePathToDelete);
                    }
                    db.Entry(articoloSalvato).State = EntityState.Detached;

                    // foto nuova
                    var fileName = Path.GetFileName(foto.FileName);
                    var path = Path.Combine("~/Content/img", fileName);
                    var absolutePath = Server.MapPath(path);
                    foto.SaveAs(absolutePath);

                    articolo.Foto = fileName;
                }

                // aggiornamento articolo
                db.Entry(articolo).State = EntityState.Modified;
                db.SaveChanges();

                // aggiornamento ingredienti
                var ingredientiDaEliminare = db.ArticoloIngredient.Where(i => i.IDArticolo == articolo.IDArticolo).ToList();
                db.ArticoloIngredient.RemoveRange(ingredientiDaEliminare);
                db.SaveChanges();

                var ingredientiDaCreare = new List<ArticoloIngredient>();
                foreach (var ingrediente in articolo.IngredientiSelezionati)
                {
                    var creare = new ArticoloIngredient();
                    creare.IDArticolo = articolo.IDArticolo;
                    creare.IDIngrediente = ingrediente;
                    ingredientiDaCreare.Add(creare);
                }
                db.ArticoloIngredient.AddRange(ingredientiDaCreare);
                db.SaveChanges();             

                return RedirectToAction("Index");
            }
            return View(articolo);
        }

        // GET: Articolo/Delete/5
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Articolo articolo = db.Articolo.Find(id);
            if (articolo == null)
            {
                return HttpNotFound();
            }

            var articoloIngredienti = db.ArticoloIngredient.Where(a => a.IDArticolo == articolo.IDArticolo).ToList();
            articoloIngredienti.ForEach(ai =>
            {
                var dettagliIngrediente = db.Ingredient.Where(i => i.IDIngredient == ai.IDIngrediente).First();
                articolo.Ingredient.Add(dettagliIngrediente);
            });

            return View(articolo);
        }

        // POST: Articolo/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Articolo articolo = db.Articolo.Find(id);

            // elimina ingredienti
            var ingredientiDaEliminare = db.ArticoloIngredient.Where(i => i.IDArticolo == articolo.IDArticolo).ToList();
            db.ArticoloIngredient.RemoveRange(ingredientiDaEliminare);
            db.SaveChanges();

            // elimina articolo
            db.Articolo.Remove(articolo);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Menu()
        {            
            return View(getArticoliConIngredienti());
        }

        private IList<Articolo> getArticoliConIngredienti()
        {
            var articoli = db.Articolo.ToList();
            foreach (var articolo in articoli)
            {
                var articoloIngredienti = db.ArticoloIngredient.Where(a => a.IDArticolo == articolo.IDArticolo).ToList();
                articoloIngredienti.ForEach(ai =>
                {
                    var dettagliIngrediente = db.Ingredient.Where(i => i.IDIngredient == ai.IDIngrediente).First();
                    articolo.Ingredient.Add(dettagliIngrediente);
                });
            }
            return articoli;
        }
    }
}
