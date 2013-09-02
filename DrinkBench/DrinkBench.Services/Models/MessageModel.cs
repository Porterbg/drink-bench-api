using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using DrinkBench.Models;

namespace DrinkBench.Services.Controllers
{
    [DataContract]
    public class MessageModel
    {
        [DataMember(Name = "id")]
        public int id { get; set; }
        [DataMember(Name = "userId")]
        public int userId { get; set; }
        [DataMember(Name = "sendDate")]
        public DateTime sendDate { get; set; }
        [DataMember(Name = "senderId")]
        public int senderId { get; set; }
        [DataMember(Name = "Text")]
        public string Text { get; set; }
        [DataMember(Name = "Type")]
        public MessageType Type { get; set; }
    }
}
