using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is attempt 1
// PlayerInteractionManager is attempt 2

namespace YT
{
    interface IInteractable
    {
        public void Old_Interact();
    }

    public class PlayerInteractorManager : CharacterInteractManager
    {
        PlayerManager player;

        public Transform[] raycastOrigins; // Assign the desired transforms in the Inspector
        public float interactRange = 2f;

        private bool hasInteracted = false; // Flag to track if interaction has occurred
        public LayerMask interactionLayer; // Assign the InteractionLayer in the Inspector

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
        }

        protected override void Start()
        {
            base.Start();
        }

        public void PerformInteract()
        {
            if (raycastOrigins == null || raycastOrigins.Length == 0)
            {
                Debug.LogError("Raycast origins are not assigned.");
                return;
            }

            // Reset the flag before performing interactions
            hasInteracted = false;

            foreach (Transform origin in raycastOrigins)
            {
                Ray ray = new Ray(origin.position, origin.forward);

                if (Physics.Raycast(ray, out RaycastHit hitInfo, interactRange, interactionLayer))
                {
                    if (!hasInteracted && hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
                    {
                        interactObj.Old_Interact();
                        hasInteracted = true; // Set the flag to indicate interaction
                    }
                }

                // Draw the ray in the Scene view for visualization
                Debug.DrawRay(origin.position, origin.forward * interactRange, Color.red, 1.0f);
            }
        }
    }
}
