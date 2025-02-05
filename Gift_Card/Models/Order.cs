using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

namespace Gift_Card.Models
{
    [SessionState(SessionStateBehavior.Default)]
    public class Order
    {
        DateTime Now_Date = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        DBConnection db = new DBConnection();
        public int uid { get; set; }
        public int oid { get; set; }
        public int rid { get; set; }
        public string bname { get; set; }
        public int pid { get; set; }
        public int qty { get; set; }
        public int boy { get; set; }
        public string status { get; set; }

        public string shipping { get; set; }
        public string repeat { get; set; }
        public string Product_Return { get; set; }
        public int peroid { get; set; }
        public DateTime odate { get; set; }
        public DateTime pdate { get; set; }
        public DateTime ddate { get; set; }

        GiftCardEntities ctx = new GiftCardEntities();
        public void insert_Order(int id, string address)
        {
            order ord = new order();
            ord.u_id = id;
            ord.o_date = Now_Date;
            ord.shipping = address;
            ord.status = "Pending";
            ctx.orders.Add(ord);
            ctx.SaveChanges();

            var order_no = ctx.orders.Max(e => e.o_id);

            var file = ctx.users.Join(ctx.carts, u => u.u_id, c => c.u_id, (u, c) => new Order()
            {
                pid = (int)c.p_id,
                qty = (int)c.qty,
                uid = (int)c.u_id,
                shipping = u.u_address
            }).Where(c => c.uid == id).ToList<Order>();

            foreach (var item in file)
            {
                var qq = ctx.products.Select(p => new { p.p_price, p.p_id }).Where(p => p.p_id == item.pid).FirstOrDefault();
                int price = (int)qq.p_price;
                invoice inv = new invoice();
                inv.p_id = item.pid;
                inv.o_id = order_no;
                inv.p_qty = item.qty;
                inv.p_price = price;
                ctx.invoices.Add(inv);
                ctx.SaveChanges();
            }

            var user = ctx.carts.Where(c => c.u_id == id).ToList();
            foreach (var item in user)
            {
                ctx.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                ctx.SaveChanges();
            }

        }


        public List<Product> display_Pending(int id)
        {
            List<Product> files = new List<Product>();

            var lastorder = ctx.orders.Where(o => o.u_id == id && o.status != "Cancel" && o.status != "Delivered");

            if (lastorder.Count() > 0)
            {

                files = ctx.invoices.Join(ctx.products, i => i.p_id, p => p.p_id, (i, p) =>
                              new Product()
                              {
                                  Product_ID = p.p_id,
                                  Product_Quantity = (int)i.p_qty,
                                  Product_Name = p.p_name,
                                  Product_Deacription = p.p_description,
                                  Product_img = p.p_img,
                                  Product_Price = (int)p.p_price,
                                  Type = p.p_img_type,
                                  Order_No = (int)i.o_id,
                              }).ToList<Product>();
            }


            return files;
        }
        public List<Product> OrderDetails(int id)
        {
            List<Product> files = new List<Product>();

            var inv = ctx.invoices.Where(i => i.o_id == id);

            files = inv.Join(ctx.products, i => i.p_id, p => p.p_id, (i, p) =>
                          new Product()
                          {
                              Product_ID = p.p_id,
                              Product_Quantity = (int)i.p_qty,
                              Product_Name = p.p_name,
                              Product_Deacription = p.p_description,
                              Product_img = p.p_img,
                              Product_Price = (int)i.p_price,
                              Type = p.p_img_type,
                              Order_No = id,
                          }).ToList<Product>();

            return files;
        }



        public void Status_Upadate(int oid, string status)
        {
            var result = ctx.orders.SingleOrDefault(o => o.o_id == oid);
            if (result != null)
            {
                result.status = status;
                ctx.SaveChanges();
            }
        }
       
        public void Delivered(int oid, string status)
        {
            var result = ctx.orders.SingleOrDefault(o => o.o_id == oid);
            if (result != null)
            {
                result.d_date = Now_Date;
                result.status = status;
                ctx.SaveChanges();
            }
        }
       
        public void Cancel(int oid, string status)
        {
            var result = ctx.orders.SingleOrDefault(o => o.o_id == oid);
            if (result != null)
            {
                result.c_date = Now_Date;
                result.status = status;
                ctx.SaveChanges();
            }
        }
       
        public List<Account> Pending_Order_Admin()
        {
            List<Account> files = null;

            var group = ctx.orders.Select(e => new { e.o_id, e.u_id, e.status, e.shipping, e.o_date, e.p_date, e.c_date, e.d_date }).OrderByDescending(o=>o.o_id).ToList();
            files = group.Join(ctx.users, i => i.u_id, u => u.u_id, (i, u) => new Account()
            {
                id = u.u_id,
                Full_Name = u.u_name,
                Email = u.u_email,
                Address = u.u_address,
                Contact = u.u_contact,
                Password = u.u_password,
                User_Type = u.u_type,
                Gender = u.u_gender,
                Account_img = u.u_img,
                type_img = u.u_img_type,
                OrderDate = i.o_date.ToString(),
                PickedDate = i.p_date.ToString(),
                statusOrder = i.status,
                order_ID = (int)i.o_id,
                rid = (int)i.o_id
            }).OrderByDescending(o => o.rid).Distinct().ToList<Account>();

            return files;
        }
       
        public List<Order> Customer_Repeat_Order(int id)
        {
            List<Order> files = new List<Order>();
            files = ctx.orders.Where(w => w.u_id == id).Select(o => new Order()
            {
                rid = o.o_id,
                oid = o.o_id,
                uid = (int)o.u_id,
                odate = (DateTime)o.o_date,
                status = o.status
            }).OrderByDescending(a => a.oid).ToList<Order>();
            return files;
        }
        public List<Account> Repeat_Order_Admin()
        {
            List<Account> files = null;

            var group = ctx.orders.Select(e => new { e.u_id, e.status, e.shipping, e.o_date, e.o_id }).Where(e => e.status != "Cancel" && e.status != "Delivered");
            files = ctx.orders.Join(ctx.users, i => i.u_id, u => u.u_id, (i, u) => new Account()
            {
                id = u.u_id,
                Full_Name = u.u_name,
                Email = u.u_email,
                Address = i.shipping,
                Contact = u.u_contact,
                Account_img = u.u_img,
                type_img = u.u_img_type,
                Register_Date = (DateTime)i.o_date,
                statusOrder = i.status,
                order_ID = i.o_id
            }).Where(w => w.repeat == "YES").Distinct().ToList<Account>();

            return files;

        }
    }
}