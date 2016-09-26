using Coddit.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coddit.Models.ViewModels
{
    public class AccountProfileVM
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }

        public string ProfilePic { get; set; }

        public string Description { get; set; }

        public string[] Tags { get; set; }

        public bool IsProgrammer { get; set; }

        public AccountProfileIdeaVM[] MyIdeas { get; set; }

    }
}
