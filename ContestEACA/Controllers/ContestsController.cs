using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ContestEACA.Data;
using ContestEACA.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using ContestEACA.Models.ViewModels;

namespace ContestEACA.Controllers
{
    public class ContestsController : Controller
    {
        private readonly ApplicationContext _context;
        private UserManager<ApplicationUser> _userManager;
        private IHostingEnvironment _appEnvironment;

        public ContestsController(UserManager<ApplicationUser> userManager, ApplicationContext context, IHostingEnvironment appEnvironment)
        {
            _userManager = userManager;
            _context = context;
            _appEnvironment = appEnvironment;
        }

        // GET: Contests
        public async Task<IActionResult> Index()
        {
            var posts = await _context.Posts.Where(x => x.Status == StatusPost.Accept).Include(x => x.Contest).ToListAsync();
            var nominations = await _context.Nominations.Include(x => x.Contest).ToListAsync();

            return View(await _context.Contests.Include(x => x.PreImage).OrderBy(x => x.Status).ToListAsync());
        }

        public async Task<IActionResult> Posts(int? id, int? nomination)
        {
            IQueryable<Post> posts = _context.Posts
                .Include(x => x.Author)
                .Include(x => x.Nomination)
                .Include(x => x.Cover)
                .Where(x => x.ContestId == id)
                .Where(x => x.Status == StatusPost.Accept);

            if (nomination != null && nomination != 0)
            {
                posts = posts.Where(x => x.NominationId == nomination);
            }

            List<Nomination> nominations = await _context.Nominations.Where(x => x.ContestId == id).ToListAsync();
            nominations.Insert(0, new Nomination { Id = 0, Name = "Все" });

            PostsViewModel viewModel = new PostsViewModel()
            {
                Posts = posts.ToList(),
                Nominations = new SelectList(nominations, "Id", "Name"),
                HelpNamePost = new Post()
            };

            ViewData["ContestId"] = id;
            ViewData["ContestName"] = _context.Contests.FirstOrDefault(x => x.Id == id).Name;

            return View(viewModel);
        }

        public async Task<IActionResult> Nominations(int? id)
        {
            ViewData["ContestId"] = id;
            ViewData["ContestName"] = _context.Contests.FirstOrDefault(x => x.Id == id).Name;

            return View(await _context.Nominations.Where(x => x.ContestId == id).ToListAsync());
        }

        public async Task<IActionResult> CreatePostInContest(int? id)
        {
            var selectList = await _context.Nominations.Where(x => x.ContestId == id).ToListAsync();

            ViewData["NominationId"] = new SelectList(selectList, "Id", "Name");
            ViewData["ContestId"] = id;

            return View();
        }

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

        private async Task<IActionResult> SaveMainContestSwitch(params Contest[] contest)
        {
            _context.UpdateRange(contest);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Contests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contest = await _context.Contests
                .Include(x => x.PreImage)
                .Include(x => x.Provision)
                .SingleOrDefaultAsync(m => m.Id == id);


            if (contest == null)
            {
                return NotFound();
            }

            return View(contest);
        }


        public async Task<IActionResult> GetContestUsers(int? id)
        {
            return View(await _context.Posts.Where(x => x.ContestId == id)
                .Include(x => x.Author)
                .Include(x => x.Contest)
                    .ThenInclude(x => x.Nominations)
                .ToListAsync());
        }



        // GET: Contests/Create
        public IActionResult Create()
        {
            return View();
        }



        // POST: Contests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,StartTime,EndTime,Rewarding,PreTitle,PreText")] Contest contest, IFormFile uploadPhoto, IFormFile uploadProvision)
        {
            if (ModelState.IsValid)
            {
                if(uploadPhoto != null)
                {
                    contest.PreImageId = await AddFile(uploadPhoto);
                }
                else
                {
                    contest.PreImageId = 20;
                }
                contest.ProvisionId = await AddFile(uploadProvision);

                _context.Add(contest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(contest);
        }

        private async Task<int?> AddFile(IFormFile file)
        {
            if (file != null)
            {
                Directory.CreateDirectory(_appEnvironment.WebRootPath + "/Files/Contests/");
                string path = "/Files/Contests/" + file.FileName;

                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                FileModel resultFile = new FileModel { Name = file.FileName, Path = path };

                _context.Add(resultFile);
                _context.SaveChanges();

                return resultFile.Id;

            }
            return null;
        }

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
                 resultpost.PreImageId = await AddFile(pic);

                _context.Update(resultpost);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(resultpost);
        }

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
                resultpost.ProvisionId = await AddFile(provision);

                _context.Update(resultpost);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(resultpost);
        }

        // GET: Contests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contest = await _context.Contests.SingleOrDefaultAsync(m => m.Id == id);
            if (contest == null)
            {
                return NotFound();
            }
            return View(contest);
        }

        // POST: Contests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Contest contest)
        {
            if (id != contest.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContestExists(contest.Id))
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
            return View(contest);
        }

        // GET: Contests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contest = await _context.Contests
                .Include(x => x.PreImage)
                .Include(x => x.Nominations)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (contest == null)
            {
                return NotFound();
            }

            return View(contest);
        }

        // POST: Contests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contest = await _context.Contests
                .Include(x => x.PreImage)
                .Include(x => x.Nominations)
                .SingleOrDefaultAsync(m => m.Id == id);

            _context.Contests.Remove(contest);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContestExists(int id)
        {
            return _context.Contests.Any(e => e.Id == id);
        }
    }
}
