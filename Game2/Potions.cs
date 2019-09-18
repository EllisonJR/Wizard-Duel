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
    public enum PotionTypes { Shield, Skull, Wall, Charge, Fireball, None}
    class Potions : AssetContainer
    {
        GraphicsDeviceManager graphics;

        public Animation glass;

        public Rectangle hitBox;

        public Vector2 location;

        bool Left;
        bool LeftRightMovement = false;

        public bool hit = false;
        bool breakage = false;

        Vector2 smokeLocation;

        public PotionTypes potionType;

        public bool activated = false;

        public int player1HP;
        public int player2HP;

        Vector2 player1Location;
        Vector2 player2Location;

        Vector2 direction = new Vector2();
        Vector2 CenterofSkull;

        float angle;

        List<PotionSmoke> potionSmokes = new List<PotionSmoke>();

        int potionTimer;

        List<Vector2> particlePlacements = new List<Vector2>();

        bool populateParticles = false;

        Random randomX;
        Random randomY;

        Random seedX = new Random();
        Random seedY = new Random();

        int seedXint;
        int seedYint;

        int particleDispersalTimer;
        public Vector2 particledirection = new Vector2();
        public List<Animation> potionParticles = new List<Animation>();

        public PlayerIndex playerIndex;

        Vector2 zeroplacement = new Vector2(0, 0);

        Vector2 WallCenter;
        Rectangle wallParticleCapture;

        Animation potionWallTop;
        Animation potionWallMiddle;
        public Animation potionWallBottom;
        Animation potionWallLiquid;
        Animation potionWallPassThrough;
        Animation potionWallMiddle2;
        Animation potionWallBottom2;

        int potionWallTimer;

        public Rectangle wallPotionCollision;
        int wallWidth = 0;

        int potionMovementTimer;

        PotionVectors potionVector;
        Vector2 potionDirection;
        Vector2 newLoc;

        public bool lockSkullPotionToRail = false;

        Vector2 centerOfScreen;
        bool leftRight;

        public Vector2 leftEye;
        public Vector2 rightEye;

        bool moving;

        int potionTimerEntire;
        int potionTimeOnField;
        Animation traveler;
        Animation potionSeller;
        Vector2 travelerLoc;
        Vector2 sellerLoc;

        float potionShrinker;
        float potionSize;

        Vector2 potionEndPoint;

        bool potionDespawn = false;
        public bool despawnLocked = false;

        Random potionDuration;
        int memeStay;

        public Potions(GraphicsDeviceManager graphics, int potionTypeInt)
        {
            this.graphics = graphics;

            potionVector = new PotionVectors(graphics);

            player1HP = 5;
            player2HP = 5;

            int constructorincrement = 0;

            for (int i = 0; i < 30; i++)
            {
                if(constructorincrement > 3)
                {
                    constructorincrement = 0;
                }
                potionParticles.Add(new Animation(potionParticleT, 2, 3, constructorincrement));
                constructorincrement++;
            }
            if(potionTypeInt == 1)
            {
                potionType = PotionTypes.Skull;
            }
            if (potionTypeInt == 2)
            {
                potionType = PotionTypes.Shield;
            }
            if (potionTypeInt == 3)
            {
                potionType = PotionTypes.Wall;
            }
            if (potionTypeInt == 4)
            {
                potionType = PotionTypes.Fireball;
            }
            if (potionTypeInt == 5)
            {
                potionType = PotionTypes.Charge;
            }

            if (potionType == PotionTypes.Charge)
            {
                glass = new Animation(chargeGlassT, 6, 10);
                location.X = graphics.PreferredBackBufferWidth / 2 - glass.width / 2;
                location.Y = graphics.PreferredBackBufferHeight / 2 - glass.height / 2 - 40;
                hitBox = new Rectangle((int)location.X + 36, (int)location.Y + 31, 28, 47);
                smokeLocation = new Vector2((int)location.X + 48, (int)location.Y + 17);
                LeftRightMovement = true;
            }
            if (potionType == PotionTypes.Shield)
            {
                glass = new Animation(shieldGlassT, 3, 10);
                location.X = graphics.PreferredBackBufferWidth / 2 - glass.width / 2;
                location.Y = graphics.PreferredBackBufferHeight / 2 - glass.height / 2 - 40;
                hitBox = new Rectangle((int)location.X + 30, (int)location.Y + 30, 44, 40);
                smokeLocation = new Vector2((int)location.X + 44, (int)location.Y + 15);
                LeftRightMovement = true;
            }
            if (potionType == PotionTypes.Wall)
            {
                glass = new Animation(wallGlassT, 6, 10);
                location.X = graphics.PreferredBackBufferWidth / 2 - glass.width / 2;
                location.Y = graphics.PreferredBackBufferHeight / 2 - glass.height / 2 - 40;
                hitBox = new Rectangle((int)location.X + 21, (int)location.Y + 37, 60, 30);
                smokeLocation = new Vector2((int)location.X + 45, (int)location.Y + 23);
                LeftRightMovement = true;

                WallCenter = new Vector2(location.X + glass.width / 2, location.Y + glass.height / 2);
                wallParticleCapture = new Rectangle((int)WallCenter.X - 3, (int)WallCenter.Y - 3, 6,6);
                potionWallTop = new Animation(wallPotionTopT, 3, 7);
                potionWallMiddle = new Animation(wallPotionMiddleT, 3, 7);
                potionWallBottom = new Animation(wallPotionBottomT, 3, 7);
                potionWallLiquid = new Animation(wallPotionLiquidT, 5, 6);
                potionWallMiddle2 = new Animation(wallPotionMiddleT, 3, 7);
                potionWallBottom2 = new Animation(wallPotionBottomT, 3, 7);

                potionWallBottom.rainbow = true;
                potionWallMiddle.rainbow = true;
                potionWallTop.rainbow = true;
                potionWallMiddle2.rainbow = true;
                potionWallBottom2.rainbow = true;
                potionWallLiquid.rainbow = true;

                potionWallTop.frameTime = 75;
                potionWallMiddle.frameTime = 75;
                potionWallBottom.frameTime = 75;
                potionWallLiquid.frameTime = 75;
                potionWallMiddle2.frameTime = 75;
                potionWallBottom2.frameTime = 75;
                
                if(playerIndex == PlayerIndex.One)
                {
                    potionWallPassThrough = new Animation(wallPotionPassThroughBottomT, 3, 7);
                }
                if(playerIndex == PlayerIndex.Two)
                {
                    potionWallPassThrough = new Animation(wallPotionPassThroughTopT, 3, 7);
                }
                potionWallPassThrough.frameTime = 75;
                potionWallPassThrough.rainbow = true;

                potionWallBottom.r = 0;
                potionWallBottom.g = 30;
                potionWallBottom.b = 60;

                potionWallMiddle.r = 30;
                potionWallMiddle.g = 60;
                potionWallMiddle.b = 90;

                potionWallTop.r = 60;
                potionWallTop.g = 90;
                potionWallTop.b = 120;

                if (playerIndex == PlayerIndex.Two)
                {
                    potionWallPassThrough.r = 90;
                    potionWallPassThrough.g = 120;
                    potionWallPassThrough.b = 150;
                }
                if (playerIndex == PlayerIndex.One)
                {
                    potionWallPassThrough.r = 225;
                    potionWallPassThrough.g = 0;
                    potionWallPassThrough.b = 30;
                }

                potionWallLiquid.r = 0;
                potionWallLiquid.g = 85;
                potionWallLiquid.b = 170;

                potionWallBottom2.r = 120;
                potionWallBottom2.g = 150;
                potionWallBottom2.b = 180;

                potionWallMiddle2.r = 90;
                potionWallMiddle2.g = 120;
                potionWallMiddle2.b = 150;
            }
            if (potionType == PotionTypes.Skull)
            {
                glass = new Animation(skullGlassT, 6, 10);
                location.X = graphics.PreferredBackBufferWidth / 2 - glass.width / 2;
                location.Y = graphics.PreferredBackBufferHeight / 2 - glass.height / 2 - 40;
                hitBox = new Rectangle((int)location.X + 32, (int)location.Y + 28, 36, 54);
                smokeLocation = new Vector2((int)location.X + 44, (int)location.Y + 14);

                player1HP = 1;
                player2HP = 1;

                CenterofSkull = new Vector2(location.X + glass.width / 2, location.Y + glass.height / 2);
            }
            if (potionType == PotionTypes.Fireball)
            {
                glass = new Animation(fireballGlassT, 4, 10);
                location.X = graphics.PreferredBackBufferWidth / 2 - glass.width / 2;
                location.Y = graphics.PreferredBackBufferHeight / 2 - glass.height / 2 - 40;
                hitBox = new Rectangle((int)location.X + 29, (int)location.Y + 38, 45, 42);
                smokeLocation = new Vector2((int)location.X + 44, (int)location.Y + 22);
                LeftRightMovement = true;
            }
            potionSeller = new Animation(potionSellerT, 1, 2);
            traveler = new Animation(travelerT, 1, 2);

            potionSeller.frameTime = 300;
            traveler.frameTime = 300;

            sellerLoc = new Vector2(-potionSeller.width, graphics.PreferredBackBufferHeight / 2 - potionSeller.height / 2);
            travelerLoc = new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight / 2 - traveler.height / 2);

            potionDuration = new Random();

            potionTimeOnField = potionDuration.Next(10000, 20000);
        }
        public void Update(GameTime gameTime)
        {
            newLoc = potionVector.ReturnPotionDirection();
            potionVector.Update(gameTime);

            if (activated == false)
            {
                potionTimerEntire += gameTime.ElapsedGameTime.Milliseconds;
            }

            if (potionTimerEntire > potionTimeOnField)
            {
                potionDespawn = true;
                PotionDespawn(gameTime);
                memeStay += gameTime.ElapsedGameTime.Milliseconds;
            }

            seedXint = seedX.Next(234,1234);
            seedYint = seedY.Next(2345,3456);

            WallCollision();
            PotionSmokeLogic(gameTime);

            if (activated == false && potionType != PotionTypes.Skull)
            {
                Movement(gameTime);
            }

            SkullMovement(gameTime);
            FrameLogic(gameTime);
            Particles();
            particleDispersal(gameTime);
            if (particlePlacements.Count >= 30)
            {
                particleDispersalTimer += gameTime.ElapsedGameTime.Milliseconds;
                populateParticles = false;
            }
            if (potionType == PotionTypes.Wall)
            {
                WallCenter = new Vector2(location.X + glass.width / 2, location.Y + glass.height / 2);
                wallParticleCapture.X = (int)WallCenter.X - 3;
                wallParticleCapture.Y = (int)WallCenter.Y - 3;
            }
            if(potionType == PotionTypes.Skull)
            {
                leftEye = new Vector2(location.X + 43, location.Y + 50);
                rightEye = new Vector2(location.X + 54, location.Y + 50);
            }
            potionEndPoint = new Vector2(sellerLoc.X + 66, sellerLoc.Y + 40);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (PotionSmoke potionSmoke in potionSmokes)
            {
                if(potionDespawn == true)
                {
                    potionSmoke.shrink = true;
                }
                potionSmoke.Draw(spriteBatch);
            }
            DrawLogic(spriteBatch);
            if (hit == true)
            {
                glass.shrink = true;
                hit = false;
                breakage = true;
            }
            
            potionSeller.Draw(spriteBatch, sellerLoc);
            traveler.Draw(spriteBatch, travelerLoc);
        }
        public PotionTypes PassType()
        {
            return potionType;
        }
        public PlayerIndex PassIndex()
        {
            return playerIndex;
        }
        public void DrawLogic(SpriteBatch spriteBatch)
        {
            if (potionDespawn == false)
            {
                if (potionType == PotionTypes.Fireball)
                {
                    if (glass.currentFrame < 32)
                    {
                        glass.Draw(spriteBatch, location);
                    }
                    if (glass.currentFrame == 20)
                    {
                        populateParticles = true;

                    }
                    if (glass.currentFrame > 20)
                    {
                        foreach (Vector2 particlePlacement in particlePlacements)
                        {
                            foreach (Animation potionParticle in potionParticles)
                            {
                                potionParticle.DrawColor(spriteBatch, particlePlacement);
                            }
                        }
                    }
                }
                if (potionType == PotionTypes.Charge)
                {
                    if (glass.currentFrame < 59)
                    {
                        glass.Draw(spriteBatch, location);
                    }
                    if (glass.currentFrame == 49)
                    {
                        populateParticles = true;

                    }
                    if (glass.currentFrame > 49)
                    {
                        foreach (Vector2 particlePlacement in particlePlacements)
                        {
                            foreach (Animation potionParticle in potionParticles)
                            {
                                potionParticle.DrawColor(spriteBatch, particlePlacement);
                            }
                        }
                    }
                }
                if (potionType == PotionTypes.Shield)
                {
                    if (glass.currentFrame < 30)
                    {
                        glass.Draw(spriteBatch, location);
                    }
                    if (glass.currentFrame == 20)
                    {
                        populateParticles = true;

                    }
                    if (glass.currentFrame > 20)
                    {
                        foreach (Vector2 particlePlacement in particlePlacements)
                        {
                            foreach (Animation potionParticle in potionParticles)
                            {
                                potionParticle.DrawColor(spriteBatch, particlePlacement);
                            }
                        }
                    }
                }
                if (potionType == PotionTypes.Wall)
                {
                    if (glass.currentFrame < 55)
                    {
                        glass.Draw(spriteBatch, location);
                    }
                    if (glass.currentFrame == 42)
                    {
                        populateParticles = true;
                    }
                    if (glass.currentFrame > 42)
                    {
                        foreach (Vector2 particlePlacement in particlePlacements)
                        {
                            foreach (Animation potionParticle in potionParticles)
                            {
                                potionParticle.DrawColor(spriteBatch, particlePlacement);
                            }
                        }
                        if (potionParticles.Count < 5)
                        {
                            if (potionWallLiquid.currentFrame < 24)
                            {
                                potionWallLiquid.Draw(spriteBatch, new Vector2(WallCenter.X - potionWallLiquid.width / 2, WallCenter.Y - potionWallLiquid.height / 2));
                            }
                            if (potionWallLiquid.currentFrame > 15)
                            {
                                if (playerIndex == PlayerIndex.One)
                                {
                                    potionWallBottom2.Draw(spriteBatch, new Vector2(WallCenter.X - potionWallTop.width / 2, WallCenter.Y - potionWallTop.height / 2 - 3));
                                    potionWallMiddle2.Draw(spriteBatch, new Vector2(WallCenter.X - potionWallTop.width / 2, WallCenter.Y - potionWallTop.height / 2 - 2));
                                    potionWallTop.Draw(spriteBatch, new Vector2(WallCenter.X - potionWallTop.width / 2, WallCenter.Y - potionWallTop.height / 2 - 1));
                                    potionWallMiddle.Draw(spriteBatch, new Vector2(WallCenter.X - potionWallTop.width / 2, WallCenter.Y - potionWallTop.height / 2));
                                    potionWallBottom.Draw(spriteBatch, new Vector2(WallCenter.X - potionWallTop.width / 2, WallCenter.Y - potionWallTop.height / 2 + 1));
                                    potionWallPassThrough.Draw(spriteBatch, new Vector2(WallCenter.X - potionWallTop.width / 2 + 7, WallCenter.Y - potionWallTop.height / 2 + 2));
                                }
                                if (playerIndex == PlayerIndex.Two)
                                {
                                    potionWallBottom2.Draw(spriteBatch, new Vector2(WallCenter.X - potionWallTop.width / 2, WallCenter.Y - potionWallTop.height / 2 + 3));
                                    potionWallMiddle2.Draw(spriteBatch, new Vector2(WallCenter.X - potionWallTop.width / 2, WallCenter.Y - potionWallTop.height / 2 + 2));
                                    potionWallTop.Draw(spriteBatch, new Vector2(WallCenter.X - potionWallTop.width / 2, WallCenter.Y - potionWallTop.height / 2 + 1));
                                    potionWallMiddle.Draw(spriteBatch, new Vector2(WallCenter.X - potionWallTop.width / 2, WallCenter.Y - potionWallTop.height / 2));
                                    potionWallBottom.Draw(spriteBatch, new Vector2(WallCenter.X - potionWallTop.width / 2, WallCenter.Y - potionWallTop.height / 2 - 1));
                                    potionWallPassThrough.Draw(spriteBatch, new Vector2(WallCenter.X - potionWallTop.width / 2 + 7, WallCenter.Y - potionWallTop.height / 2 - 2));
                                }
                            }

                        }
                    }
                }
                if (potionType == PotionTypes.Skull)
                {
                    glass.Draw(spriteBatch, location);
                }
            }
            else
            {
                glass.DrawShrink(spriteBatch, location);
            }
        }
        public void PotionDespawn(GameTime gameTime)
        {
            potionSeller.Update(gameTime);
            traveler.Update(gameTime);
            if(traveler.currentFrame == 2)
            {
                traveler.currentFrame = 0;
            }
            if(potionSeller.currentFrame == 2)
            {
                potionSeller.currentFrame = 0;
            }
            if (potionDespawn == true && despawnLocked == false)
            {
                if (despawnLocked == false)
                {
                    potionDirection.X = potionEndPoint.X - (location.X + glass.width / 2);
                    potionDirection.Y = potionEndPoint.Y - (location.Y + glass.height / 2);
                    potionDirection.Normalize();

                    location += potionDirection * 10;
                    if(location.X + glass.width / 2 < potionEndPoint.X + 5 && location.X + glass.width / 2 > potionEndPoint.X - 5 && location.Y + glass.height / 2 > potionEndPoint.Y - 5 && location.Y + glass.height / 2 < potionEndPoint.Y + 5)
                    {
                        despawnLocked = true;
                    }
                }
                if (sellerLoc.X < 0)
                {
                    sellerLoc.X += 10;
                }
                if (sellerLoc.X > 0)
                {
                    sellerLoc.X = 0;
                }
                if (travelerLoc.X > 400 - traveler.width)
                {
                    travelerLoc.X -= 10;
                }
                if (travelerLoc.X < 400 - traveler.width)
                {
                    travelerLoc.X = 400 - traveler.width;
                }
            }
            if(potionDespawn == true && despawnLocked == true && memeStay > 2000)
            {
                if (sellerLoc.X > -potionSeller.width)
                {
                    sellerLoc.X -= 10;
                }
                if (travelerLoc.X < 400 + traveler.width)
                {
                    travelerLoc.X += 10;
                }
                location.X -= 10;
            }
        }
        public void particleDispersal(GameTime gameTime)
        {
            foreach(Animation potionParticle in potionParticles)
            {
                if (activated == true)
                {
                    potionParticle.Update(gameTime);
                    if(potionParticle.currentFrame >= 4)
                    {
                        potionParticle.currentFrame = 0;
                    }
                }
            }
            if(particleDispersalTimer > 1000)
            {
                if (potionType == PotionTypes.Wall)
                {
                    for (int i = 0; i < particlePlacements.Count; i++)
                    {
                        particledirection = WallCenter - particlePlacements[i];
                        particledirection.Normalize();

                        particlePlacements[i] += particledirection * 2;
                    }
                    for (int i = 0; i < potionParticles.Count; i++)
                    {
                        if (wallParticleCapture.Contains(potionParticles[i].drawingLocation.X, potionParticles[i].drawingLocation.Y))
                        {
                            potionParticles.RemoveAt(i--);
                            break;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < particlePlacements.Count; i++)
                    {
                        if (playerIndex == PlayerIndex.One)
                        {
                            particledirection = player1Location - particlePlacements[i];
                        }
                        if (playerIndex == PlayerIndex.Two)
                        {
                            particledirection = player2Location - particlePlacements[i];
                        }
                        particledirection.Normalize();

                        particlePlacements[i] += particledirection * 8;
                    }
                    for (int i = 0; i < potionParticles.Count; i++)
                    {
                        if (activated == true)
                        {
                            if (playerIndex == PlayerIndex.One)
                            {
                                if (potionParticles[i].drawingLocation.Y + potionParticles[i].height >= player1Location.Y - 2)
                                {
                                    potionParticles.RemoveAt(i--);
                                }
                            }
                            if (playerIndex == PlayerIndex.Two)
                            {
                                if (potionParticles[i].drawingLocation.Y <= player1Location.Y + 2)
                                {
                                    potionParticles.RemoveAt(i--);
                                }
                            }
                        }
                    }
                }
            }
        }
        public void WallCollision()
        {
            if (potionType == PotionTypes.Wall)
            {
                if (potionWallLiquid.currentFrame > 15)
                {
                    if (wallWidth < potionWallMiddle.width)
                    {
                        wallWidth++;
                    }
                    wallPotionCollision = new Rectangle((int)potionWallBottom2.drawingLocation.X, (int)potionWallBottom2.drawingLocation.Y, wallWidth, 1);
                }
            }
        }
        public void PotionSmokeLogic(GameTime gameTime)
        {
            potionTimer += gameTime.ElapsedGameTime.Milliseconds;
            foreach (PotionSmoke potionSmoke in potionSmokes)
            {
                potionSmoke.Update(gameTime);
            }
            if (activated == false && potionType != PotionTypes.Skull)
            {
                if (potionTimer >= 300)
                {
                    potionSmokes.Add(new PotionSmoke(smokeLocation, smoke1, smoke2));
                    potionTimer = 0;
                }
            }
            if (potionType == PotionTypes.Skull)
            {
                if (glass.currentFrame > 41 && glass.currentFrame < 52)
                {
                    if (potionTimer >= 50)
                    {
                        potionSmokes.Add(new PotionSmoke(smokeLocation, smoke1, smoke2));
                        potionTimer = 0;
                    }
                }
                else
                {
                    if (potionTimer >= 300)
                    {
                        potionSmokes.Add(new PotionSmoke(smokeLocation, smoke1, smoke2));
                        potionTimer = 0;
                    }
                }
                if (glass.currentFrame > 29 && glass.currentFrame < 37)
                {
                    smokeLocation = new Vector2(location.X + 62, location.Y + 22);
                }
                else
                {
                    smokeLocation = new Vector2(location.X + 44, location.Y + 22);
                }
            }
            for (int i = 0; i < potionSmokes.Count; i++)
            {
                if (potionSmokes[i].smoke.currentFrame >= 9)
                {
                    potionSmokes.RemoveAt(i--);
                }
            }
            if (player1HP <= 0 || player2HP <= 0)
            {
                activated = true;
                if(player1HP <= 0)
                {
                    playerIndex = PlayerIndex.One;
                }
                if(player2HP <= 0)
                {
                    playerIndex = PlayerIndex.Two;
                }
            }
            if (activated == true)
            {
            }
        }
        public void LockObjectsToPotion()
        {
            if (potionType == PotionTypes.Charge)
            {
                hitBox = new Rectangle((int)location.X + 36, (int)location.Y + 31, 28, 47);
                smokeLocation = new Vector2((int)location.X + 48, (int)location.Y + 17);LeftRightMovement = true;
            }
            if (potionType == PotionTypes.Shield)
            {
                hitBox = new Rectangle((int)location.X + 30, (int)location.Y + 30, 44, 40);
                smokeLocation = new Vector2((int)location.X + 44, (int)location.Y + 15);
            }
            if (potionType == PotionTypes.Wall)
            {
                hitBox = new Rectangle((int)location.X + 21, (int)location.Y + 37, 60, 30);
                smokeLocation = new Vector2((int)location.X + 45, (int)location.Y + 23);

                WallCenter = new Vector2(location.X + glass.width / 2, location.Y + glass.height / 2);
                wallParticleCapture = new Rectangle((int)WallCenter.X - 3, (int)WallCenter.Y - 3, 6, 6);
                potionWallTop = new Animation(wallPotionTopT, 3, 7);
            }
            if (potionType == PotionTypes.Skull)
            {
                hitBox = new Rectangle((int)location.X + 32, (int)location.Y + 28, 36, 54);
                smokeLocation = new Vector2((int)location.X + 44, (int)location.Y + 14);
                CenterofSkull = new Vector2(location.X + glass.width / 2, location.Y + glass.height / 2);
            }
            if (potionType == PotionTypes.Fireball)
            {
                hitBox = new Rectangle((int)location.X + 29, (int)location.Y + 38, 45, 42);
                smokeLocation = new Vector2((int)location.X + 44, (int)location.Y + 22);
            }
        }
        public void Particles()
        {
            if(populateParticles == true)
            {
                int potionLocX1 = (int)glass.drawingLocation.X;
                int potionLocY1 = (int)glass.drawingLocation.Y;

                int potionLocX2 = (int)glass.drawingLocation.X + glass.width;
                int potionLocY2 = (int)glass.drawingLocation.Y + glass.height;

                
                
                randomX = new Random(seedXint);
                randomY = new Random(seedYint);

                int X = randomX.Next(potionLocX1, potionLocX2);
                int Y = randomY.Next(potionLocY1, potionLocY2);
                particlePlacements.Add(new Vector2(X, Y));
            }
        }
        public void FrameLogic(GameTime gameTime)
        {
            if (potionType == PotionTypes.Charge)
            {
                glass.Update(gameTime);
                if (activated == false)
                {
                    if (glass.currentFrame == 0)
                    {
                        glass.frameTime = 2000;
                    }
                    if (glass.currentFrame > 0)
                    {
                        glass.frameTime = 100;
                    }
                    if (glass.currentFrame == 49)
                    {
                        glass.currentFrame = 0;
                    }
                }
                else
                {
                    glass.frameTime = 100;
                    if(breakage == true)
                    {
                        glass.currentFrame = 49;
                        breakage = false;
                    }
                }
            }
            if (potionType == PotionTypes.Shield)
            {
                glass.Update(gameTime);
                if (activated == false)
                {
                    if (glass.currentFrame == 0)
                    {
                        glass.frameTime = 2000;
                    }
                    if (glass.currentFrame > 0 && glass.currentFrame < 8)
                    {
                        glass.frameTime = 50;
                    }
                    if(glass.currentFrame > 7 && glass.currentFrame < 14)
                    {
                        glass.frameTime = 25;
                    }
                    if(glass.currentFrame > 13 && glass.currentFrame < 20)
                    {
                        glass.frameTime = 50;
                    }
                    if (glass.currentFrame == 20)
                    {
                        glass.currentFrame = 0;
                    }
                }
                else
                {
                    glass.frameTime = 100;
                    if (breakage == true)
                    {
                        glass.currentFrame = 20;
                        breakage = false;
                    }
                }
            }
            if (potionType == PotionTypes.Wall)
            {
                glass.Update(gameTime);
                if (activated == false)
                {
                    if (glass.currentFrame == 0)
                    {
                        glass.frameTime = 2000;
                    }
                    if (glass.currentFrame > 0)
                    {
                        glass.frameTime = 100;
                    }
                    if (glass.currentFrame == 42 && activated == false)
                    {
                        glass.currentFrame = 0;
                    }
                }
                else
                {
                    glass.frameTime = 100;
                    if (breakage == true)
                    {
                        glass.currentFrame = 42;
                        breakage = false;
                    }
                }
                if(glass.currentFrame > 42 && potionParticles.Count < 5)
                {
                    if (potionWallLiquid.currentFrame < 24)
                    {
                        potionWallLiquid.Update(gameTime);
                    }
                    if(potionWallLiquid.currentFrame > 15)
                    {
                        potionWallTimer += gameTime.ElapsedGameTime.Milliseconds;
                        potionWallBottom2.Update(gameTime);
                        potionWallMiddle2.Update(gameTime);
                        potionWallTop.Update(gameTime);
                        potionWallMiddle.Update(gameTime);
                        potionWallBottom.Update(gameTime);
                        potionWallPassThrough.Update(gameTime);
                        if (potionWallTimer < 10000)
                        {
                            if (potionWallTop.currentFrame == 12)
                            {
                                potionWallTop.currentFrame = 5;
                            }
                            if (potionWallMiddle.currentFrame == 12)
                            {
                                potionWallMiddle.currentFrame = 5;
                            }
                            if (potionWallBottom.currentFrame == 12)
                            {
                                potionWallBottom.currentFrame = 5;
                            }
                            if (potionWallMiddle2.currentFrame == 12)
                            {
                                potionWallMiddle2.currentFrame = 5;
                            }
                            if (potionWallBottom2.currentFrame == 12)
                            {
                                potionWallBottom2.currentFrame = 5;
                            }
                            if (potionWallPassThrough.currentFrame == 12)
                            {
                                potionWallPassThrough.currentFrame = 5;
                            }
                        }
                    }
                }
            }
            if (potionType == PotionTypes.Skull)
            {
                if (glass.currentFrame != 53)
                {
                    glass.Update(gameTime);
                }
                if (activated == false)
                {
                    
                    if (glass.currentFrame == 0)
                    {
                        glass.frameTime = 2000;
                    }
                    if (glass.currentFrame == 11)
                    {
                        glass.currentFrame = 0;
                    }
                    if (glass.currentFrame > 0 && glass.currentFrame < 12)
                    {
                        glass.frameTime = 50;
                    }
                }
                else if(activated == true)
                {

                    if(breakage == true)
                    {
                        glass.currentFrame = 12;
                        breakage = false;
                    }

                    if (glass.currentFrame >= 12 && glass.currentFrame < 36)
                    {
                        glass.frameTime = 100;
                    }
                    if (glass.currentFrame == 36)
                    {
                        glass.frameTime = 500;
                    }
                    if (glass.currentFrame > 36)
                    {
                        glass.frameTime = 100;
                    }
                }
            }
            if (potionType == PotionTypes.Fireball)
            {
                glass.Update(gameTime);
                if (activated == false)
                {
                    if (glass.currentFrame == 0)
                    {
                        glass.frameTime = 2000;
                    }
                    if (glass.currentFrame > 0)
                    {
                        glass.frameTime = 75;
                    }
                    if (glass.currentFrame == 20)
                    {
                        glass.currentFrame = 0;
                    }
                }
                else
                {
                    glass.frameTime = 100;
                    if (breakage == true)
                    {
                        glass.currentFrame = 20;
                        breakage = false;
                    }
                }
            }
        }
        public void GrabPlayerLocation(Vector2 player1, Vector2 player2)
        {
            player1Location = player1;
            player2Location = player2;
        }
        public void SkullMovement(GameTime gameTime)
        {
            if(activated == false && potionType == PotionTypes.Skull)
            {
                Movement(gameTime);
            }
            if(activated == true && glass.currentFrame == 53 && potionType == PotionTypes.Skull)
            {
                LockObjectsToPotion();
                if(lockSkullPotionToRail == true)
                {
                    if(leftRight == true)
                    {
                        location.X -= 3;
                        if(location.X + glass.width / 2 < 50)
                        {
                            leftRight = false;
                        }
                    }
                    if(leftRight == false)
                    {
                        location.X += 3;
                        if(location.X + glass.width / 2 > 350)
                        {
                            leftRight = true;
                        }
                    }
                }
                else
                {
                    LockSkullPotion();
                }
            }
        }
        public void LockSkullPotion()
        {
            if(player1HP == 0)
            {
                potionDirection.X = 350 - (location.X + glass.width / 2);
                potionDirection.Y = 450 - (location.Y + glass.height / 2);
                potionDirection.Normalize();

                location += potionDirection * 5;

                if (location.X + glass.width / 2 < 355 && location.X + glass.width / 2 > 345 && location.Y + glass.height / 2 < 455 && location.Y + glass.height / 2 > 445)
                {
                    lockSkullPotionToRail = true;
                }
            }
            if(player2HP == 0)
            {
                potionDirection.X = 50 - (location.X + glass.width / 2);
                potionDirection.Y = 150 - (location.Y + glass.height / 2);
                potionDirection.Normalize();

                location += potionDirection * 5;

                if(location.X + glass.width / 2 < 55 && location.X + glass.width / 2 > 45 && location.Y + glass.height / 2 < 155 && location.Y + glass.height / 2 > 145)
                {
                    lockSkullPotionToRail = true;
                }
            }
        }
        public void Movement(GameTime gameTime)
        {
            if (potionDespawn == false)
            {
                if (location.X + glass.width / 2 > newLoc.X + 20 || location.X + glass.width / 2 < newLoc.X - 20 || location.Y + glass.height / 2 > newLoc.Y + 20 || location.Y + glass.height / 2 < newLoc.Y - 20)
                {
                    potionDirection.X = newLoc.X - (location.X + glass.width / 2);
                    potionDirection.Y = newLoc.Y - (location.Y + glass.height / 2);
                    potionDirection.Normalize();

                    location += potionDirection * 5;
                }
            }
            LockObjectsToPotion();
            potionMovementTimer += gameTime.ElapsedGameTime.Milliseconds;
            if(potionMovementTimer < 150)
            {
                location.Y -= 2;
            }
            if(potionMovementTimer >= 150 && potionMovementTimer < 250)
            {
                location.Y -= 1;
            }
            if(potionMovementTimer >= 250 && potionMovementTimer < 400)
            {
                location.Y += 1;
            }
            if(potionMovementTimer >= 400 && potionMovementTimer < 500)
            {
                location.Y += 2;
            }
            if (potionMovementTimer >= 500 && potionMovementTimer < 600)
            {
                location.Y += 1;
            }
            if(potionMovementTimer >= 600 && potionMovementTimer < 700)
            {
                location.Y -= 1;
            }
            if(potionMovementTimer >= 700)
            {
                potionMovementTimer = 0;
            }
        }
    }
}
