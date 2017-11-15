using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ContestEACA.Data;
using ContestEACA.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using ContestEACA.Controllers.FileExtentions;

namespace ContestEACA.Controllers.Contests
{
    public class NewsContestsController : Controller
    {
        private readonly ApplicationContext _context;
        private UserManager<ApplicationUser> _userManager;
        private IHostingEnvironment _appEnvironment;

        public NewsContestsController(UserManager<ApplicationUser> userManager, ApplicationContext context, IHostingEnvironment appEnvironment)
        {
            _userManager = userManager;
            _context = context;
            _appEnvironment = appEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var applicationContext = _context.News.Include(n => n.Contest);
            return View(await applicationContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsContest = await _context.News
                .Include(n => n.Contest)
                .Include(x => x.Photo)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (newsContest == null)
            {
                return NotFound();
            }

            return View(newsContest);
        }

        [Authorize(Roles = "admin, moderator")]
        public IActionResult Create(int? contestId)
        {
            ViewData["ContestName"] = _context.Contests.FirstOrDefault(x => x.Id == contestId).Name;
            ViewData["ContestId"] = contestId;
            return View();
        }

        [Authorize(Roles = "admin, moderator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Text,Link,ContestId")] NewsContest newsContest, IFormFile uploadPhoto)
        {
            if (ModelState.IsValid)
            {
                newsContest.DateCreated = DateTime.Now;
                newsContest.PhotoId = await uploadPhoto.AddFileContestDatabase(_context,_appEnvironment);

                _context.Add(newsContest);
                await _context.SaveChangesAsync();
                return RedirectToAction("News", "Contests", new { contestId = newsContest.ContestId });
            }
            ViewData["ContestId"] = new SelectList(_context.Contests, "Id", "Name", newsContest.ContestId);
            return View(newsContest);
        }

        [Authorize(Roles = "admin, moderator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePhoto(int? id, IFormFile pic)
        {
            var news = await _context.News
                .Include(x => x.Photo)
                .Include(x => x.Contest)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (news != null)
            {
                news.PhotoId = await pic.AddFileContestDatabase(_context, _appEnvironment);

                _context.Update(news);
                await _context.SaveChangesAsync();
                return RedirectToAction("News", "Contests", new { contestId = news.ContestId });
            }
            return View(news);
        }

        [Authorize(Roles = "admin, moderator")]
        public async Task<IActionResult> Edit(int? contestId, int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsContest = await _context.News.SingleOrDefaultAsync(m => m.Id == id);
            if (newsContest == null)
            {
                return NotFound();
            }
            ViewData["ContestName"] = _context.Contests.FirstOrDefault(x => x.Id == contestId).Name;
            ViewData["ContestId"] = contestId;
            return View(newsContest);
        }

        [Authorize(Roles = "admin, moderator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Text,Link,ContestId")] NewsContest newsContest)
        {
            if (id != newsContest.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    ViewData["ContestName"] = _context.Contests.FirstOrDefault(x => x.Id == newsContest.ContestId).Name;
                    ViewData["ContestId"] = newsContest.ContestId;

                    _context.Update(newsContest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NewsContestExists(newsContest.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("News", "Contests", new { contestId = newsContest.ContestId });
            }
            ViewData["ContestId"] = new SelectList(_context.Contests, "Id", "Name", newsContest.ContestId);
            return View(newsContest);
        }

        [Authorize(Roles = "admin, moderator")]
        public async Task<IActionResult> Delete(int? contestId, int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsContest = await _context.News
                .Include(n => n.Contest)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (newsContest == null)
            {
                return NotFound();
            }

            ViewData["ContestName"] = _context.Contests.FirstOrDefault(x => x.Id == newsContest.ContestId).Name;
            ViewData["ContestId"] = newsContest.ContestId;

            return View(newsContest);
        }

        [Authorize(Roles = "admin, moderator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? contestId, int id)
        {
            var newsContest = await _context.News.SingleOrDefaultAsync(m => m.Id == id);

            _context.News.Remove(newsContest);
            await _context.SaveChangesAsync();

            ViewData["ContestName"] = _context.Contests.FirstOrDefault(x => x.Id == newsContest.ContestId).Name;
            ViewData["ContestId"] = newsContest.ContestId;

            return RedirectToAction("News", "Contests", new { contestId = contestId });
        }

        private bool NewsContestExists(int id)
        {
            return _context.News.Any(e => e.Id == id);
        }
    }
}
