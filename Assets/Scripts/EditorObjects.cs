using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class EditorObjects : NetworkBehaviour
{
    public Transform objTransform;
    public float xPos, yPos, zPos;
    public float xRot, yRot, ZRot;
    public float xSca, ySca, zSca;

    // Start is called before the first frame update
    void Start()
    {
        if (objTransform == null)
        {
            objTransform=this.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
