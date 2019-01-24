using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pong.GameObject
{
    public class Player : GameObject
    {
        public Player(Vector2 Pos)
        {
            Position = Pos;
        }

        public void Move(KeyboardState state)
        {
            float x;
            float y;
            Position.Deconstruct(out x,out y);

            if (state.IsKeyDown(Keys.Up))
                y-=5;
            if (state.IsKeyDown(Keys.Down))
               y+=5;

            Position = new Vector2(x, y);
        }
   
    }
}
