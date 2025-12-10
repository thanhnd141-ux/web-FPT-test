using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Http;
using DataAccess.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using System;
using DataAccess.Entities;

namespace G1FOOD_User.Controllers
{
    public class CartController : Controller
    {
        private readonly HttpClient client = null;
        private string CartApiUrl;
        public CartController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            CartApiUrl = "https://localhost:44377/api/Cart/";
        }
        // GET: CartController
        public async Task<ActionResult> Index(string accountID)
        {
            HttpResponseMessage response = await client.GetAsync(CartApiUrl + accountID);
            string stringData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            IEnumerable<CartDTO> carts = JsonSerializer.Deserialize<IEnumerable<CartDTO>>(stringData, options);

            int toalPrice = 0;

            foreach (CartDTO c in carts)
            {
                toalPrice += c.ProductPrice;
            }

            ViewBag.ToalPrice = toalPrice;
            return View(carts);
        }

        // GET: CartController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CartController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CartController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(int quantity, string productID, string accountID)
        {
            CartDTO cartDTO = new CartDTO
            {
                CartQuantity = quantity,
                ProductId = productID,
                AccountId = accountID
            };
            HttpResponseMessage response = await client.PostAsJsonAsync(CartApiUrl, cartDTO);
            response.EnsureSuccessStatusCode();

            return RedirectToAction("Index", new { accountID = accountID });
        }

        // GET: CartController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CartController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CartController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CartController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, string accountID)
        {
            HttpResponseMessage response = await client.DeleteAsync(CartApiUrl + id);
            response.EnsureSuccessStatusCode();

            return RedirectToAction("Index", new { accountID = accountID });
        }
    }
}
