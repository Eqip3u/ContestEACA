using ContestEACA.Models;
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

        public async Task<IActionResult> ArchiveContests()
        {
            return View(await _context.Contests
                .Where(x => x.Status == StatusContest.Done)
                .ToListAsync());
        }

        public async Task<IActionResult> ArchiveContestWorks(int? contestId)
        {
            return View(await _context.Posts
                .Include(x => x.Nomination)
                .Include(x => x.Author)
                .Where(x => x.ContestId == contestId && x.Status == StatusPost.Accept).ToListAsync());
        }
    }
}
