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
        public DataPosition Position { get; set; }
        public string page { get; set; }
        public Joueur joueur { get; set; }


        public RetourRequete(object MonObjet, string _page)
        {
            joueur = Game1.joueur;
            IsConnected = true;
            page = _page;
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
                else if (MonObjet.GetType().Name == "DataPosition")
                    Position = (DataPosition)MonObjet;
            }
        }
        public RetourRequete(string _page)
        {
            joueur = Game1.joueur;
            page = _page;
            IsConnected = false;
        }
    }
}
