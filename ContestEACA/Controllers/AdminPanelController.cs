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
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace ContestEACA.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminPanelController : Controller
    {
        private readonly ApplicationPostDbContext _context;
        private IHostingEnvironment _appEnvironment;

        public AdminPanelController(ApplicationPostDbContext context, IHostingEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;

        }

        // GET: AdminPanel
        public async Task<IActionResult> Index()
        {
            return View(await _context.Posts.Include(x => x.Likes).Include(x => x.Nomination).ToListAsync());
        }

        // GET: AdminPanel/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationPost = await _context.Posts
                .SingleOrDefaultAsync(m => m.ID == id);
            if (applicationPost == null)
            {
                return NotFound();
            }

            return View(applicationPost);
        }

        // GET: AdminPanel/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AdminPanel/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Title,TextWork,LinkWork,Rating,NominationId")] Post post, IFormFile uploadedFile)
        {
            if (ModelState.IsValid)
            {
                post.Author = User.Identity.Name;

                post.DateCreated = DateTime.Now;
                post.DateModified = DateTime.Now;

                await AddFileToUser(post, uploadedFile);

                _context.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(post);
        }

        //Create File to IO
        private async Task AddFileToUser(Post post, IFormFile uploadedFile)
        {

            if (uploadedFile != null)
            {
                Directory.CreateDirectory(_appEnvironment.WebRootPath + "/Files/" + User.Identity.Name);
                string path = "/Files/" + User.Identity.Name + "/" + uploadedFile.FileName;

                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
                FileModel file = new FileModel { Name = uploadedFile.FileName, Path = path };

                _context.Files.Add(file);
                _context.SaveChanges();

                post.File = file.Path;
            }
        }


        // GET: AdminPanel/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationPost = await _context.Posts.SingleOrDefaultAsync(m => m.ID == id);
            if (applicationPost == null)
            {
                return NotFound();
            }
            return View(applicationPost);
        }

        // POST: AdminPanel/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Title,Work,Rating,Author")] Post post)
        {
            if (id != post.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    post.Author = User.Identity.Name;
                    post.DateModified = DateTime.Now;

                    _context.Update(post);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationPostExists(post.ID))
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
            return View(post);
        }

        // GET: AdminPanel/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationPost = await _context.Posts
                .SingleOrDefaultAsync(m => m.ID == id);
            if (applicationPost == null)
            {
                return NotFound();
            }

            return View(applicationPost);
        }

        // POST: AdminPanel/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var applicationPost = await _context.Posts.SingleOrDefaultAsync(m => m.ID == id);
            _context.Posts.Remove(applicationPost);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApplicationPostExists(int id)
        {
            return _context.Posts.Any(e => e.ID == id);
        }
    }
}
