using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YT
{
    public class PlayerCombatManager : CharacterCombatManager
    {
        PlayerManager player;

        public WeaponItem currentWeaponBeingUsed;

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
        }

        public void PerformWeaponBasedAction(WeaponItemAction weaponAction, WeaponItem weaponPerformingAction)
        {
            // Perform the action
            weaponAction.AttemptToPerformAction(player, weaponPerformingAction);

            // Also perform the action on other clients (notify the server we have performed the action)


        }
    }
}
