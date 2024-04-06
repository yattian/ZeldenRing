using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace YT
{
    public class PlayerUIManager : MonoBehaviour
    {
        public static PlayerUIManager instance;

        [Header("NETWORK JOIN")]
        [SerializeField] bool startGameAsClient;

        [HideInInspector] public PlayerUIHudManager playerUIHudManager;
        [HideInInspector] public PlayerUIPopUpManager playerUIPopUpManager;
        [HideInInspector] public PlayerUIDialogueManager playerUIDialogueManager;

        [Header("UI Flags")]
        public bool menuWindowIsOpen = false; // Inventory screen, equipment menu
        public bool popUpWindowIsOpen = false; // Item pick ups, dialogue pop ups

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

            playerUIHudManager = GetComponentInChildren<PlayerUIHudManager>();
            playerUIPopUpManager = GetComponentInChildren<PlayerUIPopUpManager>();
            playerUIDialogueManager = GetComponentInChildren<PlayerUIDialogueManager>();
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            if (startGameAsClient)
            {
                startGameAsClient = false;
                // WE MUST FIRST SHUT DOWN, BECAUSE WE HAVE STARTED AS A HOST DURING THE TITLE SCREEN
                NetworkManager.Singleton.Shutdown();
                // WE THEN RESTART, AS A CLIENT
                NetworkManager.Singleton.StartClient();
            }
        }
    }
}