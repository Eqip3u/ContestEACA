using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContestEACA.Models.ViewModels
{
    public class SFPFilterViewModel
    {
        public SFPFilterViewModel(List<Contest> contests, int? contest)
        {
            contests.Insert(0, new Contest { Name = "Все", Id = 0 });
            Contests = new SelectList(contests, "Id", "Name", contest);
            SelectedContests = contest;
        }
        public SelectList Contests { get; private set; }
        public int? SelectedContests{ get; private set; }
    }
}
