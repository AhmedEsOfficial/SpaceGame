using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    public Vector3 maxVelocity;
    public float rotationSpeed;
    public float noRotationMouseZone;
    public float turboCooldown;
    public float turboStrength;
    public float brakingStrength;
    
    
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
    [FormerlySerializedAs("_takingInput")] public bool takingInput;

    private bool _zRotationActive;
    // Start is called before the first frame update
    void Start()
    {
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
            _thrustRatio.z = maxVelocity.z;
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            _thrustActive = false;
            _thrustRatio.z = 0;

        }
        if (Input.GetKey(KeyCode.S))
        {
            _thrustActive = true;
            _thrustRatio.z = -maxVelocity.z ;
        }
        
        if (Input.GetKeyUp(KeyCode.S))
        {
            _thrustActive = false;
            _thrustRatio.z = 0;
        }
        
        if (Input.GetKey(KeyCode.A))
        {
            _thrustActive = true;
            _thrustRatio.x = maxVelocity.x;
            
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            _thrustActive = false;
            _thrustRatio.x = 0;

        }
        if (Input.GetKey(KeyCode.D))
        {
            _thrustActive = true;
            _thrustRatio.x = -maxVelocity.x ;
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
            if (!_brake)
            {
                //LookAtMouse(); old rotation
                ApplyRotation(); //newSmootherRotation

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
        transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (desiredDirection.direction), Time.deltaTime * 5f); // Using transform

        /*
         Using physics
        if (Mathf.Abs(turn.x - _screenMidpoint.x ) > noRotationMouseZone && Mathf.Abs(turn.y - _screenMidpoint.y) > noRotationMouseZone)
        {
            Vector3 oldPoint = transform.forward;
            Vector3 newPoint = -desiredDirection.direction;
            CalculateTorque(oldPoint, newPoint, 1);
        }
        */ 
    }
    private void LookAtMouse()
    {
        Vector2 turn= Input.mousePosition;
        turn.x = turn.x- _screenMidpoint.x;
        turn.y = turn.y- _screenMidpoint.y;
        
        
        //The following calculates the torque by reversing the equation for it in order to rotate from one direction to another in a physically realistic way

        if (Mathf.Abs(turn.x) > noRotationMouseZone && Mathf.Abs(turn.y) > noRotationMouseZone)
        {
            Vector3 oldPoint = transform.right;
            Vector3 newPoint = transform.forward;
            if (!float.IsNaN(turn.x))
            {
                CalculateTorque(oldPoint, newPoint, turn.x);

            }


            oldPoint = transform.up;
            newPoint = -transform.forward;
            if (!float.IsNaN(turn.y))
            {
                CalculateTorque(oldPoint, newPoint, -turn.y);

            }
            
            

        }
        
        if (_zRotationActive)
        {
            
            Vector3 oldPoint = transform.forward;
            Vector3 newPoint = transform.up;
            CalculateTorque(oldPoint, newPoint, turn.y);
        }
        
        





    }

    void CalculateTorque(Vector3 oldPoint, Vector3 newPoint, float direction)
    {
        Vector3 x = Vector3.Cross(oldPoint.normalized, newPoint.normalized);
        float theta = Mathf.Asin(x.magnitude);
        Vector3 w = x.normalized *theta * Time.fixedDeltaTime;
        
        Quaternion q = transform.rotation * rb.inertiaTensorRotation;
        Vector3 T = q * Vector3.Scale(rb.inertiaTensor, (Quaternion.Inverse(q) * w));
        ApplyTorque(T,direction);
        
       
    }


    private void ApplyTorque(Vector3 T, float direction)
    {
        if (!float.IsNaN(T.x) && !float.IsNaN(T.y)&& !float.IsNaN(T.z))
        {
            if (direction < 0)
            {
                rb.AddTorque(T*rotationSpeed);

            }
            else
            {
                rb.AddTorque(-T*rotationSpeed);

            }
        }
    }
    

    private void ApplyMovement()
    {
        
        if (_thrustActive )
        {
            rb.AddForce(_thrustRatio.z * transform.forward);
            rb.AddForce(_thrustRatio.x * -transform.right);
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, 400);
        }

        if (_turbo )
        {
            
            rb.AddForce(transform.forward * maxVelocity.z * turboStrength, ForceMode.Impulse);
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
        rb.AddTorque(Vector3.MoveTowards(new Vector3(0,0,0), -rb.angularVelocity, brakingStrength * Time.deltaTime), ForceMode.Impulse);
    }
}
