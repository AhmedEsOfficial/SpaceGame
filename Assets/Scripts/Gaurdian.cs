using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Gaurdian : MonoBehaviour
{

    public Transform player;
    public float maxPatience;

    private float _patience;
    // Start is called before the first frame update
    void Start()
    {
        _patience = 0;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player);
    }

    private void FixedUpdate()
    {
        if (_patience >= maxPatience)
        {
            print("The player has been cursed!");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _patience += 0.5f;
        }
    }
}   