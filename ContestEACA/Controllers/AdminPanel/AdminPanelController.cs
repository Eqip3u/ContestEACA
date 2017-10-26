using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ContestEACA.Data;
using ContestEACA.Models;
using ContestEACA.Models.EnumHelpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using ContestEACA.Models.ViewModels;

namespace ContestEACA.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminPanelController : Controller
    {
        private readonly ApplicationContext _context;

        public AdminPanelController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: AdminPanel
        public async Task<IActionResult> Index(int? nomination, SortState sortOrder = SortState.DateCreateAsc)
        {
            IQueryable<Post> posts = _context.Posts.Include(x => x.Author).Include(x => x.Likes).Include(x => x.Nomination);

            ViewData["RatingSort"] = sortOrder == SortState.RatingAsc ? SortState.RatingDesc : SortState.RatingAsc;
            ViewData["DateCreateSort"] = sortOrder == SortState.DateCreateAsc ? SortState.DateCreateDesc : SortState.DateCreateAsc;

            switch (sortOrder)
            {
                case SortState.RatingAsc:
                    posts = posts.OrderBy(x => x.Rating);
                    break;
                case SortState.RatingDesc:
                    posts = posts.OrderByDescending(x => x.Rating);
                    break;
                case SortState.DateCreateAsc:
                    posts = posts.OrderBy(x => x.DateCreated);
                    break;
                case SortState.DateCreateDesc:
                    posts = posts.OrderByDescending(x => x.DateCreated);
                    break;
                default:
                    posts = posts.OrderBy(x => x.DateCreated);
                    break;
            }

            if (nomination != null && nomination != 0)
            {
                posts = posts.Where(x => x.NominationId == nomination);
            }

            List<Nomination> nominations = await _context.Nominations.ToListAsync();
            nominations.Insert(0, new Nomination { Id = 0, Name = "All" });

            PostsViewModel viewModel = new PostsViewModel()
            {
                Posts = posts.AsNoTracking().ToList(),
                Nominations = new SelectList(nominations, "Id", "Name"),
                HelpNamePost = new Post()
            };

            return View(viewModel);
        }



        // GET: AdminPanel/Delete/5
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

        // POST: AdminPanel/Delete/5
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
