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
            return View(await _context.Nominations.ToListAsync());
        }

        // GET: Nominations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nomination = await _context.Nominations
                .SingleOrDefaultAsync(m => m.Id == id);
            if (nomination == null)
            {
                return NotFound();
            }

            return View(nomination);
        }

        // GET: Nominations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Nominations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Nomination nomination)
        {
            if (ModelState.IsValid)
            {
                _context.Add(nomination);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(nomination);
        }

        // GET: Nominations/Edit/5
        public async Task<IActionResult> Edit(int? id)
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
            return View(nomination);
        }

        // POST: Nominations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Nomination nomination)
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
                return RedirectToAction(nameof(Index));
            }
            return View(nomination);
        }

        // GET: Nominations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nomination = await _context.Nominations
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
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var nomination = await _context.Nominations.SingleOrDefaultAsync(m => m.Id == id);
            _context.Nominations.Remove(nomination);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NominationExists(int id)
        {
            return _context.Nominations.Any(e => e.Id == id);
        }
    }
}
