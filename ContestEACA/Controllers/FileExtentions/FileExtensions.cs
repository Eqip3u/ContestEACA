using ContestEACA.Data;
using ContestEACA.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ContestEACA.Controllers.FileExtentions
{
    public static class FileExtensions
    {
        public static async Task<int?> AddFileContestDatabase(this IFormFile file, ApplicationContext _context, IHostingEnvironment _appEnvironment)
        {
            if (file != null)
            {
                Directory.CreateDirectory(_appEnvironment.WebRootPath + "/Files/Contests/");
                string path = "/Files/Contests/" + file.FileName;

                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                FileModel resultFile = new FileModel { Name = file.FileName, Path = path };

                _context.Add(resultFile);
                _context.SaveChanges();

                return resultFile.Id;
            }
            return null;
        }
    }
}
