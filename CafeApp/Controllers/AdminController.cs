using System;
using System.Net;
using System.Web.Mvc;
using CafeApp.DomainEntity;
using CafeApp.Persistance.Repositories;

namespace CafeApp.Controllers
{
    public class AdminController : Controller
    {
        private UserRolesRepository userRepo = new UserRolesRepository();
        private LoginRepository loginRepo = new LoginRepository();
        public ActionResult LoginPage()
        {
            userRepo.CreateAdmin();
            return View();
        }
        [HttpPost]
        public ActionResult LoginPage(UserRoles userRoles)
        {
            UserRoles user = loginRepo.Login(userRoles);
            if (user == null || user.Roles != Roles.Admin)
            {
                ViewBag.FailMessage = "Your username / password is invalid";
                return View();
            }

            Session["AdminId"] = user.UserRolesId;
            return RedirectToAction("Index");
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("LoginPage");
        }
        // GET: Admin
        public ActionResult Index()
        {
            return View(userRepo.GetUserRoles());
        }

        // GET: Admin/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserRoles userRoles = userRepo.userRoles(id);
            if (userRoles == null)
            {
                return HttpNotFound();
            }
            return View(userRoles);
        }

        // GET: Admin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Username,Password,Roles")] UserRoles userRoles)
        {
            if (ModelState.IsValid)
            {
                bool check = userRepo.CheckDuplicateUser(userRoles);
                if (check)
                {
                    ViewBag.FailMessage = "This user is already registered";
                    return View();
                }
                userRepo.CreateTables(userRoles);
                userRepo.AddUserRoles(userRoles);
                return RedirectToAction("Index");
            }
            return View(userRoles);
        }
        // GET: Admin/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserRoles userRoles = userRepo.userRoles(id);
            if (userRoles == null)
            {
                return HttpNotFound();
            }
            return View(userRoles);
        }

        // POST: Admin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserRoles userRoles)
        {
            if (ModelState.IsValid)
            {
                //int Id = Convert.ToInt32(Session["AdminId"]);
                bool check = userRepo.CheckEditDuplicateUser(userRoles);
                if (check)
                {
                    ViewBag.FailMessage = "This data is already registered in database";
                    return View();
                }
                return RedirectToAction("Index");
            }
            return View(userRoles);
        }

        // GET: Admin/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserRoles userRoles = userRepo.userRoles(id);
            if (userRoles == null)
            {
                return HttpNotFound();
            }
            return View(userRoles);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserRoles userRoles = userRepo.userRoles(id);
            userRepo.DeleteUserRoles(userRoles);
            if (userRoles.Roles == Roles.Cashier)
            {
               int Count = userRepo.FilterCashier();
                if (Count < 1)
                {
                    TableRepository tableRepo = new TableRepository();
                    tableRepo.DeleteAllTable();
                }
            }
            return RedirectToAction("Index");
        }
    }
}
