using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class NumericButton : NetworkBehaviour, IInteractable
{
    [SerializeField] public UnityEvent buttonInteracted;

    public void Interact()
    {
        buttonInteracted.Invoke();
    }
}