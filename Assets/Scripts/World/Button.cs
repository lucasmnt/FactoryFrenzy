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
        // Détecte l'appui sur la touche "E"
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryInteractWithButton();
        }
    }

    // Méthode pour tenter d'interagir avec le bouton
    private void TryInteractWithButton()
    {
        /*// Créez un rayon depuis la caméra vers l'avant
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Vérifiez s'il y a une collision avec le bouton
        if (Physics.Raycast(ray, out hit))
        {
            // Vérifiez si l'objet touché est le bouton
            if (hit.collider.gameObject==gameObject)
            {
                // Appel à la méthode Interact si le bouton est touché
                Interact();
            }
        }*/
    }
}