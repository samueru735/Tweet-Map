using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tweet_Map.Core
{
    public class Tweet
    {
        public string User { get; set; }
        public string Text { get; set; }
        public string Place { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
    }
}
