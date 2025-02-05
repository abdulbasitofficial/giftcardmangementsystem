using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using Gift_Card.Models;
using System.Web.Security;
using System.Linq;

namespace Gift_Card.Controllers
{
    [SessionState(SessionStateBehavior.Default)]
    public class AccountController : Controller
    {
        DateTime Now_Date = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        GiftCardEntities ctx = new GiftCardEntities();
        public object PostedFile { get; private set; }
        // GET: Account
        [HttpGet]

        public ActionResult Admin()
        {

            return View();
        }

        [HttpGet]
        public ActionResult Account(int page = 1)
        {
            try
            {
                Account user = new Account();
                List<Account> files = user.dispaly_Account();

                ViewBag.TotalPages = (files.Count() / 20) + 1;
                files = files.Skip((page - 1) * 20).Take(20).ToList();
                return View(files);
            }
            catch (Exception e)
            {
                return View(e);
            }

        }
        [HttpGet]
        [AllowAnonymous]

        public ActionResult Login()
        {
            try
            {
                return View();
            }
            catch (Exception e)
            {
                return View(e);
            }

        }
        [HttpPost]

        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Account c)
        {
            try
            {
                Account user = new Account();
                Account file = user.Login(c);
                if (file != null)
                {
                    Session["name"] = file.Full_Name;
                    Session["id"] = file.id;
                    Session["contact"] = file.Contact;
                    Session["address"] = file.Address;
                    Session["email"] = file.Email;
                    Session["type"] = file.User_Type;
                    Session["image"] = file.Account_img;
                    Session["img-type"] = file.type_img;

                    Cart car = new Cart();
                    Session["cart_count"] = car.count_Item();

                    if (Session["type"].ToString().Equals("Admin"))
                    {
                        return RedirectToAction("Account", "Account");
                    }
                    else
                    {
                        return RedirectToAction("Product", "Home");
                    }
                }
                else
                {
                    ViewBag.Res = "Invalid Users Record";
                    return View();
                }
            }
            catch (Exception e)
            {
                return View(e);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult LogOut()
        {
            try
            {
                Session.RemoveAll(); //Clear all session variables
                return RedirectToAction("Product", "Home");
            }
            catch (Exception e)
            {
                return View(e);
            }
        }
        [HttpGet]
        [AllowAnonymous]

        public ActionResult SignUp()
        {
            try
            {
                return View();
            }
            catch (Exception e)
            {
                return View(e);
            }

        }

        [HttpPost]
        public ActionResult SignUp(Account a)
        {

            Account user = new Account();
            user.insert_Account(a);
            ViewBag.Res = "Successfull Registration";
            Account file = user.Login(a);
            if (file != null)
            {
                Session["name"] = file.Full_Name;
                Session["id"] = file.id;
                Session["contact"] = file.Contact;
                Session["address"] = file.Address;
                Session["email"] = file.Email;
                Session["type"] = file.User_Type;
                Session["image"] = file.Account_img;
                Session["img-type"] = file.type_img;

                Cart car = new Cart();
                Session["cart_count"] = car.count_Item();

                if (Session["type"].ToString().Equals("Admin"))
                {
                    return RedirectToAction("Account", "Account");
                }
                else if (Session["type"].ToString().Equals("DeliveryBoy"))
                {
                    return RedirectToAction("DeliveryBoy", "DeliveryBoy");
                }
                else
                {
                    return RedirectToAction("Product", "Home");
                }
            }
            else
            {
                ViewBag.Res = "Invalid Users Record";
                return View();
            }
        }
            
        [HttpGet]
        public ActionResult UserProfile()
        {
            try
            {
                return View();
            }
            catch (Exception e)
            {
                return View(e);
            }

        }
        [HttpGet]
        [AllowAnonymous]

        public ActionResult Delete(int id)
        {
            try
            {
                Account user = new Account();
                user.delete_Account(id);
                return RedirectToAction("Account");
            }
            catch (Exception e)
            {
                return View(e);
            }

        }

        [HttpGet]
        [AllowAnonymous]

        public ActionResult Edit(int id)
        {
            try
            {
                Account user = new Account();
                Account files = user.update_Account(id);
                return View(files);
            }
            catch (Exception e)
            {
                return View(e);
            }

        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Account a, HttpPostedFileBase PostedFile)
        {
            try
            {
                Account user = new Account();
                user.update_Account(a, PostedFile);
                return RedirectToAction("Account");
            }
            catch (Exception e)
            {
                return View(e);
            }

        }
        public ActionResult Profile_picture(HttpPostedFileBase PostedFile)
        {
            try
            {
                int id = (int)Session["id"];
                Account user = new Account();
                user.update_Picture(PostedFile, id);
                Account file = user.update_Account(id);

                Session["name"] = file.Full_Name;
                Session["id"] = file.id;
                Session["gender"] = file.Gender;
                Session["contact"] = file.Contact;
                Session["address"] = file.Address;
                Session["email"] = file.Email;
                Session["type"] = file.User_Type;
                Session["image"] = file.Account_img;
                Session["img-type"] = file.type_img;

                if (Session["type"].ToString().Equals("Admin"))
                {
                    return RedirectToAction("Account", "Account");
                }
                else if (Session["type"].ToString().Equals("DeliveryBoy"))
                {
                    return RedirectToAction("DeliveryBoy", "DeliveryBoy");
                }
                else
                {
                    return RedirectToAction("Customer", "Customer");
                }
            }
            catch (Exception e)
            {
                return View(e);
            }

        }
    }
}
