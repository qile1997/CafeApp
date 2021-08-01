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
        private CafeWebApp _context = new CafeWebApp();
        private UserRepository _userRepository = new UserRepository();
        private TableRepository TableRepository = new TableRepository();
        private UserService _userService = new UserService();
        private FoodRepository FoodRepository = new FoodRepository();
        private OrderCartRepository _orderCartRepository = new OrderCartRepository();
        public ActionResult LoginPage()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LoginPage(LoginCredentialsViewModel userCredential)
        {
            if (_userService.GetAllCashier() < 1)
            {
                ViewBag.FailMessage = "Sorry , cashier is unavailable . You cannot login .";
                return View();
            }

            var user = _userService.CheckUserCredentials(userCredential, Roles.Customer);

            if (user == null)
            {
                ViewBag.FailMessage = "Your username / password is invalid";
                return View();
            }

            Session["CustomerId"] = user.UserId;
            return RedirectToAction("Menu");
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("LoginPage");
        }
        public ActionResult Menu()
        {
            int SessionId = SessionID();
            var TableData = _orderCartRepository.CheckSeat(SessionId);
            if (TableData != null)
            {
                ViewBag.TableNo = TableData.TableNo;
            }
            else
            {
                ViewBag.TableNo = "Empty";
            }
            ViewBag.Username = _userRepository.GetUserById(SessionId).Username.ToString();
            ViewBag.Count = _orderCartRepository.FoodCount(SessionId);
            return View(FoodRepository.ReadAllFoods());
        }
        public int SessionID()
        {
            return Convert.ToInt32(Session["CustomerId"]);
        }
        [HttpPost]
        public ActionResult Cart(int Id)
        {
            int SessionId = SessionID();
            string message = _orderCartRepository.Cart(Id, SessionId);

            return Json(new { message }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult OrderCart()
        {
            int SessionId = SessionID();

            ViewBag.Count = _orderCartRepository.FoodCount(SessionId);
            ViewBag.FoodTotalSum = _orderCartRepository.FoodPriceSum(SessionId);
            var TableData = _orderCartRepository.CheckSeat(SessionId);
            if (TableData != null)
            {
                ViewBag.TableStatus = TableData.TableStatus.ToString();
                ViewBag.TableNo = TableData.TableNo;
            }
            return View(_orderCartRepository.OrderedFood(SessionId));
        }
        [HttpPost]
        public ActionResult Quantity(string _operator, int Id)
        {
            int SessionId = SessionID();
            _orderCartRepository.CartQuantity(Id, _operator, SessionId);
            return Json(JsonRequestBehavior.AllowGet);
        }
        public ActionResult ClearCart()
        {
            int SessionId = SessionID();
            _orderCartRepository.ClearCart(SessionId);
            return RedirectToAction("OrderCart");
        }
        public ActionResult CancelOrder()
        {
            int SessionId = SessionID();
            _orderCartRepository.CancelOrder(SessionId);
            return RedirectToAction("OrderCart");
        }
        public ActionResult ConfirmOrderPage()
        {
            int SessionId = SessionID();
            ViewBag.Count = _orderCartRepository.FoodCount(SessionId);
            var emptyseat = _orderCartRepository.GetEmptyTables();
            ViewBag.EmptySeatsList = new SelectList(emptyseat, "TableId", "TableNo");
            var tableData = TableRepository.GetTables();
            var TableStatus = _orderCartRepository.CheckSeat(SessionId);
            if (TableStatus != null)
            {
                ViewBag.TableStatus = TableStatus.TableStatus.ToString();
            }
            return View(tableData);
        }
        [HttpPost]
        public ActionResult ConfirmOrderPage(int EmptySeatsList)
        {
            int SessionId = SessionID();
            ViewBag.Count = _orderCartRepository.FoodCount(SessionId);
            var emptyseat = _orderCartRepository.GetEmptyTables();
            ViewBag.EmptySeatsList = new SelectList(emptyseat, "TableId", "TableNo");
            ViewBag.SuccessMessage = _orderCartRepository.ConfirmOrder(SessionId, EmptySeatsList);
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
            Food foods = FoodRepository.GetFoodById(id);
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
            Food foods = FoodRepository.GetFoodById(id);
            FoodRepository.DeleteFood(foods);
            return RedirectToAction("Index");
        }
    }
}
