using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace WizardDuel
{
    class Player
    {
        public ContentManager content;
        public GraphicsDeviceManager graphics;
        public PlayerIndex playerIndex { get; }
        public bool AI;
        GameStates gameState;
        
        public int playerSpeed;

        public Rectangle aiRectangle;

        public float angle;
        public Texture2D playerSprite;
        public Texture2D playerRetical;
        Texture2D playerReflect;
        Texture2D playerHealthContainer;
        Texture2D playerHealthBar;
        Texture2D playerGoal;
        public Rectangle hitBox;
        public Rectangle reflectHitBox;
        Texture2D shotMeterTexture;
        Rectangle shotMeter;
        float shotMeterCounter;
        Vector2 shotMeterLocation;

        Vector2 rotationOrigin;
        public Vector2 projectileOrigin { get; set; }
        public Vector2 playerLocation;
        public Vector2 reticalLocation;
        Vector2 reflectorLocation;
        Vector2 healthContainerLocation;
        Vector2 healthBarLocation;

        Matrix shooterMatrix;

        public Input input;

        public int health;
        public int score { get; set; }

        public float shootingAngle { get; set; }

        public InputAction inputAction { get; set; }
       
        public Player(ControlType controlType, PlayerIndex playerIndex, ContentManager content, GraphicsDeviceManager graphics, GameStates gameState)
        {
            this.content = content;
            this.graphics = graphics;
            this.playerIndex = playerIndex;
            this.gameState = gameState;
            if(this.gameState == GameStates.SinglePlayer && playerIndex == PlayerIndex.Two)
            {
                AI = true;
            }
            input = new Input(controlType, playerIndex, AI);
            health = 3;
            score = 0;
            playerSpeed = 5;
        }
        public void LoadContent()
        {
            playerSprite = content.Load<Texture2D>("sprites/player");
            playerRetical = content.Load<Texture2D>("sprites/retical");
            playerReflect = content.Load<Texture2D>("sprites/reflecter");
            playerHealthContainer = content.Load<Texture2D>("sprites/healthcontainer");
            playerHealthBar = content.Load<Texture2D>("sprites/healthbar");
            shotMeterTexture = content.Load<Texture2D>("sprites/player");

            shotMeterCounter = shotMeterTexture.Width;

            if (playerIndex == PlayerIndex.One)
            {
                playerLocation = new Vector2((graphics.PreferredBackBufferWidth / 2) - (playerSprite.Width / 2), graphics.PreferredBackBufferHeight - playerSprite.Height - 50);
                reticalLocation = new Vector2((playerLocation.X + playerSprite.Width / 2), playerLocation.Y);
                reflectorLocation = new Vector2(playerLocation.X - (playerSprite.Width / 2) - 5, playerLocation.Y - 10);
                hitBox = new Rectangle((int)playerLocation.X, (int)playerLocation.Y, playerSprite.Width, playerSprite.Height);
                reflectHitBox = new Rectangle((int)reflectorLocation.X, (int)reflectorLocation.Y, playerReflect.Width, playerReflect.Height + 5);
                healthContainerLocation = new Vector2(graphics.PreferredBackBufferWidth / 2 - (playerHealthContainer.Width / 2), graphics.PreferredBackBufferHeight - 42);
                shotMeterLocation = new Vector2(playerLocation.X, playerLocation.Y + 2 + playerSprite.Height);
            }
            else if (playerIndex == PlayerIndex.Two)
            {
                playerLocation = new Vector2((graphics.PreferredBackBufferWidth / 2) - (playerSprite.Width / 2), +50);
                reticalLocation = new Vector2(playerLocation.X + (playerSprite.Width / 2), playerLocation.Y + playerSprite.Height);
                reflectorLocation = new Vector2(playerLocation.X - (playerSprite.Width / 2) - 5, playerLocation.Y + 5);
                hitBox = new Rectangle((int)playerLocation.X, (int)playerLocation.Y, playerSprite.Width, playerSprite.Height);
                reflectHitBox = new Rectangle((int)reflectorLocation.X, (int)reflectorLocation.Y + playerSprite.Height, playerReflect.Width, playerReflect.Height + 5);
                healthContainerLocation = new Vector2(graphics.PreferredBackBufferWidth / 2 - (playerHealthContainer.Width / 2), 30);
                shotMeterLocation = new Vector2(playerLocation.X, playerLocation.Y - 4);
                if(AI == true)
                {
                    aiRectangle = new Rectangle((int)playerLocation.X - 45, (int)playerLocation.Y, 120, playerSprite.Height);
                }
            }
            healthBarLocation = new Vector2(healthContainerLocation.X + 3, healthContainerLocation.Y + 3);
        }
        public void UnloadContent()
        {
            content.Unload();
        }
        public void Update(GameTime gameTime)
        {
            if (AI == false)
            {
                angle = input.ReturnAngle();
                input.Update(gameTime);
                ShotMeter();
                inputAction = input.inputAction;
                PlayerMovement(gameTime);
                CalculateRotationOrigin();
                CalculateProjectileOriginAndDirection();
                shootingAngle = input.ReturnAngle();
            }
            else if(AI == true)
            {

            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(playerSprite, playerLocation, Color.White);
            if (inputAction == InputAction.Reflect)
            {
                spriteBatch.Draw(playerReflect, reflectHitBox, Color.White);
            }
            else
            {

            }
            if(inputAction == InputAction.Charge || inputAction == InputAction.ChargeShotReady)
            {
                spriteBatch.Draw(playerRetical, reticalLocation, null, Color.White, angle, rotationOrigin, 1f, SpriteEffects.None, 1f);
            }
            else
            {

            }
            if(health == 3)
            {
                spriteBatch.Draw(playerHealthBar, healthBarLocation, Color.White);
                spriteBatch.Draw(playerHealthBar, new Vector2(healthBarLocation.X + 1 + playerHealthBar.Width, healthBarLocation.Y), Color.White);
                spriteBatch.Draw(playerHealthBar, new Vector2(healthBarLocation.X + 2 + playerHealthBar.Width * 2, healthBarLocation.Y), Color.White);
            }
            else if(health == 2)
            {
                spriteBatch.Draw(playerHealthBar, healthBarLocation, Color.White);
                spriteBatch.Draw(playerHealthBar, new Vector2(healthBarLocation.X + 1 + playerHealthBar.Width, healthBarLocation.Y), Color.White);
            }
            else if(health == 1)
            {
                spriteBatch.Draw(playerHealthBar, healthBarLocation, Color.White);
            }
            else
            {

            }
            spriteBatch.Draw(playerHealthContainer, healthContainerLocation, Color.White);
            spriteBatch.Draw(shotMeterTexture, new Rectangle((int)shotMeterLocation.X, (int)shotMeterLocation.Y, (int)shotMeterCounter, 2), Color.White);
        }
        public void PlayerMovement(GameTime gameTime)
        {
            if (input.inputAction == InputAction.DashLeft || input.inputAction == InputAction.DashRight)
            {
                if(input.inputAction == InputAction.DashLeft)
                {
                    if (input.dashed <= 100)
                    {
                        playerSpeed = 20;
                    }
                    if (input.dashed > 100 && input.dashed <= 150)
                    {
                        playerSpeed = 8;
                    }
                    if(input.dashed > 175 && input.dashed <= 200)
                    {
                        playerSpeed = 7;
                    }
                    if(input.dashed > 200)
                    {
                        playerSpeed = 5;
                        inputAction = InputAction.None;
                    }
                    playerLocation.X -= playerSpeed;
                }
                if(input.inputAction == InputAction.DashRight)
                {
                    if (input.dashed <= 100)
                    {
                        playerSpeed = 20;
                    }
                    if (input.dashed > 100 && input.dashed <= 150)
                    {
                        playerSpeed = 8;
                    }
                    if (input.dashed > 250 && input.dashed <= 200)
                    {
                        playerSpeed = 7;
                    }
                    if (input.dashed > 200)
                    {
                        playerSpeed = 5;
                        inputAction = InputAction.None;
                    }
                    playerLocation.X += playerSpeed;
                }
            }
            if (input.inputAction == InputAction.Left)
            {
                playerSpeed = 5;
                playerLocation.X -= playerSpeed;
            }
            else if (input.inputAction == InputAction.Right)
            {
                playerSpeed = 5;
                playerLocation.X += playerSpeed;
            }
            if (playerLocation.X > graphics.PreferredBackBufferWidth - playerSprite.Width - 25)
            {
                playerLocation.X = graphics.PreferredBackBufferWidth - playerSprite.Width - 25;
            }
            if (playerLocation.X < 25)
            {
                playerLocation.X = 25;
            }
            LockObjectstoPlayer();
        }
        public void LockObjectstoPlayer()
        {
            reticalLocation.X = playerLocation.X + (playerSprite.Width / 2);
            hitBox.X = (int)playerLocation.X;
            reflectorLocation.X = playerLocation.X - 5;
            reflectHitBox.X = (int)playerLocation.X - 5;
            shotMeterLocation.X = (int)playerLocation.X;
            aiRectangle.X = (int)playerLocation.X - 30;
        }
        public void CalculateProjectileOriginAndDirection()
        {
            if (playerIndex == PlayerIndex.One)
            {
                shooterMatrix = Matrix.CreateRotationZ(input.ReturnAngle()) * Matrix.CreateTranslation(reticalLocation.X, reticalLocation.Y, 0);
                projectileOrigin = Vector2.Transform(new Vector2(0, -playerRetical.Height), shooterMatrix);//transforms the vector BASED ON the location of the new world matrix
            }
            if(playerIndex == PlayerIndex.Two)
            {
                shooterMatrix = Matrix.CreateRotationZ(input.ReturnAngle()) * Matrix.CreateTranslation(reticalLocation.X, reticalLocation.Y, 0);
                projectileOrigin = Vector2.Transform(new Vector2(0, playerRetical.Height), shooterMatrix);
            }
        }
        public void CalculateRotationOrigin()
        {
            if(playerIndex == PlayerIndex.One)
            {
                rotationOrigin = new Vector2(playerRetical.Width / 2, playerRetical.Height); 
            }
            else if(playerIndex == PlayerIndex.Two)
            {
                rotationOrigin = new Vector2(playerRetical.Width / 2, 0);
            }
        }
        public void ShotMeter()
        {
            if (AI == true)
            {
                if (shotMeterCounter < playerSprite.Width)
                {
                    shotMeterCounter += .2f;
                }
                if (inputAction == InputAction.Charge || inputAction == InputAction.ChargeShotReady)
                {
                    if (shotMeterCounter < 10)
                    {
                        inputAction = InputAction.None;
                    }
                }
                if (inputAction == InputAction.Shoot)
                {
                    if (shotMeterCounter < 10)
                    {
                        inputAction = InputAction.None;
                    }
                    else
                    {
                        shotMeterCounter -= 10;
                    }
                }
                if (inputAction == InputAction.ChargeShot)
                {
                    if (shotMeterCounter < 20)
                    {
                        inputAction = InputAction.None;
                    }
                    else
                    {
                        shotMeterCounter -= 20;
                    }
                }
            }
            else
            {
                if (shotMeterCounter < playerSprite.Width)
                {
                    shotMeterCounter += .2f;
                }
                if (input.inputAction == InputAction.Charge || input.inputAction == InputAction.ChargeShotReady)
                {
                    if (shotMeterCounter < 10)
                    {
                        input.inputAction = InputAction.None;
                    }
                }
                if (input.inputAction == InputAction.Shoot)
                {
                    if (shotMeterCounter < 10)
                    {
                        input.inputAction = InputAction.None;
                    }
                    else
                    {
                        shotMeterCounter -= 10;
                    }
                }
                if (input.inputAction == InputAction.ChargeShot)
                {
                    if (shotMeterCounter < 20)
                    {
                        input.inputAction = InputAction.None;
                    }
                    else
                    {
                        shotMeterCounter -= 20;
                    }
                }
            }
        }
    }
}
