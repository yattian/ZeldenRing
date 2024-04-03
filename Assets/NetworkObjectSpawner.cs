using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace YT
{
    public class NetworkObjectSpawner : MonoBehaviour
    {
        [Header("Object")]
        [SerializeField] protected GameObject networkGameObject;
        [SerializeField] protected GameObject instantiateGameObject;

        private void Awake()
        {

        }

        private void Start()
        {
            WorldObjectManager.instance.SpawnObject(this);
            gameObject.SetActive(false);
        }

        public virtual void AttemptToSpawnObject()
        {
            if (networkGameObject != null)
            {
                instantiateGameObject = Instantiate(networkGameObject);
                instantiateGameObject.transform.position = transform.position;
                instantiateGameObject.transform.rotation = transform.rotation;
                instantiateGameObject.GetComponent<NetworkObject>().Spawn();
            }
        }
    }
}
