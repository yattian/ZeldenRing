using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YT
{
    public class AIState : ScriptableObject
    {
        public virtual AIState Tick(AICharacterManager aiCharacter)
        {

            // Do logic to find the player

            // If found player, return pursue target state instead

            // Otherwise continue to return to idle state
            return this;
        }

        protected virtual AIState SwitchState(AICharacterManager aiCharacter, AIState newState)
        {
            ResetStateFlags(aiCharacter);
            return newState;
        }

        protected virtual void ResetStateFlags(AICharacterManager aiCharacter)
        {
            // Reset any state flags here so when you return to the state, they are blank once again
        }
    }
}
