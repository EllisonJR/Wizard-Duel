using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        Texture2D playerSprite;
        Texture2D playerRetical;
        Texture2D playerReflect;
        Texture2D playerHP;
        Texture2D playerGoal;
        Rectangle hitBox;

        Vector2 rotationOrigin;
        public Vector2 projectileOrigin { get; set; }
        public Vector2 playerLocation;
        Vector2 reticalLocation;

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
        }
        public void LoadContent()
        {
            playerSprite = content.Load<Texture2D>("sprites/player");
            playerRetical = content.Load<Texture2D>("sprites/retical");
            if (playerIndex == PlayerIndex.One)
            {
                playerLocation = new Vector2((graphics.PreferredBackBufferWidth / 2) - (playerSprite.Width / 2), graphics.PreferredBackBufferHeight - playerSprite.Height - 50);
                reticalLocation = new Vector2((playerLocation.X + playerSprite.Width / 2), playerLocation.Y);
                hitBox = new Rectangle((int)playerLocation.X, (int)playerLocation.Y, playerSprite.Width, playerSprite.Height);
            }
            else if (playerIndex == PlayerIndex.Two)
            {
                playerLocation = new Vector2((graphics.PreferredBackBufferWidth / 2) - (playerSprite.Width / 2), +50);
                reticalLocation = new Vector2(playerLocation.X + (playerSprite.Width / 2), playerLocation.Y + playerSprite.Height);
                hitBox = new Rectangle((int)playerLocation.X, (int)playerLocation.Y, playerSprite.Width, playerSprite.Height);
            }
        }
        public void UnloadContent()
        {
            content.Unload();
        }
        public void Update(GameTime gameTime)
        {
            input.Update(gameTime);
            this.inputAction = input.inputAction;
            PlayerMovement();
            CalculateRotationOrigin();
            CalculateProjectileOriginAndDirection();
            shootingAngle = input.ReturnAngle();
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(playerSprite, playerLocation, Color.White);
            spriteBatch.Draw(playerRetical, reticalLocation, null, Color.White, (float)input.ReturnAngle(), rotationOrigin, 1f, SpriteEffects.None, 1f);
        }
        public void PlayerMovement()
        {
            if (input.inputAction == InputAction.Left)
            {
                playerLocation.X -= 5;
            }
            else if (input.inputAction == InputAction.Right)
            {
                playerLocation.X += 5;
            }
            if (playerLocation.X > graphics.PreferredBackBufferWidth - playerSprite.Width - 25)
            {
                playerLocation.X = graphics.PreferredBackBufferWidth - playerSprite.Width - 25;
            }
            if (playerLocation.X < 25)
            {
                playerLocation.X = 25;
            }
            LockHitBoxAndRetical();
        }
        public void LockHitBoxAndRetical()
        {
            if (playerIndex == PlayerIndex.One)
            {
                reticalLocation.X = playerLocation.X + (playerSprite.Width / 2);
                hitBox.X = (int)playerLocation.X;
            }
            else if (playerIndex == PlayerIndex.Two)
            {
                reticalLocation.X = playerLocation.X + (playerSprite.Width / 2);
                hitBox.X = (int)playerLocation.X;
            }
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
