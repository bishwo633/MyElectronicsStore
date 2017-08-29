using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyElectronicsStore.Models;
using MyElectronicsStore.Helper;

namespace MyElectronicsStore.Controllers
{
    public class ProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Products
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.Brand).Include(p => p.Category).Include(p => p.ProductColor);
            return View(products.ToList());
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.BrandId = new SelectList(db.Brands, "Id", "Name");
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
            ViewBag.ProductColorId = new SelectList(db.ProductColors, "Id", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,BrandId,CategoryId,ProductColorId,ImageUrl,Price")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BrandId = new SelectList(db.Brands, "Id", "Name", product.BrandId);
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", product.CategoryId);
            ViewBag.ProductColorId = new SelectList(db.ProductColors, "Id", "Name", product.ProductColorId);
            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.BrandId = new SelectList(db.Brands, "Id", "Name", product.BrandId);
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", product.CategoryId);
            ViewBag.ProductColorId = new SelectList(db.ProductColors, "Id", "Name", product.ProductColorId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,BrandId,CategoryId,ProductColorId,ImageUrl,Price")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BrandId = new SelectList(db.Brands, "Id", "Name", product.BrandId);
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", product.CategoryId);
            ViewBag.ProductColorId = new SelectList(db.ProductColors, "Id", "Name", product.ProductColorId);
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult EasySearch()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
            return View(db.Products.ToList());
        }
        [HttpPost]
        public ActionResult EasySearch(string title, int? categoryId, decimal?
        priceFrom, decimal? priceTo)
        {
            var result = db.Products
            .Include(p => p.Brand).Include(p => p.Category)
.Include(p => p.ProductColor).ToList();
            if (title != null && title != "")
            {
                result = result.Where(x =>
                x.Title.ToLower().Contains(title.ToLower())).ToList();
            }
            if (categoryId != null)
            {
                result = result.Where(x => x.CategoryId ==
                categoryId).ToList();
            }
            if (priceFrom > 0 && priceTo > 0)
            {
                result = result.Where(x => x.Price >= priceFrom && x.Price <=
                priceTo).ToList();
            }
            ViewBag.CategoryId = new SelectList(db.Categories.ToList(), "Id",
            "Name", categoryId);
            return View(result);
        }

        public ActionResult Cart(int id, int? quantity)
        {
            //If quantity is not supplied, set to 1
            if (quantity == null)
                quantity = 1;

            //Initialize session for cart if not already initialized
            if (Session["MyCart"] == null)
                Session["MyCart"] = new List<Cart>();

            //Get list of carts from session and cast as List<Cart>
            var carts = (List<Cart>)Session["MyCart"];

            if (quantity > 0)
            {
                //Get product from Id
                Product product = db.Products.Find(id);
                if (product == null)
                    return HttpNotFound();

                Cart cart = new Cart();
                //Check if cart contains same item previously or not
                if (carts.Any(c => c.ProductId == id))
                {
                    cart = carts.First(c => c.ProductId == id);
                    carts.Remove(cart);
                }
                //Initialize cart and fill in the values
                cart.ProductId = product.Id;
                cart.Name = product.Title;
                cart.Quantity = cart.Quantity + (int)quantity;
                cart.Price = product.Price;
                //Add the newly initialized cart to collection of carts
                carts.Add(cart);
                //Store updated collection of carts to session
                Session["MyCart"] = carts;
                //Set message for display purpose to ViewBag
                ViewBag.Message = quantity + " " + product.Title + " added to cart successfully";
            }
            else
            {
                ViewBag.Message = "";
            }
            return View(carts);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Cart()
        {
            PaymentGateway gateway = new PaymentGateway();

            var user = HttpContext.User.Identity.Name;

            var paymentToken = db.PaymentTokens
                                .Where(x => x.UserName == user)
                                .OrderByDescending(x => x.CreatedDate)
                                .FirstOrDefault();
            if (paymentToken != null)
            {
                //Get list of carts from session and cast as List<Cart>
                var carts = (List<Cart>)Session["MyCart"];

                var totalAmount = carts.Sum(x => x.Price * x.Quantity);

                var result = gateway.Pay(paymentToken.Token, (double)totalAmount, carts);

                if (result == true)
                {
                    //Clear session and be prepared for next payment
                    Session["MyCart"] = null;

                    return RedirectToAction("Success");
                }
                else
                {
                    return RedirectToAction("Fail");
                }
            }
            else
            {
                return RedirectToAction("Fail");
            }
            return View();
        }

        [HttpGet]
        public ActionResult Success()
        {
            //If payment is successful, show success message to user

            //Note: Use ViewBag to pass message from controller to view
            ViewBag.Message = "Payment successfully completed";
            return View();
        }

        [HttpGet]
        public ActionResult Fail()
        {
            //If payment is not successful, show error message to user
            ViewBag.Message = "Payment could not be completed";
            return View();
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
