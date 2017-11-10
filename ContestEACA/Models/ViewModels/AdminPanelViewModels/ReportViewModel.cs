using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContestEACA.Models.ViewModels.AdminPanelViewModels
{
    public class ReportViewModel
    {
        public IEnumerable<Post> Posts { get; set; }
        public SelectList Contests { get; set; }
        public string Nomination { get; set; }
    }
}
