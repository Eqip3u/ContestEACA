using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using ContestEACA.Data;
using ContestEACA.Services;
using ContestEACA.Models;
using Microsoft.EntityFrameworkCore;
using ContestEACA.Models.ManageViewModels;
using ContestEACA.Models.ViewModels;
using ContestEACA.Models.EnumHelpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace ContestEACA.Controllers.ModeratePanel
{
    [Authorize(Roles = "admin, moderator")]
    public class ModerateController : Controller
    {
        private readonly ApplicationContext _context;
        private UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;


        public ModerateController(
            UserManager<ApplicationUser> userManager,
            ApplicationContext context,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _context = context;
            _emailSender = emailSender;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            var users = await _context.Users
                .Include(x => x.ContestModerate)
                    .ThenInclude(x => x.Contest)
                        .ThenInclude(x => x.Posts)
                .ToListAsync();

            var viewModel = new List<Contest>();

            foreach (var c in users)
            {
                if (c.Id == user.Id)
                {
                    var contest = c.ContestModerate.Select(x => x.Contest).ToList();
                    foreach (var item in contest)
                        viewModel.Add(item);
                }
            }

            return View(viewModel);
        }

        public async Task<IActionResult> GetAllWorksContest(int? contestId, int? nominationId)
        {
            IQueryable<Post> posts = _context.Posts
                .Include(x => x.Author)
                .Include(x => x.Likes)
                .Include(x => x.Nomination)
                .Where(x => x.ContestId == contestId);

            if (nominationId != null && nominationId != 0)
            {
                posts = posts.Where(x => x.NominationId == nominationId);
            }

            List<Nomination> nominations = await _context.Nominations.Where(x => x.ContestId == contestId).ToListAsync();
            nominations.Insert(0, new Nomination { Id = 0, Name = "Все номинации" });

            PostsViewModel viewModel = new PostsViewModel()
            {
                Posts = posts.ToList(),
                Nominations = new SelectList(nominations, "Id", "Name"),
                HelpNamePost = new Post()
            };
            ViewBag.ContestId = contestId;
            return View(viewModel);
        }

        public async Task<IActionResult> NominationsContest(int? contestId)
        {
            ViewData["ContestId"] = contestId;
            ViewData["ContestName"] = _context.Contests.FirstOrDefault(x => x.Id == contestId).Name;

            return View(await _context.Nominations.Where(x => x.ContestId == contestId).ToListAsync());
        }

        public async Task<IActionResult> GetWaitingModerationWork(int contestId)
        {
            ModerateWorkViewModel viewmodel = new ModerateWorkViewModel()
            {
                Posts = await _context.Posts
                            .Include(x => x.Contest)
                            .Include(x => x.Nomination)
                            .Include(x => x.Author)
                            .Where(x => x.Status == StatusPost.AwaitingForModeration && x.ContestId == contestId)
                            .ToListAsync()
            };

            return View(viewmodel);
        }
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

            // На продакшене раскоментить
            //await _emailSender.SendEmailModerateStatusPost(user.Email);

            return RedirectToAction(nameof(Index));
        }
    }
}