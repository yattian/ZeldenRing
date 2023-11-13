using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

namespace YT
{
    public class WorldAIManager : MonoBehaviour
    {
        public static WorldAIManager instance;

        [Header("DEBUG")]
        [SerializeField] bool despawnCharacters = false;
        [SerializeField] bool respawnCharacters = false;

        [Header("Characters")]
        [SerializeField] GameObject[] aiCharacters;
        [SerializeField] List<GameObject> spawnedInCharacters;

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
        }

        private void Start()
        {
            if (NetworkManager.Singleton.IsServer)
            {
                StartCoroutine(WaitForSceneToLoadThenSpawnCharacters());
            }
        }

        private void Update()
        {
            if (respawnCharacters)
            {
                respawnCharacters = false;
                SpawnAllCharacters();
            }

            if (despawnCharacters)
            {
                despawnCharacters = false;
                DespawnAllCharacters();
            }
        }

        private IEnumerator WaitForSceneToLoadThenSpawnCharacters()
        {
            while (!SceneManager.GetActiveScene().isLoaded)
            {
                yield return null;
            }

            SpawnAllCharacters();
        }

        private void SpawnAllCharacters()
        {
            foreach (var character in aiCharacters)
            {
                GameObject instantiatedCharacter = Instantiate(character);
                instantiatedCharacter.GetComponent<NetworkObject>().Spawn();
                spawnedInCharacters.Add(instantiatedCharacter);
            }
        }

        private void DespawnAllCharacters()
        {
            foreach (var character in spawnedInCharacters)
            {
                character.GetComponent<NetworkObject>().Despawn();
            }
        } 

        private void DisableAllCharacters()
        {
            // To do, disable character gameObjects, sync disabled status on network
            // Disable gameobjects for clients upon connecting, if disabled status is true
            // Can be used to disable characters that are far from players to save memory
            // Characters can be split into areas (area_00, area_01 etc)
        }
    }
}
