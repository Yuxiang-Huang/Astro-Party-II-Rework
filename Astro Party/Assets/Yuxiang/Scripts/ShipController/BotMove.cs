using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BotMove : MonoBehaviour
{
    [SerializeField] public NavMeshAgent agent;

    float botReloadTime;

    GameManager gameManagerScript;
    ScoreManager scoreManagerScript;

    public float traceTime;

    public bool disable;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        gameManagerScript = gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
        scoreManagerScript = GameObject.Find("Score Manager").GetComponent<ScoreManager>();

        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //no velocity for AI agent
        rb.velocity = new Vector3(0, 0, 0);

        if (!disable)
        {
            GameObject target = this.gameObject;
            float minDistance = 10000;

            foreach (List<GameObject> shipList in gameManagerScript.inGameShips)
            {
                foreach (GameObject ship in shipList)
                {
                    bool trace = true;

                    //don't trace teammates

                    if (ship.GetComponent<MutualShip>() != null)
                    {
                        if (ship.GetComponent<MutualShip>().team == GetComponent<MutualShip>().team)
                        {
                            trace = false;
                        }
                        else
                        {
                            if (ship.GetComponent<MutualShip>().highlighed)
                            {
                                target = ship;
                                minDistance = -1;
                            }
                        }
                    }

                    if (ship.GetComponent<BotPilotMove>() != null)
                    {
                        if (ship.GetComponent<BotPilotMove>().team == GetComponent<MutualShip>().team)
                        {
                            trace = false;
                        }

                        if (scoreManagerScript.gameMode == "highlight")
                        {
                            trace = false;
                        }
                    }

                    if (ship.GetComponent<PilotPlayerController>() != null)
                    {
                        if (ship.GetComponent<PilotPlayerController>().team == GetComponent<MutualShip>().team)
                        {
                            trace = false;
                        }

                        if (scoreManagerScript.gameMode == "highlight")
                        {
                            trace = false;
                        }
                    }

                    if (trace)
                    {
                        if (distance(ship, this.gameObject) < minDistance)
                        {
                            target = ship;
                            minDistance = distance(ship, this.gameObject);
                        }
                    }
                }
                
            }

            //shooting
            botReloadTime += Time.deltaTime;

            if (botReloadTime >= 1)
            {
                botReloadTime = 0;
                GetComponent<MutualShip>().fire();
            }

            //Can't trace too frequently
            if (traceTime <= 0)
            {
                agent.SetDestination(target.transform.position);
                traceTime = Random.Range(0.75f, 1.25f);
            }
            if (traceTime > 0)
            {
                traceTime -= Time.deltaTime;
            }
        }
    }

    float distance(GameObject ship1, GameObject ship2)
    {
        return Mathf.Sqrt(Mathf.Pow((ship1.transform.position.x - ship2.transform.position.x), 2) +
            Mathf.Pow((ship1.transform.position.z - ship2.transform.position.z), 2));
    }
}