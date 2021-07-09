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
            //验证邀请码
            if (user.Admin != null&& user.Admin.invite_code != INVITE_CODE)
            {
                return StatusCode(HttpStatusCode.Forbidden);
            }
            //注册为客户与租客
            if (user.Customer == null)
            {
                user.Customer = new Customer();
            }
            user.Customer.Tenant = new Tenant
            {
                tenant_credit = 60
            };
            //设置主键自增
            user.user_id = db.Database.SqlQuery<decimal>("SELECT USER_ID_SEQ.NEXTVAL FROM dual").First();
            db.User.Add(user);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (UserExists(user.user_name))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }
            return Ok();
        }

        [Route("login")]
        [HttpPost]
        [ResponseType(typeof(UserDto))]
        public IHttpActionResult Login(User src_user)
        {
            IQueryable<User> res = GetUsers(src_user.user_name);
            if (res.Count() != 1)
                return NotFound();
            User res_user = res.First();

            if (res_user == null)
                return NotFound();

            if (res_user.password != src_user.password)
            {
                return StatusCode(HttpStatusCode.Forbidden);
            }
            else
            {
                return Ok(ToDto(res_user));
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

            IQueryable<User> res = GetUsers(src_user.user_name);
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

            IQueryable<User> res = GetUsers(src_user.user_name);
            if (res.Count() != 1)
                return NotFound();
            User res_user = res.First();

            if (res_user == null)
                return NotFound();
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
            IQueryable<User> res = GetUsers(user_name);
            if (res.Count() != 1)
                return NotFound();
            User res_user = res.First();

            if (res_user == null)
                return NotFound();
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

        // DELETE: api/Users/5
        [ResponseType(typeof(User))]
        public IHttpActionResult DeleteUser(decimal id)
        {
            User user = db.User.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            db.User.Remove(user);
            db.SaveChanges();

            return Ok(user);
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

        [NonAction]
        public UserDto ToDto(User user)
        {
            UserDto dto = new UserDto
            {
                user_id = (int)user.user_id,
                user_name = user.user_name
            };
            if (user.Admin != null)
            {
                dto.Admin = new UserDto.AdminDto
                {
                    invite_code = user.Admin.invite_code
                };
            }
            dto.Customer = new UserDto.CustomerDto
            {
                customer_email = user.Customer.customer_email,
                customer_phone = user.Customer.customer_phone,
                customer_identity = user.Customer.customer_identity,
                customer_realname = user.Customer.customer_realname,
                customer_gender = user.Customer.customer_gender
            };
            dto.Customer.Tenant = new UserDto.CustomerDto.TenantDto
            {
                tenant_credit = user.Customer.Tenant.tenant_credit
            };
            if (user.Customer.Landlord != null)
            {
                dto.Customer.Landlord = new UserDto.CustomerDto.LandlordDto
                {
                    landlord_credit = user.Customer.Landlord.landlord_credit,
                    landlord_status = user.Customer.Landlord.landlord_status
                };
            }

            return dto;
        }

        [NonAction]
        public IQueryable<User> GetUsers(string kw)
        {
            return db.User
                .Where(u => u.user_name == kw ||
                u.Customer.customer_email == kw ||
                u.Customer.customer_phone == kw);
        }
    }
}