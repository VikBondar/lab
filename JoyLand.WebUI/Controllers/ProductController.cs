using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JoyLand.Domain.Abstract;
using JoyLand.Domain.Entities;
using JoyLand.WebUI.Models;

namespace JoyLand.WebUI.Controllers
{
    public class ProductController : Controller
    {
        private IProductRepository repository;
        public int pageSize = 4;
        public ProductController(IProductRepository repo)
        {
            repository = repo;
        }
        public ViewResult List(string category, string brand, string countryOfOrigin, string searchString, int page = 1)
        {
            if (!String.IsNullOrEmpty(searchString))
            {
                ProductsListViewModel model = new ProductsListViewModel
                {
                    Products = repository.Products
                .Where(p => category == null || p.Category == category)
                .Where(product => product.Name.Contains(searchString))
                .OrderBy(product => product.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItens = category == null ?
                repository.Products.Count() :
                repository.Products.Where(product => product.Category == category).Count()
                },
                CurrentCategory = category
            };
            return View(model);
            }
            else
            {
                ProductsListViewModel model = new ProductsListViewModel
                {
                    Products = repository.Products
                    .Where(p => category == null || p.Category == category)
                    .OrderBy(product => product.Id)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize),
                    PagingInfo = new PagingInfo
                    {
                        CurrentPage = page,
                        ItemsPerPage = pageSize,
                        TotalItens = category == null ?
                    repository.Products.Count() :
                    repository.Products.Where(product => product.Category == category).Count()
                    },
                    CurrentCategory = category
                };
                return View(model);
            }
        }

        public FileContentResult GetImage(int Id)
        {
            Product product = repository.Products
                .FirstOrDefault(g => g.Id == Id);

            if (product != null)
            {
                return File(product.ImageData, product.ImageMimeType);
            }
            else
            {
                return null;
            }
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

    }
}