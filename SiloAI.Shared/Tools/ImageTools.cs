namespace SiloAI.Shared.Tools;
public static class ImageTools
{
    public static string ConvertImageByteToBase64String(byte[] imageBytes)
        => $"data:image/jpeg;base64,{Convert.ToBase64String(imageBytes)}";
}
