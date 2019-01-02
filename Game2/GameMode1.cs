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
        
        List<Player> players = new List<Player>();
        List<Projectile> projectiles = new List<Projectile>();

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
        }
        public void Update(GameTime gameTime)
        {
            foreach (Player player in players)
            {
                player.Update(gameTime);
                if (player.inputAction == InputAction.Shoot || player.inputAction == InputAction.ChargeShot)
                {
                    projectiles.Add(new Projectile(player.inputAction, player.shootingAngle, player.projectileOrigin, content, graphics, player.playerIndex, boundary.bounds));
                }
                foreach(Projectile projectile in projectiles)
                {
                    projectile.Update(gameTime);
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
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
