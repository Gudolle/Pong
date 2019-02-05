using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Pong.Model;

namespace Pong.Communication
{
    public class Request
    {
        private int KeyJ1 = 262166;
        private int KeyJ2 = 262168;
        
        private object DataAEnvoyer;
        
        private string Host = "syllab.com";

        private string BaseUri = "http://syllab.com/PTRE839";
        

        public int GetPing()
        {
            RetourRequete retour = SocketSendReceive(TypeRequete.Ping);
            if (retour.IsConnected)
            {
                Ping Delay = retour.Ping;
                Delay.t3 = GetUnixNow();

                return Delay.RetourneLatence();
            }
            return 0;
            
        }
        private long GetUnixNow()
        {
            return (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds;
        }

        private string RetourneJsonMessage(object data)
        {
            return JsonConvert.SerializeObject(data);
        }
        
        public RetourRequete SocketSendReceive(TypeRequete requete, object ObjetAEnvoyer = null, bool position = false)
        {
            string MonJson = "";
            object MonObject = null;
            DataAEnvoyer = ObjetAEnvoyer;

            string MaRequeteString = GetRequete(requete);
            MonJson = GetRetour(MaRequeteString);


            if (MonJson.Contains('{'))
            {
                int Index = MonJson.IndexOf('{');
                int IndexFin = (MonJson.LastIndexOf('}') + 1);

                string chaine = MonJson.Substring(Index, IndexFin - Index);
                chaine = chaine.Replace(@"\\", string.Empty);

                if (position)
                    MonObject = JsonConvert.DeserializeObject<DataPosition>(chaine);
                else if (requete == TypeRequete.Ping)
                    MonObject = JsonConvert.DeserializeObject<Ping>(chaine);
                else
                    MonObject = JsonConvert.DeserializeObject<Data>(chaine);
            }
            else if (MonJson.Contains("TimeOut") || MonJson.Contains("500 Server error") || MonJson.Contains("504 Gateway Timeout") || MonJson.Contains("503 Service Temporarily Unavailable"))
                return new RetourRequete(MonJson);

            return new RetourRequete(MonObject, MonJson);
        }

        
        private string GetRetour(string request)
        {
            Byte[] bytesSent = Encoding.ASCII.GetBytes(request);
            Byte[] bytesReceived = new Byte[1024];
            string page = "";
            using (Socket s = ConnectionSocket())
            {
                //s.ReceiveTimeout = 5000;
                s.NoDelay = true;
                s.ReceiveBufferSize = 16384;
                s.SendBufferSize = 16384;
                // Send request to the server.
                s.Send(bytesSent, bytesSent.Length, 0);
                

                // Receive the server home page content.
                int bytes = 0;
                page = "Default HTML page on " + Host + ":\r\n";
                // The following will block until the page is transmitted.

                try
                {
                    bytes = s.Receive(bytesReceived, bytesReceived.Length, SocketFlags.None);
                    page = page + Encoding.ASCII.GetString(bytesReceived, 0, bytes);
                }
                catch(Exception ex)
                {
                    new WriteExceptionError(ex);
                    page = "ERROR : TimeOut";
                }
                
            }
            
            return page;
        }
        
        private string GetRequete(TypeRequete type)
        {
            //HttpRequestMessage request = new HttpRequestMessage();
            
            string requete = "";

            switch (type)
            {
                case TypeRequete.Ping:
                    requete = "GET /PTRE839/pings?k=262166&t0=" + GetUnixNow();
                    break;
                case TypeRequete.RequestMessage:
                    requete = "GET /PTRE839/msgs?k="+ GetKey() + "&timeout=5";
                    break;
                case TypeRequete.SendMessage:
                    requete = "POST /PTRE839/msgs?k=" + GetKey() + "&to=" + GetKeyJ2() + "&data=" + RetourneJsonMessage(DataAEnvoyer).Replace(" ", "%20");
                    break;
                case TypeRequete.clear:
                    requete = "DELETE /PTRE839/players/"+ GetKey() + "?k=262166";
                    break;
                case TypeRequete.clearAutre:
                    requete = "DELETE /PTRE839/players/" + GetKeyJ2() + "?k=262166";
                    break;
                default:
                    requete = "ERROR";
                    break;
            }
            requete += " HTTP/1.1\r\nHost: " + Host + "\r\nContent-Length: 0\r\n\r\n";
            //request.RequestUri = new Uri(requete);
            return requete;
        }

        private string GetKey()
        {
            if (Game1.joueur == Joueur.Joueur1)
            {
                return KeyJ1.ToString();
            }
            return KeyJ2.ToString();
        }
        private string GetKeyJ2()
        {
            if (Game1.joueur == Joueur.Joueur1)
            {
                return KeyJ2.ToString();
            }
            return KeyJ1.ToString();
        }

        private Socket ConnectionSocket()
        {
            Socket s = null;
            IPHostEntry hostEntry = Dns.GetHostEntry(Host);

            // Loop through the AddressList to obtain the supported AddressFamily. This is to avoid
            // an exception that occurs when the host IP Address is not compatible with the address family
            // (typical in the IPv6 case).
            foreach (IPAddress address in hostEntry.AddressList)
            {
                IPEndPoint ipe = new IPEndPoint(address, 80);
                Socket tempSocket =
                    new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                tempSocket.Connect(ipe);

                if (tempSocket.Connected)
                {
                    s = tempSocket;
                    break;
                }
                else
                {
                    continue;
                }
            }
            return s;
        }
        
    }
    public enum TypeRequete
    {
        Ping,
        SendMessage,
        RequestMessage,
        clear,
        clearAutre
    }
}
