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
    class Options
    {
        public GameStates currentGameState { get; set; }
        ContentManager content;
        GraphicsDeviceManager graphics;

        Texture2D placeholderMessage;

        Input input;

        public Options(GameStates currentGameState, ContentManager content, GraphicsDeviceManager graphics)
        {
            this.currentGameState = currentGameState;
            this.content = content;
            this.graphics = graphics;
            input = new Input(ControlType.Menu, PlayerIndex.One);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(placeholderMessage, new Vector2(0,0), Color.White);
        }
        public void Update(GameTime gameTime)
        {
            input.Update(gameTime);
            if(input.inputAction == InputAction.Back)
            {
                currentGameState = GameStates.Menu;
            }
        }
        public void LoadContent()
        {
            placeholderMessage = content.Load<Texture2D>("sprites/options screen");
        }
        public void UnloadContent()
        {
            content.Unload();
        }
    }
}
