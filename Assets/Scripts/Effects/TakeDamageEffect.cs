using System.Collections;
using System.Collections.Generic;
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
            base.ProcessEffect(character);

            // If dead, no damage effects
            if (character.isDead.Value)
                return;

            // Check for vulnerability

            CalculateDamage(character);
            // Calculate damage
            // Check which direction damage came from
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

            character.characterSoundFXManager.PlaySoundFX(physicalDamageSFX);
            // If fire damage is greater than 0, play burn SFX
            // If lightning damage is greater than 0, play zap SFX etc


        }
    }
}
