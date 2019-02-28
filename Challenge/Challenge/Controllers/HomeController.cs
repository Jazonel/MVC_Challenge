using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Challenge.Models;
using System.Net;
using Newtonsoft.Json;
using ClassLibrary1;

namespace Challenge.Controllers
{
    public class HomeController : Controller
    {
        List<JsonFormat.AsteroidInfo> informationList = new List<JsonFormat.AsteroidInfo>();

        public IActionResult Index()
        {
            informationList = CallApi();
            return View(informationList);
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




        public List<JsonFormat.AsteroidInfo> CallApi()
        {
            List<JsonFormat.AsteroidInfo> information = new List<JsonFormat.AsteroidInfo>();
   
            var webClient = new WebClient();
            var json = webClient.DownloadString(@"https://api.nasa.gov/neo/rest/v1/feed?start_date=2015-09-07&end_date=2015-09-08&api_key=rNpnnsUuVTCAxCN7ILVbLhpPMJiWzqUutrHI85De");
            JsonFormat.Asteroid asteroids = JsonConvert.DeserializeObject<JsonFormat.Asteroid>(json);

            foreach (KeyValuePair<string, List<JsonFormat.AsteroidInfo>> date in asteroids.near_earth_objects)
            {
                foreach (JsonFormat.AsteroidInfo value in date.Value)
                {
                    information.Add(value);
                }
            }
            return information;
        }
    }
}
