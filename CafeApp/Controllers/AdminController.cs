using CafeApp.DomainEntity;
using CafeApp.DomainEntity.Interfaces;
using CafeApp.DomainEntity.ViewModel;
using CafeApp.Persistance;
using CafeApp.Persistance.Repositories;
using CafeApp.Persistance.Services;
using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace CafeApp.Controllers
{
    public class AdminController : Controller
    {
        private UserRepository _userRepository = new UserRepository();
        private TableRepository TableRepository = new TableRepository();
        private UserService _userService = new UserService();
        private FoodRepository FoodRepository = new FoodRepository();
        private OrderCartRepository OrderCartRepository = new OrderCartRepository();

        public ActionResult LoginPage()
        {
            _userService.CreateAdmin();
            return View();
        }

        [HttpPost]
        public ActionResult LoginPage(LoginCredentialsViewModel userCredential)
        {
            var user = _userService.CheckUserCredentials(userCredential, Roles.Admin);

            if (user == null)
            {
                ViewBag.FailMessage = "Your username / password is invalid";
                return View();
            }

            Session["AdminId"] = user.UserId;
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
            return View(_userRepository.GetAllUsers());
        }

        // GET: Admin/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (_userRepository.GetUserById(id) == null)
            {
                return HttpNotFound();
            }
            return View(_userRepository.GetUserById(id));
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
                if (_userService.CheckDuplicateUser(userRoles))
                {
                    ViewBag.FailMessage = "This user is already registered";
                    return View();
                }
                _userService.CreateTables(userRoles);
                _userRepository.AddUser(userRoles);
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

            if (_userRepository.GetUserById(id) == null)
            {
                return HttpNotFound();
            }
            return View(_userRepository.GetUserById(id));
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

                if (_userService.CheckDuplicateUser_EditMode(userRoles))
                {
                    ViewBag.FailMessage = "This data is already registered in database";
                    return View();
                }
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
            if (_userRepository.GetUserById(id) == null)
            {
                return HttpNotFound();
            }
            return View(_userRepository.GetUserById(id));
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _userRepository.DeleteUser(_userRepository.GetUserById(id));

            if (_userRepository.GetUserById(id).Roles == Roles.Cashier && _userRepository.GetAllCashier().Count() == 0)
            {
                TableRepository.DeleteAllTables();
            }
            return RedirectToAction("Index");
        }
    }
}
