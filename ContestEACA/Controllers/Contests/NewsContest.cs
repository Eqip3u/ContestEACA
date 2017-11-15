using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContestEACA.Controllers
{
    public partial class ContestsController : Controller
    {
        public async Task<IActionResult> News(int? contestId)
        {
            var news = await _context.News
                .Include(x => x.Contest)
                .Include(x => x.Photo)
                .Where(x => x.ContestId == contestId)
                .ToListAsync();

            var userId = _userManager.GetUserId(User);
            if (userId != null)
            {
                var AccessQuery = await _context.ModerateUsersContests
                    .FirstOrDefaultAsync(x => x.ContestId == contestId && x.UserId == userId);

                if (AccessQuery != null)
                    ViewData["AccessModerator"] = true;
                else
                    ViewData["AccessModerator"] = false;
            }

            ViewData["ContestName"] = _context.Contests.FirstOrDefault(x => x.Id == contestId).Name;
            ViewData["ContestId"] = contestId;

            return View(news);
        }
    }
}
