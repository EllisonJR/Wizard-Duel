using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public enum InputAction { LeftMenu, RightMenu, Left, Right, Up, Down, Shoot, Charge, ChargeShotReady, ChargeShot, Reflect, DashLeft, DashRight, Confirm, Back, None};
public enum ControlType { Menu, GamePlay};

namespace WizardDuel
{
    class Input
    {
        public InputAction inputAction { get; set; }
        public ControlType controlType;
        PlayerIndex playerIndex;

        GamePadState oldState;
        GamePadState newState;

        GamePadState leftJoyStick;
        GamePadState rightJoyStick;

        KeyboardState oldKBState;
        KeyboardState newKBState;
        int mouseStateX;
        int mouseStateY;
        MouseState mousePos;
        MouseState newMouseState;
        MouseState oldMouseState;
        Vector2 origin;
        Vector2 direction;

        float shootingAngle;

        public int chargeTimer;
        public int reflectTimer;
        public int reflected;
        int menuTimer;
        public int dashed;

        bool AI = false;
        
        public Input(ControlType controlType, PlayerIndex playerIndex, bool AI)
        {
            this.controlType = controlType;
            this.playerIndex = playerIndex;
            this.AI = AI;
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
            if (GamePad.GetCapabilities(playerIndex).IsConnected == true)
            {
                GamePadControls(gameTime);
            }
            else
            {
                KeyboardControls(gameTime);
            }
        }
        public void KeyboardControls(GameTime gameTime)
        {
            oldKBState = newKBState;
            oldMouseState = newMouseState;
            newKBState = Keyboard.GetState();
            newMouseState = Mouse.GetState();
            mouseStateX = newMouseState.X;
            mouseStateY = newMouseState.Y;

            if (AI == false)
            {

            }
            if (controlType == ControlType.Menu)
            {
                menuTimer += gameTime.ElapsedGameTime.Milliseconds;
                if (oldKBState.IsKeyUp(Keys.Space) && newKBState.IsKeyDown(Keys.Space))
                {
                    inputAction = InputAction.Confirm;
                }
                else if (oldKBState.IsKeyUp(Keys.Escape) && newKBState.IsKeyDown(Keys.Escape))
                {
                    inputAction = InputAction.Back;
                }
                else if (oldKBState.IsKeyUp(Keys.A) && newKBState.IsKeyDown(Keys.A))
                {
                    inputAction = InputAction.LeftMenu;
                    menuTimer = 0;
                }
                else if (oldKBState.IsKeyUp(Keys.D) && newKBState.IsKeyDown(Keys.D))
                {
                    inputAction = InputAction.RightMenu;
                    menuTimer = 0;
                }
                else if (oldKBState.IsKeyUp(Keys.W) && newKBState.IsKeyDown(Keys.W))
                {
                    inputAction = InputAction.Up;
                    menuTimer = 0;
                }
                else if (oldKBState.IsKeyUp(Keys.S) && newKBState.IsKeyDown(Keys.S))
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
                else if (dashed <= 200)
                {
                    if (inputAction == InputAction.DashLeft)
                    {
                        dashed += gameTime.ElapsedGameTime.Milliseconds;
                        if (dashed >= 200)
                        {
                            if (oldKBState.IsKeyUp(Keys.W) && newKBState.IsKeyDown(Keys.W))
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
                    if (inputAction == InputAction.DashRight)
                    {
                        dashed += gameTime.ElapsedGameTime.Milliseconds;
                        if (dashed >= 200)
                        {
                            if (oldKBState.IsKeyUp(Keys.W) && newKBState.IsKeyDown(Keys.W))
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
                    else if (newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Pressed && chargeTimer <= 1000)
                    {
                        chargeTimer += gameTime.ElapsedGameTime.Milliseconds;
                        inputAction = InputAction.Charge;
                        CalculateShootingAngle();
                    }
                    else if (newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Pressed && chargeTimer > 1000)
                    {
                        inputAction = InputAction.ChargeShotReady;
                        CalculateShootingAngle();
                    }
                    else if (newMouseState.LeftButton == ButtonState.Released && oldMouseState.LeftButton == ButtonState.Pressed && chargeTimer < 1000)
                    {
                        inputAction = InputAction.Shoot;
                    }
                    else if (newMouseState.LeftButton == ButtonState.Released && oldMouseState.LeftButton == ButtonState.Pressed && chargeTimer > 1000)
                    {
                        inputAction = InputAction.ChargeShot;
                    }
                    else if (oldKBState.IsKeyUp(Keys.W) && newKBState.IsKeyDown(Keys.W))
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
                    else if (newKBState.IsKeyDown(Keys.A) && oldMouseState.RightButton == ButtonState.Pressed)
                    {
                        inputAction = InputAction.DashLeft;
                        dashed = 0;
                    }
                    else if (newKBState.IsKeyDown(Keys.A))
                    {
                        inputAction = InputAction.Left;
                    }
                    else if (newKBState.IsKeyDown(Keys.D) && oldMouseState.RightButton == ButtonState.Pressed)
                    {
                        inputAction = InputAction.DashRight;
                        dashed = 0;
                    }
                    else if (newKBState.IsKeyDown(Keys.D))
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
        public void GamePadControls(GameTime gameTime)
        {
            if (AI == false)
            {
                oldState = newState;
                newState = GamePad.GetState(playerIndex);
                leftJoyStick = GamePad.GetState(playerIndex);
                rightJoyStick = GamePad.GetState(playerIndex);
            }
            if (controlType == ControlType.Menu)
            {
                menuTimer += gameTime.ElapsedGameTime.Milliseconds;
                if (oldState.Buttons.A == ButtonState.Released && newState.Buttons.A == ButtonState.Pressed)
                {
                    inputAction = InputAction.Confirm;
                }
                else if (oldState.Buttons.B == ButtonState.Released && newState.Buttons.B == ButtonState.Pressed)
                {
                    inputAction = InputAction.Back;
                }
                else if (leftJoyStick.ThumbSticks.Left.X < -.5 & menuTimer > 250)
                {
                    inputAction = InputAction.LeftMenu;
                    menuTimer = 0;
                }
                else if (leftJoyStick.ThumbSticks.Left.X > .5 && menuTimer > 250)
                {
                    inputAction = InputAction.RightMenu;
                    menuTimer = 0;
                }
                else if (leftJoyStick.ThumbSticks.Left.Y > .5 & menuTimer > 250)
                {
                    inputAction = InputAction.Up;
                    menuTimer = 0;
                }
                else if (leftJoyStick.ThumbSticks.Left.Y < -.5 && menuTimer > 250)
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
                else if (dashed <= 200)
                {
                    if (inputAction == InputAction.DashLeft)
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
                    if (inputAction == InputAction.DashRight)
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
            if (GamePad.GetCapabilities(playerIndex).IsConnected == true)
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
                else if (playerIndex == PlayerIndex.Two)
                {
                    shootingAngle = (float)Math.Atan2(rightJoyX, rightJoyY) + (float)(Math.PI);
                }
            }
            else
            {
                Vector2 mousePos = new Vector2(mouseStateX, mouseStateY);
                direction = mousePos - origin;
                shootingAngle = (float)Math.Atan2(direction.X, direction.Y);
                
                

                if(shootingAngle  < -.96f)
                {
                    shootingAngle = -.96f;
                }
                if(shootingAngle > .96f)
                {
                    shootingAngle = .96f;
                }
            }
        }
        public float ReturnAngle()
        {
            return shootingAngle;
        }
        public void GetRotationOrigin(Vector2 origin)
        {
            this.origin = origin;
        }
    }
}
