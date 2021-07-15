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
    [RoutePrefix("api/orders")]
    public class OrdersController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();

        [Route("add")]
        [HttpPost]
        public IHttpActionResult AddOrder(Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (db.Homestay.Count(e => e.homestay_id == order.homestay_id) == 0 || db.Tenant.Count(e => e.tenant_id == order.tenant_id) == 0)
            {
                return NotFound();
            }

            order.create_time = DateTime.Now;

            order.order_id = db.Database.SqlQuery<decimal>("SELECT ORDER_ID_SEQ.NEXTVAL FROM dual").First();

            db.Order.Add(order);

            db.SaveChanges();

            return Ok();
        }

        // GET: api/Orders/5
        [Route("{id}")]
        [ResponseType(typeof(OrderDto))]
        public IHttpActionResult GetOrder(decimal id)
        {
            Order order = db.Order.Find(id);
            if (order == null)
            {
                return NotFound();
            }

            return Ok(new OrderDto(order));
        }

        [Route("user/{user_name}")]
        [ResponseType(typeof(List<OrderDto>))]
        public IHttpActionResult GetOrderByUser(string user_name)
        {
            IQueryable<User> res = GetUsersByName(user_name);
            if (res.Count() != 1)
                return NotFound();
            User res_user = res.First();

            var dtos = new List<OrderDto>();
            foreach (var order in res_user.Customer.Tenant.Order)
            {
                dtos.Add(new OrderDto(order));
            }

            return Ok(dtos);
        }

        [HttpPost]
        [Route("pay/{order_id}")]
        public IHttpActionResult Pay(decimal order_id)
        {
            Order order = db.Order.Find(order_id);
            if (order == null)
            {
                return NotFound();
            }
            order.paying_status = 1;
            db.SaveChanges();
            return Ok();
        }

        // DELETE: api/Orders/5
        [Route("{id}")]
        [HttpDelete]
        [ResponseType(typeof(Order))]
        public IHttpActionResult DeleteOrder(decimal id)
        {
            Order order = db.Order.Find(id);
            if (order == null)
            {
                return NotFound();
            }

            db.Order.Remove(order);
            db.SaveChanges();

            return Ok(order);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
            
        }

        private bool OrderExists(decimal id)
        {
            return db.Order.Count(e => e.order_id == id) > 0;
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