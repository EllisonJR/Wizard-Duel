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
    class Menu
    {
        ContentManager content;
        GraphicsDeviceManager graphics;
        Input input;

        public GameStates currentGamestate { get; set; }

        int menuPointer;
        Vector2 pointerPosition;
        double UItimer;

        Texture2D indicator;
        Texture2D play;
        Texture2D options;
        Texture2D exit;

        Vector2 playPlacement;
        Vector2 menuPlacement;
        Vector2 exitPlacement;
        Vector2 indicatorPlacement;

        public Menu(GameStates currentGameState, ContentManager content, GraphicsDeviceManager graphics)
        {
            this.currentGamestate = currentGameState;
            this.content = content;
            this.graphics = graphics;
            input = new Input(ControlType.Menu, PlayerIndex.One);
        }

        public void LoadContent()
        {
            indicatorPlacement = new Vector2(playPlacement.X, playPlacement.Y);

            indicator = content.Load<Texture2D>("sprites/indicator");
            play = content.Load<Texture2D>("sprites/playbutton");
            options = content.Load<Texture2D>("sprites/optionsbutton");
            exit = content.Load<Texture2D>("sprites/exitbutton");

            playPlacement = new Vector2(CalculateTexturePlacement(indicator.Bounds.Width, indicator.Bounds.Height).X, CalculateTexturePlacement(indicator.Bounds.Width, indicator.Bounds.Height).Y);
            menuPlacement = new Vector2(CalculateTexturePlacement(indicator.Bounds.Width, indicator.Bounds.Height).X, CalculateTexturePlacement(indicator.Bounds.Width, indicator.Bounds.Height).Y + 50);
            exitPlacement = new Vector2(CalculateTexturePlacement(indicator.Bounds.Width, indicator.Bounds.Height).X, CalculateTexturePlacement(indicator.Bounds.Width, indicator.Bounds.Height).Y + 100);
        }

        public void UnloadContent()
        {
            content.Unload();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(play, playPlacement, Color.White);
            spriteBatch.Draw(options, menuPlacement, Color.White);
            spriteBatch.Draw(exit, exitPlacement, Color.White);
            switch (menuPointer)
            {
                case 0:
                    spriteBatch.Draw(indicator, playPlacement, Color.White);
                    break;
                case 1:
                    spriteBatch.Draw(indicator, menuPlacement, Color.White);
                    break;
                case 2:
                    spriteBatch.Draw(indicator, exitPlacement, Color.White);
                    break;
            }
        }

        public void Update(GameTime gameTime)
        {
            input.Update(gameTime);
            if (input.inputAction == InputAction.Down)
            {
                menuPointer++;
                if (menuPointer > 2)
                {
                    menuPointer = 0;
                }
            }
            if (input.inputAction == InputAction.Up)
            {
                menuPointer--;
                if (menuPointer < 0)
                {
                    menuPointer = 2;
                }
            }
            if (input.inputAction == InputAction.Confirm)
            {
                switch (menuPointer)
                {
                    
                    case 0:
                        currentGamestate = GameStates.GameMode1;
                        break;
                    case 1:
                        currentGamestate = GameStates.Options;
                        break;
                    case 2:
                        currentGamestate = GameStates.Exit;
                        break;
                        
                }
            }
        }

        public Vector2 CalculateTexturePlacement(int width, int height)
        {
            int Xplacement = graphics.PreferredBackBufferWidth / 2 - (width / 2);
            int Yplacement = graphics.PreferredBackBufferHeight / 2 - (height / 2);

            Vector2 placement = new Vector2(Xplacement, Xplacement);

            return placement;
        }
    }
}
