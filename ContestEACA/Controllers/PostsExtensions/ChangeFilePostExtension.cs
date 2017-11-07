using ContestEACA.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ContestEACA.Controllers
{
    public partial class PostsController : Controller
    {
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeFile(int id, Post post, IFormFile uploadedFile)
        {
            var resultpost = await _context.Posts
                .Include(x => x.Nomination)
                .SingleOrDefaultAsync(m => m.ID == id);

            if (!(_userManager.GetUserAsync(User).Result.Email == resultpost.Author.Email))
            {
                return PartialView("_AccessDenied");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await AddFileToPost(resultpost, uploadedFile);

                    _context.Update(resultpost);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationPostExists(post.ID)) return NotFound();
                    else throw;
                }
                return RedirectToAction("Index", "MemberPanel");
            }
            return View(post);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePhoto(int id, Post post, IFormFile uploadedFile)
        {
            var resultpost = await _context.Posts
                .Include(x => x.Nomination)
                .SingleOrDefaultAsync(m => m.ID == id);

            if (!(_userManager.GetUserAsync(User).Result.Email == resultpost.Author.Email))
            {
                return PartialView("_AccessDenied");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await AddPhotoToPost(resultpost, uploadedFile);

                    _context.Update(resultpost);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationPostExists(post.ID)) return NotFound();
                    else throw;
                }
                return RedirectToAction("Index", "MemberPanel");
            }
            return View(post);
        }

    }
}
