using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SolaraNet.Models
{
    public class ImageLoader
    {
        public async Task<string> UploadImage(IFormFile file)
        {
            const string basePath = "http://37.46.130.239:82/api/image/";
            string path = default;
            if (file.Length > 0)
            {
                var filePath = Path.Combine("",
                    file.FileName);

                using (var stream = System.IO.File.Create(filePath))
                {
                    await file.CopyToAsync(stream);
                }
                path = basePath + filePath;
            }

            return path;
        }
    }
}