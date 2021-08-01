using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using CafeApp.Persistance.Repositories;
using CafeApp.DomainEntity;
using CafeApp.Persistance;
using System.Collections.Generic;
using CafeApp.DomainEntity.Interfaces;
using CafeApp.Persistance.Services;

namespace CafeApp.Controllers
{
    public class CashierController : Controller
    {
        private CafeWebApp _context { get; set; }
        public iLoginService LoginService { get; set; }
        public iTableRepository TableRepository { get; set; }
        public ActionResult LoginPage(CafeWebApp context)
        {
            _context = context;
            InitializeData();
            return View();
        }
        public void InitializeData()
        {
            LoginService = new LoginService(_context);
            TableRepository = new TableRepository(_context);
        }
        [HttpPost]
        public ActionResult LoginPage(User userRoles)
        {
            User user = LoginService.LoginUserRole(Roles.Cashier, userRoles);

            if (user == null || user.Roles != Roles.Cashier)
            {
                ViewBag.FailMessage = "Your username / password is invalid";
                return View();
            }
            Session["CashierId"] = user.UserId;
            return RedirectToAction("Tables");
        }
        // GET: Cashier
        public ActionResult Index()
        {
            bool check = TableRepository.GetTableStatus();
            if (check)
            {
                ViewBag.Message = "";
                return View(TableRepository.GetTables());
            }
            return View(TableRepository.GetTables());
        }
        public ActionResult Tables()
        {
            return View(TableRepository.GetTables());
        }

        // GET: Cashier/Create
        public ActionResult Create()
        {
            TableRepository.CreateTable();
            return RedirectToAction("Index");
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("LoginPage");
        }
        public ActionResult Delete(int? id)
        {
            Table table = TableRepository.table(id);
            TableRepository.DeleteTable(table);

            return RedirectToAction("Index");
        }
        public ActionResult ChangeStatus(int id, int SessionId)
        {
            Table table = TableRepository.table(id);
            TableRepository.ChangeStatus(table, SessionId);
            return RedirectToAction("Index");
        }
        public ActionResult ReorderTables()
        {
            TableRepository.ReorderTables();
            return RedirectToAction("Index");
        }
    }
}
