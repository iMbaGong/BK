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
using Newtonsoft.Json;
using BookingRoom.Util;
using System.Web;

namespace BookingRoom.Controllers
{
    
    [RoutePrefix("api/users")]
    public class UsersController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();

        public string INVITE_CODE = "BK2021";

        [Route("register")]
        [HttpPost]
        public IHttpActionResult Register(User user)
        {
            //校验字段是否符合约束
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (UserExists(user.user_name))
            {
                return Conflict();
            }
            //验证邀请码
            if (user.Admin != null&& user.Admin.invite_code != INVITE_CODE)
            {
                return StatusCode(HttpStatusCode.Forbidden);
            }
            //注册为客户与租客
            if (user.Customer == null||user.Customer.customer_email==null)
            {
                return BadRequest();
            }
            user.Customer.Tenant = new Tenant
            {
                tenant_credit = 60
            };
            //设置主键自增
            user.user_id = db.Database.SqlQuery<decimal>("SELECT USER_ID_SEQ.NEXTVAL FROM dual").First();

            string user_str = JsonConvert.SerializeObject(user).Replace("\r\n", "");
            byte[] user_byte = System.Text.Encoding.Default.GetBytes(user_str);
            string user_final = "http://47.93.255.191:2021/api/users/varify/email?para="+Convert.ToBase64String(user_byte);
            string dst_email = user.Customer.customer_email;
            Email.SendEmail(dst_email, user_final);
            return Ok();
        }

       [HttpGet]
       [Route("varify/email")]
       public string VarifyEmail(string para)
        {
            byte[] user_byte = Convert.FromBase64String(para);
            string json_str = System.Text.Encoding.Default.GetString(user_byte);
            User user = JsonConvert.DeserializeObject<User>(json_str);

            db.User.Add(user);
            db.SaveChanges();

            return "验证成功";
        }

        [HttpGet]
        [Route("verify/token")]
        public IHttpActionResult VarifyToken()
        {
            string token = HttpContext.Current.Request.Headers["Authorization"];
            switch (JwtHelp.GetJwtDecode(token))
            {
                case "OK":
                    return Ok();
                default:
                    return Unauthorized();
            }
        }

        [Route("login")]
        [HttpPost]
        [ResponseType(typeof(UserDto))]
        public HttpResponseMessage Login(User src_user)
        {
            IQueryable<User> res = GetUsersByName(src_user.user_name);
            if (res.Count() != 1)
                return new HttpResponseMessage(HttpStatusCode.NotFound);

            User res_user = res.First();
            
            if (res_user.password != src_user.password)
            {
                return new HttpResponseMessage(HttpStatusCode.Forbidden);
            }
            else
            {
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, new UserDto(res_user));
                response.Headers.Add("Access-Control-Expose-Headers", "token");
                response.Headers.Add("token", JwtHelp.SetJwtEncode(res_user.user_name));
                return response;
            }
        }

        
        [Route("realname")]
        [HttpPost]
        public IHttpActionResult RealnameValidate(User src_user)
        {
            if (src_user.Customer == null || src_user.Customer.customer_realname == null || src_user.Customer.customer_identity == null)
                return BadRequest();
            {
                //TODO：实名认证API
            }

            IQueryable<User> res = GetUsersByName(src_user.user_name);
            if (res.Count() != 1)
                return NotFound();
            User res_user = res.First();

            res_user.Customer.customer_realname = src_user.Customer.customer_realname;
            res_user.Customer.customer_identity = src_user.Customer.customer_identity;
            db.SaveChanges();

            return Ok();
        }

        [Route("new/admin")]
        [HttpPost]
        public IHttpActionResult NewAdmin(User src_user)
        {
            if (src_user.Admin == null)
                return BadRequest();

            IQueryable<User> res = GetUsersByName(src_user.user_name);
            if (res.Count() != 1)
                return NotFound();
            User res_user = res.First();

            if(src_user.Admin.invite_code == INVITE_CODE)
            {
                if (res_user.Admin == null)
                {
                    res_user.Admin = new Admin() { invite_code = src_user.Admin.invite_code };
                    return Ok();
                }
                else
                {
                    res_user.Admin.invite_code = src_user.Admin.invite_code;
                    return StatusCode(HttpStatusCode.Accepted);
                }
            }
            else
                return StatusCode(HttpStatusCode.Forbidden);
        }

        [Route("new/landlord/{user_name}")]
        [HttpGet]
        public IHttpActionResult NewLandlord(string user_name)
        {
            IQueryable<User> res = GetUsersByName(user_name);
            if (res.Count() != 1)
                return NotFound();
            User res_user = res.First();

            if (res_user.Customer.customer_realname == null || res_user.Customer.customer_identity == null)
                return StatusCode(HttpStatusCode.Forbidden);
            if (res_user.Customer.Landlord != null)
                return Conflict();

            res_user.Customer.Landlord = new Landlord()
            {
                landlord_credit = 60,
                landlord_status = 1
            };
            db.SaveChanges();
            return Ok();
        }

        [Route("{user_name}")]
        [ResponseType(typeof(UserDto))]
        public IHttpActionResult GetUsers(string user_name)
        {
            IQueryable<User> res = GetUsersByName(user_name);
            if (res.Count() != 1)
                return NotFound();
            User res_user = res.First();

            return Ok(new UserDto(res_user));
        }

        [HttpGet]
        [Route("credit/{user_name}/{credit}")]
        public IHttpActionResult SetTenantCredit(string user_name,decimal credit)
        {
            IQueryable<User> res = GetUsersByName(user_name);
            if (res.Count() != 1)
                return NotFound();
            User res_user = res.First();

            res_user.Customer.Tenant.tenant_credit = credit;
            db.SaveChanges();
            return Ok();
        }

        [HttpPost]
        [Route("update/landlord")]
        public IHttpActionResult UpdateLandlord(Landlord landlord)
        {
            if (db.Landlord.Count(e => e.landlord_id == landlord.landlord_id) == 0)
                return NotFound();
            db.Entry(landlord).State= EntityState.Modified;
            db.SaveChanges();
            return Ok();
        }

        [HttpPost]
        [Route("update")]
        public IHttpActionResult Update(User user)
        {
            if (db.User.Count(e => e.user_id == user.user_id) == 0)
                    return NotFound();
            if(db.User.Count(e => e.user_name== user.user_name) != 1)
                return Conflict();
            User res_user = db.User.Find(user.user_id);
            res_user.user_name = user.user_name;
            res_user.Customer.customer_phone = user.Customer.customer_phone;
            res_user.Customer.customer_email = user.Customer.customer_email;
            db.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("all/admin")]
        [ResponseType(typeof(List<UserDto>))]
        public IHttpActionResult GetAllAdmin()
        {
            var dtos = new List<UserDto>();
            foreach(var admin in db.Admin)
            {
                dtos.Add(new UserDto(admin.User));
            }
            return Ok(dtos);
        }

        [HttpGet]
        [Route("all")]
        [ResponseType(typeof(List<UserDto>))]
        public IHttpActionResult GetAllUser()
        {
            var dtos = new List<UserDto>();
            foreach (var user in db.User)
            {
                dtos.Add(new UserDto(user));
            }
            return Ok(dtos);
        }


        // DELETE: api/Users/5
        [ResponseType(typeof(UserDto))]
        public IHttpActionResult DeleteUser(decimal id)
        {
            User user = db.User.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            db.User.Remove(user);
            db.SaveChanges();

            return Ok(new UserDto(user));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(string username)
        {
            return db.User.Count(e => e.user_name == username) > 0;
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