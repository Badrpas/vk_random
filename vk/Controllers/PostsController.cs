using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
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
            
            var vkPosts = PostsService.GetPostsFromDB();
            sw.Stop();
            logger.Info("Get request / from [" + HttpContext.Current.Request.UserHostAddress + "]. Execution time: " + sw.ElapsedMilliseconds);
            return Json(vkPosts);
            
        }

        public IHttpActionResult Get(string id)
        {
            Stopwatch sw = Stopwatch.StartNew();
            VkPost post = PostsService.GetRandomPostFromDB();
            sw.Stop();
            logger.Info("Get request /random from [" + HttpContext.Current.Request.UserHostAddress + "]. Execution time: " + sw.ElapsedMilliseconds);
            return Json(post);
        }
    }
}
