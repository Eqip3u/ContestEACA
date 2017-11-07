using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContestEACA.Models.ViewModels
{
    public class ContestViewModel
    {
        public IEnumerable<Contest> Contests { get; set; }
        public int CountWorkUsers { get; set; }
    }
}
