using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrinkBench.Services.Models
{
    public class UsersModel
    {
        public int Id { get; set; }

        public string Nickname { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public string Avatar { get; set; }
    }
}
