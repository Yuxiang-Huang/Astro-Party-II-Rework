using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map2CircularMotion : MonoBehaviour
{
    Rigidbody rb;

    public float velocity;

    public float radius;

    public int mode;

    MapManager mapManagerScript;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mapManagerScript = GameObject.Find("Map Manager").GetComponent<MapManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //F = m v^2 / r
        rb.AddForce((new Vector3(0, 0, 0) - transform.position).normalized * rb.mass * velocity * velocity / radius);
    }

    public void reset()
    {
        rb = GetComponent<Rigidbody>();
        mapManagerScript = GameObject.Find("Map Manager").GetComponent<MapManager>();
        velocity = mapManagerScript.velocity;
        
        float root2 = Mathf.Sqrt(2) / 2;

        switch (mode)
        {
            case 0:
                transform.position = new Vector3(radius, 0, 0);
                rb.velocity = new Vector3(0, 0, velocity);
                break;
            case 2:
                transform.position = new Vector3(0, 0, radius);
                rb.velocity = new Vector3(-velocity, 0, 0);
                break;
            case 4:
                transform.position = new Vector3(-radius, 0, 0);
                rb.velocity = new Vector3(0, 0, -velocity);
                break;
            case 6:
                transform.position = new Vector3(0, 0, -radius);
                rb.velocity = new Vector3(velocity, 0, 0);
                break;
            case 1:
                transform.position = new Vector3(root2 * radius, 0, root2 * radius);
                rb.velocity = new Vector3(-root2 * velocity, 0, root2 * velocity);
                break;
            case 3:
                transform.position = new Vector3(-root2 * radius, 0, root2 * radius);
                rb.velocity = new Vector3(-root2 * velocity, 0, -root2 * velocity);
                break;
            case 5:
                transform.position = new Vector3(-root2 * radius, 0, -root2 * radius);
                rb.velocity = new Vector3(root2 * velocity, 0, -root2 * velocity);
                break;
            case 7:
                transform.position = new Vector3(root2 * radius, 0, -root2 * radius);
                rb.velocity = new Vector3(root2 * velocity, 0, root2 * velocity);
                break;
        }
    }
}
