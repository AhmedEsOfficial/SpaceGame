using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    public Vector3 acceleration;
    public float rotationSpeed;
    public float noRotationMouseZone;
    public float turboCooldown;
    public float turboStrength;
    public float brakingStrength;
    public float maxVelocity;
    
    
    public Rigidbody rb;
    private Vector2 _rotateRatio;
    private Vector3 _thrustRatio;
    private Vector2 _screenMidpoint;
    
    

    
    
    private bool _thrustActive;
    private float _maxThrust;
    private float _thrustThreshold;
    private bool _switchedInput;
    private bool _brake;
    private bool _turbo;
    private bool _turboOnCoolDown;
    public bool takingInput;

    private bool _zRotationActive;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        _screenMidpoint = new Vector2(Screen.width / 2, Screen.height / 2);
        takingInput = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (takingInput)
        {
            CalculateMovement();

        }
    }

    private void CalculateMovement()
    {
        
        if (Input.GetKey(KeyCode.W))
        {
            _thrustActive = true;
            _thrustRatio.z = acceleration.z;
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            _thrustActive = false;
            _thrustRatio.z = 0;

        }
        if (Input.GetKey(KeyCode.S))
        {
            _thrustActive = true;
            _thrustRatio.z = -acceleration.z ;
        }
        
        if (Input.GetKeyUp(KeyCode.S))
        {
            _thrustActive = false;
            _thrustRatio.z = 0;
        }
        
        if (Input.GetKey(KeyCode.A))
        {
            _thrustActive = true;
            _thrustRatio.x = acceleration.x;
            
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            _thrustActive = false;
            _thrustRatio.x = 0;

        }
        if (Input.GetKey(KeyCode.D))
        {
            _thrustActive = true;
            _thrustRatio.x = -acceleration.x ;
        }
        
        if (Input.GetKeyUp(KeyCode.D))
        {
            _thrustActive = false;
            _thrustRatio.x = 0;

        }
        if (Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.Q))
        {
            //_zRotationActive = true;
             
        }

        if (Input.GetKeyDown(KeyCode.X) && !_turboOnCoolDown)
        {
            _turbo = true;
        }
       
        

        if (Input.GetKey(KeyCode.Space))
        {
            _brake = true;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            _brake = false;
        }
    }



    private void FixedUpdate()
    {
        if (takingInput)
        {
            ApplyRotation(); //newSmootherRotation
            if (!_brake)
            {

                ApplyMovement();

            }
            else
            {
                Brake();
            }
        }
        
        
    }

    void ApplyRotation()
    {
        Vector2 turn = Input.mousePosition;
        Ray desiredDirection = Camera.main.ScreenPointToRay(turn);
        
        if ( Mathf.Abs(turn.x - _screenMidpoint.x) > noRotationMouseZone &&
            Mathf.Abs(turn.y - _screenMidpoint.y) > noRotationMouseZone)
        {
            rb.rotation = Quaternion.Lerp(rb.rotation, Quaternion.LookRotation (desiredDirection.direction), Time.deltaTime * rotationSpeed); // Using transform
        }
        


        
    }
    
    private void ApplyMovement()
    {
        
        if (_thrustActive )
        {
            rb.AddForce(_thrustRatio.z * transform.forward);
            rb.AddForce(_thrustRatio.x * -transform.right);
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);
        }

        if (_turbo )
        {
            
            rb.AddForce(transform.forward * acceleration.z * turboStrength, ForceMode.Force);
            _turbo = false;
            _turboOnCoolDown = true;
            StartCoroutine(TurboCountDown());
        }
        
    }

    IEnumerator TurboCountDown()
    {
        yield return new WaitForSeconds(turboCooldown);
        _turboOnCoolDown = false;

    }

    void Brake()
    {
        
        rb.AddForce(Vector3.MoveTowards(new Vector3(0,0,0),-rb.velocity, brakingStrength * Time.deltaTime), ForceMode.Impulse);
    }

    public void Die()
    {
        Debug.Log("I am a dead thing now");
    }
}
