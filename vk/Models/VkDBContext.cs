using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace vk.Models
{
    public class VkDBContext : DbContext
    {
        public DbSet<VkPost> VkPosts { get; set; }
        public DbSet<VkUser> VkUsers { get; set; }
    }
}