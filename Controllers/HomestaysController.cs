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
using BookingRoom.Data;

namespace BookingRoom.Controllers
{
    [RoutePrefix("api/homestays")]
    public class HomestaysController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();

        [HttpGet]
        [Route("{homestay_id}")]
        [ResponseType(typeof(HomestayDto))]
        public IHttpActionResult GetHomestays(decimal homestay_id)
        {
            if (db.Homestay.Count(e => e.homestay_id == homestay_id) == 0)
            {
                return NotFound();
            }

            return Ok(new HomestayDto(db.Homestay.Find(homestay_id)));
        }


        [HttpPost]
        [Route("add")]
        public IHttpActionResult AddHomestays(Homestay homestay)
        {
            //校验字段是否符合约束
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //不能同时属于两种类别
            if (homestay.WholeRoom != null && homestay.SingleRoom.Count > 0)
            {
                return BadRequest();
            }
            //不能同时不属于两种类别
            if(homestay.WholeRoom == null && homestay.SingleRoom.Count == 0)
            {
                return BadRequest();
            }
            //房东不存在
            if (db.User.Count(e => e.user_id == homestay.landlord_id) != 1)
            {
                return NotFound();
            }
            //获取民宿自增主键
            homestay.homestay_id = db.Database.SqlQuery<decimal>("SELECT HOMESTAY_ID_SEQ.NEXTVAL FROM dual").First();
            if (homestay.WholeRoom != null)
            {
                foreach(var room in homestay.WholeRoom.Room)
                {
                    //为每个Room设置主键
                    room.room_id = db.Database.SqlQuery<decimal>("SELECT ROOM_ID_SEQ.NEXTVAL FROM dual").First();
                }
            }
            else
            {
                foreach (var singleroom in homestay.SingleRoom)
                {
                    //为singleroom设置主键
                    singleroom.bed_id = db.Database.SqlQuery<decimal>("SELECT BED_ID_SEQ.NEXTVAL FROM dual").First();
                }
            }
            db.Homestay.Add(homestay);
            db.SaveChanges();

            return Ok();
        }


        [HttpGet]
        [Route("all/{page_count}/{page_num}/{order}")]
        [ResponseType(typeof(List<HomestayDto>))]
        public IHttpActionResult GetHomestays(int page_count,int page_num,string order="grade")
        {
            //获取一页内容
            var homestays = GetPageContent(page_count, page_num, order);
            //转成DTO
            var dtos = new List<HomestayDto>();
            foreach(var homestay in homestays)
            {
                dtos.Add(new HomestayDto(homestay));
            }
            return Ok(dtos);
        }

        [HttpGet]
        [Route("all")]
        [ResponseType(typeof(List<HomestayDto>))]
        public IHttpActionResult GetHomestays()
        {
            //转成DTO
            var dtos = new List<HomestayDto>();
            foreach (var homestay in db.Homestay)
            {
                dtos.Add(new HomestayDto(homestay));
            }
            return Ok(dtos);
        }

        [HttpPost]
        [Route("filter/{page_count}/{page_num}/{order}")]
        [ResponseType(typeof(List<HomestayDto>))]
        public IHttpActionResult HomestaysFilter(HomestayFilter filter, int page_count, int page_num,string order="grade")
        {
            IQueryable<Homestay> res = db.Homestay;

            if (filter.price_max != null)
                res = res.Where(h => h.homestay_price <= filter.price_max);
            if (filter.price_min != null)
                res = res.Where(h => h.homestay_price >= filter.price_min);

            if (filter.cap_max != null)
                res = res.Where(h => h.homestay_cap <= filter.cap_max);
            if (filter.cap_min != null)
                res = res.Where(h => h.homestay_cap >= filter.cap_min);

            if (filter.area_max != null)
                res = res.Where(h => h.homestay_area<= filter.area_max);
            if (filter.area_min != null)
                res = res.Where(h => h.homestay_area >= filter.area_min);

            if (filter.bed_max != null)
                res = res.Where(h => h.bed_number <= filter.bed_max);
            if (filter.bed_min != null)
                res = res.Where(h => h.bed_number >= filter.bed_min);

            if (filter.by_the_street != null)
                res = res.Where(h => h.by_the_street == filter.by_the_street);

            if (filter.wifi != null)
                res = res.Where(h => h.wifi == filter.wifi);

            if (filter.bathtub != null)
                res = res.Where(h => h.bathtub == filter.bathtub);
            
            if (filter.type != null)
            {
                if (filter.type == 0)
                {
                    res = res.Where(h => h.WholeRoom != null);
                    if (filter.room_max != null)
                        res = res.Where(h => h.WholeRoom.room_num < filter.room_max);
                    if (filter.room_min != null)
                        res = res.Where(h => h.WholeRoom.room_num > filter.room_min);
                }
                else
                    res = res.Where(h => h.SingleRoom.Count >= 0);
            }

            var homestays = GetPageContent(page_count, page_num, order);
             
            var dtos = new List<HomestayDto>();
            foreach (var homestay in homestays)
            {
                dtos.Add(new HomestayDto(homestay));
            }
            return Ok(dtos);
        }

        [HttpPost]
        [Route("filter")]
        [ResponseType(typeof(List<HomestayDto>))]
        public IHttpActionResult HomestaysFilter(HomestayFilter filter)
        {
            IQueryable<Homestay> res = db.Homestay;

            if (filter.price_max != null)
                res = res.Where(h => h.homestay_price <= filter.price_max);
            if (filter.price_min != null)
                res = res.Where(h => h.homestay_price >= filter.price_min);

            if (filter.cap_max != null)
                res = res.Where(h => h.homestay_cap <= filter.cap_max);
            if (filter.cap_min != null)
                res = res.Where(h => h.homestay_cap >= filter.cap_min);

            if (filter.area_max != null)
                res = res.Where(h => h.homestay_area <= filter.area_max);
            if (filter.area_min != null)
                res = res.Where(h => h.homestay_area >= filter.area_min);

            if (filter.bed_max != null)
                res = res.Where(h => h.bed_number <= filter.bed_max);
            if (filter.bed_min != null)
                res = res.Where(h => h.bed_number >= filter.bed_min);

            if (filter.by_the_street != null)
                res = res.Where(h => h.by_the_street == filter.by_the_street);

            if (filter.wifi != null)
                res = res.Where(h => h.wifi == filter.wifi);

            if (filter.bathtub != null)
                res = res.Where(h => h.bathtub == filter.bathtub);

            if (filter.type != null)
            {
                if (filter.type == 0)
                {
                    res = res.Where(h => h.WholeRoom != null);
                    if (filter.room_max != null)
                        res = res.Where(h => h.WholeRoom.room_num < filter.room_max);
                    if (filter.room_min != null)
                        res = res.Where(h => h.WholeRoom.room_num > filter.room_min);
                }
                else
                    res = res.Where(h => h.SingleRoom.Count >= 0);
            }

            if (filter.start_time != null&& filter.expire_time != null)
            {
                var hs = new List<Homestay>();
                foreach(var tmp in db.Order.Where(e => e.start_time < filter.start_time || e.expire_time > filter.expire_time))
                {
                    hs.Add(tmp.Homestay);
                }
                var final = res.Where(delegate(Homestay homestay){
                    foreach(var h in hs)
                    {
                        if (homestay.homestay_id == h.homestay_id)
                            return false;
                    }
                    return true;
                });

                var dtos = new List<HomestayDto>();
                foreach (var homestay in res)
                {
                    dtos.Add(new HomestayDto(homestay));
                }
                return Ok(dtos);
            }

            var dtoss = new List<HomestayDto>();
            foreach (var homestay in res)
            {
                dtoss.Add(new HomestayDto(homestay));
            }
            return Ok(dtoss);
        }

        [HttpGet]
        [Route("bylandlord/{user_name}/{page_count}/{page_num}")]
        [ResponseType(typeof(List<HomestayDto>))]
        public IHttpActionResult GetByLandlord(string user_name, int page_count, int page_num)
        {
            if (GetUsersByName(user_name).Count() == 0)
                return NotFound();
            User landlord = GetUsersByName(user_name).First();

            var dtos = new List<HomestayDto>();
            foreach (var homestay in landlord.Customer.Landlord.Homestay.OrderBy(h => -h.grade).Skip((page_count - 1) * page_num).Take(page_num))
            {
                dtos.Add(new HomestayDto(homestay));
            }
            return Ok(dtos);
        }

        [HttpGet]
        [Route("bylandlord/{user_name}")]
        [ResponseType(typeof(List<HomestayDto>))]
        public IHttpActionResult GetByLandlord(string user_name)
        {
            if (GetUsersByName(user_name).Count() == 0)
                return NotFound();
            User landlord = GetUsersByName(user_name).First();

            var dtos = new List<HomestayDto>();
            foreach (var homestay in landlord.Customer.Landlord.Homestay.OrderBy(h => -h.grade))
            {
                dtos.Add(new HomestayDto(homestay));
            }
            return Ok(dtos);
        }

        [HttpPost]
        [Route("update")]
        public IHttpActionResult UpdateHomestay(Homestay homestay)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!HomestayExists(homestay.homestay_id))
            {
                return BadRequest();
            }

            db.Entry(homestay).State = EntityState.Modified;
            db.SaveChanges();
            
            return Ok();
        }

        [HttpPost]
        [Route("changestatus/{homestay_id}/{status}")]
        public IHttpActionResult ChangeHomestayStatus(decimal homestay_id,decimal status)
        {
            
            if (!HomestayExists(homestay_id))
            {
                return BadRequest();
            }
            var homestay = db.Homestay.Find(homestay_id);
            homestay.homestay_status = status;
            db.Entry(homestay).State = EntityState.Modified;
            db.SaveChanges();

            return Ok();
        }


        [HttpGet]
        [Route("addfavor/{user_name}/{homestay_id}")]
        public IHttpActionResult AddFavor(string user_name,int homestay_id)
        {
            if (GetUsersByName(user_name).Count() == 0 || !HomestayExists(homestay_id))
                return NotFound();
            User user = GetUsersByName(user_name).First();
            user.Customer.Tenant.Favorite.Add(db.Homestay.Find(homestay_id));
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("delfavor/{user_name}/{homestay_id}")]
        public IHttpActionResult DelFavor(string user_name, int homestay_id)
        {
            if (GetUsersByName(user_name).Count() == 0 || !HomestayExists(homestay_id))
                return NotFound();
            User user = GetUsersByName(user_name).First();
            user.Customer.Tenant.Favorite.Remove(db.Homestay.Find(homestay_id));
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("favor/{user_name}/{page_count}/{page_num}")]
        public IHttpActionResult GetFavour(string user_name, int page_count, int page_num)
        {
            if (GetUsersByName(user_name).Count() == 0)
                return NotFound();
            User user = GetUsersByName(user_name).First();

            var dtos = new List<HomestayDto>();
            foreach (var homestay in user.Customer.Tenant.Favorite.OrderBy(h => -h.grade).Skip((page_count - 1) * page_num).Take(page_num))
            {
                dtos.Add(new HomestayDto(homestay));
            }
            return Ok(dtos);
        }

        [HttpGet]
        [Route("favor/{user_name}")]
        public IHttpActionResult GetFavour(string user_name)
        {
            if (GetUsersByName(user_name).Count() == 0)
                return NotFound();
            User user = GetUsersByName(user_name).First();

            var dtos = new List<HomestayDto>();
            foreach (var homestay in user.Customer.Tenant.Favorite.OrderBy(h => -h.grade))
            {
                dtos.Add(new HomestayDto(homestay));
            }

            return Ok(dtos);
        }

        // DELETE: api/Homestays/5
        [HttpGet]
        [Route("del/{id}")]
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

        private IQueryable<User> GetUsersByName(string kw)
        {
            return db.User
                .Where(u => u.user_name == kw ||
                u.Customer.customer_email == kw ||
                u.Customer.customer_phone == kw);
        }

        private IQueryable<Homestay> GetPageContent(int page_count, int page_num,string order)
        {
            IQueryable<Homestay> res = db.Homestay;
            switch (order)
            {
                case "price":
                    res = res.OrderBy(h => -h.homestay_price);
                    break;
                case "PRICE":
                    res = res.OrderBy(h => h.homestay_price);
                    break;
                case "grade":
                    res = res.OrderBy(h => -h.grade);
                    break;
                case "Grade":
                    res = res.OrderBy(h => h.grade);
                    break;
                case "favorite":
                    res = res.OrderBy(h => -h.FavoriteBy.Count());
                    break;
                case "FAVORITE":
                    res = res.OrderBy(h => h.FavoriteBy.Count());
                    break;
                default:
                    res = res.OrderBy(h => -h.grade);
                    break;
            }
            return res.Skip((page_count - 1) * page_num).Take(page_num);

        }

        private IQueryable<Homestay> GetAllContent(string order)
        {
            IQueryable<Homestay> res = db.Homestay;
            switch (order)
            {
                case "price":
                    res = res.OrderBy(h => -h.homestay_price);
                    break;
                case "PRICE":
                    res = res.OrderBy(h => h.homestay_price);
                    break;
                case "grade":
                    res = res.OrderBy(h => -h.grade);
                    break;
                case "Grade":
                    res = res.OrderBy(h => h.grade);
                    break;
                case "favorite":
                    res = res.OrderBy(h => -h.FavoriteBy.Count());
                    break;
                case "FAVORITE":
                    res = res.OrderBy(h => h.FavoriteBy.Count());
                    break;
                default:
                    res = res.OrderBy(h => -h.grade);
                    break;
            }
            return res;

        }
    }
}