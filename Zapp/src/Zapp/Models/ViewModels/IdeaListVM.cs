using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coddit.Models.ViewModels
{
    public class IdeaListVM
    {
        public int Id { get; set; }

        public int Rating { get; set; }

        public int Comment { get; set; }

        public string Subject { get; set; }

        public string Description { get; set; }

        public string[] Tags { get; set; }

        public IdeaMemberVM[] Members { get; set; }

        public bool HasVoted { get; set; }

        public bool IsEnabled { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}