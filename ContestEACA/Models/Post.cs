using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ContestEACA.Models
{
    public class Post
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public string Title { get; set; }

        public string Work { get; set; }

        public string File { get; set; }

        public int Rating { get; set; }

        public virtual ICollection<Like> Likes { get; set; }

        public string Author { get; set; }
    }
}
