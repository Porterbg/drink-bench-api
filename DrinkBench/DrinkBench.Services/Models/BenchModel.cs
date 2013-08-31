using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrinkBench.Services.Controllers
{
    public class BenchModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int UsersCount { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
    }
}
