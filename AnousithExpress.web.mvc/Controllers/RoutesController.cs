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
    public class RoutesController : Controller
    {
        private EntityContext db = new EntityContext();

        // GET: Routes
        public ActionResult Index()
        {
            return View(db.tbRoutes.ToList());
        }

        // GET: Routes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TbRoute tbRoute = db.tbRoutes.Find(id);
            if (tbRoute == null)
            {
                return HttpNotFound();
            }
            return View(tbRoute);
        }

        // GET: Routes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Routes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Route")] TbRoute tbRoute)
        {
            if (ModelState.IsValid)
            {
                db.tbRoutes.Add(tbRoute);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbRoute);
        }

        // GET: Routes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TbRoute tbRoute = db.tbRoutes.Find(id);
            if (tbRoute == null)
            {
                return HttpNotFound();
            }
            return View(tbRoute);
        }

        // POST: Routes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Route")] TbRoute tbRoute)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbRoute).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbRoute);
        }

        // GET: Routes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TbRoute tbRoute = db.tbRoutes.Find(id);
            if (tbRoute == null)
            {
                return HttpNotFound();
            }
            return View(tbRoute);
        }

        // POST: Routes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TbRoute tbRoute = db.tbRoutes.Find(id);
            db.tbRoutes.Remove(tbRoute);
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
