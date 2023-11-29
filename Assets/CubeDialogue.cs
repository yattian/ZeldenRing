using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace YT
{
    public class CubeDialogue : MonoBehaviour, IInteractable
    {
        // This function gets called when the cube is interacted with
        public void Interact()
        {
            // Start the dialogue
            PlayerUIManager.instance.playerUIDialogueManager.StartDialogue();
        }
    }

}
