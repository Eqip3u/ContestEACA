using ContestEACA.Controllers.FileExtentions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        [Authorize(Roles = "admin, moderator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeFile(int? id, IFormFile pic)
        {
            var resultpost = await _context.Contests
                .Include(x => x.Nominations)
                .Include(x => x.PreImage)
                .Include(x => x.Provision)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (resultpost != null)
            {
                resultpost.PreImageId = await pic.AddFileContestDatabase(_context, _appEnvironment);

                _context.Update(resultpost);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(resultpost);
        }

        [Authorize(Roles = "admin, moderator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeProvision(int? id, IFormFile provision)
        {
            var resultpost = await _context.Contests
                .Include(x => x.Nominations)
                .Include(x => x.PreImage)
                .Include(x => x.Provision)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (resultpost != null)
            {
                resultpost.ProvisionId = await provision.AddFileContestDatabase(_context, _appEnvironment);

                _context.Update(resultpost);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(resultpost);
        }
    }
}
