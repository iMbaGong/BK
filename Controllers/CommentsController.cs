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
using BookingRoom.Dto;
using BookingRoom.Models;

namespace BookingRoom.Controllers
{
    [RoutePrefix("api/comments")]
    public class CommentsController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();

        // GET: api/Comments
        [Route("all")]
        [HttpGet]
        [ResponseType(typeof(List<CommentDto>))]
        public IHttpActionResult AllComment()
        {
            var dtos = new List<CommentDto>();
            foreach (var comment in db.Comment)
            {
                dtos.Add(new CommentDto(comment));
            }
            return Ok(dtos);
        }

        // GET: api/Comments/5
        [ResponseType(typeof(Comment))]
        public IHttpActionResult GetComment(decimal id)
        {
            Comment comment = db.Comment.Find(id);
            if (comment == null)
            {
                return NotFound();
            }

            return Ok(comment);
        }

        [Route("add")]
        [HttpPost]
        public IHttpActionResult AddComment(Comment comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (db.Order.Count(e => e.order_id == comment.order_id) == 0)
                return NotFound();

            comment.create_time = DateTime.Now;

            db.Comment.Add(comment);
            db.SaveChanges();
            return Ok();
        }

        [HttpPost]
        [Route("changestatus/{order_id}")]
        public IHttpActionResult ChangeCommentStatus(decimal order_id)
        {
            Comment comment = db.Comment.Find(order_id);
            if (comment == null)
            {
                return NotFound();
            }
            comment.comment_status = 1 - comment.comment_status;
            db.SaveChanges();

            return Ok();
        }

        // DELETE: api/Comments/5
        [ResponseType(typeof(Comment))]
        public IHttpActionResult DeleteComment(decimal id)
        {
            Comment comment = db.Comment.Find(id);
            if (comment == null)
            {
                return NotFound();
            }

            db.Comment.Remove(comment);
            db.SaveChanges();

            return Ok(comment);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CommentExists(decimal id)
        {
            return db.Comment.Count(e => e.order_id == id) > 0;
        }
    }
}