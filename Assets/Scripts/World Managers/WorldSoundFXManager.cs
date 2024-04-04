using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YT
{
    
    public class WorldSoundFXManager : MonoBehaviour
    {
        public static WorldSoundFXManager instance;

        [Header("Boss Track")]
        [SerializeField] AudioSource bossIntroPlayer;
        [SerializeField] AudioSource bossLoopPlayer;

        [Header("Background Music")]
        public AudioClip enterWorldSFX;
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

        public void PlayBossTrack(AudioClip introClip, AudioClip loopTrack)
        {
            bossIntroPlayer.volume = 1;
            bossIntroPlayer.clip = introClip;
            bossIntroPlayer.loop = false;
            bossIntroPlayer.Play();

            bossLoopPlayer.volume = 1;
            bossLoopPlayer.clip = loopTrack;
            bossLoopPlayer.loop = true;
            bossLoopPlayer.PlayDelayed(bossIntroPlayer.clip.length);
        }

        public AudioClip ChooseRandomSFXFromArray(AudioClip[] array)
        {
            int index = Random.Range(0, array.Length);

            return array[index];
        }

        /*
        public AudioClip ChooseRandomFootStepSoundBasedOnGround(GameObject steppedOnObject, CharacterManager character)
        {
            if (steppedOnObject.tag == "Untagged")
            {
                return ChooseRandomSFXFromArray(character.characterSoundFXManager.footStepsDirt);
            }
            else if (steppedOnObject.tag == "Stone")
            {
                return ChooseRandomSFXFromArray(character.characterSoundFXManager.footStepsDirt);
            }

            return null;
        }
        */

        public void StopBossMusic()
        {
            StartCoroutine(FadeOutBossMusicThenStop());
        }

        private IEnumerator FadeOutBossMusicThenStop()
        {
            bossIntroPlayer.Stop();

            while (bossLoopPlayer.volume > 0)
            {
                bossLoopPlayer.volume -= Time.deltaTime;
                bossIntroPlayer.volume -= Time.deltaTime;

                yield return null;
            }

            bossIntroPlayer.Stop();
            bossLoopPlayer.Stop();
        }
    }
}