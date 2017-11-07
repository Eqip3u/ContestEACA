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
    [Authorize(Roles = "admin")]
    public class NominationsController : Controller
    {
        private readonly ApplicationContext _context;

        public NominationsController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: Nominations
        public async Task<IActionResult> Index()
        {
            var applicationContext = _context.Nominations.Include(n => n.Contest);
            return View(await applicationContext.ToListAsync());
        }

        // GET: Nominations/Details/5
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

        // GET: Nominations/Create
        public IActionResult Create(int? HelperId)
        {
            if (HelperId != null)
                ViewData["ContestName"] = _context.Contests.FirstOrDefault(x => x.Id == HelperId).Name;
            else
                ViewData["ContestName"] = "Всех конкурсов";

            ViewData["ContestId"] = HelperId;
            ViewData["ContestIdList"] = new SelectList(_context.Contests, "Id", "Name", HelperId);
            return View();
        }

        // POST: Nominations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int? HelperId, [Bind("Id,Name,Description,ContestId")] Nomination nomination)
        {
            if (ModelState.IsValid)
            {
                _context.Add(nomination);
                await _context.SaveChangesAsync();

                ViewData["ContestName"] = _context.Contests.FirstOrDefault(x => x.Id == nomination.ContestId).Name;
                ViewData["ContestId"] = HelperId;

                return View("~/Views/Contests/Nominations.cshtml", await
                        _context.Nominations
                            .Where(x => x.ContestId == nomination.ContestId)
                            .ToListAsync());

            }
            ViewData["ContestId"] = new SelectList(_context.Contests, "Id", "Name", nomination.ContestId);
            return View(nomination);
        }

        // GET: Nominations/Edit/5
        public async Task<IActionResult> Edit(int? HelperId, int? id)
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

        // POST: Nominations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? HelperId, int id, [Bind("Id,Name,Description,ContestId")] Nomination nomination)
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
                ViewData["ContestId"] = HelperId;

                return View("~/Views/Contests/Nominations.cshtml", await
                    _context.Nominations
                        .Where(x => x.ContestId == nomination.ContestId)
                        .ToListAsync());
            }
            ViewData["ContestId"] = new SelectList(_context.Contests, "Id", "Name", nomination.ContestId);
            return View(nomination);
        }

        // GET: Nominations/Delete/5
        public async Task<IActionResult> Delete(int? HelperId, int? id)
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

        // POST: Nominations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? HelperId, int id)
        {
            var nomination = await _context.Nominations.SingleOrDefaultAsync(m => m.Id == id);
            _context.Nominations.Remove(nomination);
            await _context.SaveChangesAsync();

            ViewData["ContestName"] = _context.Contests.FirstOrDefault(x => x.Id == nomination.ContestId).Name;
            ViewData["ContestId"] = HelperId;

            return View("~/Views/Contests/Nominations.cshtml", await
                _context.Nominations
                    .Where(x => x.ContestId == nomination.ContestId)
                    .ToListAsync());

        }

        private bool NominationExists(int id)
        {
            return _context.Nominations.Any(e => e.Id == id);
        }
    }
}
