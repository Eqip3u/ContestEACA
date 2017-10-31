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
        public async Task<IActionResult> Index(int? nomination, int page = 1, SortState sortOrder = SortState.Status)
        {

            int pageSize = 5;


            IQueryable<Post> posts = _context.Posts
                .Include(x => x.Author)
                .Include(x => x.Likes)
                .Include(x => x.Nomination);

            //Фильтрация
            if (nomination != null && nomination != 0)
            {
                posts = posts.Where(x => x.NominationId == nomination);
            }

            //Сортировка
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
                case SortState.Status:
                    posts = posts.OrderBy(x => x.Status);
                    break;
                default:
                    posts = posts.OrderBy(x => x.Status);
                    break;
            }

            //Пагинация
            var count = await posts.CountAsync();
            var items = await posts.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();


            AdminIndexViewModel viewModel = new AdminIndexViewModel()
            {
                PageViewModel = new PageViewModel(count, page, pageSize),
                SortViewModel = new AdminSortViewModel(sortOrder),
                FilterViewModel = new AdminFilterViewModel(_context.Nominations.ToList(), nomination),
                Posts = items,
                HelpNamePost = items.FirstOrDefault()
            };

            return View(viewModel);
        }


        // GET: AdminPanel/SetStatusPost/5
        [HttpGet]
        public async Task<IActionResult> SetStatusPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .SingleOrDefaultAsync(m => m.ID == id);

            if (post == null)
            {
                return NotFound();
            }

            var resultpost = new ChangeStatusPostViewModel()
            {
                Post = post,
                Status = post.Status
            };

            return View(resultpost);
        }

        [HttpPost]
        public async Task<IActionResult> SetStatusPost(int? id, ChangeStatusPostViewModel model)
        {
            if (model == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.SingleOrDefaultAsync(m => m.ID == id);

            if (post == null)
            {
                return NotFound();
            }

            post.Status = model.Status;

            _context.Update(post);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
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
