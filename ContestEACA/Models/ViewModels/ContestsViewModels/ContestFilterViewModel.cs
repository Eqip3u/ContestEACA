using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContestEACA.Models.ViewModels.PostsFSPViewModels;

namespace ContestEACA.Models.ViewModels
{
    public class ContestFilterViewModel
    {
        public ContestFilterViewModel(List<Contest> contests, int? contest)
        {
            contests.Insert(0, new Contest { Name = "Все", Id = 0 });
            Contests = new SelectList(contests, "Id", "Name", contest);
            SelectedContests = contest;
        }
        public SelectList Contests { get; private set; }
        public int? SelectedContests{ get; private set; }

        public static implicit operator ContestFilterViewModel(PostFilterViewModel v)
        {
            throw new NotImplementedException();
        }
    }
}
