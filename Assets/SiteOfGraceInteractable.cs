using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;


namespace YT
{
    public class SiteOfGraceInteractable : Interactable
    {
        [Header("Site Of Grace Info")]
        [SerializeField] int siteOfGraceID;
        public NetworkVariable<bool> isActivated = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [Header("VFX")]
        [SerializeField] GameObject activatedParticles;
        [SerializeField] GameObject unactivatedParticles;

        [Header("Interaction Text")]
        [SerializeField] string unactivatedInteractionText = "Restore Site Of Grace";
        [SerializeField] string activatedInteractionText = "Rest";

        protected override void Start()
        {
            base.Start();

            if (IsOwner)
            {
                if (WorldSaveGameManager.instance.currentCharacterData.sitesOfGrace.ContainsKey(siteOfGraceID))
                {
                    isActivated.Value = WorldSaveGameManager.instance.currentCharacterData.sitesOfGrace[siteOfGraceID];
                }
                else
                {
                    isActivated.Value = false;
                }
            }

            if (isActivated.Value)
            {
                interactableText = activatedInteractionText;
            }
            else
            {
                interactableText = unactivatedInteractionText;
            }
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            // When joinin as client when status already changed, force the onchange function to run here upon joining
            if (!IsOwner)
                OnIsActivatedChanged(false, isActivated.Value);

            isActivated.OnValueChanged += OnIsActivatedChanged;
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();

            isActivated.OnValueChanged -= OnIsActivatedChanged;
        }



        private void RestoreSiteOfGrace(PlayerManager player)
        {
            // Adds site of grace to activated sites in save files
            isActivated.Value = true;

            // If save info contains info on this site, remove it
            if (WorldSaveGameManager.instance.currentCharacterData.sitesOfGrace.ContainsKey(siteOfGraceID) ) 
                WorldSaveGameManager.instance.currentCharacterData.sitesOfGrace.Remove(siteOfGraceID);

            // Then re-add it with true (is activated)
            WorldSaveGameManager.instance.currentCharacterData.sitesOfGrace.Add(siteOfGraceID, true);

            // Play animation
            player.playerAnimatorManager.PlayTargetActionAnimation("Activate_Site_Of_Grace_01", true);

            // Send pop up
            PlayerUIManager.instance.playerUIPopUpManager.SendGraceRestoredPopUp("SITE OF GRACE RESTORED");

            // Enable/Activates this site of Grace
            StartCoroutine(WaitForAnimationAndPopUpThenRestoreCollider());
        }

        private void RestAtSiteOfGrace(PlayerManager player) 
        {
            Debug.Log("Resting");

            // Temporary code section
            interactableCollider.enabled = true; // Temporarily Re-enable the collider here so we can respawn monsters
            player.playerNetworkManager.currentHealth.Value = player.playerNetworkManager.maxHealth.Value;
            player.playerNetworkManager.currentStamina.Value = player.playerNetworkManager.maxStamina.Value;

            // Refill flasks (TODO)

            // Reset monsters/charaacters
            WorldAIManager.instance.ResetAllCharacters();

            // Update/force move quest characters (TODO)
        }

        private IEnumerator WaitForAnimationAndPopUpThenRestoreCollider()
        {
            yield return new WaitForSeconds(3); // Give time for animation to play and the pop up to begin fading
            interactableCollider.enabled = true;
        }

        public override void Interact(PlayerManager player)
        {
            base.Interact(player);

            if (!isActivated.Value)
            {
                RestoreSiteOfGrace(player);
            }
            else
            {
                // Change to rest after
                RestAtSiteOfGrace(player);
            }
        }

        private void OnIsActivatedChanged(bool oldStatus, bool newStatus)
        {
            if (isActivated.Value)
            {
                // Play some FX here if you'd like to enable a light or something to indicate this check point is on
                activatedParticles.SetActive(true);
                unactivatedParticles.SetActive(false);
                interactableText = activatedInteractionText;
            }
            else
            {
                interactableText = unactivatedInteractionText;
            }
        }
    }
}
