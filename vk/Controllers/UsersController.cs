using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using vk.Services;
using vk.Models;
using Newtonsoft.Json;

namespace vk.Controllers
{
    
    public class PostRequestJson
    {
        public object[] ids { get; set; }
    }

    public class UsersController : ApiController
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public IHttpActionResult Post(HttpRequestMessage request)
        {
            var jsonString = request.Content.ReadAsStringAsync().Result;
            logger.Info("That what we've got: "+jsonString);
            var requestJson = JsonConvert.DeserializeObject<PostRequestJson>(jsonString);
            var ids = requestJson.ids;
            var _ = "";
            if (ids != null)
            {
                foreach (var item in ids)
                {
                    _ += item;
                }
                logger.Info("received post request. ids: " + _);
            }
            else
            {
                logger.Info("received post request. ids: null");
                return null;
            }
            var _ids = String.Join(",", ids);
            var addr = "https://api.vk.com/method/users.get?user_ids=" + _ids + "&fields=photo_100&v=5.28";
            logger.Info("making Vk request: " + addr);
            var responseJson = JsonToClass._download_serialized_json_data<VkUserJson>(addr);

            var userList = new List<VkUser>();
            foreach (var user in responseJson.response)
            {
                userList.Add(
                    new VkUser
                    {
                        Id = user.id,
                        FirstName = user.first_name,
                        LastName = user.last_name,
                        Photo = user.photo_100
                    });
            }

            return Json(userList);
        }
    }
}
