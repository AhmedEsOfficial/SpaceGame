using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public References references;
    

    public List<MouseButton> MouseButtons;
    public List<SpaceGun> SpaceGuns;
    public Dictionary<MouseButton, SpaceGun> gunInputDict;


    // Start is called before the first frame update
    void Start()
    {
        gunInputDict = new Dictionary<MouseButton, SpaceGun>();
        for (int i = 0; i < MouseButtons.Count; i++)
        {
            gunInputDict.Add(MouseButtons[i], SpaceGuns[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var mb in gunInputDict.Keys)
        {
            if(Input.GetMouseButton((int) mb))
            {
                gunInputDict[mb].Shoot();
            }
            if(Input.GetMouseButtonUp((int) mb))
            {
                gunInputDict[mb].StopShooting();
            }
        }
        
    }
}
