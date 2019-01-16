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
    class GameMode1
    {
        public GameStates currentGameState { get; set; }

        ContentManager content;
        GraphicsDeviceManager graphics;
        SpriteFont font;
        Texture2D endGameScreen;

        public bool active = false;

        Boundary boundary;

        GameClock gameClock;
        GameLoopLogic gameLoopLogic;

        public GameMode1(GameStates currentGameState, ContentManager content, GraphicsDeviceManager graphics)
        {
            this.content = content;
            this.graphics = graphics;
            this.currentGameState = currentGameState;
            boundary = new Boundary(content, graphics);
            gameLoopLogic = new GameLoopLogic(content, graphics, boundary);

            gameLoopLogic.players.Add(new Player(ControlType.GamePlay, PlayerIndex.One, content, graphics));
            gameLoopLogic.players.Add(new Player(ControlType.GamePlay, PlayerIndex.Two, content, graphics));
            gameClock = new GameClock(graphics, content);
            active = true;
        }
        public void Reset()
        {
            foreach(Player player in gameLoopLogic.players)
            {
                player.health = 3;
                player.score = 0;
                player.input.controlType = ControlType.GamePlay;
            }
            
            currentGameState = GameStates.GameMode1;
            gameClock.ResetClock(currentGameState);

            active = true;

            gameLoopLogic.projectiles.Clear();
        }
        public void LoadContent()
        {
            font = content.Load<SpriteFont>("fonts/gameclock");
            endGameScreen = content.Load<Texture2D>("sprites/endgamebackground");
            boundary.Loadcontent();
            foreach (Player player in gameLoopLogic.players)
            {
                player.LoadContent();
            }
        }
        public void Unloadcontent()
        {
            content.Unload();
            boundary.UnloadContent();
            foreach(Player player in gameLoopLogic.players)
            {
                player.UnloadContent();
            }
        }
        public void Update(GameTime gameTime)
        {
            
            if (gameClock.startClock > -1)
            {
                gameClock.GameMode1Clock(gameTime);
            }
            else
            {
                if (gameClock.gameClock ==  0)
                {
                    gameLoopLogic.TimeUpConditions(gameTime);
                    foreach (Player player in gameLoopLogic.players)
                    {
                        player.Update(gameTime);
                        player.input.controlType = ControlType.Menu;
                        if (player.inputAction == InputAction.Confirm)
                        {
                            currentGameState = GameStates.Menu;
                        }
                    }
                    active = false;
                }
                else if(gameLoopLogic.players[0].health == 0 || gameLoopLogic.players[1].health == 0)
                {
                    gameLoopLogic.PlayerDeathConditions(gameTime);
                    foreach (Player player in gameLoopLogic.players)
                    {
                        player.Update(gameTime);
                        player.input.controlType = ControlType.Menu;
                        if (player.inputAction == InputAction.Confirm)
                        {
                            currentGameState = GameStates.Menu;
                        }
                    }
                    active = false;
                }
                else
                {
                    gameClock.GameMode1Clock(gameTime);
                    gameLoopLogic.ListChecks(gameTime);
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            gameLoopLogic.DrawLists(spriteBatch);
            gameClock.Draw(spriteBatch);
            boundary.Draw(spriteBatch);
            
            if (gameLoopLogic.players[0].health == 0 || gameLoopLogic.players[1].health == 0 || gameClock.gameClock == 0)
            {
                spriteBatch.Draw(endGameScreen, new Vector2(0,0), Color.White);
                spriteBatch.DrawString(font, gameLoopLogic.winnerText, new Vector2(graphics.PreferredBackBufferWidth / 2 - (font.Texture.Width / 2), graphics.PreferredBackBufferHeight / 2), Color.White);
            }
        }
    }
}
