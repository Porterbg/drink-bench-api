using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace DrinkBench.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        public virtual User User { get; set; }
        public virtual User Sender { get; set; }
        public string Text { get; set; }
        public DateTime SendDate { get; set; }
        public MessageType Type { get; set; }
    }
}
