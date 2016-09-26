using System;
using System.Collections.Generic;

namespace Coddit.Models.Entities
{
    public partial class AccountIdea
    {
        public int IdeaId { get; set; }
        public int AccountId { get; set; }

        public virtual Account Account { get; set; }
        public virtual Idea Idea { get; set; }
    }
}
