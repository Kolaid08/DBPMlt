    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Entity;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;
    using PagedList;
    using MVC_Test03.Models;
    using System.Web.UI;
using MVC_Test03.CustomAuthorize;

    namespace MVC_Test03.Controllers
    {
    public class ProductsController : Controller

        {
            private DBSportStoreEntities db = new DBSportStoreEntities();
            protected override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                base.OnActionExecuting(filterContext);
                ViewBag.Role = Session["UserRole"]; // Lấy vai trò từ session và gán cho ViewBag
            }
            // GET: Products

            public ActionResult Index(int? category,int? page)
            {
            var products = db.Products.Include(p => p.Category1);
                if (ViewBag.Role != "Admin")
                {
                    return RedirectToAction("NotFound","Error");
                }
            if (category == null)
            {
                products = db.Products.OrderByDescending(x => x.Price);
            }
            else
            {
                products = db.Products.OrderByDescending(x => x.ProductID).Where(x => x.ProductID == category);
            }
            int pageSize = 8;
            int pageNumber = (page ?? 1);

            return View(products.ToPagedList(pageNumber, pageSize));

          
            }

            // GET: Products/Details/5
            //[Authorize(Roles = "Admin")]
            public ActionResult Details(int? id)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Product product = db.Products.Find(id);
                if (product == null)
                {
                    return HttpNotFound();
                }
                return View(product);
            }

            // GET: Products/Create

            public ActionResult Create()
            {
                if (ViewBag.Role != "Admin")
                {
                    return RedirectToAction("NotFound", "Error");
                }
                ViewBag.Category = new SelectList(db.Categories, "IDCate", "NameCate");
                return View();
            }

            // POST: Products/Create
            // To protect from overposting attacks, enable the specific properties you want to bind to, for 
            // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
            [HttpPost]
            [ValidateAntiForgeryToken]

            public ActionResult Create([Bind(Include = "ProductID,NamePro,DecriptionPro,Category,Price,ImagePro")] Product product)
            {
                if (ModelState.IsValid)
                {
                    db.Products.Add(product);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.Category = new SelectList(db.Categories, "IDCate", "NameCate", product.Category);
                return View(product);
            }

            // GET: Products/Edit/5
            public ActionResult Edit(int? id)
            {
                if (ViewBag.Role != "Admin")
                {
                    return RedirectToAction("NotFound", "Error");
                }
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Product product = db.Products.Find(id);
                if (product == null)
                {
                    return HttpNotFound();
                }
                ViewBag.Category = new SelectList(db.Categories, "IDCate", "NameCate", product.Category);
                return View(product);
            }

            // POST: Products/Edit/5
            // To protect from overposting attacks, enable the specific properties you want to bind to, for 
            // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult Edit([Bind(Include = "ProductID,NamePro,DecriptionPro,Category,Price,ImagePro")] Product product)
            {
                if (ModelState.IsValid)
                {
                    db.Entry(product).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.Category = new SelectList(db.Categories, "IDCate", "NameCate", product.Category);
                return View(product);
            }

            // GET: Products/Delete/5
            public ActionResult Delete(int? id)
            {
                if (ViewBag.Role != "Admin")
                {
                    return RedirectToAction("NotFound", "Error");
                }
                Product product = db.Products.Find(id);
                if (product == null)
                {
                    return HttpNotFound();
                }
                return View(product);
            }

            // POST: Products/Delete/5
            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            public ActionResult DeleteConfirmed(int id)
            {
                if (ViewBag.Role != "Admin")
                {
                    return RedirectToAction("NotFound", "Error");
                }
                Product product = db.Products.Find(id);
                db.Products.Remove(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            // GET: Products
            public ActionResult ProductList(int? category, string idCate, string SearchString, int? page, string sortPrice)
            {   // Tạo bộ mẫu tin Products và có tham chiếu đến Category
            var products = db.Products.Include(p => p.Category1);

            if (category == null)
            {
                products = db.Products.OrderByDescending(x => x.Price);
            }
            else
            {
                products = db.Products.OrderByDescending(x => x.ProductID).Where(x => x.ProductID == category);
            }

            // Lọc theo tên sản phẩm
            if (!string.IsNullOrEmpty(SearchString))
            {
                products = products.Where(s => s.NamePro.Contains(SearchString));
            }

            // Lọc theo loại sản phẩm
            if (!string.IsNullOrEmpty(idCate))
            {
                products = products.Where(p => p.Category.Trim() == idCate.Trim());
            }

            // Lọc theo sắp xếp giá
            if (!string.IsNullOrEmpty(sortPrice))
            {
                if (sortPrice == "asc")
                {
                    products = products.OrderBy(p => p.Price);
                }
                else if (sortPrice == "desc")
                {
                    products = products.OrderByDescending(p => p.Price);
                }
            }

            // Phân trang
            int pageSize = 8;
            int pageNumber = (page ?? 1);

            ViewBag.Categories = db.Categories.ToList();
            return View(products.ToPagedList(pageNumber, pageSize));


        }

        public ActionResult FilterByIDCate(string idCate, int? page)
        {
            var products = db.Products.Include(p => p.Category1);
            if (!string.IsNullOrEmpty(idCate))
            {
                products = products.Where(p => p.Category.Trim() == idCate.Trim());

            }
            int pageSize = 8;
            // Toán tử ?? trong C# mô tả nếu page khác null thì lấy giá trị page, còn
            // nếu page = null thì lấy giá trị 1 cho biến pageNumber.
            int pageNumber = (page ?? 1);

            // Nếu page = null thì đặt lại page là 1.
            if (page == null) page = 1;
            var pagedProducts = products.OrderBy(p => p.NamePro).ToPagedList(pageNumber, pageSize);

            return View("ProductList", pagedProducts);
        }


        //[Authorize(Roles = "Admin")]
        protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                base.Dispose(disposing);
            }
        }
    }
