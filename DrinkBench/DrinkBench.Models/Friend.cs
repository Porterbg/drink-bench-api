using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrinkBench.Models
{
    public class Friend
    {
        [Key]
        public int Id { get; set; }
        public string Nickname { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Avatar { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public Friend()
        {
            this.Users = new HashSet<User>();
        }
    }
}
