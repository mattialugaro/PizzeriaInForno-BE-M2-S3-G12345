using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PizzeriaInForno.Models;

namespace PizzeriaInForno.Controllers
{
    public class ArticoloController : Controller
    {
        private ModelDbContext db = new ModelDbContext();

        // GET: Articolo
        public ActionResult Index()
        {                        
            return View(db.Articolo.ToList());
        }

        // GET: Articolo/Details/5
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
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Nome,Foto,Prezzo,TempoConsegna,IngredientiSelezionati")] Articolo articolo)
        {
            if (ModelState.IsValid)
            {
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
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDArticolo,Nome,Foto,Prezzo,TempoConsegna,IngredientiSelezionati")] Articolo articolo)
        {
            if (ModelState.IsValid)
            {
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
            return View(articolo);
        }

        // POST: Articolo/Delete/5
        [HttpPost, ActionName("Delete")]
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
            return View(db.Articolo.ToList());
        }
    }
}
