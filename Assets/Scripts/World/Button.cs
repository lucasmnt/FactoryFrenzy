using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class Bouton : NetworkBehaviour, IInteractable
{
    [SerializeField] UnityEvent buttonInteracted;

    public void Interact()
    {
        buttonInteracted.Invoke();
    }

    void Update()
    {
        // D�tecte l'appui sur la touche "E"
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryInteractWithButton();
        }
    }

    // M�thode pour tenter d'interagir avec le bouton
    private void TryInteractWithButton()
    {
        /*// Cr�ez un rayon depuis la cam�ra vers l'avant
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // V�rifiez s'il y a une collision avec le bouton
        if (Physics.Raycast(ray, out hit))
        {
            // V�rifiez si l'objet touch� est le bouton
            if (hit.collider.gameObject==gameObject)
            {
                // Appel � la m�thode Interact si le bouton est touch�
                Interact();
            }
        }*/
    }
}