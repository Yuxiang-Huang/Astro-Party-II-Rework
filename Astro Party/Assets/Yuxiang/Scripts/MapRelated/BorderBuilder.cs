using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderBuilder : MonoBehaviour
{
    public GameObject borderWall;

    // Start is called before the first frame update
    void Start()
    {
        ringBuild();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ringBuild()
    {
        GameObject pivot = new GameObject();
        pivot.transform.position = new Vector3(0, 0, 0);

        int n = 1000;

        float radius = 750;

        for (int i = 0; i < n; i++)
        {
            float angle = 2 * Mathf.PI / n * i;

            GameObject now = Instantiate(borderWall, new Vector3(radius * Mathf.Cos(angle), 0, radius * Mathf.Sin(angle)),
                borderWall.transform.rotation);
            now.transform.Rotate(new Vector3(0, 180 * angle / Mathf.PI, 0));

            now.transform.parent = pivot.transform;
        }
    }
}
