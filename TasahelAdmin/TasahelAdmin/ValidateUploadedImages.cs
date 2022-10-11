using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TasahelAdmin
{
    public static class ValidateUploadedImages
    {
        public static List<string> images_extentios = new List<string>
        {
            ".JPG",
            ".PNG",
            ".GIF",
            ".WEBP",
            ".TIFF",
            ".PSD",
            ".RAW",
            ".BMP",
            ".HEIF",
            ".INDD",
            ".SVG",
            ".AI",
            ".EPS",
            ".ICO",
        };

        public static bool ValidateImage(this IFormFile file)
        {
            var extention = Path.GetExtension(file.FileName);
            return images_extentios.Select(q => q.ToLower()).Contains(extention.ToLower());
        }
    }
}
