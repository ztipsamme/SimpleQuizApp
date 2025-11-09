using System;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;

namespace SimpleQuizApp.Services;

public class ImageService
{
    public static async Task<Bitmap> LoadAsync(
        string? imageName)
    {
        if (string.IsNullOrEmpty(imageName)) return null;

        var src = await FileService.GetImageAsync(imageName);
        return src;
    }
}