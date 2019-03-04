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
using System.Globalization;

namespace Challenge.Controllers
{
    public class HomeController : Controller
    {
        
        List<Tuple<double, JsonFormat.AsteroidInfo>> information = new List<Tuple<double, JsonFormat.AsteroidInfo>>();

        [HttpGet]
        public IActionResult Index()
        {
           
            return View();
        }

        [HttpPost]
        public IActionResult Index(string initialDate, string endDate)
        {
            information = CallApi(initialDate, endDate);
            return View(information);
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
            List<Tuple<double, JsonFormat.AsteroidInfo>> asteroidInformation = new List<Tuple<double, JsonFormat.AsteroidInfo>>();
            //DateTime initDate = Convert.ToDateTime(initialDate);
            initialDate = Convert.ToDateTime(initialDate).ToString("yyyy-MM-dd");
            endDate = Convert.ToDateTime(initialDate).ToString("yyyy-MM-dd");


            if (DateTime.TryParse(initialDate, out DateTime initDate) && DateTime.TryParse(endDate, out DateTime finalDate))
            {
                if ((finalDate - initDate).Days <= 7 && initDate <= finalDate)
                {
                    var webClient = new WebClient();
                    string url = "https://api.nasa.gov/neo/rest/v1/feed?start_date=" + initialDate + "&end_date=" + endDate + "&api_key=rNpnnsUuVTCAxCN7ILVbLhpPMJiWzqUutrHI85De";
                    var json = webClient.DownloadString(url);
                    JsonFormat.Asteroid asteroids = JsonConvert.DeserializeObject<JsonFormat.Asteroid>(json);

                    foreach (KeyValuePair<string, List<JsonFormat.AsteroidInfo>> date in asteroids.near_earth_objects)
                    {
                        foreach (JsonFormat.AsteroidInfo value in date.Value)
                        {
                            double distance = double.Parse(value.close_approach_data.First().miss_distance.kilometers) / 299792;
                            asteroidInformation.Add(new Tuple<double, JsonFormat.AsteroidInfo>(distance, value));
                        }
                    }
                    return asteroidInformation;
                }
                else
                {
                    return asteroidInformation;
                }

            }
            else
            {
                return asteroidInformation;
            }
        }
    }
}
