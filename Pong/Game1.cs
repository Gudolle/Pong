using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pong.Communication;
using Pong.GameObject;
using System.Threading;

namespace Pong
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public Player Joueur1 { get; set; }
        public Player Joueur2 { get; set; }
        public Balle Ball { get; set; }
        public int Latence { get; set; }

        public int WidthEcart = 10;
        public bool IsReady = false;

        public int WIDTH = 960;
        public int HEIGHT = 672;
        public Vector2 fontOrigin = new Vector2(0, 0);

        public GameObject.GameObject Basique = new GameObject.GameObject();


        public SpriteFont font;

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
            Joueur1 = new Player(new Vector2(WidthEcart, (HEIGHT / 2 -30)));
            base.Initialize();


            Thread MaLatence = new Thread(new ThreadStart(DrawPing));
            MaLatence.Start();


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



            font = Content.Load<SpriteFont>("Font/File");
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
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
            Joueur1.SetColor(Color.White);
            Joueur1.Move(Keyboard.GetState());
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
            Basique.DrawLatence(spriteBatch, Latence, font, fontOrigin);



            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        public void DrawPing()
        {
            while (true)
            {
                Latence = Request.GetPing();
            }
        }
    }
}
