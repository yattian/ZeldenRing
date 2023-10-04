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

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        public virtual void ProcessInstantEffect(InstantCharacterEffect effect)
        {
            // Take in an effect + process it
            effect.ProcessEffect(character);
        }
    }
}
