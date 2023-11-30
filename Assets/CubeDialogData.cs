using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace YT
{
    [CreateAssetMenu(fileName = "NewCubeDialogueData", menuName = "Dialogue/Cube Dialogue Data")]
    public class CubeDialogueData : ScriptableObject
    {
        [TextArea(3, 10)]
        public string[] dialogueLines;
        public Sprite dialogueImage;
    }
}
