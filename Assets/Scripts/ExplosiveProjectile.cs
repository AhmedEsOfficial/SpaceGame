using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveProjectile : MonoBehaviour
{
    public string targetTag;
    private Rigidbody _rb;
    private bool _exploding;
    public float timeBeforeExplosion;

    private List<Rigidbody> _enemiesInRadius;
    private void OnEnable()
    {
        //StartCoroutine(WaitingToExplode());
        _exploding = true;
        _rb = gameObject.GetComponent<Rigidbody>();
        _enemiesInRadius = new List<Rigidbody>();
    }

    private void FixedUpdate()
    {
        
        if (_enemiesInRadius.Count > 0)
        {
            foreach (Rigidbody enemy in _enemiesInRadius)
            {
                print(enemy.gameObject.name);
                enemy.AddExplosionForce(100f,transform.position, 50f);
            }
        }
    }


    IEnumerator WaitingToExplode()
    {
        yield return new WaitForSeconds(timeBeforeExplosion);
        _exploding = true;
    }

    private void OnTriggerEnter(Collider other)
    {                    

        
        if (other.gameObject.CompareTag(targetTag) )
        {
            if (_exploding)
            {
                _enemiesInRadius.Add(other.GetComponent<Rigidbody>());
                    
            }
            
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(targetTag))
        {
            _enemiesInRadius.Remove(other.GetComponent<Rigidbody>());
        }
    }
}
