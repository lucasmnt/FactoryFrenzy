using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SceneImporter : MonoBehaviour
{
    public string importFileName = "sceneExport.json";

    private void Update()
    {
        // Importer la sc�ne lorsqu'on appuie sur "p"
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

        // Convertir le JSON en donn�es de sc�ne
        SceneData sceneData = JsonUtility.FromJson<SceneData>(json);

        // Recr�er les objets dans la sc�ne
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