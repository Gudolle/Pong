using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pong.Model
{
    public class Ping
    {
        public double t0 { get; set; }
        public double t1 { get; set; }
        public double t2 { get; set; }
        public double t3 { get; set; }


        public int RetourneLatence()
        {
            return (int)(((t1 - t0) + (t3 - t2)) / 2);
        }
    }
}
