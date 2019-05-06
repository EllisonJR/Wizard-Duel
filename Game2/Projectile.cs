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

        public int projectileTime;

        Vector2 origin;
        public Vector2 location;
        public double angle;
        public double speed;
        public int originalSpeed;
        public Rectangle bounds;
        public Vector2 direction;
        public Animation fireball { get; set; }
        Texture2D fireballT;

        public CollidedWith collisionLocation;
        public Direction projectileDirection { get; set; }

        Rectangle boundary;

        public bool reversing;
        public bool accelerating;

        public int recentlyReflected;

        public bool aiMark;

        public PlayerIndex playerIndex;
        public InputAction inputAction;

        public int playerChoice;

        public int reverseTimer;

        Vector2 leftOrRight;
        Texture2D wallBounceT;
        Animation wallBounce;

        public int originalOwner;

        Texture2D fireballTrail1T;
        Texture2D fireballTrail2T;
        List<Animation> trails = new List<Animation>();

        List<Vector2> trailLocations = new List<Vector2>();

        public Vector2 bounceLocation;

        public bool chargeShot;

        public bool removalPing;

        public bool tempNoCollison;

        int noCollisionTimer;

        public Projectile(InputAction inputAction, float angle, Vector2 location, ContentManager content, GraphicsDeviceManager graphics, PlayerIndex playerIndex, Rectangle boundary, int playerChoice)
        {
            this.content = content;
            this.graphics = graphics;
            this.playerChoice = playerChoice;
            originalOwner = playerChoice;
            collisionLocation = CollidedWith.None;
            this.inputAction = inputAction;
            if (this.inputAction == InputAction.Shoot)
            {
                chargeShot = false;
                originalSpeed = 2;
                speed = 2;
                fireballT = content.Load<Texture2D>("sprites/player effects/mainfireball");
                fireballTrail1T = content.Load<Texture2D>("sprites/player effects/fireballtail1");
                fireballTrail2T = content.Load<Texture2D>("sprites/player effects/fireballtail2");
                fireball = new Animation(fireballT, 2, 3, angle);
                trails.Add(new Animation(fireballTrail1T, 2, 3, angle));
                trails.Add(new Animation(fireballTrail2T, 2, 3, angle));
                fireball.currentFrame = 2;
                trails[0].currentFrame = 1;
                trails[1].currentFrame = 0;

            }
            if (this.inputAction == InputAction.ChargeShot)
            {
                chargeShot = true;
                if(originalOwner == 1)
                {
                    fireballT = content.Load<Texture2D>("sprites/player effects/buffchargeshot");
                    fireball = new Animation(fireballT, 2, 3, angle);
                }
                if(originalOwner == 2)
                {
                    fireballT = content.Load<Texture2D>("sprites/player effects/tiredchargeshot");
                    fireball = new Animation(fireballT, 2, 3, angle);
                }
                originalSpeed = 4;
                speed = 4;
            }

            wallBounceT = content.Load<Texture2D>("sprites/border items/wallbounce");
            wallBounce = new Animation(wallBounceT, 2, 6);
            wallBounce.currentFrame = 10;

            reversing = false;
            accelerating = false;

            this.location = location;
            this.angle = angle;
            this.playerIndex = playerIndex;

            if(playerIndex == PlayerIndex.One)
            {
                this.location.Y -= fireball.height;
                this.location.X -= fireball.width / 2;
            }
            else if(playerIndex == PlayerIndex.Two)
            {
                this.location.X -= fireball.width / 2;
            }

            if(location.X + fireball.width > boundary.Right)
            {
                this.location.X = boundary.Right - fireball.width;
            }
            else if(location.X < boundary.Left)
            {
                this.location.X = boundary.Left;
            }
            bounds = new Rectangle((int)this.location.X, (int)this.location.Y, (int)fireball.width, (int)fireball.height);
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
            removalPing = false;
            tempNoCollison = false;
        }
        public Projectile(InputAction inputAction, float angle, Vector2 location, ContentManager content, GraphicsDeviceManager graphics, PlayerIndex playerIndex, Rectangle boundary, int playerChoice, bool tempNoCollision)
        {
            this.content = content;
            this.graphics = graphics;
            this.playerChoice = playerChoice;
            originalOwner = playerChoice;
            collisionLocation = CollidedWith.None;
            this.inputAction = inputAction;
            if (this.inputAction == InputAction.Shoot)
            {
                chargeShot = false;
                originalSpeed = 2;
                speed = 2;
                fireballT = content.Load<Texture2D>("sprites/player effects/mainfireball");
                fireballTrail1T = content.Load<Texture2D>("sprites/player effects/fireballtail1");
                fireballTrail2T = content.Load<Texture2D>("sprites/player effects/fireballtail2");
                fireball = new Animation(fireballT, 2, 3, angle);
                trails.Add(new Animation(fireballTrail1T, 2, 3, angle));
                trails.Add(new Animation(fireballTrail2T, 2, 3, angle));
                fireball.currentFrame = 2;
                trails[0].currentFrame = 1;
                trails[1].currentFrame = 0;

            }
            if (this.inputAction == InputAction.ChargeShot)
            {
                chargeShot = true;
                if (originalOwner == 1)
                {
                    fireballT = content.Load<Texture2D>("sprites/player effects/buffchargeshot");
                    fireball = new Animation(fireballT, 2, 3, angle);
                }
                if (originalOwner == 2)
                {
                    fireballT = content.Load<Texture2D>("sprites/player effects/tiredchargeshot");
                    fireball = new Animation(fireballT, 2, 3, angle);
                }
                originalSpeed = 4;
                speed = 4;
            }

            wallBounceT = content.Load<Texture2D>("sprites/border items/wallbounce");
            wallBounce = new Animation(wallBounceT, 2, 6);
            wallBounce.currentFrame = 10;

            reversing = false;
            accelerating = false;

            this.location = new Vector2(location.X, location.Y);
            this.angle = angle;
            this.playerIndex = playerIndex;

            if (location.X + fireball.width > boundary.Right)
            {
                this.location.X = boundary.Right - fireball.width;
            }
            else if (location.X < boundary.Left)
            {
                this.location.X = boundary.Left;
            }
            bounds = new Rectangle((int)this.location.X, (int)this.location.Y, (int)fireball.width, (int)fireball.height);
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
            removalPing = false;
            this.tempNoCollison = tempNoCollision;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            fireball.DrawProjectile(spriteBatch, location, bounds);
            foreach(Animation trail in trails)
            {
                //trail.Draw(spriteBatch, )
            }
            wallBounce.Draw(spriteBatch, leftOrRight);
        }
        public void Update(GameTime gameTime)
        {
            foreach(Animation trail in trails)
            {
                trail.Update(gameTime, (float)angle);
            }
            fireball.Update(gameTime, (float)angle);
            reverseTimer += gameTime.ElapsedGameTime.Milliseconds;
            projectileTime += gameTime.ElapsedGameTime.Milliseconds;
            UpdateLocation();
            recentlyReflected += gameTime.ElapsedGameTime.Milliseconds;
            CollisionSwitch();
            SleepyReflect();
            WallBounce(gameTime);
            ProjectileAnimations();
        }
        public void UpdateLocation()
        {
            if (bounds.Left <= boundary.Left || bounds.Right >= boundary.Right)
            {
                bounceLocation = location;
                direction.X = -direction.X;
                angle = -angle;
            }
            location.Y += direction.Y * (float)speed;
            location.X += direction.X * (float)speed;

            bounds.Location = new Point((int)location.X, (int)location.Y);
        }
        public void SleepyReflect()
        {
            if (playerChoice == 2)
            {

                if (reversing == true && speed > .1f)
                {
                    speed -= .15f;
                }
                
                if (speed < 0.1f)
                {
                    reversing = false;
                    accelerating = true;
                    direction.Y = -direction.Y;
                    bounceLocation = location;
                    angle = -angle + Math.PI;
                }
                if (speed < originalSpeed && accelerating == true)
                {
                    speed += .15f;
                    reverseTimer = 0;
                }
                if(speed == originalSpeed)
                {
                    accelerating = false;
                }
            }
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
                leftOrRight = new Vector2(bounds.Left, bounds.Center.Y - wallBounce.width / 2);
                collisionLocation = CollidedWith.Left;
            }
            if (bounds.Right >= boundary.Right)
            {
                leftOrRight = new Vector2(bounds.Right - wallBounce.width, bounds.Center.Y - wallBounce.width / 2);
                collisionLocation = CollidedWith.Right;
            }
        }
        public void WallBounce(GameTime gameTime)
        {
            
            if (wallBounce.currentFrame >= 0 && wallBounce.currentFrame <= 9)
            {
                wallBounce.Update(gameTime);
            }
            if(collisionLocation == CollidedWith.Left)
            {
                wallBounce.currentFrame = 0;
            }
            if (wallBounce.currentFrame == 4)
            {
                wallBounce.currentFrame = 10;
            }
            if (collisionLocation == CollidedWith.Right)
            {
                wallBounce.currentFrame = 5;
            }
            
            if (wallBounce.currentFrame == 10)
            {
                wallBounce.currentFrame = 10;
            }
        }
        public void ProjectileAnimations()
        {
            if (inputAction == InputAction.Shoot)
            {
                if(fireball.currentFrame >= 4)
                {
                    fireball.currentFrame = 0;
                }
            }
            else if (inputAction == InputAction.ChargeShot)
            {
                if (originalOwner == 1)
                {
                    if (fireball.currentFrame >= 6)
                    {
                        fireball.currentFrame = 0;
                    }
                }
                if (originalOwner == 2)
                {
                    if (fireball.currentFrame >= 4)
                    {
                        fireball.currentFrame = 0;
                    }
                }
            }
        }
    }
}
