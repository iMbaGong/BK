using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace BookingRoom.Util
{
    public class Email
    {
        public static void SendEmail(string dst,string content)
        {
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.Credentials = new System.Net.NetworkCredential("1092467670@qq.com", "lnjoobyknmccgeeh");
            smtpClient.Host = "smtp.qq.com";//指定发送的服务器
            smtpClient.Port = 587;//指定端口号
            smtpClient.EnableSsl = true;//是否使用SSL加密
            smtpClient.Timeout = 10 * 1000;//设置超时时间(默认100秒)

            MailMessage mailMessage = new MailMessage();
            //发送人地址
            MailAddress mailAddressFrom = new MailAddress("1092467670@qq.com","民宿预定平台");//如:你好<hello@qq.com>
            mailMessage.From = mailAddressFrom;
            //接收人(可能有多个)
            
            mailMessage.To.Add(dst);
            
            mailMessage.Subject = "邮箱验证";//标题
            
            mailMessage.Body = content;//内容
            mailMessage.BodyEncoding = Encoding.UTF8;//内容编码

            smtpClient.Send(mailMessage);
        }
    }
}