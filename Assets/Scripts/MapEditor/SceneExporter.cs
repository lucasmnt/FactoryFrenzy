using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SceneExporter : MonoBehaviour
{
    public string exportFileName = "sceneExport.json";

    private void Update()
    {
        // Exporter la sc�ne lorsqu'on appuie sur "m"
        if (Input.GetKeyDown(KeyCode.M))
        {
            ExportScene();
        }
    }

    public void ExportScene()
    {
        SceneData sceneData = new SceneData();

        // R�cup�rer tous les objets dans la sc�ne
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

        // Convertir les donn�es en JSON
        string json = JsonUtility.ToJson(sceneData);

        // Chemin complet du fichier dans le dossier "Resources/Imports"
        string filePath = "Assets/Imports/"+exportFileName;

        // Enregistrer le JSON dans un fichier
        File.WriteAllText(filePath, json);

        // Rafra�chir l'AssetDatabase pour prendre en compte les changements
        UnityEditor.AssetDatabase.Refresh();

        Debug.Log("Scene exported to "+filePath);
    }
}