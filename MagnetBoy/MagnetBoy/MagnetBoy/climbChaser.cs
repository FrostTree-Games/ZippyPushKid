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
    class climbChaser : Attribute
    {
        double walkSwitchTimer = 0;
        bool walkingLeft = false;
        const float walkerSpeed = 0.09f;
        bool chasePlayer = false;

        bool isClimbing = false;
        float temp = 0.0f;
        bool hittingWall = false;

        private Walk walkState = null;
        private bool isEnabled = true;

        public climbChaser(Enemy parent)
        {
            walkState = new Walk(parent);
        }

        public void enableDisable(bool value)
        {
            isEnabled = value;
        }

        public void update(Enemy parent, GameTime currentTime)
        {
            hittingWall = false;

            if (walkSwitchTimer == 0)
            {
                walkSwitchTimer = currentTime.TotalGameTime.TotalMilliseconds;
            }

            foreach (Entity en in Entity.globalEntityList)
            {
                if(isClimbing == true)
                {
                    parent.acceleration.Y = 0.00f;
                }

                if (en is climbWall)
                {
                    if (parent.hitTest(en))
                    {
                        hittingWall = true;
                    }
                }
            }

            if (hittingWall && chasePlayer)
            {
                if (temp > 0)
                {
                    parent.currentAnimation = "angrySawWalkLeft";
                }
                else
                {
                    parent.currentAnimation = "angrySawWalkRight";
                }
                isClimbing = true;
                parent.velocity.X = 0.00f;
                parent.velocity.Y = -0.15f;
                parent.acceleration.Y = 0.0f;
                parent.acceleration.X = 0.0f;
            }
            else
            {
                isClimbing = false;

                foreach (Entity en in Entity.globalEntityList)
                {
                    if (en is Player)
                    {
                        if (parent.PushTime < 1 && en.horizontal_pos - parent.horizontal_pos > -200 && en.horizontal_pos - parent.horizontal_pos < 200 && en.vertical_pos - parent.vertical_pos > -160 && en.vertical_pos - parent.vertical_pos < 160)
                        {
                            chasePlayer = true;
                        }
                        else
                        {
                            chasePlayer = false;
                        }

                        if (chasePlayer == true)
                        {
                            if (en.horizontal_pos - parent.horizontal_pos > -1000 && en.horizontal_pos - parent.horizontal_pos < -30 )
                            {
                                
                                if (walkingLeft == false)
                                {
                                    walkingLeft = true;
                                }

                                parent.velocity.X = -2.5f * walkerSpeed;
                                temp = parent.velocity.X;
                            }
                            else if (en.horizontal_pos - parent.horizontal_pos > -16 && en.horizontal_pos - parent.horizontal_pos < 16)
                            {
                                parent.velocity.X = 0.0f;
                            }
                            else if (en.horizontal_pos - parent.horizontal_pos > 30 && en.horizontal_pos - parent.horizontal_pos < 1000)
                           {
                                if (walkingLeft == true)
                                {
                                    walkingLeft = false;
                                }
                                parent.velocity.X = 2.5f * walkerSpeed;
                                temp = parent.velocity.X;
                            }
                        }
                    }
                }

               if( chasePlayer == false)
               {
                    walkState.update(parent, currentTime);
               }
            }
        }
    }
}
