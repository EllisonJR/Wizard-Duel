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
    class Player : AssetContainer
    {
        public GraphicsDeviceManager graphics;
        public PlayerIndex playerIndex { get; }
        public bool AI;
        GameStates gameState;
        
        public int playerSpeed;

        public Rectangle aiRectangle;

        public float angle;
        public Rectangle hitBox;
        public Rectangle reflectHitBox;
        Rectangle shotMeter;
        float shotMeterCounter;
        Vector2 shotMeterLocation;

        public Vector2 rotationOrigin;
        public Vector2 projectileOrigin { get; set; }
        public Vector2 playerLocation;
        public Vector2 reticalLocation;
        Vector2 healthContainerLocation;
        Vector2 healthBarLocation;

        public bool tiredWizardCloud = false;

        Matrix shooterMatrix;

        public Input input;

        public int health;
        public int score { get; set; }

        public float shootingAngle { get; set; }

        public InputAction inputAction { get; set; }
        public InputAction previousInputAction { get; set; }

        public Animation playerAnimations;
        public Animation tiredreflectAnimation;

        public CharacterAdjuster characterAdjuster;

        public int playerChoice;
        int tiredReflectCounter;

        public bool slept;
        public bool slowed;
        public bool stunned;
        public bool incapped;
        
        Animation slowEffect;
        Vector2 slowLocation;
        
        Animation stunnedEffect;
        Vector2 stunnedLocation;
        
        Animation sleepEffect;
        Vector2 sleepLocation;

        public ImpactLocations impactLocation;

        public int stunTimer;
        public int sleepTimer;

        public int incapTimer;
        
        Vector2 kickedUpDustLocation;
        Animation kickedUpDust;
        Vector2 landingPoofLocation;
        Animation landingPoof;
        
        List<Vector2> dashTrailLocations = new List<Vector2>();
        List<Animation> dashTrails = new List<Animation>();

        public PotionTypes potionType;

        int dashTrailTimer;

        int potionTimer;
        int r = 0;
        int g = 0;
        int b = 0;
        int rgbincrementer = 20;
        int colorTimer;
        bool incrementRed = true;
        bool incrementGreen = false;
        bool incrementBlue = false;

        Vector2 chargePotionAnimLocation;
        List<Animation> chargePotAnims1 = new List<Animation>();

        List<Animation> shieldPanels = new List<Animation>();
        Animation shieldFrame;
        Vector2 shieldLocation;
        public Rectangle shieldHitbox;

        public Player(ControlType controlType, PlayerIndex playerIndex, GraphicsDeviceManager graphics, GameStates gameState)
        {
            characterAdjuster = new CharacterAdjuster();
            this.graphics = graphics;
            this.playerIndex = playerIndex;
            this.gameState = gameState;
            impactLocation = ImpactLocations.None;
            if(this.gameState == GameStates.SinglePlayer && playerIndex == PlayerIndex.Two)
            {
                AI = true;
            }
            input = new Input(controlType, playerIndex, AI);
            health = 3;
            score = 0;
            playerSpeed = 5;
            potionType = PotionTypes.None;

            chargePotAnims1.Add(new Animation(chargePotAnimT1, 2, 3));
            chargePotAnims1.Add(new Animation(chargePotAnimT2, 2, 3));
            chargePotAnims1.Add(new Animation(chargePotAnimT3, 2, 3));
            chargePotAnims1.Add(new Animation(chargePotAnimT4, 2, 3));
            chargePotAnims1.Add(new Animation(chargePotAnimT5, 2, 3));

            kickedUpDust = new Animation(kickedUpDustT, 3, 8);
            landingPoof = new Animation(landingPoofT, 3, 10);

            sleepEffect = new Animation(sleepEffectT, 1, 4);
            slowEffect = new Animation(slowEffectT, 2, 2);
            stunnedEffect = new Animation(stunnedEffectT, 2, 3);
            playerAnimations = new Animation(buffSpriteT, 6, 20);

            if (playerChoice == 1)
            {
                playerAnimations = new Animation(buffSpriteT, 6, 20);
            }
            if (playerChoice == 2)
            {
                playerAnimations = new Animation(tiredSpriteT, 14, 10);
            }

            if (playerIndex == PlayerIndex.One)
            {
                playerLocation = new Vector2((graphics.PreferredBackBufferWidth / 2) - (playerAnimations.width / 2), graphics.PreferredBackBufferHeight - playerAnimations.height + 13);
                reticalLocation = new Vector2((playerLocation.X + playerAnimations.width / 2), playerLocation.Y + playerAnimations.height / 2);

            }
            else if (playerIndex == PlayerIndex.Two)
            {
                playerLocation = new Vector2((graphics.PreferredBackBufferWidth / 2) - (playerAnimations.width / 2), -5);
                reticalLocation = new Vector2(playerLocation.X + (playerAnimations.width / 2), playerLocation.Y + playerAnimations.height / 2);

                if (AI == true)
                {
                    aiRectangle = new Rectangle((int)playerLocation.X - 45, (int)playerLocation.Y, 120, hitBox.Height);
                }
            }

            tiredreflectAnimation = new Animation(tiredReflectT, 3, 10);
            tiredreflectAnimation.currentFrame = -1;
            tiredreflectAnimation.frameTime = 75;

            kickedUpDust.currentFrame = -1;
            landingPoof.currentFrame = -1;
            kickedUpDust.frameTime = 25;

            chargePotionAnimLocation = new Vector2(playerLocation.X + 35, playerLocation.Y + 45);

            chargePotAnims1[0].currentFrame = 0;
            chargePotAnims1[1].currentFrame = 6;
            chargePotAnims1[2].currentFrame = 6;
            chargePotAnims1[3].currentFrame = 6;
            chargePotAnims1[4].currentFrame = 6;

            shieldFrame = new Animation(shieldFrameT, 5, 12);
            shieldFrame.frameTime = 75;
            shieldFrame.currentFrame = 0;

            shieldPanels.Add(new Animation(panel1, 5, 12));
            shieldPanels.Add(new Animation(panel2, 5, 12));
            shieldPanels.Add(new Animation(panel3, 5, 12));
            shieldPanels.Add(new Animation(panel4, 5, 12));
            shieldPanels.Add(new Animation(panel5, 5, 12));
            shieldPanels.Add(new Animation(panel6, 5, 12));
            shieldPanels.Add(new Animation(panel7, 5, 12));
            shieldPanels.Add(new Animation(panel8, 5, 12));
            shieldPanels.Add(new Animation(panel9, 5, 12));
            shieldPanels.Add(new Animation(panel10, 5, 12));

            foreach(Animation shieldPanel in shieldPanels)
            {
                shieldPanel.frameTime = 75;
                shieldPanel.rainbow = true;
            }

            shieldPanels[0].r = 0;
            shieldPanels[0].g = 85;
            shieldPanels[0].b = 170;

            shieldPanels[1].r = 20;
            shieldPanels[1].g = 105;
            shieldPanels[1].b = 190;

            shieldPanels[2].r = 40;
            shieldPanels[2].g = 125;
            shieldPanels[2].b = 210;

            shieldPanels[3].r = 60;
            shieldPanels[3].g = 145;
            shieldPanels[3].b = 230;

            shieldPanels[4].r = 80;
            shieldPanels[4].g = 165;
            shieldPanels[4].b = 250;

            shieldPanels[5].r = 100;
            shieldPanels[5].g = 185;
            shieldPanels[5].b = 15;

            shieldPanels[6].r = 120;
            shieldPanels[6].g = 205;
            shieldPanels[6].b = 35;

            shieldPanels[7].r = 140;
            shieldPanels[7].g = 225;
            shieldPanels[7].b = 55;

            shieldPanels[8].r = 160;
            shieldPanels[8].g = 245;
            shieldPanels[8].b = 75;

            shieldPanels[9].r = 180;
            shieldPanels[9].g = 10;
            shieldPanels[9].b = 95;

            shieldLocation = new Vector2(playerAnimations.drawingLocation.X + playerAnimations.width / 2 - shieldFrame.width / 2,playerAnimations.drawingLocation.Y + playerAnimations.height / 2 - shieldFrame.height / 2 + 5);
            shieldHitbox = new Rectangle((int)shieldLocation.X, (int)shieldLocation.Y, shieldFrame.width, shieldFrame.height);
        }
        public void ResetPlayers(PlayerIndex playerIndex, int playerChoice)
        {
            this.playerChoice = playerChoice;
            slept = false;
            incapped = false;
            stunned = false;
            slowed = false;
            impactLocation = ImpactLocations.None;
            dashTrailTimer = 0;
            if (playerIndex == PlayerIndex.One)
            {
                playerLocation = new Vector2((graphics.PreferredBackBufferWidth / 2) - (playerAnimations.width / 2), graphics.PreferredBackBufferHeight - playerAnimations.height + 13);
                if (playerChoice == 1)
                {
                    
                    playerAnimations = new Animation(buffSpriteT, 6, 20);
                    hitBox = new Rectangle((int)playerLocation.X + (playerAnimations.width / 2) - buffHitboxT.Width / 2, ((int)playerLocation.Y + playerAnimations.height / 2) - 20, buffHitboxT.Width, buffHitboxT.Height);
                    reflectHitBox = new Rectangle((int)playerLocation.X + (playerAnimations.width / 2) - buffReflectBoxT.Width / 2, (int)playerLocation.Y + 25, buffReflectBoxT.Width, buffReflectBoxT.Height);
                }
                if (playerChoice == 2)
                {
                    tiredreflectAnimation.currentFrame = -1;
                    playerAnimations = new Animation(tiredSpriteT, 14, 10);
                    hitBox = new Rectangle((int)playerLocation.X + (playerAnimations.width / 2) - tiredHitboxT.Width / 2, ((int)playerLocation.Y + playerAnimations.height / 2) - 19, tiredHitboxT.Width, tiredHitboxT.Height);
                    reflectHitBox = new Rectangle((int)playerLocation.X + (playerAnimations.width / 2) - tiredReflectBoxT.Width / 2, (int)playerLocation.Y, tiredReflectBoxT.Width, tiredReflectBoxT.Height);
                }
                characterAdjuster.PassInCharacter(playerChoice);
                shotMeterLocation = new Vector2(hitBox.X + hitBox.Width / 2 - 15, hitBox.Y + 2 + hitBox.Height);
            }
            if (playerIndex == PlayerIndex.Two)
            {
                playerLocation = new Vector2((graphics.PreferredBackBufferWidth / 2) - (playerAnimations.width / 2), - 5);
                
                if (playerChoice == 1)
                {
                    playerAnimations = new Animation(buffSpriteT, 6, 20);
                    hitBox = new Rectangle((int)playerLocation.X + (playerAnimations.width / 2) - buffHitboxT.Width / 2, (int)playerLocation.Y + (playerAnimations.width / 2) - 10, buffHitboxT.Width, buffHitboxT.Height);
                    reflectHitBox = new Rectangle((int)playerLocation.X + (playerAnimations.width / 2) - buffReflectBoxT.Width / 2, (int)playerLocation.Y + playerAnimations.height - 50, buffReflectBoxT.Width, buffReflectBoxT.Height);
                }
                if (playerChoice == 2)
                {
                    tiredreflectAnimation.currentFrame = -1;
                    playerAnimations = new Animation(tiredSpriteT, 14, 10);
                    hitBox = new Rectangle((int)playerLocation.X + (playerAnimations.width / 2) - tiredHitboxT.Width / 2, (int)playerLocation.Y + (playerAnimations.height / 2) - tiredHitboxT.Height / 2 + 7, tiredHitboxT.Width, tiredHitboxT.Height);
                    reflectHitBox = new Rectangle((int)playerLocation.X + (playerAnimations.width / 2) - tiredReflectBoxT.Width / 2, (int)playerLocation.Y + playerAnimations.height - 30, tiredReflectBoxT.Width, tiredReflectBoxT.Height);
                }
                characterAdjuster.PassInCharacter(playerChoice);
                shotMeterLocation = new Vector2(hitBox.X + hitBox.Width / 2 - 15, hitBox.Y + 2 + hitBox.Height);
            }
            if (gameState == GameStates.SinglePlayer && playerIndex == PlayerIndex.Two)
            {
                AI = true;
            }
            shotMeterCounter = 30;
            slowLocation = new Vector2(hitBox.Center.X - slowEffect.width / 2, hitBox.Center.Y - slowEffect.height / 2);
            stunnedLocation = new Vector2(playerAnimations.width / 2 - stunnedEffect.width / 2, hitBox.Top - 20);
            sleepLocation = new Vector2(stunnedLocation.X, stunnedLocation.Y - 15);
            kickedUpDust.currentFrame = -1;
            landingPoof.currentFrame = -1;
            kickedUpDust.frameTime = 25;
            input.inputAction = InputAction.None;
            inputAction = InputAction.None;
        }
        public void Update(GameTime gameTime)
        {
            previousInputAction = inputAction;
            characterAdjuster.UpdateCharacter(gameTime); 
            characterAdjuster.GrabInput(inputAction, playerIndex, playerSpeed, stunned, slowed, slept, incapped, impactLocation);
            playerAnimations.currentFrame = characterAdjuster.PassInFrame();
                
            if (potionType != PotionTypes.None)
            {
                potionTimer += gameTime.ElapsedGameTime.Milliseconds;
                
                if(incrementRed == true)
                {
                    r += 50;
                    if(r >= 255)
                    {
                        r = 0;
                        incrementRed = false;
                        incrementBlue = true;
                    }
                }
                if(incrementBlue == true)
                {
                    b += 50;
                    if (b >= 255)
                    {
                        b = 0;
                        incrementBlue = false;
                        incrementGreen = true;
                    }
                }
                if (incrementGreen == true)
                {
                    g += 50;
                    if (g >= 255)
                    {
                        g = 0;
                        incrementGreen = false;
                        incrementRed = true;
                    }
                }
                if (potionType == PotionTypes.Shield)
                {
                    if (potionTimer >= 10000)
                    {
                        potionType = PotionTypes.None;
                    }
                }
                else
                {
                    if (potionTimer >= 5000)
                    {
                        potionType = PotionTypes.None;
                    }
                }
            }

            DashEffects(gameTime);
            UpdateDashEffects(gameTime);

            StatusEffects(gameTime);

            input.slowed = slowed;
            input.incapped = incapped;
            if (AI == false)
            {
                angle = input.ReturnAngle();
                if (stunned == false && slept == false && incapped == false)
                {
                    input.Update(gameTime);
                }
                ShotMeter();
                if (incapped == false)
                {
                    inputAction = input.inputAction;
                }
                else if(incapped == true)
                {
                    inputAction = InputAction.None;
                }
                PlayerMovement(gameTime);
                CalculateRotationOrigin();
                CalculateProjectileOriginAndDirection();
                shootingAngle = input.ReturnAngle();
                input.GetRotationOrigin(reticalLocation);
            }
            else if(AI == true)
            {

            }
            if (playerChoice == 2)
            {
                if (inputAction == InputAction.Reflect && tiredreflectAnimation.currentFrame < 0)
                {
                    tiredWizardCloud = true;
                    if (playerIndex == PlayerIndex.One)
                    {
                        tiredreflectAnimation.currentFrame = 0;
                    }
                    else if(playerIndex == PlayerIndex.Two)
                    {
                        tiredreflectAnimation.currentFrame = 12;
                    }
                }
                if (tiredreflectAnimation.currentFrame >= 0)
                {
                    tiredReflectCounter += gameTime.ElapsedGameTime.Milliseconds;
                    tiredreflectAnimation.Update(gameTime);
                    if (playerIndex == PlayerIndex.One)
                    {
                        if (tiredreflectAnimation.currentFrame == 11)
                        {
                            tiredreflectAnimation.currentFrame = 8;
                        }
                    }
                    if(playerIndex == PlayerIndex.Two)
                    {
                        if (tiredreflectAnimation.currentFrame == 23)
                        {
                            tiredreflectAnimation.currentFrame = 20;
                        }
                    }
                    if(tiredReflectCounter >= 3500)
                    {
                        tiredreflectAnimation.currentFrame = -1;
                        tiredReflectCounter = 0;
                        tiredWizardCloud = false;
                    }
                }
            }
            for (int i = 0; i < chargePotAnims1.Count; i++)
            {
                chargePotAnims1[i].Update(gameTime);
                
                if (i + 1 > chargePotAnims1.Count - 1)
                {
                    if (chargePotAnims1[i].currentFrame == 3)
                    {
                        chargePotAnims1[i - 4].currentFrame = 0;
                    }
                }
                else
                {
                    if (chargePotAnims1[i].currentFrame == 3)
                    {
                        chargePotAnims1[i + 1].currentFrame = 0;
                    }
                }
                if (chargePotAnims1[i].currentFrame == 6)
                {
                    chargePotAnims1[i].currentFrame = 6;
                }
            }
            foreach(Animation shieldPanel in shieldPanels)
            {
                if(potionType == PotionTypes.Shield)
                {
                    if(shieldPanel.currentFrame < 46 || shieldPanel.currentFrame > 46)
                    {
                        shieldPanel.Update(gameTime);
                    }
                    if (potionTimer > 9700 && potionTimer < 9750)
                    {
                        shieldPanel.currentFrame = 47;
                    }
                }
            }
            if (potionType == PotionTypes.Shield)
            {
                if (shieldFrame.currentFrame < 46 || shieldFrame.currentFrame > 46)
                {
                    shieldFrame.Update(gameTime);
                }
                if(potionTimer > 9700 && potionTimer < 9750)
                {
                    shieldFrame.currentFrame = 47;
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            DrawDashEffects(spriteBatch);
            if (playerChoice == 1)
            {
                //spriteBatch.Draw(buffHitboxT, hitBox, Color.White);
                //spriteBatch.Draw(buffReflectBoxT, reflectHitBox, Color.White);
            }
            if (playerChoice == 2)
            {
                //spriteBatch.Draw(tiredHitboxT, hitBox, Color.White);
                //spriteBatch.Draw(tiredReflectBoxT, reflectHitBox, Color.White);
            }
            if (incapped == false)
            {
                if (potionType == PotionTypes.Charge)
                {
                    playerAnimations.rainbow = true;
                    playerAnimations.Draw(spriteBatch, playerLocation);
                    foreach (Animation potionsparks in chargePotAnims1)
                    {
                        potionsparks.Draw(spriteBatch, chargePotionAnimLocation, 255, 0, 0);
                    }
                }
                else
                {
                    playerAnimations.rainbow = false;
                    playerAnimations.Draw(spriteBatch, playerLocation);
                }
            }
            else if (incapped == true)
            {
                if (incapTimer >= 0 && incapTimer <= 99)
                {
                    if (potionType == PotionTypes.Charge)
                    {
                        playerAnimations.rainbow = true;
                        playerAnimations.Draw(spriteBatch, playerLocation);
                    }
                    else
                    {
                        playerAnimations.rainbow = false;
                        playerAnimations.Draw(spriteBatch, playerLocation);
                    }
                }
                if (incapTimer >= 100 && incapTimer <= 199)
                {

                }
                if (incapTimer >= 200 && incapTimer <= 299)
                {
                    if (potionType == PotionTypes.Charge)
                    {
                        playerAnimations.rainbow = true;
                        playerAnimations.Draw(spriteBatch, playerLocation);
                    }
                    else
                    {
                        playerAnimations.rainbow = false;
                        playerAnimations.Draw(spriteBatch, playerLocation);
                    }
                    
                }
                if (incapTimer >= 300 && incapTimer <= 499)
                {

                }
            }
            if (playerChoice == 2)
            {
                if (playerIndex == PlayerIndex.One)
                {
                    if (tiredreflectAnimation.currentFrame >= 0)
                    {
                        tiredreflectAnimation.Draw(spriteBatch, new Vector2(reflectHitBox.X - 18, reflectHitBox.Y - 15));
                    }
                }
                if (playerIndex == PlayerIndex.Two)
                {
                    if (tiredreflectAnimation.currentFrame >= 0)
                    {
                        tiredreflectAnimation.Draw(spriteBatch, new Vector2(reflectHitBox.X - 18, reflectHitBox.Y - 15));
                    }
                }
            }
            if (inputAction == InputAction.Charge || inputAction == InputAction.ChargeShotReady)
            {
                spriteBatch.Draw(playerRetical1, reticalLocation, null, Color.White, angle, rotationOrigin, 1f, SpriteEffects.None, 1f);
            }
            else
            {

            }
            if (slowed == true)
            {
                slowEffect.Draw(spriteBatch, slowLocation);
            }
            if (stunned == true && playerChoice == 1 && playerAnimations.currentFrame >= 77 && playerAnimations.currentFrame <= 80 || stunned == true && playerChoice == 1 && playerAnimations.currentFrame <= 90 && playerAnimations.currentFrame >= 87 || stunned == true && playerChoice == 2 && playerAnimations.currentFrame >= 105 && playerAnimations.currentFrame <= 108 || stunned == true && playerChoice == 2 && playerAnimations.currentFrame >= 96 && playerAnimations.currentFrame <= 99)
            {
                stunnedEffect.Draw(spriteBatch, stunnedLocation);
            }
            if (slept == true && playerChoice == 2 && playerAnimations.currentFrame >= 120 && playerAnimations.currentFrame <= 123 || slept == true && playerChoice == 2 && playerAnimations.currentFrame >= 125 && playerAnimations.currentFrame <= 128)
            {
                sleepEffect.Draw(spriteBatch, sleepLocation);
            }
            if (slept == true && playerChoice == 1 && playerAnimations.currentFrame >= 104 && playerAnimations.currentFrame <= 109 || slept == true && playerChoice == 1 && playerAnimations.currentFrame >= 111 && playerAnimations.currentFrame <= 116)
            {
                sleepEffect.Draw(spriteBatch, sleepLocation);
            }
            if (potionType != PotionTypes.Fireball)
            {
                spriteBatch.Draw(shotMeterTexture, new Rectangle((int)shotMeterLocation.X, (int)shotMeterLocation.Y, (int)shotMeterCounter, 2), Color.White);
            }
            else
            {
                spriteBatch.Draw(shotMeterTexture, new Rectangle((int)shotMeterLocation.X, (int)shotMeterLocation.Y, (int)shotMeterCounter, 2), new Color(r, g, b));
            }
            foreach (Animation shieldPanel in shieldPanels)
            {
                if (potionType == PotionTypes.Shield)
                {
                    shieldPanel.Draw(spriteBatch, shieldLocation);
                }
            }
            if (potionType == PotionTypes.Shield)
            {
                shieldFrame.Draw(spriteBatch, shieldLocation);
            }
        }
        public void DashEffects(GameTime gameTime)
        {
            DashTrailLogic(gameTime);
            if(inputAction == InputAction.DashRight)
            {
                if (playerChoice == 1)
                {
                    if (playerAnimations.currentFrame == 10)
                    {
                        kickedUpDustLocation.X = hitBox.Left - kickedUpDust.width;
                        kickedUpDustLocation.Y = hitBox.Bottom - kickedUpDust.height - 5;
                        kickedUpDust.currentFrame = 0;
                    }
                }
                if (playerChoice == 2)
                {
                    if (playerAnimations.currentFrame == 63 && playerSpeed == 20)
                    {
                        kickedUpDustLocation.X = hitBox.Left - kickedUpDust.width;
                        kickedUpDustLocation.Y = hitBox.Bottom - kickedUpDust.height - 5;
                        kickedUpDust.currentFrame = 0;
                    }
                }
            }
            if(inputAction == InputAction.DashLeft)
            {
                if (playerChoice == 1)
                {
                    if (playerAnimations.currentFrame == 19)
                    {
                        kickedUpDustLocation.X = hitBox.Right;
                        kickedUpDustLocation.Y = hitBox.Bottom - kickedUpDust.height - 5;
                        kickedUpDust.currentFrame = 9;
                    }
                }
                if (playerChoice == 2)
                {
                    if (playerAnimations.currentFrame == 77 && playerSpeed == 20)
                    {
                        kickedUpDustLocation.X = hitBox.Right;
                        kickedUpDustLocation.Y = hitBox.Bottom - kickedUpDust.height - 5;
                        kickedUpDust.currentFrame = 9;
                    }
                }
            }
            if (playerChoice == 1)
            {
                if (playerAnimations.currentFrame == 26)
                {
                    landingPoofLocation.X = playerAnimations.drawingLocation.X;
                    landingPoofLocation.Y = playerAnimations.drawingLocation.Y + playerAnimations.height - 70;
                    landingPoof.currentFrame = 7;
                    landingPoof.frameTime = 50;
                }
                if (playerAnimations.currentFrame == 17)
                {
                    landingPoofLocation.X = playerAnimations.drawingLocation.X;
                    landingPoofLocation.Y = playerAnimations.drawingLocation.Y + playerAnimations.height - 70;
                    landingPoof.currentFrame = 0;
                    landingPoof.frameTime = 50;
                }
            }
            if (playerChoice == 2)
            {
                if (playerAnimations.currentFrame == 71)
                {
                    landingPoofLocation.X = playerAnimations.drawingLocation.X;
                    landingPoofLocation.Y = playerAnimations.drawingLocation.Y + playerAnimations.height - 70;
                    landingPoof.currentFrame = 21;
                }
                if(playerAnimations.currentFrame == 85)
                {
                    landingPoofLocation.X = playerAnimations.drawingLocation.X;
                    landingPoofLocation.Y = playerAnimations.drawingLocation.Y + playerAnimations.height - 70;
                    landingPoof.currentFrame = 14;
                }
            }
        }
        public void DashTrailLogic(GameTime gameTime)
        {
            dashTrailTimer += gameTime.ElapsedGameTime.Milliseconds;
            if (dashTrailTimer >= 5 && input.dashed < 400 && input.dashed > 25)
            {
                Random random = new Random();
                int randomAnim = random.Next(0, 5);
                dashTrailTimer = 0;
                if (inputAction == InputAction.DashRight)
                {
                    if (randomAnim == 0)
                    {
                        dashTrails.Add(new Animation(dashRightTrailT, 2, 4, 0, 50));
                        dashTrailLocations.Add(new Vector2(hitBox.Center.X, hitBox.Bottom - 20));
                    }
                    if(randomAnim == 1)
                    {
                        dashTrails.Add(new Animation(dashRightTrailT2, 2, 4, 0, 50));
                        dashTrailLocations.Add(new Vector2(hitBox.Center.X, hitBox.Bottom - 20));
                    }
                    if (randomAnim == 3)
                    {
                        dashTrails.Add(new Animation(dashRightTrailT3, 2, 4, 0, 50));
                        dashTrailLocations.Add(new Vector2(hitBox.Center.X, hitBox.Bottom - 20));
                    }
                    if (randomAnim == 4)
                    {
                        dashTrails.Add(new Animation(dashRightTrailT4, 2, 4, 0, 50));
                        dashTrailLocations.Add(new Vector2(hitBox.Center.X, hitBox.Bottom - 20));
                    }
                }
                if (inputAction == InputAction.DashLeft)
                {
                    if (randomAnim == 0)
                    {
                        dashTrails.Add(new Animation(dashLeftTrailT, 2, 4, 0, 50));
                        dashTrailLocations.Add(new Vector2(hitBox.Center.X, hitBox.Bottom - 20));
                    }
                    if (randomAnim == 1)
                    {
                        dashTrails.Add(new Animation(dashLeftTrailT2, 2, 4, 0, 50));
                        dashTrailLocations.Add(new Vector2(hitBox.Center.X, hitBox.Bottom - 20));
                    }
                    if (randomAnim == 3)
                    {
                        dashTrails.Add(new Animation(dashLeftTrailT3, 2, 4, 0, 50));
                        dashTrailLocations.Add(new Vector2(hitBox.Center.X, hitBox.Bottom - 20));
                    }
                    if (randomAnim == 4)
                    {
                        dashTrails.Add(new Animation(dashLeftTrailT4, 2, 4, 0, 50));
                        dashTrailLocations.Add(new Vector2(hitBox.Center.X, hitBox.Bottom - 20));
                    }
                }
            }
        }
        public void DrawDashEffects(SpriteBatch spriteBatch)
        {
            if (landingPoof.currentFrame != -1 && landingPoof.currentFrame != 6 && landingPoof.currentFrame != 13 && landingPoof.currentFrame != 20 && landingPoof.currentFrame != 27)
            {
                landingPoof.Draw(spriteBatch, landingPoofLocation);
            }
            if (kickedUpDust.currentFrame != -1 && kickedUpDust.currentFrame != 8 && kickedUpDust.currentFrame != 17)
            {
                kickedUpDust.Draw(spriteBatch, kickedUpDustLocation);
            }
            for(int i = 0; i < dashTrails.Count; i++)
            {
                dashTrails[i].Draw(spriteBatch, dashTrailLocations[i]);
            }
            for (int i = 0; i < dashTrails.Count; i++)
            {
                if (dashTrails[i].currentFrame == 7)
                {
                    dashTrailLocations.RemoveAt(i);
                    dashTrails.RemoveAt(i--);
                }
            }
        }
        public void UpdateDashEffects(GameTime gameTime)
        {
            if (landingPoof.currentFrame != -1 && landingPoof.currentFrame != 6 && landingPoof.currentFrame != 13 && landingPoof.currentFrame != 20 && landingPoof.currentFrame != 27)
            {
                landingPoof.Update(gameTime);
            }
            if (kickedUpDust.currentFrame != -1 && kickedUpDust.currentFrame != 8 && kickedUpDust.currentFrame != 17)
            {
                kickedUpDust.Update(gameTime);
            }

            foreach(Animation dashTrail in dashTrails)
            {
                if (dashTrail.currentFrame != 7)
                {
                    dashTrail.Update(gameTime);
                }
            }
            if(landingPoof.currentFrame >= 27)
            {
                landingPoof.currentFrame = -1;
            }
            if(kickedUpDust.currentFrame >= 17)
            {
                kickedUpDust.currentFrame = -1;
            }
        }
        public void StatusEffects(GameTime gameTime)
        {
            stunnedEffect.Update(gameTime);
            slowEffect.Update(gameTime);
            sleepEffect.Update(gameTime);
            if(slowEffect.currentFrame == 3)
            {
                slowEffect.currentFrame = 0;
            }
            if(slowed == true)
            {
                playerSpeed = 2;
            }
            if(stunned == true)
            {
                playerSpeed = 0;
            }
            if(slept == true)
            {
                playerSpeed = 0;
            }
            if(stunned == false && slept == false && slowed == false)
            {
                playerSpeed = 5;
                
            }
            if(stunnedEffect.currentFrame == 4)
            {
                stunnedEffect.currentFrame = 0;
            }
            if(sleepEffect.currentFrame == 4)
            {
                sleepEffect.currentFrame = 0;
            }
            if(stunned == true)
            {
                playerAnimations.frameTime = 150;
                stunTimer += gameTime.ElapsedGameTime.Milliseconds;
                if(stunTimer >= 3000)
                {
                    playerAnimations.frameTime = 100;
                    stunned = false;
                }
            }
            if(slept == true)
            {
                playerAnimations.frameTime = 200;
                sleepTimer += gameTime.ElapsedGameTime.Milliseconds;
                if(sleepTimer >= 5000)
                {
                    playerAnimations.frameTime = 100;
                    slept = false;
                }
            }
            if(incapped == true)
            {
                incapTimer += gameTime.ElapsedGameTime.Milliseconds;
                if(incapTimer >= 500)
                {
                    incapped = false;
                }
                if (playerChoice == 1)
                {
                    if (playerAnimations.currentFrame == 68)
                    {
                        playerLocation.X -= .25f;
                    }
                    if (playerAnimations.currentFrame == 70)
                    {
                        playerLocation.X += .25f;
                    }
                }
                if(playerChoice == 2)
                {
                    if (playerAnimations.currentFrame == 130)
                    {
                        playerLocation.X -= .25f;
                    }
                    if (playerAnimations.currentFrame == 129)
                    {
                        playerLocation.X += .25f;
                    }
                }
            }
        }
        public void PlayerMovement(GameTime gameTime)
        {
            if (slowed == false && stunned == false && slept == false && incapped == false)
            {
                if (input.inputAction == InputAction.DashLeft || input.inputAction == InputAction.DashRight)
                {
                    if (input.inputAction == InputAction.DashLeft)
                    {
                        if (input.dashed <= 100)
                        {
                            playerSpeed = 20;
                        }
                        if (input.dashed > 100 && input.dashed <= 175)
                        {
                            playerSpeed = 8;
                        }
                        if (input.dashed > 175 && input.dashed <= 200)
                        {
                            playerSpeed = 7;
                        }
                        if (input.dashed > 200 && input.dashed <= 450)
                        {
                            playerSpeed = 0;
                        }
                        playerLocation.X -= playerSpeed;
                        if (input.dashed > 450)
                        {
                            inputAction = InputAction.None;
                            playerSpeed = 5;
                        }
                    }
                    if (input.inputAction == InputAction.DashRight)
                    {
                        if (input.dashed <= 100)
                        {
                            playerSpeed = 20;
                        }
                        if (input.dashed > 100 && input.dashed <= 170)
                        {
                            playerSpeed = 8;
                        }
                        if (input.dashed > 175 && input.dashed <= 200)
                        {
                            playerSpeed = 7;
                        }
                        if (input.dashed > 200 && input.dashed <= 450)
                        {
                            playerSpeed = 0;
                        }
                        playerLocation.X += playerSpeed;
                        if (input.dashed > 450)
                        {
                            inputAction = InputAction.None;
                            playerSpeed = 5;
                        }
                    }
                }
            }

            if (incapped == false)
            {
                if (input.inputAction == InputAction.Left)
                {
                    playerLocation.X -= playerSpeed;
                }
                else if (input.inputAction == InputAction.Right)
                {
                    playerLocation.X += playerSpeed;
                }
                if (playerLocation.X + (playerAnimations.width / 2) + (hitBox.Width / 2) > graphics.PreferredBackBufferWidth - 25)
                {
                    playerLocation.X = graphics.PreferredBackBufferWidth - 25 - (playerAnimations.width / 2) - (hitBox.Width / 2);
                }
                if (playerLocation.X + (playerAnimations.width / 2) - (hitBox.Width / 2) < 25)
                {
                    playerLocation.X = 25 - (playerAnimations.width / 2) + (hitBox.Width / 2);
                }
            }
            LockObjectstoPlayer();
        }
        public void LockObjectstoPlayer()
        {
            reticalLocation.X = playerLocation.X + (playerAnimations.width / 2);
            hitBox.X = (int)playerLocation.X + (playerAnimations.width / 2) - hitBox.Width / 2;
            if (tiredWizardCloud == false)
            {
                reflectHitBox.X = (int)playerLocation.X + (playerAnimations.width / 2) - reflectHitBox.Width / 2;
            }
            shotMeterLocation.X = (int)hitBox.X + hitBox.Width / 2 - 15;
            aiRectangle.X = (int)playerLocation.X - 30;

            stunnedLocation.X = hitBox.Center.X - stunnedEffect.width / 2;
            slowLocation.X = hitBox.Center.X - slowEffect.width / 2;
            sleepLocation.X = stunnedLocation.X;

            chargePotionAnimLocation.X = playerLocation.X + 32;
            
            shieldLocation = new Vector2(playerAnimations.drawingLocation.X + playerAnimations.width / 2 - shieldFrame.width / 2, playerAnimations.drawingLocation.Y + playerAnimations.height / 2 - shieldFrame.height / 2 + 5);
            shieldHitbox.X = (int)playerAnimations.drawingLocation.X + (playerAnimations.width / 2) - (int)shieldHitbox.Width / 2;
            shieldHitbox.Y = (int)playerAnimations.drawingLocation.Y + playerAnimations.height / 2 - shieldHitbox.Width / 2;
        }
        public void CalculateProjectileOriginAndDirection()
        {
            if (playerIndex == PlayerIndex.One)
            {
                shooterMatrix = Matrix.CreateRotationZ(input.ReturnAngle()) * Matrix.CreateTranslation(reticalLocation.X, reticalLocation.Y, 0);
                projectileOrigin = Vector2.Transform(new Vector2(0, -playerRetical1.Height), shooterMatrix);//transforms the vector BASED ON the location of the new world matrix
            }
            if(playerIndex == PlayerIndex.Two)
            {
                shooterMatrix = Matrix.CreateRotationZ(input.ReturnAngle()) * Matrix.CreateTranslation(reticalLocation.X, reticalLocation.Y, 0);
                projectileOrigin = Vector2.Transform(new Vector2(0, playerRetical2.Height), shooterMatrix);
            }
        }
        public void CalculateRotationOrigin()
        {
            if(playerIndex == PlayerIndex.One)
            {
                rotationOrigin = new Vector2(playerRetical1.Width / 2, playerRetical1.Height); 
            }
            else if(playerIndex == PlayerIndex.Two)
            {
                rotationOrigin = new Vector2(playerRetical2.Width / 2, 0);
            }
        }
        public void ShotMeter()
        {
            if (AI == true)
            {
                if (shotMeterCounter < 30)
                {
                    shotMeterCounter += .2f;
                }
                if (inputAction == InputAction.Charge || inputAction == InputAction.ChargeShotReady)
                {
                    if (shotMeterCounter < 10)
                    {
                        inputAction = InputAction.None;
                    }
                }
                if (inputAction == InputAction.Shoot)
                {
                    if (shotMeterCounter < 10)
                    {
                        inputAction = InputAction.None;
                    }
                    else
                    {
                        shotMeterCounter -= 10;
                    }
                }
                if (inputAction == InputAction.ChargeShot)
                {
                    if (shotMeterCounter < 20)
                    {
                        inputAction = InputAction.None;
                    }
                    else
                    {
                        shotMeterCounter -= 20;
                    }
                }
            }
            else
            {
                if (shotMeterCounter < 30)
                {
                    shotMeterCounter += .2f;
                }
                if (input.inputAction == InputAction.Charge || input.inputAction == InputAction.ChargeShotReady)
                {
                    if (shotMeterCounter < 10)
                    {
                        input.inputAction = InputAction.None;
                    }
                }
                if (potionType != PotionTypes.Fireball)
                {
                    if (input.inputAction == InputAction.Shoot)
                    {
                        if (shotMeterCounter < 10)
                        {
                            input.inputAction = InputAction.None;
                        }
                        else
                        {
                            shotMeterCounter -= 10;
                        }
                    }
                    if (input.inputAction == InputAction.ChargeShot)
                    {
                        if (shotMeterCounter < 20)
                        {
                            input.inputAction = InputAction.None;
                        }
                        else
                        {
                            shotMeterCounter -= 20;
                        }
                    }
                }
            }
        }
    }
}
