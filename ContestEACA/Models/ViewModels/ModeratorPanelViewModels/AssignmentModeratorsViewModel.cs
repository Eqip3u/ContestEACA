using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContestEACA.Models.ViewModels
{
    public class AssignmentModeratorsViewModel
    {
        public int ContestId { get; set; }

        public SelectList Users { get; set; }

        public IEnumerable<ApplicationUser> Moderators { get; set; }
    }
}
