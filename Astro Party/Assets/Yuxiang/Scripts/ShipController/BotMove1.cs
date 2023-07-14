using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BotMove1: MonoBehaviour
{
    //[SerializeField] public NavMeshAgent agent;

    float botReloadTime;
    float botTurnTime;

    GameManager gameManagerScript;
    ScoreManager scoreManagerScript;

    public bool disable;

    public int speed = 500;
    public int maxVelocity = 300;
    Rigidbody rb;

    public float threshold = 0.1f;

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

            //moving
            rb.AddRelativeForce(new Vector3(0, 0, speed), ForceMode.Force);

            if (rb.velocity.magnitude > maxVelocity)
            {
                rb.velocity = rb.velocity.normalized * maxVelocity;
            }

            //rotating
            //Debug.Log(Mathf.Atan2(target.transform.position.z - transform.position.z,
            //    target.transform.position.x - transform.position.x));
            //Debug.Log(transform.rotation.ToEulerAngles().y);

            if (botTurnTime <= 0)
            {
                transform.LookAt(target.transform);
                botTurnTime = Random.Range(0, 0.5f);
            }
            else
            {
                botTurnTime -= Time.deltaTime;
            }

            //shooting
            botReloadTime += Time.deltaTime;

            if (botReloadTime >= 1)
            {
                botReloadTime = 0;
                GetComponent<MutualShip>().fire();
            }
        }
    }

    float distance(GameObject ship1, GameObject ship2)
    {
        return Mathf.Sqrt(Mathf.Pow((ship1.transform.position.x - ship2.transform.position.x), 2) +
            Mathf.Pow((ship1.transform.position.z - ship2.transform.position.z), 2));
    }
}