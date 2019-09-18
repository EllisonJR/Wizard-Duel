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
    class AssetContainer
    {
        public ContentManager content;

        public static SpriteFont font;

        public static Texture2D bigDoubleImpactT1;
        public static Texture2D bigDoubleImpactT2;
        public static Texture2D bigDoubleImpactT3;
        public static Texture2D bigDoubleImpactT4;
        public static Texture2D buffImpactOnPlayerT;
        public static Texture2D tiredImpactOnPlayerT;
        public static Texture2D chargeImpactDefaultT;
        public static Texture2D nonChargeImpactT;

        public static Texture2D brickT;
        public static Texture2D pillarT;
        public static Texture2D brickDebrisT;
        public static Texture2D border;

        public static Texture2D statsButtonT;
        public static Texture2D versusButtonT;
        public static Texture2D controlInfoButtonT;
        public static Texture2D exitButtonT;
        public static Texture2D singleplayerbuttonT;
        public static Texture2D title1;
        public static Texture2D title2;
        public static Texture2D title3;
        public static Texture2D menuIdle;
        public static Texture2D fireballAnimationT;
        public static Texture2D explosionAnimationT;
        public static Texture2D upwardWipeTransitionT;
        public static Texture2D nonCombatTransitionT;

        public static Texture2D endGameScreen;
        public static Texture2D upwardSwipeTransitionT;
        public static Texture2D playerWinT;
        public static Texture2D pauseScreenT;
        public static Texture2D countDownT;

        public static Texture2D chargePotAnimT1;
        public static Texture2D chargePotAnimT2;
        public static Texture2D chargePotAnimT3;
        public static Texture2D chargePotAnimT4;
        public static Texture2D chargePotAnimT5;
        public static Texture2D dashRightTrailT;
        public static Texture2D dashLeftTrailT;
        public static Texture2D dashRightTrailT2;
        public static Texture2D dashLeftTrailT2;
        public static Texture2D dashRightTrailT3;
        public static Texture2D dashLeftTrailT3;
        public static Texture2D dashRightTrailT4;
        public static Texture2D dashLeftTrailT4;
        public static Texture2D landingPoofT;
        public static Texture2D kickedUpDustT;
        public static Texture2D sleepEffectT;
        public static Texture2D stunnedEffectT;
        public static Texture2D slowEffectT;
        public static Texture2D buffHitboxT;
        public static Texture2D tiredHitboxT;
        public static Texture2D tiredReflectBoxT;
        public static Texture2D buffReflectBoxT;
        public static Texture2D shotMeterTexture;
        public static Texture2D buffSpriteT;
        public static Texture2D tiredSpriteT;
        public static Texture2D tiredReflectT;
        public static Texture2D playerRetical1;
        public static Texture2D playerRetical2;

        public static Texture2D buffWizardT;
        public static Texture2D tiredWizardT;
        public static Texture2D randomT;
        public static Texture2D selecterT;
        public static Texture2D toCombatTransitionT;
        public static Texture2D toMenuTransitionT;
        public static Texture2D toSelectTransitionT;
        public static Texture2D backArrowT;

        public static Texture2D keyboardcontrols;
        public static Texture2D controlcontrols;

        public static Texture2D chargeGlassT;
        public static Texture2D shieldGlassT;
        public static Texture2D skullGlassT;
        public static Texture2D wallGlassT;
        public static Texture2D fireballGlassT;
        public static Texture2D smoke1;
        public static Texture2D smoke2;
        public static Texture2D potionParticleT;
        public static Texture2D wallPotionLiquidT;
        public static Texture2D wallPotionTopT;
        public static Texture2D wallPotionMiddleT;
        public static Texture2D wallPotionBottomT;
        public static Texture2D wallPotionPassThroughTopT;
        public static Texture2D wallPotionPassThroughBottomT;
        public static Texture2D rainbowWallBounceT;
        public static Texture2D shieldFrameT;
        public static Texture2D panel1;
        public static Texture2D panel2;
        public static Texture2D panel3;
        public static Texture2D panel4;
        public static Texture2D panel5;
        public static Texture2D panel6;
        public static Texture2D panel7;
        public static Texture2D panel8;
        public static Texture2D panel9;
        public static Texture2D panel10;
        public static Texture2D potionExplosionGlowT;
        public static Texture2D potionIntroBallT;
        public static Texture2D potionIntroFireT;
        public static Texture2D potionIntroIconT;
        public static Texture2D skullBulletT;
        public static Texture2D skullBulletImpactT;
        public static Texture2D potionSellerT;
        public static Texture2D travelerT;

        public static Texture2D fireballT;
        public static Texture2D fireballTrailT1;
        public static Texture2D fireballTrailT2;
        public static Texture2D tiredWizardChargeShotT;
        public static Texture2D buffWizardChargeShotT;
        public static Texture2D wallBounceT;

        public static Texture2D porbs;

        public AssetContainer()
        {
        }
        public void GrabContentManager(ContentManager content)
        {
            this.content = content;
        }
        public void LoadImpactAssets()
        {
            bigDoubleImpactT1 = content.Load<Texture2D>("sprites/player effects/multichargeimpactone");
            bigDoubleImpactT2 = content.Load<Texture2D>("sprites/player effects/multichargeimpacttwo");
            bigDoubleImpactT3 = content.Load<Texture2D>("sprites/player effects/multichargeimpactthree");
            bigDoubleImpactT4 = content.Load<Texture2D>("sprites/player effects/multichargeimpactfour");
            buffImpactOnPlayerT = content.Load<Texture2D>("sprites/player effects/buffimpactonplayer");
            tiredImpactOnPlayerT = content.Load<Texture2D>("sprites/player effects/tiredwizardonplayershot");
            chargeImpactDefaultT = content.Load<Texture2D>("sprites/border items/chargeimpact");
            nonChargeImpactT = content.Load<Texture2D>("sprites/player effects/nonchargeimpact");
        }
        public void LoadBoundaryAssets()
        {
            brickT = content.Load<Texture2D>("sprites/border items/breakwall");
            brickDebrisT = content.Load<Texture2D>("sprites/border items/brickdebris");
            border = content.Load<Texture2D>("sprites/fireball");
            pillarT = content.Load<Texture2D>("sprites/border items/pillar");
        }
        public void LoadMainMenuAssets()
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
        }
        public void LoadGameLoopAssets()
        {
            upwardSwipeTransitionT = content.Load<Texture2D>("transitions/upwardscreenwipetransitionin");
            playerWinT = content.Load<Texture2D>("full screen art/playerwin");
            endGameScreen = content.Load<Texture2D>("sprites/endgamebackground");
            pauseScreenT = content.Load<Texture2D>("full screen art/pausescreen");
            countDownT = content.Load<Texture2D>("full screen art/countdown");
        }
        public void LoadPlayerAssets()
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
            chargePotAnimT1 = content.Load<Texture2D>("sprites/player effects/chargepotionanim1");
            chargePotAnimT2 = content.Load<Texture2D>("sprites/player effects/chargepotionanim2");
            chargePotAnimT3 = content.Load<Texture2D>("sprites/player effects/chargepotionanim3");
            chargePotAnimT4 = content.Load<Texture2D>("sprites/player effects/chargepotionanim4");
            chargePotAnimT5 = content.Load<Texture2D>("sprites/player effects/chargepotionanim5");
            playerRetical1 = content.Load<Texture2D>("sprites/playerSprites/aimingretical");
            playerRetical2 = content.Load<Texture2D>("sprites/playerSprites/aimingreticalP2");
        }
        public void LoadPlayerSelectAssets()
        {
            buffWizardT = content.Load<Texture2D>("character select buttons/buffwizardicon");
            tiredWizardT = content.Load<Texture2D>("character select buttons/tiredwizardicon");
            randomT = content.Load<Texture2D>("character select buttons/randomicon");
            selecterT = content.Load<Texture2D>("character select buttons/playerdongle");
            toMenuTransitionT = content.Load<Texture2D>("transitions/swipetransition");
            toCombatTransitionT = content.Load<Texture2D>("transitions/playerselectconfirm");
            toSelectTransitionT = content.Load<Texture2D>("transitions/circletransitionreverse");
            backArrowT = content.Load<Texture2D>("transitions/backarrow");
        }
        public void LoadControlInfoScreenAssets()
        {
            keyboardcontrols = content.Load<Texture2D>("full screen art/kbcontrols");
            controlcontrols = content.Load<Texture2D>("full screen art/controllercontrols");
        }
        public void LoadMiscAssets()
        {
            smoke1 = content.Load<Texture2D>("sprites/Potions/smoke1");
            smoke2 = content.Load<Texture2D>("sprites/Potions/smoke2");
            potionParticleT = content.Load<Texture2D>("sprites/Potions/potionsparkle");
            chargeGlassT = content.Load<Texture2D>("sprites/Potions/chargepotion");
            shieldGlassT = content.Load<Texture2D>("sprites/Potions/shieldpotion");
            fireballGlassT = content.Load<Texture2D>("sprites/Potions/fireballpotion");
            wallGlassT = content.Load<Texture2D>("sprites/Potions/wallpotion");
            skullGlassT = content.Load<Texture2D>("sprites/Potions/skullpotion");
            wallPotionBottomT = content.Load<Texture2D>("sprites/Potions/wallbottom"); ;
            wallPotionMiddleT = content.Load<Texture2D>("sprites/Potions/wallmiddle");
            wallPotionTopT = content.Load<Texture2D>("sprites/Potions/walltop");
            wallPotionLiquidT = content.Load<Texture2D>("sprites/Potions/wallpotionanimationfluid");
            wallPotionPassThroughTopT = content.Load<Texture2D>("sprites/Potions/wallpassthroughontop");
            wallPotionPassThroughBottomT = content.Load<Texture2D>("sprites/Potions/wallpassthroughonbottom");
            rainbowWallBounceT = content.Load<Texture2D>("sprites/Potions/rainbowwallbounce");
            shieldFrameT = content.Load<Texture2D>("sprites/Potions/shieldframeanimation");
            panel1 = content.Load<Texture2D>("sprites/Potions/panel 1");
            panel2 = content.Load<Texture2D>("sprites/Potions/panel 2");
            panel3 = content.Load<Texture2D>("sprites/Potions/panel 3");
            panel4 = content.Load<Texture2D>("sprites/Potions/panel 4");
            panel5 = content.Load<Texture2D>("sprites/Potions/panel 5");
            panel6 = content.Load<Texture2D>("sprites/Potions/panel 6");
            panel7 = content.Load<Texture2D>("sprites/Potions/panel 7");
            panel8 = content.Load<Texture2D>("sprites/Potions/panel 8");
            panel9 = content.Load<Texture2D>("sprites/Potions/panel 9");
            panel10 = content.Load<Texture2D>("sprites/Potions/panel 10");
            potionExplosionGlowT = content.Load<Texture2D>("full screen art/potionexplosionglow");
            potionIntroBallT = content.Load<Texture2D>("full screen art/potionintroball");
            potionIntroFireT = content.Load<Texture2D>("full screen art/potionintrofire");
            potionIntroIconT = content.Load<Texture2D>("full screen art/potionintroicon");
            skullBulletT = content.Load<Texture2D>("sprites/Potions/skullBullet");
            skullBulletImpactT = content.Load<Texture2D>("sprites/Potions/skullbulletimpact");
            potionSellerT = content.Load<Texture2D>("full screen art/potionseller");
            travelerT = content.Load<Texture2D>("full screen art/traveller");
        }
        public void LoadProjectileAssets()
        {
            fireballT = content.Load<Texture2D>("sprites/player effects/mainfireball");
            fireballTrailT1 = content.Load<Texture2D>("sprites/player effects/fireballtail1");
            fireballTrailT2 = content.Load<Texture2D>("sprites/player effects/fireballtail2");
            buffWizardChargeShotT = content.Load<Texture2D>("sprites/player effects/buffchargeshot");
            tiredWizardChargeShotT = content.Load<Texture2D>("sprites/player effects/tiredchargeshot");
            wallBounceT = content.Load<Texture2D>("sprites/border items/wallbounce");
        }
    }
}
