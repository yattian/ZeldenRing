using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YT
{
    
    public class WorldSoundFXManager : MonoBehaviour
    {
        public static WorldSoundFXManager instance;

        //[Header("Background Music")]
        //public AudioClip backgroundSFX;
        //private AudioSource audioSource;

        [Header("Whoosh Sounds")]
        public AudioClip[] whooshSFX;

        [Header("Damage Sounds")]
        public AudioClip[] physicalDamageSFX;

        [Header("Action Sounds")]
        public AudioClip rollSFX;

        [Header("UI Sounds")]
        public AudioClip mainMenuUIClick;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }   
            else
            {
                Destroy(gameObject);
            }

            //audioSource = GetComponent<AudioSource>();
            //audioSource.clip = backgroundSFX;
            //audioSource.loop = true;

            //audioSource.Play();
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        public AudioClip ChooseRandomSFXFromArray(AudioClip[] array)
        {
            int index = Random.Range(0, array.Length);

            return array[index];
        }
    }
}