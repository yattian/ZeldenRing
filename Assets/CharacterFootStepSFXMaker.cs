using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YT
{
    public class CharacterFootStepSFXMaker : MonoBehaviour
    {
        CharacterManager character;

        AudioSource audioSource;
        GameObject steppedOnObject;

        private bool hasTouchedGround = false;
        private bool hasPlayedFootStepSFX = false;
        [SerializeField] float distanceToGround = 10f;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            character = GetComponentInParent<CharacterManager>();
        }

        private void FixedUpdate()
        {
            CheckForFootSteps();
        }

        private void CheckForFootSteps()
        {
            if (character == null)
                return;

            if (!character.characterNetworkManager.isMoving.Value)
                return;

            RaycastHit hit;

            if (Physics.Raycast(transform.position, character.transform.TransformDirection(Vector3.down), out hit, distanceToGround, WorldUtilityManager.Instance.GetEnviroLayers()))
            {
                hasTouchedGround = true;

                if (!hasPlayedFootStepSFX)
                    steppedOnObject = hit.transform.gameObject;
            }
            else
            {
                hasTouchedGround = false;
                hasPlayedFootStepSFX = false;
                steppedOnObject = null;
            }

            if (hasTouchedGround && !hasPlayedFootStepSFX)
            {
                hasPlayedFootStepSFX = true;
                PlayFootStepSoundFX();
            }
        }

        private void PlayFootStepSoundFX()
        {
            // Play different sound FX depending on layer of ground or a tag such as snow, wood, stone etc
            // Method 1
            //audioSource.PlayOneShot(WorldSoundFXManager.instance.ChooseRandomFootStepSoundBasedOnGround(steppedOnObject, character));

            // Method 2 (simple)
            character.characterSoundFXManager.PlayFootStepSoundFX();
        }
    }
}
