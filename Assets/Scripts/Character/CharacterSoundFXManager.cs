using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YT
{
    public class CharacterSoundFXManager : MonoBehaviour
    {
        private AudioSource audioSource;

        protected virtual void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.PlayOneShot(WorldSoundFXManager.instance.backgroundSFX);
        }

        public void PlaySoundFX(AudioClip soundFX, float volume = 1, bool randomizePitch = true, float pitchRandom = 0.1f)
        {
            audioSource.PlayOneShot(soundFX, volume);
            // Resets our pitch
            audioSource.pitch = 1;

            if (randomizePitch)
            {
                audioSource.pitch += Random.Range(-pitchRandom, pitchRandom);
            }
        }

        public void PlayRollSoundFX()
        {
            audioSource.PlayOneShot(WorldSoundFXManager.instance.rollSFX);
        }

        public void PlayWhooshSoundFX()
        {
            AudioClip whooshSFX = WorldSoundFXManager.instance.ChooseRandomSFXFromArray(WorldSoundFXManager.instance.whooshSFX);
            audioSource.PlayOneShot(whooshSFX);
        }
    }
}