using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

public enum GameStates { MainMenu, ControlScreen, MultiPlayer, Exit, Paused, SinglePlayer, PlayerSelectSingle, PlayerSelectDouble}
public enum Characters { BuffWizard, TiredWizard}

namespace WizardDuel
{
    public class WizardDuel : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        GameStates currentGameState;
        GameStates previousGameState;

        MainMenu menu;
        Multiplayer multiPlayer;
        ControlInfoScreen controlInfoScreen;
        SinglePlayer singlePlayer;
        PlayererSelect playerSelect;

        SpriteFont font;

        bool importedChoices;

        public WizardDuel()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.GraphicsProfile = GraphicsProfile.HiDef;
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferredBackBufferWidth = 400;
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            currentGameState = GameStates.MainMenu;
            menu = new MainMenu(currentGameState, Content, graphics);
            multiPlayer = new Multiplayer(currentGameState, Content, graphics);
            controlInfoScreen = new ControlInfoScreen(currentGameState, Content, graphics);
            singlePlayer = new SinglePlayer(currentGameState, Content, graphics);
            playerSelect = new PlayererSelect(currentGameState, Content, graphics);

            base.Initialize();
            Window.Title = "Wizard Duel";
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("fonts/gameclock");

            switch (currentGameState)
            {
                case GameStates.MultiPlayer:
                    if (controlInfoScreen.outsideGamestate != GameStates.MultiPlayer)
                    {
                        multiPlayer.LoadContent();
                    }
                    break;
                case GameStates.MainMenu:
                    menu.LoadContent();
                    break;
                case GameStates.ControlScreen:
                    controlInfoScreen.LoadContent();
                    break;
                case GameStates.SinglePlayer:
                    if (controlInfoScreen.outsideGamestate != GameStates.SinglePlayer)
                    {
                        singlePlayer.LoadContent();
                    }
                    break;
                case GameStates.PlayerSelectSingle:
                case GameStates.PlayerSelectDouble:
                    playerSelect.LoadContent();
                    break;
            }
        }

        protected override void UnloadContent()
        {
            switch (currentGameState)
            {
                case GameStates.MultiPlayer:
                    if (currentGameState != GameStates.MultiPlayer)
                    {
                        multiPlayer.Unloadcontent();
                    }
                    break;
                case GameStates.MainMenu:
                    if(currentGameState != GameStates.MainMenu)
                    {
                        menu.UnloadContent();
                    }
                    break;
                case GameStates.ControlScreen:
                    if(currentGameState != GameStates.ControlScreen)
                    {
                        controlInfoScreen.UnloadContent();
                    }
                    break;
                case GameStates.SinglePlayer:
                    if(currentGameState != GameStates.SinglePlayer)
                    {
                        singlePlayer.Unloadcontent();
                    }
                    break;
            }
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            previousGameState = currentGameState;
            

            switch (currentGameState)
            {
                case GameStates.MultiPlayer:
                    if (importedChoices == false)
                    {
                        multiPlayer.CharacterChoices(playerSelect.PlayerOneChoice(), playerSelect.PlayerTwoChoice());
                        importedChoices = true;
                    }
                    controlInfoScreen.timer = 0;
                    controlInfoScreen.outsideGamestate = currentGameState;
                    multiPlayer.Update(gameTime);
                    menu.input.inputAction = InputAction.None;
                    playerSelect.Reset();
                    currentGameState = multiPlayer.currentGameState;
                    multiPlayer.currentGameState = currentGameState;
                    break;
                case GameStates.SinglePlayer:
                    if(importedChoices == false)
                    {
                        singlePlayer.CharacterChoices(playerSelect.PlayerOneChoice(), 2);
                        importedChoices = true;
                    }
                    controlInfoScreen.timer = 0;
                    controlInfoScreen.outsideGamestate = currentGameState;
                    singlePlayer.Update(gameTime);
                    menu.input.inputAction = InputAction.None;
                    playerSelect.Reset();
                    currentGameState = singlePlayer.currentGameState;
                    singlePlayer.currentGameState = currentGameState;
                    break;
                case GameStates.MainMenu:
                    menu.Update(gameTime);
                    controlInfoScreen.outsideGamestate = currentGameState;
                    controlInfoScreen.timer = 0;
                    if (multiPlayer.active == false)
                    {
                        multiPlayer.Reset();
                        multiPlayer.paused = false;
                    }
                    if (singlePlayer.active == false)
                    {
                        singlePlayer.Reset();
                        singlePlayer.paused = false;
                    }
                    playerSelect.active = false;
                    playerSelect.inputs.Clear();
                    currentGameState = menu.currentGamestate;
                    menu.currentGamestate = currentGameState;
                    break;
                case GameStates.ControlScreen:
                    menu.fromNonCombat = true;
                    singlePlayer.gameLoopLogic.backFromPause = true;
                    multiPlayer.gameLoopLogic.backFromPause = true;
                    menu.input.inputAction = InputAction.None;
                    controlInfoScreen.Update(gameTime);
                    if(controlInfoScreen.nonCombatTransition.currentFrame == 6 && controlInfoScreen.outsideGamestate == GameStates.SinglePlayer)
                    {
                        singlePlayer.gameLoopLogic.upwardSwipeTransition.currentFrame = 0;
                    }
                    if (controlInfoScreen.nonCombatTransition.currentFrame == 6 && controlInfoScreen.outsideGamestate == GameStates.MultiPlayer)
                    {
                        multiPlayer.gameLoopLogic.upwardSwipeTransition.currentFrame = 0;
                    }
                    currentGameState = controlInfoScreen.currentGameState;
                    controlInfoScreen.currentGameState = currentGameState;
                    break;
                case GameStates.PlayerSelectSingle:
                case GameStates.PlayerSelectDouble:
                    importedChoices = false;
                    singlePlayer.gameLoopLogic.backFromPause = true;
                    singlePlayer.gameLoopLogic.upwardSwipeTransition.currentFrame = 0;
                    if (multiPlayer.active == false)
                    {
                        multiPlayer.Reset();
                    }
                    if (singlePlayer.active == false)
                    {
                        singlePlayer.Reset();
                        singlePlayer.paused = false;
                    }
                    if(playerSelect.active == false)
                    {
                        playerSelect.Reset(currentGameState);
                        playerSelect.active = true;
                    }

                    playerSelect.Update(gameTime);
                    menu.fromNonCombat = true;
                    menu.input.inputAction = InputAction.None;
                    currentGameState = playerSelect.currentGameState;
                    playerSelect.currentGameState = currentGameState;
                    break;
            }

            menu.currentGamestate = currentGameState;
            multiPlayer.currentGameState = currentGameState;
            singlePlayer.currentGameState = currentGameState;
            controlInfoScreen.currentGameState = currentGameState;
            playerSelect.currentGameState = currentGameState;

            if (currentGameState != previousGameState)
            {
                LoadContent();
                
                UnloadContent();
            }

            if (currentGameState == GameStates.Exit)
            {
                Exit();
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            
            switch (currentGameState)
            {
                case GameStates.MultiPlayer:
                    multiPlayer.Draw(spriteBatch);
                    break;
                case GameStates.MainMenu:
                    menu.Draw(spriteBatch);
                    break;
                case GameStates.ControlScreen:
                    controlInfoScreen.Draw(spriteBatch);
                    break;
                case GameStates.SinglePlayer:
                    singlePlayer.Draw(spriteBatch);
                    break;
                case GameStates.PlayerSelectSingle:
                case GameStates.PlayerSelectDouble:
                    playerSelect.Draw(spriteBatch);
                    break;
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
