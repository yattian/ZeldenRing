using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace YT
{
    public class CubeDialogue : MonoBehaviour, IInteractable
    {
        [SerializeField] private CubeDialogueData cubeDialogueData; // Reference to CubeDialogueData for this cube

        public void Interact()
        {
            if (cubeDialogueData != null)
            {
                PlayerUIManager.instance.playerUIDialogueManager.StartDialogue(cubeDialogueData.dialogueLines);
            }
            else
            {
                Debug.LogWarning("CubeDialogueData not assigned to cube: " + gameObject.name);
            }
        }
    }

}
