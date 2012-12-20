﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

/* This class is to be a public abstraction for keyboard/mouse input.
 * It is to follow a singleton pattern.
 */

namespace MagnetBoy
{
    class GameInput
    {
        public enum PlayerButton
        {
            UpDirection,
            DownDirection,
            LeftDirection,
            RightDirection,
            Push,
            Jump
        }

        private static GraphicsDevice graphicsDevice = null;

        private static MouseState mouse;
        private static Vector2 p1MouseDirection;

        private static GamePadState p1PadState;

        public static Vector2 P1MouseDirection
        {
            get
            {
                return p1MouseDirection;
            }
        }

        public static Vector2 P1MouseDirectionNormal
        {
            get
            {
                return Vector2.Normalize(p1MouseDirection);
            }
        }

        public GameInput(GraphicsDevice device)
        {
            if (graphicsDevice == null && device != null)
            {
                graphicsDevice = device;
            }

            update();
        }

        public static Boolean P1MouseDown
        {
            get
            {
                if (mouse.LeftButton == ButtonState.Pressed)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static bool isButtonDown(PlayerButton button)
        {
            switch (button)
            {
                case PlayerButton.UpDirection:
                    if (p1PadState.DPad.Up == ButtonState.Pressed)
                    {
                        return true;
                    }
                    break;
                case PlayerButton.DownDirection:
                    if (p1PadState.DPad.Down == ButtonState.Pressed)
                    {
                        return true;
                    }
                    break;
                case PlayerButton.LeftDirection:
                    if (p1PadState.DPad.Left == ButtonState.Pressed)
                    {
                        return true;
                    }
                    break;
                case PlayerButton.RightDirection:
                    if (p1PadState.DPad.Right == ButtonState.Pressed)
                    {
                        return true;
                    }
                    break;
                case PlayerButton.Push:
                    if (p1MouseDirection.Length() > 0.01f)
                    {
                        return true;
                    }
                    break;
                case PlayerButton.Jump:
                    if (p1PadState.Buttons.RightShoulder == ButtonState.Pressed)
                    {
                        return true;
                    }
                    break;
                default:
                    return false;
            }

            return false;
        }

        public void update()
        {
            mouse = Mouse.GetState();
            p1PadState = GamePad.GetState(PlayerIndex.One);

            if (graphicsDevice.Viewport.Bounds.Contains(new Point(mouse.X, mouse.Y)))
            {
                p1MouseDirection.X = mouse.X - (graphicsDevice.Viewport.Bounds.Width / 2);
                p1MouseDirection.Y = mouse.Y - (graphicsDevice.Viewport.Bounds.Height / 2);
            }
            else if (p1PadState.IsConnected)
            {
                p1MouseDirection = p1PadState.ThumbSticks.Right;
                p1MouseDirection.Y *= -1;

                if (p1MouseDirection.Y == 0 && p1MouseDirection.X != 0)
                {
                    p1MouseDirection.Y = 0.0001f;
                }

            }
            else
            {
                p1MouseDirection = Vector2.Zero;
            }
        }
    }
}
