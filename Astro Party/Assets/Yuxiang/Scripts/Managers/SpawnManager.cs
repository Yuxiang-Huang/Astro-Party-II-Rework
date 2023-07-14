using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
    public string mode;
    public Text modeText; 

    public GameObject smallAsteroid;
    public GameObject mediumpAsteroid;
    public GameObject largeAsteroid;
    public GameObject PowerUpAsteroid;

    public GameObject PowerUpAsteroidOrig;

    PowerUpManager powerUpManagerScript;
    GameManager gameManagerScript;

    public bool startSpawn;

    int space = 100;
    int asteroidY = 0;

    int requireNum;
     
    // Start is called before the first frame update
    void Start()
    {
        powerUpManagerScript = GameObject.Find("PowerUp Manager").GetComponent<PowerUpManager>();
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();

        mode = "some";

        InvokeRepeating("intervalUpdate", 0, 5.0f);
    }

    public void intervalUpdate()
    {
        //spawn when started
        if (startSpawn)
        {
            bool spawn = true;

            //check for number of asteroids
            if (gameManagerScript.inGameAsteroids.Count > requireNum)
            {
                spawn = false;
            }

            //check for if there is one power up asteroid
            if (powerUpManagerScript.indicators.Count > 0 && !spawn)
            {
                spawn = true;
                foreach (GameObject asteroid in gameManagerScript.inGameAsteroids)
                {
                    if (asteroid.GetComponent<Asteroid>().type == "powerUp")
                    {
                        spawn = false;
                    }
                }
            }

            if (spawn)
            {
                spawnAsteroids();
            }
        }
    }

    public void RoundSpawn()
    {
        if (powerUpManagerScript.indicators.Count > 0)
        {
            int ran = Random.Range(0, powerUpManagerScript.indicators.Count);

            GameObject powerUp = Instantiate(powerUpManagerScript.indicators[ran],
                new Vector3(0, powerUpManagerScript.powerUpyValue, 0),
                powerUpManagerScript.indicators[ran].transform.rotation);

            gameManagerScript.inGameIndicators.Add(powerUp);

            PowerUpAsteroid = PowerUpAsteroidOrig;
        }
        else
        {
            //if no indicators, no powerUp asteroid
            switch (Random.Range(0, 3))
            {
                case 0: PowerUpAsteroid = smallAsteroid; break;
                case 1: PowerUpAsteroid = mediumpAsteroid; break;
                case 2: PowerUpAsteroid = largeAsteroid; break;
            }
        }

        //set requireNum
        requireNum = 3;

        switch (mode)
        {
            case "none": requireNum = 0; break;
            case "few": requireNum = 1; break;
            case "many": requireNum = 5; break;
        }

        startSpawn = true;

        intervalUpdate();
    }

    void spawnAsteroids()
    {
        if (mode != "none")
        {
            //definitely a powerUp asteroid
            GameObject asteroidClone = Instantiate(PowerUpAsteroid, generateRanPos(asteroidY), PowerUpAsteroid.transform.rotation);

            gameManagerScript.inGameAsteroids.Add(asteroidClone);

            //spawn required number of random asteroids
            while (gameManagerScript.inGameAsteroids.Count < requireNum + 2)
            {
                GameObject asteroid = PowerUpAsteroid;

                int ran = Random.Range(0, 4);

                switch (ran)
                {
                    case 0: asteroid = smallAsteroid; break;
                    case 1: asteroid = mediumpAsteroid; break;
                    case 2: asteroid = largeAsteroid; break;
                    case 3: asteroid = PowerUpAsteroid; break;
                }

                asteroidClone = Instantiate(asteroid, generateRanPos(asteroidY), PowerUpAsteroid.transform.rotation);

                gameManagerScript.inGameAsteroids.Add(asteroidClone);
            }
        }
    }

    float distance(Vector3 ship1, Vector3 ship2)
    {
        return Mathf.Sqrt(Mathf.Pow((ship1.x - ship2.x), 2) + Mathf.Pow((ship1.z - ship2.z), 2));
    }

    //generate a random position at least space away from any ship with a specific y value
    public Vector3 generateRanPos(int y)
    {
        Vector3 ranPos = new Vector3(Random.Range(-gameManagerScript.spawnRadius, gameManagerScript.spawnRadius), y,
          Random.Range(-gameManagerScript.spawnRadius, gameManagerScript.spawnRadius));

        //outside the circle
        bool flag = true;
        while (flag)
        {
            flag = false;

            ranPos = new Vector3(Random.Range(-gameManagerScript.spawnRadius, gameManagerScript.spawnRadius), y,
Random.Range(-gameManagerScript.spawnRadius, gameManagerScript.spawnRadius));

            foreach (List<GameObject> shipList in gameManagerScript.inGameShips)
            {
                foreach (GameObject ship in shipList)
                {
                    if (ship != null)
                    {
                        if (distance(ship.transform.position, ranPos) < space)
                        {
                            flag = true;
                        }
                    }
                }
            }

            if (distance(ranPos, new Vector3(0, 0, 0)) > gameManagerScript.spawnRadius)
            {
                flag = true;
            }
        }
        return ranPos;
    }

    public void changeMode()
    {
        switch (mode)
        {
            case "some": mode = "many"; modeText.text = "Asteroids: Many"; break;
            case "many": mode = "none"; modeText.text = "Asteroids: None"; break;
            case "none": mode = "few"; modeText.text = "Asteroids: Few"; break;
            case "few": mode = "some"; modeText.text = "Asteroids: Some"; break;
        }
    }
}
