using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Http;
using DataAccess.DTO;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace G1FOOD_Web_Staff.Controllers
{
    public class ChefOrderController : Controller
    {
        private readonly HttpClient client = null;
        private string ChefOrderApiUrl;
        private string StaffOrderHistoryApiUrl;
        private string OrderApiUrl;
        private string OrderDetailsApiUrl;

        public ChefOrderController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            ChefOrderApiUrl = "https://localhost:44377/api/ChefOrder/";
            StaffOrderHistoryApiUrl = "https://localhost:44377/api/StaffOrderHistory/";
            OrderApiUrl = "https://localhost:44377/api/Order/";
            OrderDetailsApiUrl = "https://localhost:44377/api/OrderDetails/";
        }
        // GET: ChefOrderController
        public async Task<ActionResult> Index()
        {
            HttpResponseMessage response = await client.GetAsync(ChefOrderApiUrl);
            string stringData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            IEnumerable<OrderDTO> orders = JsonSerializer.Deserialize<IEnumerable<OrderDTO>>(stringData, options);

            return View(orders);
        }

        public async Task<ActionResult> ChefHistory(string id)
        {
            HttpResponseMessage response = await client.GetAsync(StaffOrderHistoryApiUrl + id);
            string stringData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            IEnumerable<OrderDTO> orders = JsonSerializer.Deserialize<IEnumerable<OrderDTO>>(stringData, options);

            return View(orders);
        }

        // GET: ChefOrderController/Details/5
        public async Task<ActionResult> Details(string id)
        {
            HttpResponseMessage response = await client.GetAsync(OrderDetailsApiUrl + id);
            string stringData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            IEnumerable<OrderDetailsDTO> orderDetails = JsonSerializer.Deserialize<IEnumerable<OrderDetailsDTO>>(stringData, options);

            ViewBag.OrderDetails = orderDetails;


            response = await client.GetAsync(OrderApiUrl + id);
            stringData = await response.Content.ReadAsStringAsync();
            options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            OrderDTO order = JsonSerializer.Deserialize<OrderDTO>(stringData, options);
            int toalPrice = 0;

            foreach (OrderDetailsDTO c in orderDetails)
            {
                toalPrice += c.OrderDprice;
            }

            ViewBag.ToalPrice = toalPrice;
            ViewBag.OrderID = order.OrderId;
            ViewBag.BuyerFullName = order.BuyerFullName;
            ViewBag.BuyerPhone = order.BuyerPhone;
            ViewBag.BuyerAddress = order.BuyerAddress;
            ViewBag.Note = order.OrderNote;
            ViewBag.OrderDate = order.OrderDate.ToString("dd/MM/yyyy HH:mm:ss");
            ViewBag.OrderStatus = order.OrderStatus;

            return View(new OrderDTO());
        }

        // GET: ChefOrderController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ChefOrderController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Confirm(string orderID, string accountID, string status)
        {
            OrderDTO orderDTO = new OrderDTO
            {
                OrderId = orderID,
                AccountId = accountID,
                OrderStatus = status
            };
            try
            {
                HttpResponseMessage response = await client.PostAsJsonAsync(ChefOrderApiUrl, orderDTO);
                response.EnsureSuccessStatusCode();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        // GET: ChefOrderController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ChefOrderController/Edit/5
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

        // GET: ChefOrderController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ChefOrderController/Delete/5
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
