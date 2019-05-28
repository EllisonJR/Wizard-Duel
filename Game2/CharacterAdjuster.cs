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
    class CharacterAdjuster
    {
        ContentManager content;
        GraphicsDeviceManager graphics;

        PlayerIndex playerIndex;
        int character;
        InputAction inputAction;
        InputAction previousInputAction;

        int currentFrame;
        int playerSpeed;

        int frameTime = 100;
        int frameTimer;

        bool stunned;
        bool slowed;
        bool slept;
        bool incapped;

        ImpactLocations impactLocation;

        public CharacterAdjuster(ContentManager content, GraphicsDeviceManager graphics)
        {
            this.content = content;
            this.graphics = graphics;
            stunned = false;
            slowed = false;
            slept = false;
            incapped = false;
            impactLocation = ImpactLocations.None;
        }
        public int PassInFrame()
        {
            return currentFrame;
        }
        public void GrabInput(InputAction inputAction, PlayerIndex playerIndex, int speed, bool stunned, bool slowed, bool slept, bool incapped, ImpactLocations impactLocation)
        {
            this.playerIndex = playerIndex;
            this.inputAction = inputAction;
            playerSpeed = speed;
            this.stunned = stunned;
            this.slowed = slowed;
            this.slept = slept;
            this.incapped = incapped;
            this.impactLocation = impactLocation;
        }
        public void PassInCharacter(int character)
        {
            this.character = character;
        }
        public void UpdateCharacter(GameTime gameTime)
        {
            frameTimer += gameTime.ElapsedGameTime.Milliseconds;
            if (stunned == false && slept == false && incapped == false)
            {
                if (character == 1)
                {
                    BuffWizard();
                }
                if (character == 2)
                {
                    TiredWizard();
                }
            }
            else if(stunned == true || slept == true || incapped == true)
            {
                if (frameTimer >= frameTime)
                {
                    if (incapped == false)
                    {
                        currentFrame++;
                        frameTimer = 0;
                    }
                }
                if (character == 1)
                {
                    if (impactLocation == ImpactLocations.LeftImpact && incapped == false)
                    {
                        if (currentFrame == 0 || currentFrame >= 1 && currentFrame <= 4 || currentFrame >= 10 && currentFrame <= 18)
                        {
                            currentFrame = 72;
                        }
                        if (currentFrame == 9 || currentFrame >= 5 && currentFrame <= 8 || currentFrame >= 19 && currentFrame <= 27)
                        {
                            currentFrame = 82;
                        }
                        else
                        {
                            currentFrame = 72;
                        }
                    }
                    else if (impactLocation == ImpactLocations.RightImpact && incapped == false)
                    {
                        if (currentFrame == 0 || currentFrame >= 1 && currentFrame <= 4 || currentFrame >= 10 && currentFrame <= 18)
                        {
                            currentFrame = 92;
                        }
                        if (currentFrame == 9 || currentFrame >= 5 && currentFrame <= 8 || currentFrame >= 19 && currentFrame <= 27)
                        {
                            currentFrame = 98;
                        }
                        else
                        {
                            currentFrame = 92;
                        }
                    }
                    else if (stunned == true)
                    {
                        if (currentFrame == 81)
                        {
                            currentFrame = 77;
                        }
                        if (currentFrame == 91)
                        {
                            currentFrame = 87;
                        }
                        if (currentFrame == 97)
                        {
                            currentFrame = 79;
                        }
                        if (currentFrame == 103)
                        {
                            currentFrame = 89;
                        }
                    }
                    else if(slept == true)
                    {
                        if(currentFrame == 97)
                        {
                            currentFrame = 104;
                        }
                        if(currentFrame == 103)
                        {
                            currentFrame = 111;
                        }
                        if(currentFrame == 110)
                        {
                            currentFrame = 104;
                        }
                        if(currentFrame == 117)
                        {
                            currentFrame = 111;
                        }
                        if(currentFrame == 77)
                        {
                            currentFrame = 104;
                        }
                        if(currentFrame == 87)
                        {
                            currentFrame = 111;
                        }
                    }
                    else if (incapped == true)
                    {
                        if (currentFrame >= 0 && currentFrame <= 4 || currentFrame >= 10 && currentFrame <= 18)
                        {
                            currentFrame = 68;
                        }
                        if (currentFrame >= 5 && currentFrame <= 9 || currentFrame >= 19 && currentFrame <= 27)
                        {
                            currentFrame = 70;
                        }
                        if(currentFrame == 68)
                        {
                            currentFrame = 68;
                        }
                        if(currentFrame == 70)
                        {
                            currentFrame = 70;
                        }
                    }
                }
                if(character == 2)
                {
                    if (impactLocation == ImpactLocations.LeftImpact)
                    {
                        if (slept == true || stunned == true)
                        {
                            if (currentFrame == 0 || currentFrame >= 12 && currentFrame <= 21 || currentFrame >= 77 && currentFrame <= 90)
                            {
                                currentFrame = 101;
                            }
                            if (currentFrame == 1 || currentFrame >= 2 && currentFrame <= 11 || currentFrame >= 63 && currentFrame <= 76)
                            {
                                currentFrame = 110;
                            }
                            else
                            {
                                currentFrame = 110;
                            }
                        }
                    }
                    else if(impactLocation == ImpactLocations.RightImpact)
                    {
                        if (slept == true || stunned == true)
                        {
                            if (currentFrame == 0 || currentFrame >= 12 && currentFrame <= 21 || currentFrame >= 77 && currentFrame <= 90)
                            {
                                currentFrame = 116;
                            }
                            if (currentFrame == 1 || currentFrame >= 2 && currentFrame <= 11 || currentFrame >= 63 && currentFrame <= 76)
                            {
                                currentFrame = 92;
                            }
                            else
                            {
                                currentFrame = 92;
                            }
                        }
                    }
                    if (stunned == true)
                    {
                        if (currentFrame == 109)
                        {
                            currentFrame = 105;
                        }
                        if (currentFrame == 114)
                        {
                            currentFrame = 98;
                        }
                        if (currentFrame == 100)
                        {
                            currentFrame = 96;
                        }
                        if (currentFrame == 119)
                        {
                            currentFrame = 105;
                        }
                    }
                    if(slept == true)
                    {
                        if(currentFrame == 106)
                        {
                            currentFrame = 125;
                        }
                        if(currentFrame == 129)
                        {
                            currentFrame = 125;
                        }
                        if(currentFrame == 114)
                        {
                            currentFrame = 98;
                        }
                        if(currentFrame == 100)
                        {
                            currentFrame = 120;
                        }
                        if(currentFrame == 119)
                        {
                            currentFrame = 107;
                        }
                        if(currentFrame == 109)
                        {
                            currentFrame = 125;
                        }
                        if(currentFrame == 129)
                        {
                            currentFrame = 125;
                        }
                        if(currentFrame == 97)
                        {
                            currentFrame = 120;
                        }
                        if(currentFrame == 124)
                        {
                            currentFrame = 120;
                        }
                    }
                    if (incapped == true)
                    {
                        if (currentFrame == 0 || currentFrame >= 12 && currentFrame <= 21 || currentFrame >= 77 && currentFrame <= 90)
                        {
                            currentFrame = 129;
                        }
                        if (currentFrame == 1 || currentFrame >= 2 && currentFrame <= 11 || currentFrame >= 63 && currentFrame <= 76)
                        {
                            currentFrame = 130;
                        }
                        if (currentFrame == 129)
                        {
                            currentFrame = 129;
                        }
                        if (currentFrame == 130)
                        {
                            currentFrame = 130;
                        }
                    }
                }
            }
        }
        public void TiredWizard()
        {
            
            if (inputAction != InputAction.None)
            {
                if (frameTimer >= frameTime)
                {
                    currentFrame++;
                    frameTimer = 0;
                }
                if (inputAction == InputAction.Left)
                {
                    if (slowed == true)
                    {
                        frameTime = 150;
                    }
                    else
                    {
                        frameTime = 50;
                    }
                    if (currentFrame <= 12 || currentFrame > 21)
                    {
                        currentFrame = 12;
                    }
                }
                if (inputAction == InputAction.Right)
                {
                    if (slowed == true)
                    {
                        frameTime = 150;
                    }
                    else
                    {
                        frameTime = 50;
                    }
                    if (currentFrame <= 2 || currentFrame > 11)
                    {
                        currentFrame = 2;
                    }
                }
                if (slowed == false)
                {
                    if (inputAction == InputAction.DashLeft)
                    {
                        frameTime = 25;
                        if (currentFrame < 77 || currentFrame > 90)
                        {
                            currentFrame = 77;
                        }
                    }
                    if (inputAction == InputAction.DashRight)
                    {
                        frameTime = 25;
                        if (currentFrame < 63 || currentFrame > 76)
                        {
                            currentFrame = 63;
                        }
                    }
                }
                if (playerIndex == PlayerIndex.One)
                {
                    if (inputAction == InputAction.Reflect)
                    {
                        frameTime = 75;
                        if (currentFrame < 22)
                        {
                            currentFrame = 22;
                        }
                        if (currentFrame >= 26)
                        {
                            currentFrame = 26;
                        }
                    }
                    if (inputAction == InputAction.Charge)
                    {
                        frameTime = 50;
                        if (currentFrame < 33 || currentFrame > 39)
                        {
                            currentFrame = 33;
                        }
                    }
                    if (inputAction == InputAction.ChargeShotReady)
                    {
                        if (currentFrame >= 48)
                        {
                            currentFrame = 41;
                        }
                    }
                }
                if (playerIndex == PlayerIndex.Two)
                {
                    if (inputAction == InputAction.Reflect)
                    {
                        frameTime = 75;
                        if (currentFrame < 27)
                        {
                            currentFrame = 27;
                        }
                        if (currentFrame >= 31)
                        {
                            currentFrame = 31;
                        }
                    }
                    if (inputAction == InputAction.Charge)
                    {
                        frameTime = 50;
                        if (currentFrame < 49 || currentFrame > 54)
                        {
                            currentFrame = 49;
                        }
                    }
                    if (inputAction == InputAction.ChargeShotReady)
                    {
                        if (currentFrame > 62)
                        {
                            currentFrame = 56;
                        }
                    }
                }
                if (inputAction == InputAction.ChargeShot)
                {
                    currentFrame = 0;
                }
            }
            else if (inputAction == InputAction.None)
            {
                frameTime = 100;
                if (currentFrame >= 12 && currentFrame <= 21)
                {
                    currentFrame = 0;
                }
                if (currentFrame >= 2 && currentFrame <= 11)
                {
                    currentFrame = 1;
                }
                if (currentFrame >= 77 && currentFrame <= 90)
                {
                    currentFrame = 0;
                }
                if (currentFrame == 0)
                {
                    currentFrame = 0;
                }
                if (currentFrame == 1)
                {
                    currentFrame = 1;
                }
                if (currentFrame >= 63 && currentFrame <= 76)
                {
                    currentFrame = 1;
                }
                if (currentFrame >= 22 && currentFrame <= 26)
                {
                    currentFrame = 0;
                }
                if (currentFrame >= 33 && currentFrame <= 48)
                {
                    currentFrame = 0;
                }
                if (currentFrame > 60)
                {
                    currentFrame = 0;
                }
                if (currentFrame >= 27 && currentFrame <= 31)
                {
                    currentFrame = 0;
                }
                if (currentFrame >= 49 && currentFrame <= 62)
                {
                    currentFrame = 0;
                }
            }
        }
        public void BuffWizard()
        {
            if (inputAction != InputAction.None)
            {
                if (frameTimer >= frameTime)
                {
                    currentFrame++;
                    frameTimer = 0;
                }
                if (inputAction == InputAction.Left)
                {
                    frameTime = 100;
                    if (currentFrame <= 5 || currentFrame > 8)
                    {
                        currentFrame = 5;
                    }
                }
                if (inputAction == InputAction.Right)
                {
                    frameTime = 100;
                    if (currentFrame <= 1 || currentFrame > 4)
                    {
                        currentFrame = 1;
                    }
                }
                if(inputAction == InputAction.DashLeft)
                {
                    frameTime = 50;
                    if(currentFrame < 19 || currentFrame > 27)
                    {
                        currentFrame = 19;
                    }
                }
                if(inputAction == InputAction.DashRight)
                {
                    frameTime = 50;
                    if(currentFrame < 10 || currentFrame > 18)
                    {
                        currentFrame = 10;
                    }
                }
                if (playerIndex == PlayerIndex.One)
                {
                    if (inputAction == InputAction.Reflect)
                    {
                        frameTime = 75;
                        if (currentFrame < 44)
                        {
                            currentFrame = 44;
                        }
                        if (currentFrame >= 47)
                        {
                            currentFrame = 47;
                        }
                    }
                    if (inputAction == InputAction.Charge)
                    {
                        frameTime = 50;
                        if (currentFrame < 28 || currentFrame > 34)
                        {
                            currentFrame = 28;
                        }
                    }
                    if (inputAction == InputAction.ChargeShotReady)
                    {
                        if (currentFrame >= 43)
                        {
                            currentFrame = 36;
                        }
                    }
                }
                if(playerIndex == PlayerIndex.Two)
                {
                    if (inputAction == InputAction.Reflect)
                    {
                        frameTime = 75;
                        if (currentFrame < 48)
                        {
                            currentFrame = 48;
                        }
                        if (currentFrame >= 51)
                        {
                            currentFrame = 51;
                        }
                    }
                    if (inputAction == InputAction.Charge)
                    {
                        frameTime = 50;
                        if (currentFrame < 52 || currentFrame > 58)
                        {
                            currentFrame = 52;
                        }
                    }
                    if (inputAction == InputAction.ChargeShotReady)
                    {
                        if (currentFrame > 66)
                        {
                            currentFrame = 60;
                        }
                    }
                }
                if(inputAction == InputAction.ChargeShot)
                {
                    currentFrame = 0;
                }
            }
            else if(inputAction == InputAction.None)
            {
                frameTime = 100;
                if (currentFrame >= 5 && currentFrame <= 8)
                {
                    currentFrame = 9;
                }
                if (currentFrame >= 1 && currentFrame <= 4)
                {
                    currentFrame = 0;
                }
                if(currentFrame >= 44 && currentFrame <= 47)
                {
                    currentFrame = 0;
                }
                if(currentFrame == 0)
                {
                    currentFrame = 0;
                }
                if(currentFrame == 9)
                {
                    currentFrame = 9;
                }
                if(currentFrame >= 19 && currentFrame <= 27)
                {
                    currentFrame = 9;
                }
                if(currentFrame >= 10 && currentFrame <= 18)
                {
                    currentFrame = 0;
                }
                if(currentFrame >= 28 && currentFrame <= 34)
                {
                    currentFrame = 0;
                }
                if (currentFrame > 60)
                {
                    currentFrame = 0;
                }
                if(currentFrame >= 48 && currentFrame <= 51)
                {
                    currentFrame = 0;
                }
                if(currentFrame >= 52 && currentFrame <= 58)
                {
                    currentFrame = 0;
                }
            }
        }
    }
}
