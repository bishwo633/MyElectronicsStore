using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyElectronicsStore.Models;

namespace MyElectronicsStore.Controllers
{
    [Authorize]
    public class PaymentTokensController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: PaymentTokens
        public ActionResult Index()
        {
            return View(db.PaymentTokens.ToList());
        }

        // GET: PaymentTokens/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaymentToken paymentToken = db.PaymentTokens.Find(id);
            if (paymentToken == null)
            {
                return HttpNotFound();
            }
            return View(paymentToken);
        }

        // GET: PaymentTokens/Create
        public ActionResult Create()
        {
            var token = new PaymentToken();
            //Generate a new GUID and set as token
            token.Token = Guid.NewGuid().ToString();
            //Assign current date as created date
            token.CreatedDate = DateTime.Now;
            //Assign expiry date after 1 year
            token.ExpiryDate = DateTime.Now.AddYears(1);
            //by default, make it active on create
            token.IsActive = true;
            return View(token);

        }

        // POST: PaymentTokens/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserName,Token,CreatedDate,ExpiryDate,IsActive,Amount")] PaymentToken paymentToken)
        {
            paymentToken.UserName = User.Identity.Name;
            ModelState.Clear();
            if (ModelState.IsValid)
            {
                db.PaymentTokens.Add(paymentToken);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(paymentToken);
        }

        // GET: PaymentTokens/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaymentToken paymentToken = db.PaymentTokens.Find(id);
            if (paymentToken == null)
            {
                return HttpNotFound();
            }
            return View(paymentToken);
        }

        // POST: PaymentTokens/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserName,Token,CreatedDate,ExpiryDate,IsActive,Amount")] PaymentToken paymentToken)
        {
            if (ModelState.IsValid)
            {
                db.Entry(paymentToken).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(paymentToken);
        }

        // GET: PaymentTokens/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaymentToken paymentToken = db.PaymentTokens.Find(id);
            if (paymentToken == null)
            {
                return HttpNotFound();
            }
            return View(paymentToken);
        }

        // POST: PaymentTokens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PaymentToken paymentToken = db.PaymentTokens.Find(id);
            db.PaymentTokens.Remove(paymentToken);
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
