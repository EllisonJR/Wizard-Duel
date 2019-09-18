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
    class PotionSmoke
    {
        Vector2 spawnLoc;

        public Animation smoke;

        public float smokespeed;

        int timer;
        int choice;

        public bool shrink = false;

        public PotionSmoke(Vector2 spawnLoc, Texture2D smoke1, Texture2D smoke2)
        {
            this.spawnLoc = spawnLoc;

            Random random = new Random();

            choice = random.Next(0,2);

            if (choice == 0)
            {
                this.smoke = new Animation(smoke1, 2, 5);
            }
            if (choice == 1)
            {
                this.smoke = new Animation(smoke2, 2, 5);
            }
            smoke.frameTime = 100;

            choice = random.Next(0, 3);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (shrink == true)
            {
                smoke.Draw(spriteBatch, spawnLoc);
            }
        }
        public void Update(GameTime gameTime)
        {
            timer += gameTime.ElapsedGameTime.Milliseconds;
            if(timer < 50)
            {
                smokespeed = 5;
            }
            if(timer > 49 && timer < 80)
            {
                smokespeed = 2.5f;
            }
            if(timer > 79)
            {
                smokespeed = 1f;
            }
            smoke.Update(gameTime);
            spawnLoc.Y -= smokespeed;
            if(choice == 0)
            {
                spawnLoc.X -= .5f;
            }
            if(choice == 1)
            {
                spawnLoc.X += .5f;
            }
        }
    }
}
