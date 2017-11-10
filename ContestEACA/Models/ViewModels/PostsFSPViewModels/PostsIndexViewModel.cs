using System.Collections.Generic;

namespace ContestEACA.Models.ViewModels.PostsFSPViewModels
{
    public class PostsIndexViewModel
    {
        public IEnumerable<Post> Posts { get; set; }
        public PostsPageViewModel PageViewModel { get; set; }
        public PostFilterViewModel FilterViewModel { get; set; }
        public PostsSortViewModel SortViewModel { get; set; }
        public Post HelpNamePost {get; set;}
    }
}
