using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SceneExporter : MonoBehaviour
{
    public string exportFileName = "sceneExport.json";

    private void Update()
    {
        // Exporter la scène lorsqu'on appuie sur "m"
        if (Input.GetKeyDown(KeyCode.M))
        {
            ExportScene();
        }
    }

    public void ExportScene()
    {
        SceneData sceneData = new SceneData();

        // Récupérer tous les objets dans la scène
        GameObject[] sceneObjects = GameObject.FindObjectsOfType<GameObject>();

        foreach (GameObject obj in sceneObjects)
        {
            SceneObjectData objectData = new SceneObjectData();
            objectData.name=obj.name;
            objectData.position=obj.transform.position;
            objectData.rotation=obj.transform.rotation;
            objectData.scale=obj.transform.localScale;

            sceneData.objects.Add(objectData);
        }

        // Convertir les données en JSON
        string json = JsonUtility.ToJson(sceneData);

        // Chemin complet du fichier dans le dossier "Resources/Imports"
        string filePath = "Assets/Imports/"+exportFileName;

        // Enregistrer le JSON dans un fichier
        File.WriteAllText(filePath, json);

        // Rafraîchir l'AssetDatabase pour prendre en compte les changements
        UnityEditor.AssetDatabase.Refresh();

        Debug.Log("Scene exported to "+filePath);
    }
}