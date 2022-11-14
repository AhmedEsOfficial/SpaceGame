using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class References : MonoBehaviour
{
    public GameObject player;
    public GameObject camera;
    private PlayerController _playerController;
    private Camera _camera;
    private CameraController _cameraController;

    
    
     

    public List<Rigidbody> activeBulletsInScene;
    private void Awake()
    {
        _playerController = player.GetComponent<PlayerController>();
        _cameraController = camera.GetComponent<CameraController>();
    }

    public PlayerController GetPlayerController()
    {
        return _playerController;
    }
}
