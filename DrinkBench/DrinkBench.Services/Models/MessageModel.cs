using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrinkBench.Services.Controllers
{
    public class MessageModel
    {
        public int id { get; set; }

        public int userId { get; set; }

        public DateTime sendDate { get; set; }

        public int senderId { get; set; }

        public string Text { get; set; }

        public Models.MessageType Type { get; set; }
    }
}
