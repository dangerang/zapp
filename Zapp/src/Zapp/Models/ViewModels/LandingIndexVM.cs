using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coddit.Models.ViewModels
{
    public class LandingIndexVM
    {
        public LandingIndexVM()
        {
            Login = new LandingLoginVM();
            Register = new LandingRegisterVM();
        }

        public LandingLoginVM Login { get; set; }
        public LandingRegisterVM Register { get; set; }
    }
}
