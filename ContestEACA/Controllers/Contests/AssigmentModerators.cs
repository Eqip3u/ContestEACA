using ContestEACA.Models;
using ContestEACA.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContestEACA.Controllers
{
    public partial class ContestsController : Controller
    {
        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> AssignmentModerators(int contestId)
        {
            var contests = await _context.Contests.Include(x => x.Moderators).ThenInclude(x => x.User).ToListAsync();

            var moderators = new List<ApplicationUser>();

            foreach (var c in contests)
            {
                if (c.Id == contestId)
                {
                    var moderatorslist = c.Moderators.Select(x => x.User).ToList();
                    foreach (var item in moderatorslist)
                        moderators.Add(item);
                }
            }

            AssignmentModeratorsViewModel viewModel = new AssignmentModeratorsViewModel()
            {
                ContestId = contestId,
                Users = new SelectList(await _context.Users.ToListAsync(), "Id", "Email"),
                Moderators = moderators
            };

            return View(viewModel);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> AssignmentModerators(int contestId, string userId)
        {
            ModerateUserContest usercontest = new ModerateUserContest()
            {
                ContestId = contestId,
                UserId = userId
            };

            _context.Add(usercontest);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteAssignmentModerator(int contestId, string userId)
        {
            var keys = await _context.ModerateUsersContests.SingleOrDefaultAsync(x => x.UserId == userId && x.ContestId == contestId);

            _context.Remove(keys);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
