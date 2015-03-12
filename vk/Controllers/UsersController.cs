using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using vk.Services;
using vk.Models;

namespace vk.Controllers
{
    public class UsersController : ApiController
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public IHttpActionResult Post([FromUri]int[] ids)
        {
            var _ = "";
            foreach (var item in ids)
            {
                _ += item;
            }
            logger.Info(_);
            var _ids = String.Join(" ", ids);
            var addr = "https://api.vk.com/method/users.get?user_ids=" + _ids + "&fields=photo_100&v=5.28";
            var responseJson = JsonToClass._download_serialized_json_data<VkUserJson>(addr);
            var userList = new List<VkUser>();
            foreach (var user in responseJson.response)
            {
                userList.Add(
                    new VkUser{
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
