using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YT
{
    [CreateAssetMenu(fileName = "NewCubeDialogueData", menuName = "Dialogue/Cube Dialogue Data")]
    public class CubeDialogueData : ScriptableObject
    {
        [TextArea(3, 10)]
        public string[] dialogueLines;
    }
}
