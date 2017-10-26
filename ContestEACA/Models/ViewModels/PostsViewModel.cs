using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ContestEACA.Models.ViewModels
{
    public class PostsViewModel
    {
        public ApplicationUser User { get; set; }

        public IEnumerable<Post> Posts { get; set; }

        [Display(Name = "Номинация")]
        public SelectList Nominations { get; set; }

        public Post HelpNamePost { get; set; }
    }
}
