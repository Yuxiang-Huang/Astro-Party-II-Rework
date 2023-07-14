using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public string type;

    PowerUpManager powerUpManagerScript;
    GameManager gameManagerScript;

    public int health;

    SpawnManager spawnManagerScript;

    public GameObject powerUp;

    Rigidbody rb;

    int force = 100;

    // Start is called before the first frame update
    void Start()
    {
        powerUpManagerScript = GameObject.Find("PowerUp Manager").GetComponent<PowerUpManager>();
        gameManagerScript = gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
        spawnManagerScript = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();

        rb = GetComponent<Rigidbody>();

        switch (type)
        {
            case "large": health = 1; break;

            case "medium": health = 2; break;

            case "small": health = 1; break;

            case "powerUp": health = 1;
                powerUp = powerUpManagerScript.indicators[Random.Range(0, powerUpManagerScript.indicators.Count)];
                break;
        }

        rb.AddForce(new Vector3(Random.Range(-force, force), 0, Random.Range(-force, force)), ForceMode.VelocityChange);
    }

    private void Update()
    {
        if (health <= 0)
        {
            //SEManagerScript.generalAudio.PlayOneShot(SEManagerScript.shipExplode);

            switch (type)
            {
                case "large":
                    spawnTwoAsteroids(spawnManagerScript.mediumpAsteroid, spawnManagerScript.PowerUpAsteroid);
                    break;
                case "medium":
                    spawnTwoAsteroids(spawnManagerScript.smallAsteroid, spawnManagerScript.smallAsteroid);
                    break;
            }

            gameManagerScript.inGameAsteroids.Remove(gameObject);
            Destroy(gameObject);

            //spawn power up
            if (type == "powerUp")
            {
                GameObject toAdd = Instantiate(powerUp,
                    new Vector3(transform.position.x, powerUpManagerScript.powerUpyValue, transform.position.z),
                    powerUp.transform.rotation);

                gameManagerScript.inGameIndicators.Add(toAdd);
            }
        }
    }

    void spawnTwoAsteroids(GameObject asteroid1, GameObject asteroid2)
    {
        GameObject toAdd1 = Instantiate(asteroid1, transform.position, asteroid1.transform.rotation);
        GameObject toAdd2 = Instantiate(asteroid2, transform.position, asteroid2.transform.rotation);
        gameManagerScript.inGameAsteroids.Add(toAdd1);
        gameManagerScript.inGameAsteroids.Add(toAdd2);
    }
}
