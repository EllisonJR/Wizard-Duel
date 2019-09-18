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
    class ControlInfoScreen : AssetContainer
    {
        public GameStates currentGameState { get; set; }
        GraphicsDeviceManager graphics;

        public bool fromCombat;

        public Animation nonCombatTransition;
        public Animation upwarpSwipeTransition;

        bool introTransitionDone = false;
        bool outroTransitionStarted = false;

        Vector2 zeroLocation = new Vector2(0, 0);

        public Input input;

        public GameStates outsideGamestate;

        public int timer;

        public ControlInfoScreen(GameStates currentGameState, GraphicsDeviceManager graphics)
        {
            this.currentGameState = currentGameState;
            this.graphics = graphics;
            input = new Input(ControlType.Menu, PlayerIndex.One, false);
            fromCombat = false;

            nonCombatTransition = new Animation(nonCombatTransitionT, 2, 5);
            upwarpSwipeTransition = new Animation(upwardSwipeTransitionT, 3, 2);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            DrawTransitions(spriteBatch);
            if (GamePad.GetCapabilities(PlayerIndex.One).IsConnected == true)
            {
                spriteBatch.Draw(controlcontrols, new Vector2(0, 0), Color.White);
            }
            else
            {
                spriteBatch.Draw(keyboardcontrols, new Vector2(0, 0), Color.White);
            }
        }
        public void Update(GameTime gameTime)
        {
            if (timer < 100)
            {
                input.inputAction = InputAction.None;
            }
            UpdateTransitions(gameTime);
            timer += gameTime.ElapsedGameTime.Milliseconds;
            
            if (introTransitionDone == true || outroTransitionStarted == false)
            {
                input.Update(gameTime);
            }
            if(nonCombatTransition.currentFrame >= 7 && outsideGamestate == GameStates.MainMenu)
            {
                currentGameState = GameStates.MainMenu;
                introTransitionDone = false;
                outroTransitionStarted = false;
            }
            else if(nonCombatTransition.currentFrame >= 7 && outsideGamestate == GameStates.SinglePlayer)
            {
                currentGameState = GameStates.SinglePlayer;
                introTransitionDone = false;
                outroTransitionStarted = false;
            }
            else if (nonCombatTransition.currentFrame >= 7 && outsideGamestate == GameStates.MultiPlayer)
            {
                currentGameState = GameStates.MultiPlayer;
                introTransitionDone = false;
                outroTransitionStarted = false;
            }
        }
        public void LoadContent()
        {
            
        }
        public void UnloadContent()
        {
            content.Unload();
        }
        public void DrawTransitions(SpriteBatch spriteBatch)
        {
            if(introTransitionDone == false)
            {
                upwarpSwipeTransition.Draw(spriteBatch, zeroLocation);
            }
            if(outroTransitionStarted == true)
            {
                nonCombatTransition.Draw(spriteBatch, zeroLocation);
            }
        }
        public void UpdateTransitions(GameTime gameTime)
        {
            if(input.inputAction == InputAction.Confirm && timer > 100)
            {
                
                outroTransitionStarted = true;
            }
            if(upwarpSwipeTransition.currentFrame >= 6)
            {
                introTransitionDone = true;
            }
            if(introTransitionDone == false)
            {
                upwarpSwipeTransition.Update(gameTime);
            }
            if(outroTransitionStarted == true)
            {
                nonCombatTransition.Update(gameTime);
            }
        }
    }
}
