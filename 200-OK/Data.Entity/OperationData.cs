using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _200_OK.Data.Entity
{
    public class OperationData
    {
        [Key]
        public string PostID { get; set; }
        public int LikesCount { get; set; }
        public int CommentsCount { get; set; }
        public bool IsFinished { get; set; }
        public Likes LastLike { get; set; }
        public Comment LastComment { get; set; }
    }
}
