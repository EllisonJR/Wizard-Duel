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
    class MainMenu
    {
        ContentManager content;
        GraphicsDeviceManager graphics;
        public Input input;

        Animation titleAnimation1;
        Animation titleAnimation2;
        Animation titleAnimation3;

        Animation controlsButton;
        Animation exitButton;
        Animation singleplayerButton;
        Animation statsButton;
        Animation versusButton;

        Animation controlsIdle;
        Animation exitIdle;
        Animation singleplayerIdle;
        Animation versusIdle;
        Animation statsIdle;
        Animation graphicIdle;
        Animation fireballAnimation;
        Animation explosionAnimation;
        Animation upwardWipeTransition;
        Animation nonCombatTransition;

        SpriteFont font;

        bool multiplayerDenial;

        public GameStates currentGamestate { get; set; }

        int menuPointer;
        MouseState mouse;
        Point mousePoint;
        Vector2 pointerPosition;
        double UItimer;
        int titleCounter = 0;

        Texture2D statsButtonT;
        Texture2D versusButtonT;
        Texture2D controlInfoButtonT;
        Texture2D exitButtonT;
        Texture2D singleplayerbuttonT;
        Texture2D title1;
        Texture2D title2;
        Texture2D title3;
        Texture2D menuIdle;
        Texture2D fireballAnimationT;
        Texture2D explosionAnimationT;
        Texture2D upwardWipeTransitionT;
        Texture2D nonCombatTransitionT;

        Vector2 zeroPlacement = new Vector2(0, 0);
        Vector2 titlePlacement = new Vector2(0, -100);
        Vector2 singlePlayerFireballPlacement;
        Vector2 versusFireballPlacement;
        Vector2 versusPlacement;
        Vector2 controlInfoPlacement;
        Vector2 exitPlacement;
        Vector2 singleplayerPlacement;
        Vector2 statsPlacement;
        Vector2 graphicIdlePlacement;
        Vector2 fireBallPlacement;
        Vector2 singleExplosionPlacement;
        Vector2 versusExplosionPlacement;

        Rectangle statsRect;
        Rectangle versusRect;
        Rectangle controlInfoRect;
        Rectangle exitRect;
        Rectangle singlePlayerRect;

        bool titleanimation = true;
        bool titledone = false;
        bool menuintro = false;
        public bool playerHasChosen = false;
        bool menuSelectAnimationDone = false;
        public bool fromNonCombat = false;

        public MainMenu(GameStates currentGameState, ContentManager content, GraphicsDeviceManager graphics)
        {
            this.currentGamestate = currentGameState;
            this.content = content;
            this.graphics = graphics;
            input = new Input(ControlType.Menu, PlayerIndex.One, false);
            multiplayerDenial = false;
        }

        public void LoadContent()
        {
            versusButtonT = content.Load<Texture2D>("menu buttons/versusbutton");
            controlInfoButtonT = content.Load<Texture2D>("menu buttons/controlsbutton");
            exitButtonT = content.Load<Texture2D>("menu buttons/exitbutton");
            statsButtonT = content.Load<Texture2D>("menu buttons/statsbutton");
            font = content.Load<SpriteFont>("fonts/gameclock");
            singleplayerbuttonT = content.Load<Texture2D>("menu buttons/singleplayerbutton");
            title1 = content.Load<Texture2D>("full screen art/titlescreen_1");
            title2 = content.Load<Texture2D>("full screen art/titlescreen_2");
            title3 = content.Load<Texture2D>("full screen art/titlescreen_3");
            menuIdle = content.Load<Texture2D>("full screen art/menuidle");
            fireballAnimationT = content.Load<Texture2D>("transitions/menuconfirmfireball");
            explosionAnimationT = content.Load<Texture2D>("transitions/explodetransition");
            nonCombatTransitionT = content.Load<Texture2D>("transitions/tononcombatscreentransition");
            upwardWipeTransitionT = content.Load<Texture2D>("transitions/upwardscreenwipetransitionin");

            titleAnimation1 = new Animation(title1, 4, 10);
            titleAnimation2 = new Animation(title2, 5, 10);
            titleAnimation3 = new Animation(title3, 6, 5);
            versusButton = new Animation(versusButtonT, 5, 5);
            controlsButton = new Animation(controlInfoButtonT, 4, 6);
            singleplayerButton = new Animation(singleplayerbuttonT, 4, 6);
            statsButton = new Animation(statsButtonT, 3, 5);
            exitButton = new Animation(exitButtonT, 5, 5);
            graphicIdle = new Animation(menuIdle, 7, 10);
            fireballAnimation = new Animation(fireballAnimationT, 2, 2);
            explosionAnimation = new Animation(explosionAnimationT, 6, 3);
            nonCombatTransition = new Animation(nonCombatTransitionT, 2, 5);
            upwardWipeTransition = new Animation(upwardWipeTransitionT, 3, 2);

            graphicIdlePlacement = new Vector2(graphics.PreferredBackBufferWidth - graphicIdle.width, graphics.PreferredBackBufferHeight - graphicIdle.height);
            versusPlacement = new Vector2(0, singleplayerButton.height);
            controlInfoPlacement = new Vector2(0, singleplayerButton.height + versusButton.height);
            exitPlacement = new Vector2(0, singleplayerButton.height + versusButton.height + controlsButton.height + statsButton.height);
            singleplayerPlacement = new Vector2(0,0);
            statsPlacement = new Vector2(0, singleplayerButton.height + versusButton.height + controlsButton.height);
            singlePlayerFireballPlacement = new Vector2(singleplayerPlacement.X - fireballAnimation.width, singleplayerPlacement.Y);
            versusFireballPlacement = new Vector2(versusPlacement.X - fireballAnimation.width, versusPlacement.Y);
            singleExplosionPlacement = new Vector2(0, 0 + (singleplayerButton.height / 2) - (explosionAnimation.height / 2));
            versusExplosionPlacement = new Vector2(0, 0 + (singleplayerButton.height + (versusButton.height / 2)) - (explosionAnimation.height / 2));

            statsRect = new Rectangle((int)statsPlacement.X, (int)statsPlacement.Y, statsButton.width, statsButton.height);
            versusRect = new Rectangle((int)versusPlacement.X, (int)versusPlacement.Y, versusButton.width, versusButton.height);
            singlePlayerRect = new Rectangle((int)singleplayerPlacement.X, (int)singleplayerPlacement.Y, singleplayerButton.width, singleplayerButton.height);
            controlInfoRect = new Rectangle((int)controlInfoPlacement.X, (int)controlInfoPlacement.Y, controlsButton.width, controlsButton.height);
            exitRect = new Rectangle((int)exitPlacement.X, (int)exitPlacement.Y, exitButton.width, exitButton.height);
        }

        public void UnloadContent()
        {
            content.Unload();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (fromNonCombat == true)
            {
                upwardWipeTransition.Draw(spriteBatch, zeroPlacement);
            }
            if (titleanimation == true)
            {
                TitleAnimationDraw(spriteBatch);
            }
            if(menuintro == true)
            {
                MenuAnimationIntroDraw(spriteBatch);
            }
            else if(titleanimation == false && menuintro == false)
            {
                if (playerHasChosen == true)
                {
                    TransitionAnimations(spriteBatch);
                }
                MenuAnimationsDraw(spriteBatch);
                graphicIdle.Draw(spriteBatch, graphicIdlePlacement);
                if(multiplayerDenial == true)
                {
                    spriteBatch.DrawString(font, "Please connect \ntwo controllers.", new Vector2(versusPlacement.X + versusButton.width, versusPlacement.Y + 10), Color.White);
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            if (titleanimation == true)
            {
                TitleAnimationUpdate(gameTime);
            }
            if(menuintro == true)
            {
                MenuAnimationIntroUpdate(gameTime);
            }
            else if(titleanimation == false && menuintro == false)
            {
                if (fromNonCombat == true)
                {
                    upwardWipeTransition.Update(gameTime);
                    if (upwardWipeTransition.currentFrame == 5)
                    {
                        fromNonCombat = false;
                    }
                }
                menuIdleAnimations(gameTime);
                MenuAnimationsUpdate(gameTime);

                if(multiplayerDenial == true)
                {
                    Denial();
                }
                if (playerHasChosen == true)
                {
                    TransitionAnimationFrameLogic(gameTime);
                }
                else
                {
                    if(fromNonCombat == false)
                    {
                        if (GamePad.GetCapabilities(PlayerIndex.One).IsConnected == false)
                        {
                            MousePointer();
                        }
                        input.Update(gameTime);
                    }
                }
                
                if (input.inputAction == InputAction.Down)
                {
                    menuPointer++;
                    if (menuPointer > 4)
                    {
                        menuPointer = 0;
                    }
                }
                if (input.inputAction == InputAction.Up)
                {
                    menuPointer--;
                    if (menuPointer < 0)
                    {
                        menuPointer = 4;
                    }
                }
                if (input.inputAction == InputAction.Confirm)
                {
                    switch (menuPointer)
                    {
                        case 0:
                            playerHasChosen = true;
                            if (explosionAnimation.currentFrame >= explosionAnimation.totalFrames)
                            {
                                menuSelectAnimationDone = false;
                                playerHasChosen = false;
                                currentGamestate = GameStates.PlayerSelectSingle;
                            }
                            break;
                        case 1:
                            if (GamePad.GetCapabilities(PlayerIndex.Two).IsConnected == true)
                            {
                                playerHasChosen = true;
                                if (explosionAnimation.currentFrame >= explosionAnimation.totalFrames)
                                {
                                    menuSelectAnimationDone = false;
                                    playerHasChosen = false;
                                    currentGamestate = GameStates.PlayerSelectDouble;
                                }
                            }
                            else
                            {
                                multiplayerDenial = true;
                            }
                            break;
                        case 2:
                            playerHasChosen = true;
                            if (nonCombatTransition.currentFrame == 6)
                            {
                                menuSelectAnimationDone = false;
                                playerHasChosen = false;
                                currentGamestate = GameStates.ControlScreen;
                            }
                            break;
                        case 3:
                                
                            break;
                        case 4:
                            currentGamestate = GameStates.Exit;
                            break;

                    }
                }
                
            }
        }
        
        public void MousePointer()
        {
            if(singlePlayerRect.Contains(input.mousePos))
            {
                menuPointer = 0;
            }
            if(versusRect.Contains(input.mousePos))
            {
                menuPointer = 1;
            }
            if(controlInfoRect.Contains(input.mousePos))
            {
                menuPointer = 2;
            }
            if(statsRect.Contains(input.mousePos))
            {
                menuPointer = 3;
            }
            if(exitRect.Contains(input.mousePos))
            {
                menuPointer = 4;
            }
        }
        public void Denial()
        {
            if(menuPointer != 1)
            {
                multiplayerDenial = false;
            }
        }
        public void TitleAnimationDraw(SpriteBatch spriteBatch)
        {
            if (titleAnimation1.currentFrame < 40)
            {
                titleAnimation1.Draw(spriteBatch, titlePlacement);
            }
            else if (titleAnimation1.currentFrame == 40 && titleAnimation2.currentFrame < 40)
            {
                titleAnimation2.Draw(spriteBatch, titlePlacement);
            }
            else if (titleAnimation2.currentFrame == 40 && titleAnimation3.currentFrame < 11)
            {
                titleAnimation3.Draw(spriteBatch, titlePlacement);
            }
            else if (titleAnimation3.currentFrame >= 11)
            {
                titleAnimation3.Draw(spriteBatch, titlePlacement);
            }
        }
        public void TitleAnimationUpdate(GameTime gameTime)
        {
            if (titleAnimation1.currentFrame < 40)
            {
                titleAnimation1.Update(gameTime);
            }
            else if (titleAnimation1.currentFrame == titleAnimation1.totalFrames && titleAnimation2.currentFrame < 40)
            {
                titleAnimation2.Update(gameTime);
            }
            else if (titleAnimation2.currentFrame == 40 && titleAnimation3.currentFrame >= 0 && titleAnimation3.currentFrame <= 10)
            {
                input.Update(gameTime);
                titleAnimation3.Update(gameTime);
                if (input.inputAction == InputAction.Confirm)
                {
                    titleAnimation3.currentFrame = 11;
                    titleAnimation2.currentFrame = 41;
                    titleAnimation1.currentFrame = 41;
                }
                if (titleAnimation3.currentFrame == 10)
                {
                    titledone = true;
                    titleAnimation3.currentFrame = 0;
                }
            }
            else if (titleAnimation3.currentFrame >= 11)
            {
                titleAnimation3.Update(gameTime);
                if (titleAnimation3.currentFrame == 16 && titleCounter < 2)
                {
                    titleAnimation3.currentFrame = 12;
                    titleCounter++;
                }
                if (titleCounter == 2)
                {
                    if (titleAnimation3.currentFrame < 16)
                    {
                        titleAnimation3.currentFrame = 16;
                    }
                    titleAnimation3.Update(gameTime);
                }
                if (titleAnimation3.currentFrame == 30)
                {
                    menuintro = true;
                    titleanimation = false;
                }
            }
        }
        public void MenuAnimationIntroDraw(SpriteBatch spriteBatch)
        {
            singleplayerButton.Draw(spriteBatch, singleplayerPlacement);
            if (singleplayerButton.currentFrame >= 3)
            {
                versusButton.Draw(spriteBatch, versusPlacement);
            }
            if (versusButton.currentFrame >= 3)
            {
                controlsButton.Draw(spriteBatch, controlInfoPlacement);
            }
            if (controlsButton.currentFrame >= 3)
            {
                statsButton.Draw(spriteBatch, statsPlacement);
            }
            if (statsButton.currentFrame >= 3)
            {
                exitButton.Draw(spriteBatch, exitPlacement);
            }
        }
        public void MenuAnimationIntroUpdate(GameTime gameTime)
        {
            singleplayerButton.Update(gameTime);
            if (singleplayerButton.currentFrame >= 3)
            {
                versusButton.Update(gameTime);
            }
            if (versusButton.currentFrame >= 3)
            {
                controlsButton.Update(gameTime);
            }
            if(controlsButton.currentFrame >= 3)
            {
                statsButton.Update(gameTime);
            }
            if(statsButton.currentFrame >= 3)
            {
                exitButton.Update(gameTime);
            }

            if(singleplayerButton.currentFrame == 8)
            {
                singleplayerButton.currentFrame = 5;
            }
            if(versusButton.currentFrame == 8)
            {
                versusButton.currentFrame = 5;
            }
            if(controlsButton.currentFrame == 8)
            {
                controlsButton.currentFrame = 5;
            }
            if(statsButton.currentFrame == 8)
            {
                statsButton.currentFrame = 5;
            }
            if(exitButton.currentFrame == 8)
            {
                exitButton.currentFrame = 5;
            }
            if(exitButton.currentFrame == 5)
            {
                menuintro = false;
            }
        }
        public void MenuAnimationsDraw(SpriteBatch spriteBatch)
        {
            singleplayerButton.Draw(spriteBatch, singleplayerPlacement);
            versusButton.Draw(spriteBatch, versusPlacement);
            statsButton.Draw(spriteBatch, statsPlacement);
            controlsButton.Draw(spriteBatch, controlInfoPlacement);
            exitButton.Draw(spriteBatch, exitPlacement);
        }
        public void MenuAnimationsUpdate(GameTime gameTime)
        {
            singleplayerButton.Update(gameTime);
            versusButton.Update(gameTime);
            statsButton.Update(gameTime);
            controlsButton.Update(gameTime);
            exitButton.Update(gameTime);
            if (singleplayerButton.currentFrame >= 5)
            {
                if (menuPointer == 0)
                {
                    if (playerHasChosen == true)
                    {
                        if (singleplayerButton.currentFrame <= 15)
                        {
                            singleplayerButton.currentFrame = 16;
                        }
                        if (singleplayerButton.currentFrame == 22)
                        {
                            menuSelectAnimationDone = true;
                            singleplayerButton.currentFrame = 19;
                        }
                    }
                    else
                    {
                        if (singleplayerButton.currentFrame == 15)
                        {
                            singleplayerButton.currentFrame = 13;
                        }
                    }
                }
                else if(singleplayerButton.currentFrame >= 8)
                {
                    singleplayerButton.currentFrame = 5;
                }
            }
            if (versusButton.currentFrame >= 5)
            {
                if (menuPointer == 1)
                {
                    if (playerHasChosen == true)
                    {
                        if (versusButton.currentFrame <= 14)
                        {
                            versusButton.currentFrame = 15;
                        }
                        if (versusButton.currentFrame == 21)
                        {
                            menuSelectAnimationDone = true;
                            versusButton.currentFrame = 18;
                        }
                    }
                    else
                    {
                        if (versusButton.currentFrame == 14)
                        {
                            versusButton.currentFrame = 12;
                        }
                    }
                }
                else if(versusButton.currentFrame >= 8)
                {
                    versusButton.currentFrame = 5;
                }
            }
            if (controlsButton.currentFrame >= 5)
            {
                if (menuPointer == 2)
                {
                    if (playerHasChosen == true)
                    {
                        if (controlsButton.currentFrame <= 15)
                        {
                            controlsButton.currentFrame = 16;
                        }
                        if (controlsButton.currentFrame == 22)
                        {
                            menuSelectAnimationDone = true;
                            controlsButton.currentFrame = 19;
                        }
                    }
                    else
                    {
                        if (controlsButton.currentFrame == 15)
                        {
                            controlsButton.currentFrame = 13;
                        }
                    }
                }
                else if(controlsButton.currentFrame >= 8)
                {
                    controlsButton.currentFrame = 5;
                }
            }
            if (statsButton.currentFrame >= 5)
            {
                if (menuPointer == 3)
                {
                    if (statsButton.currentFrame == 15)
                    {
                        statsButton.currentFrame = 13;
                    }
                }
                else if(statsButton.currentFrame >= 8)
                {
                    statsButton.currentFrame = 5;
                }
            }
            if (exitButton.currentFrame >= 5)
            {
                if (menuPointer == 4)
                {
                    if (exitButton.currentFrame == 15)
                    {
                        exitButton.currentFrame = 13;
                    }
                }
                else if(exitButton.currentFrame >= 8)
                {
                    exitButton.currentFrame = 5;
                }
            }
        }
        public void menuIdleAnimations(GameTime gameTime)
        {
            graphicIdle.Update(gameTime);
            if(menuPointer == 0)
            {
                if (graphicIdle.currentFrame >= 18 || graphicIdle.currentFrame < 14)
                {
                    graphicIdle.currentFrame = 14;
                }
            }
            if (menuPointer == 1)
            {
                if(graphicIdle.currentFrame >= 4 || graphicIdle.currentFrame < 1)
                {
                    graphicIdle.currentFrame = 1;
                }
            }
            if (menuPointer == 2)
            {
                if (graphicIdle.currentFrame >= 55 || graphicIdle.currentFrame < 50)
                {
                    graphicIdle.currentFrame = 50;
                }
            }
            if (menuPointer == 3)
            {
                if (graphicIdle.currentFrame >= 49 || graphicIdle.currentFrame < 19)
                {
                    graphicIdle.currentFrame = 19;
                }
            }
            if(menuPointer == 4)
            {
                if (graphicIdle.currentFrame >= 67 || graphicIdle.currentFrame < 55)
                {
                    graphicIdle.currentFrame = 55;
                }
            }
        }
        public void TransitionAnimations(SpriteBatch spriteBatch)
        {
            if (playerHasChosen == true)
            {
                if (menuPointer == 0)
                {
                    if (menuSelectAnimationDone == true)
                    {
                        fireballAnimation.Draw(spriteBatch, singlePlayerFireballPlacement);
                        if (singlePlayerFireballPlacement.X + fireballAnimation.width - 30 >= 400)
                        {
                            explosionAnimation.Draw(spriteBatch, singleExplosionPlacement);
                        }
                    }
                }
                if (menuPointer == 1)
                {
                    if (menuSelectAnimationDone == true)
                    {
                        fireballAnimation.Draw(spriteBatch, versusFireballPlacement);
                        if (versusFireballPlacement.X + fireballAnimation.width - 30 >= 400)
                        {
                            explosionAnimation.Draw(spriteBatch, versusExplosionPlacement);
                        }
                    }
                }
                if (menuPointer == 2)
                {
                    if(menuSelectAnimationDone == true)
                    {
                        nonCombatTransition.Draw(spriteBatch, zeroPlacement);
                    }
                }
                if (menuPointer == 3)
                {

                }
                if (menuPointer == 4)
                {

                }
            }
        }
        public void TransitionAnimationFrameLogic(GameTime gameTime)
        {
            
            if(menuPointer == 0)
            {
                if (menuSelectAnimationDone == true)
                {
                    fireballAnimation.Update(gameTime);
                    singlePlayerFireballPlacement.X += 10;

                    if (singlePlayerFireballPlacement.X + fireballAnimation.width - 30 >= 400)
                    {
                        explosionAnimation.Update(gameTime);
                    }
                    if (fireballAnimation.currentFrame == fireballAnimation.totalFrames)
                    {
                        fireballAnimation.currentFrame = 0;
                    }
                }
            }
            if(menuPointer == 1)
            {
                if (menuSelectAnimationDone == true)
                {
                    fireballAnimation.Update(gameTime);
                    versusFireballPlacement.X += 10;

                    if (versusFireballPlacement.X + fireballAnimation.width - 30 >= 400)
                    {
                        explosionAnimation.Update(gameTime);
                    }
                    if (fireballAnimation.currentFrame == fireballAnimation.totalFrames)
                    {
                        fireballAnimation.currentFrame = 0;
                    }
                }
            }
            if(menuPointer == 2)
            {
                if(menuSelectAnimationDone == true)
                {
                    if (nonCombatTransition.currentFrame >= 6)
                    {

                    }
                    else
                    {
                        nonCombatTransition.Update(gameTime);
                    }
                }
            }
            if(menuPointer == 3)
            {

            }
            if(menuPointer == 4)
            {

            }
        }
    }
}
