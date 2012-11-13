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
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static Texture2D globalTestWalrus = null;
        public static Texture2D globalTestPositive = null;
        public static Texture2D globalTestNegative = null;

        private AnimationFactory aFac = null;

        public static Random gameRandom = null;

        List<Entity> testList = null;

        Camera testCam = null;

        Rectangle mapView;
        public static Map map = null; //this shouldn't be public/static, but for now we need a way of referencing it in an Entity

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
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
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            aFac = new AnimationFactory(this.Content);
            aFac.pushSheet("actor3"); // texture stolen from http://www.spriters-resource.com/community/archive/index.php?thread-19817.html
            aFac.pushAnimation("actor3Anims");
            aFac.pushSheet("playerSheet");
            aFac.pushAnimation("playerAnims");

            globalTestWalrus = this.Content.Load<Texture2D>("walrus");
            globalTestPositive = this.Content.Load<Texture2D>("posTest");
            globalTestNegative = this.Content.Load<Texture2D>("negTest");

            map = Content.Load<Map>("testMap1");

            //MagnetBoyDataTypes.Animation testAnim = Content.Load<MagnetBoyDataTypes.Animation>("testAnimation");

            testList = new List<Entity>();

            testCam = new Camera();

            foreach (ObjectLayer layer in map.ObjectLayers)
            {
                foreach (MapObject obj in layer.MapObjects)
                {
                    Entity en = null;

                    switch (obj.Name)
                    {
                        case "player":
                            en = new Player(obj.Bounds.X, obj.Bounds.Y);
                            testList.Add(en);
                            testCam.setNewFocus(ref en);
                            break;
                        case "wm_pos":
                            testList.Add(new WallMagnet(obj.Bounds.X, obj.Bounds.Y, Entity.Polarity.Positive));
                            break;
                        case "wm_neg":
                            testList.Add(new WallMagnet(obj.Bounds.X, obj.Bounds.Y, Entity.Polarity.Negative));
                            break;
                        case "walker_pos":
                            testList.Add(new WalkingEnemy(obj.Bounds.X, obj.Bounds.Y, Entity.Polarity.Positive));
                            break;
                        case "walker_neg":
                            testList.Add(new WalkingEnemy(obj.Bounds.X, obj.Bounds.Y, Entity.Polarity.Negative));
                            break;
                        default:
                            break;
                    }
                }
            }

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            foreach (Entity a in testList)
            {
                a.death();
            }
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            foreach (Entity a in testList)
            {
                a.update(gameTime);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            Matrix mx = new Matrix();
            Rectangle rx = new Rectangle();
            testCam.getDrawTranslation(ref mx, ref mapView, ref map);
            testCam.getDrawRectangle(ref rx, ref mapView, ref map);

            GraphicsDevice.Clear(Color.Gray);

            // draw map
            spriteBatch.Begin();
            map.Draw(spriteBatch, rx);
            spriteBatch.End();

            // draw sprites
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, mx);

            foreach (Entity a in testList)
            {
                a.draw(spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
