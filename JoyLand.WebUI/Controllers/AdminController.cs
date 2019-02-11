using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JoyLand.Domain.Abstract;
using JoyLand.Domain.Concrete;
using JoyLand.Domain.Entities;
using JoyLand.WebUI.Models;

namespace JoyLand.WebUI.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        IProductRepository repository;
        public AdminController (IProductRepository repo)
        {
            repository = repo;
        }
        public ViewResult Index ()
        {
            return View(repository.Products);
        }

        public ViewResult Edit(int Id)
        {
            Product product = repository.Products
                .FirstOrDefault(g => g.Id == Id);
            return View(product);
        }

        [HttpPost]
        public ActionResult Edit(Product product, HttpPostedFileBase image = null)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    product.ImageMimeType = image.ContentType;
                    product.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(product.ImageData, 0, image.ContentLength);
                }
                repository.SaveProduct(product);
                TempData["message"] = string.Format("Изменения в товаре \"{0}\" были сохранены", product.Name);
                return RedirectToAction("Index");
            }
            else
            {
                // Что-то не так со значениями данных
                return View(product);
            }
        }

        [HttpPost]
        public ActionResult Delete(int Id)
        {
            Product deletedProduct = repository.DeleteProduct(Id);
            if (deletedProduct != null)
            {
                TempData["message"] = string.Format("Товар \"{0}\" была удалена",
                    deletedProduct.Name);
            }
            return RedirectToAction("Index");
        }

        public ViewResult Create()
        {
            return View("Edit", new Product());
        }

        public ActionResult Purchase()
        {
            EFDbContext db = new EFDbContext();

            var shoppingDetails = db.ShoppingDetails;
            return View(shoppingDetails.ToList());
        }
        public ActionResult UsersList()
        {
            OurDbContext db = new OurDbContext();
            return View(db.userAccount);
        }

    }
}