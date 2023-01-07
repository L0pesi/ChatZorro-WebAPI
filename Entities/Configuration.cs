using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatZorro.Entities
{
    public class Configuration
    {
        public int UserCounter { get; set; }
        public int ChatCounter { get; set; }
        public int MessageCounter { get; set; }
        public string SqlConnectionString { get; set; }
    }
}
