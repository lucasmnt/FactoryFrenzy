using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // Do something
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider collision)
    {
        // Do something
        Destroy(gameObject);
    }
}