using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pong.Model
{
    class Ping
    {
        public double t0 { get; set; }
        public double t1 { get; set; }
        public double t2 { get; set; }
        public double t3 { get; set; }


        public int RetourneLatence()
        {
            return (int)(((t1 - t0) + (t2 - t3)) / 2);
        }
    }
}
