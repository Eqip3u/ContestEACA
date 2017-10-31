using ContestEACA.Models.EnumHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContestEACA.Models.ViewModels
{
    public class AdminSortViewModel
    {
        public SortState RatingSort { get; private set; } 
        public SortState DateSort { get; private set; }
        public SortState Status { get; set; }
        public SortState Current { get; private set; } 

        public AdminSortViewModel(SortState sortOrder)
        {
            RatingSort = sortOrder == SortState.RatingAsc ? SortState.RatingDesc : SortState.RatingAsc;
            DateSort = sortOrder == SortState.DateCreateAsc ? SortState.DateCreateDesc : SortState.DateCreateAsc;

            Current = sortOrder;
        }
    }
}
