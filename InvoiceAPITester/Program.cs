using InvoiceAPITester.Model;
using Mono.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;


namespace InvoiceAPITester
{
    static class Program
    {
        // Replace <Subscription Key> with your valid subscription key.
        const string subscriptionKey = "f577641951704b30912fbcee867135f2";
        static string responseURI = "";

        // You must use the same region in your REST call as you used to
        // get your subscription keys. For example, if you got your
        // subscription keys from westus, replace "westcentralus" in the URL
        // below with "westus".
        //
        // Free trial subscription keys are generated in the westcentralus region.
        // If you use a free trial subscription key, you shouldn't need to change
        // this region.
        const string uriBase = "https://westus.api.cognitive.microsoft.com/vision/v2.0/recognizeText";
            //"https://westus.api.cognitive.microsoft.com/vision/v2.0/ocr";

        static void Main()
        {
            Boundary testB = new Boundary("123,123,124,125");
            Console.WriteLine(testB.XBegin);
            Console.WriteLine(testB.XSize);
            Console.WriteLine(testB.YBegin);
            Console.WriteLine(testB.YSize);
            extractImageData();
        }

        private static void extractImageData()
        {
            // Get the path and filename to process from the user.
            Console.WriteLine("Optical Character Recognition:");
            Console.Write("Enter the path to an image with text you wish to read: ");
            //string imageFilePath = Console.ReadLine();
            string imageFilePath = "D:/images/target.jpg";

            if (File.Exists(imageFilePath))
            {
                // Make the REST API call.
                Console.WriteLine("\nWait a moment for the results to appear.\n");
                MakeOCRRequest(imageFilePath).Wait();
                MakeRequest(responseURI).Wait();
            }
            else
            {
                Console.WriteLine("\nInvalid file path");
            }
            Console.ReadLine();
        }


        /// <summary>
        /// Gets the text visible in the specified image file by using
        /// the Computer Vision REST API.
        /// </summary>
        /// <param name="imageFilePath">The image file with printed text.</param>
        static async Task MakeOCRRequest(string imageFilePath)
        {
            try
            {
                HttpClient client = new HttpClient();

                // Request headers.
                client.DefaultRequestHeaders.Add(
                    "Ocp-Apim-Subscription-Key", subscriptionKey);

                // Request parameters.
                string requestParameters = "mode=Printed";
                // Assemble the URI for the REST API Call.
                string uri = uriBase + "?" + requestParameters;

                HttpResponseMessage response;

                // Request body. Posts a locally stored JPEG image.
                byte[] byteData = GetImageAsByteArray(imageFilePath);

                using (ByteArrayContent content = new ByteArrayContent(byteData))
                {
                    // This example uses content type "application/octet-stream".
                    // The other content types you can use are "application/json"
                    // and "multipart/form-data".
                    content.Headers.ContentType =
                        new MediaTypeHeaderValue("application/octet-stream");

                    // Make the REST API call.
                    response = await client.PostAsync(uri, content);
                }
                IEnumerable<string> values;

                // Get the JSON response.
                response.Headers.TryGetValues("Operation-Location", out values);
                
                Console.WriteLine(values.FirstOrDefault());
                responseURI = values.FirstOrDefault();
            }
            catch (Exception e)
            {
                Console.WriteLine("\n" + e.Message);
            }
        }

        static async Task MakeRequest(string responseURI)
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

            var uri = responseURI;

            //check when response is ready 
            bool status = false;
            while (status == false)
            {
                var response = await client.GetAsync(uri);
                string contentString = await response.Content.ReadAsStringAsync();
                var imageData = JsonConvert.DeserializeObject<ImageResData>(contentString);
                if(imageData.status.Equals("Succeeded"))
                {
                    status = true;
                    string lines = JToken.Parse(contentString).ToString();

                    System.IO.File.WriteAllText(@"D:\TestFolder\WriteText.txt", lines);
                    Console.WriteLine("Done Task " + uri);
                }
                else
                {
                    Console.WriteLine("Not ready yet " + uri);
                    await Task.Delay(2000);
                }
            }
        }

        /// <summary>
        /// Returns the contents of the specified file as a byte array.
        /// </summary>
        /// <param name="imageFilePath">The image file to read.</param>
        /// <returns>The byte array of the image data.</returns>
        static byte[] GetImageAsByteArray(string imageFilePath)
        {
            using (FileStream fileStream =
                new FileStream(imageFilePath, FileMode.Open, FileAccess.Read))
            {
                BinaryReader binaryReader = new BinaryReader(fileStream);
                return binaryReader.ReadBytes((int)fileStream.Length);
            }
        }

    }
}