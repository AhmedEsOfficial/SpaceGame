using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityProjectile : MonoBehaviour
{
    public Rigidbody objectPhysics;

    public References references;
    public float gravityConstant;

    public bool lockOrbit;


    Transform target;

    private Vector3 dir;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (references.activeBulletsInScene.Count > 0)
        {
            foreach (var rb in references.activeBulletsInScene)
            {
                float distance = Vector3.Distance(objectPhysics.position, rb.position);
                target = rb.transform;
                dir = target.transform.position -
                      transform.position; //direction from your object towards the target object what you will orbit (the other side of the plane)
                Vector3 force = ((gravityConstant * objectPhysics.mass * rb.mass) / distance) *
                                (objectPhysics.position-rb.position); //Newton came up with this

                if (float.IsNaN(force.x) || float.IsNaN(force.x) || float.IsNaN(force.x))
                {
                    
                }
                else
                {
                    rb.AddForce(force);
                    //rb.AddForce(force);
                    if (lockOrbit)
                    {
                        dir = target.transform.position - transform.position;

                        transform.LookAt(dir.normalized); // look at the target
                        rb.AddForce(force.magnitude *
                                    transform.right); //add the force to make your object move (orbit)            
                    }

                }
                
            }
        }
        
    }
}
