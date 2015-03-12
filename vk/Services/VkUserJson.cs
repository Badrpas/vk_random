using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace vk.Services
{
    public class VkUserJson
    {
        public VkUserJsonResponse[] response { get; set; }
    }

    public class VkUserJsonResponse
    {
        public int id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string photo_100 { get; set; }
        public int hidden { get; set; }
    }
}