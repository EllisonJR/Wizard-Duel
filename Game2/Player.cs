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
        ContentManager content;
        GraphicsDeviceManager graphics;
        public PlayerIndex playerIndex { get; }
        
        int playerSpeed;

        Texture2D playerSprite;
        Texture2D playerRetical;
        Texture2D playerReflect;
        Texture2D playerHP;
        Texture2D playerGoal;
        Rectangle hitBox;
        public Rectangle reflectHitBox;

        Vector2 rotationOrigin;
        public Vector2 projectileOrigin { get; set; }
        public Vector2 playerLocation;
        Vector2 reticalLocation;
        Vector2 reflectorLocation;

        Matrix shooterMatrix;

        Input input;

        public int health { get; set; }
        public int score { get; set; }

        public float shootingAngle { get; set; }

        public InputAction inputAction { get; set; }
       
        public Player(ControlType controlType, PlayerIndex playerIndex, ContentManager content, GraphicsDeviceManager graphics)
        {
            input = new Input(controlType, playerIndex);
            this.content = content;
            this.graphics = graphics;
            this.playerIndex = playerIndex;
            health = 3;
            score = 0;
            playerSpeed = 5;
        }
        public void LoadContent()
        {
            playerSprite = content.Load<Texture2D>("sprites/player");
            playerRetical = content.Load<Texture2D>("sprites/retical");
            playerReflect = content.Load<Texture2D>("sprites/reflecter");

            if (playerIndex == PlayerIndex.One)
            {
                playerLocation = new Vector2((graphics.PreferredBackBufferWidth / 2) - (playerSprite.Width / 2), graphics.PreferredBackBufferHeight - playerSprite.Height - 50);
                reticalLocation = new Vector2((playerLocation.X + playerSprite.Width / 2), playerLocation.Y);
                reflectorLocation = new Vector2(playerLocation.X - (playerSprite.Width / 2) - 5, playerLocation.Y - 10);
                hitBox = new Rectangle((int)playerLocation.X, (int)playerLocation.Y, playerSprite.Width, playerSprite.Height);
                reflectHitBox = new Rectangle((int)reflectorLocation.X, (int)reflectorLocation.Y, playerReflect.Width, playerReflect.Height + 5);
            }
            else if (playerIndex == PlayerIndex.Two)
            {
                playerLocation = new Vector2((graphics.PreferredBackBufferWidth / 2) - (playerSprite.Width / 2), +50);
                reticalLocation = new Vector2(playerLocation.X + (playerSprite.Width / 2), playerLocation.Y + playerSprite.Height);
                reflectorLocation = new Vector2(playerLocation.X - (playerSprite.Width / 2) - 5, playerLocation.Y + 5);
                hitBox = new Rectangle((int)playerLocation.X, (int)playerLocation.Y, playerSprite.Width, playerSprite.Height);
                reflectHitBox = new Rectangle((int)reflectorLocation.X, (int)reflectorLocation.Y + playerSprite.Height, playerReflect.Width, playerReflect.Height + 5);
            }

            
        }
        public void UnloadContent()
        {
            content.Unload();
        }
        public void Update(GameTime gameTime)
        {
            input.Update(gameTime);
            
            inputAction = input.inputAction;
            PlayerMovement(gameTime);
            CalculateRotationOrigin();
            CalculateProjectileOriginAndDirection();
            shootingAngle = input.ReturnAngle();
            if (playerIndex == PlayerIndex.One)
            {
                Debug.WriteLine(playerSpeed);
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(playerSprite, playerLocation, Color.White);

            if (input.inputAction == InputAction.Reflect)
            {
                spriteBatch.Draw(playerReflect, reflectHitBox, Color.White);
            }
            else
            {

            }
            if(input.inputAction == InputAction.Charge || input.inputAction == InputAction.ChargeShotReady)
            {
                spriteBatch.Draw(playerRetical, reticalLocation, null, Color.White, (float)input.ReturnAngle(), rotationOrigin, 1f, SpriteEffects.None, 1f);
            }
            else
            {

            }
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
    }
}
