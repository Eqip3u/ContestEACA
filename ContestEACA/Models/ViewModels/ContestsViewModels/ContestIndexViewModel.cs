using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContestEACA.Models.ViewModels
{
    public class ContestIndexViewModel
    {
        public IEnumerable<Post> Posts { get; set; }
        public ContestPageViewModel PageViewModel { get; set; }
        public ContestFilterViewModel FilterViewModel { get; set; }
        public ContestSortViewModel SortViewModel { get; set; }
        public Post HelpNamePost { get; set; }
    }
}
