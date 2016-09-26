using System;
using System.Collections.Generic;

namespace Coddit.Models.Entities
{
    public partial class TagIdea
    {
        public int TagId { get; set; }
        public int IdeaId { get; set; }

        public virtual Idea Idea { get; set; }
        public virtual Tag Tag { get; set; }
    }
}
