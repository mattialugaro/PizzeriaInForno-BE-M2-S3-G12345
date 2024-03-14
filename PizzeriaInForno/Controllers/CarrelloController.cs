using PizzeriaInForno.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PizzeriaInForno.Controllers
{
    public class CarrelloController : Controller
    {
         private ModelDbContext db = new ModelDbContext();
        // GET: Carrello
        public ActionResult Index()
        {
            var carrello = Session["carrello"] as List<OrdineArticolo>;
            if (carrello == null || !carrello.Any())
            {
                return RedirectToAction("Menu", "Articolo");
            }

            foreach (var articolo in carrello)
            {
                var dettagli = db.Articolo.Where(a => a.IDArticolo == articolo.IDArticolo).First();
                articolo.Articolo = dettagli;

                var ingredienti = db.ArticoloIngredient.Where(a => a.IDArticolo == articolo.IDArticolo).ToList();
                foreach (var ingrediente in ingredienti)
                {
                    var dettagliIngrediente = db.Ingredient.Where(i => i.IDIngredient == ingrediente.IDIngrediente).First();
                    articolo.Articolo.Ingredient.Add(dettagliIngrediente);
                }
            }

            return View(carrello);
        }

        public ActionResult Delete(int? id)
        {
            var carrello = Session["carrello"] as List<OrdineArticolo>;
            if (carrello != null)
            {
                var rimuoviProdotto = carrello.FirstOrDefault(a => a.IDArticolo == id);
                if (rimuoviProdotto != null)
                {
                    if(rimuoviProdotto.Quantita > 1)
                    {
                        rimuoviProdotto.Quantita--;
                    }
                    carrello.Remove(rimuoviProdotto);
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult AggiungiArticolo(int id) 
        {
            var carrello = Session["carrello"] as List<OrdineArticolo>;
            if(carrello == null)
            {
                carrello = new List<OrdineArticolo>();
                Session["carrello"] = carrello;
            }

            var aggiungiProdotto = carrello.FirstOrDefault(a => a.IDArticolo == id);
            if (aggiungiProdotto != null)
            {
                aggiungiProdotto.Quantita++;
            }
            else
            {
                aggiungiProdotto = new OrdineArticolo();
                aggiungiProdotto.IDArticolo = id;
                aggiungiProdotto.Quantita = 1;
                carrello.Add(aggiungiProdotto);
            }

            return RedirectToAction("Menu", "Articolo");
        }

        public ActionResult Ordine(string indirizzo, string note)
        {
            ModelDbContext db = new ModelDbContext();
            var IDUtente = db.Utente.FirstOrDefault(u => u.Email == User.Identity.Name).IDUtente;

            var carrello = Session["carrello"] as List<OrdineArticolo>;
            if (carrello != null && carrello.Any())
            {
                Ordine o = new Ordine();
                o.IndirizzoConsegna = indirizzo;
                o.Note = note;
                o.Evaso = false;
                o.IDUtente = IDUtente;
                // manca la data e il totale ordine

                db.Ordine.Add(o);
                db.SaveChanges();

                foreach (var articolo in carrello)
                {
                    OrdineArticolo oa = new OrdineArticolo();
                    oa.IDOrdine = o.IDOrdine;
                    oa.IDArticolo = articolo.IDArticolo;
                    oa.Quantita = articolo.Quantita;
                    db.OrdineArticolo.Add(oa);
                    db.SaveChanges();
                }
                carrello.Clear();
            }

            return RedirectToAction("Menu", "Articolo");
        }
    }
}