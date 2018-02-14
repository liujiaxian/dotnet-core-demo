using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public class MailHelper
    {
        private const string emailAddress = "admin@jihuiweb.com";
        private const string emailPassword = "LJXgzh@159263";
        private const int emailPort = 465;
        private const string smtpServer = "smtp.exmail.qq.com";


        /// <summary>
        /// 发送电子邮件
        /// </summary>
        /// <param name="smtpServer">SMTP服务器</param>
        /// <param name="enableSsl">是否启用SSL加密</param>
        /// <param name="userName">登录帐号</param>
        /// <param name="pwd">登录密码</param>
        /// <param name="nickName">发件人昵称</param>
        /// <param name="fromEmail">发件人</param>
        /// <param name="toEmail">收件人</param>
        /// <param name="subj">主题</param>
        /// <param name="bodys">内容</param>
        public static async Task SendMailAsync(bool enableSsl,  string nickName,  string toMail, string subj, string bodys)
        {
            SmtpClient smtpClient = new SmtpClient();

            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;//指定电子邮件发送方式

            smtpClient.Host = smtpServer;//指定SMTP服务器

            smtpClient.Credentials = new NetworkCredential(emailAddress, emailPassword);//用户名和密码

            smtpClient.EnableSsl = enableSsl;

            MailAddress fromAddress = new MailAddress(emailAddress, nickName);

            MailAddress toAddress = new MailAddress(toMail);

            MailMessage mailMessage = new MailMessage(fromAddress, toAddress);

            mailMessage.Subject = subj;//主题

            mailMessage.Body = bodys;//内容

            mailMessage.BodyEncoding = Encoding.Default;//正文编码

            mailMessage.IsBodyHtml = true;//设置为HTML格式

            mailMessage.Priority = MailPriority.Normal;//优先级

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}