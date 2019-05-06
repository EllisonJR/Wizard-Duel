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
    class Boundary
    {
        GraphicsDeviceManager graphics;
        ContentManager content;

        Texture2D border;
        Vector2 borderLocation;
        public Rectangle bounds { get; set; }
        Texture2D pillarT;

        Animation pillarA1;
        Animation pillarA2;
        Animation pillarA3;
        Animation pillarA4;

        Vector2 pillar1;
        Vector2 pillar2;
        Vector2 pillar3;
        Vector2 pillar4;

        public Vector2 leftSide;
        public Vector2 rightSide;

        float boundaryDistance;

        float alpha;

        public List<Vector2> boundaryRenderLocs = new List<Vector2>();

        List<Projectile> projLocs = new List<Projectile>();

        public Boundary(ContentManager content, GraphicsDeviceManager graphics)
        {
            this.content = content;
            this.graphics = graphics;
        }
        public void Loadcontent()
        {
            border = content.Load<Texture2D>("sprites/fireball");
            pillarT = content.Load<Texture2D>("sprites/border items/pillar");
            pillarA1 = new Animation(pillarT, 2, 3);
            pillarA2 = new Animation(pillarT, 2, 3);
            pillarA3 = new Animation(pillarT, 2, 3);
            pillarA4 = new Animation(pillarT, 2, 3);

            pillar1 = new Vector2(0,0);
            pillar2 = new Vector2(graphics.PreferredBackBufferWidth - pillarA1.width, 0);
            pillar3 = new Vector2(0, graphics.PreferredBackBufferHeight - pillarA1.height);
            pillar4 = new Vector2(graphics.PreferredBackBufferWidth - pillarA1.width, graphics.PreferredBackBufferHeight - pillarA1.height);

            bounds = new Rectangle((int)(pillar1.X + pillarA1.width / 2), 0, graphics.PreferredBackBufferWidth - pillarA1.width, graphics.PreferredBackBufferHeight);
            rightSide = new Vector2(bounds.Right, pillar2.Y + pillarA2.height / 2);
            leftSide = new Vector2(bounds.Left, pillar2.Y + pillarA2.height / 2);

            boundaryDistance = (pillar3.Y + (pillarA1.height / 2)) - (pillar1.Y + (pillarA2.height / 2));
            pillarA1.currentFrame = 3;
            pillarA2.currentFrame = 3;
            pillarA3.currentFrame = 0;
            pillarA4.currentFrame = 0;
            SetRenderTargets();
        }
        public void UnloadContent()
        {
            content.Unload();
        }
        public void Update(GameTime gameTime)
        {
            pillarA1.Update(gameTime);
            pillarA2.Update(gameTime);
            pillarA3.Update(gameTime);
            pillarA4.Update(gameTime);

            if(pillarA3.currentFrame == 3)
            {
                pillarA3.currentFrame = 0;
            }
            if (pillarA4.currentFrame == 3)
            {
                pillarA4.currentFrame = 0;
            }
            if (pillarA1.currentFrame == 6)
            {
                pillarA1.currentFrame = 3;
            }
            if (pillarA2.currentFrame == 6)
            {
                pillarA2.currentFrame = 3;
            }
        }
        public void SetRenderTargets()
        {
            for (int i = 0; i <= boundaryDistance; i++)
            {
                boundaryRenderLocs.Add(new Vector2((int)rightSide.X, (int)rightSide.Y + i));
                boundaryRenderLocs.Add(new Vector2((int)leftSide.X, (int)leftSide.Y + i));
            }
        }
        public void GrabPorjectileLoc(List<Projectile> locs)
        {
            projLocs = locs;
        }
        public void Draw(SpriteBatch spriteBatch)
        {

            foreach(Vector2 boundaryRender in boundaryRenderLocs)
            {
                foreach (Projectile vector in projLocs)
                {
                    if ((vector.location - boundaryRender).Length() <= 50)
                    {
                        
                        spriteBatch.Draw(border, boundaryRender, new Rectangle((int)boundaryRender.X, (int)boundaryRender.Y, 1, 1), Color.White * ((boundaryRender - vector.location).Length() / .100f));
                    }
                }
            }

            pillarA1.Draw(spriteBatch, pillar1);
            pillarA2.Draw(spriteBatch, pillar2);
            pillarA3.Draw(spriteBatch, pillar3);
            pillarA4.Draw(spriteBatch, pillar4);
        }
    }
}
