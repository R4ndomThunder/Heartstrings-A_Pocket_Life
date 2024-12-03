using RTDK.Logger;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static SaveSystem;

public class SaveSystem
{
    public class GameData
    {
        public long lastUpdated;
        public CharacterData character = new();
        public SettingsData settings = new();
    }

    [Serializable]
    public class CharacterData
    {
        public DateTime lastCryDate = DateTime.Now;
        public string characterName = "Her";
        public float happyness = 100, love = 100, creativity = 100, hunger = 100, energy = 100;
        public int currentMood = 0;
        public int currentState = 0;

        public CharacterData()
        {
            lastCryDate = DateTime.Now;
            characterName = "Her";
            happyness = 100;
            love = 100;
            creativity = 100;
            hunger = 100;
            energy = 100;

            currentMood = (int)PetMood.Normal;
            currentState = (int)PetState.Idle;
        }
    }

    [Serializable]
    public class SettingsData
    {
        public float musicVol, soundVol;
        public int windowSize;

        public SettingsData()
        {
            musicVol = 60;
            soundVol = 60;
            windowSize = 0;
        }
    }

    public static GameData gameData = new();
}

public class SaveFileHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";


    private bool useEncryption = false;
    private readonly string encryptionCodeWord = "rtdksavesystem";
    private readonly string backupExt = ".bkp";


    public SaveFileHandler(string _dataDirPath, string _dataFileName, bool _useEncryption)
    {
        this.dataDirPath = _dataDirPath;
        this.dataFileName = _dataFileName;
        this.useEncryption = _useEncryption;
    }

    /// <summary>
    /// Load data from selected file
    /// </summary>
    /// <param name="profileId"></param>
    /// <param name="allowRestoreFromBackup"></param>
    /// <returns></returns>
    public GameData Load(string profileId, bool allowRestoreFromBackup = true)
    {
        if (profileId == null)
            return null;

        string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);

        GameData loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                using FileStream stream = new(fullPath, FileMode.Open);
                using StreamReader reader = new(stream);

                dataToLoad = reader.ReadToEnd();

                if (useEncryption)
                    dataToLoad = EncryptDecrypt(dataToLoad);

                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {

                if (allowRestoreFromBackup)
                {
                    RTDKLogger.Warning($"Error occurred when trying to load data. Attemping to roll back. \n{e}");
                    bool rollbackSuccess = AttemptRollback(fullPath);
                    if (rollbackSuccess)
                    {
                        loadedData = Load(profileId, false);
                    }
                }
                else
                {
                    RTDKLogger.Error($"Error occured when trying to load file {fullPath} and backup did not work.\n{e}");
                }
            }
        }

        return loadedData;
    }

    /// <summary>
    /// Save data to selected file
    /// </summary>
    /// <param name="data"></param>
    /// <param name="profileId"></param>
    public void Save(GameData data, string profileId)
    {
        if (profileId == null) return;

        string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);
        string backupFilePath = fullPath + backupExt;
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonUtility.ToJson(data, true);

            if (useEncryption)
                dataToStore = EncryptDecrypt(dataToStore);

            using FileStream stream = new(fullPath, FileMode.Create);
            using StreamWriter writer = new(stream);

            writer.Write(dataToStore);

            GameData verifiedGameData = Load(profileId);

            if (verifiedGameData != null)
            {
                File.Copy(fullPath, backupFilePath, true);
            }
            else
            {
                throw new Exception("Save file could not be verified and backup could not be created.");
            }
        }
        catch (Exception e)
        {
            RTDKLogger.Error($"Error occurred when trying to save data to file: {fullPath} \n{e}");
        }
    }

    /// <summary>
    /// Delete selected saved file
    /// </summary>
    /// <param name="profileId"></param>
    public void Delete(string profileId)
    {
        if (profileId == null) return;

        string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);
        try
        {
            if (File.Exists(fullPath))
            {
                Directory.Delete(Path.GetDirectoryName(fullPath), true);
            }
            else
            {
                RTDKLogger.Warning($"Tried to delete profile data, but data was not found at path: {fullPath}");
            }
        }
        catch (Exception e)
        {
            RTDKLogger.Error($"Failed to delete profile data for profileId {profileId} at path: {fullPath}\n{e}");
        }
    }

    /// <summary>
    /// Get all saved profiles
    /// </summary>
    /// <returns></returns>
    public Dictionary<string, GameData> LoadAllProfiles()
    {
        Dictionary<string, GameData> profileDictionary = new();

        IEnumerable<DirectoryInfo> dirInfos = new DirectoryInfo(dataDirPath).EnumerateDirectories();
        foreach (DirectoryInfo dirInfo in dirInfos)
        {
            string profileId = dirInfo.Name;

            string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);
            if (!File.Exists(fullPath))
            {
                RTDKLogger.Warning($"Skipping directory when loading all profiles because it does not contains data: {profileId}");
                continue;
            }

            GameData profileData = Load(profileId);
            if (profileData != null)
            {
                profileDictionary.Add(profileId, profileData);
            }
            else
            {
                RTDKLogger.Error($"Tried to load profile but something went wrong. ProfileId: {profileId}");
            }
        }
        return profileDictionary;
    }


    /// <summary>
    /// Get the most recently saved file
    /// </summary>
    /// <returns></returns>
    public string GetMostRecentlyUpdatedProfileId()
    {
        string mostRecentProfileId = null;

        Dictionary<string, GameData> profilesGameData = LoadAllProfiles();
        foreach (var pair in profilesGameData)
        {
            string profileId = pair.Key;
            GameData gameData = pair.Value;

            if (gameData == null)
                continue;

            if (mostRecentProfileId == null)
                mostRecentProfileId = profileId;
            else
            {
                DateTime mostRecentDateTime = DateTime.FromBinary(profilesGameData[mostRecentProfileId].lastUpdated);
                DateTime newDateTime = DateTime.FromBinary(gameData.lastUpdated);

                if (newDateTime > mostRecentDateTime)
                    mostRecentProfileId = profileId;
            }
        }

        return mostRecentProfileId;
    }

    /// <summary>
    /// XOR Encryption
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    private string EncryptDecrypt(string data)
    {
        string modifiedData = "";
        for (int i = 0; i < data.Length; i++)
        {
            modifiedData += (char)(data[i] ^ encryptionCodeWord[i % encryptionCodeWord.Length]);
        }

        return modifiedData;
    }

    /// <summary>
    /// Rollbacks to the backup save
    /// </summary>
    /// <param name="fullpath"></param>
    /// <returns></returns>
    private bool AttemptRollback(string fullpath)
    {
        bool success = false;

        string backupFilePath = fullpath + backupExt;

        try
        {
            if (File.Exists(backupFilePath))
            {
                File.Copy(backupFilePath, fullpath, true);
                success = true;
                RTDKLogger.Warning($"Had to roll back to backup file at: {backupFilePath}");
            }
            else
            {
                throw new Exception("Tried to roll back, but no backup file currently exists");
            }
        }
        catch (Exception e)
        {
            RTDKLogger.Error($"Error occured when trying to roll back to backup file at: {backupFilePath}\n{e}");
        }
        return success;
    }
}
