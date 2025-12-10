using DataAccess.DTO;
using DataAccess.Paging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace G1FOOD_Web_Admin.Controllers
{
    public class ProductController : Controller
    {
        private readonly HttpClient client = null;
        private string ProductApiUrl;
        public ProductController()
        {
            client= new HttpClient();
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
            int pageSize = 10;
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

        // GET: ManageProductController/Details/5
        public async Task<ActionResult> Details(string id)
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

        // GET: ManageProductController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ManageProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ProductDTO product)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(ProductApiUrl, product);
            response.EnsureSuccessStatusCode();

            return RedirectToAction("Index");
        }

        // GET: ManageProductController/Edit/5
        public async Task<ActionResult> Edit(string id)
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

        // POST: ManageProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, ProductDTO product)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync(ProductApiUrl + id, product);
            response.EnsureSuccessStatusCode();

            return RedirectToAction("Index");
        }

        // GET: ManageProductController/Delete/5
        public async Task<ActionResult> Delete(string id)
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

        // POST: ManageProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string id, IFormCollection collection)
        {
            HttpResponseMessage response = await client.DeleteAsync(ProductApiUrl + id);
            response.EnsureSuccessStatusCode();

            return RedirectToAction("Index");
        }
    }
}
