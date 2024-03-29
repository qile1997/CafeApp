﻿using CafeApp.DomainEntity;
using CafeApp.DomainEntity.Interfaces;
using CafeApp.DomainEntity.ViewModel;
using CafeApp.Persistance;
using CafeApp.Persistance.Repositories;
using CafeApp.Persistance.Services;
using System.Web.Mvc;

namespace CafeApp.Controllers
{
    public class CashierController : Controller
    {
        private UserRepository _userRepository = new UserRepository();
        private TableRepository _tableRepository = new TableRepository();
        private TableService _tableService = new TableService();
        private UserService _userService = new UserService();
        private FoodRepository _foodRepository = new FoodRepository();
        private OrderCartRepository _orderCartRepository = new OrderCartRepository();
        public ActionResult LoginPage()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LoginPage(LoginCredentialsViewModel userCredential)
        {
            if (_userService.CheckUserCredentials(userCredential, Roles.Cashier) == null)
            {
                ViewBag.FailMessage = "Your username / password is invalid";
                return View();
            }
            Session["CashierId"] = _userService.CheckUserCredentials(userCredential, Roles.Cashier).UserId;
            return RedirectToAction("Tables");
        }
        // GET: Cashier
        public ActionResult Index()
        {
            return View(_tableRepository.GetAllTables());
        }

        public ActionResult Tables()
        {
            return View(_tableRepository.GetAllTables());
        }

        // GET: Cashier/Create
        public ActionResult Create()
        {
            _tableRepository.CreateTableWithOneClick();
            return RedirectToAction("Index");
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("LoginPage");
        }

        public ActionResult Delete(int? id)
        {
            _tableRepository.DeleteTable(_tableRepository.GetTableById(id));
            _tableService.ReorderTables();
            return RedirectToAction("Index");
        }

        public ActionResult ChangeStatus(int id, int SessionId)
        {
            _tableService.ChangeTableStatus(_tableRepository.GetTableById(id), SessionId);
            return RedirectToAction("Index");
        }

    }
}
