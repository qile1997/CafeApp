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
        private CafeWebApp db = new CafeWebApp();
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
            loginRepo.Login(userRoles);
            UserRoles user = loginRepo.Login(userRoles);

            if (user.Roles == Roles.Customer)
            {
                Session["CustomerId"] = user.UserRolesId;
                return RedirectToAction("Index");
            }

            return View();
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            return View("LoginPage");
        }
        public ActionResult Menu()
        {
            var checkId = Convert.ToInt32(Session["CustomerId"]);
            ViewBag.Count = orderRepo.FoodCount(checkId);
            return View(foodRepo.GetFoods());
        }
        [HttpPost]
        public ActionResult Cart(int Id)
        {
            var checkId = Convert.ToInt32(Session["CustomerId"]);
            OrderCart cart = new OrderCart();

            var filterCart = db.OrderCart.Where(d => d.FoodsId == Id && d.UserRolesId == checkId).SingleOrDefault();
            var filterFood = db.Foods.Where(d => d.FoodsId == Id).SingleOrDefault();

            if (filterCart != null)
            {
                filterCart.FoodQuantity++;
                filterCart.TotalAmount = filterFood.Price * filterCart.FoodQuantity;
                db.SaveChanges();
                return Json(new { Msg = filterFood.FoodName + " added into quantity order" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                cart.FoodsId = Id;
                cart.TotalAmount = filterFood.Price;
                cart.FoodQuantity = 1;
                cart.UserRolesId = checkId;

                db.OrderCart.Add(cart);
                db.SaveChanges();
                return Json(new { Msg = filterFood.FoodName + " added into order" }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult OrderCart()
        {
            var checkId = Convert.ToInt32(Session["CustomerId"]);

            ViewBag.Count = orderRepo.FoodCount(checkId);
            ViewBag.FoodTotalSum = orderRepo.FoodPriceSum(checkId);
            return View(orderRepo.OrderedFood(checkId));
        }
        [HttpPost]
        public ActionResult Quantity(string _operator, int Id)
        {
            var checkId = Convert.ToInt32(Session["CustomerId"]);
            var filterCart = db.OrderCart.Where(d => d.FoodsId == Id).SingleOrDefault();
            var filterFood = db.Foods.Where(d => d.FoodsId == Id).SingleOrDefault();
            if (_operator == "+")
            {
                filterCart.FoodQuantity++;
                filterCart.TotalAmount = filterFood.Price * filterCart.FoodQuantity;
                db.SaveChanges();
                return Json(new { CartMessage = true, Msg = "Quantity Updated" }, JsonRequestBehavior.AllowGet);
            }
            else if (_operator == "x")
            {
                db.OrderCart.Remove(filterCart);
                db.SaveChanges();
                return Json(new { CartMessage = false, Msg = "Food Removed" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (filterCart.FoodQuantity > 0)
                {
                    filterCart.FoodQuantity--;
                    if (filterCart.FoodQuantity == 0)
                    {
                        db.OrderCart.Remove(filterCart);
                        db.SaveChanges();
                        return Json(new { CartMessage = false, Msg = "Food Removed" }, JsonRequestBehavior.AllowGet);
                    }
                    filterCart.TotalAmount = filterFood.Price * filterCart.FoodQuantity;
                    db.SaveChanges();
                    return Json(new { CartMessage = false, Msg = "Quantity Deleted" }, JsonRequestBehavior.AllowGet);
                }
            }
            return View();
        }
        public ActionResult ClearCart()
        {
            foreach (var item in db.OrderCart)
            {
                db.OrderCart.Remove(item);
            }

            db.SaveChanges();
            return Json(new { Msg = "Cart Cleared" }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ConfirmOrder(int Seat)
        {
            int checkId = Convert.ToInt32(Session["CustomerId"]);
            var checkSeat = db.Table.Where(d => d.UserRolesId == checkId).SingleOrDefault();
            var replaceSeat = db.Table.Where(d => d.TableId == Seat).SingleOrDefault();
            var filterfood = db.OrderCart.Where(d => d.UserRolesId == checkId).ToList();

            if (filterfood.Count < 1)
            {
                return Json(new { ConfirmSeat = false, Msg = "No food in order cart. Cannot confirm order" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (checkSeat != null)
                {
                    return Json(new { ConfirmSeat = false, Msg = "Your order is already confirmed, your table seat is " + replaceSeat.TableNo }, JsonRequestBehavior.AllowGet);
                }

                replaceSeat.UserRolesId = checkId;
                replaceSeat.TableStatus = TableStatus.Occupied;
                db.SaveChanges();

                return Json(new { ConfirmSeat = true, Msg = "Order confirmed, your table seat is " + replaceSeat.TableNo }, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult ConfirmOrderPage()
        {
            var checkId = Convert.ToInt32(Session["CustomerId"]);
            var filterCount = db.OrderCart.Where(d => d.UserRolesId == checkId).ToList();
            int Count = 0;

            foreach (var item in filterCount)
            {
                Count += item.FoodQuantity;
            }
            ViewBag.Count = Count;

            var filterEmptyseats = db.Table.Where(d => d.TableStatus == TableStatus.Empty).ToList();

            ViewBag.EmptySeatsList = new SelectList(filterEmptyseats, "TableId", "TableNo");

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
