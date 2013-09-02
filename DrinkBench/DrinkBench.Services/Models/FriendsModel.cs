using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace DrinkBench.Services.Models
{
    [DataContract]
    public class FriendsModel
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }
        [DataMember(Name = "nickname")]
        public string Nickname { get; set; }
        [DataMember(Name = "startTime")]
        public DateTime StartTime { get; set; }
        [DataMember(Name = "endTime")]
        public DateTime EndTime { get; set; }
        [DataMember(Name = "avatar")]
        public string Avatar { get; set; }
    }
}
