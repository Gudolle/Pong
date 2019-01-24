using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pong.GameObject
{
    public class GameObject
    { 
        public Texture2D Texture { get; set; }
        public Vector2 Position { get; set; }

        public int i = 0;
        private TimeSpan now = new TimeSpan();
         
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(Texture, Position, Color.White);
            spriteBatch.End();
        }

        public void DrawLatence(SpriteBatch spriteBatch, int Latence, SpriteFont font, Vector2 fontOrigin)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(
                font, Latence.ToString(),
                new Vector2(50,50),
                Color.White, 0, fontOrigin, 1, SpriteEffects.None, 0);
            spriteBatch.End();
        }
        public void DrawMessage(SpriteBatch spriteBatch, SpriteFont font, Vector2 fontOrigin, GameTime time)
        {
            string text = "En attende d'un autre joueur.";
            for (int x = 0; x < i; x++){
                text += ".";
            }


            spriteBatch.Begin();
            spriteBatch.DrawString(
                font, text,
                new Vector2(200, 200),
                Color.White, 0, fontOrigin, 1, SpriteEffects.None, 0);
            spriteBatch.End();

            if ((time.TotalGameTime - now).TotalMilliseconds > 100)
            {
                i++;
                now = time.TotalGameTime;
            }
            if (i == 6)
                i = 0;
        }

        public void SetColor(Color color)
        {
            int taille = Texture.Bounds.Height * Texture.Bounds.Width;
            Color[] data = new Color[taille];
            for (int i = 0; i < data.Length; ++i) data[i] = color;
            Texture.SetData(data);
        }
    }
}
