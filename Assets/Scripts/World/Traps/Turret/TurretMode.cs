using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TurretMode : MonoBehaviour
{
    Transform _Player;
    float dist;
    public float howClose;
    public Transform head, barrel;
    public GameObject _projectile;
    public float fireRate, nextFire;

    void Start()
    {
        _Player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        dist = Vector3.Distance(_Player.position, transform.position);
        if (dist <= howClose) 
        {
            head.LookAt(_Player);
            if(Time.time >= nextFire)
            {
                nextFire = Time.time + 1f / fireRate;
                shoot();
            }

        }
    }

    void shoot()
    {
        GameObject clone = Instantiate(_projectile, barrel.position, head.rotation);
        clone.GetComponent<Rigidbody>().AddForce(head.forward * 1500);
        Destroy(clone, 10);
    }
}
