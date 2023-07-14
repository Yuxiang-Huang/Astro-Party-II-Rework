using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freezer : MonoBehaviour
{
    public int id;
    public int team;

    ScoreManager scoreManagerScript;

    // Start is called before the first frame update
    void Start()
    {
        scoreManagerScript = GameObject.Find("Score Manager").GetComponent<ScoreManager>();
        StartCoroutine("selfDestruct");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ship"))
        {
            bool toFreeze = true;
            if (!scoreManagerScript.friendlyFire)
            {
                if (team == collision.gameObject.GetComponent<MutualShip>().team)
                {
                    toFreeze = false;
                }
            }

            //don't freeze yourself
            if (id == collision.gameObject.GetComponent<MutualShip>().id)
            {
                toFreeze = false;
            }

            if (toFreeze)
            {
                collision.gameObject.GetComponent<MutualShip>().freezeTime += 1f;
            }
        }


        if (collision.gameObject.CompareTag("Pilot"))
        {
            if (collision.gameObject.GetComponent<PilotPlayerController>() != null)
            {
                collision.gameObject.GetComponent<PilotPlayerController>().kill(id, team);
            }
            else
            {
                collision.gameObject.GetComponent<BotPilotMove>().kill(id, team);
            }
        }
    }

    IEnumerator selfDestruct()
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }
}
