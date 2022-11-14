using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class EnemyController : MonoBehaviour
{
    public Transform target;
    
    public float speed;
    public float range;
    public  float maxSpeed = 10;

    public List<GameObject> nearByEnemies;

    private Rigidbody _rb;
    private GameObject nearestEnemy;

    public List<SpaceGun> enemyGuns;



    private bool _pursuing;

    private bool _inRange;
    // Start is called before the first frame update
    void Start()
    {
        _rb = gameObject.GetComponent<Rigidbody>();
        _pursuing = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        transform.LookAt(target);
        if (_pursuing)
        {
            Vector3 nearestEnemyDistance = Vector3.negativeInfinity;
            

            if (nearByEnemies.Count > 0)
            {
                foreach (var enemy in nearByEnemies)
                {
                    if (Vector3.Distance(transform.position, enemy.transform.position) < Vector3.Distance(transform.position, nearestEnemyDistance) )
                    {
                        nearestEnemy = enemy;
                        nearestEnemyDistance = nearestEnemy.transform.position;
                        Vector3 steerAwayDirection =  _rb.transform.position- nearestEnemy.transform.position ;
                        _rb.AddForce(steerAwayDirection*speed/2);
                    }
                }
            }
            

            Vector3 direction =  target.transform.position- _rb.transform.position;
            if (!_inRange)
            {
                foreach (var spaceGun in enemyGuns)
                {
                    spaceGun.StopShooting();
                }
                _rb.AddForce(direction*speed);
                
                _rb.velocity = Vector3.ClampMagnitude(_rb.velocity, maxSpeed);


            }
            else
            {
                Brake();
                Attack();
            }

            if (_rb.velocity.magnitude > 200)
            {
                _rb.AddForce(Vector3.MoveTowards(new Vector3(0,0,0),-_rb.velocity, 120 * Time.deltaTime), ForceMode.Impulse);            }
        }
    }

    private void Attack()
    {
        foreach (var spaceGun in enemyGuns)
        {
            spaceGun.Shoot();
        }
    }

    private void Brake()
    {
        _rb.AddForce(-_rb.velocity, ForceMode.Impulse);
        _rb.AddTorque(-_rb.angularVelocity, ForceMode.Impulse);    
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            _inRange = true;
        }
        if (other.gameObject.layer == this.gameObject.layer)
        {
            if (!nearByEnemies.Contains(other.gameObject))
            {
                nearByEnemies.Add(other.gameObject);

            }
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            _inRange = false;
        }
        if (other.gameObject.layer == this.gameObject.layer)
        {
            nearByEnemies.Remove(other.gameObject);
            nearestEnemy = null;
        }
    }
}
