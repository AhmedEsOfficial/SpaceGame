using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class BaseProjectile : MonoBehaviour
{
    public ProjectileDescription projectileDescription;

    public Rigidbody rb;


    // Start is called before the first frame update
    private void Awake()
    {
        rb = GetComponentInParent<Rigidbody>();
        List<GameObject> aP = projectileDescription.modConfigs;


        foreach (GameObject aM in aP)
        {
            Instantiate(aM, transform);
            aM.SetActive(true);
            
            
        }
        
        
    }

    
}
