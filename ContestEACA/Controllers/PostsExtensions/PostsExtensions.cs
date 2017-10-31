using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ContestEACA.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace ContestEACA.Controllers
{
    public partial class PostsController : Controller
    {
        //Create File to IO
        private async Task AddFileToPost(Post post, IFormFile uploadedFile)
        {

            if (uploadedFile != null)
            {
                post.FileId = await AddFile(uploadedFile);
            }
        }

        //Create File to IO
        private async Task AddPhotoToPost(Post post, IFormFile photo)
        {
            if (photo != null)
            {
                post.CoverId = await AddFile(photo);
            }
        }

        private async Task<int> AddFile(IFormFile file)
        {
            Directory.CreateDirectory(_appEnvironment.WebRootPath + "/Files/" + User.Identity.Name);
            string path = "/Files/" + User.Identity.Name + "/" + file.FileName;

            using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            FileModel resultfile = new FileModel { Name = file.FileName, Path = path };

            _context.Add(resultfile);
            _context.SaveChanges();

            return resultfile.Id;
        }

       
        // Helper Details View
        private void CheckFileAndLink(Post post)
        {
            if (post.LinkWork != null)
            {
                ParsingURLYouTube(post);
                ViewBag.LinkEnable = true;
            }
            else
                ViewBag.LinkEnable = false;

            if (post.File != null)
            {
                ViewData["FilePath"] = post.File.Path;
                ViewBag.FileEnable = true;
            }
            else
                ViewBag.FileEnable = false;
        }
    }
}
