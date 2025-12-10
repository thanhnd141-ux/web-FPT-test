using DataAccess.DTO;
using G1FOOD_User.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using DataAccess.Paging;
using DataAccess.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace G1FOOD_User.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly HttpClient client = null;
        private string ProductApiUrl;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            ProductApiUrl = "https://localhost:44377/api/Product/";
        }

        // GET: ManageProductController
        public async Task<ActionResult> Index(string searchTitle, string searchOrder, string currentFilter, string searchName, string searchCategory, int? pageNumber = 1)
        {
            HttpResponseMessage response = await client.GetAsync(ProductApiUrl);
            string stringData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<ProductDTO> products = JsonSerializer.Deserialize<List<ProductDTO>>(stringData, options);

            if (!(searchCategory is null))
            {
                products = products.Where(p => p.CategoryId.Equals(searchCategory)).ToList();
            }

            if (!String.IsNullOrEmpty(searchName))
            {
                products = products.Where(p => p.ProductName.ToLower().Contains(searchName.ToLower())).ToList();
            }

            if (!String.IsNullOrEmpty(searchOrder))
            {
                if (searchOrder.Contains("desc"))
                {
                    switch (searchTitle)
                    {
                        case "name":
                            products = products.OrderByDescending(p => p.ProductName).ToList();
                            break;
                        case "price":
                            products = products.OrderByDescending(p => p.ProductPrice).ToList();
                            break;
                    }
                }
                else
                {
                    switch (searchTitle)
                    {
                        case "name":
                            products = products.OrderBy(p => p.ProductName).ToList();
                            break;
                        case "price":
                            products = products.OrderBy(p => p.ProductPrice).ToList();
                            break;
                    }
                }
            }


            int pageIndex = pageNumber.Value;
            int pageSize = 8;
            int totalPage = (int)Math.Ceiling(products.Count / (double)pageSize);

            PaginatedList<ProductDTO> listProducts = PaginatedList<ProductDTO>.Create(products.AsQueryable(), pageNumber ?? 1, pageSize);

            int count = listProducts.Count;

            List<SelectListItem> categories = new List<SelectListItem>
            {
                new SelectListItem { Value = "FOOD", Text = "Food" },
                new SelectListItem { Value = "DRINK", Text = "Drink" },
                new SelectListItem { Value = "SOUP", Text = "Soup" },
                new SelectListItem { Value = "COMBO", Text = "Combo" }
            };

            ViewBag.CatId = categories;
            ViewBag.SelectedCategory = searchCategory;
            ViewBag.SearchOrder = searchOrder;
            ViewBag.TotalPage = totalPage;
            ViewBag.PageIndex = pageIndex;
            ViewBag.PageSize = pageSize;
            ViewBag.SearchTitle = searchTitle;
            ViewBag.SearchName = searchName;
            return View(listProducts);
        }

        public async Task<ActionResult> ProductDetails(string id)
        {
            HttpResponseMessage response = await client.GetAsync(ProductApiUrl + id);
            string stringData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            ProductDTO product = JsonSerializer.Deserialize<ProductDTO>(stringData, options);

            return View(product);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
