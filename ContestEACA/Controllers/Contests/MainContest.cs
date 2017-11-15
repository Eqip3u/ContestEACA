using ContestEACA.Models;
using Microsoft.AspNetCore.Authorization;
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
        private async Task<IActionResult> SaveMainContestSwitch(params Contest[] contest)
        {
            _context.UpdateRange(contest);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> SetMainContest(int? id)
        {
            var contest = await _context.Contests.FirstOrDefaultAsync(x => x.Id == id);
            var contestSwitch = await _context.Contests.FirstOrDefaultAsync(x => x.MainContest);

            if (contestSwitch == null)
            {
                contest.MainContest = true;
                return await SaveMainContestSwitch(contest);

            }
            else
            {
                if (contest.Id == contestSwitch.Id)
                {
                    contest.MainContest = false;
                    return await SaveMainContestSwitch(contest);
                }

                contestSwitch.MainContest = false;
                contest.MainContest = true;
                return await SaveMainContestSwitch(contest, contestSwitch);
            }
        }
    }
}
