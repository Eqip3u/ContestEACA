using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ContestEACA.Data;
using ContestEACA.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using ContestEACA.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using ContestEACA.Controllers.FileExtentions;

namespace ContestEACA.Controllers
{
    public partial class ContestsController : Controller
    {
        private readonly ApplicationContext _context;
        private UserManager<ApplicationUser> _userManager;
        private IHostingEnvironment _appEnvironment;

        public ContestsController(UserManager<ApplicationUser> userManager, ApplicationContext context, IHostingEnvironment appEnvironment)
        {
            _userManager = userManager;
            _context = context;
            _appEnvironment = appEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var posts = await _context.Posts.Where(x => x.Status == StatusPost.Accept).Include(x => x.Contest).ToListAsync();
            var nominations = await _context.Nominations.Include(x => x.Contest).ToListAsync();
            var news = await _context.News.Include(x => x.Contest).ToListAsync();

            return View(await _context.Contests
                .Include(x => x.PreImage)
                .OrderBy(x => x.Status)
                .ToListAsync());
        }

        public async Task<IActionResult> Posts(int? id, int? nomination)
        {
            IQueryable<Post> posts = _context.Posts
                .Include(x => x.Author)
                .Include(x => x.Nomination)
                .Include(x => x.Cover)
                .Where(x => x.ContestId == id)
                .Where(x => x.Status == StatusPost.Accept);

            if (nomination != null && nomination != 0)
            {
                posts = posts.Where(x => x.NominationId == nomination);
            }

            List<Nomination> nominations = await _context.Nominations.Where(x => x.ContestId == id).ToListAsync();
            nominations.Insert(0, new Nomination { Id = 0, Name = "Все" });

            PostsViewModel viewModel = new PostsViewModel()
            {
                Posts = posts.ToList(),
                Nominations = new SelectList(nominations, "Id", "Name"),
                HelpNamePost = new Post()
            };

            ViewData["ContestId"] = id;
            ViewData["ContestName"] = _context.Contests.FirstOrDefault(x => x.Id == id).Name;

            return View(viewModel);
        }

        public async Task<IActionResult> Nominations(int? id)
        {
            ViewData["ContestId"] = id;
            ViewData["ContestName"] = _context.Contests.FirstOrDefault(x => x.Id == id).Name;

            return View(await _context.Nominations.Where(x => x.ContestId == id).ToListAsync());
        }

        [Authorize]
        public async Task<IActionResult> CreatePostInContest(int? id)
        {
            var selectList = await _context.Nominations.Where(x => x.ContestId == id).ToListAsync();

            ViewData["NominationId"] = new SelectList(selectList, "Id", "Name");
            ViewData["ContestId"] = id;

            return View();
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contest = await _context.Contests
                .Include(x => x.PreImage)
                .Include(x => x.Provision)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (contest == null)
            {
                return NotFound();
            }

            return View(contest);
        }

        public async Task<IActionResult> GetContestUsers(int? id)
        {
            return View(await _context.Posts.Where(x => x.ContestId == id)
                .Include(x => x.Author)
                .Include(x => x.Contest)
                    .ThenInclude(x => x.Nominations)
                .ToListAsync());
        }

        /// <summary>
        /// CRUD Next
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Contest contest, IFormFile uploadPhoto, IFormFile uploadProvision)
        {
            if (ModelState.IsValid)
            {
                contest.PreImageId = await uploadPhoto.AddFileContestDatabase(_context, _appEnvironment);
                contest.ProvisionId = await uploadProvision.AddFileContestDatabase(_context, _appEnvironment);

                contest.DateCreated = DateTime.Now;
                contest.WhoCreated = await _context.Users.FirstOrDefaultAsync(x => x.Id == _userManager.GetUserAsync(User).Result.Id);

                contest.DateModified = DateTime.Now;
                contest.WhoModified = await _context.Users.FirstOrDefaultAsync(x => x.Id == _userManager.GetUserAsync(User).Result.Id);

                _context.Add(contest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(contest);
        }



        [Authorize(Roles = "admin, moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contest = await _context.Contests.SingleOrDefaultAsync(m => m.Id == id);
            if (contest == null)
            {
                return NotFound();
            }
            return View(contest);
        }

        [Authorize(Roles = "admin, moderator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Contest contest)
        {
            if (id != contest.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    contest.DateModified = DateTime.Now;
                    contest.WhoModified = await _context.Users.FirstOrDefaultAsync(x => x.Id == _userManager.GetUserAsync(User).Result.Id);

                    _context.Update(contest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContestExists(contest.Id))
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
            return View(contest);
        }

        [Authorize(Roles = "admin, moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contest = await _context.Contests
                .Include(x => x.PreImage)
                .Include(x => x.Nominations)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (contest == null)
            {
                return NotFound();
            }

            return View(contest);
        }

        [Authorize(Roles = "admin, moderator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contest = await _context.Contests
                .Include(x => x.PreImage)
                .Include(x => x.Nominations)
                .SingleOrDefaultAsync(m => m.Id == id);

            _context.Contests.Remove(contest);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContestExists(int id)
        {
            return _context.Contests.Any(e => e.Id == id);
        }
    }
}
