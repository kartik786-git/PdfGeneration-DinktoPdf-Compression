using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Net.Http;
using System.Text;
using WebApplication4.Services;

namespace WebApplication4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PdfController : ControllerBase
    {
        private readonly IPdfGenerationService _pdfGenerationService;

        public PdfController(IPdfGenerationService pdfGenerationService)
        {
            _pdfGenerationService = pdfGenerationService;
        }

        [HttpGet]
        public IActionResult GeneratePdf()
        {

            //string htmlContent = "<html><body><h1>Hello, World!</h1>" +
            //   "<img src='https://cdn.pixabay.com/photo/2014/02/27/16/10/flowers-276014_1280.jpg' " +
            //   "alt='Example Image'></body></html>";

            #region generate pdf with heavy images
            double tolalsize = 0;
            string[] imagesUrl =
            {
                "https://kartik786-git.github.io/TestStaticPage/DinkToPdfFile/SamplePNGImage_10mbmb.png",
                "https://kartik786-git.github.io/TestStaticPage/DinkToPdfFile/SamplePNGImage_20mbmb.png",
                "https://kartik786-git.github.io/TestStaticPage/DinkToPdfFile/file_example_TIFF_10MB.tiff",
                "https://kartik786-git.github.io/TestStaticPage/DinkToPdfFile/pexels-eberhard-grossgasteiger-1366919.jpg",
                "https://kartik786-git.github.io/TestStaticPage/DinkToPdfFile/pexels-piccinng-3052361.jpg",
                "https://kartik786-git.github.io/TestStaticPage/DinkToPdfFile/pexels-jarod-lovekamp-3791466.jpg"
            };

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<html><body>");
            stringBuilder.Append("<h1>Hello, Friend this is pdf compress session!</h1>");
            for (int i = 0; i < imagesUrl.Length; i++)
            {
                // using with httpclient
                using var client = new HttpClient();

                var imageData = client.GetByteArrayAsync(imagesUrl[i]).Result;

                var comprssSize = _pdfGenerationService.CompressImageFromImageBytes(imageData, 50);

                int byteArrayLength = imageData.Length;
                double megabytes = (double)byteArrayLength / (1024 * 1024);
                tolalsize += megabytes;

                string fileName = Path.GetFileName(imagesUrl[i]);
                stringBuilder.Append($"<h1>Image Namme {fileName} and size {megabytes} mb</h1><p><img src='data:image/jpeg;base64,{_pdfGenerationService.ConvertImageToBase64(comprssSize)}' alt='Example Image'></p>");
                //stringBuilder.Append($"<h1>Image Namme {fileName} and size {megabytes} mb</h1><p><img src='{imagesUrl[i]}'alt='Example Image'></p>");
            }
            stringBuilder.Append($"<h1 style=\"font-size:60px;\">Total real file size is {tolalsize}</h1>");
            stringBuilder.Append("</body></html>");

            var pdfBytes = _pdfGenerationService.GeneratePdf(stringBuilder.ToString());

            #endregion

            //var pdfBytes = _pdfGenerationService.GeneratePdf(htmlContent);
            return File(pdfBytes, "application/pdf", $"{DateTime.Now.ToString()}.pdf");
        }
    }
}
