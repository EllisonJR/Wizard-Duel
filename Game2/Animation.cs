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

        public Vector2 center;

        public bool shrink = false;

        Vector2 sizer = new Vector2(2, 2);
        float shrinker = .1f;

        public int r = 255;
        public int g = 170;
        public int b = 85;

        public bool rainbow = false;
        public bool fire = false;
        public bool rainbowRapid;

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
        public Animation(Texture2D texture, int rows, int columns, int currentFrame, int frameTime)
        {
            spriteSheet = texture;
            this.rows = rows;
            this.columns = columns;
            currentFrame = 0;
            totalFrames = rows * columns;
            width = spriteSheet.Width / columns;
            height = spriteSheet.Height / rows;
            this.currentFrame = currentFrame;
            this.frameTime = frameTime;
        }
        public Animation(Texture2D texture, int rows, int columns, int currentFrame, int frameTime, int i)
        {
            spriteSheet = texture;
            this.rows = rows;
            this.columns = columns;
            currentFrame = i;
            totalFrames = rows * columns;
            width = spriteSheet.Width / columns;
            height = spriteSheet.Height / rows;
            this.currentFrame = currentFrame;
            this.frameTime = frameTime;
        }
        public Animation(Texture2D texture, int rows, int columns,int frameTime, int r, int g, int b)
        {
            spriteSheet = texture;
            this.rows = rows;
            this.columns = columns;
            totalFrames = rows * columns;
            width = spriteSheet.Width / columns;
            height = spriteSheet.Height / rows;
            this.frameTime = frameTime;
            this.r = r;
            this.g = g;
            this.b = b;
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
            if(rainbow == true)
            {
                r += 5;
                if (r > 255)
                {
                    r = 0;
                }
                g += 5;
                if (g > 255)
                {
                    g = 0;
                }
                b += 5;
                if (b > 255)
                {
                    b = 0;
                }
            }
            if (rainbowRapid == true)
            {
                r += 30;
                if (r > 255)
                {
                    r = 0;
                }
                g += 30;
                if (g > 255)
                {
                    g = 0;
                }
                b += 30;
                if (b > 255)
                {
                    b = 0;
                }
            }
            if (fire == true)
            {
                r -= 5;
                if (r <= 200)
                {
                    r = 250;
                }
                g = 80;
                b = 20;
            }
            drawingLocation = location;

            int row = (int)((float)currentFrame / (float)columns);
            int column = currentFrame % columns;

            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
            Rectangle destinationRectangle = new Rectangle((int)location.X, (int)location.Y, width, height);

            center = new Vector2(width / 2, height / 2);

            if (shrink == false)
            {
                if (rainbow == false)
                {
                    spriteBatch.Draw(spriteSheet, destinationRectangle, sourceRectangle, Color.White);
                }
                if(rainbow == true || fire == true || rainbowRapid == true)
                {
                    spriteBatch.Draw(spriteSheet, destinationRectangle, sourceRectangle, new Color(r,g,b));
                }
            }
            if(shrink == true)
            {
                if(rainbow == true || fire == true || rainbowRapid == true)
                {
                    spriteBatch.Draw(spriteSheet, new Vector2(destinationRectangle.Location.X, destinationRectangle.Location.Y) + center, sourceRectangle, new Color(r,g,b), 0f, center, sizer, SpriteEffects.None, 0f);
                }
                if (rainbow == false)
                {
                    spriteBatch.Draw(spriteSheet, new Vector2(destinationRectangle.Location.X, destinationRectangle.Location.Y) + center, sourceRectangle, Color.White, 0f, center, sizer, SpriteEffects.None, 0f);
                }
                sizer.X -= shrinker;
                sizer.Y -= shrinker;
                if(sizer.X <= 1)
                {
                    shrink = false;
                    sizer = new Vector2(2, 2);
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch, Vector2 location, int r, int g, int b)
        {
            if (rainbow == true)
            {
                r += 50;
                if (r > 255)
                {
                    r = 0;
                }
                g += 50;
                if (g > 255)
                {
                    g = 0;
                }
                b += 50;
                if (b > 255)
                {
                    b = 0;
                }
            }

            drawingLocation = location;

            int row = (int)((float)currentFrame / (float)columns);
            int column = currentFrame % columns;

            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
            Rectangle destinationRectangle = new Rectangle((int)location.X, (int)location.Y, width, height);

            center = new Vector2(width / 2, height / 2);

            if (shrink == false)
            {
                spriteBatch.Draw(spriteSheet, destinationRectangle, sourceRectangle, new Color(r, g, b));
            }
            if (shrink == true)
            {
                spriteBatch.Draw(spriteSheet, destinationRectangle, sourceRectangle, new Color(r, g, b));
                sizer.X -= shrinker;
                sizer.Y -= shrinker;
                if (sizer.X <= 1)
                {
                    shrink = false;
                    sizer = new Vector2(2, 2);
                }
            }
        }
        public void DrawShrink(SpriteBatch spriteBatch, Vector2 location)
        {
            drawingLocation = location;

            int row = (int)((float)currentFrame / (float)columns);
            int column = currentFrame % columns;

            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
            Rectangle destinationRectangle = new Rectangle((int)location.X, (int)location.Y, width, height);

            center = new Vector2(width / 2, height / 2);

            spriteBatch.Draw(spriteSheet, new Vector2(destinationRectangle.Location.X, destinationRectangle.Location.Y) + center, sourceRectangle, Color.White, 0f, center, sizer, SpriteEffects.None, 0f);
            if (sizer.Y > .7f)
            {
                sizer.X -= shrinker;
                sizer.Y -= shrinker;
            }
        }
        public void DrawColor(SpriteBatch spriteBatch, Vector2 location)
        {
            r += 50;
            if (r > 255)
            {
                r = 0;
            }
            g += 50;
            if (g > 255)
            {
                g = 0;
            }
            b += 50;
            if (b > 255)
            {
                b = 0;
            }
            drawingLocation = location;

            int row = (int)((float)currentFrame / (float)columns);
            int column = currentFrame % columns;

            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
            Rectangle destinationRectangle = new Rectangle((int)location.X, (int)location.Y, width, height);

            center = new Vector2(width / 2, height / 2);

            if (shrink == false)
            {
                spriteBatch.Draw(spriteSheet, destinationRectangle, sourceRectangle, new Color(r,g,b));
            }
            if (shrink == true)
            {
                spriteBatch.Draw(spriteSheet, new Vector2(destinationRectangle.Location.X, destinationRectangle.Location.Y) + center, sourceRectangle, Color.White, 0f, center, sizer, SpriteEffects.None, 0f);
                sizer.X -= shrinker;
                sizer.Y -= shrinker;
                if (sizer.X <= 1)
                {
                    shrink = false;
                    sizer = new Vector2(2, 2);
                }
            }
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
        public void DrawPotion(SpriteBatch spriteBatch, Vector2 location)
        {
            drawingLocation = location;

            int row = (int)((float)currentFrame / (float)columns);
            int column = currentFrame % columns;

            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
            Rectangle destinationRectangle = new Rectangle((int)location.X + width / 2, (int)location.Y + height / 2, width, height);

            center = new Vector2(width / 2, height / 2);

            spriteBatch.Draw(spriteSheet, destinationRectangle, sourceRectangle, Color.White, angle += .01f, center, SpriteEffects.None, 0f);
        }
    }
}
