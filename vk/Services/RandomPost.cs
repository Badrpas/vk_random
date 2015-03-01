using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using vk.Models;
using Newtonsoft.Json;


namespace vk.Services
{
    public class PostsService
    {
        private static VkPostContext db = new VkPostContext();

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public static VkPost GetRandomPost()
        {
            var posts = db.VkPosts.ToList();
            VkPost post = posts[0];

            return post;
        }
        private static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
        private static VkPost GetPostFromVk(int ownerId)
        {
            var addr = "https://api.vk.com/method/wall.get?owner_id="+ownerId+"&count=1&v=5.28";
            var responseJson = JsonToClass._download_serialized_json_data<VkPostJson>(addr);
            if (responseJson.response.count == 0)
                return null;
            var postJson = responseJson.response.items[0];
            logger.Info(JsonConvert.SerializeObject(postJson));
            VkPost post = new VkPost { 
                Id = 2,
                Date = UnixTimeStampToDateTime(postJson.date),
                Likes = postJson.likes.count, 
                OwnerId = postJson.owner_id, 
                PostId = postJson.id, 
                Reposts = postJson.reposts.count, 
                Text = postJson.text 
            };

            return post;
        }
    }
}

