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

namespace ContestEACA.Controllers
{
    [Authorize(Roles = "admin, moderator")]
    public class NominationsController : Controller
    {
        private readonly ApplicationContext _context;

        public NominationsController(ApplicationContext context)
        {
            _context = context;
        }

        //public async Task<IActionResult> Index()
        //{
        //    return View();
        //}

        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nomination = await _context.Nominations
                .Include(n => n.Contest)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (nomination == null)
            {
                return NotFound();
            }

            return View(nomination);
        }

        public IActionResult Create(int? contestId)
        {
            if (contestId != null)
                ViewData["ContestName"] = _context.Contests.FirstOrDefault(x => x.Id == contestId).Name;
            else
                ViewData["ContestName"] = "Всех конкурсов";

            ViewData["ContestId"] = contestId;
            ViewData["ContestIdList"] = new SelectList(_context.Contests, "Id", "Name", contestId);
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int? contestId, [Bind("Id,Name,Description,ContestId")] Nomination nomination)
        {
            if (ModelState.IsValid)
            {
                _context.Add(nomination);
                await _context.SaveChangesAsync();
                return RedirectToAction("Nominations", "Contests", new { id = contestId });
            }
            ViewData["ContestId"] = new SelectList(_context.Contests, "Id", "Name", nomination.ContestId);
            return RedirectToAction("Nominations", "Contests", new { id = contestId });
        }

        public async Task<IActionResult> Edit(int? contestId, int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nomination = await _context.Nominations.SingleOrDefaultAsync(m => m.Id == id);
            if (nomination == null)
            {
                return NotFound();
            }
            ViewData["ContestId"] = new SelectList(_context.Contests, "Id", "Name", nomination.ContestId);

            return View(nomination);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? contestId, int id, [Bind("Id,Name,Description,ContestId")] Nomination nomination)
        {
            if (id != nomination.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nomination);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NominationExists(nomination.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                ViewData["ContestName"] = _context.Contests.FirstOrDefault(x => x.Id == nomination.ContestId).Name;
                ViewData["ContestId"] = contestId;

                return RedirectToAction("Nominations", "Contests", new { id = contestId });
            }
            ViewData["ContestId"] = new SelectList(_context.Contests, "Id", "Name", nomination.ContestId);
            return View(nomination);
        }


        public async Task<IActionResult> Delete(int? contestId, int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nomination = await _context.Nominations
                .Include(n => n.Contest)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (nomination == null)
            {
                return NotFound();
            }

            ViewData["ContestId"] = contestId;

            return View(nomination);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? contestId, int id)
        {
            var nomination = await _context.Nominations.SingleOrDefaultAsync(m => m.Id == id);
            _context.Nominations.Remove(nomination);
            await _context.SaveChangesAsync();

            ViewData["ContestName"] = _context.Contests.FirstOrDefault(x => x.Id == nomination.ContestId).Name;
            ViewData["ContestId"] = contestId;

            return RedirectToAction("Nominations", "Contests", new { id = contestId });
        }

        private bool NominationExists(int id)
        {
            return _context.Nominations.Any(e => e.Id == id);
        }
    }
}
