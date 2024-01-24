using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public GameSettingsData gameSettingsData;

    public static GameManager Instance
    {
        get
        {
            if (instance==null)
            {
                // S'il n'y a pas encore d'instance, essayez de la trouver dans la sc�ne
                instance=FindObjectOfType<GameManager>();

                // Si aucune instance n'a �t� trouv�e, cr�ez une nouvelle instance
                if (instance==null)
                {
                    GameObject obj = new GameObject("GameManager");
                    instance=obj.AddComponent<GameManager>();
                }
            }
            return instance;
        }
    }

    // Vos autres membres et m�thodes ici

    private void Awake()
    {
        // Assurez-vous que seule une instance de GameManager existe
        if (instance==null)
        {
            instance=this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void SaveGameSettings(GameSettingsData gameSettings)
    {
        this.gameSettingsData = gameSettings;
    }
}
