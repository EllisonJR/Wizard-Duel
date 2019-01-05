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
        SpriteFont font;

        int gameClock;
        int clockCounter;
        int startClock;
        int countDuration;
        double currentTime;

        Boundary boundary;
        
        List<Player> players = new List<Player>();
        List<Projectile> projectiles = new List<Projectile>();

        public GameMode1(GameStates currentGameState, ContentManager content, GraphicsDeviceManager graphics)
        {
            this.content = content;
            this.graphics = graphics;
            players.Add(new Player(ControlType.GamePlay, PlayerIndex.One, content, graphics));
            players.Add(new Player(ControlType.GamePlay, PlayerIndex.Two, content, graphics));
            boundary = new Boundary(content, graphics);

            gameClock = 30;
            clockCounter = 0;
            countDuration = 1;
            currentTime = 0;
        }
        public void LoadContent()
        {
            font = content.Load<SpriteFont>("fonts/gameclock");
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
        }
        public void Update(GameTime gameTime)
        {
            currentTime += gameTime.ElapsedGameTime.TotalSeconds;
            if(currentTime >= countDuration)
            {
                gameClock--;
                currentTime -= countDuration;
            }

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
                for(int i = 0; i < projectiles.Count; i++)
                {
                    if(projectiles[i].collisionLocation == CollidedWith.TopGoal || projectiles[i]. collisionLocation == CollidedWith.BottomGoal)
                    {
                        projectiles.RemoveAt(i--);
                    }
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, ":"+gameClock, new Vector2((graphics.PreferredBackBufferWidth / 2), (graphics.PreferredBackBufferHeight / 2)), Color.White);
            foreach (Player player in players)
            {
                player.Draw(spriteBatch);
            }
            foreach(Projectile projectile in projectiles)
            {
                projectile.Draw(spriteBatch);
            }
            boundary.Draw(spriteBatch);
        }
    }
}
