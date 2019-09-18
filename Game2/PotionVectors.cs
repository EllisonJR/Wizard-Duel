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
    class PotionVectors
    {
        GraphicsDeviceManager graphics;

        int timeToNextVector;
        int vectorTimer;
        int vectorIndex;
        List<Vector2> vectors = new List<Vector2>();

        Random randomTime = new Random();
        Random randomVector = new Random();

        public PotionVectors(GraphicsDeviceManager graphics)
        {
            vectors.Add(new Vector2(graphics.PreferredBackBufferWidth / 4, graphics.PreferredBackBufferHeight / 4));
            vectors.Add(new Vector2(graphics.PreferredBackBufferWidth / 4, (graphics.PreferredBackBufferHeight / 4) * 2));
            vectors.Add(new Vector2(graphics.PreferredBackBufferWidth / 4, (graphics.PreferredBackBufferHeight / 4) * 3));

            vectors.Add(new Vector2(graphics.PreferredBackBufferWidth / 4 * 2, graphics.PreferredBackBufferHeight / 4));
            vectors.Add(new Vector2(graphics.PreferredBackBufferWidth / 4 * 2, (graphics.PreferredBackBufferHeight / 4) * 2));
            vectors.Add(new Vector2(graphics.PreferredBackBufferWidth / 4 * 2, graphics.PreferredBackBufferHeight / 4 * 3));

            vectors.Add(new Vector2(graphics.PreferredBackBufferWidth / 4 * 3, graphics.PreferredBackBufferHeight / 4));
            vectors.Add(new Vector2(graphics.PreferredBackBufferWidth / 4 * 3, graphics.PreferredBackBufferHeight / 4 * 2));
            vectors.Add(new Vector2(graphics.PreferredBackBufferWidth / 4 * 3, graphics.PreferredBackBufferHeight / 4 * 3));
            timeToNextVector = 2000;
        }
        public void Update(GameTime gameTime)
        {
            
            vectorTimer += gameTime.ElapsedGameTime.Milliseconds;
            if(vectorTimer > timeToNextVector)
            {
                vectorIndex = randomVector.Next(0, 9);
                vectorTimer = 0;
                timeToNextVector = randomVector.Next(2000, 5000);
            }

        }
        public Vector2 ReturnPotionDirection()
        {
            return vectors[vectorIndex];
        }
    }
}
