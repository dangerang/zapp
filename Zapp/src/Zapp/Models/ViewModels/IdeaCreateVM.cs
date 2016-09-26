using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Coddit.Models.ViewModels
{
    public class IdeaCreateVM
    {
        [Required(ErrorMessage = "Required Field")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Required Field")]
        public string Description { get; set; }

        //[Required(ErrorMessage = "You must have at least one tag")]
        public string[] Tags { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile[] Uploads { get; set; }

        public bool IsUpvoted { get; set; }

    }
}
