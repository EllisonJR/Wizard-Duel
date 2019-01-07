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
        Texture2D endGameScreen;

        string winnerText= "";

        public bool active = false;

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

            active = true;

            gameClock = 60;
            startClock = 3;
            clockCounter = 0;
            countDuration = 1;
            currentTime = 0;
        }
        public void Reset()
        {
            foreach(Player player in players)
            {
                player.health = 3;
                player.score = 0;
                player.input.controlType = ControlType.GamePlay;
            }

            currentGameState = GameStates.GameMode1;
            active = true;

            gameClock = 60;
            startClock = 3;
            clockCounter = 0;
            countDuration = 1;
            currentTime = 0;

            projectiles.Clear();
        }
        public void LoadContent()
        {
            font = content.Load<SpriteFont>("fonts/gameclock");
            endGameScreen = content.Load<Texture2D>("sprites/endgamebackground");
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
            if (startClock > -1)
            {
                if (currentTime >= countDuration)
                {
                    startClock--;
                    currentTime -= countDuration;
                }
            }
            else
            {
                if (gameClock == 0)
                {

                    if(players[0].score > players[1].score)
                    {
                        winnerText = "Player 1 wins!\nPress A to return \nto the main menu.";
                    }
                    else if(players[1].score > players[0].score)
                    {
                        winnerText = "Player 2 wins!\nPress A to return \nto the main menu.";
                    }
                    else
                    {

                        winnerText = "Draw!";
                    }
                    foreach(Player player in players)
                    {
                        player.input.controlType = ControlType.Menu;
                        if(player.inputAction == InputAction.Confirm)
                        {
                            currentGameState = GameStates.Menu;
                        }
                    }
                    active = false;
                }
                else if(players[0].health == 0 || players[1].health == 0)
                {
                    if(players[0].health == 0)
                    {
                        winnerText = "Player 2 wins!\nPress A to return \nto the main menu.";
                    }
                    else if(players[1].health == 0)
                    {
                        winnerText = "Player 1 wins!\nPress A to return \nto the main menu.";
                    }
                    foreach (Player player in players)
                    {
                        player.Update(gameTime);
                        player.input.controlType = ControlType.Menu;
                        if (player.inputAction == InputAction.Confirm)
                        {
                            currentGameState = GameStates.Menu;
                        }
                    }
                    active = false;
                }
                else
                {
                    if (currentTime >= countDuration)
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
                        for (int i = 0; i < projectiles.Count; i++)
                        {
                            if (projectiles[i].collisionLocation == CollidedWith.TopGoal || projectiles[i].collisionLocation == CollidedWith.BottomGoal)
                            {
                                projectiles.RemoveAt(i--);
                            }
                        }
                        for (int i = 0; i < projectiles.Count; i++)
                        {
                            if (projectiles[i].bounds.Intersects(player.hitBox))
                            {
                                projectiles.RemoveAt(i--);
                                player.health--;
                            }
                        }
                    }
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (gameClock < 10)
            {
                spriteBatch.DrawString(font, "00:0" + gameClock, new Vector2(boundary.bounds.Right - 60, (graphics.PreferredBackBufferHeight / 2)), Color.White);
            }
            else
            {
                spriteBatch.DrawString(font, "00:" + gameClock, new Vector2(boundary.bounds.Right - 60, (graphics.PreferredBackBufferHeight / 2)), Color.White);
            }
            foreach (Player player in players)
            {
                player.Draw(spriteBatch);
            }
            foreach(Projectile projectile in projectiles)
            {
                projectile.Draw(spriteBatch);
            }
            boundary.Draw(spriteBatch);
            if(startClock > -1)
            {
                if(startClock == 0)
                {
                    spriteBatch.DrawString(font, "Go!", new Vector2(graphics.PreferredBackBufferWidth / 2 - 10, graphics.PreferredBackBufferHeight / 2), Color.White);
                }
                else if(startClock > 0)
                {
                    spriteBatch.DrawString(font, "" + startClock, new Vector2(graphics.PreferredBackBufferWidth / 2 - 10, graphics.PreferredBackBufferHeight / 2), Color.White);
                }
                else
                {

                }
            }
            if (players[0].health == 0 || players[1].health == 0 || gameClock == 0)
            {
                spriteBatch.Draw(endGameScreen, new Vector2(0,0), Color.White);
                spriteBatch.DrawString(font, winnerText, new Vector2(graphics.PreferredBackBufferWidth / 2 - (font.Texture.Width / 2), graphics.PreferredBackBufferHeight / 2), Color.White);
            }
        }
    }
}
