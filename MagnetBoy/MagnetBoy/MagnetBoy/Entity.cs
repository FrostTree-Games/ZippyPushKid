﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using FuncWorks.XNA.XTiled;

namespace MagnetBoy
{
    class Entity
    {
        //sexy class data for our game entity goes here

        protected float horizontal_pos = 0.0f;
        protected float vertical_pos = 0.0f;

        protected float width = 0.0f;
        protected float height = 0.0f;

        protected Vector2 velocity;
        protected Vector2 acceleration;

        protected bool onTheGround = false;
        
        public Entity()
        {
            horizontal_pos = 0.0f;
            vertical_pos = 0.0f;
        }
        
        public Entity(float initialx, float initialy)
        {
            horizontal_pos = initialx;
            vertical_pos = initialy;
        }

        public virtual void update(GameTime currentTime)
        {
            return;
        }

        public virtual void draw(SpriteBatch sb)
        {
            sb.Draw(Game1.globalTestWalrus, new Vector2(horizontal_pos, vertical_pos), Color.White);
        }

        // pass a map and a vector stating where you'd like to move to
        // this method checks against nearby walls and prevents you from overdoing boundaries
        // this method modifies step
        public void checkForWalls(Map checkMap, ref Vector2 delta)
        {
            if (this.horizontal_pos < 0 || this.vertical_pos < 0 || this.horizontal_pos + this.width >= checkMap.Width * checkMap.TileWidth || this.vertical_pos + this.height >= checkMap.Height * checkMap.TileHeight)
            {
                return;
            }

            foreach (TileLayer layer in checkMap.TileLayers)
            {
                bool isSolid = false;

                foreach (KeyValuePair<string, Property> p in layer.Properties)
                {
                    if (p.Key.Equals("solid") && p.Value.AsInt32 == 1)
                    {
                        isSolid = true;
                    }
                }

                if (isSolid == true)
                {
                    if (delta.Y - vertical_pos < -0.0001)
                    {
                        int i;
                        for (i = ((int)delta.Y) / checkMap.TileHeight; i >= 0; i--)
                        {
                            if (layer.Tiles[((int)horizontal_pos) / checkMap.TileWidth][i] != null || layer.Tiles[((int)(horizontal_pos + width)) / checkMap.TileWidth][i] != null)
                            {
                                break;
                            }
                        }

                        onTheGround = false;

                        delta.Y = Math.Max(delta.Y, (i + 1) * checkMap.TileHeight);
                    }
                    else if (delta.Y - vertical_pos > 0.0001)
                    {
                        int i;
                        for (i = ((int)(delta.Y + width)) / checkMap.TileHeight; i < checkMap.Height; i++)
                        {
                            if (layer.Tiles[((int)horizontal_pos) / checkMap.TileWidth][i] != null || layer.Tiles[((int)(horizontal_pos + width)) / checkMap.TileWidth][i] != null)
                            {
                                break;
                            }
                        }

                        if ((i - 1) * 16 < delta.Y)
                        {
                            onTheGround = true;
                        }
                        else
                        {
                            onTheGround = false;
                        }

                        delta.Y = Math.Min(delta.Y, (i - 1) * checkMap.TileHeight);
                    }


                }
            }
        }
    }
}
