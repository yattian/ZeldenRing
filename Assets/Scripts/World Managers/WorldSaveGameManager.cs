using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace YT
{
    public class WorldSaveGameManager : MonoBehaviour
    {
        public static WorldSaveGameManager instance;

        [SerializeField] PlayerManager player;

        [Header("SAVE/LOAD")]
        [SerializeField] bool saveGame;
        [SerializeField] bool loadGame;

        [Header("World Scene Index")]
        [SerializeField] int worldSceneIndex = 1;

        [Header("Save Data Writer")]
        private SaveFileDataWriter saveFileDataWriter;

        [Header("Current Character Data")]
        public CharacterSlot currentCharacterSlotBeingUsed;
        public CharacterSaveData currentCharacterData;
        private string saveFileName;

        [Header("Character Slots")]
        public CharacterSaveData characterSlot01;
        //public CharacterSaveData characterSlot02;
        //public CharacterSaveData characterSlot03;
        //public CharacterSaveData characterSlot04;
        //public CharacterSaveData characterSlot05;
        //public CharacterSaveData characterSlot06;
        //public CharacterSaveData characterSlot07;
        //public CharacterSaveData characterSlot08;
        //public CharacterSaveData characterSlot09;
        //public CharacterSaveData characterSlot10;

        private void Awake()
        {
            // THERE CAN ONLY BE ONE INSTANCE OF THIS SCRIPT AT ONE TIME, IF ANOTHER EXISTS, DESTROY IT
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
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            if (saveGame)
            {
                saveGame = false;
                SaveGame();
            }

            if (loadGame)
            {
                loadGame = false;
                LoadGame();
            }
        }

        private void DecideCharacterFileNameBasedOnCharacterSlotBeingUsed()
        {
            switch (currentCharacterSlotBeingUsed)
            {
                case CharacterSlot.CharacterSlot_01:
                    saveFileName = "CharacterSlot_01";
                    break;
                case CharacterSlot.CharacterSlot_02:
                    saveFileName = "CharacterSlot_02";
                    break;
                case CharacterSlot.CharacterSlot_03:
                    saveFileName = "CharacterSlot_03";
                    break;
                case CharacterSlot.CharacterSlot_04:
                    saveFileName = "CharacterSlot_04";
                    break;
                case CharacterSlot.CharacterSlot_05:
                    saveFileName = "CharacterSlot_05";
                    break;
                case CharacterSlot.CharacterSlot_06:
                    saveFileName = "CharacterSlot_06";
                    break;
                case CharacterSlot.CharacterSlot_07:
                    saveFileName = "CharacterSlot_07";
                    break;
                case CharacterSlot.CharacterSlot_08:
                    saveFileName = "CharacterSlot_08";
                    break;
                case CharacterSlot.CharacterSlot_09:
                    saveFileName = "CharacterSlot_09";
                    break;
                case CharacterSlot.CharacterSlot_10:
                    saveFileName = "CharacterSlot_10";
                    break;
            }
        }
        
        public void CreateNewGame()
        {
            // Create new file, with file name depending on which slot using
            DecideCharacterFileNameBasedOnCharacterSlotBeingUsed();

            currentCharacterData = new CharacterSaveData();

        }

        public void LoadGame()
        {
            // Loading previous file, with file name depending on which slot we are using
            DecideCharacterFileNameBasedOnCharacterSlotBeingUsed();

            saveFileDataWriter = new SaveFileDataWriter();

            // Generally works on mutliple machine types 
            saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
            saveFileDataWriter.SaveFileName = saveFileName;
            currentCharacterData = saveFileDataWriter.LoadSaveFile();

            StartCoroutine(LoadWorldScene());
        }

        public void SaveGame()
        {
            // Save the current file under a file name depending on which slot
            DecideCharacterFileNameBasedOnCharacterSlotBeingUsed();

            saveFileDataWriter = new SaveFileDataWriter();
            // Generally works on mutliple machine types 
            saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
            saveFileDataWriter.SaveFileName = saveFileName;

            // Pass player info, from game, to their save file
            player.SaveGameDataToCurrentCharacterData(ref currentCharacterData);

            // Write that info onto a JSON file, savede to this machine
            saveFileDataWriter.CreateNewCharacterSaveFile(currentCharacterData);

        }

        public IEnumerator LoadWorldScene()
        {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(worldSceneIndex);

            yield return null;
        }

        public int GetWorldSceneIndex()
        {
            return worldSceneIndex;
        }
    }
}

