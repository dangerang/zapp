using System;
using System.Collections.Generic;

namespace Coddit.Models.Entities
{
    public partial class Comment
    {
        public int CommentId { get; set; }
        public int AccountId { get; set; }
        public int IdeaId { get; set; }
        public string Text { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Account Account { get; set; }
        public virtual Idea Idea { get; set; }
    }
}
