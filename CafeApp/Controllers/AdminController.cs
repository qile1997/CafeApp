using System;
using System.Net;
using System.Web.Mvc;
using CafeApp.DomainEntity;
using CafeApp.DomainEntity.Interfaces;
using CafeApp.Persistance;
using CafeApp.Persistance.Repositories;
using CafeApp.Persistance.Services;

namespace CafeApp.Controllers
{
    public class AdminController : Controller
    {
        private CafeWebApp _context { get; set; }
        public iUserRepository UserRolesRepository { get; set; }
        public iUserService UserService { get; set; }
        public iTableRepository TableRepository { get; set; }

        public ActionResult LoginPage(CafeWebApp context)
        {
            _context = context;
            InitializeData();
            return View();
        }

        public void InitializeData()
        {
            UserRolesRepository = new UserRepository(_context);
            UserService = new UserService(_context);
            TableRepository = new TableRepository(_context);
            UserService.CreateAdmin();
        }

        [HttpPost]
        public ActionResult LoginPage(User user)
        {
            if (!UserService.CheckUserCredentials(user, Roles.Admin))
            {
                ViewBag.FailMessage = "Your username / password is invalid";
                return View();
            }

            Session["AdminId"] = user;
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
            return View(UserRolesRepository.GetAllUsers());
        }

        // GET: Admin/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User userRoles = UserRolesRepository.GetUserById(id);
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
        public ActionResult Create([Bind(Include = "Id,Username,Password,Roles")] User userRoles)
        {
            if (ModelState.IsValid)
            {
                bool check = UserService.CheckDuplicateUser(userRoles);
                if (check)
                {
                    ViewBag.FailMessage = "This user is already registered";
                    return View();
                }
                UserService.CreateTables(userRoles);
                UserRolesRepository.AddUser(userRoles);
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
            User userRoles = UserRolesRepository.GetUserById(id);
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
        public ActionResult Edit(User userRoles)
        {
            if (ModelState.IsValid)
            {
                //int Id = Convert.ToInt32(Session["AdminId"]);
                bool check = UserService.CheckEditDuplicateUser(userRoles);
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
            User userRoles = UserRolesRepository.GetUserById(id);
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
            User userRoles = UserRolesRepository.GetUserById(id);
            UserRolesRepository.DeleteUser(userRoles);
            if (userRoles.Roles == Roles.Cashier)
            {
                int Count = UserService.GetAllCashier();
                if (Count < 1)
                {
                    TableRepository.DeleteAllTable();
                }
            }
            return RedirectToAction("Index");
        }
    }
}
