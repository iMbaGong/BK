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
using BookingRoom.Models;
using BookingRoom.Dto;

namespace BookingRoom.Controllers
{
    [RoutePrefix("api/homestays")]
    public class HomestaysController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();

        [Route("all/{page_count}/{page_num}")]
        [ResponseType(typeof(List<HomestayDto>))]
        // GET: api/Homestays
        public IHttpActionResult GetHomestay(int page_count,int page_num)
        {
            var homestays = db.Homestay.Skip((page_count-1)*page_num).Take(page_num).ToList();
            var dtos = new List<HomestayDto>();
            foreach(var homestay in homestays)
            {
                dtos.Add(new HomestayDto(homestay));
            }
            return Ok(dtos);
        }

        // GET: api/Homestays/5
        [ResponseType(typeof(Homestay))]
        public IHttpActionResult GetHomestay(decimal id)
        {
            Homestay homestay = db.Homestay.Find(id);
            if (homestay == null)
            {
                return NotFound();
            }

            return Ok(homestay);
        }

        // PUT: api/Homestays/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutHomestay(decimal id, Homestay homestay)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != homestay.homestay_id)
            {
                return BadRequest();
            }

            db.Entry(homestay).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HomestayExists(id))
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

        // POST: api/Homestays
        [ResponseType(typeof(Homestay))]
        public IHttpActionResult PostHomestay(Homestay homestay)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Homestay.Add(homestay);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (HomestayExists(homestay.homestay_id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = homestay.homestay_id }, homestay);
        }

        // DELETE: api/Homestays/5
        [ResponseType(typeof(Homestay))]
        public IHttpActionResult DeleteHomestay(decimal id)
        {
            Homestay homestay = db.Homestay.Find(id);
            if (homestay == null)
            {
                return NotFound();
            }

            db.Homestay.Remove(homestay);
            db.SaveChanges();

            return Ok(homestay);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool HomestayExists(decimal id)
        {
            return db.Homestay.Count(e => e.homestay_id == id) > 0;
        }
    }
}