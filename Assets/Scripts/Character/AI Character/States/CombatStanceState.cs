using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace YT
{
    [CreateAssetMenu(menuName = "A.I/States/Combat Stance")]
    public class CombatStanceState : AIState
    {
        // 1. Select an attack for the attack state, depending on distance and angle of target in relation to character
        // 2. Process any combat logic here whilst waiting to attack (blocking, strafing, dodging etc)
        // 3. If Target moves out of combat range, switch to pursue target state
        // 4. If target is no longer present, switch to idle state

        [Header("Attacks")]
        public List<AICharacterAttackAction> aiCharacterAttacks; // A list of all possible attacks this character can do
        protected List<AICharacterAttackAction> potentialAttacks; // A list that is created during the state, all attacks possible in this situation (based on angle, distance etc)
        private AICharacterAttackAction chosenAttack;
        private AICharacterAttackAction previousAttack;
        private bool hasAttack = false;

        [Header("Combo")]
        [SerializeField] protected bool canPerformCombo = false; // If the character can perform a combo attack, after the initial attack
        [SerializeField] protected int chanceToPerformCombo = 25; // The chance (in percent) of the character to perform a combo attack
        protected bool hasRolledForComboChance = false; // If we already have rolled for the change during this state

        [Header("Engagement Distance")]
        [SerializeField] protected float maximumEngagementDistance = 5; // The distance we have to be away from target before we enter pursue target state

        public override AIState Tick(AICharacterManager aiCharacter)
        {
            if (aiCharacter.isPerformingAction)
                return this;

            if (!aiCharacter.navMeshAgent.enabled)
                aiCharacter.navMeshAgent.enabled = true;

            // If you want AI character to face and turn towards its target when its outside it's FOV include this
            if (!aiCharacter.aiCharacterNetworkManager.isMoving.Value)
            {
                if (aiCharacter.aiCharacterCombatManager.viewableAngle < -30 || aiCharacter.aiCharacterCombatManager.viewableAngle > 30)
                    aiCharacter.aiCharacterCombatManager.PivotTowardsTarget(aiCharacter);
            }

            // Rotate to face our target

            // If target is no longer present, switch back to idle
            if (aiCharacter.aiCharacterCombatManager.currentTarget == null)
                return SwitchState(aiCharacter, aiCharacter.idle);

            // If we do not have an attack, get one
            if (!hasAttack)
            {
                GetNewAttack(aiCharacter);
            }
            else
            {
                // Check recovery timer
                // Pass attack to attack state
                // Roll for combo chance
                // Switch state 
            }

            // If we are outside combat engagement distance, switch to pursue target state
            if (aiCharacter.aiCharacterCombatManager.distanceFromTarget > maximumEngagementDistance)
                return SwitchState(aiCharacter, aiCharacter.pursueTarget);

            NavMeshPath path = new NavMeshPath();
            aiCharacter.navMeshAgent.CalculatePath(aiCharacter.aiCharacterCombatManager.currentTarget.transform.position, path);
            aiCharacter.navMeshAgent.SetPath(path);

            return this;
        }

        protected virtual void GetNewAttack(AICharacterManager aiCharacter)
        {
            potentialAttacks = new List<AICharacterAttackAction>();
            
            foreach(var potentialAttack in potentialAttacks)
            {
                // If we are too close for this attack, move to next attack
                if (potentialAttack.minimumAttackAngle > aiCharacter.aiCharacterCombatManager.distanceFromTarget)
                    continue;

                // If we are too far for this attack, move to next attack
                if (potentialAttack.maximumAttackAngle < aiCharacter.aiCharacterCombatManager.distanceFromTarget)
                    continue;

                // If target is outside minimum field for this attack, move to next attack
                if (potentialAttack.minimumAttackAngle > aiCharacter.aiCharacterCombatManager.viewableAngle)
                    continue;

                // If target is outside maximum field for this attack, move to next attack
                if (potentialAttack.maximumAttackAngle < aiCharacter.aiCharacterCombatManager.viewableAngle)
                    continue;

                potentialAttacks.Add(potentialAttack);
            }

            if (potentialAttacks.Count <= 0)
                Debug.Log("Missing potential attacks");
                return;

            var totalWeight = 0;

            foreach (var attack in potentialAttacks)
            {
                totalWeight += attack.attackWeight;
            }

            var randomWeightValue = Random.Range(1, totalWeight + 1);
            var processedWeight = 0;

            foreach (var attack in potentialAttacks)
            {
                processedWeight += attack.attackWeight;

                if (randomWeightValue <= processedWeight)
                {
                    chosenAttack = attack;
                    previousAttack = chosenAttack;
                    hasAttack = true;
                }
            }

            // 1. Sort through all possible attacks
            // 2. Remove attacks that cant be used in this situation (Based on angle and distance)
            // 3. Place remaining attacks into a list
            // 4. Pick one of the reminaing attacks randomly, based on weight
            // 5. Select this attack and pass to attack site
        }

        protected virtual bool RollForOutcomeChance(int outcomeChance)
        {
            bool outcomeWillBePerformed = false;

            int randomPercentage = Random.Range(0, 101);

            if (randomPercentage < outcomeChance)
                outcomeWillBePerformed = true;

            return outcomeWillBePerformed;
        }

        protected override void ResetStateFlags(AICharacterManager aiCharacter)
        {
            base.ResetStateFlags(aiCharacter);

            hasAttack = false;
            hasRolledForComboChance = false;
        }

    }
}
