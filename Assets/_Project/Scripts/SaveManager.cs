using UnityEngine;

public class SaveManager : MonoBehaviour
{
    SaveFileHandler fileHandler;

    private void Awake()
    {
        fileHandler = new(Application.persistentDataPath, "save.dat", true);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SaveSystem.gameData = fileHandler?.Load("0");
    }

    private void OnApplicationQuit()
    {
        fileHandler.Save(SaveSystem.gameData, "0");
    }
}
