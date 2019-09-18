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
    enum Characters { Buff = 1, Tired = 2, Random = 3}
    class PlayerSelect : AssetContainer
    {
        GraphicsDeviceManager graphics;
        public GameStates currentGameState;

        public bool active;

        public List<Input> inputs = new List<Input>();

        Animation buffWizard;
        Animation tiredWizard;
        Animation random;
        Animation P1Select;
        Animation P2Select;
        public Animation toCombatTransition;
        public Animation toMenuTransition;
        public Animation toSelectTransition;
        public Animation backArrow;
        
        Vector2 zeroPlacement = new Vector2(0, 0);
        Vector2 donglePlacement1;
        Vector2 donglePlacement2;
        Vector2 donglePlacement3;

        Vector2 portraitPlacement1;
        Vector2 portraitPlacement2;
        Vector2 portraitPlacement3;

        Rectangle buffWizardRect;
        Rectangle tiredWizardRect;
        Rectangle randomRect;
        Rectangle backArrowRect;
        Rectangle playerConfirmCombatRect;

        bool player1Select;
        bool player2Select;

        int[] menuPointers = new int[2];

        int menuPointer1;
        int menuPointer2;

        bool justChosen1;
        bool justChosen2;

        bool buffChoice = false;
        bool tiredChoice = false;
        bool randomChoice = false;

        public bool toCombat = false;

        public PlayerSelect(GameStates currentGameState, GraphicsDeviceManager graphics)
        {
            this.currentGameState = currentGameState;
            this.graphics = graphics;

            buffWizard = new Animation(buffWizardT, 4, 5);
            tiredWizard = new Animation(tiredWizardT, 4, 8);
            random = new Animation(randomT, 2, 8);
            P1Select = new Animation(selecterT, 4, 10);
            P2Select = new Animation(selecterT, 4, 10);
            toMenuTransition = new Animation(toMenuTransitionT, 3, 2);
            toCombatTransition = new Animation(toCombatTransitionT, 4, 10);
            toSelectTransition = new Animation(toSelectTransitionT, 2, 5);
            backArrow = new Animation(backArrowT, 5, 5);

            portraitPlacement1 = new Vector2(75, 75);
            portraitPlacement2 = new Vector2(portraitPlacement1.X + buffWizard.width + 50, portraitPlacement1.Y);
            portraitPlacement3 = new Vector2(portraitPlacement1.X, portraitPlacement1.Y + buffWizard.height + 75);
            donglePlacement1 = new Vector2(portraitPlacement1.X, portraitPlacement1.Y - P1Select.height);
            donglePlacement2 = new Vector2(portraitPlacement2.X, portraitPlacement2.Y - P1Select.height);
            donglePlacement3 = new Vector2(portraitPlacement3.X, portraitPlacement3.Y - P1Select.height);

            buffWizardRect = new Rectangle((int)portraitPlacement1.X, (int)portraitPlacement1.Y, buffWizard.width, buffWizard.height);
            tiredWizardRect = new Rectangle((int)portraitPlacement2.X, (int)portraitPlacement2.Y, tiredWizard.width, tiredWizard.height);
            randomRect = new Rectangle((int)portraitPlacement3.X, (int)portraitPlacement3.Y, random.width, random.height);
            backArrowRect = new Rectangle((int)zeroPlacement.X, (int)zeroPlacement.Y, backArrow.width, backArrow.height);
            playerConfirmCombatRect = new Rectangle((int)zeroPlacement.X, (int)zeroPlacement.Y + 480, toCombatTransition.width, 48);

            toCombatTransition.frameTime = 50;

            backArrow.currentFrame = 0;
            toMenuTransition.currentFrame = 0;
            toCombatTransition.currentFrame = 0;
            toSelectTransition.currentFrame = -2;
            P1Select.currentFrame = 0;
            P2Select.currentFrame = 16;
        }
        public void LoadContent()
        {
            
        }
        public void Reset()
        {
            if (toCombat == true)
            {
                toCombatTransition.currentFrame = 0;
                toCombat = false;
                backArrow.currentFrame = 0;
                toMenuTransition.currentFrame = 0;
                toCombatTransition.currentFrame = 0;
                toSelectTransition.currentFrame = -2;
                P1Select.currentFrame = 0;
                P2Select.currentFrame = 16;
            }
        }
        public int PlayerOneChoice()
        {
            return menuPointer1;
        }
        public int PlayerTwoChoice()
        {
            return menuPointer2;
        }
        public void Update(GameTime gameTime)
        {
            if (!GamePad.GetCapabilities(PlayerIndex.One).IsConnected)
            {
                MouseInput(gameTime);
            }
            if (toSelectTransition.currentFrame < 6)
            {
                toSelectTransition.Update(gameTime);
            }
            if(backArrow.currentFrame < 16)
            {
                if (currentGameState == GameStates.PlayerSelectDouble)
                {
                    if (inputs[0].inputAction == InputAction.BackHold || inputs[1].inputAction == InputAction.BackHold)
                    {
                        backArrow.Update(gameTime);
                    }
                    else
                    {
                        if (backArrow.currentFrame != 0)
                        {
                            if (backArrow.currentFrame <= 8)
                            {
                                backArrow.currentFrame = 0;
                            }
                            backArrow.Reverse(gameTime);
                        }
                    }
                }
                else if(currentGameState == GameStates.PlayerSelectSingle)
                {
                    if (inputs[0].inputAction == InputAction.BackHold)
                    {
                        backArrow.Update(gameTime);
                    }
                    else
                    {
                        if (GamePad.GetCapabilities(PlayerIndex.One).IsConnected)
                        {
                            if (backArrow.currentFrame != 0)
                            {
                                if (backArrow.currentFrame <= 8)
                                {
                                    backArrow.currentFrame = 0;
                                }
                                backArrow.Reverse(gameTime);
                            }
                        }
                    }
                }
            }
            if(backArrow.currentFrame >= 16)
            {
                backArrow.Update(gameTime);
                if(backArrow.currentFrame >= 23)
                {
                    toMenuTransition.Update(gameTime);
                }
                
                if(toMenuTransition.currentFrame == 5)
                {
                    currentGameState = GameStates.MainMenu;
                }
            }

            if (toCombat == false)
            {
                AdjustMenuPointers(gameTime);
            }
            else if(toCombatTransition.currentFrame < 17)
            {
                toCombatTransition.currentFrame = 17;
            }
            else
            {
                toCombatTransition.frameTime = 100;
                toCombatTransition.Update(gameTime);
                if (toCombatTransition.currentFrame == 38)
                {
                    if (currentGameState == GameStates.PlayerSelectSingle)
                    {
                        currentGameState = GameStates.SinglePlayer;
                    }
                    else if (currentGameState == GameStates.PlayerSelectDouble)
                    {
                        currentGameState = GameStates.MultiPlayer;
                    }
                }
            }
            PointerAnimations(gameTime);
            IconAnimations(gameTime);
            PlayerSelectAnimations(gameTime);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            toSelectTransition.Draw(spriteBatch, zeroPlacement);
            buffWizard.Draw(spriteBatch, portraitPlacement1);
            tiredWizard.Draw(spriteBatch, portraitPlacement2);
            random.Draw(spriteBatch, portraitPlacement3);
            DrawMenuPointers(spriteBatch);
            toCombatTransition.Draw(spriteBatch, zeroPlacement);
            toMenuTransition.Draw(spriteBatch, zeroPlacement);
            backArrow.Draw(spriteBatch, zeroPlacement);
        }
        public void Reset(GameStates currentGameState)
        {
            if (currentGameState == GameStates.PlayerSelectDouble)
            {
                menuPointer1 = 1;
                menuPointer2 = 2;
                inputs.Add(new Input(ControlType.Menu, PlayerIndex.One, false));
                inputs.Add(new Input(ControlType.Menu, PlayerIndex.Two, false));

                player1Select = false;
                player2Select = false;

                justChosen1 = false;
                justChosen2 = false;

                inputs[0].inputAction = InputAction.None;
                inputs[1].inputAction = InputAction.None;
            }
            if (currentGameState == GameStates.PlayerSelectSingle)
            {
                inputs.Add(new Input(ControlType.Menu, PlayerIndex.One, false));
                
                menuPointer1 = 1;
                menuPointer2 = -1;
                player1Select = false;
                player2Select = true;
                justChosen2 = true;
                justChosen1 = false;
                inputs[0].inputAction = InputAction.None;
            }
        }
        public void MouseInput(GameTime gameTime)
        {
            if (player1Select == false)
            {
                if (buffWizardRect.Contains(inputs[0].mousePos))
                {
                    menuPointer1 = 1;
                }
                if (tiredWizardRect.Contains(inputs[0].mousePos))
                {
                    menuPointer1 = 2;
                }
                if (randomRect.Contains(inputs[0].mousePos))
                {
                    menuPointer1 = 3;
                }
            }
            if(backArrowRect.Contains(inputs[0].mousePos))
            {
                backArrow.Update(gameTime);
            }
            else if(!backArrowRect.Contains(inputs[0].mousePos) && backArrow.currentFrame < 15)
            {
                if(backArrow.currentFrame != 0)
                {
                    if (backArrow.currentFrame == 8)
                    {
                        backArrow.currentFrame = 0;
                    }
                    else if(backArrow.currentFrame > 8)
                    {
                        backArrow.Reverse(gameTime);
                    }
                }
            }
            if (player1Select == true)
            {
                if (!buffWizardRect.Contains(inputs[0].mousePos) && !tiredWizardRect.Contains(inputs[0].mousePos) && !randomRect.Contains(inputs[0].mousePos) && !backArrowRect.Contains(inputs[0].mousePos) && !playerConfirmCombatRect.Contains(inputs[0].mousePos))
                {
                    if(inputs[0].inputAction == InputAction.Confirm)
                    {
                        player1Select = false;
                        P1Select.currentFrame = 0;
                    }
                }
                else if (playerConfirmCombatRect.Contains(inputs[0].mousePos))
                {
                    if (inputs[0].inputAction == InputAction.Confirm)
                    {
                        toCombat = true;
                    }
                }
                if (inputs[0].inputAction == InputAction.Back)
                {
                    player1Select = false;
                    P1Select.currentFrame = 0;
                }
            }
        }
        public void AdjustMenuPointers(GameTime gameTime)
        {
            foreach (Input input in inputs)
            {
                input.Update(gameTime);
                if (currentGameState == GameStates.PlayerSelectSingle)
                {
                    if (player1Select == false)
                    {
                        PointerAdjustment(input.playerIndex, input.inputAction);
                    }
                    if (GamePad.GetCapabilities(PlayerIndex.One).IsConnected)
                    {
                        if (player1Select == true)
                        {
                            if (input.inputAction == InputAction.Back)
                            {
                                P1Select.currentFrame = 0;
                                player1Select = false;
                            }
                            if (toCombatTransition.currentFrame == 13)
                            {
                                if (input.inputAction == InputAction.Confirm)
                                {
                                    toCombat = true;
                                }
                            }
                        }
                    }
                }
                else if (currentGameState == GameStates.PlayerSelectDouble)
                {
                    if (player2Select == false && input.playerIndex == PlayerIndex.Two)
                    {
                        PointerAdjustment(input.playerIndex, input.inputAction);
                    }
                    if (player1Select == false && input.playerIndex == PlayerIndex.One)
                    {
                        PointerAdjustment(input.playerIndex, input.inputAction);
                    }
                    if (player2Select == true)
                    {
                        if (inputs[1].inputAction == InputAction.Back)
                        {
                            P2Select.currentFrame = 16;
                            player2Select = false;
                            if (menuPointer2 != menuPointer1)
                            {
                                if (buffChoice == true)
                                {
                                    buffChoice = false;
                                }
                                if (tiredChoice == true)
                                {
                                    tiredChoice = false;
                                }
                                if (randomChoice == true)
                                {
                                    randomChoice = false;
                                }
                            }
                            if (menuPointer2 == menuPointer1)
                            {
                                if (player1Select == false)
                                {
                                    if (buffChoice == true)
                                    {
                                        buffChoice = false;
                                    }
                                    if (tiredChoice == true)
                                    {
                                        tiredChoice = false;
                                    }
                                    if (randomChoice == true)
                                    {
                                        randomChoice = false;
                                    }
                                }
                            }
                        }
                    }
                    if (player1Select == true)
                    {
                        if (inputs[0].inputAction == InputAction.Back)
                        {
                            P1Select.currentFrame = 0;
                            player1Select = false;
                            if (menuPointer1 != menuPointer2)
                            {
                                if (buffChoice == true)
                                {
                                    buffChoice = false;
                                }
                                if (tiredChoice == true)
                                {
                                    tiredChoice = false;
                                }
                                if (randomChoice == true)
                                {
                                    randomChoice = false;
                                }
                            }
                            if (menuPointer1 == menuPointer2)
                            {
                                if (player2Select == false)
                                {
                                    if (buffChoice == true)
                                    {
                                        buffChoice = false;
                                    }
                                    if (tiredChoice == true)
                                    {
                                        tiredChoice = false;
                                    }
                                    if (randomChoice == true)
                                    {
                                        randomChoice = false;
                                    }
                                }
                            }
                        }
                    }
                }
                if (input.inputAction == InputAction.Confirm)
                {
                    if (input.playerIndex == PlayerIndex.One)
                    {
                        player1Select = true;
                    }
                    if (input.playerIndex == PlayerIndex.Two)
                    {
                        player2Select = true;
                    }
                    if (player1Select == true && player2Select == true)
                    {
                        if (GamePad.GetCapabilities(PlayerIndex.One).IsConnected)
                        {
                            if (toCombatTransition.currentFrame == 13)
                            {
                                if (input.inputAction == InputAction.Confirm)
                                {
                                    toCombat = true;
                                }
                            }
                        }
                    }
                }
            }
        }
        public void DrawMenuPointers(SpriteBatch spriteBatch)
        {
            if (menuPointer1 == 1)
            {
                P1Select.Draw(spriteBatch, donglePlacement1);
            }
            if(menuPointer1 == 2)
            {
                P1Select.Draw(spriteBatch, donglePlacement2);
            }
            if(menuPointer1 == 3)
            {
                P1Select.Draw(spriteBatch, donglePlacement3);
            }
            if(currentGameState == GameStates.PlayerSelectDouble)
            {
                if (menuPointer1 == menuPointer2)
                {
                    P2Select.Draw(spriteBatch, new Vector2(P1Select.drawingLocation.X + P1Select.width, P1Select.drawingLocation.Y));
                }
                else
                {
                    if (menuPointer2 == 1)
                    {
                        P2Select.Draw(spriteBatch, donglePlacement1);
                    }
                    if (menuPointer2 == 2)
                    {
                        P2Select.Draw(spriteBatch, donglePlacement2);
                    }
                    if (menuPointer2 == 3)
                    {
                        P2Select.Draw(spriteBatch, donglePlacement3);
                    }
                }
            }
        }
        public void IconAnimations(GameTime gameTime)
        {
            tiredWizard.Update(gameTime);
            buffWizard.Update(gameTime);
            random.Update(gameTime);

            if (currentGameState == GameStates.PlayerSelectSingle)
            {
                if (menuPointer1 == 1)
                {
                    if (player1Select == true)
                    {
                        if (P1Select.currentFrame == 11)
                        {
                            if (buffWizard.currentFrame < 5)
                            {
                                buffWizard.currentFrame = 5;
                            }
                        }
                        if (buffWizard.currentFrame == 18)
                        {
                            buffWizard.currentFrame = 13;
                        }
                    }
                    if (player1Select == false)
                    {
                        if (buffWizard.currentFrame >= 4)
                        {
                            buffWizard.currentFrame = 0;
                        }
                    }
                }
                else
                {
                    if (buffWizard.currentFrame >= 4)
                    {
                        buffWizard.currentFrame = 0;
                    }
                }
                if (menuPointer1 == 2)
                {
                    if (player1Select == true)
                    {
                        if (P1Select.currentFrame == 11)
                        {
                            if (tiredWizard.currentFrame < 10)
                            {
                                tiredWizard.currentFrame = 10;
                            }
                        }
                        if (tiredWizard.currentFrame == 28)
                        {
                            tiredWizard.currentFrame = 22;
                        }
                    }
                    if (player1Select == false)
                    {
                        if (tiredWizard.currentFrame >= 9)
                        {
                            tiredWizard.currentFrame = 0;
                        }
                    }
                }
                else
                {
                    if (tiredWizard.currentFrame >= 9)
                    {
                        tiredWizard.currentFrame = 0;
                    }
                }
                if (menuPointer1 == 3)
                {
                    if (player1Select == true)
                    {
                        if (P1Select.currentFrame == 11)
                        {
                            if (random.currentFrame < 7)
                            {
                                random.currentFrame = 7;
                            }
                        }
                        if (random.currentFrame == 9)
                        {
                            random.currentFrame = 6;
                        }
                    }
                    if (player1Select == false)
                    {
                        if (random.currentFrame >= 6)
                        {
                            random.currentFrame = 0;
                        }
                    }
                }
                else
                {
                    if (random.currentFrame >= 6)
                    {
                        random.currentFrame = 0;
                    }
                }
            }
            else if (currentGameState == GameStates.PlayerSelectDouble)
            {
                if(menuPointer1 == 1 && player1Select == true)
                {
                    buffChoice = true;
                }
                else if(menuPointer1 == 2 && player1Select == true)
                {
                    tiredChoice = true;
                }
                else if (menuPointer1 == 3 && player1Select == true)
                {
                    randomChoice = true;
                }
                if (menuPointer2 == 1 && player2Select == true)
                {
                    buffChoice = true;
                }
                else if (menuPointer2 == 2 && player2Select == true)
                {
                    tiredChoice = true;
                }
                else if (menuPointer2 == 3 && player2Select == true)
                {
                    randomChoice = true;
                }
                if(buffChoice == true)
                {
                    if (P1Select.currentFrame == 11 || P2Select.currentFrame == 27)
                    {
                        if (buffWizard.currentFrame < 5 && buffWizard.currentFrame < 13)
                        {
                            buffWizard.currentFrame = 5;
                        }
                    }
                    if (buffWizard.currentFrame == 18)
                    {
                        buffWizard.currentFrame = 13;
                    }
                }
                else
                {
                    if (buffWizard.currentFrame >= 4)
                    {
                        buffWizard.currentFrame = 0;
                    }
                }
                if(tiredChoice == true)
                {
                    if (P1Select.currentFrame == 11 || P2Select.currentFrame == 27)
                    {
                        if (tiredWizard.currentFrame < 10 && tiredWizard.currentFrame < 22)
                        {
                            tiredWizard.currentFrame = 10;
                        }
                    }
                    if (tiredWizard.currentFrame == 28)
                    {
                        tiredWizard.currentFrame = 22;
                    }
                }
                else
                {
                    if (tiredWizard.currentFrame >= 9)
                    {
                        tiredWizard.currentFrame = 0;
                    }
                }
                if(randomChoice == true)
                {
                    if (P1Select.currentFrame == 11 || P2Select.currentFrame == 27)
                    {
                        if (random.currentFrame < 7)
                        {
                            random.currentFrame = 7;
                        }
                    }
                    if (random.currentFrame == 9)
                    {
                        random.currentFrame = 7;
                    }
                }
                else
                {
                    if (random.currentFrame >= 6)
                    {
                        random.currentFrame = 0;
                    }
                }
            }
        }
        public void PlayerSelectAnimations(GameTime gameTime)
        {
            if (player1Select == true && player2Select == true)
            {
                if (toCombatTransition.currentFrame != 13)
                {
                    toCombatTransition.Update(gameTime);
                }
            }
            if (player1Select == false || player2Select == false)
            {
                if(toCombatTransition.currentFrame > 0)
                {
                    toCombatTransition.Update(gameTime);
                    if(toCombatTransition.currentFrame == 16)
                    {
                        toCombatTransition.currentFrame = 0;
                    }
                }
            }
        }
        public void PointerAnimations(GameTime gameTime)
        {
            if (player1Select == false)
            {
                P1Select.Update(gameTime);
                if (P1Select.currentFrame == 7)
                {
                    P1Select.currentFrame = 0;
                }
            }
            else if (player1Select == true)
            {
                if(P1Select.currentFrame <= 7)
                {
                    P1Select.Update(gameTime);
                    P1Select.currentFrame = 8;
                }
                if (P1Select.currentFrame < 15)
                {
                    P1Select.Update(gameTime);
                }
                else if(P1Select.currentFrame == 15)
                {

                }
            }
            if (player2Select == false)
            {
                P2Select.Update(gameTime);
                if (P2Select.currentFrame == 23)
                {
                    P2Select.currentFrame = 16;
                }
            }
            else if(player2Select == true)
            {
                if (P2Select.currentFrame <= 23)
                {
                    P2Select.Update(gameTime);
                    P2Select.currentFrame = 24;
                }
                if (P2Select.currentFrame < 31)
                {
                    P2Select.Update(gameTime);
                }
                else if (P2Select.currentFrame == 31)
                {

                }
            }
        }
        public void PointerAdjustment(PlayerIndex playerIndex, InputAction inputAction)
        {
            if (playerIndex == PlayerIndex.One)
            {
                if (inputAction == InputAction.RightMenu)
                {

                    if (menuPointer1 == 2)
                    {
                        menuPointer1 = 1;
                    }
                    else if (menuPointer1 == 3)
                    {
                        menuPointer1 = 2;
                    }
                    else
                    {
                        menuPointer1++;
                    }
                }
                if (inputAction == InputAction.Down)
                {
                    if (menuPointer1 == 1)
                    {
                        menuPointer1 = 3;
                    }
                    else if (menuPointer1 == 3)
                    {
                        menuPointer1 = 1;
                    }
                    else
                    {
                        menuPointer1++;
                    }
                }
                if (inputAction == InputAction.LeftMenu)
                {
                    if (menuPointer1 == 1)
                    {
                        menuPointer1 = 2;
                    }
                    else if (menuPointers[0] == 3)
                    {
                        menuPointer1 = 3;
                    }
                    else
                    {
                        menuPointer1--;
                    }
                }
                if (inputAction == InputAction.Up)
                {
                    if (menuPointer1 == 1)
                    {
                        menuPointer1 = 3;
                    }
                    else if (menuPointer1 == 3)
                    {
                        menuPointer1 = 1;
                    }
                    else if (menuPointer1 == 2)
                    {
                        menuPointer1 = 2;
                    }
                    else
                    {
                        menuPointer1--;
                    }
                }
            }
            if(playerIndex == PlayerIndex.Two)
            {
                if (inputAction == InputAction.RightMenu)
                {

                    if (menuPointer2 == 2)
                    {
                        menuPointer2 = 1;
                    }
                    else if (menuPointer2 == 3)
                    {
                        menuPointer2 = 2;
                    }
                    else
                    {
                        menuPointer2++;
                    }
                }
                if (inputAction == InputAction.Down)
                {
                    if (menuPointer2 == 1)
                    {
                        menuPointer2 = 3;
                    }
                    else if (menuPointer2 == 3)
                    {
                        menuPointer2 = 1;
                    }
                    else
                    {
                        menuPointer2++;
                    }
                }
                if (inputAction == InputAction.LeftMenu)
                {
                    if (menuPointer2 == 1)
                    {
                        menuPointer2 = 2;
                    }
                    else if (menuPointer2 == 3)
                    {
                        menuPointer2 = 3;
                    }
                    else
                    {
                        menuPointer2--;
                    }
                }
                if (inputAction == InputAction.Up)
                {
                    if (menuPointer2 == 1)
                    {
                        menuPointer2 = 3;
                    }
                    else if (menuPointer2 == 3)
                    {
                        menuPointer2 = 1;
                    }
                    else if (menuPointer2 == 2)
                    {
                        menuPointer2 = 2;
                    }
                    else
                    {
                        menuPointer2--;
                    }
                }
            }
        }
    }
}
