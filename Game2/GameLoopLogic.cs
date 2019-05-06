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
    class GameLoopLogic
    {
        public List<Projectile> projectiles = new List<Projectile>();
        public List<Player> players = new List<Player>();
        public List<WallSegment> bricks = new List<WallSegment>();

        AIcontroller aiController;

        ContentManager content;
        GraphicsDeviceManager graphics;
        Boundary boundary;

        public bool paused = false;

        public int menuPointer = 1;

        Texture2D pauseScreenT;
        Animation pauseScreen;

        Texture2D nonCombatTransitionT;
        Texture2D upwardSwipeTransitionT;
        Texture2D playerWinT;

        public Animation nonCombatTransition;
        public Animation upwardSwipeTransition;
        public Animation playerWin;

        Texture2D endGameScreen;

        Rectangle restartRect;
        Rectangle controlsRect;
        Rectangle exitRect;

        public bool backFromPause = false;

        public Vector2 brickLoc1;
        public Vector2 brickLoc2;

        public bool multiChargeSpread;
        public Vector2 multiChargeSpreadLoc;

        public List<FireballImpact> fireballImpacts = new List<FireballImpact>();

        int pauseTimer;

        int spreadTimer;

        public GameLoopLogic(ContentManager content, GraphicsDeviceManager graphics, Boundary boundary)
        {
            this.content = content;
            this.graphics = graphics;
            this.boundary = boundary;
            aiController = new AIcontroller(graphics);
            pauseScreenT = content.Load<Texture2D>("full screen art/pausescreen");
            pauseScreen = new Animation(pauseScreenT, 3, 1);
            pauseScreen.currentFrame = 0;

            upwardSwipeTransitionT = content.Load<Texture2D>("transitions/upwardscreenwipetransitionin");
            nonCombatTransitionT = content.Load<Texture2D>("transitions/tononcombatscreentransition");
            playerWinT = content.Load<Texture2D>("full screen art/playerwin");
            endGameScreen = content.Load<Texture2D>("sprites/endgamebackground");

            playerWin = new Animation(playerWinT, 8, 5);
            nonCombatTransition = new Animation(nonCombatTransitionT, 2, 5);
            upwardSwipeTransition = new Animation(upwardSwipeTransitionT, 3, 2);

            nonCombatTransition.currentFrame = -1;
            upwardSwipeTransition.currentFrame = -1;

            restartRect = new Rectangle(83, 241, 235, 53);
            controlsRect = new Rectangle(restartRect.X, restartRect.Y + restartRect.Height, restartRect.Width, restartRect.Height);
            exitRect = new Rectangle(controlsRect.X, controlsRect.Y + controlsRect.Height, controlsRect.Width, controlsRect.Height);

            brickLoc1 = new Vector2(graphics.PreferredBackBufferWidth / 2 - 27, graphics.PreferredBackBufferHeight - 10);
            brickLoc2 = new Vector2(graphics.PreferredBackBufferWidth / 2 - 27, -10);
            LayBricks();

            multiChargeSpread = false;

            multiChargeSpreadLoc = new Vector2();
        }
        public void UpdateTransitions(GameTime gameTime)
        {
            if (backFromPause == true)
            {
                backFromPause = false;
                upwardSwipeTransition.currentFrame = 0;
            }
            if (upwardSwipeTransition.currentFrame == 5)
            {
                upwardSwipeTransition.currentFrame = -1;
            }
            if (nonCombatTransition.currentFrame > -1)
            {
                nonCombatTransition.Update(gameTime);
            }
            if (upwardSwipeTransition.currentFrame > -1)
            {
                upwardSwipeTransition.Update(gameTime);
            }
        }
        public void LayBricks()
        {
            int index = 1;
            for(int i = 0; i < 14; i++)
            {
                bricks.Add(new WallSegment(content, graphics));
            }
            foreach(WallSegment brick in bricks)
            {
                if(index < 8)
                {
                    if (index == 1)
                    {
                        brick.PlaceBricks((int)brickLoc1.X, (int)brickLoc1.Y, PlayerIndex.One, index);
                    }
                    if (index == 2)
                    {
                        brick.PlaceBricks((int)brickLoc1.X + brick.brick.width, (int)brickLoc1.Y, PlayerIndex.One, index);
                    }
                    if (index == 3)
                    {
                        brick.PlaceBricks((int)brickLoc1.X - brick.brick.width, (int)brickLoc1.Y, PlayerIndex.One, index);
                    }
                    if (index == 4)
                    {
                        brick.PlaceBricks((int)brickLoc1.X + brick.brick.width * 2, (int)brickLoc1.Y, PlayerIndex.One, index);
                    }
                    if (index == 5)
                    {
                        brick.PlaceBricks((int)brickLoc1.X - brick.brick.width * 2, (int)brickLoc1.Y, PlayerIndex.One, index);
                    }
                    if (index == 6)
                    {
                        brick.PlaceBricks((int)brickLoc1.X + brick.brick.width * 3, (int)brickLoc1.Y, PlayerIndex.One, index);
                    }
                    if (index == 7)
                    {
                        brick.PlaceBricks((int)brickLoc1.X - brick.brick.width * 3, (int)brickLoc1.Y, PlayerIndex.One, index);
                    }
                }
                if(index > 7)
                {
                    if (index == 8)
                    {
                        brick.PlaceBricks((int)brickLoc2.X, (int)brickLoc2.Y, PlayerIndex.Two, index);
                    }
                    if (index == 9)
                    {
                        brick.PlaceBricks((int)brickLoc2.X + brick.brick.width, (int)brickLoc2.Y, PlayerIndex.Two, index);
                    }
                    if (index == 10)
                    {
                        brick.PlaceBricks((int)brickLoc2.X - brick.brick.width, (int)brickLoc2.Y, PlayerIndex.Two, index);
                    }
                    if (index == 11)
                    {
                        brick.PlaceBricks((int)brickLoc2.X + brick.brick.width * 2, (int)brickLoc2.Y, PlayerIndex.Two, index);
                    }
                    if (index == 12)
                    {
                        brick.PlaceBricks((int)brickLoc2.X - brick.brick.width * 2, (int)brickLoc2.Y, PlayerIndex.Two, index);
                    }
                    if (index == 13)
                    {
                        brick.PlaceBricks((int)brickLoc2.X + brick.brick.width * 3, (int)brickLoc2.Y, PlayerIndex.Two, index);
                    }
                    if (index == 14)
                    {
                        brick.PlaceBricks((int)brickLoc2.X - brick.brick.width * 3, (int)brickLoc2.Y, PlayerIndex.Two, index);
                    }
                }
                index++;
            }
        }
        public void ListChecks(GameTime gameTime)
        {
            
            if (multiChargeSpread == true)
            {
                projectiles.Add(new Projectile(InputAction.ChargeShot, 1f, multiChargeSpreadLoc, content, graphics, PlayerIndex.One, boundary.bounds, 2, true));
                projectiles.Add(new Projectile(InputAction.ChargeShot, 0f, multiChargeSpreadLoc, content, graphics, PlayerIndex.One, boundary.bounds, 1, true));
                projectiles.Add(new Projectile(InputAction.ChargeShot, -1f, multiChargeSpreadLoc, content, graphics, PlayerIndex.One, boundary.bounds, 1, true));
                projectiles.Add(new Projectile(InputAction.ChargeShot, 1f, multiChargeSpreadLoc, content, graphics, PlayerIndex.Two, boundary.bounds, 1, true));
                projectiles.Add(new Projectile(InputAction.ChargeShot, 0f, multiChargeSpreadLoc, content, graphics, PlayerIndex.Two, boundary.bounds, 2, true));
                projectiles.Add(new Projectile(InputAction.ChargeShot, -1f, multiChargeSpreadLoc, content, graphics, PlayerIndex.Two, boundary.bounds, 2, true));
                multiChargeSpread = false;
            }
            
            boundary.Update(gameTime);
            boundary.GrabPorjectileLoc(projectiles);
            foreach(WallSegment brick in bricks)
            {
                brick.Update(gameTime);
            }
            if (paused == false)
            {
                players[0].Update(gameTime);
                players[1].Update(gameTime);
                players[0].impactLocation = ImpactLocations.None;
                players[1].impactLocation = ImpactLocations.None;
                foreach (Player player in players)
                {
                    player.input.controlType = ControlType.GamePlay;
                    if(player.inputAction == InputAction.Pause)
                    {
                        paused = true;
                        player.inputAction = InputAction.None;
                    }
                    if (player.inputAction == InputAction.Shoot || player.inputAction == InputAction.ChargeShot)
                    {
                        if (player.AI == true)
                        {
                            projectiles.Add(new Projectile(player.inputAction, aiController.angle, aiController.projectileOrigin, content, graphics, player.playerIndex, boundary.bounds, player.playerChoice));
                        }
                        else if (player.AI == false)
                        {
                            projectiles.Add(new Projectile(player.inputAction, player.shootingAngle, player.projectileOrigin, content, graphics, player.playerIndex, boundary.bounds, player.playerChoice));
                        }
                    }
                    if (player.inputAction == InputAction.Reflect)
                    {
                        foreach (Projectile projectile in projectiles)
                        {
                            if (projectile.bounds.Intersects(player.reflectHitBox))
                            {
                                if (projectile.recentlyReflected >= 50)
                                {
                                    if (player.playerChoice != 2)
                                    {
                                        projectile.playerChoice = player.playerChoice;
                                        if (projectile.playerIndex == PlayerIndex.One)
                                        {
                                            projectile.playerIndex = PlayerIndex.Two;
                                        }
                                        else if (projectile.playerIndex == PlayerIndex.Two)
                                        {
                                            projectile.playerIndex = PlayerIndex.One;
                                        }
                                        projectile.tempNoCollison = false;
                                        projectile.direction.Y = -projectile.direction.Y;
                                        projectile.bounceLocation = projectile.location;
                                        projectile.angle = -projectile.angle + Math.PI;
                                    }
                                }
                                projectile.recentlyReflected = 0;
                            }
                        }
                    }
                    foreach (Projectile projectile in projectiles)
                    {
                        projectile.Update(gameTime);
                        
                        if (projectile.collisionLocation == CollidedWith.TopGoal || projectile.collisionLocation == CollidedWith.BottomGoal)
                        {
                            if (player.playerIndex == PlayerIndex.One)
                            {
                                player.score++;
                            }
                            if (player.playerIndex == PlayerIndex.Two)
                            {
                                player.score++;
                            }
                        }
                        if (projectile.bounds.Intersects(player.reflectHitBox))
                        {
                            if (player.playerChoice == 2)
                            {
                                if (player.tiredreflectAnimation.currentFrame > -1)
                                {
                                    projectile.playerChoice = 2;
                                    if (projectile.playerIndex != player.playerIndex)
                                    {
                                        if (projectile.speed == projectile.originalSpeed && projectile.reverseTimer > 200)
                                        {
                                            if (projectile.playerIndex == PlayerIndex.One)
                                            {
                                                projectile.playerIndex = PlayerIndex.Two;
                                            }
                                            else if (projectile.playerIndex == PlayerIndex.Two)
                                            {
                                                projectile.playerIndex = PlayerIndex.One;
                                            }
                                            projectile.reversing = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    for (int i = 0; i < projectiles.Count; i++)
                    {
                        
                        if (projectiles[i].removalPing == true)
                        {
                            projectiles.RemoveAt(i--);
                        }
                        else if (projectiles[i].collisionLocation == CollidedWith.TopGoal || projectiles[i].collisionLocation == CollidedWith.BottomGoal)
                        {
                            projectiles.RemoveAt(i--);
                            aiController.markCounter--;
                        }
                        else
                        {
                            for (int i_ = 0; i_ < projectiles.Count; i_++)
                            {
                                if (projectiles[i].location != projectiles[i_].location)
                                {
                                    if (projectiles[i].tempNoCollison == false && projectiles[i_].tempNoCollison == false)
                                    {
                                        if (projectiles[i].bounds.Intersects(projectiles[i_].bounds))
                                        {
                                            if (projectiles[i].chargeShot == true && projectiles[i_].chargeShot == false)
                                            {
                                                fireballImpacts.Add(new FireballImpact(graphics, content, new Vector2(projectiles[i_].bounds.Center.X, projectiles[i_].bounds.Center.Y), projectiles[i_].originalOwner, projectiles[i_].chargeShot, false, 0, false));
                                                projectiles.RemoveAt(i_--);
                                            }
                                            if (projectiles[i].chargeShot == false && projectiles[i_].chargeShot == false)
                                            {
                                                projectiles[i].removalPing = true;
                                                fireballImpacts.Add(new FireballImpact(graphics, content, new Vector2(projectiles[i_].bounds.Center.X, projectiles[i_].bounds.Center.Y), projectiles[i_].originalOwner, projectiles[i_].chargeShot, false, 0, false));
                                                projectiles.RemoveAt(i_--);
                                            }
                                            if (projectiles[i].chargeShot == true && projectiles[i_].chargeShot == true && projectiles[i].originalOwner == 1 && projectiles[i_].originalOwner == 2)
                                            {
                                                projectiles[i_].removalPing = true;
                                                multiChargeSpreadLoc = projectiles[i].location;
                                                fireballImpacts.Add(new FireballImpact(graphics, content, new Vector2(projectiles[i_].bounds.Center.X, projectiles[i_].bounds.Center.Y), projectiles[i_].originalOwner, projectiles[i_].chargeShot, false, 0, true));
                                                projectiles.RemoveAt(i--);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    for (int i = 0; i < projectiles.Count; i++)
                    {
                        if (projectiles[i].bounds.Intersects(player.hitBox))
                        {
                            if (projectiles[i].playerIndex != player.playerIndex)
                            {
                                if(projectiles[i].bounds.Center.X > player.hitBox.Center.X)
                                {
                                    player.impactLocation = ImpactLocations.RightImpact;
                                    if (projectiles[i].chargeShot == true)
                                    {
                                        if (projectiles[i].originalOwner == 1)
                                        {
                                            player.stunned = true;
                                            player.stunTimer = 0;
                                        }
                                        if (projectiles[i].originalOwner == 2)
                                        {
                                            player.slept = true;
                                            player.sleepTimer = 0;
                                        }
                                    }
                                }
                                else
                                {
                                    player.impactLocation = ImpactLocations.LeftImpact;
                                    if (projectiles[i].chargeShot == true)
                                    {
                                        if (projectiles[i].originalOwner == 1)
                                        {
                                            player.stunned = true;
                                            player.stunTimer = 0;
                                        }
                                        if(projectiles[i].originalOwner == 2)
                                        {
                                            player.slept = true;
                                            player.sleepTimer = 0;
                                        }
                                    }
                                }
                                fireballImpacts.Add(new FireballImpact(graphics, content, new Vector2(projectiles[i].bounds.Center.X, projectiles[i].bounds.Top), projectiles[i].originalOwner, projectiles[i].chargeShot, true, (float)projectiles[i].angle, false));
                                projectiles.RemoveAt(i--);
                                aiController.markCounter--;
                            }
                        }
                    }
                    foreach (WallSegment brick in bricks)
                    {
                        for (int i = 0; i < projectiles.Count; i++)
                        {
                            if (projectiles[i].bounds.Intersects(brick.brickRect) && brick.health >= 0)
                            {
                                if (brick.health == 0)
                                {

                                }
                                else
                                {
                                    if (projectiles[i].location.Y > 300)
                                    {
                                        fireballImpacts.Add(new FireballImpact(graphics, content, new Vector2(projectiles[i].bounds.Center.X, projectiles[i].bounds.Bottom), projectiles[i].originalOwner, projectiles[i].chargeShot, false, 0, false));
                                    }
                                    if (projectiles[i].location.Y < 300)
                                    {
                                        fireballImpacts.Add(new FireballImpact(graphics, content, new Vector2(projectiles[i].bounds.Center.X, projectiles[i].bounds.Top), projectiles[i].originalOwner, projectiles[i].chargeShot, false, 0, false));
                                    }
                                    projectiles.RemoveAt(i--);
                                    brick.health--;
                                    if (brick.timer > 300)
                                    {
                                        brick.timer = 0;
                                    }
                                }
                            }
                        }
                    }
                    if(fireballImpacts.Count == 0)
                    {
                        player.slowed = false;
                    }
                    for (int i = 0; i < fireballImpacts.Count; i++)
                    {
                        fireballImpacts[i].Update(gameTime);
                        if (fireballImpacts[i].doubleCharge == true)
                        {
                            
                            if (fireballImpacts[i].impact4.currentFrame == 4 && spreadTimer == 0)
                            {
                                
                                multiChargeSpread = true;
                                spreadTimer += gameTime.ElapsedGameTime.Milliseconds;
                            }
                            if(fireballImpacts[i].impact4.currentFrame == 5)
                            {
                                spreadTimer = 0;
                            }
                        }
                        if(player.hitBox.Intersects(fireballImpacts[i].sleepyEffectBounds))
                        {
                            player.slowed = true;
                        }
                        else
                        {
                            player.slowed = false;
                        }
                        if (fireballImpacts[i].chargeShot == true)
                        {
                            if (fireballImpacts[i].bulletType == 1 || fireballImpacts[i].bulletType == 2)
                            {
                                if (fireballImpacts[i].impact.currentFrame == 8 && fireballImpacts[i].bulletType == 1 || fireballImpacts[i].impact.currentFrame == 12 && fireballImpacts[i].onPlayer == true && fireballImpacts[i].bulletType == 2 || fireballImpacts[i].chargeShot == false && fireballImpacts[i].impact.currentFrame == 7 || fireballImpacts[i].bulletType == 2 && fireballImpacts[i].sleepTimer > 5000)
                                {
                                    fireballImpacts.RemoveAt(i--);
                                }
                            }
                        }
                    }
                }

                if (players[1].AI == true)
                {
                    aiController.AIParser(players, projectiles);
                    aiController.Update(gameTime);
                }
                pauseTimer = 0;
            }
            else
            {
                pauseTimer += gameTime.ElapsedGameTime.Milliseconds;
                if (pauseTimer > 300)
                {
                    foreach (Player player in players)
                    {
                        player.input.controlType = ControlType.Menu;
                        player.input.Update(gameTime);
                        if (player.input.inputAction == InputAction.Pause)
                        {
                            paused = false;
                            player.input.inputAction = InputAction.None;
                            player.inputAction = InputAction.None;
                        }
                        if (player.input.inputAction == InputAction.Up)
                        {
                            menuPointer--;
                        }
                        if (player.input.inputAction == InputAction.Down)
                        {
                            menuPointer++;
                        }
                        if (menuPointer < 1)
                        {
                            menuPointer = 3;
                        }
                        if (menuPointer > 3)
                        {
                            menuPointer = 1;
                        }
                        if (restartRect.Contains(player.input.mousePos))
                        {
                            menuPointer = 1;
                        }
                        if (controlsRect.Contains(player.input.mousePos))
                        {
                            menuPointer = 2;
                        }
                        if (exitRect.Contains(player.input.mousePos))
                        {
                            menuPointer = 3;
                        }
                        if (player.input.inputAction == InputAction.Confirm && menuPointer == 1)
                        {
                            nonCombatTransition.currentFrame = 0;
                        }
                        if (player.input.inputAction == InputAction.Confirm && menuPointer == 2)
                        {
                            nonCombatTransition.currentFrame = 0;
                        }
                        if (player.input.inputAction == InputAction.Confirm && menuPointer == 3)
                        {
                            nonCombatTransition.currentFrame = 0;
                        }
                    }
                }
            }
        }
        public void TimeUpConditions(GameTime gameTime)
        {
            playerWin.Update(gameTime);
            if (playerWin.currentFrame < 25)
            {
                playerWin.currentFrame = 25;
            }
            if(playerWin.currentFrame > 35)
            {
                playerWin.currentFrame = 35;
            }
        }
        public void DrawTie(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(endGameScreen, new Vector2(0, 0), Color.White);
            playerWin.Draw(spriteBatch, new Vector2(0, 0));
        }
        public void PlayerDeathConditions(GameTime gameTime)
        {
            playerWin.Update(gameTime);
            if (players[0].health == 0)
            {
                if(playerWin.currentFrame == 8)
                {
                    playerWin.currentFrame = 17;
                }
                if(playerWin.currentFrame > 24)
                {
                    playerWin.currentFrame = 24;
                }
            }
            else if (players[1].health == 0)
            {
                if(playerWin.currentFrame > 16)
                {
                    playerWin.currentFrame = 16;
                }
            }
        }
        public void DrawLists(SpriteBatch spriteBatch)
        {
            
            foreach (Player player in players)
            {
                player.Draw(spriteBatch);
            }
            foreach (Projectile projectile in projectiles)
            {
                projectile.Draw(spriteBatch);
            }
            foreach(FireballImpact fireballImpact in fireballImpacts)
            {
                fireballImpact.Draw(spriteBatch);
            }
            if (paused == true)
            {
                pauseScreen.Draw(spriteBatch, new Vector2(0, 0));
                if (menuPointer == 1)
                {
                    pauseScreen.currentFrame = 0;
                }
                if (menuPointer == 2)
                {
                    pauseScreen.currentFrame = 1;
                }
                if (menuPointer == 3)
                {
                    pauseScreen.currentFrame = 2;
                }
            }
            if (upwardSwipeTransition.currentFrame > -1)
            {
                upwardSwipeTransition.Draw(spriteBatch, new Vector2(0, 0));
            }
            if (nonCombatTransition.currentFrame > -1)
            {
                nonCombatTransition.Draw(spriteBatch, new Vector2(0, 0));
            }
            if (players[0].health == 0 || players[1].health == 0)
            {
                spriteBatch.Draw(endGameScreen, new Vector2(0, 0), Color.White);
                playerWin.Draw(spriteBatch, new Vector2(0, 0));
            }
            foreach(WallSegment brick in bricks)
            {
                brick.Draw(spriteBatch);
            }
        }
    }
}
