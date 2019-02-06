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
    public class Animation
    {
        public Texture2D spriteSheet { get; set; }
        public int rows { get; set; }
        public int columns { get; set; }
        public int currentFrame;
        int totalFrames;

        public Animation(Texture2D texture, int rows, int columns)
        {
            spriteSheet = texture;
            this.rows = rows;
            this.columns = columns;
            currentFrame = 0;
            totalFrames = rows * columns;
        }
        public void Update(GameTime gameTime)
        {
            currentFrame++;
        }
        public void Draw(SpriteBatch spriteBatch, Vector2 location)
        {
            int width = spriteSheet.Width / columns;
            int height = spriteSheet.Height / rows;
            int row = (int)((float)currentFrame / (float)columns);
            int column = currentFrame % columns;

            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
            Rectangle destinationRectangle = new Rectangle((int)location.X, (int)location.Y, width, height);

            spriteBatch.Draw(spriteSheet, destinationRectangle, sourceRectangle, Color.White);
        }
    }
}
