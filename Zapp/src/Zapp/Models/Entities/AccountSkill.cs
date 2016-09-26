using System;
using System.Collections.Generic;

namespace Coddit.Models.Entities
{
    public partial class AccountSkill
    {
        public int SkillId { get; set; }
        public int AccountId { get; set; }

        public virtual Account Account { get; set; }
        public virtual Skill Skill { get; set; }
    }
}
