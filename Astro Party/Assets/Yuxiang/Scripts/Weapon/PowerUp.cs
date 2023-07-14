using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public string powerUpName;

    GameManager gameManagerScript;

    public int attractDist = 150;

    public int attracSpeed = 10;

    // Start is called before the first frame update
    void Start()
    {
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (List<GameObject> shipList in gameManagerScript.inGameShips)
        {
            foreach (GameObject ship in shipList)
            {
                if (ship.GetComponent<MutualShip>() != null)
                {
                    if (distance(ship, this.gameObject) <= attractDist)
                    {
                        transform.position += (ship.transform.position - transform.position).normalized * attracSpeed;
                    }
                } 
            }
        }
    }

    float distance(GameObject ship1, GameObject ship2)
    {
        return Mathf.Sqrt(Mathf.Pow((ship1.transform.position.x - ship2.transform.position.x), 2) +
            Mathf.Pow((ship1.transform.position.z - ship2.transform.position.z), 2));
    }
}