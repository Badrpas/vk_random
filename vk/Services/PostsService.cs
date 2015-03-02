using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using vk.Models;
using Newtonsoft.Json;


namespace vk.Services
{
    public class PostsService
    {
        private static VkPostContext db = new VkPostContext();
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
                var posts = GetPostFromVk(ownerId);
                if (posts != null)
                {
                    foreach (var post in posts)
                    {
                        logger.Info("We got something from id" + ownerId + ": " + post.Text);
                        db.VkPosts.Add(post);
                    }
                    db.SaveChanges();
                }

                Thread.Sleep(200);
            }
        }

        // Возвращает случайный пост из базы данных
        public static VkPost GetRandomPostFromDB()
        {
            //var posts = db.VkPosts.ToList();
            if (db.VkPosts.Any()) // Есть ли одна или более записей
            {
                int randomNumber = random.Next(db.VkPosts.Count()) + 1;
                return db.VkPosts.Where(p => p.Id == randomNumber).FirstOrDefault();
            }
            else
                return null;
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
        private static List<VkPost> GetPostFromVk(int ownerId)
        {
            var addr = "https://api.vk.com/method/wall.get?owner_id="+ownerId+"&count=10&v=5.28&filter=owner";
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
                    ) legitPost = false;
                }
                // Отсеиваем резальтаты по длинне
                // и спаму из приложений (ссылки в основном только из них в тексте попадаются)
                else
                    if ((postJson.text.Length < 16)                     // Отсеиваем резальтаты по длинне
                    ||  (postJson.text.Contains("http"))                // и спаму из приложений 
                    ||  (postJson.text.Contains("vk.com"))              // ручным способом т.к. для 
                    ||  (postJson.text.Contains("vkontakte.ru"))        // post_source нужна авторизация
                    ||  (postJson.text.Contains("зайди по ссылке"))
                    ||  (postJson.text.Contains("фамили"))
                    ||  (postJson.text.Contains("посещ") && postJson.text.Contains("страницу"))
                    )   legitPost = false;
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

