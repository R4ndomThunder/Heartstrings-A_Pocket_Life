using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class SaveSystem
{
    public static GameData gameData = new GameData();

    public static void Load()
    {
        string path = Application.persistentDataPath + "/saveData.dat";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            gameData = JsonUtility.FromJson<GameData>(json);
        }
        else
        {
            gameData = new GameData();
        }
    }

    public static void Save()
    {
        string json = JsonUtility.ToJson(gameData);
        string path = Application.persistentDataPath + "/saveData.dat";
        File.WriteAllText(path, json);
    }

}

public class GameData
{
    public CharacterData character = new();
    public SettingsData settings = new();
    public List<WindowState> windows = new();

    public GameData()
    {
        character = new();
        settings = new();
    }

    public void SaveWindowState(string name, Vector2 pos, bool isOpen)
    {
        List<WindowState> list = windows;
        var win = list.FirstOrDefault(x => x.windowName == name);
        if (win != null)
        {
            win.x = pos.x;
            win.y = pos.y;
            win.isOpen = isOpen;
        }
        else
        {
            list.Add(new WindowState
            {
                windowName = name,
                x = pos.x,
                y = pos.y,
                isOpen = isOpen
            });
        }

        windows = list;
    }

    public WindowState GetWindowState(string name) => windows.FirstOrDefault(x => x.windowName == name);
}

[Serializable]
public class CharacterData
{
    public long lastCryDate = DateTime.Now.ToBinary();
    public string characterName = "Her";
    public float happyness = 100, love = 100, creativity = 100, hunger = 100, energy = 100;
    public int currentMood = 0;
    public int currentState = 0;
    public int activityCounter = 0;

    public CharacterData()
    {
        lastCryDate = DateTime.Now.ToBinary();
        characterName = "Her";
        happyness = 100;
        love = 100;
        creativity = 100;
        hunger = 100;
        energy = 100;

        currentMood = 0;
        currentState = 0;
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

[Serializable]
public class WindowState
{
    public string windowName;
    public float x, y;
    public bool isOpen;
}