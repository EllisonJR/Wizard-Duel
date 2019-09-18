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
    class PotionSpawner : AssetContainer
    {
        Animation potionBall;
        public Animation potionFire;
        Animation potionIcon;
        Animation potionExplosionGlow;

        int potionRandomizer;

        Vector2 zeroLoc;

        int timeToNextPotion;
        public int potionChoice;
        int potionTimer;

        bool animationOn;

        Random potionRandom;
        Random timerRandom;

        public PotionSpawner()
        {
            zeroLoc = new Vector2(0, 0);
            animationOn = false;

            potionBall = new Animation(potionIntroBallT, 6, 10);
            potionFire = new Animation(potionIntroFireT, 6, 10);
            potionIcon = new Animation(potionIntroIconT, 6, 10);
            potionExplosionGlow = new Animation(potionExplosionGlowT, 3, 2);

            potionBall.frameTime = 75;
            potionFire.frameTime = 75;
            potionIcon.frameTime = 75;
            potionExplosionGlow.frameTime = 75;

            potionFire.fire = true;
            potionExplosionGlow.fire = true;
            potionBall.rainbowRapid = true;

            potionExplosionGlow.currentFrame = -1;
            timeToNextPotion = 5000;

            potionRandom = new Random();
            timerRandom = new Random();
        }
        public void Update(GameTime gameTime)
        {
            if(animationOn == false)
            {
                potionTimer += gameTime.ElapsedGameTime.Milliseconds;
                potionChoice = potionRandom.Next(5, 5);
            }
            if(potionFire.currentFrame == 54)
            {
                potionFire.currentFrame = 0;
                potionBall.currentFrame = 0;
                potionIcon.currentFrame = 0;
            }
            
            if(potionTimer >= timeToNextPotion)
            {
                animationOn = true;
                potionTimer = 0;
            }
            if(animationOn == true)
            {
                potionFire.Update(gameTime);
                potionBall.Update(gameTime);
                potionIcon.Update(gameTime);
            }
            if(potionFire.currentFrame == 49)
            {
                potionExplosionGlow.currentFrame = 0;
            }
            if(potionExplosionGlow.currentFrame != -1)
            {
                potionExplosionGlow.Update(gameTime);
            }
            if(potionExplosionGlow.currentFrame == 6)
            {
                potionExplosionGlow.currentFrame = -1;
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if(animationOn == true)
            {
                potionFire.Draw(spriteBatch, zeroLoc);
                potionBall.Draw(spriteBatch, zeroLoc);
                potionIcon.Draw(spriteBatch, zeroLoc);
                if(potionIcon.currentFrame == 54)
                {
                    timeToNextPotion = timerRandom.Next(100000, 210000);
                    animationOn = false;
                }
                if(potionExplosionGlow.currentFrame != -1)
                {
                    potionExplosionGlow.Draw(spriteBatch, zeroLoc);
                }
            }
        }
    }
}
