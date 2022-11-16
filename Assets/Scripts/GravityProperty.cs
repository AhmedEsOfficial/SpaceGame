using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GravityProperty : MonoBehaviour
{
    public List<Rigidbody> GravityObjects;
    public Rigidbody objectPhysics;

    public float gravityConstant;

    public bool lockOrbit;
    public bool isEnemyProjectile;

    public bool hasChangedGravityOnce;
        
    Transform target;
    private Vector3 dir ;
    // Start is called before the first frame update
    void Start()
    {
        if (isEnemyProjectile)
        {
            GravityObjects[0] = GameObject.FindWithTag("Planet").GetComponent<Rigidbody>();
        }

    }

    public void AddGravityObject(Rigidbody rb)
    {
        GravityObjects.Add(rb);
    }

    public void ResetGravity()
    {
        GravityObjects = new List<Rigidbody>();
    }

    public void AssignGravityConstant(float G)
    {
        gravityConstant = G;
    }

    public void LockOrbit(bool state)
    {
        lockOrbit = false;
    }
    // Update is called once per frame
    void FixedUpdate()
    {

        if (GravityObjects.Count > 0)
        {
            foreach (var rb in GravityObjects)
            {
                float distance = Vector3.Distance(objectPhysics.position, rb.position);
                target = rb.transform;
                dir = target.transform.position - transform.position;//direction from your object towards the target object what you will orbit (the other side of the plane)
                Vector3 force = ((gravityConstant * objectPhysics.mass * rb.mass) / distance) *
                                (rb.position - objectPhysics.position);            //Newton came up with this

                objectPhysics.AddForce(force);
                //rb.AddForce(force);
                if (lockOrbit)
                {
                    dir = target.transform.position - transform.position;
                
                    transform.LookAt(rb.position); // look at the target
                    objectPhysics.AddForce(force.magnitude * transform.right); //add the force to make your object move (orbit)            
                }   
            
            }
        }
        

        
        
        

    }
}
