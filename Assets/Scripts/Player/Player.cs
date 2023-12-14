using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField, Range(0f, 100f)] 
    float interactionDistance = 0f;

    [SerializeField, Range(0f, 5f)] 
    private const float maxHoldTime = 2f; // temps maximum pour force max.

    [SerializeField, Range(0f, 5f)] 
    private float minDistance = 1f; // Set your minimum distance here

    [SerializeField, Range(0f, 10f)] 
    private float maxDistance = 5f; // Set your maximum distance here

    [SerializeField, Range(0f, 100f)] 
    private float itemRotationSpeed = 10f;

    [SerializeField, Range(0f, 200f)]
    private float heatlh = 200f;

    [SerializeField] 
    LayerMask raycastLayerMask;

    void Start()
    {

    }

    void Update()
    {
        #region Input
        if (Input.GetKeyDown(KeyCode.F))
        {
            // Raycast from the main camera
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, interactionDistance, raycastLayerMask))
            {
                // Check if the hit object implements the IInteractable interface
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    interactable.Interact();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }

        #endregion
    }

    private void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying=false;
        #else
            Application.Quit();
        #endif
    }
}
