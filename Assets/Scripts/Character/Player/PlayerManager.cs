using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YT
{
    public class PlayerManager : CharacterManager
    {
        PlayerLocomotionManager playerLocomotionManager;
        protected override void Awake()
        {
            base.Awake();

            // DO MORE STUFF FOR PLAYER

            playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        }

        protected override void Update()
        {
            base.Update();

            if (!IsOwner)
                return;
            // HANDLE MOVEMENT
            playerLocomotionManager.HandleAllMovement();
        }
    }
}
