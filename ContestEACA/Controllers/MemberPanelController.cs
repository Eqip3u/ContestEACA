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
using Microsoft.AspNetCore.Identity;
using ContestEACA.Models.ViewModels;
using ContestEACA.Models.EnumHelpers;

namespace ContestEACA.Controllers
{
    [Authorize(Roles = "user, admin")]
    public class MemberPanelController : Controller
    {
        private readonly ApplicationContext _context;
        private UserManager<ApplicationUser> _userManager;

        public MemberPanelController(ApplicationContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int? contest, int page = 1, ContestSortState sortOrder = ContestSortState.Status)
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);

            int pageSize = 10;

            var posts = _context.Users
                .Include(x => x.Posts)
                .ThenInclude(x => x.Contest)
                .ThenInclude(x => x.Nominations)
                .FirstOrDefault(x => x.Id == user.Id)
                .Posts
                .ToAsyncEnumerable();

            //Фильтрация
            if (contest != null && contest != 0)
            {
                posts = posts.Where(x => x.ContestId == contest);
            }

            //Сортировка
            switch (sortOrder)
            {
                case ContestSortState.RatingAsc:
                    posts = posts.OrderBy(x => x.Rating);
                    break;
                case ContestSortState.RatingDesc:
                    posts = posts.OrderByDescending(x => x.Rating);
                    break;
                case ContestSortState.DateCreateAsc:
                    posts = posts.OrderBy(x => x.DateCreated);
                    break;
                case ContestSortState.DateCreateDesc:
                    posts = posts.OrderByDescending(x => x.DateCreated);
                    break;
                case ContestSortState.NominationAsc:
                    posts = posts.OrderBy(x => x.Nomination.Name);
                    break;
                case ContestSortState.NominationDesc:
                    posts = posts.OrderByDescending(x => x.Nomination.Name);
                    break;
                case ContestSortState.Status:
                    posts = posts.OrderBy(x => x.Status);
                    break;
                default:
                    posts = posts.OrderBy(x => x.Status);
                    break;
            }

            //Пагинация
            var count = await posts.Count();
            var items = await posts.Skip((page - 1) * pageSize).Take(pageSize).ToList();


            ContestIndexViewModel viewModel = new ContestIndexViewModel()
            {
                PageViewModel = new ContestPageViewModel(count, page, pageSize),
                SortViewModel = new ContestSortViewModel(sortOrder),
                FilterViewModel = new ContestFilterViewModel(_context.Contests.ToList(), contest),
                Posts = items,
                HelpNamePost = items.FirstOrDefault()
            };

            ViewBag.CountWork = posts.Count();

            return View(viewModel);
            
        }
    }
}
