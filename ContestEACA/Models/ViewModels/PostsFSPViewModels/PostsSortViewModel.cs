using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContestEACA.Models.ViewModels.PostsFSPViewModels
{
    public class PostsSortViewModel
    {
        public PostsSortState NameSort { get; private set; }
        public PostsSortState RatingSort { get; private set; }
        public PostsSortState Current { get; private set; }     // текущее значение сортировки

        public PostsSortViewModel(PostsSortState sortOrder)
        {
            NameSort = sortOrder == PostsSortState.NameAsc ? PostsSortState.NameDesc : PostsSortState.NameAsc;
            RatingSort = sortOrder == PostsSortState.RatingAsc ? PostsSortState.RatingDesc : PostsSortState.RatingAsc;
            Current = sortOrder;
        }
    }
}
