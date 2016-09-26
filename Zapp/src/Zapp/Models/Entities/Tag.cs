using System;
using System.Collections.Generic;

namespace Coddit.Models.Entities
{
    public partial class Tag
    {
        public Tag()
        {
            TagIdea = new HashSet<TagIdea>();
        }

        public int TagId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<TagIdea> TagIdea { get; set; }
    }
}
