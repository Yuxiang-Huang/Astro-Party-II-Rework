using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    Camera myCamera;
    GameManager gameManagerScript;

    public int space = 50;

    public float itchScreenFactor = 1.6f;

    public bool started;
    public bool startLock;

    public bool SPU;

    // Start is called before the first frame update
    void Start()
    {
        myCamera = GetComponent<Camera>();
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    void LateUpdate()
    {
        if (started)
        {
            float minX = gameManagerScript.spawnRadius;
            float maxX = -gameManagerScript.spawnRadius;
            float minZ = gameManagerScript.spawnRadius;
            float maxZ = -gameManagerScript.spawnRadius;

            foreach (List<GameObject> shipList in gameManagerScript.inGameShips)
            {
                foreach (GameObject ship in shipList)
                {
                    if (ship != null)
                    {
                        minX = Mathf.Min(minX, ship.transform.position.x) - space;
                        maxX = Mathf.Max(maxX, ship.transform.position.x) + space;
                        minZ = Mathf.Min(minZ, ship.transform.position.z) - space;
                        maxZ = Mathf.Max(maxZ, ship.transform.position.z) + space;
                    }
                }
            }

            float lenX = (maxX - minX) / 4;
            float lenZ = (maxZ - minZ) / 4;

            minX = Mathf.Max(minX - lenX, -gameManagerScript.spawnRadius - space);
            maxX = Mathf.Min(maxX + lenX, gameManagerScript.spawnRadius + space);

            minZ = Mathf.Max(minZ - lenZ, -gameManagerScript.spawnRadius - space);
            maxZ = Mathf.Min(maxZ + lenZ, gameManagerScript.spawnRadius + space);

            myCamera.orthographicSize = Mathf.Max(500, Mathf.Max((maxX - minX) / itchScreenFactor, maxZ - minZ)) / 2;

            transform.position = new Vector3((minX + maxX) / 2, transform.position.y, (minZ + maxZ) / 2);

            //Debug.Log("minX after: " + minX);
            //Debug.Log("minZ: " + minZ);
            //Debug.Log("maxX: " + maxX);
            //Debug.Log("maxZ: " + maxZ);
        }
        else
        {
            myCamera.orthographicSize = 800;
            transform.position = new Vector3(0, transform.position.y, 0);
        }
    }

    //used in gameManager startRound
    IEnumerator delayStart()
    {
        //reset
        myCamera.orthographicSize = 800;
        transform.position = new Vector3(0, transform.position.y, 0);

        //longer for starting power up
        if (SPU)
        {
            yield return new WaitForSeconds(3.0f);
        }
        else
        {
            yield return new WaitForSeconds(2.0f);
        }

        //for map 6
        if (startLock)
        {
            startLock = false;
        }
        else
        {
            started = true;
        }
    }
}
