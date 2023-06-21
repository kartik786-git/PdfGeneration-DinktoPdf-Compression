using DinkToPdf;
using DinkToPdf.Contracts;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net;

namespace WebApplication4.Services
{
    public class PdfGenerationService : IPdfGenerationService
    {
        private readonly IConverter _converter;

        public PdfGenerationService(IConverter converter)
        {
            _converter = converter;
        }
        public byte[] GeneratePdf(string htmlContent)
        {
            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings =
            {
                PaperSize = PaperKind.A4,
                Orientation = Orientation.Portrait
            },
                Objects =
            {
                new ObjectSettings()
                {
                    HtmlContent = htmlContent
                }
            }
            };

            var pdfBytes = _converter.Convert(doc);

            return pdfBytes;

        }


        public string ConvertImageToBase64(byte[] imageBytes)
        {
            string base64String = Convert.ToBase64String(imageBytes);
            return base64String;
        }

        public byte[] CompressImageFromImageBytes(byte[] imageBytes, long quality)
        {

            using (var inputStream = new MemoryStream(imageBytes))
            using (var image = Image.FromStream(inputStream))
            using (var outputStream = new MemoryStream())
            {
                var encoderParameters = new EncoderParameters(1);
                encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);

                var jpegEncoder = GetEncoder(ImageFormat.Jpeg);
                image.Save(outputStream, jpegEncoder, encoderParameters);

                return outputStream.ToArray();
            }
        }


        private ImageCodecInfo GetEncoder(ImageFormat format)
        {
            var codecs = ImageCodecInfo.GetImageEncoders();
            foreach (var codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
    }
}
        