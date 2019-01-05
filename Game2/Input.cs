using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public enum InputAction { Left, Right, Up, Down, Shoot, Charge, ChargeShotReady, ChargeShot, Reflect, DashLeft, DashRight, Confirm, Back, None};
public enum ControlType { Menu, GamePlay};

namespace WizardDuel
{
    class Input
    {
        public InputAction inputAction { get; set; }
        ControlType controlType;
        PlayerIndex playerIndex;

        GamePadState oldState;
        GamePadState newState;

        GamePadState leftJoyStick;
        GamePadState rightJoyStick;

        float shootingAngle;

        int chargeTimer;
        int reflectTimer;
        int reflected;
        int menuTimer;
        public int dashed;
        
        public Input(ControlType controlType, PlayerIndex playerIndex)
        {
            this.controlType = controlType;
            this.playerIndex = playerIndex;

            oldState = new GamePadState();
            newState = new GamePadState();
            leftJoyStick = new GamePadState();
            rightJoyStick = new GamePadState();
            dashed = 301;
            reflected = 201;
            reflectTimer = 301;
        }

        public void Update(GameTime gameTime)
        {
            oldState = newState;
            newState = GamePad.GetState(playerIndex);
            leftJoyStick = GamePad.GetState(playerIndex);
            rightJoyStick = GamePad.GetState(playerIndex);
            
            if(controlType == ControlType.Menu)
            {
                menuTimer += gameTime.ElapsedGameTime.Milliseconds;
                if(oldState.Buttons.A == ButtonState.Released && newState.Buttons.A == ButtonState.Pressed)
                {
                    inputAction = InputAction.Confirm;
                }
                else if(oldState.Buttons.B == ButtonState.Released && newState.Buttons.B == ButtonState.Pressed)
                {
                    inputAction = InputAction.Back;
                }
                else if(leftJoyStick.ThumbSticks.Left.X < -.5 & menuTimer > 250)
                {
                    inputAction = InputAction.Left;
                    menuTimer = 0;
                }
                else if(leftJoyStick.ThumbSticks.Left.X > .5 && menuTimer > 250)
                {
                    inputAction = InputAction.Right;
                    menuTimer = 0;
                }
                else if(leftJoyStick.ThumbSticks.Left.Y > .5 & menuTimer > 250)
                {
                    inputAction = InputAction.Up;
                    menuTimer = 0;
                }
                else if(leftJoyStick.ThumbSticks.Left.Y < -.5 && menuTimer > 250)
                {
                    inputAction = InputAction.Down;
                    menuTimer = 0;
                }
                else
                {
                    inputAction = InputAction.None;
                }
            }
            if (controlType == ControlType.GamePlay) 
            {
                reflected += gameTime.ElapsedGameTime.Milliseconds;
                reflectTimer += gameTime.ElapsedGameTime.Milliseconds;
                if (reflected <= 200)
                {
                    inputAction = InputAction.Reflect;
                }
                else if(dashed <= 200)
                {
                    if(inputAction == InputAction.DashLeft)
                    {
                        dashed += gameTime.ElapsedGameTime.Milliseconds;
                        if (dashed >= 200)
                        {
                            if (oldState.Buttons.Y == ButtonState.Pressed && newState.Buttons.Y == ButtonState.Pressed)
                            {
                                inputAction = InputAction.Reflect;
                                reflectTimer = 0;
                                reflected = 0;
                            }
                            else
                            {
                                inputAction = InputAction.None;
                            }
                        }
                    }
                    if(inputAction == InputAction.DashRight)
                    {
                        dashed += gameTime.ElapsedGameTime.Milliseconds;
                        if (dashed >= 200)
                        {
                            if (oldState.Buttons.Y == ButtonState.Pressed && newState.Buttons.Y == ButtonState.Pressed)
                            {
                                inputAction = InputAction.Reflect;
                                reflectTimer = 0;
                                reflected = 0;
                            }
                            else
                            {
                                inputAction = InputAction.None;
                            }
                        }
                    }
                }
                else
                {
                    if (inputAction == InputAction.ChargeShot || inputAction == InputAction.Shoot)
                    {
                        inputAction = InputAction.None;
                        chargeTimer = 0;
                        shootingAngle = 0;
                    }
                    else if (oldState.Triggers.Right == 1 && newState.Triggers.Right == 1 && chargeTimer <= 1000)
                    {
                        chargeTimer += gameTime.ElapsedGameTime.Milliseconds;
                        inputAction = InputAction.Charge;
                        CalculateShootingAngle();
                    }
                    else if (oldState.Triggers.Right == 1 && newState.Triggers.Right == 1 && chargeTimer > 1000)
                    {
                        inputAction = InputAction.ChargeShotReady;
                        CalculateShootingAngle();
                    }
                    else if (oldState.Triggers.Right == 1 && newState.Triggers.Right < 1 && chargeTimer < 1000)
                    {
                        inputAction = InputAction.Shoot;
                    }
                    else if (oldState.Triggers.Right == 1 && newState.Triggers.Right < 1 && chargeTimer > 1000)
                    {
                        inputAction = InputAction.ChargeShot;
                    }
                    else if (oldState.Buttons.Y == ButtonState.Released && newState.Buttons.Y == ButtonState.Pressed)
                    {
                        if (reflectTimer >= 300)
                        {
                            inputAction = InputAction.Reflect;
                            reflectTimer = 0;
                            reflected = 0;
                        }
                        else
                        {

                        }
                    }
                    else if (newState.Buttons.A == ButtonState.Pressed && oldState.Buttons.A == ButtonState.Released && leftJoyStick.ThumbSticks.Left.X < -.3f)
                    {
                        inputAction = InputAction.DashLeft;
                        dashed = 0;
                    }
                    else if (leftJoyStick.ThumbSticks.Left.X < -.3f)
                    {
                        inputAction = InputAction.Left;
                    }
                    else if (newState.Buttons.A == ButtonState.Pressed && oldState.Buttons.A == ButtonState.Released && leftJoyStick.ThumbSticks.Left.X > .3f)
                    {
                        inputAction = InputAction.DashRight;
                        dashed = 0;
                    }
                    else if (leftJoyStick.ThumbSticks.Left.X >= .3f)
                    {
                        inputAction = InputAction.Right;
                    }
                    else
                    {
                        inputAction = InputAction.None;
                    }
                }
            }
        }
        private void CalculateShootingAngle()
        {
            float rightJoyX = rightJoyStick.ThumbSticks.Right.X;
            float rightJoyY = rightJoyStick.ThumbSticks.Right.Y;

            if (playerIndex == PlayerIndex.One)
            {
                if (rightJoyY < .7 && rightJoyX <= 0)
                {
                    rightJoyY = .7f;
                    rightJoyX = -1;
                }
                if (rightJoyY < .7 && rightJoyX > 0)
                {
                    rightJoyY = .7f;
                    rightJoyX = 1;
                }
            }
            if (playerIndex == PlayerIndex.Two)
            {
                if (rightJoyY > -.7 && rightJoyX <= 0)
                {
                    rightJoyY = -.7f;
                    rightJoyX = -1;
                }
                if (rightJoyY > -.7 && rightJoyX > 0)
                {
                    rightJoyY = -.7f;
                    rightJoyX = 1;
                }
            }
            if (playerIndex == PlayerIndex.One)
            {
                shootingAngle = (float)Math.Atan2(rightJoyX, rightJoyY);
            }
            else if(playerIndex == PlayerIndex.Two)
            {
                shootingAngle = (float)Math.Atan2(rightJoyX, rightJoyY) + (float)(Math.PI);
            }
        }
        public float ReturnAngle()
        {
            return shootingAngle;
        }
    }
}
