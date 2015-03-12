using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using System.Diagnostics;
using vk.Models;
using Newtonsoft.Json;


namespace vk.Services
{
    public class PostsService
    {
        //private static VkPostContext db = new VkPostContext();
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private static Random random = new Random();

        private static Thread thread = new Thread(FetchRandomPostFromVk);
        private static object _object = new object();

        private static bool _fetching = false;
        public static bool Fetching { get { return _fetching; } set { _fetching = value; } }

        public static void StartFetchingVk()
        {
            if (! Fetching)
            lock (_object) // TODO: разобраться как это работает - в последний раз этот лок ничем не помог
            {
                if (thread.IsAlive)
                {
                    logger.Info("Fetching thread already started");
                }
                else
                {
                    try
                    {
                        logger.Info("Start fetching vk posts");
                        Fetching = true;
                        thread.Start();
                        logger.Info("Fetching started");
                    }
                    catch (Exception e) {
                        logger.Error(e);
                    }

                }
            }
        }

        private static void FetchRandomPostFromVk()
        {
            while (Fetching)
            {
                var ownerId = random.Next(222000000);
                //logger.Info("Fetching vk post (id"+ownerId+")");
                var posts = FetchPostsFromVk(ownerId);
                if (posts != null)
                {
                    using (var context = new VkDBContext())
                    {
                        foreach (var post in posts)
                        {
                            logger.Info("We got something from id" + ownerId + ": " + post.Text);
                            context.VkPosts.Add(post);
                        }
                        context.SaveChanges();
                    }
                }

                Thread.Sleep(200);
            }
        }

        // Возвращает случайный пост из базы данных
        public static VkPost GetRandomPostFromDB()
        {
            //var posts = db.VkPosts.ToList();
            using (var context = new VkDBContext())
            {
                if (context.VkPosts.Any()) // Есть ли одна или более записей
                {
                    int randomNumber = random.Next(context.VkPosts.Count()) + 1;
                    return context.VkPosts.Where(p => p.Id == randomNumber).FirstOrDefault();
                }
                else
                    return null;
            }
        }
        public static List<VkPost> GetPostsFromDB()
        {
            //var posts = db.VkPosts.ToList();
            var posts = new List<VkPost>();
            using (var context = new VkDBContext())
            {
                if (context.VkPosts.Any()) // Есть ли одна или более записей
                {
                    var count = 10;
                    if (context.VkPosts.Count() < 10)
                        count = context.VkPosts.Count();

                    var randomPosts = context.VkPosts.Take(count);
                    posts = randomPosts.ToList();
                    var res = context.Database.ExecuteSqlCommand("DELETE TOP (" + count + ") FROM [dbo].[VkPosts]");
                    
                    logger.Info(res);

                    //context.SaveChanges();
                }
            }
            return posts;
        }


        private static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
        //
        // Returns last post from given vk.com user (ownerId)
        // 
        private static List<VkPost> FetchPostsFromVk(int ownerId)
        {
            var count = 1;
            var addr = "https://api.vk.com/method/wall.get?owner_id="+ownerId+"&count="+count+"&v=5.28&filter=owner";
            var responseJson = JsonToClass._download_serialized_json_data<VkPostJson>(addr);
            
            if (responseJson.response == null)
                return null;
            if (responseJson.response.count == 0)
                return null;

            var posts = new List<VkPost>();

            foreach (var postJson in responseJson.response.items)
            {
                var legitPost = true;

                if (postJson.post_source != null)
                {
                    if ((postJson.post_source.type.CompareTo("widget") == 0)
                    ||  (postJson.post_source.type.CompareTo("api") == 0)
                    ) continue;
                }
                else
                    if ((postJson.text.Length < 16)                    // Отсеиваем резальтаты по длинне
                    || (postJson.text.Contains("http"))                // и спаму из приложений 
                    || (postJson.text.Contains("vk.com"))              // ручным способом т.к. для 
                    || (postJson.text.Contains("vkontakte.ru"))        // post_source нужна авторизация
                    || (postJson.text.Contains("зайди по ссылке"))
                    || (postJson.text.Contains("фамил"))
                    || (postJson.text.Contains("посещ") && postJson.text.Contains("страницу"))
                    || (postJson.text.Contains("уров"))
                    ) continue;
                //logger.Info(JsonConvert.SerializeObject(postJson));

                if (!legitPost)
                    continue;

                posts.Add(
                    new VkPost
                    {
                        Date = UnixTimeStampToDateTime(postJson.date),
                        Likes = postJson.likes.count,
                        OwnerId = postJson.owner_id,
                        PostId = postJson.id,
                        Reposts = postJson.reposts.count,
                        Text = postJson.text
                    });
            }

            return posts;
        }
    }
}

