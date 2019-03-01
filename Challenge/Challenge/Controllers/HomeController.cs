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
        
        List<Tuple<double, JsonFormat.AsteroidInfo>> test = new List<Tuple<double, JsonFormat.AsteroidInfo>>();

        [HttpGet]
        public IActionResult Index()
        {
           
            return View();
        }

        [HttpPost]
        public IActionResult Index(string initialDate, string endDate)
        {
            test = CallApi(initialDate, endDate);
            return View(test);
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


        public List<Tuple<double, JsonFormat.AsteroidInfo>> CallApi(string initialDate, string endDate)
        {

            List<JsonFormat.AsteroidInfo> information = new List<JsonFormat.AsteroidInfo>();
            List<Tuple<double, JsonFormat.AsteroidInfo>> information2 = new List<Tuple<double, JsonFormat.AsteroidInfo>>();   
            var webClient = new WebClient();
            string url = "https://api.nasa.gov/neo/rest/v1/feed?start_date=" + initialDate + "&end_date=" + endDate + "&api_key=rNpnnsUuVTCAxCN7ILVbLhpPMJiWzqUutrHI85De";
            var json = webClient.DownloadString(url);
            JsonFormat.Asteroid asteroids = JsonConvert.DeserializeObject<JsonFormat.Asteroid>(json);

            
            foreach (KeyValuePair<string, List<JsonFormat.AsteroidInfo>> date in asteroids.near_earth_objects)
            {
                foreach (JsonFormat.AsteroidInfo value in date.Value)
                {
                    double distance = double.Parse(value.close_approach_data.First().miss_distance.kilometers) / 299792;
                    information2.Add(new Tuple<double, JsonFormat.AsteroidInfo>(distance,value));
                }
            }
            return information2;
        }
    }
}
