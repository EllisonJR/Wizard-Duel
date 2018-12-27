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
    class Projectile
    {
        GraphicsDeviceManager graphics;
        ContentManager content;

        Vector2 origin;
        Vector2 location;
        float angle;
        float speed;
        public Texture2D fireball { get; set; }

        public bool currentlyActive;
        public bool previouslyActive;

        PlayerIndex playerIndex;
        InputAction inputAction;

        public Projectile(InputAction inputAction, float angle, Vector2 location, ContentManager content, GraphicsDeviceManager graphics, PlayerIndex playerIndex)
        {
            LoadContent();
            this.content = content;
            this.graphics = graphics;
            this.location = location;
            this.angle = angle;
            this.inputAction = inputAction;
            this.playerIndex = playerIndex;
            if(this.inputAction == InputAction.Shoot)
            {
                speed = 2;
            }
            if(this.inputAction == InputAction.ChargeShot)
            {
                speed = 4;
            }
        }
        public void LoadContent()
        {
            if(inputAction == InputAction.Shoot)
            {
                fireball = content.Load<Texture2D>("sprites/fireball");
            }
            if(inputAction == InputAction.ChargeShot)
            {
                fireball = content.Load<Texture2D>("sprites/chargedfireball");
            }
        }
        public void UnloadContent()
        {
            content.Unload();
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(fireball, location, Color.White);
        }
        public void Update(GameTime gameTime)
        {
            UpdateLocation();
        }
        public void UpdateLocation()
        {
            Vector2 direction;
            if (playerIndex == PlayerIndex.One)
            {
                direction = new Vector2((float)Math.Cos(angle + (Math.PI / -2f)), (float)Math.Sin(angle + (Math.PI / -2f)));
                direction.Normalize();
                location += direction * speed;
            }
            else if(playerIndex == PlayerIndex.Two)
            {
                direction = new Vector2((float)Math.Cos(angle + (Math.PI / 2)), (float)Math.Sin(angle + (Math.PI / 2)));
                direction.Normalize();
                location += direction * speed;
            }
        }
    }
}
