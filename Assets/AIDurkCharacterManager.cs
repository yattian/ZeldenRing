using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YT
{
    public class AIDurkCharacterManager : AIBossCharacterManager
    {
        // Why give Durk own character manager?

        // Character manager acts as a hub to where we can reference all components of a character

        // A player manager for example will have all the unique componenets of a player character

        [HideInInspector] public AIDurkSoundFXManager durkSoundFXManager;
        [HideInInspector] public AIDurkCombatManager durkCombatManager;

        protected override void Awake()
        {
            base.Awake();
            durkSoundFXManager = GetComponent<AIDurkSoundFXManager>();
            durkCombatManager = GetComponent<AIDurkCombatManager>();
        }
    }
}
