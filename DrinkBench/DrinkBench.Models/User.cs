using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace DrinkBench.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Nickname { get; set; }
        [Required]
        public string AuthCode { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Avatar { get; set; }
        public string SessionKey { get; set; }
        public virtual Bench Bench { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public virtual ICollection<User> Friends { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
        public User()
        {
            this.Friends = new HashSet<User>();
            this.Messages = new HashSet<Message>();
        }
    }
}
