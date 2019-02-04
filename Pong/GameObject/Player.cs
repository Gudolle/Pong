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
        public Player()
        {

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

        public void CreateOrRejoindParty(KeyboardState state)
        {
            if(state.IsKeyDown(Keys.C))
            {
                Game1.Party = TypeParty.Create;
                Game1.joueur = Joueur.Joueur1;
                Game1.Text = "En attende du joueur 2";
                Game1.CreationParty.Start();
                Position = new Vector2(10, (Game1.HEIGHT / 2 - 30));
            }
            if (state.IsKeyDown(Keys.J))
            {
                Game1.Party = TypeParty.Joins;
                Game1.joueur = Joueur.Joueur2;
                Game1.Text = "En attende du joueur 1";
                Game1.CreationParty.Start();
                Position = new Vector2(Game1.WIDTH-20, (Game1.HEIGHT / 2 - 30));
            }
        }
   
    }
}
