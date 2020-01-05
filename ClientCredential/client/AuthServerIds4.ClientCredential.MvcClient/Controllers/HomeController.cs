using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AuthServerIds4.ClientCredential.MvcClient.Models;
using Microsoft.AspNetCore.Authentication;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using IdentityModel.Client;

namespace AuthServerIds4.ClientCredential.MvcClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
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
      
        public async Task<IActionResult> CallApi()
        {
            // Identity Model Client Configuration and discovery in the Ids
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync("https://localhost:44366");
            if (disco.IsError)
            {
                //throw new ApplicationException(disco.Error);
                return Json(disco.Error);
            }
            // Get Token from Ids
            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,

                ClientId = "clientcredentialId",
                ClientSecret = "clientCredentialSecret",
                Scope = "clientcreapi"
            });

            if (tokenResponse.IsError)
            {
                return Json(tokenResponse.Error);
            }
            //Request to api with access token
            client.SetBearerToken(tokenResponse.AccessToken);
            var response = await client.GetAsync("https://localhost:44367/api/identity");
            var content = await response.Content.ReadAsStringAsync();

            ViewBag.Json = JArray.Parse(content).ToString();
            return View("json");
        }


        public IActionResult Logout()
        {
            return SignOut(CookieAuthenticationDefaults.AuthenticationScheme, OpenIdConnectDefaults.AuthenticationScheme);
        }
    }
}
