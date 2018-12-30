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
    public enum Collided { Left, Right, None, Top, Bottom}
    class Projectile
    {
        GraphicsDeviceManager graphics;
        ContentManager content;

        Vector2 origin;
        Vector2 location;
        float angle;
        float speed;
        public Rectangle bounds;
        public Vector2 direction;
        public Texture2D fireball { get; set; }

        public Collided collisionLocation;
        public Collided previousCollision;
        public bool collided;

        PlayerIndex playerIndex;
        InputAction inputAction;

        public Projectile(InputAction inputAction, float angle, Vector2 location, ContentManager content, GraphicsDeviceManager graphics, PlayerIndex playerIndex)
        {
            this.content = content;
            this.graphics = graphics;
            collisionLocation = Collided.None;
            collided = false;
            this.inputAction = inputAction;
            if (this.inputAction == InputAction.Shoot)
            {
                speed = 2;
                fireball = content.Load<Texture2D>("sprites/fireball");
            }
            if (this.inputAction == InputAction.ChargeShot)
            {
                speed = 4;
                fireball = content.Load<Texture2D>("sprites/chargedfireball");
            }

            this.location = location;
            this.angle = angle;
            this.playerIndex = playerIndex;

            bounds = new Rectangle((int)this.location.X, (int)this.location.Y, (int)fireball.Width, (int)fireball.Height);
        }
        public void UnloadContent()
        {
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(fireball, location, Color.White);
        }
        public void Update(GameTime gameTime)
        {
            UpdateLocation();
        }
        public void UpdateLocation()
        {
            previousCollision = collisionLocation;
            if (playerIndex == PlayerIndex.One)
            {
                if (collisionLocation == Collided.None)
                {
                    direction = new Vector2((float)Math.Cos(angle + (Math.PI / -2f)), (float)Math.Sin(angle + (Math.PI / -2f)));
                    direction.Normalize();
                    location += direction * speed;

                    bounds.Location = new Point((int)location.X, (int)location.Y);
                }
                if(collisionLocation == Collided.Left)
                {
                    if (collided == false)
                    {
                        direction = new Vector2((float)Math.Cos(angle + (Math.PI / -2f)), (float)Math.Sin(angle + (Math.PI / -2f)));
                        direction.Normalize();
                        location.X -= direction.X * speed;
                        location.Y += direction.Y * speed;

                        bounds.Location = new Point((int)location.X, (int)location.Y);
                    }
                    else if(collided == true)
                    {
                        direction = new Vector2((float)Math.Cos(angle + (Math.PI / -2f)), (float)Math.Sin(angle + (Math.PI / -2f)));
                        direction.Normalize();
                        location.X += direction.X * speed;
                        location.Y += direction.Y * speed;

                        bounds.Location = new Point((int)location.X, (int)location.Y);
                    }
                }
                if(collisionLocation == Collided.Right)
                {
                    if (collided == false)
                    {
                        direction = new Vector2((float)Math.Cos(angle + (Math.PI / -2f)), (float)Math.Sin(angle + (Math.PI / -2f)));
                        direction.Normalize();
                        location.X -= direction.X * speed;
                        location.Y += direction.Y * speed;

                        bounds.Location = new Point((int)location.X, (int)location.Y);
                    }
                    else if(collided == true)
                    {
                        direction = new Vector2((float)Math.Cos(angle + (Math.PI / -2f)), (float)Math.Sin(angle + (Math.PI / -2f)));
                        direction.Normalize();
                        location.X += direction.X * speed;
                        location.Y += direction.Y * speed;

                        bounds.Location = new Point((int)location.X, (int)location.Y);
                    }
                }
            }
            else if(playerIndex == PlayerIndex.Two)
            {
                if (collisionLocation == Collided.None)
                {
                    direction = new Vector2((float)Math.Cos(angle + (Math.PI / 2)), (float)Math.Sin(angle + (Math.PI / 2)));
                    direction.Normalize();
                    location += direction * speed;

                    bounds.Location = new Point((int)location.X, (int)location.Y);
                }
                if (collisionLocation == Collided.Left)
                {
                    if (collided == false)
                    {
                        direction = new Vector2((float)Math.Cos(angle + (Math.PI / 2f)), (float)Math.Sin(angle + (Math.PI / 2f)));
                        direction.Normalize();
                        location.X -= direction.X * speed;
                        location.Y += direction.Y * speed;

                        bounds.Location = new Point((int)location.X, (int)location.Y);
                    }
                    else if (collided == true)
                    {
                        direction = new Vector2((float)Math.Cos(angle + (Math.PI / 2f)), (float)Math.Sin(angle + (Math.PI / 2f)));
                        direction.Normalize();
                        location.X += direction.X * speed;
                        location.Y += direction.Y * speed;

                        bounds.Location = new Point((int)location.X, (int)location.Y);
                    }
                }
                if (collisionLocation == Collided.Right)
                {
                    if (collided == false)
                    {
                        direction = new Vector2((float)Math.Cos(angle + (Math.PI / 2f)), (float)Math.Sin(angle + (Math.PI / 2f)));
                        direction.Normalize();
                        location.X -= direction.X * speed;
                        location.Y += direction.Y * speed;

                        bounds.Location = new Point((int)location.X, (int)location.Y);
                    }
                    else if (collided == true)
                    {
                        direction = new Vector2((float)Math.Cos(angle + (Math.PI / 2f)), (float)Math.Sin(angle + (Math.PI / 2f)));
                        direction.Normalize();
                        location.X += direction.X * speed;
                        location.Y += direction.Y * speed;

                        bounds.Location = new Point((int)location.X, (int)location.Y);
                    }
                }
            }
        }
    }
}
