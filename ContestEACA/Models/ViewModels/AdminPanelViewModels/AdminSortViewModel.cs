using ContestEACA.Models.EnumHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContestEACA.Models.ViewModels
{
    public class SFPSortViewModel
    {
        public SortState RatingSort { get; private set; } 
        public SortState DateSort { get; private set; }
        public SortState NominationSort { get; private set; }
        public SortState Status { get; set; }
        public SortState Current { get; private set; } 

        public SFPSortViewModel(SortState sortOrder)
        {
            RatingSort = sortOrder == SortState.RatingAsc ? SortState.RatingDesc : SortState.RatingAsc;
            DateSort = sortOrder == SortState.DateCreateAsc ? SortState.DateCreateDesc : SortState.DateCreateAsc;
            NominationSort = sortOrder == SortState.NominationAsc ? SortState.NominationDesc : SortState.NominationAsc;

            Current = sortOrder;
        }
    }
}
