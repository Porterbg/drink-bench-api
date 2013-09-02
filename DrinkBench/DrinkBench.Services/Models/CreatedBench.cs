using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace DrinkBench.Services.Controllers
{
    [DataContract]
    class CreatedBench
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "id")]
        public int Id { get; set; }
    }
}
