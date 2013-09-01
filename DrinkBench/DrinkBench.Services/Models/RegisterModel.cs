using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrinkBench.Services.Controllers
{
    public class RegisterModel
    {
        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string Nickname { get; set; }

        public string Avatar { get; set; }

        public string AuthCode { get; set; }
    }
}
