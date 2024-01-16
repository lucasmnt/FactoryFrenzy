using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class NumericalPad : NetworkBehaviour
{
    public int currentNumber;
    public TMP_Text textBuilding;
    public TMP_Text textSent;

    [SerializeField] UnityEvent sendNumberToScreen;

    public void OnNumericButtonPressed(int numericValue)
    {
        textBuilding.text = textBuilding.text + numericValue.ToString();
    }

    public void OnValidateButtonPressed()
    {
        textSent.text = textBuilding.text;
        sendNumberToScreen.Invoke();
        textBuilding.text = "";
    }

    public void OnDeleteButtonPressed()
    {
        textBuilding.text = "";
    }
}