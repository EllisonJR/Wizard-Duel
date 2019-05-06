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
        public int totalFrames;

        public int width;
        public int height;

        public int frameTimer;

        public Vector2 drawingLocation;

        public int frameTime;

        public float angle;

        Vector2 center;

        public Animation(Texture2D texture, int rows, int columns)
        {
            spriteSheet = texture;
            this.rows = rows;
            this.columns = columns;
            currentFrame = 0;
            totalFrames = rows * columns;
            width = spriteSheet.Width / columns;
            height = spriteSheet.Height / rows;
            angle = 0f;

            frameTime = 100;
        }
        public Animation(Texture2D texture, int rows, int columns, float angle)
        {
            spriteSheet = texture;
            this.rows = rows;
            this.columns = columns;
            currentFrame = 0;
            totalFrames = rows * columns;
            width = spriteSheet.Width / columns;
            height = spriteSheet.Height / rows;
            this.angle = angle;

            frameTime = 100;
        }
        public void Update(GameTime gameTime)
        {
            frameTimer += gameTime.ElapsedGameTime.Milliseconds;
            if (frameTimer >= frameTime)
            {
                currentFrame++;
                frameTimer = 0;
            }
        }
        public void Update(GameTime gameTime, float angle)
        {
            frameTimer += gameTime.ElapsedGameTime.Milliseconds;
            if (frameTimer >= frameTime)
            {
                currentFrame++;
                frameTimer = 0;
            }
            this.angle = angle;
        }
        public void Reverse(GameTime gameTime)
        {
            frameTimer += gameTime.ElapsedGameTime.Milliseconds;
            if (frameTimer >= frameTime)
            {
                currentFrame--;
                frameTimer = 0;
            }
        }
        public void Draw(SpriteBatch spriteBatch, Vector2 location)
        {
            drawingLocation = location;

            int row = (int)((float)currentFrame / (float)columns);
            int column = currentFrame % columns;

            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
            Rectangle destinationRectangle = new Rectangle((int)location.X, (int)location.Y, width, height);

            spriteBatch.Draw(spriteSheet, destinationRectangle, sourceRectangle, Color.White);
        }
        public void DrawProjectile(SpriteBatch spriteBatch, Vector2 location, Rectangle bounds)
        {
            drawingLocation = location;

            int row = (int)((float)currentFrame / (float)columns);
            int column = currentFrame % columns;

            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
            Rectangle destinationRectangle = new Rectangle((int)location.X + width/2, (int)location.Y + height / 2, width, height);

            center = new Vector2(width / 2, height / 2);

            spriteBatch.Draw(spriteSheet, destinationRectangle, sourceRectangle, Color.White, angle, center, SpriteEffects.None, 0f);
        }
    }
}
