using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pong.Model
{
    class WriteExceptionError
    {
        public WriteExceptionError(Exception ex)
        {

            if (Directory.Exists(@"Erreur"))
                Directory.CreateDirectory(@"Erreur");

            //File.WriteAllText(@"Erreur\" + DateTime.Now + ".txt", ex.Message);
        }
    }
}
