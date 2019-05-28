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
    class Player
    {
        public ContentManager content;
        public GraphicsDeviceManager graphics;
        public PlayerIndex playerIndex { get; }
        public bool AI;
        GameStates gameState;
        
        public int playerSpeed;

        public Rectangle aiRectangle;

        public float angle;
        public Texture2D buffSpriteT;
        public Texture2D tiredSpriteT;
        public Texture2D tiredReflectT;
        public Texture2D playerRetical;
        public Rectangle hitBox;
        public Rectangle reflectHitBox;
        Texture2D buffHitboxT;
        Texture2D tiredHitboxT;
        Texture2D tiredReflectBoxT;
        Texture2D buffReflectBoxT;
        Texture2D shotMeterTexture;
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

        Texture2D slowEffectT;
        Animation slowEffect;
        Vector2 slowLocation;

        Texture2D stunnedEffectT;
        Animation stunnedEffect;
        Vector2 stunnedLocation;

        Texture2D sleepEffectT;
        Animation sleepEffect;
        Vector2 sleepLocation;

        public ImpactLocations impactLocation;

        public int stunTimer;
        public int sleepTimer;

        public int incapTimer;

        Texture2D kickedUpDustT;
        Vector2 kickedUpDustLocation;
        Animation kickedUpDust;
        Texture2D landingPoofT;
        Vector2 landingPoofLocation;
        Animation landingPoof;
        Texture2D dashRightTrailT;
        Texture2D dashLeftTrailT;
        Texture2D dashRightTrailT2;
        Texture2D dashLeftTrailT2;
        Texture2D dashRightTrailT3;
        Texture2D dashLeftTrailT3;
        Texture2D dashRightTrailT4;
        Texture2D dashLeftTrailT4;
        List<Vector2> dashTrailLocations = new List<Vector2>();
        List<Animation> dashTrails = new List<Animation>();

        int dashTrailTimer;

        public Player(ControlType controlType, PlayerIndex playerIndex, ContentManager content, GraphicsDeviceManager graphics, GameStates gameState)
        {
            characterAdjuster = new CharacterAdjuster(content, graphics);
            this.content = content;
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

        }
        public void LoadContent()
        {
            buffSpriteT = content.Load<Texture2D>("sprites/playersprites/buffwizard");
            tiredSpriteT = content.Load<Texture2D>("sprites/playersprites/tiredwizard");
            tiredReflectT = content.Load<Texture2D>("sprites/player effects/tiredwizardreflect");
            shotMeterTexture = content.Load<Texture2D>("sprites/fireball");
            buffHitboxT = content.Load<Texture2D>("sprites/hitboxes/buffwizardhitbox");
            tiredHitboxT = content.Load<Texture2D>("sprites/hitboxes/tiredwizardhitbox");
            buffReflectBoxT = content.Load<Texture2D>("sprites/hitboxes/buffwizardreflecthitbox");
            tiredReflectBoxT = content.Load<Texture2D>("sprites/hitboxes/tiredwizardreflecthitbox");
            slowEffectT = content.Load<Texture2D>("sprites/playerSprites/sloweffect");
            stunnedEffectT = content.Load<Texture2D>("sprites/playerSprites/stuneffect");
            sleepEffectT = content.Load<Texture2D>("sprites/player effects/sleepeffect");

            kickedUpDustT = content.Load<Texture2D>("sprites/player effects/dashstartparticles");
            landingPoofT = content.Load<Texture2D>("sprites/player effects/dash landing particles");

            dashRightTrailT = content.Load<Texture2D>("sprites/player effects/dashRightTrail");
            dashLeftTrailT = content.Load<Texture2D>("sprites/player effects/dashLeftTrail");
            dashRightTrailT2 = content.Load<Texture2D>("sprites/player effects/dashRightTrail2");
            dashLeftTrailT2 = content.Load<Texture2D>("sprites/player effects/dashLeftTrail2");
            dashRightTrailT3 = content.Load<Texture2D>("sprites/player effects/dashRightTrail3");
            dashLeftTrailT3 = content.Load<Texture2D>("sprites/player effects/dashLeftTrail3");
            dashRightTrailT4 = content.Load<Texture2D>("sprites/player effects/dashRightTrail4");
            dashLeftTrailT4 = content.Load<Texture2D>("sprites/player effects/dashLeftTrail4");


            kickedUpDust = new Animation(kickedUpDustT, 3, 8);
            landingPoof = new Animation(landingPoofT, 3, 10);

            sleepEffect = new Animation(sleepEffectT, 1, 4);
            slowEffect = new Animation(slowEffectT, 2, 2);
            stunnedEffect = new Animation(stunnedEffectT, 2, 3);
            playerAnimations = new Animation(buffSpriteT, 6, 20);

            if(playerChoice == 1)
            {
                playerAnimations = new Animation(buffSpriteT, 6, 20);
            }
            if(playerChoice == 2)
            {
                playerAnimations = new Animation(tiredSpriteT, 14, 10);
            }

            if (playerIndex == PlayerIndex.One)
            {
                playerRetical = content.Load<Texture2D>("sprites/playerSprites/aimingretical");
                playerLocation = new Vector2((graphics.PreferredBackBufferWidth / 2) - (playerAnimations.width / 2), graphics.PreferredBackBufferHeight - playerAnimations.height + 13);
                reticalLocation = new Vector2((playerLocation.X + playerAnimations.width / 2), playerLocation.Y + playerAnimations.height / 2);
                
            }
            else if (playerIndex == PlayerIndex.Two)
            {
                playerRetical = content.Load<Texture2D>("sprites/playerSprites/aimingreticalP2");
                playerLocation = new Vector2((graphics.PreferredBackBufferWidth / 2) - (playerAnimations.width / 2), - 5);
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
                playerRetical = content.Load<Texture2D>("sprites/playerSprites/aimingretical");
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
                playerRetical = content.Load<Texture2D>("sprites/playerSprites/aimingreticalP2");
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
        public void UnloadContent()
        {
            content.Unload();
        }
        public void Update(GameTime gameTime)
        {
            previousInputAction = inputAction;
            characterAdjuster.UpdateCharacter(gameTime); 
            characterAdjuster.GrabInput(inputAction, playerIndex, playerSpeed, stunned, slowed, slept, incapped, impactLocation);
            playerAnimations.currentFrame = characterAdjuster.PassInFrame();

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
                playerAnimations.Draw(spriteBatch, playerLocation);
            }
            else if (incapped == true)
            {
                if (incapTimer >= 0 && incapTimer <= 99)
                {
                    playerAnimations.Draw(spriteBatch, playerLocation);
                }
                if (incapTimer >= 100 && incapTimer <= 199)
                {

                }
                if (incapTimer >= 200 && incapTimer <= 299)
                {
                    playerAnimations.Draw(spriteBatch, playerLocation);
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
                spriteBatch.Draw(playerRetical, reticalLocation, null, Color.White, angle, rotationOrigin, 1f, SpriteEffects.None, 1f);
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
            spriteBatch.Draw(shotMeterTexture, new Rectangle((int)shotMeterLocation.X, (int)shotMeterLocation.Y, (int)shotMeterCounter, 2), Color.White);
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
        }
        public void CalculateProjectileOriginAndDirection()
        {
            if (playerIndex == PlayerIndex.One)
            {
                shooterMatrix = Matrix.CreateRotationZ(input.ReturnAngle()) * Matrix.CreateTranslation(reticalLocation.X, reticalLocation.Y, 0);
                projectileOrigin = Vector2.Transform(new Vector2(0, -playerRetical.Height), shooterMatrix);//transforms the vector BASED ON the location of the new world matrix
            }
            if(playerIndex == PlayerIndex.Two)
            {
                shooterMatrix = Matrix.CreateRotationZ(input.ReturnAngle()) * Matrix.CreateTranslation(reticalLocation.X, reticalLocation.Y, 0);
                projectileOrigin = Vector2.Transform(new Vector2(0, playerRetical.Height), shooterMatrix);
            }
        }
        public void CalculateRotationOrigin()
        {
            if(playerIndex == PlayerIndex.One)
            {
                rotationOrigin = new Vector2(playerRetical.Width / 2, playerRetical.Height); 
            }
            else if(playerIndex == PlayerIndex.Two)
            {
                rotationOrigin = new Vector2(playerRetical.Width / 2, 0);
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
