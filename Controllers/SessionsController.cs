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
    [RoutePrefix("api/sessions")]
    public class SessionsController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();


        [HttpPost]
        [Route("send")]
        public IHttpActionResult SendSession(Session session)
        {
            
            if (session.Sender == null || session.Sender.user_name == null || session.Receiver == null || session.Receiver.user_name == null)
                return BadRequest();
            if (GetUsersByName(session.Sender.user_name).Count() == 0 || GetUsersByName(session.Receiver.user_name).Count() == 0)
                return NotFound();

            User sender = GetUsersByName(session.Sender.user_name).First();
            User receiver = GetUsersByName(session.Receiver.user_name).First();
            if (sender.user_id == receiver.user_id)
                return Conflict();

            session.Sender = sender;
            session.Receiver = receiver;

            session.sender_id = sender.user_id;
            session.receiver_id = receiver.user_id;

            session.create_time = DateTime.Now;

            session.session_id= db.Database.SqlQuery<decimal>("SELECT SESSION_ID_SEQ.NEXTVAL FROM dual").First();

            db.Session.Add(session);

            db.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("recv/{user_name}")]
        [ResponseType(typeof(List<SessionDto>))]
        public IHttpActionResult GetSendSession(string user_name)
        {
            if (GetUsersByName(user_name).Count() == 0)
                return NotFound();

            var dtos = new List<SessionDto>();
            foreach(var session in GetUsersByName(user_name).First().SessionRecv)
            {
                dtos.Add(new SessionDto(session));    
            }

            return Ok(dtos);
        }

        [HttpGet]
        [Route("send/{user_name}")]
        [ResponseType(typeof(List<SessionDto>))]
        public IHttpActionResult GetRecvSession(string user_name)
        {
            if (GetUsersByName(user_name).Count() == 0)
                return NotFound();

            var dtos = new List<SessionDto>();
            foreach (var session in GetUsersByName(user_name).First().SessionSend)
            {
                    dtos.Add(new SessionDto(session));
            }

            return Ok(dtos);
        }

        [HttpGet]
        [Route("readed/{session_id}")]
        public IHttpActionResult ReadedSession(decimal session_id)
        {
            if (!SessionExists(session_id))
                return NotFound();

            Session session = db.Session.Find(session_id);
            session.session_status = 1;
            db.Entry(session).State = EntityState.Modified;
            db.SaveChanges();
            return Ok();
        }

        // DELETE: api/Sessions/5
        [ResponseType(typeof(Session))]
        public IHttpActionResult DeleteSession(decimal id)
        {
            Session session = db.Session.Find(id);
            if (session == null)
            {
                return NotFound();
            }

            db.Session.Remove(session);
            db.SaveChanges();

            return Ok(session);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SessionExists(decimal id)
        {
            return db.Session.Count(e => e.session_id == id) > 0;
        }
        private IQueryable<User> GetUsersByName(string kw)
        {
            return db.User
                .Where(u => u.user_name == kw ||
                u.Customer.customer_email == kw ||
                u.Customer.customer_phone == kw);
        }
    }
}