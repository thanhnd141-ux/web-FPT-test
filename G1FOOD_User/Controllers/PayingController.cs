using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Http;
using DataAccess.DTO;
using DataAccess.Entities;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace G1FOOD_User.Controllers
{
    public class PayingController : Controller
    {
        private readonly HttpClient client = null;
        private string CartApiUrl;
        private string AccountApiUrl;
        private string PayingApiUrl;
        public PayingController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            CartApiUrl = "https://localhost:44377/api/Cart/";
            AccountApiUrl = "https://localhost:44377/api/Account/";
            PayingApiUrl = "https://localhost:44377/api/Paying/";
        }
        // GET: PayingController
        public ActionResult Index()
        {
            return View();
        }

        // GET: PayingController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PayingController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PayingController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: PayingController/Edit/5
        public async Task<ActionResult> Paying(string id)
        {
            HttpResponseMessage response = await client.GetAsync(AccountApiUrl + id);
            string stringData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            AccountDTO account = JsonSerializer.Deserialize<AccountDTO>(stringData, options);

            ViewBag.BuyerFullName = account.AccountName;
            ViewBag.BuyerPhone = account.AccountPhone;
            ViewBag.BuyerAddress = account.AccountAddress;

            string accountID = id;

            response = await client.GetAsync(CartApiUrl + accountID);
            stringData = await response.Content.ReadAsStringAsync();
            options = new JsonSerializerOptions
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
            ViewBag.Carts = carts;

            return View();
        }

        // POST: PayingController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Paying(int id, OrderDTO orderDTO)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync(PayingApiUrl + id, orderDTO);
            response.EnsureSuccessStatusCode();

            return RedirectToAction("Index","Home");
        }

        // GET: PayingController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PayingController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
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
    }
}
