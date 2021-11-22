using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chatApi.Entities
{
    public class UserLike
    {
        public AppUser SourceUser { get; set; }
        //usuario a quien le dio el megusta 
        public int SourceUserId { get; set; }
        public AppUser LikedUser { get; set; }
        //usuario que dio el megusta
        public int LikedUserId { get; set; }
    }
}
