using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


    //{response: {
    //count: 169,
    //items: [{
    //id: 45640,
    //from_id: 1,
    //owner_id: 1,
    //date: 1419967791,
    //post_type: 'post',
    //text: '12 Angry Men 1957 года – один из лучших фильмов в истории кинематографа. Фильм имеет не только культурную, но и историческую ценность, так как демонстрирует пример работы жюри присяжных в США середины XX века. Институт суда присяжных – один из ключевых факторов успеха США как государства.',
    //attachments: [{
    //type: 'video',
    //video: {
    //id: 165040157,
    //owner_id: -33322342,
    //title: '12 разгневанных мужчин / 12 Angry Men 1957',
    //duration: 5781,
    //description: '8.5 IMDb: 8.9 Английские субтитры.
    //Юношу обвиняют в убийстве собственного отца, ему грозит электрический стул. Двенадцать присяжных собираются чтобы вынести вердикт: виновен или нет.С начала заседания почти все склонились к тому, что виновен, и лишь только один из двенадцати позволил себе усомниться. Счет голосов присяжных по принципу «виновен — невиновен» был 11:1. К концу собрания мнения судей кардинально изменились…',
    //date: 1366566132,
    //views: 7723,
    //comments: 938,
    //photo_130: 'https://pp.vk.me/...eo/s_5abf85bc.jpg',
    //photo_320: 'https://pp.vk.me/...eo/l_ebd0523b.jpg',
    //access_key: 'e80cc7aa6fa6c87496'
    //}
    //}],
    //post_source: {
    //type: 'vk'
    //},
    //comments: {
    //count: 0,
    //can_post: 0
    //},
    //likes: {
    //count: 41342,
    //user_likes: 0,
    //can_like: 1,
    //can_publish: 1
    //},
    //reposts: {
    //count: 2303,
    //user_reposted: 0
    //}
    //}]
    //}
    //}

namespace vk.Services
{

    public class VkPostJson
    {
        public Response response { get; set; }
    }

    public class Response
    {
        public int count { get; set; }
        public Item[] items { get; set; }
    }

    public class Item
    {
        public int id { get; set; }
        public int from_id { get; set; }
        public int owner_id { get; set; }
        public int date { get; set; }
        public string post_type { get; set; }
        public string text { get; set; }
        public Attachment[] attachments { get; set; }
        public Post_Source post_source { get; set; }
        public Comments comments { get; set; }
        public Likes likes { get; set; }
        public Reposts reposts { get; set; }
    }

    public class Post_Source
    {
        public string type { get; set; }
    }

    public class Comments
    {
        public int count { get; set; }
        public int can_post { get; set; }
    }

    public class Likes
    {
        public int count { get; set; }
        public int user_likes { get; set; }
        public int can_like { get; set; }
        public int can_publish { get; set; }
    }

    public class Reposts
    {
        public int count { get; set; }
        public int user_reposted { get; set; }
    }

    public class Attachment
    {
        public string type { get; set; }
        public Video video { get; set; }
    }

    public class Video
    {
        public int id { get; set; }
        public int owner_id { get; set; }
        public string title { get; set; }
        public int duration { get; set; }
        public string description { get; set; }
        public int date { get; set; }
        public int views { get; set; }
        public int comments { get; set; }
        public string photo_130 { get; set; }
        public string photo_320 { get; set; }
        public string access_key { get; set; }
    }


}