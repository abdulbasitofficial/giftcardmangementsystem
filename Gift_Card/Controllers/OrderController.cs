using Gift_Card.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

namespace Gift_Card.Controllers
{
    [SessionState(SessionStateBehavior.Default)]
    public class OrderController : Controller
    {
        // GET: Order

        [HttpPost]
        public ActionResult Insert_Order(string address)
        {
            if (Session["email"] != null)
            {
                int id = (int)Session["id"];
                Order pen = new Order();
                pen.insert_Order(id, address);
                return RedirectToAction("Product", "Home");
            }
            else
            {
                return RedirectToAction("Product", "Home");
            }

        }

        public ActionResult Order()
        {
            return View();
        }
        public ActionResult PendingOrder()
        {
            Order ord = new Order();
            Account acc = new Account();
            List<Account> files = ord.Pending_Order_Admin();
            Session["Pending_Order"] = files.Count();
            ViewBag.DeliveryBoy = "";
            ViewBag.DeliveryBoyAssign = "";
            return View(files);
        }
        public ActionResult OrdersDetailsAdmin(int id, string name, string address)
        {
            Order ord = new Order();
            List<Product> files = ord.OrderDetails(id);
            ViewBag.Name = name;
            ViewBag.Address = address;
            return View(files);
        }
        public ActionResult OrdersDetailsUser(int id, string name)
        {
            Order ord = new Order();
            List<Product> files = ord.OrderDetails(id);
            ViewBag.Name = name;
            return View(files);
        }
        public ActionResult Cancel(int id)
        {
            string status = "Cancel";
            Order ord = new Order();
            ord.Cancel(id, status);
            return RedirectToAction("PendingOrder", "Home");
        }
        public ActionResult Delivered(int id)
        {
            string status = "Delivered";
            Order ord = new Order();
            ord.Delivered(id, status);
            return RedirectToAction("PendingOrder", "Order");
        }
    }
}