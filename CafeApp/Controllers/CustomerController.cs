using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using CafeApp.DomainEntity;
using CafeApp.DomainEntity.Interfaces;
using CafeApp.Persistance;
using CafeApp.Persistance.Repositories;
using CafeApp.Persistance.Services;

namespace CafeApp.Controllers
{
    public class CustomerController : Controller
    {
        private CafeWebApp _context { get; set; }
        private iFoodRepository FoodRepository { get; set; }
        private iOrderCartRepository OrderCartRepository { get; set; }
        private iTableRepository TableRepository { get; set; }
        private iUserRepository UserRolesRepository { get; set; }
        private iUserService UserService { get; set; }
        public ActionResult LoginPage(CafeWebApp context)
        {
            _context = context;
            InitializeData();
            return View();
        }
        private void InitializeData()
        {
            FoodRepository = new FoodRepository(_context);
            OrderCartRepository = new OrderCartRepository(_context);
            TableRepository = new TableRepository(_context);
            UserRolesRepository = new UserRepository(_context);
            UserService = new UserService(_context);
        }
        [HttpPost]
        public ActionResult LoginPage(User user)
        {
            if (UserService.GetAllCashier() < 1)
            {
                ViewBag.FailMessage = "Sorry , cashier is unavailable . You cannot login .";
                return View();
            }

            if (UserService.CheckUserCredentials(user, Roles.Customer))
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
            var TableData = OrderCartRepository.CheckSeat(SessionId);
            if (TableData != null)
            {
                ViewBag.TableNo = TableData.TableNo;
            }
            else
            {
                ViewBag.TableNo = "Empty";
            }
            ViewBag.Username = UserRolesRepository.GetUserById(SessionId).Username.ToString();
            ViewBag.Count = OrderCartRepository.FoodCount(SessionId);
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
            string message = OrderCartRepository.Cart(Id, SessionId);

            return Json(new { message }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult OrderCart()
        {
            int SessionId = SessionID();

            ViewBag.Count = OrderCartRepository.FoodCount(SessionId);
            ViewBag.FoodTotalSum = OrderCartRepository.FoodPriceSum(SessionId);
            var TableData = OrderCartRepository.CheckSeat(SessionId);
            if (TableData != null)
            {
                ViewBag.TableStatus = TableData.TableStatus.ToString();
                ViewBag.TableNo = TableData.TableNo;
            }
            return View(OrderCartRepository.OrderedFood(SessionId));
        }
        [HttpPost]
        public ActionResult Quantity(string _operator, int Id)
        {
            int SessionId = SessionID();
            OrderCartRepository.CartQuantity(Id, _operator, SessionId);
            return Json(JsonRequestBehavior.AllowGet);
        }
        public ActionResult ClearCart()
        {
            int SessionId = SessionID();
            OrderCartRepository.ClearCart(SessionId);
            return RedirectToAction("OrderCart");
        }
        public ActionResult CancelOrder()
        {
            int SessionId = SessionID();
            OrderCartRepository.CancelOrder(SessionId);
            return RedirectToAction("OrderCart");
        }
        public ActionResult ConfirmOrderPage()
        {
            int SessionId = SessionID();
            ViewBag.Count = OrderCartRepository.FoodCount(SessionId);
            var emptyseat = OrderCartRepository.GetEmptyTables();
            ViewBag.EmptySeatsList = new SelectList(emptyseat, "TableId", "TableNo");
            var tableData = TableRepository.GetTables();
            var TableStatus = OrderCartRepository.CheckSeat(SessionId);
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
            ViewBag.Count = OrderCartRepository.FoodCount(SessionId);
            var emptyseat = OrderCartRepository.GetEmptyTables();
            ViewBag.EmptySeatsList = new SelectList(emptyseat, "TableId", "TableNo");
            ViewBag.SuccessMessage = OrderCartRepository.ConfirmOrder(SessionId, EmptySeatsList);
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
