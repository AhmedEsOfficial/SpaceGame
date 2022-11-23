using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class GravityProjectile : ProjectileMod
{
    private List<float> _fields;
    public float gravityConstant;
    public References references;
    public bool lockOrbit;
    public List<string> targetTags;
    Transform target;
    private Vector3 dir;



    
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
                    gp.ResetGravity(gravityConstant);
                    gp.AddGravityObject(_objectPhysics);
                    gp.LockOrbit(false);
                    Debug.Log(gp.gameObject.name);
                    Debug.Log(tag);
                }

            }
        }
    }
}
