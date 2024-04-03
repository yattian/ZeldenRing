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
        private static int defaultLayerIndex = 0;
        LayerMask layerMask = (1 << defaultLayerIndex) | (1 << 1);

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            character = GetComponentInParent<CharacterManager>();
        }

        private void Start()
        {
            Debug.Log("Layer mask value: " + WorldUtilityManager.Instance.GetEnviroLayers());
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
            Vector3 direction = character.transform.TransformDirection(Vector3.down).normalized;
            Debug.DrawRay(transform.position, direction * distanceToGround, Color.red, 2f);
            if (Physics.Raycast(transform.position, character.transform.TransformDirection(Vector3.down), out hit, distanceToGround, layerMask))
            {
                Debug.Log("hasTouchedGround");
                hasTouchedGround = true;

                if (!hasPlayedFootStepSFX)
                    steppedOnObject = hit.transform.gameObject;
            }
            else
            {
                Debug.Log("hasNotTouchedGround");
                hasTouchedGround = false;
                hasPlayedFootStepSFX = false;
                steppedOnObject = null;
            }

            if (hasTouchedGround && !hasPlayedFootStepSFX)
            {
                Debug.Log("Boom");
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
