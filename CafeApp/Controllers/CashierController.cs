using CafeApp.DomainEntity;
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
            var user = _userService.CheckUserCredentials(userCredential, Roles.Cashier);

            if (user == null)
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
            bool check = _tableRepository.GetTableStatus();
            if (check)
            {
                ViewBag.Message = "";
                return View(_tableRepository.GetTables());
            }
            return View(_tableRepository.GetTables());
        }
        public ActionResult Tables()
        {
            return View(_tableRepository.GetTables());
        }

        // GET: Cashier/Create
        public ActionResult Create()
        {
            _tableRepository.CreateTable();
            return RedirectToAction("Index");
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("LoginPage");
        }
        public ActionResult Delete(int? id)
        {
            Table table = _tableRepository.table(id);
            _tableRepository.DeleteTable(table);

            return RedirectToAction("Index");
        }
        public ActionResult ChangeStatus(int id, int SessionId)
        {
            Table table = _tableRepository.table(id);
            _tableRepository.ChangeStatus(table, SessionId);
            return RedirectToAction("Index");
        }
        public ActionResult ReorderTables()
        {
            _tableRepository.ReorderTables();
            return RedirectToAction("Index");
        }
    }
}
