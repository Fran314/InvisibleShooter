using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallsDetector : MonoBehaviour
{
    public int is_coliding = 0;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        is_coliding++;
    }

    private void OnTriggerExit(Collider other)
    {
        is_coliding--;
    }
}
