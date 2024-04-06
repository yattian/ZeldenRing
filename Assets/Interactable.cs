using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace YT
{
    public class Interactable : MonoBehaviour
    {
        public string interactableText; // Text prompt when entering the interactable (pick up item, pull lever etc)
        [SerializeField] protected Collider interactableCollider; // Collider that checks for player interaction
        [SerializeField] protected bool hostOnlyInteractable = true; // When enabled, object cannot be interacted by co-op players

        protected virtual void Awake()
        {
            // Check if its null, in some cases you may want to manually asign a collider as a child object (depending on interactable)
            if (interactableCollider == null)
                interactableCollider = GetComponent<Collider>();
        }

        protected virtual void Start()
        {

        }

        public virtual void Interact(PlayerManager player)
        {
            Debug.Log("You have interacted!");

            if (!player.IsOwner)
                return;

            interactableCollider.enabled = false;
            player.playerInteractionManager.RemoveInteractionFromList(this);
            PlayerUIManager.instance.playerUIPopUpManager.CloseAllPopUpWindows();


        }

        public virtual void OnTriggerEnter(Collider other)
        {
            PlayerManager player = other.GetComponent<PlayerManager>(); 

            if (player != null)
            {
                if (!player.playerNetworkManager.IsHost && hostOnlyInteractable)
                    return;

                if (!player.IsOwner)
                    return;

                // Pass the interaction to the player
                player.playerInteractionManager.AddInteractionToList(this);

            }
        }

        public virtual void OnTriggerExit(Collider other)
        {
            PlayerManager player = other.GetComponent<PlayerManager>();

            if (player != null)
            {
                if (!player.playerNetworkManager.IsHost && hostOnlyInteractable)
                    return;

                if (!player.IsOwner)
                    return;

                // Remove the interaction from the player
                player.playerInteractionManager.RemoveInteractionFromList(this);
                PlayerUIManager.instance.playerUIPopUpManager.CloseAllPopUpWindows();
            }
        }
    }
}
