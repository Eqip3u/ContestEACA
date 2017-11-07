using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContestEACA.Models.ManageViewModels
{
    public class ModerateWorkViewModel
    {
        public IEnumerable<Post> Posts { get; set; }
        public Post HelperPost { get; set; }
    }
}
