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
    public class TbTimesController : Controller
    {
        private EntityContext db = new EntityContext();

        // GET: TbTimes
        public ActionResult Index()
        {
            return View(db.tbTimes.ToList());
        }

        // GET: TbTimes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TbTime tbTime = db.tbTimes.Find(id);
            if (tbTime == null)
            {
                return HttpNotFound();
            }
            return View(tbTime);
        }

        // GET: TbTimes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TbTimes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Time")] TbTime tbTime)
        {
            if (ModelState.IsValid)
            {
                db.tbTimes.Add(tbTime);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbTime);
        }

        // GET: TbTimes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TbTime tbTime = db.tbTimes.Find(id);
            if (tbTime == null)
            {
                return HttpNotFound();
            }
            return View(tbTime);
        }

        // POST: TbTimes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Time")] TbTime tbTime)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbTime).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbTime);
        }

        // GET: TbTimes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TbTime tbTime = db.tbTimes.Find(id);
            if (tbTime == null)
            {
                return HttpNotFound();
            }
            return View(tbTime);
        }

        // POST: TbTimes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TbTime tbTime = db.tbTimes.Find(id);
            db.tbTimes.Remove(tbTime);
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
