using UnityEngine;
using UnityEngine.Events;

public class Bouton : MonoBehaviour, IInteractable
{
    [SerializeField] UnityEvent buttonInteracted;

    public void Interact()
    {
        buttonInteracted.Invoke();
    }

    void Start()
    {
    }
}