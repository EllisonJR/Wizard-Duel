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
    class WallSegment
    {
        ContentManager content;
        GraphicsDeviceManager graphics;

        Texture2D brickT;
        public Animation brick;
        public Rectangle brickRect;
        public Vector2 brickLocation;

        public Vector2 brickDebrisLoc;
        Texture2D brickDebrisT;
        public Animation brickDebris;

        public int health;

        public PlayerIndex playerIndex;

        public int index;

        public bool collisionOff;

        int shakeX;
        int shakeY;

        public int timer;

        public WallSegment(ContentManager content, GraphicsDeviceManager graphics)
        {
            this.content = content;
            this.graphics = graphics;

            brickT = content.Load<Texture2D>("sprites/border items/breakwall");
            brick = new Animation(brickT, 4, 4);

            brickDebrisT = content.Load<Texture2D>("sprites/border items/brickdebris");
            brickDebris = new Animation(brickDebrisT, 2, 4);
        }
        public void PlaceBricks(int brickLocationX, int brickLocationY, PlayerIndex playerIndex, int index)
        {
            brickLocation = new Vector2(brickLocationX, brickLocationY);
            brickRect = new Rectangle(brickLocationX, brickLocationY, brick.width, brick.height);
            this.playerIndex = playerIndex;
            this.index = index;
            health = 10;
            collisionOff = false;

            shakeX = 0;
            shakeY = 0;
            timer = 300;

            if (index <= 7)
            {
                brickDebrisLoc = new Vector2(brickLocationX, brickLocationY - brickRect.Height);
            }
            if (index > 7)
            {
                brickDebrisLoc = new Vector2(brickLocationX, brickLocationY + brickRect.Height);
            }
            brickDebris.currentFrame = -1;
        }
        public void Reset()
        {
            health = 10;
        }
        public void Update(GameTime gameTime)
        {
            BrickShaker(gameTime);
            if (brickDebris.currentFrame >= 0)
            {
                brickDebris.Update(gameTime);
            }
            brickLocation.X += shakeX;
            brickLocation.Y += shakeY;
            if (playerIndex == PlayerIndex.One)
            {
                if (health == 10)
                {
                    brick.currentFrame = 0;
                }
                if (health == 9 || health == 8)
                {
                    brick.currentFrame = 1;
                }
                if (health == 7 || health == 6)
                {
                    brick.currentFrame = 2;
                }
                if (health == 5 || health == 4)
                {
                    brick.currentFrame = 3;
                }
                if (health == 3 || health == 2)
                {
                    brick.currentFrame = 4;
                }
                if (health == 1)
                {
                    brick.currentFrame = 5;
                }
                if (health == 0)
                {
                    brick.currentFrame = 6;
                    collisionOff = true;
                }
            }
            else
            {
                if (health == 10)
                {
                    brick.currentFrame = 7;
                }
                if (health == 9 || health == 8)
                {
                    brick.currentFrame = 8;
                }
                if (health == 7 || health == 6)
                {
                    brick.currentFrame = 9;
                }
                if (health == 5 || health == 4)
                {
                    brick.currentFrame = 10;
                }
                if (health == 3 || health == 2)
                {
                    brick.currentFrame = 11;
                }
                if (health == 1)
                {
                    brick.currentFrame = 12;
                }
                if (health == 0)
                {
                    brick.currentFrame = 13;
                    collisionOff = true;
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            brick.Draw(spriteBatch, brickLocation);
            if (brickDebris.currentFrame >= 0)
            {
                brickDebris.Draw(spriteBatch, brickDebrisLoc);
            }
        }
        public void Debris()
        {
            if(timer >= 0 && timer < 50)
            {
                if(index <= 7)
                {
                    brickDebris.currentFrame = 0;
                }
                if(index > 7)
                {
                    brickDebris.currentFrame = 4;
                }
            }
            if (brickDebris.currentFrame == 3 || brickDebris.currentFrame == 7)
            {
                brickDebris.currentFrame = -1;
            }
        }
        public void BrickShaker(GameTime gameTime)
        {
            timer += gameTime.ElapsedGameTime.Milliseconds;
            Debris();
            if(timer >= 0 && timer < 50)
            {
                //shakeX = 3;
                shakeY = 3;
            }
            if (timer >= 50 && timer < 100)
            {
                //shakeX = -3;
                shakeY = -3;
            }
            if (timer >= 100 && timer < 150)
            {
                //shakeX = 2;
                shakeY = 2;
            }
            if (timer >= 150 && timer < 200)
            {
                //shakeX = -2;
                shakeY = -2;
            }
            if (timer >= 200 && timer < 250)
            {
                //shakeX = 1;
                shakeY = 1;
            }
            if (timer >= 250 && timer < 300)
            {
                //shakeX = -1;
                shakeY = -1;
            }
            if(timer >= 300)
            {
                //shakeX = 0;
                shakeY = 0;
            }
        }
    }
}
