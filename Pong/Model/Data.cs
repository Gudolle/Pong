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
        public InformationJ2 information { get; set; }
        
        public void Genere()
        {
            position = JsonConvert.DeserializeObject<Position>(data);
        }
        public void GenereJ2()
        {
            information = JsonConvert.DeserializeObject<InformationJ2>(data);
        }
    }
    public class InformationJ2
    {
        public Position joueur { get; set; }
        public Position balle { get; set; }
    }
    public class Position
    {
        public float X { get; set; }
        public float Y { get; set; }
    }
}
