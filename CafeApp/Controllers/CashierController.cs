using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using CafeApp.Persistance.Repositories;
using CafeApp.DomainEntity;

namespace CafeApp.Controllers
{
    public class CashierController : Controller
    {
        //private CafeWebApp db = new CafeWebApp();
        private LoginRepository loginRepo = new LoginRepository();
        public ActionResult LoginPage()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LoginPage(UserRoles userRoles)
        {
            loginRepo.Login(userRoles);
            UserRoles user = loginRepo.Login(userRoles);

            if (user.Roles == Roles.Cashier)
            {
                Session["CashierId"] = user.UserRolesId;
                return RedirectToAction("Index");
            }

            return View();
        }
        // GET: Cashier
        public ActionResult Index()
        {
            return View(db.Table.ToList());
        }

        // GET: Cashier/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Table table = db.Table.Find(id);
            if (table == null)
            {
                return HttpNotFound();
            }
            return View(table);
        }

        // GET: Cashier/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Cashier/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TableId,TableNo,TableStatus")] Table table)
        {
            if (ModelState.IsValid)
            {
                db.Table.Add(table);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(table);
        }

        // GET: Cashier/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Table table = db.Table.Find(id);
            if (table == null)
            {
                return HttpNotFound();
            }
            return View(table);
        }

        // POST: Cashier/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TableId,TableNo,TableStatus")] Table table)
        {
            int checkId = Convert.ToInt32(Session["CashierId"]);
            if (ModelState.IsValid)
            {
                db.Entry(table).State = EntityState.Modified;
                table.UserRolesId = checkId;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(table);
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            return View("LoginPage");
        }
        // GET: Cashier/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Table table = db.Table.Find(id);
            if (table == null)
            {
                return HttpNotFound();
            }
            return View(table);
        }

        // POST: Cashier/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Table table = db.Table.Find(id);
            db.Table.Remove(table);
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
