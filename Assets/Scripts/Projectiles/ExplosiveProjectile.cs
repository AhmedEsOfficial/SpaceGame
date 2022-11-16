using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveProjectile : MonoBehaviour
{
    public List<string> targetTags;
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
                enemy.AddExplosionForce(10000f,transform.position, 50f);
            }
        }
    }


    IEnumerator WaitingToExplode()
    {
        yield return new WaitForSeconds(timeBeforeExplosion);
        _exploding = true;
    }

    private void OnTriggerStay(Collider other)
    {

        foreach (var tTag in targetTags)
        {
            if (other.gameObject.CompareTag(tTag) )
            {
                Rigidbody rb = other.GetComponent<Rigidbody>();
                if (_exploding && !_enemiesInRadius.Contains(rb))
                {
                    _enemiesInRadius.Add(rb);
                    
                }
            
            }
        }
        
        
        
    }

    private void OnTriggerExit(Collider other)
    {
        foreach (var tTag in targetTags)
        {
            if (other.gameObject.CompareTag(tTag))
            {
                _enemiesInRadius.Remove(other.GetComponent<Rigidbody>());
            }
        }
        
    }
}
