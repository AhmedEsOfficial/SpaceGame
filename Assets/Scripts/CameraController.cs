using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform target;
 
    [SerializeField]
    private Vector3 offsetPosition;
 
    [SerializeField]
    private Space offsetPositionSpace = Space.Self;
 
    [SerializeField]
    private bool lookAt = true;

    public bool strategicView;

    private References _references;

    private void Start()
    {
        _references = GameObject.Find("References").GetComponent<References>();
    }   

    private void Update()
    {
        Refresh();
    }
 
    public void Refresh()
    {
        if(target == null)
        {
            Debug.LogWarning("Missing target ref !", this);
 
            return;
        }
 
        if (strategicView)
        {
            offsetPosition = new Vector3(0, 4, 0); 
            transform.position = target.TransformPoint(offsetPosition);
            transform.rotation = Quaternion.AngleAxis(90, Vector3.right);
            _references.GetPlayerController().takingInput = false;
        }
        else
        {
            // compute position
            if(offsetPositionSpace == Space.Self)
            {
                transform.position = target.TransformPoint(offsetPosition);
            }
            else
            {
                transform.position = target.position + offsetPosition;
            }

        }
        
        
 
        // compute rotation
        if(lookAt)
        {
            transform.LookAt(target);
        }
        else
        {
            transform.rotation = target.rotation;
        }
    }
}