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
    class GameClock : AssetContainer
    {
        GameStates gameState;
        GraphicsDeviceManager graphics;

        public int gameClock { get; set; }
        public int startClock { get; set; }
        double currentTime;

        public Animation countDown;

        Vector2 center;

        public GameClock(GraphicsDeviceManager graphics)
        {
            this.graphics = graphics;
            
            countDown = new Animation(countDownT, 5, 8);

            center = new Vector2(graphics.PreferredBackBufferWidth / 2 - countDown.width / 2, graphics.PreferredBackBufferHeight / 2 - countDown.height / 2);

            gameClock = 60;
            startClock = 3;
            currentTime = 1f;
        }
        public void GameMode1Clock(GameTime gameTime)
        {
            currentTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            countDown.Update(gameTime);
            if (currentTime < 0)
            {
                if(startClock > -1)
                {
                    startClock--;
                    currentTime = 1;
                }
                if(startClock <= -1)
                {
                    if (gameClock != 0)
                    {
                        gameClock--;
                        currentTime = 1;
                    }
                    else if(gameClock == 0)
                    {

                    }
                }
            }
        }
        public void ResetClock(GameStates currentGameState)
        {
            if (currentGameState == GameStates.MultiPlayer || currentGameState == GameStates.SinglePlayer)
            {
                gameClock = 60;
                startClock = 3;
                countDown.currentFrame = 0;
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (gameClock < 10)
            {
                spriteBatch.DrawString(font, "00:0" + gameClock, new Vector2(graphics.PreferredBackBufferWidth - 90, (graphics.PreferredBackBufferHeight / 2)), Color.White);
            }
            else
            {
                spriteBatch.DrawString(font, "00:" + gameClock, new Vector2(graphics.PreferredBackBufferWidth - 90, (graphics.PreferredBackBufferHeight / 2)), Color.White);
            }
            if (startClock > -2)
            {
                countDown.Draw(spriteBatch, center);
            }
        }
    }
}
