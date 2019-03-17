using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using static Android.Graphics.ColorSpace;
using Newtonsoft.Json;

namespace UVapp
{
    

    class SkintypeClassification
    {
        /* Classification is done by the server
         *
         */
        private static readonly Uri funcUri = new Uri("https://uvsafe-skin.azurewebsites.net/api/skintypeClassify?code=kcz53oGm8HnLIpbaAZqR/UQOzKicu2kK5VS1QOPyP0ayNVthdJ7zpA==");

        public static async Task<HttpResponseMessage> serverClassifyImage(Bitmap image, HttpClient httpClient)
        {
            
            MemoryStream encodedImageStream = new MemoryStream();
            
            image.Compress(Bitmap.CompressFormat.Jpeg, 100, encodedImageStream);

            byte[] byteArray = encodedImageStream.ToArray();

            var content = new StringContent(Convert.ToBase64String(byteArray));
            //content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
            HttpResponseMessage response = await httpClient.PostAsync(funcUri, content);
            return response; 
        }

        public class ClassifiedColor
        {
            public int skinType;
        }
    }

    
}