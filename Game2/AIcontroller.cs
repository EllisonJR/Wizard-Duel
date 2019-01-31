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
    class AIcontroller
    {
        List<Player> players = new List<Player>();
        List<Projectile> projectiles = new List<Projectile>();

        GraphicsDeviceManager graphics;

        int directionalController;
        int reflected;
        int reflectTimer;
        int dashed;
        int dashTimer;

        int shotTimer;
        int chargeTimer;
        float realAngle;
        public float angle;
        bool gotAngle;
        double angleRangeMin;
        double angleRangeMax;

        public int markCounter;

        Matrix shooterMatrix;
        public Vector2 projectileOrigin;

        public AIcontroller(GraphicsDeviceManager graphics)
        {
            this.graphics = graphics;
            reflected = 201;
            reflectTimer = 301;
            dashed = 201;
            dashTimer = 701;
            markCounter = 0;
            gotAngle = false;
        }

        public void Update(GameTime gameTime)
        {
            
            shotTimer += gameTime.ElapsedGameTime.Milliseconds;
            if(shotTimer < 2000)
            {
                MarkProjectiles(gameTime);
                InputAbstraction(gameTime);
                AIadjuster();
            }
            else
            {
                AdjustAngle();
                gotAngle = true;
                HumanizeAim();
                CalculateShot();
                players[1].angle = angle;
                players[1].inputAction = InputAction.Charge;
                if (angle >= angleRangeMin && angle <= angleRangeMax)
                {
                    players[1].inputAction = InputAction.Shoot;
                    shotTimer = 0;
                    gotAngle = false;
                }
            }
            players[1].LockObjectstoPlayer();
            players[1].ShotMeter();
            players[1].CalculateRotationOrigin();
        }
        public void AIParser(List<Player> players, List<Projectile> projectiles)
        {
            this.players = players;
            this.projectiles = projectiles;
        }
        public void InputAbstraction(GameTime gameTime)
        {
            dashTimer += gameTime.ElapsedGameTime.Milliseconds;
            dashed += gameTime.ElapsedGameTime.Milliseconds;
            reflectTimer += gameTime.ElapsedGameTime.Milliseconds;
            reflected += gameTime.ElapsedGameTime.Milliseconds;
            directionalController += gameTime.ElapsedGameTime.Milliseconds;
            foreach (Player player in players)
            {
                if(player.playerIndex == PlayerIndex.Two && player.AI == true)
                {
                    foreach (Projectile projectile in projectiles)
                    {
                        if (reflected < 200)
                        {
                            player.inputAction = InputAction.Reflect;
                        }
                        else if (dashed <= 200)
                        {
                            if (player.inputAction == InputAction.DashLeft)
                            {
                                player.inputAction = InputAction.DashLeft;
                            }
                            if (player.inputAction == InputAction.DashRight)
                            {
                                player.inputAction = InputAction.DashRight;
                            }
                        }
                        else if (projectile.bounds.Intersects(players[1].reflectHitBox))
                        {
                            if (reflectTimer > 300)
                            {
                                player.inputAction = InputAction.Reflect;
                                reflected = 0;
                                reflectTimer = 0;
                            }
                        }
                        else if (projectile.bounds.Intersects(player.aiRectangle))
                        {
                            if (projectile.bounds.Center.X > player.hitBox.Center.X && dashTimer >= 700)
                            {
                                player.inputAction = InputAction.DashLeft;
                                dashed = 0;
                                dashTimer = 0;
                            }
                            if (projectile.bounds.Center.X < player.hitBox.Center.X && dashTimer >= 700)
                            {
                                player.inputAction = InputAction.DashRight;
                                dashed = 0;
                                dashTimer = 0;
                            }
                        }
                        
                        else if (projectile.aiMark == true)
                        {
                            
                            if (directionalController >= 400)
                            {
                                if (projectile.bounds.Center.X > player.hitBox.Center.X)
                                {
                                    if (projectile.bounds.Right < player.hitBox.Right + 5)
                                    {
                                        player.inputAction = InputAction.None;
                                        directionalController = 200;
                                    }
                                    else
                                    {
                                        player.inputAction = InputAction.Right;
                                    }
                                }
                                else if (projectile.bounds.Center.X < player.hitBox.Center.X)
                                {
                                    if (projectile.bounds.Left > player.hitBox.Left - 5)
                                    {
                                        player.inputAction = InputAction.None;
                                        directionalController = 200;
                                    }
                                    else
                                    {
                                        player.inputAction = InputAction.Left;
                                    }
                                }
                                
                                else
                                {
                                    player.inputAction = InputAction.None;
                                    directionalController = 0;
                                }
                            }
                            else
                            {
                                player.inputAction = InputAction.None;
                            }
                        }
                        else
                        {
                            player.inputAction = InputAction.None;
                        }
                    }
                }
            }
        }
        public void MarkProjectiles(GameTime gameTime)
        {
            foreach (Projectile projectile in projectiles)
            {
                if (projectile.aiMark == true && projectile.direction.Y < 0)
                {
                    projectile.aiMark = false;
                    markCounter--;
                }
                if(projectile.aiMark == true && projectile.bounds.Top > players[1].hitBox.Bottom)
                {
                    projectile.aiMark = false;
                    markCounter--;
                }
            }
            foreach(Projectile projectile in projectiles)
            {
                if (projectile.bounds.Top < graphics.PreferredBackBufferHeight / 2)
                {
                    if (markCounter == 1)
                    {
                        
                    }
                    else
                    {
                        if (projectile.aiMark == false)
                        {
                            if (projectile.direction.Y > 0 || projectile.bounds.Top < players[1].aiRectangle.Bottom || markCounter == 1)
                            {

                            }
                            else
                            {
                                projectile.aiMark = true;
                                markCounter++;
                            }
                        }
                    }
                }
            }
            if(markCounter < 0)
            {
                markCounter = 0;
            }
        }
        public void AIadjuster()
        {
            foreach (Player player in players)
            {
                if (player.playerIndex == PlayerIndex.Two && player.AI == true)
                {
                    if (player.inputAction == InputAction.DashLeft || player.inputAction == InputAction.DashRight)
                    {
                        if (player.inputAction == InputAction.DashLeft)
                        {
                            if (dashed <= 100)
                            {
                                player.playerSpeed = 20;
                            }
                            if (dashed > 100 && dashed <= 150)
                            {
                                player.playerSpeed = 8;
                            }
                            if (dashed > 175 && dashed <= 200)
                            {
                                player.playerSpeed = 7;
                            }
                            if (dashed > 200)
                            {
                                player.playerSpeed = 5;
                                player.inputAction = InputAction.None;
                            }
                            player.playerLocation.X -= player.playerSpeed;
                        }
                        if (player.inputAction == InputAction.DashRight)
                        {
                            if (dashed <= 100)
                            {
                                player.playerSpeed = 20;
                            }
                            if (dashed > 100 && dashed <= 150)
                            {
                                player.playerSpeed = 8;
                            }
                            if (dashed > 250 && dashed <= 200)
                            {
                                player.playerSpeed = 7;
                            }
                            if (dashed > 200)
                            {
                                player.playerSpeed = 5;
                                player.inputAction = InputAction.None;
                            }
                            player.playerLocation.X += player.playerSpeed;
                        }
                    }
                    if (player.inputAction == InputAction.Left)
                    {
                        player.playerSpeed = 5;
                        player.playerLocation.X -= player.playerSpeed;
                    }
                    else if (player.inputAction == InputAction.Right)
                    {
                        player.playerSpeed = 5;
                        player.playerLocation.X += player.playerSpeed;
                    }
                    if (player.playerLocation.X > graphics.PreferredBackBufferWidth - player.playerSprite.Width - 25)
                    {
                        player.playerLocation.X = graphics.PreferredBackBufferWidth - player.playerSprite.Width - 25;
                    }
                    if (player.playerLocation.X < 25)
                    {
                        player.playerLocation.X = 25;
                    }
                }
            }
        }
        public double CalculateAngle()
        {
            Random randomAngle = new Random();
            return randomAngle.NextDouble() * (0.96 - -0.96) + -0.96;
        }
        public void AdjustAngle()
        {
            if (gotAngle == false)
            {
                angle = (float)CalculateAngle();
                realAngle = angle;
                angleRangeMax = realAngle + .05;
                angleRangeMin = realAngle - .05;
                if (angle > 0)
                {
                    angle -= .70f;
                }
                else if (angle < 0)
                {
                    angle += .70f;
                }
            }
        }
        public void HumanizeAim()
        {
            if(angle > realAngle)
            {
                angle -= .07f;
            }
            if(angle < realAngle)
            {
                angle += .07f;
            }

        }
        public void CalculateShot()
        {
            shooterMatrix = Matrix.CreateRotationZ(angle) * Matrix.CreateTranslation(players[1].reticalLocation.X, players[1].reticalLocation.Y, 0);
            projectileOrigin = Vector2.Transform(new Vector2(0, players[1].playerRetical.Height), shooterMatrix);
        }
    }
}
