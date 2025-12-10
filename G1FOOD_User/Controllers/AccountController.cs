using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Http;
using DataAccess.DTO;
using DataAccess.Entities;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using System.Security.Principal;

namespace G1FOOD_User.Controllers
{
    public class AccountController : Controller
    {
        private readonly HttpClient client = null;
        private string AccountApiUrl;
        private string UserApiUrl;
        public AccountController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            AccountApiUrl = "https://localhost:44377/api/Account/";
            UserApiUrl = "https://localhost:44377/api/User/";
        }
        // GET: AccountController
        public ActionResult Index()
        {
            return View();
        }

        // GET: AccountController/Details/5
        public async Task<ActionResult> UserProfile(string accountID)
        {
            HttpResponseMessage response = await client.GetAsync(AccountApiUrl + accountID);
            string stringData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            AccountDTO account = JsonSerializer.Deserialize<AccountDTO>(stringData, options);

            return View(account);
        }

        // GET: AccountController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AccountController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AccountDTO accountDTO)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(UserApiUrl, accountDTO);
            response.EnsureSuccessStatusCode();

            return RedirectToAction("Index", "Login");
        }

        // GET: AccountController/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            HttpResponseMessage response = await client.GetAsync(AccountApiUrl + id);
            string stringData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            AccountDTO account = JsonSerializer.Deserialize<AccountDTO>(stringData, options);

            return View(account);
        }

        // POST: AccountController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, AccountDTO accountDTO)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync(AccountApiUrl + id, accountDTO);
            response.EnsureSuccessStatusCode();

            HttpContext.Session.SetString("AccountName", accountDTO.AccountName);
            return RedirectToAction("UserProfile", new {accountID = id });
        }

        // GET: AccountController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AccountController/Delete/5
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
