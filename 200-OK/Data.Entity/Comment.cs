using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _200_OK.Data.Entity
{
   public class Comment
   {
       public Comment()
       {
           Likes = new List<Likes>();
       }
       [Key]
       public string CommentID { get; set; }
       public string PostID { get; set; }
       public string Text { get; set; }
       public List<Likes> Likes { get; set; }
       public Users From { get; set; }
    }
}
