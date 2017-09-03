using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _200_OK.Data.Entity
{
   public class Likes
    {
       [Key]
       public string LikeID { get; set; }
       public string To { get; set; }
       public Users From { get; set; }
    }
}
