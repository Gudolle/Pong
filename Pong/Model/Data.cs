using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pong.Model
{
    public class Data
    {
        public string from { get; set; }
        public object data { get; set; }
    }
    public class DataPosition
    {
        public string from { get; set; }
        public string data { get; set; }
        public Position position { get; set; }
        public void Genere()
        {
            position = JsonConvert.DeserializeObject<Position>(data);
        }
    }
    public class Position
    {
        public float X { get; set; }
        public float Y { get; set; }
    }
}
