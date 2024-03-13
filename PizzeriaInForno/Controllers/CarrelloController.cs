using PizzeriaInForno.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
                return RedirectToAction("Index", "Articolo");
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
                    carrello.Remove(rimuoviProdotto);
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult Ordine(string indirizzo, string note)
        {
            ModelDbContext db = new ModelDbContext();
            var IDUtente = db.Utente.FirstOrDefault(u => u.Email == User.Identity.Name).IDUtente;

            var carrello = Session["cart"] as List<OrdineArticolo>;
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

            return RedirectToAction("Index", "Articolo");
        }
    }
}