using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class TextUpdater : NetworkBehaviour
{
    public TMP_Text textComponent;
    public int counterValue = 0;
    public int maxValue = 10;
    public int minValue = 0;
    public bool enable = false;
    public TypeOfParameters typeOfParameters = TypeOfParameters.NOT_IMPLEMENTED;

    public enum TypeOfParameters
    {
        Boolean,
        Int,
        Float,
        String,
        NOT_IMPLEMENTED
    }

    void Start()
    {

    }

    // Function to update the text dynamically
    public void UpdateText(string newText)
    {
        if (textComponent!=null)
        {
            textComponent.text=newText;
        }
    }

    public void IncrementCounter()
    {
        if (counterValue<maxValue)
        {
            counterValue++;
        }
        UpdateCounterText();
    }

    public void DecrementCounter()
    {
        if (counterValue>minValue)
        {
            counterValue--;
        }
        UpdateCounterText();
    }

    public void ToggleEnable()
    {
        if (enable==false)
        {
            enable=true;
            textComponent.text = "Enabled";
        }
        else
        {
            enable=false;
            textComponent.text= "Disabled";
        }
    }

    private void UpdateCounterText()
    {
        textComponent.text=counterValue.ToString();
    }
}