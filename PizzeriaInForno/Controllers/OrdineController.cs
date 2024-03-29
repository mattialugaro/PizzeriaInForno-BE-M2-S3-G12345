﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PizzeriaInForno.Models;


namespace PizzeriaInForno.Controllers
{
    [Authorize(Roles = "admin")]
    public class OrdineController : Controller
    {
        private ModelDbContext db = new ModelDbContext();

        // GET: Ordine
        public ActionResult Index()
        {
            var ordine = db.Ordine.Include(o => o.Utente);
            return View(ordine.ToList());
        }

        // GET: Ordine/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ordine ordine = db.Ordine.Find(id);
            if (ordine == null)
            {
                return HttpNotFound();
            }
            return View(ordine);
        }

        // GET: Ordine/Create
        public ActionResult Create()
        {
            ViewBag.IDUtente = new SelectList(db.Utente, "IDUtente", "Nome");
            return View();
        }

        // POST: Ordine/Create
        // Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDOrdine,IndirizzoConsegna,Note,Evaso,IDUtente")] Ordine ordine)
        {
            if (ModelState.IsValid)
            {
                ordine.DataOrdine = DateTime.Now;
                db.Ordine.Add(ordine);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IDUtente = new SelectList(db.Utente, "IDUtente", "Nome", ordine.IDUtente);
            return View(ordine);
        }

        // GET: Ordine/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ordine ordine = db.Ordine.Find(id);
            if (ordine == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDUtente = new SelectList(db.Utente, "IDUtente", "Nome", ordine.IDUtente);
            return View(ordine);
        }

        // POST: Ordine/Edit/5
        // Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDOrdine,IndirizzoConsegna,Note,Evaso,IDUtente")] Ordine ordine)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ordine).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IDUtente = new SelectList(db.Utente, "IDUtente", "Nome", ordine.IDUtente);
            return View(ordine);
        }

        // GET: Ordine/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ordine ordine = db.Ordine.Find(id);
            if (ordine == null)
            {
                return HttpNotFound();
            }
            return View(ordine);
        }

        // POST: Ordine/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ordine ordine = db.Ordine.Find(id);
            db.Ordine.Remove(ordine);
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

        public ActionResult Dashboard()
        {
            return View();
        }

        [HttpGet]
        public ActionResult OrdiniEvasi() //Errore nel funzionamento, problema con la conversione del dato
        {
            List<Ordine> ordine;
            string connectionstring = ConfigurationManager.ConnectionStrings["MyDB"].ConnectionString.ToString();

            using (SqlConnection conn = new SqlConnection(connectionstring))
            {
                string sql = @"SELECT COUNT(Evaso) AS OrdiniEvasi FROM Ordine WHERE Evaso = 1";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Evaso", "1");

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                ordine = new List<Ordine>();
                while (reader.Read())
                {
                    //int ordiniEvasiCount = Convert.ToInt16(reader["OrdiniEvasi"]);
                    //Ordine o = new Ordine { Evaso = ordiniEvasiCount };
                    //ordine.Add(o);
                }
            }

            return Json(ordine, JsonRequestBehavior.AllowGet);
        }
    }
}
