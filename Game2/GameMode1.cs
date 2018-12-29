using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        Texture2D border;
        Rectangle bounds;
        Vector2 borderLocation;

        Player player1;
        Player player2;

        Projectile fireballs;

        List<Projectile> projectiles = new List<Projectile>();
        List<Player> players = new List<Player>();

        public GameMode1(GameStates currentGameState, ContentManager content, GraphicsDeviceManager graphics)
        {
            this.content = content;
            this.graphics = graphics;
            players.Add(new Player(ControlType.GamePlay, PlayerIndex.One, content, graphics));
            players.Add(new Player(ControlType.GamePlay, PlayerIndex.Two, content, graphics));
        }
        public void LoadContent()
        {
            foreach (Projectile projectile in projectiles)
            {
                projectile.LoadContent();
            }
            foreach (Player player in players)
            {
                player.LoadContent();
            }
        }
        public void Unloadcontent()
        {
            content.Unload();
            foreach(Player player in players)
            {
                player.UnloadContent();
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
                foreach(Projectile projectile in projectiles)
                {
                    if (player.inputAction == InputAction.Shoot || player.inputAction == InputAction.ChargeShot)
                    {
                        projectile.LoadContent();
                    }
                    projectile.Update(gameTime);
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
        }
    }
}
