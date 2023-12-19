using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turbine : MonoBehaviour
{
    [SerializeField]
    public Animation anim;
    
    [SerializeField]
    public bool isBlowing=true;

    List<Rigidbody> RigidbodiesInWindZoneList = new List<Rigidbody>();
    public Vector3 windDirection = Vector3.right;
    public float windStrength = 5;

    private void Start()
    {
        anim=gameObject.GetComponentInChildren<Animation>();
        PlayAnimationLoop();
    }

    private IEnumerator PlayAnimationLoop()
    {
        while (isBlowing)
        {
            PlayAnimation("TurbineRunning");
            yield return new WaitForSeconds(0.2f);
        }
    }

    // Plays an animation clip.
    private void PlayAnimation(string animationName)
    {
        anim.Play(animationName);
    }

    private void OnTriggerEnter(Collider col)
    {
        Rigidbody objectRigid = col.gameObject.GetComponent<Rigidbody>();
        if (objectRigid != null)
            RigidbodiesInWindZoneList.Add(objectRigid);
    }

    private void OnTriggerExit(Collider col)
    {
        Rigidbody objectRigid = col.gameObject.GetComponent<Rigidbody>();
        if (objectRigid != null)
            RigidbodiesInWindZoneList.Remove(objectRigid);
    }

    private void FixedUpdate()
    {
        if (RigidbodiesInWindZoneList.Count > 0)
        {
            foreach (Rigidbody rigid in RigidbodiesInWindZoneList)
            {
                rigid.AddForce(windDirection * windStrength);
            }
        }
    }
}