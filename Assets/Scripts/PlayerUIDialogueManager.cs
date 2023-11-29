using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace YT
{
    public class PlayerUIDialogueManager : MonoBehaviour
    {
        [SerializeField] GameObject dialoguePopUpGameObject;
        [SerializeField] TextMeshProUGUI dialoguePopUpText;
        private string[] lines = { "First dialogue", "Second dialogue" };
        public float textSpeed;

        private int index;

        // Update is called once per frame
        public void SkipDialogue()
        {

            if (dialoguePopUpText.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                dialoguePopUpText.text = lines[index];
            }

        }

        public void StartDialogue()
        {
            PlayerInputManager.instance.isInDialogue = true;
            dialoguePopUpGameObject.SetActive(true);
            Debug.Log(Random.Range(0, 100));
            dialoguePopUpText.text = string.Empty;
            index = 0;
            StartCoroutine(TypeLine());
        }

        IEnumerator TypeLine()
        {
            foreach (char c in lines[index].ToCharArray())
            {
                dialoguePopUpText.text += c;
                yield return new WaitForSeconds(textSpeed);
            }
        }

        void NextLine()
        {
            if (index < lines.Length - 1)
            {
                index++;
                dialoguePopUpText.text = string.Empty;
                StartCoroutine(TypeLine());
            }
            else
            {
                dialoguePopUpGameObject.SetActive(false);
                PlayerInputManager.instance.isInDialogue = false;
            }
        }
    }
}
