using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YT { 
    public class CharacterEffectsManager : MonoBehaviour
    {
        // Process instant effects (damage, heal etc)

        // Process timed effects (poison, build ups)

        // Process static effects (adding/removing buffs from talismans etc)

        CharacterManager character;

        [Header("VFX")]
        [SerializeField] GameObject bloodSplatterVFX;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        public virtual void ProcessInstantEffect(InstantCharacterEffect effect)
        {
            // Take in an effect + process it
            effect.ProcessEffect(character);
        }

        public void PlayBloodSplatterVFX(Vector3 contactPoint)
        {
            // If we manually have placeed a blood splatter VFX on this model, play its version
            if (bloodSplatterVFX != null)
            {
                GameObject bloodSplatter = Instantiate(bloodSplatterVFX, contactPoint, Quaternion.identity);
            }
            // else, we play the generic version we have elsewhere
            else
            {
                GameObject bloodSplatter = Instantiate(WorldCharacterEffectsManager.instance.bloodSplatterVFX, contactPoint, Quaternion.identity);
            }
        }
    }
}
