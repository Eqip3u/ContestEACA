using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ContestEACA.Controllers
{
    public partial class PostsController : Controller
    {
        private async Task<bool> EmailConfirmed()
        {
            return !await _userManager.IsEmailConfirmedAsync(_userManager.GetUserAsync(User).Result);
        }
    }
}
