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
using ContestEACA.Models.ViewModels.PostsFSPViewModels;

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
        public async Task<IActionResult> Index(
            int? contestId,
            int? nominationId,
            int page = 1,
            PostsSortState sortOrder = PostsSortState.RatingAsc)
        {

            int pageSize = 10;

            IQueryable<Post> posts = _context.Posts
                .Where(x => x.ContestId == contestId)
                .Include(x => x.Author)
                .Include(x => x.Likes)
                .Include(x => x.Nomination);

            //Фильтрация
            if (nominationId != null && nominationId != 0)
            {
                posts = posts.Where(x => x.NominationId == nominationId);
            }

            //Сортировка
            switch (sortOrder)
            {
                case PostsSortState.RatingAsc:
                    posts = posts.OrderBy(x => x.Rating);
                    break;
                case PostsSortState.RatingDesc:
                    posts = posts.OrderByDescending(x => x.Rating);
                    break;
                case PostsSortState.NameAsc:
                    posts = posts.OrderBy(x => x.Title);
                    break;
                case PostsSortState.NameDesc:
                    posts = posts.OrderByDescending(x => x.Title);
                    break;
            }

            //Пагинация
            var count = await posts.CountAsync();
            var items = await posts.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();


            PostsIndexViewModel viewModel = new PostsIndexViewModel()
            {
                PageViewModel = new PostsPageViewModel(count, page, pageSize),
                SortViewModel = new PostsSortViewModel(sortOrder),
                FilterViewModel = new PostFilterViewModel(_context.Nominations.Where(x => x.ContestId == contestId).ToList(), nominationId),
                Posts = items,
                HelpNamePost = new Post()
            };

            ViewBag.ContestId = contestId;
            ViewBag.ContestName =  _context.Contests.FirstOrDefaultAsync(x => x.Id == contestId).Result.Name;

            return View(viewModel);
        }

        public async Task<IActionResult> ContestList()
        {
            var contests = await _context.Contests
                .Include(x => x.Nominations)
                .Include(x => x.Posts)
                .Where(x => x.Status == StatusContest.Active || x.Status == StatusContest.Coming)
                .OrderBy(x => x.EndTime)
                .ToListAsync();

            return View(contests);
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
