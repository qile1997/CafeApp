using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using CafeApp.DomainEntity;
using CafeApp.DomainEntity.Interfaces;
using CafeApp.DomainEntity.ViewModel;
using CafeApp.Persistance;
using CafeApp.Persistance.Repositories;
using CafeApp.Persistance.Services;

namespace CafeApp.Controllers
{
    public class CustomerController : Controller
    {
        private UserRepository _userRepository = new UserRepository();
        private TableRepository _tableRepository = new TableRepository();
        private UserService _userService = new UserService();
        private FoodRepository FoodRepository = new FoodRepository();
        private OrderCartRepository _orderCartRepository = new OrderCartRepository();
        private OrderCartService _orderCartService = new OrderCartService();
        public ActionResult LoginPage()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LoginPage(LoginCredentialsViewModel userCredential)
        {
            if (_userRepository.GetAllCashier().Count() < 1)
            {
                ViewBag.FailMessage = "Sorry , cashier is unavailable . You cannot login .";
                return View();
            }

            if (_userService.CheckUserCredentials(userCredential, Roles.Customer) == null)
            {
                ViewBag.FailMessage = "Your username / password is invalid";
                return View();
            }

            Session["CustomerId"] = _userService.CheckUserCredentials(userCredential, Roles.Customer).UserId;
            return RedirectToAction("Menu");
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("LoginPage");
        }
        public ActionResult Menu()
        {
            if (_orderCartService.CheckUserTable(Convert.ToInt32(Session["CustomerId"])) != null)
            {
                ViewBag.TableNo = _orderCartService.CheckUserTable(Convert.ToInt32(Session["CustomerId"])).TableNo;
            }
            else
            {
                ViewBag.TableNo = "Empty";
            }
            ViewBag.Username = _userRepository.GetUserById(Convert.ToInt32(Session["CustomerId"])).Username.ToString();
            ViewBag.Count = _orderCartService.FoodCount(Convert.ToInt32(Session["CustomerId"]));
            return View(FoodRepository.ReadAllFoods());
        }

        [HttpPost]
        public ActionResult Cart(int Id)
        {
            string message = _orderCartService.UserOrderCartService(Id, Convert.ToInt32(Session["CustomerId"]));

            return Json(new { message }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult OrderCart()
        {
            ViewBag.Count = _orderCartService.FoodCount(Convert.ToInt32(Session["CustomerId"]));
            ViewBag.FoodTotalSum = _orderCartService.FoodPriceSum(Convert.ToInt32(Session["CustomerId"]));

            if (_orderCartService.CheckUserTable(Convert.ToInt32(Session["CustomerId"])) != null)
            {
                ViewBag.TableStatus = _orderCartService.CheckUserTable(Convert.ToInt32(Session["CustomerId"])).TableStatus.ToString();
                ViewBag.TableNo = _orderCartService.CheckUserTable(Convert.ToInt32(Session["CustomerId"])).TableNo;
            }
            return View(_orderCartService.UserOrderedFoodList(Convert.ToInt32(Session["CustomerId"])));
        }

        [HttpPost]
        public ActionResult Quantity(string _operator, int FoodId)
        {
            _orderCartService.UserOrderCartQuantityService(FoodId, _operator, Convert.ToInt32(Session["CustomerId"]));
            return Json(JsonRequestBehavior.AllowGet);
        }

        public ActionResult ClearCart()
        {
            _orderCartService.ClearUserCartService(Convert.ToInt32(Session["CustomerId"]));
            return RedirectToAction("OrderCart");
        }

        public ActionResult CancelOrder()
        {
            _orderCartService.CancelUserOrderService(Convert.ToInt32(Session["CustomerId"]));
            return RedirectToAction("OrderCart");
        }

        public ActionResult ConfirmOrderPage()
        {
            ViewBag.Count = _orderCartService.FoodCount(Convert.ToInt32(Session["CustomerId"]));
            ViewBag.EmptySeatsList = new SelectList(_orderCartRepository.GetEmptyStatusTables(), "TableId", "TableNo");

            if (_orderCartService.CheckUserTable(Convert.ToInt32(Session["CustomerId"])) != null)
            {
                ViewBag.TableStatus = _orderCartService.CheckUserTable(Convert.ToInt32(Session["CustomerId"])).TableStatus.ToString();
            }
            return View(_tableRepository.GetAllTables());
        }

        [HttpPost]
        public ActionResult ConfirmOrderPage(int EmptySeatsList)
        {
            ViewBag.Count = _orderCartService.FoodCount(Convert.ToInt32(Session["CustomerId"]));
            ViewBag.EmptySeatsList = new SelectList(_orderCartRepository.GetEmptyStatusTables(), "TableId", "TableNo");
            ViewBag.SuccessMessage = _orderCartService.ConfirmUserOrderService(Convert.ToInt32(Session["CustomerId"]), EmptySeatsList);
            return View("ConfirmOrderSuccess");
        }
        // GET: Customer
        public ActionResult Index()
        {
            return View(FoodRepository.ReadAllFoods());
        }

        // GET: Customer/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Food foods = FoodRepository.GetFoodById(id);
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
        public ActionResult Create([Bind(Include = "FoodsId,FoodCategory,FoodName,Price,Remarks")] Food foods)
        {
            if (ModelState.IsValid)
            {
                FoodRepository.CreateFood(foods);
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
            Food foods = FoodRepository.GetFoodById(id);
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
        public ActionResult Edit([Bind(Include = "FoodsId,FoodCategory,FoodName,Price,Remarks")] Food foods)
        {
            if (ModelState.IsValid)
            {
                FoodRepository.UpdateFood(foods);
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

            if (FoodRepository.GetFoodById(id) == null)
            {
                return HttpNotFound();
            }
            return View(FoodRepository.GetFoodById(id));
        }

        // POST: Customer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FoodRepository.DeleteFood(FoodRepository.GetFoodById(id));
            return RedirectToAction("Index");
        }
    }
}
