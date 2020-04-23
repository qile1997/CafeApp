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

namespace CafeApp.Controllers
{
    public class CashierController : Controller
    {
        private CafeWebApp db = new CafeWebApp();
        private LoginRepository loginRepo = new LoginRepository();
        private TableRepository tableRepo = new TableRepository();
        public ActionResult LoginPage()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LoginPage(UserRoles userRoles)
        {
            UserRoles user = loginRepo.Login(userRoles);

            if (user == null || user.Roles != Roles.Cashier)
            {
                ViewBag.FailMessage = "Your username / password is invalid";
                return View();
            }
            Session["CashierId"] = user.UserRolesId;
            return RedirectToAction("Tables");
        }
        // GET: Cashier
        public ActionResult Index()
        {
            bool check = tableRepo.GetTableStatus();
            if (check)
            {
                ViewBag.Message = "";
                return View(tableRepo.GetTables());
            }
            return View(tableRepo.GetTables());
        }
        public ActionResult Tables()
        {
            return View(tableRepo.GetTables());
        }

        // GET: Cashier/Create
        public ActionResult Create()
        {
            tableRepo.CreateTable();
            return RedirectToAction("Index");
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("LoginPage");
        }
        public ActionResult Delete(int? id)
        {
            Table table = tableRepo.table(id);
            tableRepo.DeleteTable(table);

            return RedirectToAction("Index");
        }
        public ActionResult ChangeStatus(int id, int SessionId)
        {
            Table table = tableRepo.table(id);
            tableRepo.ChangeStatus(table, SessionId);
            return RedirectToAction("Index");
        }
        public ActionResult ReorderTables()
        {
            //bool check = tableRepo.GetTableStatus();
            //if (check)
            //{
            //    ViewBag.Message = "Customers still eating . Cant re-order tables now.";
            //    return RedirectToAction("Index");
            //}
            tableRepo.ReorderTables();
            return RedirectToAction("Index");
        }
    }
}
