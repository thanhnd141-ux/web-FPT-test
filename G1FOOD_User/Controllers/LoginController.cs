using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using System;
using DataAccess.DTO;
using System.Text.Json;
using System.Threading.Tasks;

namespace G1FOOD_Web_Admin.Controllers
{
    public class LoginController : Controller
    {
        private readonly HttpClient client = null;
        private string LoginApiUrl;
        public LoginController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            LoginApiUrl = "https://localhost:44377/api/Login/";
        }

        // GET: LoginController
        [AllowAnonymous]
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index([FromForm] string email, [FromForm] string password)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(LoginApiUrl, new LoginDTO { AccountEmail = email, AccountPassword = password});

            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "Wrong email or password. Please try again!";
                return RedirectToAction("Index", "Login");
            }

            string stringData = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            AccountDTO account = JsonSerializer.Deserialize<AccountDTO>(stringData, options);

            if (account != null && account.AccountId.Contains("US"))
            {
                HttpContext.Session.SetString("AccountID", account.AccountId);
                HttpContext.Session.SetString("AccountName", account.AccountName);
                return RedirectToAction("Index", "Home");
            } 

            TempData["ErrorMessage"] = "Wrong email or password. Please try again!";
            return RedirectToAction("Index", "Login");
        }
    }
}
