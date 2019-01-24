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
    class GameLoopLogic
    {
        public List<Projectile> projectiles = new List<Projectile>();
        public List<Player> players = new List<Player>();

        AIcontroller aiController;

        ContentManager content;
        GraphicsDeviceManager graphics;
        Boundary boundary;

        public string winnerText = "";

        public GameLoopLogic(ContentManager content, GraphicsDeviceManager graphics, Boundary boundary)
        {
            this.content = content;
            this.graphics = graphics;
            this.boundary = boundary;
            aiController = new AIcontroller(graphics);
        }

        public void ListChecks(GameTime gameTime)
        {
            foreach (Player player in players)
            {
                player.Update(gameTime);
                if (player.inputAction == InputAction.Shoot || player.inputAction == InputAction.ChargeShot)
                {
                    projectiles.Add(new Projectile(player.inputAction, player.shootingAngle, player.projectileOrigin, content, graphics, player.playerIndex, boundary.bounds));
                    
                }
                if (player.inputAction == InputAction.Reflect)
                {
                    foreach (Projectile projectile in projectiles)
                    {
                        if (projectile.bounds.Intersects(player.reflectHitBox))
                        {
                            if (projectile.recentlyReflected >= 50)
                            {
                                projectile.direction.Y = -projectile.direction.Y;
                            }
                            projectile.recentlyReflected = 0;
                        }
                    }
                }
                foreach (Projectile projectile in projectiles)
                {
                    projectile.Update(gameTime);
                    if (projectile.collisionLocation == CollidedWith.TopGoal || projectile.collisionLocation == CollidedWith.BottomGoal)
                    {
                        if (player.playerIndex == PlayerIndex.One)
                        {
                            player.score++;
                        }
                        if (player.playerIndex == PlayerIndex.Two)
                        {
                            player.score++;
                        }
                    }
                }
                for (int i = 0; i < projectiles.Count; i++)
                {
                    if (projectiles[i].collisionLocation == CollidedWith.TopGoal || projectiles[i].collisionLocation == CollidedWith.BottomGoal)
                    {
                        projectiles.RemoveAt(i--);
                        aiController.markCounter--;
                    }
                }
                for (int i = 0; i < projectiles.Count; i++)
                {
                    if (projectiles[i].bounds.Intersects(player.hitBox))
                    {
                        projectiles.RemoveAt(i--);
                        player.health--;
                        aiController.markCounter--;
                    }
                }
            }
            
            aiController.AIParser(players, projectiles);
            aiController.Update(gameTime);
        }
        public void TimeUpConditions(GameTime gameTime)
        {
            if (players[0].score > players[1].score)
            {
                winnerText = "Player 1 wins!\nPress A to return \nto the main menu.";
            }
            else if (players[1].score > players[0].score)
            {
                winnerText = "Player 2 wins!\nPress A to return \nto the main menu.";
            }
            else
            {

                winnerText = "Draw!";
            }
        }
        public void PlayerDeathConditions(GameTime gameTime)
        {
            if (players[0].health == 0)
            {
                winnerText = "Player 2 wins!\nPress A to return \nto the main menu.";
            }
            else if (players[1].health == 0)
            {
                winnerText = "Player 1 wins!\nPress A to return \nto the main menu.";
            }
        }

        public void DrawLists(SpriteBatch spriteBatch)
        {
            foreach (Player player in players)
            {
                player.Draw(spriteBatch);
            }
            foreach (Projectile projectile in projectiles)
            {
                projectile.Draw(spriteBatch);
            }
        }
    }
}
