using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YT
{
    public class PlayerInteractionManager : MonoBehaviour
    {
        PlayerManager player;

        private List<Interactable> currentInteractableActions; // Do not serialise if using unity V 2022.3.11f1, cause bug

        private void Awake()
        {
            player = GetComponent<PlayerManager>();
        }

        private void Start()
        {
            currentInteractableActions = new List<Interactable>();
        }

        private void FixedUpdate()
        {
            if (!player.IsOwner)
                return;

            // If our UI menu is not open, and we don't have a pop up (current interaction)
            if (!PlayerUIManager.instance.menuWindowIsOpen && !PlayerUIManager.instance.popUpWindowIsOpen)
                CheckForInteractable();
        }

        private void CheckForInteractable()
        {
            if (currentInteractableActions.Count == 0)
                return;

            if (currentInteractableActions[0] == null)
            {
                currentInteractableActions.RemoveAt(0); // If the current interactable item at position 0 becomes null, remove position (if someone picks up other than you)
                return;
            }

            // If we have an interactable item at position 0 and have not notified our player, we do so here
            if (currentInteractableActions[0] != null)
                PlayerUIManager.instance.playerUIPopUpManager.SendPlayerMessagePopUp(currentInteractableActions[0].interactableText);
        }

        private void RefreshInteractionList()
        {
            for (int i = currentInteractableActions.Count - 1; i > -1; i--)
            {
                if (currentInteractableActions[i] == null)
                    currentInteractableActions.RemoveAt(i);
            }
        }

        public void AddInteractionToList(Interactable interactableObject)
        {
            RefreshInteractionList();

            if (!currentInteractableActions.Contains(interactableObject))
                currentInteractableActions.Add(interactableObject);
        }

        public void RemoveInteractionFromList(Interactable interactableObject)
        {
            if (currentInteractableActions.Contains(interactableObject))
                currentInteractableActions.Remove(interactableObject);

            RefreshInteractionList();
        }

        public void Interact()
        {
            if (currentInteractableActions.Count == 0)
                return;

            if (currentInteractableActions[0] != null)
            {
                currentInteractableActions[0].Interact(player);
                RefreshInteractionList();
            }
        }
    }
}
