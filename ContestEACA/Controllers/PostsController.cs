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
        private readonly ApplicationContext _context;
        private UserManager<ApplicationUser> _userManager;
        private IHostingEnvironment _appEnvironment;

        public PostsController(UserManager<ApplicationUser> userManager, ApplicationContext context, IHostingEnvironment appEnvironment)
        {
            _userManager = userManager;
            _context = context;
            _appEnvironment = appEnvironment;
        }

        // GET: Posts
        public async Task<IActionResult> Index(int? nomination)
        {
            IQueryable<Post> posts = _context.Posts
                .Include(x => x.Author)
                .Include(x => x.Nomination);

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

        // GET: Posts/Create
        [Authorize]
        public async Task<IActionResult> Create()
        {
            ViewData["NominationId"] = new SelectList(_context.Nominations, "Id", "Name");

            if (await EmailConfirmed())
            {
                return PartialView("_AccessDenied");
            }

            return View();
        }

        // POST: Posts/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Title,TextWork,LinkWork,Rating,NominationId")] Post post, IFormFile uploadedFile)
        {
            if (await EmailConfirmed())
            {
                return PartialView("_AccessDenied");
            }

            if (ModelState.IsValid)
            {
                post.Author = await _context.Users.FirstOrDefaultAsync(x => x.Id == _userManager.GetUserAsync(User).Result.Id);

                post.DateCreated = DateTime.Now;
                post.DateModified = DateTime.Now;

                await AddFileToPost(post, uploadedFile);

                _context.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(post);
        }



        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var post = await _context.Posts
                .Include(x => x.Author)
                .Include(x => x.Nomination)
                .Include(x => x.File)
                .SingleOrDefaultAsync(m => m.ID == id);

            if (post == null)
                return NotFound();

            CheckFileAndLink(post);

            return View(post);
        }

        // GET: Posts/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var post = await _context.Posts
                .Include(x => x.Nomination)
                .SingleOrDefaultAsync(m => m.ID == id);

            if (post == null)
                return NotFound();

            if (await EmailConfirmed())
            {
                return PartialView("_AccessDenied");
            }

            ViewData["NominationId"] = new SelectList(_context.Nominations, "Id", "Name", post.NominationId);

            return View(post);
        }

        // POST: Posts/Edit/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Post post)
        {
            if (id != post.ID)
                return NotFound();

            var user = await _userManager.GetUserAsync(User);

            var resultpost = await _context.Posts
                .Include(x => x.Nomination)
                .SingleOrDefaultAsync(m => m.ID == id);

            if (!(user.Id == resultpost.AuthorId) || await EmailConfirmed())
            {
                return PartialView("_AccessDenied");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await UpdatePropertyPost(post, resultpost);

                    _context.Update(resultpost);
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

        // GET: Posts/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var post = await _context.Posts.SingleOrDefaultAsync(m => m.ID == id);

            if (post == null)
                return NotFound();

            if (await EmailConfirmed())
            {
                return PartialView("_AccessDenied");
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            var post = await _context.Posts.SingleOrDefaultAsync(m => m.ID == id);
            var user = _userManager.GetUserAsync(User).Result;

            if (await EmailConfirmed())
            {
                return PartialView("_AccessDenied");
            }

            if (user.Email == post.Author.Email)
            {
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return PartialView("_AccessDenied");
        }

        private bool ApplicationPostExists(int id)
        {
            return _context.Posts.Any(e => e.ID == id);
        }
    }
}
