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
    class Boundary
    {
        GraphicsDeviceManager graphics;
        ContentManager content;

        Texture2D border { get; set; }
        Vector2 borderLocation;

        public Boundary(ContentManager content, GraphicsDeviceManager graphics)
        {
            this.content = content;
            this.graphics = graphics;
        }
        public void Loadcontent()
        {
            border = content.Load<Texture2D>("sprites/border");
            borderLocation = new Vector2(graphics.PreferredBackBufferWidth / 2 - border.Width / 2, graphics.PreferredBackBufferHeight / 2 - border.Bounds.Height / 2);

        }
        public void UnloadContent()
        {
            content.Unload();
        }
        public void Update(GameTime gameTime)
        {

        }
        public void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
