using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Gift_Card.Models;
namespace Gift_Card.Controllers
{
    public class CustomerController : Controller
    {
        // GET: Customer
        public ActionResult Customer()
        {
            if (Session["email"] != null)
            {
                int id = (int)Session["id"];
                Order ord = new Order();
                //session Update
                List<Order> file = ord.Customer_Repeat_Order(id);
                Session["Repeat_My_Order"] = file.Count();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            

            return View();
        }
        public ActionResult Order()
        {
            if (Session["email"] != null)
            {
                int id = (int)Session["id"];
                Order ord = new Order();
                //session Update
                List<Order> file = ord.Customer_Repeat_Order(id);
                Session["Repeat_My_Order"] = file.Count();

                Product pro = new Product();
                ViewBag.Repeate = pro.Repeat_Order();
                return View(file);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
           
        }
       
    }
}