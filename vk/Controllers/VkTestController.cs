using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using vk.Models;

namespace vk.Controllers
{
    public class VkTestController : ApiController
    {
        private VkPostContext db = new VkPostContext();

        // GET api/VkTest
        public IHttpActionResult GetVkPosts()
        {
            return Json(db.VkPosts);
        }

        // GET api/VkTest/5
        [ResponseType(typeof(VkPost))]
        public IHttpActionResult GetVkPost(long id)
        {
            VkPost vkpost = db.VkPosts.Find(id);
            if (vkpost == null)
            {
                return NotFound();
            }

            return Ok(vkpost);
        }

        // PUT api/VkTest/5
        public IHttpActionResult PutVkPost(long id, VkPost vkpost)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != vkpost.Id)
            {
                return BadRequest();
            }

            db.Entry(vkpost).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VkPostExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST api/VkTest
        [ResponseType(typeof(VkPost))]
        public IHttpActionResult PostVkPost(VkPost vkpost)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.VkPosts.Add(vkpost);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = vkpost.Id }, vkpost);
        }

        // DELETE api/VkTest/5
        [ResponseType(typeof(VkPost))]
        public IHttpActionResult DeleteVkPost(long id)
        {
            VkPost vkpost = db.VkPosts.Find(id);
            if (vkpost == null)
            {
                return NotFound();
            }

            db.VkPosts.Remove(vkpost);
            db.SaveChanges();

            return Ok(vkpost);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool VkPostExists(long id)
        {
            return db.VkPosts.Count(e => e.Id == id) > 0;
        }
    }
}