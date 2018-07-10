using Realms;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectOnlineMobile2.Models2
{
    public class UserModel : RealmObject
    {
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public int UserId { get; set; }
    }
}
