using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


public enum ProjectileType{Kinetic = 0, Gravity = 1, Explosive = 2

}
public class ProjectileMod : MonoBehaviour
{
    public ProjectileType id;
    public Rigidbody _objectPhysics;

    public void AssignRigidBody(Rigidbody rb)
    {
        _objectPhysics = rb;

    }
    
    
}
