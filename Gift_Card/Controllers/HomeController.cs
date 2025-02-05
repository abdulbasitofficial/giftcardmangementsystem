using Gift_Card.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

namespace Gift_Card.Controllers
{
    [SessionState(SessionStateBehavior.Default)]

    public class HomeController : Controller
    {
        GiftCardEntities ctx = new GiftCardEntities();
        // GET: Home
        [HttpGet]
        public ActionResult Product(int page = 1)
        {
            Cart car = new Cart();
            Session["cart_count"] = car.count_Item();
            if (Session["id"] != null)
            {
                int u_id = (int)Session["id"];
                ViewBag.CartCheck = car.Check_Cart_Product(u_id);
            }
            Product pro = new Product();
            IList<Product> files = pro.Home_Product();

            ViewBag.TotalPages = (files.Count() / 30) + 1;
            files = files.Skip((page - 1) * 30).Take(30).ToList();



            return View(files);

        }
        public ActionResult Search(string name, int page = 1)
        {
            if (name != "")
            {
                Product pro = new Product();
                List<Product> files = new List<Product>();
                files = pro.Search_Product(name);
                ViewBag.TotalPages = (files.Count() / 30) + 1;
                files = files.Skip((page - 1) * 30).Take(30).ToList();
                return View("Product", files);
            }
            else
            {
                return RedirectToAction("Product");
            }
        }
    }
}