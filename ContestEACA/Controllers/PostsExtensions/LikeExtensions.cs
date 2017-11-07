using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ContestEACA.Models;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace ContestEACA.Controllers
{
    public partial class PostsController : Controller
    {
        [Authorize]
        public IActionResult LikeUp(int? id)
        {
            var post = _context.Posts.Include(x => x.Likes).SingleOrDefault(x => x.ID == id);

            var user = _context.Users.SingleOrDefault(x => x.Email == User.Identity.Name);

            foreach (var item in post.Likes)
                if (item.UserId == user.Id)
                    return StatusCode(400);

            post.Rating++;

            _context.Update(post);

            var like = new Like()
            {
                PostId = post.ID,
                UserId = user.Id
            };

            _context.Likes.Add(like);

            _context.SaveChanges();

            return Json(post.Rating);
        }
    }
}
