using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace DrinkBench.Services.Controllers
{
    [DataContract]
    public class BenchModel
    {
        [DataMember(Name="id")]
        public int Id { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "usersCount")]
        public int UsersCount { get; set; }
        [DataMember(Name = "latitude")]
        public decimal Latitude { get; set; }
        [DataMember(Name = "longitude")]
        public decimal Longitude { get; set; }
        [DataMember(Name = "startTime")]
        public DateTime StartTime { get; set; }
        [DataMember(Name = "endTime")]
        public DateTime EndTime { get; set; }
        [DataMember(Name = "users")]
        public IEnumerable<UserModel> Users { get; set; }
    }
}
