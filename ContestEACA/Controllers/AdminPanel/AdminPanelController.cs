using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ContestEACA.Data;
using ContestEACA.Models;
using ContestEACA.Models.EnumHelpers;
using Microsoft.AspNetCore.Authorization;
using ContestEACA.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using ContestEACA.Services;
using ContestEACA.Models.ManageViewModels;

namespace ContestEACA.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminPanelController : Controller
    {
        private readonly ApplicationContext _context;
        private UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;


        public AdminPanelController(
            UserManager<ApplicationUser> userManager, 
            ApplicationContext context,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _context = context;
            _emailSender = emailSender;
        }


        // GET: AdminPanel
        public async Task<IActionResult> Index(int? contest, int page = 1, SortState sortOrder = SortState.Status)
        {

            int pageSize = 10;


            IQueryable<Post> posts = _context.Posts
                .Include(x => x.Author)
                .Include(x => x.Likes)
                .Include(x => x.Nomination);

            //Фильтрация
            if (contest != null && contest != 0)
            {
                posts = posts.Where(x => x.ContestId == contest);
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
                case SortState.NominationAsc:
                    posts = posts.OrderBy(x => x.Nomination.Name);
                    break;
                case SortState.NominationDesc:
                    posts = posts.OrderByDescending(x => x.Nomination.Name);
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


            SFPIndexViewModel viewModel = new SFPIndexViewModel()
            {
                PageViewModel = new SFPPageViewModel(count, page, pageSize),
                SortViewModel = new SFPSortViewModel(sortOrder),
                FilterViewModel = new SFPFilterViewModel(_context.Contests.ToList(), contest),
                Posts = items,
                HelpNamePost = items.FirstOrDefault()
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> UserDetails(string userId)
        {
            return View(await _context.Users
                .Include(x => x.Posts)
                    .ThenInclude(x => x.Nomination)
                .Include(x => x.Posts)
                    .ThenInclude(x => x.Contest)
                .FirstOrDefaultAsync(x => x.Id == userId));
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

            var user = _context.Users.FirstOrDefault(x => x.Id == post.AuthorId);

            // На продакшене раскоментить
            //await _emailSender.SendEmailModerateStatusPost(user.Email);

            ModerateWorkViewModel viewmodel = new ModerateWorkViewModel()
            {
                Posts = _context.Posts
                .Include(x => x.Contest)
                .Include(x => x.Nomination)
                .Where(x => x.Status == StatusPost.AwaitingForModeration)
            };

            return View("~/Views/Manage/ModerateWork.cshtml", viewmodel);
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
