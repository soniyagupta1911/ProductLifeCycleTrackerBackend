using LifeTrackerApiService.Domain.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
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
        Dictionary<string, string> details = new Dictionary<string, string>();
        public async Task<Dictionary<string, string>> FetchProductNameAndExpirationDate(string pname , byte[] byteArray)
        {

            string imageTextContent = "";
            Dictionary<string, string> details = new Dictionary<string, string>();
            imageTextContent = await ProductDetails.readImageText(byteArray);
            details.Add(pname, imageTextContent);
            return details;
        }

        [HttpPost]
        public async Task<Dictionary<string, string>> GetProductDetails([FromBody] ImageObject imgObj)
        {
            byte[] bytes = Convert.FromBase64String(imgObj.image);
            // IFormFile img = null;
            return await FetchProductNameAndExpirationDate(imgObj.name,bytes);
        }
    }
}

public class ImageObject
{
    public string name { get; set; }

    public string image { get; set; }

}
