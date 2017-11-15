using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContestEACA.Models
{
    public enum StatusPost
    {
        [Display(Name = "Принято")]
        Accept,

        [Display(Name = "Отказано")]
        Denied,

        [Display(Name = "В ожиданий модерации")]
        AwaitingForModeration,
    }

    public class Post
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Display(Name = "Название")]
        public string Title { get; set; }

        [Display(Name = "Текст работы")]
        public string TextWork { get; set; }

        [Display(Name = "Ссылка на видео")]
        public string LinkWork { get; set; }

        [Display(Name = "Рейтинг")]
        public int Rating { get; set; }

        [Display(Name = "Дата создания")]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Дата изменения")]
        public DateTime DateModified { get; set; }

        [Display(Name = "Статус")]
        public StatusPost Status { get; set; }

        [Display(Name = "Фото участников проекта")]
        public int? CoverId { get; set; }
        public FileModel Cover { get; set; }

        [Display(Name = "Файл")]
        public int? FileId { get; set; }
        public FileModel File { get; set; }

        [Display(Name = "Автор")]
        public string AuthorId { get; set; }
        public virtual ApplicationUser Author { get; set; }

        [Display(Name = "Номинация")]
        public int NominationId { get; set; }
        public virtual Nomination Nomination { get; set; }

        [Display(Name = "Конкурс")]
        public int ContestId { get; set; }
        public virtual Contest Contest { get; set; }

        public virtual ICollection<Like> Likes { get; set; }
    }
}
