using DataAccess.DTO;
using DataAccess.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace G1FOOD_Web_Staff.Controllers
{
    public class ShipOrderController : Controller
    {
        private readonly HttpClient client = null;
        private string ShipOrderApiUrl;
        private string StaffOrderHistoryApiUrl;
        private string OrderApiUrl;
        private string OrderDetailsApiUrl;

        public ShipOrderController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            ShipOrderApiUrl = "https://localhost:44377/api/ShipOrder/";
            StaffOrderHistoryApiUrl = "https://localhost:44377/api/StaffOrderHistory/";
            OrderApiUrl = "https://localhost:44377/api/Order/";
            OrderDetailsApiUrl = "https://localhost:44377/api/OrderDetails/";
        }
        // GET: ChefOrderController
        public async Task<ActionResult> Index(string id)
        {
            HttpResponseMessage response = await client.GetAsync(ShipOrderApiUrl + id);
            string stringData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            string isDelivering = JsonSerializer.Deserialize<string>(stringData, options);

            if (!isDelivering.Equals("Not Delivering"))
            {
                return RedirectToAction("Details", new { id = isDelivering });
            }

            ViewBag.Message = "Không có đơn hàng cần ship";

            return View();
        }

        public async Task<ActionResult> ShipHistory(string id)
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

        // GET: ShipOrderController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ShipOrderController/Create
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

        // GET: ShipOrderController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ShipOrderController/Edit/5
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
                HttpResponseMessage response = await client.PostAsJsonAsync(ShipOrderApiUrl, orderDTO);
                response.EnsureSuccessStatusCode();

                return RedirectToAction("Index", new {id = accountID});
            }
            catch
            {
                return View();
            }
        }

        // GET: ShipOrderController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ShipOrderController/Delete/5
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
