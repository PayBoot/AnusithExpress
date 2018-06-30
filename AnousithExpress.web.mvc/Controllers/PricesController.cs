using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AnousithExpress.DataEntry;
using AnousithExpress.DataEntry.Models;

namespace AnousithExpress.web.mvc.Controllers
{
    public class PricesController : Controller
    {
        private EntityContext db = new EntityContext();

        // GET: Prices
        public ActionResult Index()
        {
            return View(db.tbPrices.ToList());
        }

        // GET: Prices/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TbPrice tbPrice = db.tbPrices.Find(id);
            if (tbPrice == null)
            {
                return HttpNotFound();
            }
            return View(tbPrice);
        }

        // GET: Prices/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Prices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Condition,PriceSet")] TbPrice tbPrice)
        {
            if (ModelState.IsValid)
            {
                db.tbPrices.Add(tbPrice);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbPrice);
        }

        // GET: Prices/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TbPrice tbPrice = db.tbPrices.Find(id);
            if (tbPrice == null)
            {
                return HttpNotFound();
            }
            return View(tbPrice);
        }

        // POST: Prices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Condition,PriceSet")] TbPrice tbPrice)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbPrice).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbPrice);
        }

        // GET: Prices/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TbPrice tbPrice = db.tbPrices.Find(id);
            if (tbPrice == null)
            {
                return HttpNotFound();
            }
            return View(tbPrice);
        }

        // POST: Prices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TbPrice tbPrice = db.tbPrices.Find(id);
            db.tbPrices.Remove(tbPrice);
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
    }
}
