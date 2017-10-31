using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContestEACA.Models.ViewModels
{
    public class AdminFilterViewModel
    {
        public AdminFilterViewModel(List<Nomination> nominations, int? nomination)
        {
            nominations.Insert(0, new Nomination { Name = "Все", Id = 0 });
            Nominations = new SelectList(nominations, "Id", "Name", nomination);
            SelectedNomination = nomination;
        }
        public SelectList Nominations { get; private set; }
        public int? SelectedNomination{ get; private set; }
    }
}
