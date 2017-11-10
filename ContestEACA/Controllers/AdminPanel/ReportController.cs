using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using ContestEACA.Models;
using ContestEACA.Data;
using Microsoft.EntityFrameworkCore;
using ContestEACA.Models.ViewModels.AdminPanelViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ContestEACA.Controllers.AdminPanel
{
    public class ReportController : Controller
    {
        private readonly ApplicationContext _context;
        private UserManager<ApplicationUser> _userManager;

        public ReportController(UserManager<ApplicationUser> userManager, ApplicationContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index(int? contest, string nomination)
        {
            IQueryable<Post> posts = _context.Posts.Include(p => p.Author).Include(x => x.Nomination).Include(x => x.Contest);

            if (contest != null && contest != 0)
            {
                posts = posts.Where(p => p.ContestId == contest);
            }

            if (!String.IsNullOrEmpty(nomination))
            {
                posts = posts.Where(p => p.Nomination.Name.Contains(nomination));
            }

            List<Contest> contests = await _context.Contests.ToListAsync();

            contests.Insert(0, new Contest { Name = "Все", Id = 0 });

            ReportViewModel viewModel = new ReportViewModel
            {
                Posts = posts.ToList(),
                Contests = new SelectList(contests, "Id", "Name"),
                Nomination = nomination
            };
            return View(viewModel);
        }
    }
}