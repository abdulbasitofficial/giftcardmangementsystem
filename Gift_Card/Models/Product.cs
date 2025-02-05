using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace Gift_Card.Models
{
    public class Product
    {
        DateTime Now_Date = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        public int Product_ID { get; set; }

        [Required(ErrorMessage = "Please Enter Product Name")]
        [Display(Name = "Product Name")]
        [StringLength(100, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 2)]
        [RegularExpression(@"^(([A-za-z]+[\s]{1}[A-za-z]+)|([A-Za-z]+))$", ErrorMessage = "Please enter a valid Name")]
        public string Product_Name { get; set; }


        [Required(ErrorMessage = "Please Enter Product Price")]
        [Display(Name = "Product Price")]

        public int Product_Price { get; set; }

        [Required(ErrorMessage = "Please Enter Product Description")]
        [Display(Name = "Product Description")]
        [StringLength(200, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 10)]
        public string Product_Deacription { get; set; }


        [Display(Name = "Product Image")]
        public byte[] Product_img { get; set; }

        public string Type { get; set; }

        // public string img64 { get; set; }
        public string img64url { get; set; }

        public DateTime odate { get; set; }
        public string contact { get; set; }
        public string boyname { get; set; }
        public int uid { get; set; }
        public int rid { get; set; }
        public int Order_No { get; set; }
        public int Product_Quantity { get; set; }
        public string status { get; set; }
        public bool IsActive { get; set; }
        public DateTime Insert_Date { get; set; }

        GiftCardEntities ctx = new GiftCardEntities();
        public void insert_Product(Product p, HttpPostedFileBase PostedFile)
        {
            byte[] bytes;
            BinaryReader br = new BinaryReader(PostedFile.InputStream);
            bytes = br.ReadBytes(PostedFile.ContentLength);

            product pro = new product();
            pro.p_name = p.Product_Name;
            pro.p_price = p.Product_Price;
            pro.p_description = p.Product_Deacription;
            pro.p_img = bytes;
            pro.p_img_type = PostedFile.ContentType;
            pro.i_date = Now_Date;
            pro.isActive = true;
            ctx.products.Add(pro);
            ctx.SaveChanges();

        }
        public List<Product> dispaly_Product()
        {
            List<Product> files = null;

            files = ctx.products.Select(p => new Product()
            {
                Product_ID = p.p_id,
                Product_Name = p.p_name,
                Product_Deacription = p.p_description,
                Product_img = p.p_img,
                IsActive = (bool)p.isActive,
                Product_Price = (int)p.p_price,
                Insert_Date = (DateTime)p.i_date,
                Type = p.p_img_type,
            }).OrderBy(e => e.Product_Name).ToList<Product>();


            return files;
        }
        public List<Product> Home_Product()
        {

            List<Product> files = null;

            files = ctx.products.Where(w=>w.isActive==true).Select(p => new Product()
            {
                Product_ID = p.p_id,
                Product_Name = p.p_name,
                Product_Deacription = p.p_description,
                Product_img = p.p_img,
                IsActive = (bool)p.isActive,
                Product_Price = (int)p.p_price,
                Insert_Date = (DateTime)p.i_date,
                Type = p.p_img_type,
            }).OrderBy(e => e.Product_Name).ToList<Product>();


            return files;
        }
        public void delete_Product(int id, bool IsActive)
        {
            var result = ctx.products.FirstOrDefault(b => b.p_id == id);
            if (result != null)
            {
                result.isActive = IsActive;
                ctx.SaveChanges();
            }
        }
        public Product upadate_Product(int id)
        {
            Product files = single_Product(id);
            return files;
        }

        public void upadate_Product(Product p, HttpPostedFileBase PostedFile)
        {

            var result = ctx.products.FirstOrDefault(b => b.p_id == p.Product_ID);
            if (result != null)
            {
                result.p_name = p.Product_Name;
                result.p_price = p.Product_Price;
                result.p_description = p.Product_Deacription;
                ctx.SaveChanges();
            }

        }
        public Product single_Product(int id)
        {
            Product files = null;

            files = ctx.products.Where(p => p.p_id == id).Select(p => new Product()
            {
                Product_ID = p.p_id,
                Product_Name = p.p_name,
                Product_Deacription = p.p_description,
                Product_img = p.p_img,
                Product_Price = (int)p.p_price,
                Type = p.p_img_type

            }).SingleOrDefault();

            return files;
        }


        public List<Product> Search_Product(string name)
        {
            List<Product> files = null;

            files = ctx.products.Select(p => new Product()
            {
                Product_ID = p.p_id,
                Product_Name = p.p_name,
                Product_Deacription = p.p_description,
                Product_img = p.p_img,
                Product_Price = (int)p.p_price,
                Type = p.p_img_type
            }).Where(p => p.Product_Name.Contains(name)).ToList<Product>();

            return files;
        }

        public List<SelectListItem> Repeat_Order()
        {
            List<SelectListItem> category = new List<SelectListItem>();
            var data = new[]{
                new SelectListItem { Value = "0", Text = "No Repeat" },
                new SelectListItem { Value = "1", Text = "Repeat After One Day" },
                new SelectListItem { Value = "7", Text = "Repeat After One Week" },
                new SelectListItem { Value = "14", Text = "Repeat After Two Week" },
                new SelectListItem { Value = "21", Text = "Repeat After Three Week" },
                new SelectListItem { Value = "30", Text = " Repeat After One Month" }
            };
            category = data.ToList();
            return category;
        }
    }

}

public enum offer { No, Week, Discount, Other };