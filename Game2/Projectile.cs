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
    public enum CollidedWith { Left, Right, None, Top, Bottom, BottomGoal, TopGoal}
    public enum Direction { UpLeft, UpRight, DownLeft, DownRight, None}
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

        public CollidedWith collisionLocation;
        public Direction projectileDirection { get; set; }

        Rectangle boundary;

        PlayerIndex playerIndex;
        InputAction inputAction;

        public Projectile(InputAction inputAction, float angle, Vector2 location, ContentManager content, GraphicsDeviceManager graphics, PlayerIndex playerIndex, Rectangle boundary)
        {
            this.content = content;
            this.graphics = graphics;
            collisionLocation = CollidedWith.None;
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
            this.boundary = boundary;
            if (playerIndex == PlayerIndex.One)
            {
                direction = new Vector2((float)Math.Cos(angle + (Math.PI / -2f)), (float)Math.Sin(angle + (Math.PI / -2f)));
            }
            else if (playerIndex == PlayerIndex.Two)
            {
                direction = new Vector2((float)Math.Cos(angle + (Math.PI / 2)), (float)Math.Sin(angle + (Math.PI / 2)));
            }
            direction.Normalize();
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(fireball, location, Color.White);
        }
        public void Update(GameTime gameTime)
        {
            CollisionSwitch();
            UpdateLocation();
        }
        public void UpdateLocation()
        {
            if (bounds.Top <= boundary.Top || bounds.Bottom >= boundary.Bottom)
            {
                direction.Y = -direction.Y;
            }
            if (bounds.Left <= boundary.Left || bounds.Right >= boundary.Right)
            {
                direction.X = -direction.X;
            }
            location.Y += direction.Y * speed;
            location.X += direction.X * speed;

            bounds.Location = new Point((int)location.X, (int)location.Y);
        }
        public void CollisionSwitch()
        {
            /*else if (bounds.Intersects(//add player 1 reflect rect))
            {
                
            }
            else if (bounds.Intersects(//add player 2 reflect rect))
            {
                
            }*/
            /*else if (bounds.Top <= boundary.Top)
            {
                collisionLocation = CollidedWith.TopGoal;
            }
            else if (bounds.Bottom >= boundary.Bottom)
            {
                collisionLocation = CollidedWith.BottomGoal;
            }*/
        }
    }
}
