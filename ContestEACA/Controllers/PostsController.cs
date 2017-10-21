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
        public async Task<IActionResult> Index(int? nomination)
        {
            IQueryable<Post> posts = _context.Posts.Include(x => x.Likes).Include(x => x.Nomination);

            if (nomination != null && nomination != 0)
            {
                posts = posts.Where(x => x.NominationId == nomination);
            }

            List<Nomination> nominations = await _context.Nominations.ToListAsync();
            nominations.Insert(0, new Nomination { Id = 0, Name = "All" });

            PostsViewModel viewModel = new PostsViewModel()
            {
                Posts = posts.ToList(),
                Nominations = new SelectList(nominations, "Id", "Name"),
                HelpNamePost = new Post()
            };

            return View(viewModel);
        }


        // Like UP
        [Authorize]
        public IActionResult LikeUp(int? id)
        {

            var post = _context.Posts.Include(x => x.Likes).SingleOrDefault(x => x.ID == id);

            var user = _userContext.Users.SingleOrDefault(x => x.Email == User.Identity.Name);

            foreach (var item in post.Likes)
                if (item.UserId == user.Id)
                    return RedirectToAction("Index");


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
                return NotFound();

            var post = await _context.Posts.Include(x => x.Likes).Include(x => x.Nomination).SingleOrDefaultAsync(m => m.ID == id);

            if (post == null)
                return NotFound();

            ViewData["FilePath"] = post.File;
            return View(post);
        }

        // GET: ApplicationPosts/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["NominationId"] = new SelectList(_context.Nominations, "Id", "Name");

            return View();
        }

        // POST: ApplicationPosts/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Title,TextWork,LinkWork,Rating,NominationId")] Post post, IFormFile uploadedFile)
        {
            if (ModelState.IsValid)
            {
                post.Author = User.Identity.Name;

                post.DateCreated = DateTime.Now;
                post.DateModified = DateTime.Now;

                await AddFileToUser(post, uploadedFile);

                _context.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(post);
        }

        //Create File to IO
        private async Task AddFileToUser(Post post, IFormFile uploadedFile)
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

                post.File = file.Path;
            }
        }

        // GET: ApplicationPosts/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var post = await _context.Posts.SingleOrDefaultAsync(m => m.ID == id);

            if (post == null)
                return NotFound();


            ViewData["NominationId"] = new SelectList(_context.Nominations, "Id", "Name", post.NominationId);

            return View(post);
        }

        // POST: ApplicationPosts/Edit/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Rating,File,DateCreated,DateModified,Title,TextWork,LinkWork,NominationId")] Post post, IFormFile uploadedFile)
        {
            if (id != post.ID)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    post.Author = User.Identity.Name;
                    post.DateModified = DateTime.Now;

                    _context.Update(post);
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

        // GET: ApplicationPosts/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var post = await _context.Posts.SingleOrDefaultAsync(m => m.ID == id);

            if (post == null)
                return NotFound();

            return View(post);
        }

        // POST: ApplicationPosts/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            var post = await _context.Posts.SingleOrDefaultAsync(m => m.ID == id);
            var user = _userManager.GetUserAsync(User).Result;
            if (user.Email == post.Author)
            {
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return PartialView("DeletedNotUser");
        }

        private bool ApplicationPostExists(int id)
        {
            return _context.Posts.Any(e => e.ID == id);
        }
    }
}
