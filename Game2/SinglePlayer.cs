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
    class SinglePlayer
    {
        public GameStates currentGameState;

        ContentManager content;
        GraphicsDeviceManager graphics;
        SpriteFont font;

        public bool active = false;

        Boundary boundary;

        GameClock gameClock;
        public GameLoopLogic gameLoopLogic;

        public int loopCounter;

        public bool paused = false;

        public int playerOneChoice;
        public int playerTwoChoice;
        
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
        public void CharacterChoices(int choice1, int choice2)
        {
            gameLoopLogic.players[0].ResetPlayers(PlayerIndex.One, choice1);
            gameLoopLogic.players[1].ResetPlayers(PlayerIndex.Two, choice2);
            playerOneChoice = choice1;
            playerTwoChoice = choice2;
        }
        public void Reset()
        {
            foreach (Player player in gameLoopLogic.players)
            {
                player.health = 3;
                player.score = 0;
                player.input.controlType = ControlType.GamePlay;
                player.inputAction = InputAction.None;
                player.tiredWizardCloud = false;
                if(player.playerIndex == PlayerIndex.Two)
                {
                    player.AI = true;
                }
            }
            gameLoopLogic.players[0].ResetPlayers(PlayerIndex.One, playerOneChoice);
            gameLoopLogic.players[1].ResetPlayers(PlayerIndex.Two, playerTwoChoice);
            gameLoopLogic.playerWin.currentFrame = 0;
            loopCounter = 0;
            currentGameState = GameStates.SinglePlayer;
            gameClock.ResetClock(currentGameState);
            foreach(WallSegment brick in gameLoopLogic.bricks)
            {
                brick.brick.currentFrame = 0;
                brick.Reset();
            }
            active = true;

            gameLoopLogic.projectiles.Clear();
            gameLoopLogic.fireballImpacts.Clear();
        }
        public void LoadContent()
        {
            font = content.Load<SpriteFont>("fonts/gameclock");
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
            
            if (gameLoopLogic.upwardSwipeTransition.currentFrame == -1 && gameClock.startClock > -1)
            {
                gameClock.GameMode1Clock(gameTime);
            }
            else
            {

                if (gameClock.gameClock == 0)
                {
                    gameLoopLogic.TimeUpConditions(gameTime);

                    gameLoopLogic.players[0].input.Update(gameTime);
                    gameLoopLogic.players[0].input.controlType = ControlType.Menu;
                    if (gameLoopLogic.players[0].input.inputAction == InputAction.Confirm)
                    {
                        currentGameState = GameStates.MainMenu;
                    }
                    active = false;
                }
                else if (gameLoopLogic.players[0].health == 0 || gameLoopLogic.players[1].health == 0)
                {
                    gameLoopLogic.PlayerDeathConditions(gameTime);

                    gameLoopLogic.players[0].input.Update(gameTime);
                    gameLoopLogic.players[0].input.controlType = ControlType.Menu;
                    if (gameLoopLogic.players[0].input.inputAction == InputAction.Confirm)
                    {
                        currentGameState = GameStates.MainMenu;
                    }
                    active = false;
                }
                else
                {
                    if (gameLoopLogic.paused != true)
                    {
                        gameClock.GameMode1Clock(gameTime);
                    }
                    gameLoopLogic.UpdateTransitions(gameTime);
                    if (gameClock.startClock < 0)
                    {
                        gameLoopLogic.ListChecks(gameTime);
                    }
                }
            }
            PauseTransitions(gameTime);
        }
        public void PauseTransitions(GameTime gameTime)
        {
            if(gameClock.startClock == 3)
            {
                if (gameLoopLogic.upwardSwipeTransition.currentFrame > -1)
                {
                    gameLoopLogic.upwardSwipeTransition.Update(gameTime);
                }
                if (gameLoopLogic.nonCombatTransition.currentFrame == 6)
                {
                    gameLoopLogic.upwardSwipeTransition.currentFrame = -1;
                }
            }
            if (gameLoopLogic.paused == true)
            {
                if (gameLoopLogic.menuPointer == 1)
                {
                    if (gameLoopLogic.nonCombatTransition.currentFrame == 6)
                    {
                        Reset();
                        gameLoopLogic.paused = false;
                        gameLoopLogic.nonCombatTransition.currentFrame = -1;
                        gameLoopLogic.upwardSwipeTransition.currentFrame = 0;
                    }
                }
                if (gameLoopLogic.menuPointer == 2)
                {
                    if (gameLoopLogic.nonCombatTransition.currentFrame == 6)
                    {
                        currentGameState = GameStates.ControlScreen;
                        gameLoopLogic.nonCombatTransition.currentFrame = -1;
                    }
                }
                if (gameLoopLogic.menuPointer == 3)
                {
                    if (gameLoopLogic.nonCombatTransition.currentFrame == 6)
                    {
                        currentGameState = GameStates.MainMenu;
                        active = false;
                        gameLoopLogic.paused = false;
                        gameLoopLogic.nonCombatTransition.currentFrame = -1;
                    }
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            gameClock.Draw(spriteBatch);
            boundary.Draw(spriteBatch);
            gameLoopLogic.DrawLists(spriteBatch);
            if(gameClock.gameClock == 0)
            {
                gameLoopLogic.DrawTie(spriteBatch);
            }
        }
    }
}
