using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using vk.Models;

namespace vk.Controllers
{
    public class BombomController : ApiController
    {
        Entity[] entities = new Entity[] 
        { 
            new Entity { Name = "Tomato Soup", Number = 1 }, 
            new Entity { Name = "Yo-yo", Number = 3 }, 
            new Entity { Name = "Hammer", Number = 6 } 
        };

        public IHttpActionResult Get()
        {
            return Json(entities);
        }

        public IHttpActionResult GetAll(int id)
        {
            var product = entities.FirstOrDefault((p) => p.Number == id);
            if (product == null)
            {
                return NotFound();
            }
            return Json(product);
        }

    }
}
// EDIT > Paste Special > Paste JSON as classes