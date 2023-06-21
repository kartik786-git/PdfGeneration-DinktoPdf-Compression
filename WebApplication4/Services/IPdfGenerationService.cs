namespace WebApplication4.Services
{
    public interface IPdfGenerationService
    {
        byte[] GeneratePdf(string htmlContent);
        string ConvertImageToBase64(byte[] imageBytes);
        byte[] CompressImageFromImageBytes(byte[] imageBytes, long quality);
    }
}