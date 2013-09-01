using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DrinkBench.Services.Models;

namespace DrinkBench.Services.Controllers
{
    public class UserModel
    {
        public int Id { get; set; }

        public string SessionKey { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string Nickname { get; set; }

        public List<UsersModel> Friends { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public string AuthCode { get; set; }

        public string Avatar { get; set; }

        public BenchModel Bench { get; set; }
    }
}
