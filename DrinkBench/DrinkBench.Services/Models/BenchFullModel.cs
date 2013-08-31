using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrinkBench.Services.Controllers
{
    public class BenchFullModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<UserModel> Users { get; set; }
    }
}
