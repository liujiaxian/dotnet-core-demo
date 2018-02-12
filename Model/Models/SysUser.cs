using System;
namespace Model.Models
{
    public class SysUser
    {
        public long UserId { get; set; }

        public string UserName { get; set; }

        public string UserPassword { get; set; }

        public string UserEmail { get; set; }

        public string CreatedTime { get; set; }
    }
}
