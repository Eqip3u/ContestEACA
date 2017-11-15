using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ContestEACA.Models
{
    public enum StatusContest
    {
        Active,
        Done,
        Coming
    }

    public class Contest
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Конкурс")]
        public string Name { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        [Display(Name = "Начало конкурса")]
        public DateTime StartTime { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        [Display(Name = "Конец конкурса")]
        public DateTime EndTime { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        [Display(Name = "Награждение")]
        public DateTime Rewarding { get; set; }

        [Required]
        [Display(Name = "Тайтл на главной")]
        public string PreTitle { get; set; }

        [Required]
        [Display(Name = "Текст на главной")]
        public string PreText { get; set; }


        public bool MainContest { get; set; }


        [Display(Name = "Положение о конкурсе")]
        public int? ProvisionId { get; set; }
        public virtual FileModel Provision { get; set; }

        [Display(Name = "Картинка на главной")]
        public int? PreImageId { get; set; }
        public virtual FileModel PreImage { get; set; }


        [Display(Name = "Дата создания")]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Дата изменения")]
        public DateTime DateModified { get; set; }


        [Display(Name = "Кто создал")]
        public string WhoCreatedId { get; set; }
        public ApplicationUser WhoCreated { get; set; }

        [Display(Name = "Кто внёс последнее изменение")]
        public string WhoModifiedId { get; set; }
        public ApplicationUser WhoModified { get; set; }



        [Display(Name = "Модераторы")]
        public virtual ICollection<ModerateUserContest> Moderators { get; set; }


        [Display(Name = "Новости")]
        public virtual ICollection<NewsContest> News { get; set; }


        [Display(Name = "Работы")]
        public virtual ICollection<Post> Posts { get; set; }

        [Display(Name = "Номинации")]
        public virtual ICollection<Nomination> Nominations { get; set; }


        public StatusContest Status
        {
            get
            {
                if (DateTime.Now >= EndTime)
                    return StatusContest.Done;
                else if (DateTime.Now <= EndTime && DateTime.Now >= StartTime)
                    return StatusContest.Active;
                else
                    return StatusContest.Coming;
            }
        }

    }
}
