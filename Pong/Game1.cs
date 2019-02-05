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
        public Thread GetPosition;
        public Thread EnvoiePosition;
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
            GetPosition = new Thread(new ThreadStart(GetPositionJ2));
            EnvoiePosition = new Thread(new ThreadStart(EnvoiePositionJ2));

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
            EnvoiePosition.Abort();
            GetPosition.Abort();
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
                Request MonPing = new Request();
                Latence = MonPing.GetPing();
                Thread.Sleep(1000);
            }
        }
        public void IsFirstPlayer()
        {

            RetourRequete result = new RetourRequete(string.Empty);
            switch (joueur)
            {
                case Joueur.Joueur1:
                    MesRequete.SocketSendReceive(TypeRequete.clear);
                    while (!result.IsConnected)
                    {
                        MesRequete.SocketSendReceive(TypeRequete.clearAutre);
                        result = MesRequete.SocketSendReceive(TypeRequete.RequestMessage);
                        MesRequete.SocketSendReceive(TypeRequete.SendMessage, new { text = "Connection_Joueur_1" });
                    }
                    Text = "Joueur 1 : Pret";
                    GetPosition.Start();
                    break;
                case Joueur.Joueur2:
                    MesRequete.SocketSendReceive(TypeRequete.clear);
                    while (!result.IsConnected)
                    {
                        MesRequete.SocketSendReceive(TypeRequete.clearAutre);
                        result = MesRequete.SocketSendReceive(TypeRequete.RequestMessage);
                        MesRequete.SocketSendReceive(TypeRequete.SendMessage, new { text = "Connection_Joueur_2" });
                    }
                    Text = "Joueur 2 : Pret";
                    EnvoiePosition.Start();
                    break;
            }
            
        }

        public void EnvoiePositionJ2()
        {
            while (true)
            {
                MesRequete.SocketSendReceive(TypeRequete.SendMessage, new { X = Joueur1.Position.X, Y = Joueur1.Position.Y });
                Thread.Sleep(10);
            }
        }
        public void GetPositionJ2()
        {
            while (true)
            {
                RetourRequete result = MesRequete.SocketSendReceive(TypeRequete.RequestMessage, position: true);
                if (result.IsConnected)
                {
                    try
                    {
                        result.Position.Genere();
                        
                        Joueur2.Position = new Vector2(result.Position.position.X, result.Position.position.Y);
                    }
                    catch { }
                }
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
