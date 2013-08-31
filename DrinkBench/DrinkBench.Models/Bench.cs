using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrinkBench.Models
{
    public class Bench
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public Bench()
        {
            this.Users = new HashSet<User>();
        }
    }
}
