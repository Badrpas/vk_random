using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Diagnostics;
using vk.Models;
using vk.Services;

namespace vk.Controllers
{
    //[EnableCors(origins: "https://api.vk.com/method/users.get", headers: "*", methods: "*")]
    public class PostsController : ApiController
    {
        //private VkPostContext db = new VkPostContext();
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public IHttpActionResult Get()
        {
            Stopwatch sw = Stopwatch.StartNew();
            //VkPost[] posts = new VkPost[]{ 
            //    new VkPost { Id = 1, Date = new DateTime(2007, 1, 31, 12, 37, 0), Likes = 9000, OwnerId = 1, PostId = 45639, Reposts = 43, Text = "baba musya approovit"},
            //    new VkPost { Id = 2, Date = new DateTime(2007, 1, 31, 12, 39, 0), Likes = 9000, OwnerId = 1, PostId = 45639, Reposts = 3, Text = "Ряльно"},
            //    PostsService.GetRandomPostFromDB()
            //};
            //var vkPosts = new List<VkPost>();
            //var count = 10;
            //using (var context = new VkPostContext())
            //    if (context.VkPosts.Count() < 10)
            //        count = context.VkPosts.Count();
            
            //for (int i = 0; i < count; i++)
            //{
            //    vkPosts.Add(PostsService.GetRandomPostFromDB());
            //}
            var vkPosts = PostsService.GetPostsFromDB();
            sw.Stop();
            logger.Info("Get request /. Execution time: " + sw.ElapsedMilliseconds);
            return Json(vkPosts);
            
        }

        public IHttpActionResult Get(string id)
        {
            Stopwatch sw = Stopwatch.StartNew();
            VkPost post = PostsService.GetRandomPostFromDB();
            sw.Stop();
            logger.Info("Get request /random. Execution time: " + sw.ElapsedMilliseconds);
            return Json(post);
        }
    }
}
