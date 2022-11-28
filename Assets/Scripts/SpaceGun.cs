using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;

public class SpaceGun : MonoBehaviour
{
    
    public GameObject bullet;
    public float bulletSpeed;
    public AudioSource _as;
    public References references;
    public float gunCoolDown;

    public Transform target;

    public Transform bulletSpawn;
    public ProjectileType gunType;

    public bool isPlayer;

    private bool _isOnCoolDown;

    public bool AimOn { get; set; }

    private bool _shootCommand;


    private void Start()
    {
        if (!isPlayer)
        {
            target = GameObject.FindWithTag("Player").transform;
        }
       
    }

    void FixedUpdate()
    {
        if (_shootCommand)
        {
            if (!_isOnCoolDown)
            {

                CreateBullet(transform.forward);
            }
        }


    }

    public void Shoot()
    {
        _shootCommand = true;
    }

    public void StopShooting()
    {
        _shootCommand = false;
    }

    private void Update()
    {
        if (isPlayer)
        {
            if(_shootCommand)
            {
                _as.Stop();
            }

            
        }
        
    }


    void CreateBullet(Vector3 direction)
    {
        if (isPlayer)
        {
            if (AimOn)
            {
                Ray aim = Camera.main.ScreenPointToRay(Input.mousePosition);
                direction = aim.direction;
            }
        }
        else
        {
            direction =  target.position- transform.position;
        }
        
        GameObject newBullet = Instantiate(bullet, bulletSpawn.transform);
        newBullet.SetActive(true);
        Rigidbody rb = newBullet.GetComponentInChildren<Rigidbody>();
        rb.AddForce(bulletSpeed*direction,ForceMode.Impulse);
        _isOnCoolDown = true;

        StartCoroutine(GunCoolDown());

    }
    
    

    IEnumerator GunCoolDown()
    {
        yield return new WaitForSeconds(gunCoolDown);
        _isOnCoolDown = false;

    }
}


