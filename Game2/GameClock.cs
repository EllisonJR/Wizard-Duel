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
    class GameClock
    {
        GameStates gameState;
        GraphicsDeviceManager graphics;
        SpriteFont font;

        public int gameClock { get; set; }
        public int startClock { get; set; }
        double currentTime;

        public GameClock(GraphicsDeviceManager graphics, ContentManager content)
        {
            this.graphics = graphics;
            font = content.Load<SpriteFont>("fonts/gameclock");

            gameClock = 60;
            startClock = 3;
            currentTime = 1f;
        }
        public void GameMode1Clock(GameTime gameTime)
        {
            currentTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (currentTime < 0)
            {
                if(startClock > -1)
                {
                    startClock--;
                    currentTime =1;
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
            if (currentGameState == GameStates.GameMode1 || currentGameState == GameStates.SinglePlayer)
            {
                gameClock = 60;
                startClock = 3;
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
            if (startClock > -1)
            {
                if (startClock == 0)
                {
                    spriteBatch.DrawString(font, "Go!", new Vector2(graphics.PreferredBackBufferWidth / 2 - 10, graphics.PreferredBackBufferHeight / 2), Color.White);
                }
                else if (startClock > 0)
                {
                    spriteBatch.DrawString(font, "" + startClock, new Vector2(graphics.PreferredBackBufferWidth / 2 - 10, graphics.PreferredBackBufferHeight / 2), Color.White);
                }
                else
                {

                }
            }
        }
    }
}
