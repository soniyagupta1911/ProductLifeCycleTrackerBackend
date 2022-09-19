using LifeTrackerApiService.Domain.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LifeTrackerApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LifeTrackerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        Dictionary<string, string> details = new Dictionary<string, string>();
        public async Task<Dictionary<string, string>> FetchProductNameAndExpirationDate()
        {
           
            string configurationFile = Path.Combine(Directory.GetCurrentDirectory(), @"Images\");
            string[] totalfiles = Directory.GetFiles(configurationFile);
            Dictionary<string, string> details = new Dictionary<string, string>();
            string imageTextContent = "";
            for (int i = 0; i < totalfiles.Length; i++)
            {

                string imgPath = totalfiles[i];
                string ext = Path.GetExtension(imgPath);
                if (ext.ToLower() == ".png" || ext.ToLower() == ".jpg" || ext.ToLower() == ".jpeg")
                {
                     imageTextContent= await ProductDetails.readImageText(imgPath);
                   string key= imgPath.Split("\\").AsQueryable().Last();
                     details.Add(key, imageTextContent);
                }
                else
                    Console.WriteLine(imgPath + " does not have valid extension");

            }
            return details;
        }

        [HttpGet]
        public async Task<Dictionary<string, string>> GetProductDetails()
        {
           return await FetchProductNameAndExpirationDate();
        }
    }
}
