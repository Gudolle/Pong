using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pong.Model
{
    public class RetourRequete
    {
        public bool IsConnected { get; set; }
        public Data Data { get; set; }
        public Ping Ping { get; set; }


        public RetourRequete(object MonObjet)
        {
            IsConnected = true;
            if (MonObjet != null)
            {
                if (MonObjet.GetType().Name == "Data")
                {
                    Data = (Data)MonObjet;
                }
                else if (MonObjet.GetType().Name == "Ping")
                {
                    Ping = (Ping)MonObjet;
                }
            }
        }
        public RetourRequete()
        {
            IsConnected = false;
        }
    }
}
