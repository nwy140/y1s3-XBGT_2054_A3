using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavMeshSurfaceXY2D : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void BuildNav()
    {
        grids.transform.rotation = Quaternion.Euler(90, 0, 0);
        transform.rotation = Quaternion.Euler(0, 0, 0);
        GetComponent<NavMeshSurface>().BuildNavMesh();
        grids.transform.rotation = Quaternion.Euler(0, 0, 0);
        transform.rotation = Quaternion.Euler(-90, 0, 0);
    }
}
