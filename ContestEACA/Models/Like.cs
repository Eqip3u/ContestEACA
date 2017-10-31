using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ContestEACA.Models
{
    public class Like
    {
        public int Id { get; set; }

        public string UserId { get; set; }


        public int PostId { get; set; }

        [ForeignKey("PostId")]
        public virtual Post Post { get; set; }
    }
}
