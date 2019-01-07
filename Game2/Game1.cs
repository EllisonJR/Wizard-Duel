using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum GameStates { Menu, Options, GameMode1, Exit, Paused}

namespace WizardDuel
{
    public class WizardDuel : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        GameStates currentGameState;
        GameStates previousGameState;

        Menu menu;
        GameMode1 gameMode1;
        Options options;

        public WizardDuel()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferredBackBufferWidth = 400;
        }

        protected override void Initialize()
        {
            currentGameState = GameStates.Menu;
            menu = new Menu(currentGameState, Content, graphics);
            gameMode1 = new GameMode1(currentGameState, Content, graphics);
            options = new Options(currentGameState, Content, graphics);

            base.Initialize();
            Window.Title = "Wizard Duel";
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            

            switch(currentGameState)
            {
                case GameStates.GameMode1:
                    gameMode1.LoadContent();
                    break;
                case GameStates.Menu:
                    menu.LoadContent();
                    break;
                case GameStates.Options:
                    options.LoadContent();
                    break;
            }
        }

        protected override void UnloadContent()
        {
            switch (currentGameState)
            {
                case GameStates.GameMode1:
                    if (currentGameState != GameStates.GameMode1)
                    {
                        gameMode1.Unloadcontent();
                    }
                    break;
                case GameStates.Menu:
                    if(currentGameState != GameStates.Menu)
                    {
                        menu.UnloadContent();
                    }
                    break;
                case GameStates.Options:
                    if(currentGameState != GameStates.Options)
                    {
                        options.UnloadContent();
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
                case GameStates.GameMode1:
                    gameMode1.Update(gameTime);
                    currentGameState = gameMode1.currentGameState;
                    break;
                case GameStates.Menu:
                    menu.Update(gameTime);
                    if (gameMode1.active == false)
                    {
                        gameMode1.Reset();
                    }
                    currentGameState = menu.currentGamestate;
                    break;
                case GameStates.Options:
                    options.Update(gameTime);
                    currentGameState = options.currentGameState;
                    break;
            }

            menu.currentGamestate = currentGameState;
            gameMode1.currentGameState = currentGameState;
            options.currentGameState = currentGameState;

            
            if (currentGameState != previousGameState)
            {
                LoadContent();
                UnloadContent();
            }
            
            if(currentGameState == GameStates.Exit)
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
                case GameStates.GameMode1:
                    gameMode1.Draw(spriteBatch);
                    break;
                case GameStates.Menu:
                    menu.Draw(spriteBatch);
                    break;
                case GameStates.Options:
                    options.Draw(spriteBatch);
                    break;
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
