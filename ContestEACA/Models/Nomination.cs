using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ContestEACA.Models
{
    public class Nomination
    {
        public int Id { get; set; }

        [Display(Name = "Номинация")]
        public string Name { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }


        public virtual ICollection<Post> Posts { get; set; }

        [Display(Name = "Конкурс")]
        public int? ContestId { get; set; }
        public virtual Contest Contest { get; set; }

    }
}