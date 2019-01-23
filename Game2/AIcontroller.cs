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

        int markCounter;
        bool markMatch;

        public AIcontroller(GraphicsDeviceManager graphics)
        {
            this.graphics = graphics;
            reflected = 201;
            reflectTimer = 301;
            dashed = 201;
            markCounter = 0;
        }

        public void Update(GameTime gameTime)
        {
            MarkProjectiles(gameTime);
            InputAbstraction(gameTime);
            AIadjuster();
        }
        public void AIParser(List<Player> players, List<Projectile> projectiles)
        {
            this.players = players;
            this.projectiles = projectiles;
        }
        public void InputAbstraction(GameTime gameTime)
        {
            foreach (Player player in players)
            {
                if(player.playerIndex == PlayerIndex.Two && player.AI == true)
                {
                    reflected += gameTime.ElapsedGameTime.Milliseconds;
                    reflectTimer += gameTime.ElapsedGameTime.Milliseconds;
                    directionalController += gameTime.ElapsedGameTime.Milliseconds;
                    foreach (Projectile projectile in projectiles)
                    {
                        if (projectile.firstMark == true || projectile.secondMark == true)
                        {
                            if (reflected <= 200)
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
                                    reflectTimer = 0;
                                }
                            }
                            else if (directionalController >= 700)
                            {
                                if (projectile.bounds.Center.X > player.hitBox.Center.X && projectile.direction.Y < 0)
                                {
                                    player.inputAction = InputAction.Right;
                                }
                                else if (projectile.bounds.Center.X < player.hitBox.Center.X && projectile.direction.Y < 0)
                                {
                                    player.inputAction = InputAction.Left;
                                }
                                else if (projectile.bounds.Left < player.hitBox.Center.X + 20 || projectile.bounds.Right > player.hitBox.Center.X - 20)
                                {
                                    player.inputAction = InputAction.None;
                                }
                                else
                                {
                                    player.inputAction = InputAction.None;
                                }
                            }
                            else if (projectile.bounds.Intersects(player.aiRectangle))
                            {
                                if (projectile.bounds.Center.X > player.hitBox.Center.X)
                                {
                                    player.inputAction = InputAction.DashLeft;
                                    dashed = 0;
                                }
                                if (projectile.bounds.Center.X < player.hitBox.Center.X)
                                {
                                    player.inputAction = InputAction.DashRight;
                                    dashed = 0;
                                }
                            }
                            directionalController = 0;
                        }
                    }
                }
            }
        }
        public void MarkProjectiles(GameTime gameTime)
        {
            foreach (Projectile projectile in projectiles)
            {
                if(projectile.aiMark == true && projectile.direction.Y < 0)
                {
                    projectile.aiMark = false;
                    markCounter--;
                }
                else if(projectile.aiMark == true && projectile.bounds.Intersects(players[1].aiRectangle))
                {
                    projectile.aiMark = false;
                    markCounter--;
                }
                else if(projectile.aiMark == true && projectile.bounds.Intersects(players[1].hitBox))
                {
                    markCounter--;
                }
            }
            foreach(Projectile projectile in projectiles)
            {
                if (projectile.bounds.Top < graphics.PreferredBackBufferHeight / 2)
                {
                    if (markCounter == 2)
                    {
                        
                    }
                    else
                    {
                        if (projectile.aiMark == false)
                        {
                            if (projectile.direction.Y > 0 || projectile.bounds.Top < players[1].aiRectangle.Bottom)
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
            foreach(Projectile projectile in projectiles)
            {
                foreach(Projectile _projectile in projectiles)
                {
                    if(projectile.projectileDirection)
                }
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
                    player.LockObjectstoPlayer();
                    player.ShotMeter();
                }
            }
        }
    }
}
