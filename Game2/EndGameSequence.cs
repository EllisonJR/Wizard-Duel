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
    class EndGameSequence
    {
        GraphicsDeviceManager graphics;
        ContentManager content;

        Input input;

        Texture2D player1CutInT;
        Texture2D player2CutInT;
        Texture2D beamPartT;

        Animation player1CutIn;
        Animation player2CutIn;
        Animation beamPart;

        Rectangle beamPartCollision;

        Vector2 cutInLocationSingle;
        Vector2 cutInLocationDouble1;
        Vector2 cutInLocationDouble2;
        List<Vector2> beamParts = new List<Vector2>();

        PlayerIndex playerIndex;

        bool draw = false;
        bool P1Win = false;
        bool P2Win = false;

        public int midWayPointP1;
        public int midWayPointP2;

        Characters winner1;
        Characters winner2;

        public int player1Xcoord;
        public int player2Xcoord;

        public EndGameSequence(ContentManager content, GraphicsDeviceManager graphics, PlayerIndex playerIndex, bool draw, Characters winner1, Characters winner2, int p1Width, int p2Width)
        {
            this.content = content;
            this.graphics = graphics;
            this.playerIndex = playerIndex;
            this.draw = draw;
            this.winner1 = winner1;
            this.winner2 = winner2;
            
            if(winner1 == Characters.Buff)
            {
                player1CutInT = content.Load<Texture2D>("full screen art/buffult.png");
                player1CutIn = new Animation(player2CutInT, 6, 10);
                cutInLocationDouble1 = new Vector2(0, graphics.PreferredBackBufferHeight / 2 - player2CutIn.height);
            }
            if (winner1 == Characters.Tired)
            {
                player1CutInT = content.Load<Texture2D>("full screen art/tiredult.png");
                player1CutIn = new Animation(player1CutInT, 6, 10);
                cutInLocationDouble2 = new Vector2(0, graphics.PreferredBackBufferHeight / 2 - player1CutIn.height);
            }
            if (winner2 == Characters.Tired)
            {
                player2CutInT = content.Load<Texture2D>("full screen art/tiredult.png");
                player2CutIn = new Animation(player1CutInT, 6, 10);
                cutInLocationDouble2 = new Vector2(0, graphics.PreferredBackBufferHeight / 2 - 90);
            }
            if (winner2 == Characters.Buff)
            {
                player2CutInT = content.Load<Texture2D>("full screen art/buffult.png");
                player2CutIn = new Animation(player2CutInT, 6, 10);
                cutInLocationDouble2 = new Vector2(0, graphics.PreferredBackBufferHeight / 2 - 90);
            }
            beamPartT = content.Load<Texture2D>("sprites/playerSprites/beampart");
            beamPart = new Animation(beamPartT, 1, 4);

            midWayPointP1 = graphics.PreferredBackBufferWidth / 2 - p1Width / 2;
            midWayPointP2 = graphics.PreferredBackBufferWidth / 2 - p2Width / 2;
        }
        public EndGameSequence(ContentManager content, GraphicsDeviceManager graphics, PlayerIndex playerIndex, bool P1Win, bool P2Win, Characters winner, int p1Width)
        {
            this.content = content;
            this.graphics = graphics;
            this.playerIndex = playerIndex;
            this.P1Win = P1Win;
            this.P2Win = P2Win;
            this.winner1 = winner;
            
            if(winner == Characters.Buff)
            {
                player1CutInT = content.Load<Texture2D>("full screen art/buffult.png");
                player1CutIn = new Animation(player2CutInT, 6, 10);
            }
            if(winner == Characters.Tired)
            {
                player1CutInT = content.Load<Texture2D>("full screen art/tiredult.png");
                player1CutIn = new Animation(player1CutInT, 5, 10);
            }
            beamPartT = content.Load<Texture2D>("sprites/playerSprites/beampart");
            beamPart = new Animation(beamPartT, 1, 4);

            midWayPointP1 = graphics.PreferredBackBufferWidth / 2 - p1Width / 2;
        }
        public void Update(GameTime gameTime)
        {
            UpdateInitialCutscene(gameTime);
            if(player1CutIn.currentFrame >= 25)
            {
                if(draw == true)
                {
                    TieBreaker();
                }
                else
                {
                    PlayerWin();
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            DrawInitialCutscene(spriteBatch);
        }
        public void PlayerWin()
        {
            MoveFirstCharacter(this.player1Xcoord);
        }
        public void TieBreaker()
        {
            MoveFirstCharacter(this.player1Xcoord);
            MoveSecondCharacters(this.player2Xcoord);
        }
        public void grabXcoords(int player1Xcoord, int player2Xcoord)
        {
            //this is called in the gameloop class
            if(draw == true)
            {
                this.player1Xcoord = player1Xcoord;
                this.player2Xcoord = player2Xcoord;
            }
            if(draw == false)
            {
                this.player1Xcoord = player1Xcoord;
            }
        }
        public int Player1Xcoords()
        {
            //this and the next are called in the gameloop class
            return player1Xcoord;
        }
        public int Player2Xcoords()
        {
            return player2Xcoord;
        }
        public void MoveFirstCharacter(int player1Xcoord)
        {
            if (this.player1Xcoord != midWayPointP1)
            {
                if (this.player1Xcoord > midWayPointP1 + 15)
                {
                    player1Xcoord -= 15;
                }
                if (player1Xcoord < midWayPointP1 - 15)
                {
                    player1Xcoord += 15;
                }
                if (player1Xcoord >= midWayPointP1 - 15 && player1Xcoord < midWayPointP1)
                {
                    player1Xcoord = midWayPointP1;
                }
                if (player1Xcoord <= midWayPointP1 + 15 && player1Xcoord > midWayPointP1)
                {
                    player1Xcoord = midWayPointP1;
                }
            }
        }
        public void MoveSecondCharacters(int playe2rXcoord)
        {
            if (player2Xcoord != midWayPointP2)
            {
                if (playe2rXcoord > midWayPointP2 + 15)
                {
                    player2Xcoord -= 15;
                }
                if (player2Xcoord < midWayPointP2 - 15)
                {
                    player2Xcoord += 15;
                }
                if (player2Xcoord >= midWayPointP2 - 15 && player2Xcoord < midWayPointP2)
                {
                    player2Xcoord = midWayPointP2;
                }
                if (player2Xcoord <= midWayPointP2 + 15 && player2Xcoord > midWayPointP2)
                {
                    player2Xcoord = midWayPointP2;
                }
            }
        }
        public void CalculateBeamLine()
        {

        }
        public void DrawInitialCutscene(SpriteBatch spriteBatch)
        {
            if(draw == true)
            {
                if (player2CutIn.currentFrame < 55)
                {
                    player1CutIn.Draw(spriteBatch, cutInLocationDouble1);
                    player2CutIn.Draw(spriteBatch, cutInLocationDouble2);
                }
            }
            if(draw == false)
            {
                player1CutIn.Draw(spriteBatch, cutInLocationSingle);
            }
        }
        public void UpdateInitialCutscene(GameTime gameTime)
        {
            if (draw == true)
            {
                if (player2CutIn.currentFrame < 55)
                {
                    player1CutIn.Update(gameTime);
                    player2CutIn.Update(gameTime);
                }
            }
            if (draw == false)
            {
                player1CutIn.Update(gameTime);
            }
        }
    }
}
