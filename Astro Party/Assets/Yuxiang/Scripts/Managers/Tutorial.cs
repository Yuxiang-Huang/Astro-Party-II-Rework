using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tutorial : MonoBehaviour
{
    public GameObject startScreen;
    public GameObject prepScreen;
    public GameObject directionScreen;
    public GameObject endScreen;

    public GameObject endScreenText;
    public GameObject pauseText;

    public GameObject playerShip;

    public GameObject tutorialMap;

    public List<GameObject> directions;
    int directionId = 0;
    public GameObject endButton;
    public GameObject lastDirectionButton;
    public GameObject nextDirectionButton;

    bool started;
    int spawnRadius = 700;

    public int shipId;
    public KeyCode shipRotate;
    public KeyCode shipShoot;

    public GameObject cube0;
    public GameObject cube1;
    public List<GameObject> threeBody;
    public List<GameObject> asteroids;
    public GameObject bot;
    public GameObject bot2;
    public GameObject powerUpScreen;

    GameManager gameManagerScript;
    CameraManager cameraMangerScript;

    bool notFirst;
    int numOfDirectionBack;

    // Start is called before the first frame update
    void Start()
    {
        shipRotate = KeyCode.A;
        shipShoot = KeyCode.S;

        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
        cameraMangerScript = GameObject.Find("Main Camera").GetComponent<CameraManager>();

        reset();
    }

    // Update is called once per frame
    void Update()
    {
        if (started)
        {
            if (distance(playerShip.transform.position, new Vector3(0, 0, 0)) > spawnRadius)
            {
                float angle = Mathf.Atan2(playerShip.transform.position.x, playerShip.transform.position.z);

                playerShip.transform.position =
                    new Vector3(spawnRadius * Mathf.Sin(angle), 10, spawnRadius * Mathf.Cos(angle));
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (endScreen.activeSelf)
                {
                    endScreen.SetActive(false);
                    Time.timeScale = 1;
                }
                else
                {
                    endScreen.SetActive(true);
                    Time.timeScale = 0;
                }
            }
        }

        //player dead
        if (started && gameManagerScript.inGameShips[0].Count == 0)
        {
            endScreen.SetActive(true);
            endScreenText.SetActive(true);
            pauseText.SetActive(false);
            gameManagerScript.endRound();
        }
    }

    public void prep()
    {
        startScreen.SetActive(false);
        prepScreen.SetActive(true);
    }

    public void backToStart()
    {
        prepScreen.SetActive(false);
        startScreen.SetActive(true);
    }

    public void startTutorial()
    {
        prepScreen.SetActive(false);
        tutorialMap.SetActive(true);

        GameObject shipPlayer = Instantiate(playerShip, new Vector3(0, 10, 0),
playerShip.transform.rotation);
        shipPlayer.GetComponent<MutualShip>().id = shipId;
        shipPlayer.GetComponent<PlayerController>().turn = shipRotate;
        shipPlayer.GetComponent<PlayerController>().shoot = shipShoot;
        gameManagerScript.inGameShips[0].Add(shipPlayer);

        directionScreen.SetActive(true);
        directions[0].gameObject.SetActive(true);
        started = true;
    }

    public void nextDirection()
    {
        if (directionId == 0)
        {
            lastDirectionButton.SetActive(true);
        }

        directions[directionId].SetActive(false);
        directionId++;
        directions[directionId].SetActive(true);

        if (directionId == directions.Count - 1)
        {
            nextDirectionButton.SetActive(false);
        }

        if (directionId == 5)
        {
            powerUpScreen.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            powerUpScreen.SetActive(false);
            Time.timeScale = 1;
        }

        //to prevent multiple calls
        if (numOfDirectionBack == 0)
        {
            //special directions
            if (directionId == 3)
            {
                foreach (GameObject asteroid in asteroids)
                {
                    GameObject curr = Instantiate(asteroid, generateRanPos(), asteroid.transform.rotation);
                    gameManagerScript.inGameAsteroids.Add(curr);
                }
            }

            if (directionId == 6)
            {
                cube0.transform.position = generateRanPos();
                cube1.transform.position = generateRanPos();
                cube0.SetActive(true);
                cube1.SetActive(true);
            }

            if(directionId == 7)
            {
                foreach (GameObject body in threeBody)
                {
                    spawnBody(body);
                }   
            }
            else
            {
                foreach (GameObject body in threeBody)
                {
                    body.SetActive(false);
                }
            }

            if (directionId == 8)
            {
                GameObject toAddBot1 = Instantiate(bot, generateRanPos(), bot.transform.rotation);
                gameManagerScript.inGameShips[1].Add(toAddBot1);

                GameObject toAddBot2 = Instantiate(bot2, generateRanPos(), bot2.transform.rotation);
                gameManagerScript.inGameShips[1].Add(toAddBot2);

                cameraMangerScript.StartCoroutine("delayStart");

                int id = playerShip.GetComponent<MutualShip>().id + 1;
                if (id == 6)
                {
                    id = 1;
                }
                toAddBot1.GetComponent<MutualShip>().id = id;
                toAddBot2.GetComponent<MutualShip>().id = id;
                toAddBot1.GetComponent<MutualShip>().team = id;
                toAddBot2.GetComponent<MutualShip>().team = id;

                endButton.SetActive(true);
            }
        }

        if (numOfDirectionBack < 0)
        {
            numOfDirectionBack++;
        }
    }

    void spawnBody(GameObject body)
    {
        body.transform.position = generateRanPos();

        body.GetComponent<Rigidbody>().velocity =
            new Vector3(Random.Range(-50, 50), 0, Random.Range(-50, 50));

        body.SetActive(true);
    }

    Vector3 generateRanPos()
    {
        Vector3 ranPos = new Vector3(Random.Range(-spawnRadius, spawnRadius), -10,
          Random.Range(-spawnRadius, spawnRadius));

        while (distance(ranPos, new Vector3(0, 0, 0)) > spawnRadius ||
            distance(playerShip.transform.position, ranPos) < 100)
        {
            ranPos = new Vector3(Random.Range(-spawnRadius, spawnRadius), -10,
Random.Range(-spawnRadius, spawnRadius));
        }

        return ranPos;
    }

    float distance(Vector3 ship1, Vector3 ship2)
    {
        return Mathf.Sqrt(Mathf.Pow((ship1.x - ship2.x), 2) + Mathf.Pow((ship1.z - ship2.z), 2));
    }

    public void lastDirection()
    {
        if (directionId == directions.Count - 1)
        {
            nextDirectionButton.SetActive(true);
        }

        directions[directionId].SetActive(false);
        directionId--;
        directions[directionId].SetActive(true);

        //powerup info screen, special case
        if (directionId == 5)
        {
            powerUpScreen.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            powerUpScreen.SetActive(false);
            Time.timeScale = 1;
        }

        if (directionId == 0)
        {
            lastDirectionButton.SetActive(false);
        }

        numOfDirectionBack--;
    }

    public void reset()
    {
        //managers not set at first
        if (notFirst)
        {
            gameManagerScript.endRound();
        }
        else
        {
            notFirst = true;
        }

        //screens and maps
        prepScreen.SetActive(false);
        directionScreen.SetActive(false);
        endScreen.SetActive(false);
        tutorialMap.SetActive(false);
        startScreen.SetActive(true);

        //directions
        directionId = 0;
        nextDirectionButton.SetActive(true);
        lastDirectionButton.SetActive(false);
        endButton.SetActive(false);
        foreach (GameObject direction in directions)
        {
            direction.SetActive(false);
        }
        numOfDirectionBack = 0;

        cube0.SetActive(false);
        cube1.SetActive(false);
        foreach (GameObject body in threeBody)
        {
            body.SetActive(false);
        }

        //others
        started = false;
        cameraMangerScript.started = false;
        endScreenText.SetActive(false);
        pauseText.SetActive(true);
        Time.timeScale = 1;
    }
}