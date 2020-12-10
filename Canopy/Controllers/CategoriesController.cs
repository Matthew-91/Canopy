using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Canopy.Data;

namespace Canopy.Controllers
{
    public class CategoriesController : Controller
    {
        private CanopyEntities db = new CanopyEntities();

        // GET: Categories
        public ActionResult Index(int id)
        {
            var categories = db.Categories.Where(b => b.AccountId == id).Include(c => c.BankAccount);
            ViewBag.AccountId = id;
            return View(categories.ToList());
        }

        // GET: Categories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: Categories/Create
        public ActionResult Create(int? id)
        {
            ViewBag.AccountId = new SelectList(db.BankAccounts, "BankAccountId", "AccountName");
            Category newCategory = new Category();
            newCategory.AccountId = id;
            return View(newCategory);
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int AccountId, string Name)
        {
            Category category = new Category { AccountId = AccountId, Name = Name };
            if (ModelState.IsValid)
            {
                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = AccountId });
            }

            ViewBag.AccountId = new SelectList(db.BankAccounts, "BankAccountId", "AccountName", category.AccountId);
            return View(category);
        }

        // GET: Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            ViewBag.AccountId = new SelectList(db.BankAccounts, "BankAccountId", "AccountName", category.AccountId);
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CategoryId,AccountId,Name")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = category.AccountId });
            }
            ViewBag.AccountId = new SelectList(db.BankAccounts, "BankAccountId", "AccountName", category.AccountId);
            return View(category);
        }

        // GET: Categories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = db.Categories.Find(id);
            int accountId = category.AccountId ?? 0;
            db.Categories.Remove(category);
            db.SaveChanges();
            return RedirectToAction("Index", new { id = accountId });
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
