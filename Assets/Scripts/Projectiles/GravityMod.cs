using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class GravityMod : ProjectileMod
{
    private List<float> _fields;
    public float gravityConstant;
    public List<string> targetTags;
    Transform target;
    private Vector3 dir;
    private List<Rigidbody> targetRigidbodies;


    private void OnEnable()
    {
        targetRigidbodies = new List<Rigidbody>();
        AssignRigidBody(gameObject.GetComponentInParent<Rigidbody>());
    }

    private void FixedUpdate()
    {
        if (targetRigidbodies.Count > 0)
        {
            foreach (var targetRb in targetRigidbodies)
            {
                float distance = Vector3.Distance(_objectPhysics.position, targetRb.position);
                targetRb.AddForce(((gravityConstant * _objectPhysics.mass * targetRb.mass) / distance) *
                                  (targetRb.position - _objectPhysics.position));   
            }
        }
       
    }

    private void OnTriggerEnter(Collider other)
    {   
        foreach (var tag in targetTags)
        {
            if (other.CompareTag(tag))
            {
                
                Rigidbody rb = other.GetComponent<Rigidbody>();
                Debug.Log(rb.gameObject.name);
                if (!targetRigidbodies.Contains(rb))
                {
                    targetRigidbodies.Add(rb);
                }
                
            }
        }
    }
}
