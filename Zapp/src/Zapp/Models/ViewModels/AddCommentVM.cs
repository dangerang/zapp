using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Coddit.Models.ViewModels
{
    public class AddCommentVM
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "You can't submit an empty comment!")]
        public string Text { get; set; }
    }
}
