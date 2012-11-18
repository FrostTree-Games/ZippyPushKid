﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MagnetBoy
{
    class Enemy: Entity
    {
        string currentAnimation = null;
        int currentFrame = 0;
        double lastFrameIncrement = 0;
        public static List<Attribute> list = null;

        public Enemy()
        {
            creation();

            horizontal_pos = 3.0f;
            vertical_pos = 3.0f;

            velocity = Vector2.Zero;
            acceleration = Vector2.Zero;

            acceleration.Y = 0.001f;
        }

        public Enemy(float initialx, float initialy)
        {
            creation();

            horizontal_pos = initialx;
            vertical_pos = initialy;

            width = 31.5f;
            height = 31.5f;

            velocity = Vector2.Zero;
            acceleration = Vector2.Zero;

            acceleration.Y = 0.001f;

            pole = Polarity.Neutral;
            magneticMoment = 0.5f;

            list = new List<Attribute>();
            list.Add(new Walk(this));
            list.Add(new Jump(this));

        }

        public override void update(GameTime currentTime)
        {
            double delta = currentTime.ElapsedGameTime.Milliseconds;
            KeyboardState ks = Keyboard.GetState();

            //reset the acceleration vector and recompute it
            acceleration = Vector2.Zero;
            acceleration.Y = 0.001f;

            Console.WriteLine("Before going into attributes" + velocity.X);
            foreach( Attribute n in list)
            {
                n.update(this, currentTime);
            }

            Console.WriteLine("return from attributes" + velocity.X);

            acceleration = acceleration + computeMagneticForce();
            
            Vector2 keyAcceleration = Vector2.Zero;
            Vector2 step = new Vector2(horizontal_pos, vertical_pos);
            Vector2 finalAcceleration = acceleration + keyAcceleration;

            velocity.X += (float)(finalAcceleration.X * delta);
            velocity.Y += (float)(finalAcceleration.Y * delta);

            step.X += (float)(((velocity.X) * delta) + (0.5) * (Math.Pow(delta, 2.0)) * finalAcceleration.X);
            step.Y += (float)(((velocity.Y) * delta) + (0.5) * (Math.Pow(delta, 2.0)) * finalAcceleration.Y);

            checkForWalls(Game1.map, ref step);
            if (solid)
            {
                checkForSolidObjects(ref step);
            }

            horizontal_pos = step.X;
            vertical_pos = step.Y;
        }

        protected virtual void enemyUpdate(GameTime currentTime)
        {
            return;
        }

        public override void draw(SpriteBatch sb)
        {
            sb.Draw(Game1.globalTestWalrus, new Vector2(horizontal_pos, vertical_pos), Color.Yellow);
        }

        public void addAttribute(Attribute attr)
        {
            list.Add(new Walk(this));
        }
    }
}
