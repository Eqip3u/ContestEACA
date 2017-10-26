using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ContestEACA.Data;
using ContestEACA.Models;
using ContestEACA.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace ContestEACA.Controllers
{
    public partial class PostsController : Controller
    {
        // Email Check
        private async Task<bool> EmailConfirmed()
        {
            return !await _userManager.IsEmailConfirmedAsync(_userManager.GetUserAsync(User).Result);
        }

        // Like UP
        [Authorize]
        public IActionResult LikeUp(int? id)
        {

            var post = _context.Posts.Include(x => x.Likes).SingleOrDefault(x => x.ID == id);

            var user = _context.Users.SingleOrDefault(x => x.Email == User.Identity.Name);

            foreach (var item in post.Likes)
                if (item.UserId == user.Id)
                    return StatusCode(400);


            post.Rating++;

            _context.Update(post);

            var like = new Like();
            like.PostId = post.ID;
            like.UserId = user.Id;
            _context.Likes.Add(like);

            _context.SaveChanges();

            return Json(post.Rating);

        }

        // Parsing URL 
        private void ParsingURLYouTube(Post post)
        {
            var userlink = post.LinkWork;
            string result = "";
            var index = userlink.IndexOf('=') + 1;

            for (int i = index; i < userlink.Length; i++)
                result += userlink[i];


            ViewBag.ParsURL = result;
        }



        //Create File to IO
        private async Task AddFileToPost(Post post, IFormFile uploadedFile)
        {

            if (uploadedFile != null)
            {
                Directory.CreateDirectory(_appEnvironment.WebRootPath + "/Files/" + User.Identity.Name);
                string path = "/Files/" + User.Identity.Name + "/" + uploadedFile.FileName;

                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
                Models.FileModel file = new Models.FileModel { Name = uploadedFile.FileName, Path = path };

                _context.Add(file);
                _context.SaveChanges();

                post.FileId = file.Id;
            }
            else
            {
                post.File = null;
                post.FileId = null;
            }
        }

        //POST: Change File To User
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeFile(int id, Post post, IFormFile uploadedFile)
        {
            if (id != post.ID)
                return NotFound();

            var user = await _userManager.GetUserAsync(User);

            var updatepost = await _context.Posts.Include(x => x.Author).Include(x => x.Likes).Include(x => x.Nomination).Include(x => x.File).SingleOrDefaultAsync(m => m.ID == post.ID);

            if (!(user.Email == updatepost.Author.Email))
            {
                return PartialView("_AccessDenied");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    updatepost.Author = await _context.Users.FirstOrDefaultAsync(x => x.Id == user.Id);
                    updatepost.DateModified = DateTime.Now;

                    await AddFileToPost(updatepost, uploadedFile);

                    _context.Update(updatepost);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationPostExists(post.ID)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(post);
        }


    }
}
