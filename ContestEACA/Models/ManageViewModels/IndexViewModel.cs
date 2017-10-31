using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ContestEACA.Models.ManageViewModels
{
    public class IndexViewModel
    {
        [Display(Name = "Имя")]
        public string FirstName { get; set; }

        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Почта")]
        public string Email { get; set; }

        [Phone]
        [Display(Name = "Номер телефона")]
        public string PhoneNumber { get; set; }

        public string StatusMessage { get; set; }
    }
}
