using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using CafeApp.DomainEntity;
using CafeApp.Persistance;
using CafeApp.Persistance.Repositories;

namespace CafeApp.Controllers
{
    public class CustomerController : Controller
    {
        private FoodRepository foodRepo = new FoodRepository();
        private LoginRepository loginRepo = new LoginRepository();
        private OrderCartRepository orderRepo = new OrderCartRepository();
        public ActionResult LoginPage()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LoginPage(UserRoles userRoles)
        {
            UserRoles user = loginRepo.Login(userRoles);

            if (user == null || user.Roles != Roles.Customer)
            {
                ViewBag.FailMessage = "Your username / password is invalid";
                return View();
            }
            Session["CustomerId"] = user.UserRolesId;
            return RedirectToAction("Menu");
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            return View("LoginPage");
        }
        public ActionResult Menu()
        {
            int checkId = SessionID();
            ViewBag.Count = orderRepo.FoodCount(checkId);
            return View(foodRepo.GetFoods());
        }
        public int SessionID()
        {
            int checkId = Convert.ToInt32(Session["CustomerId"]);
            return checkId;
        }
        [HttpPost]
        public ActionResult Cart(int Id)
        {
            int checkId = SessionID();
            string message = orderRepo.Cart(Id, checkId);

            return Json(new { message }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult OrderCart()
        {
            int checkId = SessionID();

            ViewBag.Count = orderRepo.FoodCount(checkId);
            ViewBag.FoodTotalSum = orderRepo.FoodPriceSum(checkId);
            return View(orderRepo.OrderedFood(checkId));
        }
        [HttpPost]
        public ActionResult Quantity(string _operator, int Id)
        {
            string successmessage = orderRepo.CartQuantity(Id, _operator);
            return Json(successmessage, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ClearCart()
        {
            string message = orderRepo.ClearCart();
            return Json(new { message }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ConfirmOrder(int Seat)
        {
            int checkId = SessionID();
            string message = orderRepo.ConfirmOrder(checkId, Seat);
            return Json(new { message }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult ConfirmOrderPage()
        {
            int checkId = SessionID();
            ViewBag.Count = orderRepo.FoodCount(checkId);
            var emptyseat = orderRepo.GetEmptyTables();
            ViewBag.EmptySeatsList = new SelectList(emptyseat, "TableId", "TableNo");

            return View();
        }
        // GET: Customer
        public ActionResult Index()
        {
            return View(foodRepo.GetFoods());
        }

        // GET: Customer/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Foods foods = foodRepo.food(id);
            if (foods == null)
            {
                return HttpNotFound();
            }
            return View(foods);
        }

        // GET: Customer/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customer/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FoodsId,FoodCategory,FoodName,Price,Remarks")] Foods foods)
        {
            if (ModelState.IsValid)
            {
                foodRepo.AddFood(foods);
                return RedirectToAction("Index");
            }

            return View(foods);
        }

        // GET: Customer/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Foods foods = foodRepo.food(id);
            if (foods == null)
            {
                return HttpNotFound();
            }
            return View(foods);
        }

        // POST: Customer/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FoodsId,FoodCategory,FoodName,Price,Remarks")] Foods foods)
        {
            if (ModelState.IsValid)
            {
                foodRepo.UpdateFood(foods);
                return RedirectToAction("Index");
            }
            return View(foods);
        }

        // GET: Customer/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Foods foods = foodRepo.food(id);
            if (foods == null)
            {
                return HttpNotFound();
            }
            return View(foods);
        }

        // POST: Customer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Foods foods = foodRepo.food(id);
            foodRepo.DeleteFood(foods);
            return RedirectToAction("Index");
        }

    }
}
