using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YT
{
    public class PlayerStatsManager : CharacterStatsManager
    {
        PlayerManager player;
        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
            
        }

        protected override void Start()
        {
            base.Start();

            // Why calculate these here?
            // Because when create new character menu, set stats depending on the class, calculated there
            // Until then however, stats are never calculated so we have to calculate it somewwhere, if a save file exists, they will be overwritten when loading into scene
            CalculateHealthBasedOnVitalityLevel(player.playerNetworkManager.vitality.Value);
            CalculateStaminaBasedOnEnduranceLevel(player.playerNetworkManager.endurance.Value);
        }

    }
}
