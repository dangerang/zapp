using System;
using System.Collections.Generic;

namespace Coddit.Models.Entities
{
    public partial class Rating
    {
        public int RatingId { get; set; }
        public int AccountId { get; set; }
        public int IdeaId { get; set; }

        public virtual Account Account { get; set; }
        public virtual Idea Idea { get; set; }
    }
}
