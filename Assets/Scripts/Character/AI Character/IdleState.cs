using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YT
{
    [CreateAssetMenu(menuName = "A.I/States/Idle")]
    public class IdleState : AIState
    {
        public override AIState Tick(AICharacterManager aiCharacter)
        {
            if (aiCharacter.characterCombatManager.currentTarget != null)
            {
                Debug.Log("We have a target!");

                return this;
            }
            else
            {
                // Return this state, to continually search for a target
                aiCharacter.aiCharacterCombatManager.FindATargetViaLineOFSight(aiCharacter);
                Debug.Log("Searching for a target.");
                return this;
            }
        }


    }
}
