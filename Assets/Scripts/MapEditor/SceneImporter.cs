using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SceneImporter : MonoBehaviour
{
    public string importFileName = "sceneExport.json";

    private void Update()
    {
        // Importer la scène lorsqu'on appuie sur "p"
        if (Input.GetKeyDown(KeyCode.P))
        {
            ImportScene();
        }
    }

    public void ImportScene()
    {
        // Chemin complet du fichier dans le dossier "Assets/Imports"
        string filePath = "Assets/Imports/"+importFileName;

        // Lire le JSON du fichier
        string json = File.ReadAllText(filePath);

        // Convertir le JSON en données de scène
        SceneData sceneData = JsonUtility.FromJson<SceneData>(json);

        // Recréer les objets dans la scène
        foreach (SceneObjectData objectData in sceneData.objects)
        {
            GameObject obj = new GameObject(objectData.name);
            obj.transform.position=objectData.position;
            obj.transform.rotation=objectData.rotation;
            obj.transform.localScale=objectData.scale;
        }

        Debug.Log("Scene imported from "+filePath);
    }
}