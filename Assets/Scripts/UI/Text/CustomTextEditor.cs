using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class CustomTextEditor : MonoBehaviour, IWritable
{
    [SerializeField]
    private TMP_Text textComponent; // Ou utilisez Text si vous n'utilisez pas TextMeshPro

    [SerializeField]
    private UnityEvent setIsWritingToPlayerTrue;

    [SerializeField]
    private UnityEvent setIsWritingToPlayerFalse;

    private string currentText = "";

    private bool isWriting = false;

    public bool IsWriting
    {
        get { return isWriting; }
    }

    void Start()
    {
        UpdateText();
    }

    void Update()
    {
        UpdateText();
        if (isWriting)
        {
            HandleInput();
        }
    }

    private void HandleInput()
    {
        foreach (char c in Input.inputString)
        {
            if (c=='\b') // Backspace
            {
                if (currentText.Length>0)
                {
                    currentText=currentText.Substring(0, currentText.Length-1);
                }
            }
            else if (c=='\r'||c=='\n') // Enter
            {
                isWriting=false;
                setIsWritingToPlayerFalse.Invoke();
            }
            else if (c=='\u001b') // Escape
            {
                isWriting=false;
                currentText=""; // Réinitialisez le texte si l'utilisateur appuie sur Escape
                setIsWritingToPlayerFalse.Invoke();
            }
            else
            {
                currentText+=c;
            }
        }
        UpdateText();
    }

    private void UpdateText()
    {
        textComponent.text=currentText;
    }

    public void Write(bool isWriting)
    {
        this.isWriting=isWriting;

        if (isWriting)
        {
            setIsWritingToPlayerTrue.Invoke();
            Debug.Log("Start Writing");
        }
        else
        {
            // Le texte a été confirmé, vous pouvez traiter le texte ici si nécessaire
            Debug.Log("Text confirmed: "+currentText);
            setIsWritingToPlayerFalse.Invoke();
        }
    }
}