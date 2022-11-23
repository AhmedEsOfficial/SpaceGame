using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Object = System.Object;


[CreateAssetMenu]
public class ProjectileDescription : ScriptableObject
{
    
    


    [SerializeField] public List<GameObject> modConfigs;


}
