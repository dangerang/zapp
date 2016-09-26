using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Coddit.Models.ViewModels
{
    public class IdeaEditVM
    {
        [Required(ErrorMessage = "Required Field")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Required Field")]
        public string Description { get; set; }

        //[Required(ErrorMessage = "You must have at least one tag")]
        //public string Tags { get; set; }

        //[DataType(DataType.Upload)]
        //public IFormFile[] Uploads { get; set; }
    }
}