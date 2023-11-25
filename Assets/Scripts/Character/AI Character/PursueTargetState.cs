using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace YT
{
    [CreateAssetMenu(menuName = "A.I/States/Pursue Target")]
    public class PursueTargetState : AIState
    {
        public override AIState Tick(AICharacterManager aiCharacter)
        {
            // Check if we are performing an action (if so do nothing until action is complete)
            if (aiCharacter.isPerformingAction)
                return this;

            // Check if our target is null, if we do not have a target, return to idle state
            if (aiCharacter.aiCharacterCombatManager.currentTarget == null)
                return SwitchState(aiCharacter, aiCharacter.idle);

            // Make sure our navmesh agent is active, if its not enable it
            if (!aiCharacter.navMeshAgent.enabled)
                aiCharacter.navMeshAgent.enabled = true;

            aiCharacter.aiCharacterLocmotionManager.RotateTowardsAgent(aiCharacter);

            // If we are within combat range of a target, switch state to combat stance state

            // If the target is not reachable, and they are far away, return home

            // Pursue target

            // Another option if bottom one doesn't work
            //aiCharacter.navMeshAgent.SetDestination(aiCharacter.aiCharacterCombatManager.currentTarget.transform.position);

            NavMeshPath path = new NavMeshPath();
            aiCharacter.navMeshAgent.CalculatePath(aiCharacter.aiCharacterCombatManager.currentTarget.transform.position, path);
            aiCharacter.navMeshAgent.SetPath(path);

            return this;
        }
    }
}
