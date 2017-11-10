using ContestEACA.Models.EnumHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContestEACA.Models.ViewModels
{
    public class ContestSortViewModel
    {
        public ContestSortState RatingSort { get; private set; } 
        public ContestSortState DateSort { get; private set; }
        public ContestSortState NominationSort { get; private set; }
        public ContestSortState Status { get; set; }
        public ContestSortState Current { get; private set; } 

        public ContestSortViewModel(ContestSortState sortOrder)
        {
            RatingSort = sortOrder == ContestSortState.RatingAsc ? ContestSortState.RatingDesc : ContestSortState.RatingAsc;
            DateSort = sortOrder == ContestSortState.DateCreateAsc ? ContestSortState.DateCreateDesc : ContestSortState.DateCreateAsc;
            NominationSort = sortOrder == ContestSortState.NominationAsc ? ContestSortState.NominationDesc : ContestSortState.NominationAsc;

            Current = sortOrder;
        }
    }
}
