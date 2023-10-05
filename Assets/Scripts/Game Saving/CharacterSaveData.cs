using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YT
{
    [System.Serializable]
    // SINCE WE WANT TO REFERENCE THIS DATA FOR EVERY SAVE FILE, THIS SCRIPT IS NOT A MONOBEHAVIOUR AND IS INSTEAD SERIALIZABLE
    public class CharacterSaveData
    {
        // Think this was about episode 14? +-1 
        //[Header("SCENE INDEX")]
        //public int sceneIndex = 1;

        [Header("Character Name")]
        public string characterName = "Character";

        [Header("Time Played")]
        public float secondsPlayed;

        // Can only save basic variable types, so not vector 3
        [Header("World Coordinates")]
        public float xPosition;
        public float yPosition;
        public float zPosition;

        [Header("Resources")]
        public float currentHealth;
        public float currentStamina;
        

        [Header("Stats")]
        public int vitality;
        public int endurance;
    }
}
