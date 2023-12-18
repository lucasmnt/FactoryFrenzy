using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UnitySceneManager : MonoBehaviour
{ 
    public void LoadGameScene()
    {
        // Utiliser SceneManager.LoadScene pour charger la scène OfficeHubScene
        SceneManager.LoadScene("GameScene");
    }

    public void LoadHubMenu()
    {
        // Utiliser SceneManager.LoadScene pour charger la scène WorkingDayScene
        SceneManager.LoadScene("HubMenu");
    }

    public void LoadHubMulti()
    {
        // Utiliser SceneManager.LoadScene pour charger la scène WorkingDayScene
        SceneManager.LoadScene("HubMulti");
    }

    public void LoadSceneWithName(string sceneName)
    {
        // Utiliser SceneManager.LoadScene pour charger la scène WorkingDayScene
        SceneManager.LoadScene(sceneName);
    }
}
