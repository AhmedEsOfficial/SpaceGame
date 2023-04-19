using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGridSystem : MonoBehaviour
{
    // Start is called before the first frame update
    public int gridX, gridY, gridZ;
    public Vector3 gridOrigin;
    public GameObject s;
    public float offset;
    void Start()
    {
        for (float x = gridOrigin.x; x < gridX * offset; x += offset)
        {
            for (float y = gridOrigin.y; y < gridY * offset; y += offset)
            {
                for (float z = gridOrigin.z; z < gridZ * offset; z += offset)
                {
                    GameObject cell = Instantiate(s,transform);
                    cell.transform.position = new Vector3(x, y, z);
                }
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
