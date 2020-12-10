using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Canopy.Data;
using Microsoft.AspNet.Identity;

namespace Canopy.Controllers
{
    [Authorize]
    public class TransactionsController : Controller
    {
        private CanopyEntities db = new CanopyEntities();

        //public int CurrentBankAccountId { get; set; }

        //public TransactionsController()
        //{
        //    CurrentBankAccountId = 1;
        //}

        // GET: Transactions
        public ActionResult Index(int? id)
        {
            var userId = User.Identity.GetUserId();

            var customer = db.Customers.Single(c => c.AspNetUserId == userId);

            var transactions = customer.BankAccounts.SelectMany(a => a.Transactions);

            if (id.HasValue)
            {
                transactions = transactions.Where(t => t.AccountId == id.Value);
            }

            transactions = transactions.OrderByDescending(t => t.When);
            ViewBag.BankAccountId = id;
            return View(transactions.ToList());
        }

        public ActionResult QuickList(int? id)
        {
            var userId = User.Identity.GetUserId();

            var customer = db.Customers.Single(c => c.AspNetUserId == userId);

            var transactions = customer.BankAccounts.SelectMany(a => a.Transactions);

            if (id.HasValue)
            {
                transactions = transactions.Where(t => t.AccountId == id.Value);
            }

            transactions = transactions.OrderByDescending(t => t.When).Take(5);
            ViewBag.BankAccountId = id; 
            return PartialView(transactions.ToList());
        }

        // GET: Transactions/Details/5
        public ActionResult Details(int? id, bool? isFromAccount)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }

            ViewBag.isFromAccount = isFromAccount;

            return View(transaction);
        }

        // GET: Transactions/Create
        public ActionResult Create(bool? isFromAccount, int? id)
        {

            var transaction = new Transaction {
                AccountId = id.Value,
                When = DateTime.Today
            };

            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name");
            ViewBag.isFromAccount = isFromAccount;

            return View(transaction);
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TransactionsId,AccountId,CategoryId,Amount,Description,When,Memo,IsWithdraw")] Transaction transaction, bool? isFromAccount)
        {
            if (ModelState.IsValid)
            {
                db.Transactions.Add(transaction);
                db.SaveChanges();
                if (isFromAccount.HasValue && isFromAccount.Value)
                {
                    return RedirectToAction("Details", "BankAccounts", new { id = transaction.AccountId });
                }
                else
                {
                    return RedirectToAction("Index");
                }
                    
            }
            
            ViewBag.AccountId = new SelectList(db.BankAccounts, "BankAccountId", "AccountName", transaction.AccountId);
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", transaction.CategoryId);
            ViewBag.isFromAccount = isFromAccount;

            return View(transaction);
        }

        // GET: Transactions/Edit/5
        public ActionResult Edit(int? id, bool? isFromAccount)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            ViewBag.AccountId = new SelectList(db.BankAccounts, "BankAccountId", "AccountName", transaction.AccountId);
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", transaction.CategoryId);
            ViewBag.isFromAccount = isFromAccount;
            return View(transaction);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TransactionsId,AccountId,CategoryId,Amount,Description,When,Memo,IsWithdraw")] Transaction transaction, bool? isFromAccount)
        {
            if (ModelState.IsValid)
            {
                db.Entry(transaction).State = EntityState.Modified;
                db.SaveChanges();
                if (isFromAccount.HasValue && isFromAccount.Value)
                {
                    return RedirectToAction("Details", "BankAccounts", new { id = transaction.AccountId });
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }

            ViewBag.AccountId = new SelectList(db.BankAccounts, "BankAccountId", "AccountName", transaction.AccountId);
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", transaction.CategoryId);
            ViewBag.isFromAccount = isFromAccount;
            return View(transaction);
        }

        // GET: Transactions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            //ViewBag.isFromAccount = isFromAccount;
            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Transaction transaction = db.Transactions.Find(id);
            int accountId = transaction.AccountId??0;
            db.Transactions.Remove(transaction);
            db.SaveChanges();
            return RedirectToAction("Details", "BankAccounts", new { id = accountId });

            //if (isFromAccount.HasValue && isFromAccount.Value)
            //{
            //    return RedirectToAction("Details", "BankAccounts", new { id = transaction.AccountId });
            //}
            //else
            //{
            //    return RedirectToAction("Index");
            //}
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
