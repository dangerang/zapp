using System;
using System.Collections.Generic;

namespace Coddit.Models.Entities
{
    public partial class Idea
    {
        public Idea()
        {
            AccountIdea = new HashSet<AccountIdea>();
            Comment = new HashSet<Comment>();
            Rating = new HashSet<Rating>();
            TagIdea = new HashSet<TagIdea>();
        }

        public int IdeaId { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public int CreatorId { get; set; }
        public string Uploads { get; set; }
        public string City { get; set; }
        public string GitHub { get; set; }
        public DateTime CreatedDate { get; set; }
        //public bool IsEnabled { get; set; }

        public virtual ICollection<AccountIdea> AccountIdea { get; set; }
        public virtual ICollection<Comment> Comment { get; set; }
        public virtual ICollection<Rating> Rating { get; set; }
        public virtual ICollection<TagIdea> TagIdea { get; set; }
        public virtual Account Creator { get; set; }
    }
}
