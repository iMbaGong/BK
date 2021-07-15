using JWT;
using JWT.Algorithms;
using JWT.Exceptions;
using JWT.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookingRoom.Util
{
    public class JwtHelp
    {

        //私钥  web.config中配置
        //"GQDstcKsx0NHjPOuXOYg5MbeJ1XT0uFiwDVvVBrk";
        private static string secret = "footmark";

        /// <summary>
        /// 生成JwtToken
        /// </summary>
        /// <param name="payload">不敏感的用户数据</param>
        /// <returns></returns>
        public static string SetJwtEncode(string user_name)
        {

            //格式如下
            IDateTimeProvider provider = new UtcDateTimeProvider();
            var now = provider.GetNow();
            var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            //过期时间
            var secondsSinceEpoch = Math.Round((now - unixEpoch).TotalSeconds);

            var payload = new Dictionary<string, object>
            {
                { "exp", secondsSinceEpoch+3600 },  //3600秒后过期
                { "username",user_name },
            };

            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

            var token = encoder.Encode(payload, secret);
            return token;
        }

        /// <summary>
        /// 根据jwtToken  获取实体
        /// </summary>
        /// <param name="token">jwtToken</param>
        /// <returns></returns>
        public static string GetJwtDecode(string token)
        {
            try
            {
                IJsonSerializer serializer = new JsonNetSerializer();
                IDateTimeProvider provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtDecoder decoder = new JwtDecoder(serializer, urlEncoder);
                //token为之前生成的字符串
                var userInfo = decoder.DecodeToObject(token,secret,false);
                //此处json为IDictionary<string, object> 类型
                //string username = userInfo["username"].ToString();  //可获取当前用户名
                return "OK";

            }
            catch (TokenExpiredException)
            {
                //Console.WriteLine("Token has expired");
            }
            catch (SignatureVerificationException)
            {
                //Console.WriteLine("Token has invalid signature");
            }
            catch (Exception)
            {

            }
            return "Error";
        }
    }
}