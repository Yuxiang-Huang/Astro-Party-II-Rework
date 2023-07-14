using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map1RotatingObject : MonoBehaviour
{
    public float velocity;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3 (0, velocity, 0));
    }
}
