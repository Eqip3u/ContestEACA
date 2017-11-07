using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContestEACA.Models.ViewModels
{
    public class SFPIndexViewModel
    {
        public IEnumerable<Post> Posts { get; set; }
        public SFPPageViewModel PageViewModel { get; set; }
        public SFPFilterViewModel FilterViewModel { get; set; }
        public SFPSortViewModel SortViewModel { get; set; }
        public Post HelpNamePost { get; set; }
    }
}
