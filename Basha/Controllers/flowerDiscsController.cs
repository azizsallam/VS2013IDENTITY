using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Basha.Models;

namespace Basha.Controllers
{
    public class flowerDiscsController : Controller
    {
        private FlowerContext db = new FlowerContext();

        // GET: flowerDiscs
        public ActionResult Index()
        {
            var flowerDiscs = db.flowerDiscs.Include(f => f.FlowerName);
            return View(flowerDiscs.ToList());
        }

        // GET: flowerDiscs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            flowerDisc flowerDisc = db.flowerDiscs.Find(id);
            if (flowerDisc == null)
            {
                return HttpNotFound();
            }
            return View(flowerDisc);
        }

        // GET: flowerDiscs/Create
        public ActionResult Create()
        {
            ViewBag.FlowerId = new SelectList(db.Flowers, "FlowerId", "FlowerName");
            return View();
        }

        // POST: flowerDiscs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "flowerDiscId,FlowerId,fcolor")] flowerDisc flowerDisc)
        {
            if (ModelState.IsValid)
            {
                db.flowerDiscs.Add(flowerDisc);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FlowerId = new SelectList(db.Flowers, "FlowerId", "FlowerName", flowerDisc.FlowerId);
            return View(flowerDisc);
        }

        // GET: flowerDiscs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            flowerDisc flowerDisc = db.flowerDiscs.Find(id);
            if (flowerDisc == null)
            {
                return HttpNotFound();
            }
            Session[("flowerdisid")] = id; 
            ViewBag.FlowerId = new SelectList(db.Flowers, "FlowerId", "FlowerName", flowerDisc.FlowerId);
            return View(flowerDisc);
        }

        // POST: flowerDiscs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "flowerDiscId,FlowerId,fcolor")] flowerDisc flowerDisc)
        {

             int dd = Convert.ToInt32(Session[("flowerdisid")]);
             flowerDisc = db.flowerDiscs.Find(dd);

             if (ModelState.IsValid)
            {
                db.Entry(flowerDisc).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FlowerId = new SelectList(db.Flowers, "FlowerId", "FlowerName", flowerDisc.FlowerId);
            return View(flowerDisc);
        }

        // GET: flowerDiscs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            flowerDisc flowerDisc = db.flowerDiscs.Find(id);
            if (flowerDisc == null)
            {
                return HttpNotFound();
            }
            return View(flowerDisc);
        }

        // POST: flowerDiscs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            flowerDisc flowerDisc = db.flowerDiscs.Find(id);
            db.flowerDiscs.Remove(flowerDisc);
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
    /////////////////
        public JsonResult MGetflowers(int id)
        {
            var zz = (from k in db.flowerDiscs
                      where k.FlowerId == id
                      select k).ToList();
            return Json(zz, JsonRequestBehavior.AllowGet);
        }
     
    //////////////////
    
    }
}
