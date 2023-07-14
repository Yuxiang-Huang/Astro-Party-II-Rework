using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fog : MonoBehaviour
{
    public string direction;

    int outOfBoundLen = 850;

    public int speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (outOfBound(transform.position)) {
            Destroy(gameObject);
        }

        Vector3 blow = new Vector3 (0, 0, 0);

        switch (direction)
        {
            case "right": blow = new Vector3(-speed, 0, 0); break;
            case "left": blow = new Vector3(speed, 0, 0); break;
            case "down": blow = new Vector3(0, 0, speed); break;
            case "up": blow = new Vector3(0, 0, -speed); break;
        }

        transform.position = transform.position + blow;
    }

    bool outOfBound(Vector3 pos)
    {
        return pos.x > outOfBoundLen || pos.z > outOfBoundLen ||
            pos.x < -outOfBoundLen || pos.z < -outOfBoundLen;
    }
}
