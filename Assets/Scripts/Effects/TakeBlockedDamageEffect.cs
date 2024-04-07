using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YT
{
    [CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Blocked Damage")]
    public class TakeBlockedDamageEffect : InstantCharacterEffect
    {
        [Header("Character Causing Damage")]
        public CharacterManager characterCausingDamage; // If the damage is caused by another character attack it will be stored here

        [Header("Damage")]
        public float physicalDamage = 0; // In the future, split into "Standard", "Strike", "Slash", "Pierce"
        public float magicDamage = 0;
        public float fireDamage = 0;
        public float lightningDamage = 0;
        public float holyDamage = 0;

        [Header("Final Damage")]
        private int finalDamageDealt = 0; // The damage the character takes after ALL calculations have been made

        [Header("Poise")]
        public float poiseDamage = 0;
        public bool poiseIsBroken = false; // If broken, they will be stunned and play damage animation

        // To do...
        // Build ups
        // Build up effect amounts

        [Header("Animation")]
        public bool playDamageAnimation = true;
        public bool manuallySelectDamageAnimation = false;
        public string damageAnimation;

        [Header("Sound FX")]
        public bool willPlayDamageSFX = true;
        public AudioClip elementalDamageSoundFX; // Used on top of regular sfx if there is elemental damage present (magic/fire/lightning/holy)

        [Header("Direction Damage Taken From")]
        public float angleHitFrom; // Used to determine what damage animation to play (move backward if hit from front)
        public Vector3 contactPoint; // Used to determine where the blood fx instantiates 

        public override void ProcessEffect(CharacterManager character)
        {
            if (character.characterNetworkManager.isInvulnerable.Value)
                return;

            base.ProcessEffect(character);

            Debug.Log("Hit was blocaked!");

            // If dead, no damage effects
            if (character.isDead.Value)
                return;

            // Discord help, delete later when fixed - FIXED IN EP 46
            // https://discordapp.com/channels/388072935807778836/690242548211515459/1224308709619011654 
            /*
            if (characterCausingDamage != null)
                if (!WorldUtilityManager.Instance.CanIDamageThisTarget(characterCausingDamage.characterGroup, character.characterGroup))
                    return;
            */

            CalculateDamage(character);
            // Check which direction damage came from
            PlayDirectionalBasedBlockingAnimation(character);

            // Play a damage animation

            // Check for build ups (poison, bleed etc)

            // Play damage sound fx
            PlayDamageSFX(character);

            // Play damage vfx (blood)
            PlayDamageVFX(character);

            // If character is AI, check for new target if character causing damage is present
        }

        private void CalculateDamage(CharacterManager character)
        {
            if (!character.IsOwner)
                return;

            if (characterCausingDamage != null)
            {
                // Check for damage modifiers and modify base damage (physical damage buff, elemental damage buff etc)
                // physical += physicalModifier etc
            }

            // Check character for flat defenses and subtract them from the damage

            Debug.Log("Original physical damage: " + physicalDamage);

            // Check character for armor absorptions, and subtract the percentage from the damage
            physicalDamage -= (physicalDamage * (character.characterStatsManager.blockingPhysicalAbsorption / 100));
            magicDamage -= (magicDamage * (character.characterStatsManager.blockingMagicAbsorption / 100));
            fireDamage -= (fireDamage * (character.characterStatsManager.blockingFireAbsorption / 100));
            lightningDamage -= (lightningDamage * (character.characterStatsManager.blockingLightningAbsorption / 100));
            holyDamage -= (holyDamage * (character.characterStatsManager.blockingHolyAbsorption / 100));

            // Add all damage types together, and apply final damage 
            finalDamageDealt = Mathf.RoundToInt(physicalDamage + magicDamage + fireDamage + lightningDamage + holyDamage);

            if (finalDamageDealt <= 0)
            {
                finalDamageDealt = 1;
            }

            Debug.Log("Final physical damage: " + physicalDamage);
            character.characterNetworkManager.currentHealth.Value -= finalDamageDealt;

            // Calculate poise damage to determine if character will be stunned


        }

        private void PlayDamageVFX(CharacterManager character)
        {
            // If we have fire damage, play fire particles
            // Lightning damage, lightning particles

            // Get VFX based on blocking weapon (TODO)
        }

        private void PlayDamageSFX(CharacterManager character)
        {
            // If fire damage is greater than 0, play burn SFX
            // If lightning damage is greater than 0, play zap SFX etc

            // Get SFX based on blocking weapon
        }

        private void PlayDirectionalBasedBlockingAnimation(CharacterManager character)
        {
            if (!character.IsOwner)
                return;

            if (character.isDead.Value)
                return;

            DamageIntensity damageIntensity = WorldUtilityManager.Instance.GetDamageIntensityBasedOnPoiseDamage(poiseDamage);

            //poiseIsBroken = true;

            // Play a proper animation based on intensity
            // Check two handing (TODO)
            switch (damageIntensity)
            {
                case DamageIntensity.Ping:
                    damageAnimation = "Block_Ping_01";
                    break;
                case DamageIntensity.Light:
                    damageAnimation = "Block_Light_01";
                    break;
                case DamageIntensity.Medium:
                    damageAnimation = "Block_Medium_01";
                    break;
                case DamageIntensity.Heavy:
                    damageAnimation = "Block_Heavy_01";
                    break;
                case DamageIntensity.Colossal:
                    damageAnimation = "Block_Colossal_01";
                    break;
                default:
                    break;
            }

            // Always play staggering when blocking
            character.characterAnimatorManager.lastDamageAnimationPlayed = damageAnimation;
            character.characterAnimatorManager.PlayTargetActionAnimation(damageAnimation, true);

        }
    }
}
