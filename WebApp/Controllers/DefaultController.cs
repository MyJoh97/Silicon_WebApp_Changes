using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using WebApp.ViewModels;

namespace WebApp.Controllers;

public class DefaultController(HttpClient httpClient) : Controller
{
    private readonly HttpClient _httpClient = httpClient;

    public IActionResult Home()
    {
        return View();
    }


    [HttpPost]
    public async Task<IActionResult> Subscribe(SubscribeViewModel model)
    {
        if (ModelState.IsValid)
        {
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var respons = await _httpClient.PostAsync("https://localhost:7116/api/subscribe", content);
            if (respons.IsSuccessStatusCode)
            {
                TempData["StatusMessage"] = "You are now subscribed";
            }
            else if (respons.StatusCode == System.Net.HttpStatusCode.Conflict)
            {
                TempData["StatusMessage"] = "You are already subscribed";
            }
        }
        else
        {
            TempData["StatusMessage"] = "Invalid email address";
        }

        return RedirectToAction("Home", "Default", "subscribe");
    }
}
