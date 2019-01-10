using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WizardDuel
{
    class GameClock
    {
        GameStates gameState;

        public int gameClock { get; set; }
        public int startClock { get; set; }
        double currentTime;
        public float millisecondTimer { get; set; }

        public GameClock()
        {
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
                    else
                    {

                    }
                }
            }
        }
        public void MillisecondTimer(GameTime gameTime)
        {
            millisecondTimer += gameTime.ElapsedGameTime.Milliseconds;
        }
        public void ResetMillisecondTimer()
        {
            millisecondTimer = 0;
        }
        public void ResetClock(GameStates currentGameState)
        {
            if (currentGameState == GameStates.GameMode1)
            {
                gameClock = 60;
                startClock = 3;
            }
        }
    }
}
