using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pong.GameObject
{
    public class Balle : GameObject
    {
        public Balle(int height, int width) => Position = new Vector2(width / 2, height / 2);
    }
}
