using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEditor.Build;

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

        [Header("Music")]
        [SerializeField] AudioClip bossIntroClip;
        [SerializeField] AudioClip bossBattleLoopClip;
        
        [Header("Status")]
        public NetworkVariable<bool> bossFightIsActive = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> hasBeenAwakened = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> hasBeenDefeated = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        [SerializeField] List<FogWallInteractable> fogWalls;
        [SerializeField] string sleepAnimation;
        [SerializeField] string awakenAnimation;

        [Header("Phase Shift")]
        public float minimumHealthPercentageToShift = 50;
        [SerializeField] string phaseShiftAnimation = "Phase_Change_01";
        [SerializeField] CombatStanceState phase02CombatStanceState;

        [Header("States")]
        [SerializeField] BossSleepState sleepState;

        protected override void Awake()
        {
            base.Awake();
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            bossFightIsActive.OnValueChanged += OnBossFightIsActiveChanged;
            OnBossFightIsActiveChanged(false, bossFightIsActive.Value); // So if you join when the fight is already active, you will get a hp bar

            if (IsOwner)
            {
                sleepState = Instantiate(sleepState);
                currentState = sleepState;
            }

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
                    hasBeenDefeated.Value = WorldSaveGameManager.instance.currentCharacterData.bossesDefeated[bossID];
                    hasBeenAwakened.Value = WorldSaveGameManager.instance.currentCharacterData.bossesAwakened[bossID];


                }

                // Locate fog walls
                StartCoroutine(GetFogWallsFromWorldObjectManager());

                // If awake, enable
                if (hasBeenAwakened.Value)
                {
                    for (int i = 0; i < fogWalls.Count; i++)
                    {
                        fogWalls[i].isActive.Value = true;
                    }
                }

                // If defeated, disable fog walls
                if (hasBeenDefeated.Value)
                {
                    for (int i = 0; i < fogWalls.Count; i++)
                    {
                        fogWalls[i].isActive.Value = false;
                    }
                    aiCharacterNetworkManager.isActive.Value = false;
                }
            }

            if (!hasBeenAwakened.Value)
            {
                characterAnimatorManager.PlayTargetActionAnimation(sleepAnimation, true);
            }
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();

            bossFightIsActive.OnValueChanged -= OnBossFightIsActiveChanged;
        }

        private IEnumerator GetFogWallsFromWorldObjectManager()
        {
            while (WorldObjectManager.instance.fogWalls.Count == 0)
                yield return new WaitForEndOfFrame();

            fogWalls = new List<FogWallInteractable>();

            foreach (var fogWall in WorldObjectManager.instance.fogWalls)
            {
                if (fogWall.fogWallID == bossID)
                    fogWalls.Add(fogWall);
            }
        }

        public override IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false)
        {
            PlayerUIManager.instance.playerUIPopUpManager.SendBossDefeatedPopUp("GREAT FOE FELLED");
            if (IsOwner)
            {
                characterNetworkManager.currentHealth.Value = 0;
                isDead.Value = true;
                bossFightIsActive.Value = false;

                foreach (var fogWall in fogWalls)
                {
                    fogWall.isActive.Value = false;
                }
                // Reset any flags here that need to be reset
                // Nothing yet (eg attack animations)

                // If not grounded, play aerial death animation

                if (!manuallySelectDeathAnimation)
                {
                    characterAnimatorManager.PlayTargetActionAnimation("Death_01", true);
                }

                hasBeenDefeated.Value = true;

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

        public void WakeBoss()
        {
            if (IsOwner)
            {
                if (!hasBeenAwakened.Value)
                {
                    characterAnimatorManager.PlayTargetActionAnimation(awakenAnimation, true);
                }

                bossFightIsActive.Value = true;
                hasBeenAwakened.Value = true;
                currentState = idle;

                if (!WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.ContainsKey(bossID))
                {
                    WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Add(bossID, true);
                }
                // Otherwise, load the data that already exists on this boss
                else
                {
                    WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Remove(bossID);
                    WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Add(bossID, true);
                }

                for (int i = 0; i < fogWalls.Count; i++)
                {
                    fogWalls[i].isActive.Value = true;
                }
            }
        }

        private void OnBossFightIsActiveChanged(bool oldStatus, bool newStatus)
        {
            WorldSoundFXManager.instance.PlayBossTrack(bossIntroClip, bossBattleLoopClip);

            if (bossFightIsActive.Value)
            {
                GameObject bossHealthBar = 
                    Instantiate(PlayerUIManager.instance.playerUIHudManager.bossHealthBarObject, PlayerUIManager.instance.playerUIHudManager.bossHealthBarParent);

                UI_Boss_HP_Bar bossHPBar = bossHealthBar.GetComponentInChildren<UI_Boss_HP_Bar>();
                bossHPBar.EnableBossHPBar(this);
            } 
            else
            {
                WorldSoundFXManager.instance.StopBossMusic();
            }
        }

        public void PhaseShift()
        {
            characterAnimatorManager.PlayTargetActionAnimation(phaseShiftAnimation, true);
            combatStance = Instantiate(phase02CombatStanceState);
            currentState = combatStance;
        }
    }
}
