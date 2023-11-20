using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slingshot1script : MonoBehaviour
{
    LineRenderer lineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Lines()
    {
        lineRenderer.startWidth = 0.15f;
        lineRenderer.endWidth = 0.17f;
    }
}
