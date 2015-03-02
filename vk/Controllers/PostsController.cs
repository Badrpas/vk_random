using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using vk.Models;
using vk.Services;

namespace vk.Controllers
{
    public class PostsController : ApiController
    {
        private VkPostContext db = new VkPostContext();
        public IHttpActionResult Get()
        {
            VkPost[] posts = new VkPost[]{ 
                new VkPost { Id = 1, Date = new DateTime(2007, 1, 31, 12, 37, 0), Likes = 9000, OwnerId = 1, PostId = 45639, Reposts = 43, Text = "baba musya approovit"},
                new VkPost { Id = 2, Date = new DateTime(2007, 1, 31, 12, 39, 0), Likes = 9000, OwnerId = 1, PostId = 45639, Reposts = 3, Text = "Ряльно"},
                PostsService.GetRandomPostFromDB()
            };
            var vkPosts = new List<VkPost>();
            var count = 10;
            if (db.VkPosts.Count() < 10)
                count = db.VkPosts.Count();
            for (int i = 0; i < count; i++)
            {
                vkPosts.Add(PostsService.GetRandomPostFromDB());
            }
            return Json(vkPosts);
        }

        public IHttpActionResult Get(string id)
        {
            VkPost post = PostsService.GetRandomPostFromDB();
            return Json(post);
        }
    }
}
