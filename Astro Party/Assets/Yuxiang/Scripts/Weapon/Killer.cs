using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killer : MonoBehaviour
{
    public int id = -1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            collision.gameObject.GetComponent<Asteroid>().health = 0;
        }

        if (collision.gameObject.CompareTag("Breakable"))
        {
            collision.gameObject.SetActive(false);
        }

        if (collision.gameObject.CompareTag("Pilot"))
        {
            if (collision.gameObject.GetComponent<PilotPlayerController>() != null)
            {
                collision.gameObject.GetComponent<PilotPlayerController>().kill(id, -1);
            }
            else
            {
                collision.gameObject.GetComponent<BotPilotMove>().kill(id, -1);
            }
        }

        if (collision.gameObject.CompareTag("Ship"))
        {
            collision.gameObject.GetComponent<MutualShip>().damage(id, -1);
        }

        if (collision.gameObject.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject);
        }
    }

    IEnumerator selfDestruct()
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }
}
