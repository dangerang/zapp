using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coddit.Models.ViewModels
{
    public class CommentListVM
    {
        public static int IdeaId { get; set; }
        public int CommentId { get; set; }
        public string Username { get; set; }
        public string Text { get; set; }
        public bool IsCreator { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
