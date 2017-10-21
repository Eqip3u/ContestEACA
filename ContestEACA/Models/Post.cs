using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContestEACA.Models
{
    public class Post
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public string Author { get; set; }

        public string Title { get; set; }

        public string TextWork { get; set; }

        public string LinkWork { get; set; }

        public string File { get; set; }

        public int Rating { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }

        public int NominationId { get; set; }
        public Nomination Nomination { get; set; }

        public virtual ICollection<Like> Likes { get; set; }


    }
}
