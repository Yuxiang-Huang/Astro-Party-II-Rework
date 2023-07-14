using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PilotPlayerController : MonoBehaviour
{
    public int id;
    public int team;

    int speed = 500;
    int maxVelocity = 300;
    float rotatingSpeed = 2f;
    bool rotating;
    bool moving;

    public KeyCode turn = KeyCode.A;
    public KeyCode move = KeyCode.D;

    public Rigidbody playerRb;

    GameManager gameManagerScript;
    ScoreManager scoreManagerScript;
    SEManager SEManagerScript;

    public Renderer bodyRend;
    public Renderer headRend;
    public Material blue1;
    public Material red2;
    public Material yellow3;
    public Material cyan4;
    public Material green5;

    public bool attacked;

    // Start is called before the first frame update
    void Start()
    {
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
        scoreManagerScript = GameObject.Find("Score Manager").GetComponent<ScoreManager>();
        SEManagerScript = GameObject.Find("SoundEffect Manager").GetComponent<SEManager>();

        playerRb = GetComponent<Rigidbody>();

        switch (id)
        {
            case 1:
                bodyRend.material = blue1;
                headRend.material = blue1;
                break;
            case 2:
                bodyRend.material = red2;
                headRend.material = red2;
                break;
            case 3:
                bodyRend.material = yellow3;
                headRend.material = yellow3;
                break;
            case 4:
                bodyRend.material = cyan4;
                headRend.material = cyan4;
                break;
            case 5:
                bodyRend.material = green5;
                headRend.material = green5;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(move))
        {
            moving = true;
        }
        if (Input.GetKeyUp(move))
        {
            moving = false;
        }
        if (moving)
        {
            playerRb.AddRelativeForce(new Vector3(0, speed, 0), ForceMode.Force);
        }
        if (playerRb.velocity.magnitude > maxVelocity)
        {
            //Debug.Log(playerRb.velocity);
            playerRb.velocity = playerRb.velocity.normalized * maxVelocity;
        }

        if (Input.GetKeyDown(turn))
        {
            rotating = true;
        }
        if (Input.GetKeyUp(turn))
        {
            rotating = false;
        }

        if (rotating)
        {
            playerRb.freezeRotation = false;
            transform.Rotate(0, 0, -rotatingSpeed);
            playerRb.freezeRotation = true;
        }
    }

    IEnumerator respawn()
    {
        yield return new WaitForSeconds(7f);

        GameObject targetShip = null;

        foreach (List<GameObject> shipList in gameManagerScript.ships)
        {
            foreach (GameObject ship in shipList)
            {
                if (ship.GetComponent<MutualShip>().id == id)
                {
                    targetShip = ship;
                }
            }
        }

        GameObject myShip = Instantiate(targetShip, transform.position, transform.rotation);
        myShip.SetActive(true);
        myShip.transform.Rotate(-90, 0, 0);
        myShip.GetComponent<MutualShip>().team = team;
        myShip.GetComponent<MutualShip>().id = id;

        gameManagerScript.inGameShips[team].Add(myShip);
        gameManagerScript.inGameShips[team].Remove(this.gameObject);

        Destroy(this.gameObject);
    }

    public void kill(int otherID, int otherTeam)
    {
        bool toKill = true;
        //Friendly Fire 
        if (!scoreManagerScript.friendlyFire)
        {
            if (team == otherTeam)
            {
                toKill = false;
            }
        }

        if (attacked)
        {
            toKill = false;
        }

        if (toKill)
        {
            attacked = true;

            if (scoreManagerScript.gameMode == "pilot")
            {
                //sound effect
                SEManagerScript.generalAudio.PlayOneShot(SEManagerScript.pilotDeath);


                if (otherID == id)
                {
                    suicide();
                }
                else
                {
                    earnPoint(otherID);
                }
            }

            if (scoreManagerScript.gameMode != "highlight")
            { 
                Destroy(this.gameObject);
            }
        }
    }

    void earnPoint(int ID)
    {
        if (scoreManagerScript.SoloOrTeam == "solo")
        {
            //suicide
            if (ID == -1)
            {
                switch (id)
                {
                    case 1:
                        scoreManagerScript.P1Suicide = true;
                        break;
                    case 2:
                        scoreManagerScript.P2Suicide = true;
                        break;
                    case 3:
                        scoreManagerScript.P3Suicide = true;
                        break;
                    case 4:
                        scoreManagerScript.P4Suicide = true;
                        break;
                    case 5:
                        scoreManagerScript.P5Suicide = true;
                        break;
                }
            }

            else
            {
                switch (ID)
                {
                    case 1:
                        scoreManagerScript.P1Score++;
                        break;
                    case 2:
                        scoreManagerScript.P2Score++;
                        break;
                    case 3:
                        scoreManagerScript.P3Score++;
                        break;
                    case 4:
                        scoreManagerScript.P4Score++;
                        break;
                    case 5:
                        scoreManagerScript.P5Score++;
                        break;
                }
            }
        }
    }

    void suicide()
    {
        if (scoreManagerScript.SoloOrTeam == "solo")
        {
            switch (id)
            {
                case 1:
                    scoreManagerScript.P1Suicide = true;
                    break;
                case 2:
                    scoreManagerScript.P2Suicide = true;
                    break;
                case 3:
                    scoreManagerScript.P3Suicide = true;
                    break;
                case 4:
                    scoreManagerScript.P4Suicide = true;
                    break;
                case 5:
                    scoreManagerScript.P5Suicide = true;
                    break;
            }
        }
    }
}