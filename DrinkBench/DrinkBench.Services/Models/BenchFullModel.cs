using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DrinkBench.Services.Models;

namespace DrinkBench.Services.Controllers
{
    public class BenchFullModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<UsersModel> Users { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
    }
}
