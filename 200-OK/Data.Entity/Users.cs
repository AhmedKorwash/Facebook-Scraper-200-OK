using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _200_OK.Data.Entity
{
   public class Users
    {
       public Users()
       {
           Likes = new List<Likes>();
           Comments = new List<Comment>();
       }

       [Key]
        public string UserID { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string FullName { get; set; }
        public List<Likes> Likes { get; set; }
        public List<Comment> Comments { get; set; }
        public string Keywords { get; set; }
    }
}
