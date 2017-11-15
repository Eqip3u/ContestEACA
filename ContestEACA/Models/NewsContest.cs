using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ContestEACA.Models
{
    public class NewsContest
    {
        public int Id { get; set; }

        [Display(Name = "Название")]
        public string Name { get; set; }

        [Display(Name = "Текст")]
        public string Text { get; set; }

        [Display(Name = "Ссылка куда-нибудь")]
        public string Link { get; set; }

        [Display(Name = "Фото")]
        public int? PhotoId { get; set; }
        public FileModel Photo { get; set; }

        [Display(Name = "Дата создания")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime DateCreated { get; set; }

        public int ContestId { get; set; }
        public Contest Contest { get; set; }
    }
}
