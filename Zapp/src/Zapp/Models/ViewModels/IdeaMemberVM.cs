using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coddit.Models.ViewModels
{
    public class IdeaMemberVM
    {
        public bool IsCreator { get; set; }
        public string UserName { get; set; }
        public bool IsActive { get; set; }
    }
}
