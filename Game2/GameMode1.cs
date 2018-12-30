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
    class GameMode1
    {
        public GameStates currentGameState { get; set; }

        ContentManager content;
        GraphicsDeviceManager graphics;

        Boundary boundary;

        List<Projectile> projectiles = new List<Projectile>();
        List<Player> players = new List<Player>();

        public GameMode1(GameStates currentGameState, ContentManager content, GraphicsDeviceManager graphics)
        {
            this.content = content;
            this.graphics = graphics;
            players.Add(new Player(ControlType.GamePlay, PlayerIndex.One, content, graphics));
            players.Add(new Player(ControlType.GamePlay, PlayerIndex.Two, content, graphics));
            boundary = new Boundary(content, graphics);
        }
        public void LoadContent()
        {
            boundary.Loadcontent();
            foreach (Player player in players)
            {
                player.LoadContent();
            }
        }
        public void Unloadcontent()
        {
            content.Unload();
            boundary.UnloadContent();
            foreach(Player player in players)
            {
                player.UnloadContent();
            }
            foreach(Projectile projectile in projectiles)
            {
                projectile.UnloadContent();
            }
        }
        public void Update(GameTime gameTime)
        {
            foreach(Player player in players)
            {
                player.Update(gameTime);
                if(player.inputAction == InputAction.Shoot || player.inputAction == InputAction.ChargeShot)
                {
                    projectiles.Add(new Projectile(player.inputAction, player.shootingAngle, player.projectileOrigin, content, graphics, player.playerIndex));
                }
                for (int i = 0; i < projectiles.Count; i++)
                {
                    projectiles[i].Update(gameTime);

                    if (projectiles[i].bounds.Left <= boundary.bounds.Left)
                    {
                        projectiles[i].collisionLocation = Collided.Left;
                        if (projectiles[i].previousCollision == Collided.Right)
                        {
                            if (projectiles[i].collided == false)
                            {
                                projectiles[i].collided = true;
                            }
                            else if (projectiles[i].collided == true)
                            {
                                projectiles[i].collided = false;
                            }
                        }
                    }
                    else if (projectiles[i].bounds.Right >= boundary.bounds.Right)
                    {
                        projectiles[i].collisionLocation = Collided.Right;
                        if (projectiles[i].previousCollision == Collided.Left)
                        {
                            if (projectiles[i].collided == false)
                            {
                                projectiles[i].collided = true;
                            }
                            else if (projectiles[i].collided == true)
                            {
                                projectiles[i].collided = false;
                            }
                        }
                    }
                    else if (projectiles[i].bounds.Top <= boundary.bounds.Top)
                    {
                        projectiles[i].collisionLocation = Collided.Top;
                    }
                    else if(projectiles[i].bounds.Bottom >= boundary.bounds.Bottom)
                    {
                        projectiles[i].collisionLocation = Collided.Bottom;
                    }

                    if(projectiles[i].collisionLocation == Collided.Bottom)
                    {
                        player.score += 1;
                        projectiles.RemoveAt(i);
                    }
                    else if(projectiles[i].collisionLocation == Collided.Top)
                    {
                        player.score += 1;
                        projectiles.RemoveAt(i);
                    }
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Player player in players)
            {
                player.Draw(spriteBatch);
                foreach (Projectile projectile in projectiles)
                {
                    projectile.Draw(spriteBatch);
                }
            }
            boundary.Draw(spriteBatch);
        }
    }
}
