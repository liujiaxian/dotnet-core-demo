using System;
namespace Model.Models
{
    public class SysUser
    {
        public long UserId { get; set; }

        public string UserName { get; set; }

        public string UserPassword { get; set; }

        public string UserEmail { get; set; }

        public string EmailToken { get; set; }
        public DateTime EmailExpire { get; set; }

        public DateTime CreatedTime { get; set; }
    }
}
