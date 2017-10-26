using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ContestEACA.Models.AccountViewModels
{
    public class ExternalLoginViewModel
    {
        [Required]
        [Display(Name = "�����")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "���")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "�������")]
        public string LastName { get; set; }
    }
}
