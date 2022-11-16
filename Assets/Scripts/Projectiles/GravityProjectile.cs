using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityProjectile : MonoBehaviour
{
    public Rigidbody objectPhysics;
    

    public References references;
    public float gravityConstant;

    public bool lockOrbit;

    public List<string> targetTags;

    Transform target;

    private Vector3 dir;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        
        
    }

    private void OnTriggerStay(Collider other)
    {
        foreach (var tag in targetTags)
        {
            if (other.CompareTag(tag))
            {
                GravityProperty gp =  other.GetComponent<GravityProperty>();
                if (!gp.hasChangedGravityOnce)
                {
                    gp.hasChangedGravityOnce = true;
                    gp.ResetGravity();
                    gp.LockOrbit(false);
                    gp.AddGravityObject(objectPhysics);
                    gp.AssignGravityConstant(gravityConstant);
                }

            }
        }
        
    }
}
