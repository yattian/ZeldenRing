using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YT
{
    public class DurksStompCollider : DamageCollider
    {
        [SerializeField] AIDurkCharacterManager durkCharacterManager;

        protected override void Awake()
        {
            base.Awake();

            durkCharacterManager = GetComponentInParent<AIDurkCharacterManager>();
        }

        public void StompAttack()
        {
            GameObject stompVFX = Instantiate(durkCharacterManager.durkCombatManager.durkImpactVFX, transform);

            Collider[] colliders = Physics.OverlapSphere(transform.position, durkCharacterManager.durkCombatManager.stompAttackAOERadius, WorldUtilityManager.Instance.GetCharacterLayers());
            List<CharacterManager> charactersDamaged = new List<CharacterManager>();

            foreach (var collider in colliders)
            {
                CharacterManager character = collider.GetComponentInParent<CharacterManager>();

                if (character != null)
                {
                    if (charactersDamaged.Contains(character))
                        continue;

                    // We don't want Durk to hurt himself when he stomps
                    if (character == durkCharacterManager)
                        continue;

                    charactersDamaged.Add(character);

                    // Only process damage if the character is owner so that they only get damaged if the collider connects on their client
                    if (character.IsOwner)
                    {
                        // Check for block
                        TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeDamageEffect);
                        damageEffect.physicalDamage = durkCharacterManager.durkCombatManager.stompDamage;
                        damageEffect.poiseDamage = durkCharacterManager.durkCombatManager.stompDamage;

                        character.characterEffectsManager.ProcessInstantEffect(damageEffect);
                    }
                }
            }
        }
    }
}
