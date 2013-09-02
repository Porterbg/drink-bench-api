using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using DrinkBench.Services.Models;

namespace DrinkBench.Services.Controllers
{
    [DataContract]
    public class UserModel
    {
        [DataMember(Name="id")]
        public int Id { get; set; }
        [DataMember(Name = "firstname")]
        public string Firstname { get; set; }
        [DataMember(Name = "lastname")]
        public string Lastname { get; set; }
        [DataMember(Name = "nickname")]
        public string Nickname { get; set; }
        [DataMember(Name = "friends")]
        public List<FriendsModel> Friends { get; set; }
        [DataMember(Name = "startTime")]
        public DateTime StartTime { get; set; }
        [DataMember(Name = "endTime")]
        public DateTime EndTime { get; set; }
        [DataMember(Name = "avatar")]
        public string Avatar { get; set; }
        [DataMember(Name = "bench")]
        public BenchModel Bench { get; set; }
    }
}
