﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace DrinkBench.Services.Controllers
{
    [DataContract]
    public class RegisterModel
    {
        [DataMember(Name = "firstname")]
        public string Firstname { get; set; }
        [DataMember(Name = "lastname")]
        public string Lastname { get; set; }
        [DataMember(Name = "nickname")]
        public string Nickname { get; set; }
        [DataMember(Name = "avatar")]
        public string Avatar { get; set; }
        [DataMember(Name = "authCode")]
        public string AuthCode { get; set; }
    }
}
