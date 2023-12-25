using Blog.Common.Constants;
using Microsoft.AspNetCore.Http;
using System.Drawing;
using System.Xml.Linq;

namespace Blog.Common.Extensions;
public static class WorkWithFilesExtensions
{
    public static void SaveImage(this IFormFile image, string name, string extension, string folder)
    {
        var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot","images", folder, name + extension);
        using var stream = new FileStream(imagePath, FileMode.Create);
        image.CopyTo(stream);
    }

    public static void RemoveImage(this string imageName, string folder)
    {
        var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot","images", folder, imageName);
        File.Delete(imagePath);
    }
}
