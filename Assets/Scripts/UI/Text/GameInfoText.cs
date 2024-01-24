using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class GameInfoText : MonoBehaviour
{
    [SerializeField]
    public TMP_Text textSource1;
    [SerializeField]
    public TMP_Text textSource2;
    [SerializeField]
    public TMP_Text textSource3;
    [SerializeField]
    public TMP_Text textSource4;
    [SerializeField]
    public TMP_Text textSource5;
    [SerializeField]
    public TMP_Text combinedText;

    public bool validated = false;

    void Start()
    {
        CombineAndDisplayText();
    }

    void Update()
    {
        if (!validated)
        {
            CombineAndDisplayText();
        }
    }

    void CombineAndDisplayText()
    {
        // Utilisez StringBuilder pour concaténer les textes
        StringBuilder combinedStringBuilder = new StringBuilder();
        string s;

        // Ajoutez le texte de chaque source avec un saut de ligne
        s = "Number of rounds : " + textSource1.text;
        combinedStringBuilder.AppendLine(s);
        s = "Number of players : " + textSource2.text;
        combinedStringBuilder.AppendLine(s);
        s = "Time Limit : " + textSource3.text;
        combinedStringBuilder.AppendLine(s);
        s="Time Limit after win: "+textSource5.text;
        combinedStringBuilder.AppendLine(s);
        s = "Minigames : " + textSource4.text;
        combinedStringBuilder.AppendLine(s);

        // Assurez-vous que le texte combiné est affiché dans votre objet Text
        combinedText.text = combinedStringBuilder.ToString();
    }
}
