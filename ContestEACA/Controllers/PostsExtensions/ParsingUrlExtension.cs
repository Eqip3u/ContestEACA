using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ContestEACA.Models;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;

namespace ContestEACA.Controllers
{
    public partial class PostsController : Controller
    {
        private void ParsingURLYouTube(Post post)
        {
            var userlink = post.LinkWork;
            string result = "";
            var index = userlink.IndexOf('=') + 1;

            for (int i = index; i < userlink.Length; i++)
                result += userlink[i];


            ViewBag.ParsURL = result;
        }
    }
}
