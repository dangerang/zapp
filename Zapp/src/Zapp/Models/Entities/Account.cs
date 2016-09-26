using System;
using System.Collections.Generic;

namespace Coddit.Models.Entities
{
    public partial class Account
    {
        public Account()
        {
            AccountIdea = new HashSet<AccountIdea>();
            AccountSkill = new HashSet<AccountSkill>();
            Comment = new HashSet<Comment>();
            Idea = new HashSet<Idea>();
            Rating = new HashSet<Rating>();
        }

        public int AccountId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public bool? IsProgrammer { get; set; }
        public string Description { get; set; }
        public string ImageSrc { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<AccountIdea> AccountIdea { get; set; }
        public virtual ICollection<AccountSkill> AccountSkill { get; set; }
        public virtual ICollection<Comment> Comment { get; set; }
        public virtual ICollection<Idea> Idea { get; set; }
        public virtual ICollection<Rating> Rating { get; set; }
    }
}
