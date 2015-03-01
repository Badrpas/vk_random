using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace vk.Models
{
    public class VkPost
    {
        public long Id { get; set; }
        public long PostId { get; set; }
        public long OwnerId { get; set; }
        public DateTime Date { get; set; }
        public string Text { get; set; }
        public int Likes { get; set; }
        public int Reposts { get; set; }
    }

    public class VkPostContext : DbContext
    {
        public DbSet<VkPost> VkPosts { get; set; }
    }

}


