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
    class SinglePlayer
    {
        public GameStates currentGameState;

        ContentManager content;
        GraphicsDeviceManager graphics;
        SpriteFont font;
        Texture2D endGameScreen;

        public bool active = false;

        Boundary boundary;

        GameClock gameClock;
        GameLoopLogic gameLoopLogic;

        public SinglePlayer(GameStates currentGameState, ContentManager content, GraphicsDeviceManager graphics)
        {
            this.content = content;
            this.graphics = graphics;
            this.currentGameState = currentGameState;
            boundary = new Boundary(content, graphics);
            gameLoopLogic = new GameLoopLogic(content, graphics, boundary);

            gameLoopLogic.players.Add(new Player(ControlType.GamePlay, PlayerIndex.One, content, graphics, this.currentGameState));
            gameLoopLogic.players.Add(new Player(ControlType.GamePlay, PlayerIndex.Two, content, graphics, GameStates.SinglePlayer));
            gameClock = new GameClock(graphics, content);
            active = true;
        }
        public void Reset()
        {
            foreach (Player player in gameLoopLogic.players)
            {
                player.health = 3;
                player.score = 0;
                player.input.controlType = ControlType.GamePlay;
                player.input.inputAction = InputAction.None;
            }

            currentGameState = GameStates.SinglePlayer;
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
            foreach (Player player in gameLoopLogic.players)
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
                if (gameClock.gameClock == 0)
                {
                    gameLoopLogic.TimeUpConditions(gameTime);
                    
                    gameLoopLogic.players[0].Update(gameTime);
                    gameLoopLogic.players[0].input.controlType = ControlType.Menu;
                    if (gameLoopLogic.players[0].inputAction == InputAction.Confirm)
                    {
                        currentGameState = GameStates.Menu;
                    }
                    active = false;
                }
                else if (gameLoopLogic.players[0].health == 0 || gameLoopLogic.players[1].health == 0)
                {
                    gameLoopLogic.PlayerDeathConditions(gameTime);

                    gameLoopLogic.players[0].Update(gameTime);
                    gameLoopLogic.players[0].input.controlType = ControlType.Menu;
                    if (gameLoopLogic.players[0].inputAction == InputAction.Confirm)
                    {
                        currentGameState = GameStates.Menu;
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
                spriteBatch.Draw(endGameScreen, new Vector2(0, 0), Color.White);
                spriteBatch.DrawString(font, gameLoopLogic.winnerText, new Vector2(graphics.PreferredBackBufferWidth / 2 - (font.Texture.Width / 2), graphics.PreferredBackBufferHeight / 2), Color.White);
            }
        }
    }
}
