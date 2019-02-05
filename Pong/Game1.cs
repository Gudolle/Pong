using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using Pong.Communication;
using Pong.GameObject;
using Pong.Model;
using System.Threading;

namespace Pong
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        public static Joueur joueur;
        public static TypeParty Party = TypeParty.None;



        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public static Player Joueur1 { get; set; }
        public static Player Joueur2 { get; set; }
        public Balle Ball { get; set; }
        public int Latence { get; set; }

        public static string Text = "Appuyez sur C pour creer ou J pour rejoindre la partie";
        public bool IsReady = false;

        public static int WIDTH = 960;
        public static int HEIGHT = 672;
        public Vector2 fontOrigin = new Vector2(0, 0);

        public GameObject.GameObject Basique = new GameObject.GameObject();

        public Request MesRequete = new Request();

        public SpriteFont font;


        public static Thread CreationParty;
        public Thread BoulotJ1thread;
        public Thread BoulotJ2thread;
        public Thread MaLatence;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = WIDTH;
            graphics.PreferredBackBufferHeight = HEIGHT;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Joueur1 = new Player();
            Joueur2 = new Player();
            Ball = new Balle(HEIGHT, WIDTH);
            base.Initialize();

            MaLatence = new Thread(new ThreadStart(DrawPing));
            MaLatence.Start();
            CreationParty = new Thread(new ThreadStart(IsFirstPlayer));
            BoulotJ1thread = new Thread(new ThreadStart(BoulotJ1));
            BoulotJ2thread = new Thread(new ThreadStart(BoulotJ2));

        }
        
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Joueur1.Texture = new Texture2D(graphics.GraphicsDevice, 10, 60);
            Joueur1.SetColor(Color.White);

            Joueur2.Texture = new Texture2D(graphics.GraphicsDevice, 10, 60);
            Joueur2.SetColor(Color.White);
            Ball.Texture = new Texture2D(graphics.GraphicsDevice, 15, 15);
            Ball.SetColor(Color.White);
            Ball.Hidden = true;


            font = Content.Load<SpriteFont>("Font/File");
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            MesRequete.SocketSendReceive(TypeRequete.clear);
            CreationParty.Abort();
            BoulotJ2thread.Abort();
            BoulotJ1thread.Abort();
            MaLatence.Abort();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            // TODO: Add your update logic here
            Joueur1.Move(Keyboard.GetState());

            if (Party == TypeParty.None)
                Joueur1.CreateOrRejoindParty(Keyboard.GetState());

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            
            GraphicsDevice.Clear(Color.Black);
            Joueur1.Draw(spriteBatch);
            Joueur2.Draw(spriteBatch);
            Ball.Draw(spriteBatch);
            Basique.DrawLatence(spriteBatch, Latence, font, fontOrigin);

            if (!IsReady)
            {
                Basique.DrawMessage(spriteBatch, font, fontOrigin, gameTime, Text);
            }

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        public void DrawPing()
        {
            while (true)
            {
                //MesRequete.prendre();
                //Latence = MesRequete.GetPing();
                //MesRequete.poser();
            }
        }
        public void IsFirstPlayer()
        {

            RetourRequete result = new RetourRequete(string.Empty);

            MesRequete.prendre();
            switch (joueur)
            {
                case Joueur.Joueur1:

                    MesRequete.SocketSendReceive(TypeRequete.clear);

                    MesRequete.poser();
                    BoulotJ1thread.Start();
                    //while (!result.IsConnected)
                    //{
                    //    MesRequete.SocketSendReceive(TypeRequete.clearAutre);
                    //    MesRequete.SocketSendReceive(TypeRequete.SendMessage, new { text = "Connection_Joueur_1" });
                    //    result = MesRequete.SocketSendReceive(TypeRequete.RequestMessage);
                    //}
                    Text = "Joueur 1 : Pret";
                    break;
                case Joueur.Joueur2:
                    MesRequete.SocketSendReceive(TypeRequete.clear);
                    MesRequete.poser();
                    BoulotJ2thread.Start();
                    //while (!result.IsConnected)
                    //{
                    //    MesRequete.SocketSendReceive(TypeRequete.clearAutre);
                    //    MesRequete.SocketSendReceive(TypeRequete.SendMessage, new { text = "Connection_Joueur_2" });
                    //    result = MesRequete.SocketSendReceive(TypeRequete.RequestMessage);
                    //}
                    Text = "Joueur 2 : Pret";
                    break;
            }
            
        }
        public void Spam()
        {
        }


        public void BoulotJ2()
        {
            MesRequete.SocketSendReceive(TypeRequete.clear);
            while (true)
            {
                MesRequete.prendre();
                RetourRequete result = MesRequete.SocketSendReceive(TypeRequete.RequestMessage, position: true);
                MesRequete.SocketSendReceive(TypeRequete.SendMessage, new { X = Joueur1.Position.X, Y = Joueur1.Position.Y });
                MesRequete.poser();
                if (result.IsConnected)
                {
                    try
                    {
                        result.Position.GenereJ2();

                        Joueur2.Position = new Vector2(result.Position.information.joueur.X, result.Position.information.joueur.Y);
                        Ball.Position = new Vector2(result.Position.information.balle.X, result.Position.information.balle.Y);
                    }
                    catch { }
                }
                else
                    MesRequete.SocketSendReceive(TypeRequete.clear);
            }
        }
        public void BoulotJ1()
        {
            while (true)
            {
                MesRequete.prendre();
                RetourRequete result = MesRequete.SocketSendReceive(TypeRequete.RequestMessage, position: true);
                MesRequete.SocketSendReceive(TypeRequete.SendMessage, new { joueur = new { Joueur1.Position.X, Joueur1.Position.Y }, balle = new { Ball.Position.X, Ball.Position.Y } });
                MesRequete.poser();
                if (result.IsConnected)
                {
                    try
                    {
                        result.Position.Genere();
                        
                        Joueur2.Position = new Vector2(result.Position.position.X, result.Position.position.Y);
                    }
                    catch { }
                }
                else
                    MesRequete.SocketSendReceive(TypeRequete.clear);
            }
        }
    }

    public enum Joueur
    {
        Joueur1,
        Joueur2
    }
    public enum TypeParty
    {
        Create,
        Joins,
        None
    }
}
