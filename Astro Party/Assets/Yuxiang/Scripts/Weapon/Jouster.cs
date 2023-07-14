using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jouster : MonoBehaviour
{
    public int id;
    public int team;
    public int health;

    private void Awake()
    {
        id = GetComponentInParent<MutualShip>().id;
        id = GetComponentInParent<MutualShip>().team;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Asteroid"))
        {
            health -= other.GetComponent<Asteroid>().health;
            other.GetComponent<Asteroid>().health = 0;
        }

        if (other.CompareTag("Breakable"))
        {
            other.gameObject.SetActive(false);
            health -= 1;
        }

        if (other.CompareTag("Pilot"))
        {
            if (other.GetComponent<PilotPlayerController>() != null)
            {
                other.GetComponent<PilotPlayerController>().kill(id, team);
            }
            else
            {
                other.GetComponent<BotPilotMove>().kill(id, team);
            }
        }

        ////double ontrigger issue
        //if (other.CompareTag("Ship"))
        //{
        //    other.GetComponent<MutualShip>().damage(id, team);
        //    health = 0;
        //}

        if (health <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
