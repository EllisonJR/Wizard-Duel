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

public enum ImpactLocations { LeftImpact, RightImpact, None}

namespace WizardDuel
{
    class FireballImpact : AssetContainer
    {
        public Animation impact;
        public Animation impact2;
        public Animation impact3;
        public Animation impact4;

        public Rectangle impactBounds;
        public Rectangle sleepyEffectBounds;

        Vector2 impactLocation;

        public int bulletType;
        public int sleepTimer;
        public bool chargeShot;
        public bool onPlayer;
        public float angle;
        public bool doubleCharge;
        public bool rainbowWall = false;

        public bool fromSkull = false;

        public FireballImpact(Vector2 location, int bulletType, bool chargeShot, bool onPlayer, float angle, bool doubleCharge, bool fromSkull)
        {
            this.bulletType = bulletType;
            this.chargeShot = chargeShot;
            this.onPlayer = onPlayer;
            this.angle = angle;
            this.doubleCharge = doubleCharge;

            sleepTimer = 0;
            if (rainbowWall == false)
            {
                if (chargeShot == true)
                {
                    if (onPlayer == false)
                    {
                        if (doubleCharge == true)
                        {
                            impact = new Animation(bigDoubleImpactT1, 2, 5);
                            impact2 = new Animation(bigDoubleImpactT2, 3, 5);
                            impact3 = new Animation(bigDoubleImpactT3, 2, 5);
                            impact4 = new Animation(bigDoubleImpactT4, 3, 5);

                            impact.frameTime = 150;
                            impact2.frameTime = 150;
                            impact3.frameTime = 150;
                            impact4.frameTime = 150;

                            impact.currentFrame = 0;
                            impact2.currentFrame = 0;
                            impact3.currentFrame = 0;
                            impact4.currentFrame = 0;

                            impactLocation = new Vector2(location.X - impact.width / 2, location.Y - impact.height / 2);
                        }
                        else
                        {
                            impact = new Animation(chargeImpactDefaultT, 3, 8);

                            impactLocation = new Vector2(location.X - impact.width / 2, location.Y - impact.height / 2);
                            if (bulletType == 1)
                            {
                                impact.currentFrame = 0;
                            }
                            if (bulletType == 2)
                            {
                                impact.currentFrame = 9;

                                impactBounds = new Rectangle((int)impactLocation.X, (int)impactLocation.Y, impact.width, impact.height);
                                sleepyEffectBounds = new Rectangle((int)impactBounds.X + 11, (int)impactBounds.Y, impactBounds.Width - 38, impactBounds.Height);
                            }
                        }
                    }
                    else if (onPlayer == true)
                    {
                        if (bulletType == 1)
                        {
                            impact = new Animation(buffImpactOnPlayerT, 1, 10);

                            impactLocation = new Vector2(location.X - impact.width / 2, location.Y - impact.height / 2);
                        }
                        if (bulletType == 2)
                        {
                            impact = new Animation(tiredImpactOnPlayerT, 1, 12);

                            impactLocation = new Vector2(location.X - impact.width / 2, location.Y - impact.height / 2);
                        }
                    }
                }
                else if (chargeShot == false && fromSkull == false)
                {
                    impact = new Animation(nonChargeImpactT, 2, 4);
                    impactBounds = new Rectangle((int)impactLocation.X, (int)impactLocation.Y, impact.width, impact.height);
                    impactLocation = new Vector2(location.X - impact.width / 2, location.Y - impact.height / 2);
                }
                else if (fromSkull == true)
                {
                    this.fromSkull = fromSkull;
                    impact = new Animation(skullBulletImpactT, 2, 3);
                    impactLocation = new Vector2(location.X - impact.width / 2, location.Y - impact.height / 2);
                }
            }
            else
            {
                impact = new Animation(rainbowWallBounceT, 1, 3);
                impact.frameTime = 125;
                impact.rainbow = true;
                impactLocation = new Vector2(location.X - impact.width / 2, location.Y - impact.height / 2);
            }
        }
        public FireballImpact(Vector2 location, int bulletType, bool chargeShot, bool onPlayer, float angle, bool doubleCharge, bool rainbowWall, bool rainbow, bool fromSkull)
        {
            this.bulletType = bulletType;
            this.chargeShot = chargeShot;
            this.onPlayer = onPlayer;
            this.angle = angle;
            this.doubleCharge = doubleCharge;
            this.rainbowWall = rainbowWall;

            sleepTimer = 0;
            if (rainbowWall == false)
            {
                if (chargeShot == true)
                {
                    if (onPlayer == false)
                    {
                        if (doubleCharge == true)
                        {
                            impact = new Animation(bigDoubleImpactT1, 2, 5);
                            impact2 = new Animation(bigDoubleImpactT2, 3, 5);
                            impact3 = new Animation(bigDoubleImpactT3, 2, 5);
                            impact4 = new Animation(bigDoubleImpactT4, 3, 5);

                            impact.frameTime = 150;
                            impact2.frameTime = 150;
                            impact3.frameTime = 150;
                            impact4.frameTime = 150;

                            impact.currentFrame = 0;
                            impact2.currentFrame = 0;
                            impact3.currentFrame = 0;
                            impact4.currentFrame = 0;

                            impactLocation = new Vector2(location.X - impact.width / 2, location.Y - impact.height / 2);
                        }
                        else
                        {
                            impact = new Animation(chargeImpactDefaultT, 3, 8);

                            impactLocation = new Vector2(location.X - impact.width / 2, location.Y - impact.height / 2);
                            if (bulletType == 1)
                            {
                                impact.currentFrame = 0;
                            }
                            if (bulletType == 2)
                            {
                                impact.currentFrame = 9;

                                impactBounds = new Rectangle((int)impactLocation.X, (int)impactLocation.Y, impact.width, impact.height);
                                sleepyEffectBounds = new Rectangle((int)impactBounds.X + 11, (int)impactBounds.Y, impactBounds.Width - 38, impactBounds.Height);
                            }
                        }
                    }
                    else if (onPlayer == true)
                    {
                        if (bulletType == 1)
                        {
                            impact = new Animation(buffImpactOnPlayerT, 1, 10);

                            impactLocation = new Vector2(location.X - impact.width / 2, location.Y - impact.height / 2);
                        }
                        if (bulletType == 2)
                        {
                            impact = new Animation(tiredImpactOnPlayerT, 1, 12);

                            impactLocation = new Vector2(location.X - impact.width / 2, location.Y - impact.height / 2);
                        }
                    }
                }
                else if (chargeShot == false && fromSkull == false)
                {
                    impact = new Animation(nonChargeImpactT, 2, 4);
                    impact.rainbow = rainbow;
                    impactBounds = new Rectangle((int)impactLocation.X, (int)impactLocation.Y, impact.width, impact.height);
                    impactLocation = new Vector2(location.X - impact.width / 2, location.Y - impact.height / 2);
                }
                else if (fromSkull == true)
                {
                    this.fromSkull = fromSkull;
                    impact = new Animation(skullBulletImpactT, 2, 3);
                    impactLocation = new Vector2(location.X - impact.width / 2, location.Y - impact.height / 2);
                }
            }
            
            else
            {
                impact = new Animation(rainbowWallBounceT, 1, 3);
                impact.frameTime = 125;
                impact.rainbow = true;
                impactLocation = new Vector2(location.X - impact.width / 2, location.Y - impact.height / 2);
            }
        }
        public void Update(GameTime gameTime)
        {
            if (rainbowWall == false)
            {
                if (chargeShot == true && doubleCharge == false)
                {
                    if (bulletType == 1 && chargeShot == true)
                    {
                        if (onPlayer == true)
                        {
                            impact.frameTime = 75;
                            impact.Update(gameTime, angle);
                        }
                        else
                        {
                            impact.frameTime = 75;
                            impact.Update(gameTime);
                        }
                    }
                    if (bulletType == 2 && chargeShot == true)
                    {
                        impact.frameTime = 150;
                        impact.Update(gameTime);
                    }
                    if (impact.currentFrame == 21)
                    {
                        impact.currentFrame = 17;
                    }
                    if (bulletType == 2 && impact.currentFrame >= 17)
                    {
                        sleepTimer += gameTime.ElapsedGameTime.Milliseconds;

                    }
                }
                else if (chargeShot == false && doubleCharge == false)
                {
                    impact.Update(gameTime);
                }
                else if (doubleCharge == true && fromSkull == false)
                {
                    if (impact.currentFrame < 9)
                    {
                        impact.Update(gameTime);
                    }
                    if (impact2.currentFrame < 11 && impact.currentFrame == 9)
                    {
                        impact2.Update(gameTime);
                    }
                    if (impact3.currentFrame < 10 && impact2.currentFrame == 11)
                    {
                        impact3.Update(gameTime);
                    }
                    if (impact4.currentFrame < 13 && impact3.currentFrame == 10)
                    {
                        impact4.Update(gameTime);
                    }
                }
            }
            else
            {
                impact.Update(gameTime);
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (rainbowWall == false)
            {
                if (onPlayer == false && doubleCharge == false)
                {
                    impact.Draw(spriteBatch, impactLocation);
                }
                else if (doubleCharge == true)
                {
                    if (impact.currentFrame < 9)
                    {
                        impact.Draw(spriteBatch, impactLocation);
                    }
                    if (impact2.currentFrame < 11 && impact.currentFrame == 9)
                    {
                        impact2.Draw(spriteBatch, impactLocation);
                    }
                    if (impact3.currentFrame < 10 && impact2.currentFrame == 11)
                    {
                        impact3.Draw(spriteBatch, impactLocation);
                    }
                    if (impact4.currentFrame < 13 && impact3.currentFrame == 10)
                    {
                        impact4.Draw(spriteBatch, impactLocation);
                    }
                }
                else
                {
                    impact.DrawProjectile(spriteBatch, impactLocation, new Rectangle(0, 0, 0, 0));
                }
            }
            else
            {
                impact.Draw(spriteBatch, impactLocation);
            }
        }
    }
}
