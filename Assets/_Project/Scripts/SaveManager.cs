using RTDK.Logger;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    [SerializeField]
    private float autoSaveTimeSeconds = 5f;
    private Coroutine autoSaveCoroutine;
    private WaitForSeconds autoSaveTime;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnApplicationQuit()
    {
        SaveSystem.Save();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SaveSystem.Load();

        if (autoSaveCoroutine != null)
            StopCoroutine(autoSaveCoroutine);

        autoSaveCoroutine = StartCoroutine(AutoSave());
    }

    private IEnumerator AutoSave()
    {
        autoSaveTime = new WaitForSeconds(autoSaveTimeSeconds);
        while (true)
        {
            yield return autoSaveTime;
            SaveSystem.Save();
            RTDKLogger.Log("Game Saved");
        }
    }
}
