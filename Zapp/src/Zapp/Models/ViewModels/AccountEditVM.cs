using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Coddit.Models.ViewModels
{
    public class AccountEditVM
    {
        public string Email { get; set; }
        public string Description { get; set; }

        [Display(Name = "Trust me, I'm a programmer.")]
        public bool IsProgrammer { get; set; }
        public string[] Tags { get; set; }
    }
}
