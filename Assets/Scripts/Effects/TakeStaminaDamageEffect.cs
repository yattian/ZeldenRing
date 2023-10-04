using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YT
{
    [CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Stamina Damage")]
    public class TakeStaminaDamageEffect : InstantCharacterEffect
    {
        public float staminaDamage;
        public override void ProcessEffect(CharacterManager character)
        {
            CalculateStaminaDamage(character);
        }

        private void CalculateStaminaDamage(CharacterManager character)
        {
            // Compared the base stamina damage against other player effects/modifiers
            // Change the value before subtracting/adding it
            // Play sound fx or vfx during effect

            if (character.IsOwner)
            {
                Debug.Log("Character is taking: " + staminaDamage + " stamina damage");
                character.characterNetworkManager.currentStamina.Value -= staminaDamage;
            }
        }
    }
}
