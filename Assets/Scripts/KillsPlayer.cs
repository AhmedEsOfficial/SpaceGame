using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * You can attach this to any object and assign the value and it will kill the player if it enters it's radius
 */

public class KillsPlayer : MonoBehaviour
{
    
    public float killRadius;
    public string _deathMessage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().Die();
        }
    }
}
