using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Http;
using DataAccess.DTO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;

namespace G1FOOD_Web_Admin.Controllers
{
    public class OrderController : Controller
    {
        private readonly HttpClient client = null;
        private string OrderApiUrl;
        private string OrderDetailsApiUrl;
        public OrderController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            OrderApiUrl = "https://localhost:44377/api/Order/";
            OrderDetailsApiUrl = "https://localhost:44377/api/OrderDetails/";
        }
        // GET: OrderController
        public async Task<ActionResult> Index()
        {
            HttpResponseMessage response = await client.GetAsync(OrderApiUrl);
            string stringData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            IEnumerable<OrderDTO> orders = JsonSerializer.Deserialize<IEnumerable<OrderDTO>>(stringData, options);

            return View(orders);
        }

        // GET: OrderController/Details/5
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
            ViewBag.Chef = order.AccIdofChef;
            ViewBag.Shipper = order.AccIdofShipper;
            ViewBag.BuyerFullName = order.BuyerFullName;
            ViewBag.BuyerPhone = order.BuyerPhone;
            ViewBag.BuyerAddress = order.BuyerAddress;
            ViewBag.Note = order.OrderNote;
            ViewBag.OrderDate = order.OrderDate.ToString("dd/MM/yyyy HH:mm:ss");
            ViewBag.OrderStatus = order.OrderStatus;

            return View(new OrderDTO());
        }

        // GET: OrderController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OrderController/Create
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

        // GET: OrderController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: OrderController/Edit/5
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

        // GET: OrderController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: OrderController/Delete/5
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
