using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace DrinkBench.Services.Controllers
{
    [DataContract]
    public class LoggedUserModel
    {
        [DataMember(Name = "firstname")]
        public string Firstname { get; set; }
        [DataMember(Name = "sessionKey")]
        public string SessionKey { get; set; }
        [DataMember(Name = "id")]
        public int Id { get; set; }
    }
}
