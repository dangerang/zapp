using System;
using System.Collections.Generic;

namespace Coddit.Models.Entities
{
    public partial class Skill
    {
        public Skill()
        {
            AccountSkill = new HashSet<AccountSkill>();
        }

        public int SkillId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<AccountSkill> AccountSkill { get; set; }
    }
}
