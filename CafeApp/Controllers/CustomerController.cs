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
        private TableRepository tableRepo = new TableRepository();
        private UserRolesRepository userRepo = new UserRolesRepository();
        public ActionResult LoginPage()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LoginPage(UserRoles userRoles)
        {
            UserRoles user = loginRepo.Login(userRoles);
            int Count = userRepo.FilterCashier();
                                                                    
            if (Count < 1)
            {
                ViewBag.FailMessage = "Sorry , cashier is unavailable . You cannot login .";
                return View();
            }

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
            return RedirectToAction("LoginPage");
        }
        public ActionResult Menu()
        {
            int SessionId = SessionID();
            var TableData = orderRepo.CheckSeat(SessionId);
            if (TableData != null)
            {
                ViewBag.TableNo = TableData.TableNo;
            }
            else
            {
                ViewBag.TableNo = "Empty";
            }
            ViewBag.Username = userRepo.userRoles(SessionId).Username.ToString();
            ViewBag.Count = orderRepo.FoodCount(SessionId);
            return View(foodRepo.GetFoods());
        }
        public int SessionID()
        {
            int SessionId = Convert.ToInt32(Session["CustomerId"]);
            return SessionId;
        }
        [HttpPost]
        public ActionResult Cart(int Id)
        {
            int SessionId = SessionID();
            string message = orderRepo.Cart(Id, SessionId);

            return Json(new { message }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult OrderCart()
        {
            int SessionId = SessionID();

            ViewBag.Count = orderRepo.FoodCount(SessionId);
            ViewBag.FoodTotalSum = orderRepo.FoodPriceSum(SessionId);
            var TableData = orderRepo.CheckSeat(SessionId);
            if (TableData != null)
            {
                ViewBag.TableStatus = TableData.TableStatus.ToString();
                ViewBag.TableNo = TableData.TableNo;
            }      
            return View(orderRepo.OrderedFood(SessionId));
        }
        [HttpPost]
        public ActionResult Quantity(string _operator, int Id)
        {
            orderRepo.CartQuantity(Id, _operator);
            return Json(JsonRequestBehavior.AllowGet);
        }
        public ActionResult ClearCart()
        {
            int SessionId = SessionID();
            orderRepo.ClearCart(SessionId);
            return RedirectToAction("OrderCart");
        }
        public ActionResult CancelOrder()
        {
            int SessionId = SessionID();
            orderRepo.CancelOrder(SessionId);
            return RedirectToAction("OrderCart");
        }
        public ActionResult ConfirmOrderPage()
        {
            int SessionId = SessionID();
            ViewBag.Count = orderRepo.FoodCount(SessionId);
            var emptyseat = orderRepo.GetEmptyTables();
            ViewBag.EmptySeatsList = new SelectList(emptyseat, "TableId", "TableNo");
            var tableData = tableRepo.GetTables();
            var TableStatus = orderRepo.CheckSeat(SessionId);
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
            ViewBag.Count = orderRepo.FoodCount(SessionId);
            var emptyseat = orderRepo.GetEmptyTables();
            ViewBag.EmptySeatsList = new SelectList(emptyseat, "TableId", "TableNo");
            ViewBag.SuccessMessage = orderRepo.ConfirmOrder(SessionId, EmptySeatsList);
            return View("ConfirmOrderSuccess");
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
