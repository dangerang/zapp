using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coddit.Models.ViewModels
{
    public class IdeasIndexVM
    {
        public IdeasIndexVM()
        {
            LandingProjectModel = new LandingProjectVM();
        }

        public IdeaListVM[] IdeaListModel { get; set; }
        //public IdeaListVM[] PopularIdeaListModel { get; set; }
        public LandingProjectVM LandingProjectModel { get; set; }
    }
}
