using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContestEACA.Models.ViewModels
{
    public class AdminIndexViewModel
    {
        public IEnumerable<Post> Posts { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public AdminFilterViewModel FilterViewModel { get; set; }
        public AdminSortViewModel SortViewModel { get; set; }
        public Post HelpNamePost { get; set; }
    }
}
