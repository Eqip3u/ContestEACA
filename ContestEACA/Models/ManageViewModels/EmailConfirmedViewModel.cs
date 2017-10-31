using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ContestEACA.Models.ManageViewModels
{
    public class EmailConfirmedViewModel
    {
        [Display(Name = "Почта")]
        public string Email { get; set; }

        public bool IsEmailConfirmed { get; set; }

        public string StatusMessage { get; set; }
    }
}
