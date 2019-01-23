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

        public int recentlyReflected;

        public bool aiMark;

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

            if(playerIndex == PlayerIndex.One)
            {
                this.location.Y -= fireball.Height;
                this.location.X -= fireball.Width / 2;
            }
            else if(playerIndex == PlayerIndex.Two)
            {
                this.location.X -= fireball.Width / 2;
            }

            if(location.X + fireball.Width > boundary.Right)
            {
                this.location.X = boundary.Right - fireball.Width;
            }
            else if(location.X < boundary.Left)
            {
                this.location.X = boundary.Left;
            }
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
            recentlyReflected = 0;

            aiMark = false;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(fireball, location, Color.White);
        }
        public void Update(GameTime gameTime)
        {
            UpdateLocation();
            recentlyReflected += gameTime.ElapsedGameTime.Milliseconds;
            CollisionSwitch();
        }
        public void UpdateLocation()
        {
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
            collisionLocation = CollidedWith.None;
            if (bounds.Top <= boundary.Top)
            {
                collisionLocation = CollidedWith.TopGoal;
            }
            if (bounds.Bottom >= boundary.Bottom)
            {
                collisionLocation = CollidedWith.BottomGoal;
            }
            if (bounds.Left <= boundary.Left)
            {
                collisionLocation = CollidedWith.Left;
            }
            if (bounds.Right >= boundary.Right)
            {
                collisionLocation = CollidedWith.Right;
            }
        }
    }
}
