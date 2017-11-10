using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContestEACA.Models
{
    public class ModerateUserContest
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int ContestId { get; set; }
        public Contest Contest { get; set; }
    }
}
