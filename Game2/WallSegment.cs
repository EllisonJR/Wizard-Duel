﻿using System;
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
    class WallSegment : AssetContainer
    {
        public Animation brick;
        public Animation brickDebris;

        public Rectangle brickRect;

        public Vector2 brickLocation;
        public Vector2 brickDebrisLoc;

        public int health;
        public int index;

        public PlayerIndex playerIndex;

        public bool collisionOff;

        int shakeX;
        int shakeY;

        public int timer;

        public WallSegment()
        {
            brick = new Animation(brickT, 4, 4);
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
            BrickHealthLogic();
            if (brickDebris.currentFrame >= 0)
            {
                brickDebris.Update(gameTime);
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
        void BrickHealthLogic()
        {
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
        void Debris()
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
        void BrickShaker(GameTime gameTime)
        {
            timer += gameTime.ElapsedGameTime.Milliseconds;
            Debris();
            brickLocation.X += shakeX;
            brickLocation.Y += shakeY;
            if (timer >= 0 && timer < 50)
            {
                shakeY = 3;
            }
            if (timer >= 50 && timer < 100)
            {
                shakeY = -3;
            }
            if (timer >= 100 && timer < 150)
            {
                shakeY = 2;
            }
            if (timer >= 150 && timer < 200)
            {
                shakeY = -2;
            }
            if (timer >= 200 && timer < 250)
            {
                shakeY = 1;
            }
            if (timer >= 250 && timer < 300)
            {
                shakeY = -1;
            }
            if(timer >= 300)
            {
                shakeY = 0;
            }
        }
    }
}
