using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Printing;
using UnityEngine;

namespace YT
{
    [CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Damage")]
    public class TakeDamageEffect : InstantCharacterEffect
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
            PlayDirectionalBasedDamageAnimation(character);

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

            // Check character for armor absorptions, and subtract the percentage from the damage

            // Add all damage types together, and apply final damage 
            finalDamageDealt = Mathf.RoundToInt(physicalDamage + magicDamage + fireDamage + lightningDamage + holyDamage);

            if (finalDamageDealt <= 0)
            {
                finalDamageDealt = 1;
            }

            Debug.Log("Damage dealt " + finalDamageDealt);
            character.characterNetworkManager.currentHealth.Value -= finalDamageDealt;

            // Calculate poise damage to determine if character will be stunned


        }

        private void PlayDamageVFX(CharacterManager character)
        {
            // If we have fire damage, play fire particles
            // Lightning damage, lightning particles

            character.characterEffectsManager.PlayBloodSplatterVFX(contactPoint);
        }

        private void PlayDamageSFX(CharacterManager character)
        {
            AudioClip physicalDamageSFX = WorldSoundFXManager.instance.ChooseRandomSFXFromArray(WorldSoundFXManager.instance.physicalDamageSFX);
            // Debug.Log(physicalDamageSFX);
            character.characterSoundFXManager.PlaySoundFX(physicalDamageSFX);
            character.characterSoundFXManager.PlayDamageGruntSoundFX();
            // If fire damage is greater than 0, play burn SFX
            // If lightning damage is greater than 0, play zap SFX etc
        }

        private void PlayDirectionalBasedDamageAnimation(CharacterManager character)
        {
            if (!character.IsOwner)
                return;

            if (character.isDead.Value)
                return;

            // TODO: Calculate if poise is broken
            poiseIsBroken = true;

            if (angleHitFrom >= 145 && angleHitFrom <= 180)
            {
                // Play front animation
                damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.forward_Medium_Damage);
            }
            else if (angleHitFrom <= -145 && angleHitFrom >= -180)
            {
                // Play front animation
                damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.forward_Medium_Damage);
            }
            else if (angleHitFrom >= -45 && angleHitFrom <= 45)
            {
                // Play back animation
                damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.backward_Medium_Damage);
            }
            else if (angleHitFrom >= -144 && angleHitFrom <= -45)
            {
                // Play left animation
                damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.left_Medium_Damage);
            }
            else if (angleHitFrom >= 45 && angleHitFrom <= 144)
            {
                // Play right animation
                damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.right_Medium_Damage);
            }

            // If poise is broken, play a staggering damage animation
            if (poiseIsBroken)
            {
                character.characterAnimatorManager.lastDamageAnimationPlayed = damageAnimation;
                character.characterAnimatorManager.PlayTargetActionAnimation(damageAnimation, true);
            }
        }
    }
}
