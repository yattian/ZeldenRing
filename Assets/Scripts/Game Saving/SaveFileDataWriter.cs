using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

namespace YT
{
    public class SaveFileDataWriter
    {
        public string saveDataDirectoryPath = "";
        public string saveFileName = "";

        // Before create new save, check if file already exists
        public bool CheckToSeeIfFileExists()
        {
            if (File.Exists(Path.Combine(saveDataDirectoryPath, saveFileName)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Delete character save files
        public void DeleteSaveFile()
        {
            File.Delete(Path.Combine(saveDataDirectoryPath, saveFileName));
        }

        // Used to create new save file upon starting new game
        public void CreateNewCharacterSaveFile(CharacterSaveData characterData)
        {
            // Make a path to save file 
            string savePath = Path.Combine(saveDataDirectoryPath, saveFileName);

            try
            {
                // Create directory of file will be written to
                Directory.CreateDirectory(Path.GetDirectoryName(savePath));
                Debug.Log("Creating save file, at save path: " + savePath);

                // Serialize the C# game data object to JSON file
                string dataToStore = JsonUtility.ToJson(characterData, true);

                // Write file to our system
                using (FileStream stream = new FileStream(savePath, FileMode.Create))
                {
                    using (StreamWriter fileWriter = new StreamWriter(stream))
                    {
                        fileWriter.Write(dataToStore);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("Error while trying to save character data, game not saved " + savePath + "\n" + ex);
            }
        }
    
        // Used to load a save file
        public CharacterSaveData LoadSaveFile()
        {
            CharacterSaveData characterData = null;

            string loadPath = Path.Combine(saveDataDirectoryPath, saveFileName);

            if (File.Exists(loadPath))
            {
                try
                {
                    string dataToLoad = "";

                    using (FileStream stream = new FileStream(loadPath, FileMode.Open))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            dataToLoad = reader.ReadToEnd();
                        }
                    }

                    // Deserialize
                    characterData = JsonUtility.FromJson<CharacterSaveData>(dataToLoad);
                }
                catch (Exception ex)
                {
                    Debug.LogError("Error while trying to load character data " + loadPath + "\n" + ex);
                }
                
            }
            return characterData;
        }
    
    }

}
