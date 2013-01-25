using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using FuncWorks.XNA.XTiled;

namespace MagnetBoy
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private static bool exitGame = false;
        public static bool ExitGame
        {
            get { return exitGame; }
            set { exitGame = value; }
        }

        public static GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        GameInput gameInput = null;

        public static Texture2D globalTestWalrus = null;
        public static Texture2D globalTestPositive = null;
        public static Texture2D globalTestNegative = null;

        public static Texture2D globalBlackPixel = null;

        public static SpriteFont gameFontText = null;

        public static Effect tintRedEffect = null;
        public static Effect grayCheckerBoard = null;

        public static Song testSong = null;

        private AnimationFactory aFac = null;
        private AudioFactory audFac = null;

        public static Random gameRandom = null;

        public static Rectangle mapView;

        private GameScreenManager screenManager = null;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            //game runs in 480p widescreen
            graphics.PreferredBackBufferWidth = 720;
            graphics.PreferredBackBufferHeight = 620;

            //graphics.IsFullScreen = true;

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            gameRandom = new Random();

            base.Initialize();

            mapView = graphics.GraphicsDevice.Viewport.Bounds;

            gameInput = new GameInput(graphics.GraphicsDevice);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            gameFontText = Content.Load<SpriteFont>("testFont");

            aFac = new AnimationFactory(this.Content);
            audFac = new AudioFactory(this.Content);

            aFac.pushSheet("actor3"); // texture stolen from http://www.spriters-resource.com/community/archive/index.php?thread-19817.html
            aFac.pushSheet("playerSheet");
            aFac.pushSheet("conveyer");
            aFac.pushSheet("spikes");
            aFac.pushSheet("cursorTarget");
            aFac.pushSheet("testBulletSheet");
            aFac.pushSheet("angrySawSheet");
            aFac.pushSheet("pushArrow");
            aFac.pushSheet("lolrusSheet");
            aFac.pushSheet("bucketSheet");
            aFac.pushSheet("lavaBottomSheet");
            aFac.pushSheet("fireballSheet");
            aFac.pushSheet("lavaDumperSheet");
            aFac.pushSheet("shieldGuySheet");
            aFac.pushSheet("heartSheet");
            aFac.pushSheet("dansParticleSheet");
            aFac.pushSheet("flagDoorSheet");
            aFac.pushSheet("flagKeySheet");
            aFac.pushSheet("wopleySheet");
            aFac.pushSheet("heartSheet2");
            aFac.pushSheet("brainSheet");
            aFac.pushSheet("lungSheet");
            aFac.pushSheet("tutorialSignsSheet");
            aFac.pushSheet("endLevelSheet");
            aFac.pushSheet("guiComponentSheet");
            aFac.pushSheet("testParallaxSheet");

            aFac.pushAnimation("actor3Anims");
            aFac.pushAnimation("playerAnims");
            aFac.pushAnimation("conveyerAnims");
            aFac.pushAnimation("spikesAnim");
            aFac.pushAnimation("cursorTargetAnims");
            aFac.pushAnimation("testBulletAnims");
            aFac.pushAnimation("angrySaw");
            aFac.pushAnimation("pushArrowAnims");
            aFac.pushAnimation("lolrus");
            aFac.pushAnimation("bucket");
            aFac.pushAnimation("lavaBottom");
            aFac.pushAnimation("fireball");
            aFac.pushAnimation("lavaDumper");
            aFac.pushAnimation("shieldGuy");
            aFac.pushAnimation("heart");
            aFac.pushAnimation("dansParticleAnims");
            aFac.pushAnimation("flagDoorAnims");
            aFac.pushAnimation("flagKeyAnims");
            aFac.pushAnimation("wopley");
            aFac.pushAnimation("heartProjectile");
            aFac.pushAnimation("brain");
            aFac.pushAnimation("lung");
            aFac.pushAnimation("guiComponentAnims");
            aFac.pushAnimation("tutorialSignsAnims");
            aFac.pushAnimation("endLevelAnims");
            aFac.pushAnimation("testParallaxAnims");

            tintRedEffect = this.Content.Load<Effect>("TintRed");
            tintRedEffect.CurrentTechnique = tintRedEffect.Techniques["Technique1"];

            grayCheckerBoard = this.Content.Load<Effect>("GrayCheckerBoard");
            grayCheckerBoard.CurrentTechnique = grayCheckerBoard.Techniques["Technique1"];

            globalTestWalrus = this.Content.Load<Texture2D>("walrus");
            globalTestPositive = this.Content.Load<Texture2D>("posTest");
            globalTestNegative = this.Content.Load<Texture2D>("negTest");
            globalBlackPixel = this.Content.Load<Texture2D>("1x1BlackPixel");

            AudioFactory.pushNewSong("songs/song1");
            AudioFactory.pushNewSong("songs/introTheme");

            AudioFactory.pushNewSFX("sfx/lose");
            AudioFactory.pushNewSFX("sfx/menu");
            AudioFactory.pushNewSFX("sfx/explosion");

            screenManager = new GameScreenManager(this.Content);
            GameScreenManager.switchScreens(GameScreenManager.GameScreenType.Menu, "TitleScreenMenu");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            AudioFactory.stopSong();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                exitGame = true;
            }

            // Allows the game to exit
            if (exitGame)
            {
                this.Exit();
            }

            gameInput.update();

            screenManager.CurrentNode.update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            screenManager.CurrentNode.draw(spriteBatch);

            base.Draw(gameTime);
        }
    }
}
