using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ContestEACA.Data;
using ContestEACA.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;



namespace ContestEACA.Controllers
{
    //[Authorize(Roles ="admin")]
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext _userContext;
        private readonly ApplicationPostDbContext _context;
        private UserManager<ApplicationUser> _userManager;
        private IHostingEnvironment _appEnvironment;

        public PostsController(UserManager<ApplicationUser> userManager, ApplicationPostDbContext context, IHostingEnvironment appEnvironment, ApplicationDbContext userContext)
        {
            _userManager = userManager;
            _context = context;
            _appEnvironment = appEnvironment;
            _userContext = userContext;
        }

        // GET: ApplicationPosts
        public async Task<IActionResult> Index()
        {
            ViewBag.Likes = await _context.Likes.ToListAsync();

            ViewBag.Posts = await _context.Posts.Include(x => x.Likes).ToListAsync();
            
            return View(await _context.Posts.OrderByDescending(x => x.Rating).ToListAsync());
        }


        // Like UP
        [Authorize]
        public IActionResult LikeUp(int? id)
        {

            var post = _context.Posts.Include(x => x.Likes).SingleOrDefault(x => x.ID == id);

            var user = _userContext.Users.SingleOrDefault(x => x.Email == User.Identity.Name);

            foreach (var item in post.Likes)
            {
                if (item.UserId == user.Id)
                {
                    return RedirectToAction("Index");
                }
            }

            post.Rating++;

            _context.Update(post);

            var like = new Like();
            like.PostId = post.ID;
            like.UserId = user.Id;
            _context.Likes.Add(like);

            _context.SaveChanges();

            return RedirectToAction("Index");

        }


        // GET: ApplicationPosts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationPost = await _context.Posts
                .SingleOrDefaultAsync(m => m.ID == id);
            if (applicationPost == null)
            {
                return NotFound();
            }

            return View(applicationPost);
        }

        // GET: ApplicationPosts/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: ApplicationPosts/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Title,Work,Rating")] Post applicationPost, IFormFile uploadedFile)
        {
            if (ModelState.IsValid)
            {
                applicationPost.Author = User.Identity.Name;

                await AddFileToUser(applicationPost, uploadedFile);

                _context.Add(applicationPost);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(applicationPost);
        }

        private async Task AddFileToUser(Post applicationPost, IFormFile uploadedFile)
        {
            if (uploadedFile != null)
            {
                Directory.CreateDirectory(_appEnvironment.WebRootPath + "/Files/" + User.Identity.Name);
                string path = "/Files/" + User.Identity.Name + "/" + uploadedFile.FileName;

                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
                FileModel file = new FileModel { Name = uploadedFile.FileName, Path = path };

                _context.Files.Add(file);
                _context.SaveChanges();

                applicationPost.File = file.Path;
            }
        }

        // GET: ApplicationPosts/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationPost = await _context.Posts.SingleOrDefaultAsync(m => m.ID == id);
            if (applicationPost == null)
            {
                return NotFound();
            }
            return View(applicationPost);
        }

        // POST: ApplicationPosts/Edit/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Title,Work,Rating")] Post applicationPost, IFormFile uploadedFile)
        {
            if (id != applicationPost.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    applicationPost.Author = User.Identity.Name;
                    await AddFileToUser(applicationPost, uploadedFile);
                    _context.Update(applicationPost);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationPostExists(applicationPost.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(applicationPost);
        }

        // GET: ApplicationPosts/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationPost = await _context.Posts
                .SingleOrDefaultAsync(m => m.ID == id);
            if (applicationPost == null)
            {
                return NotFound();
            }

            return View(applicationPost);
        }

        // POST: ApplicationPosts/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var applicationPost = await _context.Posts.SingleOrDefaultAsync(m => m.ID == id);
            _context.Posts.Remove(applicationPost);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApplicationPostExists(int id)
        {
            return _context.Posts.Any(e => e.ID == id);
        }
    }
}
