
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace LifeTrackerApiService.Domain.Model
{
    public class ProductDetails
    {
      
        public static async Task<string> readImageText(byte[] byteArray)
        {
            const string subscriptionKey = "";
            const string uriBase = "https://computervisionsoniyahack.cognitiveservices.azure.com/vision/v3.2/read/analyze";
            string imageTextContent = "";
            HttpClient httpclient = new HttpClient();

            // Add Subscription Key in Request headers.  
            httpclient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
            //Set "handwriting" to true in case handwritin text else true for printed text.  
            string requestParams = "handwriting=false & detectOrientation=true";
            // Final URI  
            string uri = uriBase + "?" + requestParams;
            HttpResponseMessage httpresponse = null;
            //string resultStorageLocation = null;
            // Get the image as byte array; this method is defined below  
            byte[] imagebByteData = byteArray;
            ByteArrayContent imageContent = new ByteArrayContent(imagebByteData);
            //Set content type: "application/octet-stream" or "application/json"  
            imageContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            //The 1st REST APT call to start the async process by submitting the image.  
            httpresponse = await httpclient.PostAsync(uri, imageContent);
            Thread.Sleep(5000);
            //Get location of result from response  
            if (httpresponse.IsSuccessStatusCode)
            {
                //httpresponse.Headers.GetValues("Operation-Location").FirstOrDefault();
                //2nd REST API call to get the text content from image  
                httpresponse = await httpclient.GetAsync(httpresponse.Headers.GetValues("Operation-Location").FirstOrDefault());
                imageTextContent = await httpresponse.Content.ReadAsStringAsync();
                //TO DO: This imageTextContent is raw JSON string; Need to format this JSON string for further processing.  
                //Returns the byte array of input image  

                //Console.WriteLine("\nResponse:\n\n{0}\n",

                Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(imageTextContent);
                try
                {
                    string expiryDate = String.Empty;

                    string pattern1 = @"(0?[1-9]|[12][0-9]|3[01])[\/\-](0?[1-9]|1[012])[\/\-]\d{2}|(0?[1-9]|1[012])[\/\-]\d{4}|(0?[1-9]|1[012])[\/\-]\d{2}";
                    string pattern2 = @"(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]|(?:Jan|Mar|May|Jul|Aug|Oct|Dec)))\1|(?:(?:29|30)(\/|-|\.)(?:0?[1,3-9]|1[0-2]|(?:Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})|(?:29(\/|-|\.)(?:0?2|(?:Feb))\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))|(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9]|(?:Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep))|(?:1[0-2]|(?:Oct|Nov|Dec)))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})|(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]|(?:Jan|Mar|May|Jul|Aug|Oct|Dec)))\1|(?:(?:29|30)(\/|-|\.)(?:0?[1,3-9]|1[0-2]|(?:Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)(?:0?2|(?:Feb))\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))|(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9]|(?:Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep))|(?:1[0-2]|(?:Oct|Nov|Dec)))\4(?:(?:1[6-9]|[2-9]\d)?\d{4})";

                    // Create a Regex  
                    Regex rg = new Regex(pattern2, RegexOptions.IgnoreCase);
                    var parsedResult = myDeserializedClass.AnalyzeResult.ReadResults[0];
                    var text = JsonConvert.SerializeObject(parsedResult);
                    Console.WriteLine(myDeserializedClass.AnalyzeResult.ReadResults[0].ToString());
                    MatchCollection matchedAuthors = rg.Matches(text.ToString().ToLower());
                    if (matchedAuthors.Count == 0)
                    {
                        Regex rg1 = new Regex(pattern1, RegexOptions.IgnoreCase);
                        matchedAuthors = rg1.Matches(text.ToString().ToLower());
                    }
                    for (int count = 0; count < matchedAuthors.Count; count++)
                    {
                        if (string.IsNullOrEmpty(expiryDate))
                        {

                            Console.WriteLine(matchedAuthors[count].Value);
                            expiryDate = checkExpiryDate(matchedAuthors[count].Value);
                        }
                        else
                        {
                            break;
                        }

                    }


                return expiryDate;
            }
                catch(Exception ex)
                {
                    Console.WriteLine(imageTextContent);
                }
            }
            return null;
        }

        private static string checkExpiryDate(string value)
        {
            try
            {
                DateTime enteredDate = DateTime.Parse(value);
                Console.WriteLine(enteredDate.ToString());
                int dateComparisonResult = DateTime.Compare(enteredDate, DateTime.Now);
                if (dateComparisonResult > 0)
                {
                    return new DateTimeOffset(enteredDate).ToUnixTimeMilliseconds().ToString(); 
;
                }
            }
            catch(Exception e){
                Console.WriteLine(e.Message);
                try
                {
                    DateTime enteredDate = DateTime.ParseExact(value, "dd/MM/yyyy", null);
                    Console.WriteLine(enteredDate.ToString());
                    int dateComparisonResult = DateTime.Compare(enteredDate, DateTime.Now);
                    if (dateComparisonResult > 0)
                    {
                        return new DateTimeOffset(enteredDate).ToUnixTimeMilliseconds().ToString();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);

                }
            }
            return "";

        }

    }
}
