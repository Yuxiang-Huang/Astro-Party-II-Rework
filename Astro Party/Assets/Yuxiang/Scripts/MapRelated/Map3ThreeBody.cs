using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map3ThreeBody : MonoBehaviour
{
    public int threeBodyID;
    public GameObject otherOne;
    public GameObject otherTwo;
    Rigidbody rb;

    public float G;

    public int radius = 500;
    public int randomSpeed;

    MapManager mapManagerScript;

    private void Awake()
    {
        mapManagerScript = GameObject.Find("Map Manager").GetComponent<MapManager>();
        rb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    { 
        rb.velocity += new Vector3 ( (otherOne.transform.position.x - transform.position.x) /
            Mathf.Pow(distance(otherOne.transform.position, transform.position), 2) * G, 0,
            (otherOne.transform.position.z - transform.position.z) /
            Mathf.Pow(distance(otherOne.transform.position, transform.position), 2) * G);
    }

    float distance(Vector3 ship1, Vector3 ship2)
    {
        return Mathf.Sqrt(Mathf.Pow((ship1.x - ship2.x), 2) +
            Mathf.Pow((ship1.z - ship2.z), 2));
    }

    public void reset()
    {
        if (mapManagerScript.isFixedSpawned(3)){
            //fixed position
            switch (threeBodyID)
            {
                case 1:
                    transform.position = new Vector3(0, 0, radius);
                    //rb.velocity = new Vector3(0, 0, randomSpeed);
                    break;
                case 2:
                    transform.position = new Vector3(radius * Mathf.Sqrt(3) / 2, 0, -radius / 2);
                    //rb.velocity = new Vector3(0, 0, -randomSpeed);
                    break;
                case 3:
                    transform.position = new Vector3(-radius * Mathf.Sqrt(3) / 2, 0, -radius / 2);
                    //rb.velocity = new Vector3(0, 0, -randomSpeed);
                    break;
            }
        }
        else
        {
            //random position
            float ranAngle = Random.Range(0, Mathf.PI * 2);
            float ranRadius = Random.Range(10, radius);
            transform.position = new Vector3(Mathf.Cos(ranAngle) * ranRadius, 0, Mathf.Sin(ranAngle) * ranRadius);
        }

        rb.velocity = new Vector3(Random.Range(-randomSpeed, randomSpeed), 0, Random.Range(-randomSpeed, randomSpeed));
    }
}
