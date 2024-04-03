using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace YT
{
    public class WorldObjectManager : MonoBehaviour
    {
        public static WorldObjectManager instance;

        [Header("Objects")]
        [SerializeField] List<NetworkObjectSpawner> networkObjectSpawners;
        [SerializeField] List<GameObject> spawnedInObjects;

        [Header("Fog Walls")]
        public List<FogWallInteractable> fogWalls;

        // 1. Create an object script that will hold the logic for fog walls
        // 2. Spawn in those fog walls as network objects during start of game (must have a spawner object)
        // 3. Create general object spawner script and perfab
        // 4. When fog walls are spawned, add them to the world fog wall list
        // 5. Grab the correct fog wall from the list on the boss manager when the boss is being initialized 

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

        public void SpawnObject(NetworkObjectSpawner networkObjectSpawner)
        {
            if (NetworkManager.Singleton.IsServer)
            {
                networkObjectSpawners.Add(networkObjectSpawner);
                networkObjectSpawner.AttemptToSpawnObject();
            }
        }

        public void AddFogWallToList(FogWallInteractable fogWall)
        {
            if (!fogWalls.Contains(fogWall))
            {
                fogWalls.Add(fogWall);
            }
        }

        public void RemoveFogWallFromList(FogWallInteractable fogWall)
        {
            if (fogWalls.Contains(fogWall))
            {
                fogWalls.Remove(fogWall);
            }
        }
    }
}
