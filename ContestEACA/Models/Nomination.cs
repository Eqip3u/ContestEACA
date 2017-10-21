using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace ContestEACA.Models
{
    public class Nomination
    {
        public int Id { get; set; }
        
        public string Name { get; set; }

        public virtual ICollection<Post> Posts { get; set; }

    }
}