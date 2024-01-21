using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace YT
{
    public class AIBossCharacterManager : AICharacterManager
    {
        // Give AI unique ID
        // When spawned, check save file... (dictionary)
        // If save file does not contain boss monster with this ID, add it
        // If present, check if defeated
        // If defeated, disable game object
        // If not, allow object to continue active

        public int bossID = 0;
        [SerializeField] bool hasBeenDefeated = false;

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            // If this is the hosts world
            if (IsServer)
            {
                // If our save game data does not contain information, then add it now
                if (!WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.ContainsKey(bossID))
                {
                    WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Add(bossID, false);
                    WorldSaveGameManager.instance.currentCharacterData.bossesDefeated.Add(bossID, false);
                }
                // Otherwise, load the data that already exists on this boss
                else
                {
                    hasBeenDefeated = WorldSaveGameManager.instance.currentCharacterData.bossesDefeated[bossID];

                    if (hasBeenDefeated)
                    {
                        aiCharacterNetworkManager.isActive.Value = false;
                    }
                }
            }
        }

        public override IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false)
        {
            if (IsOwner)
            {
                characterNetworkManager.currentHealth.Value = 0;
                isDead.Value = true;

                // Reset any flags here that need to be reset
                // Nothing yet (eg attack animations)

                // If not grounded, play aerial death animation

                if (!manuallySelectDeathAnimation)
                {
                    characterAnimatorManager.PlayTargetActionAnimation("Death_01", true);
                }

                hasBeenDefeated = true;

                // If our save game data does not contain information, then add it now
                if (!WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.ContainsKey(bossID))
                {
                    WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Add(bossID, true);
                    WorldSaveGameManager.instance.currentCharacterData.bossesDefeated.Add(bossID, true);
                }
                // Otherwise, load the data that already exists on this boss
                else
                {
                    WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Remove(bossID);
                    WorldSaveGameManager.instance.currentCharacterData.bossesDefeated.Remove(bossID);
                    WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Add(bossID, true);
                    WorldSaveGameManager.instance.currentCharacterData.bossesDefeated.Add(bossID, true);
                }

                WorldSaveGameManager.instance.SaveGame();
            }

            // Play some death SFX

            yield return new WaitForSeconds(5);

            // Award players with runes 

            // Disable character
        }
    }
}
