using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    public int id;
    public int team;
    public float explodingRadius = 500;
    public bool triggered;

    public GameObject killerPlane;

    GameManager gameManagerScript;
    SEManager SEManagerScript;

    // Start is called before the first frame update
    void Start()
    {
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
        SEManagerScript = GameObject.Find("SoundEffect Manager").GetComponent<SEManager>();
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
                    if (ship.GetComponent<MutualShip>().team != team)
                    {
                        if (distance(ship.transform.position, transform.position) < explodingRadius)
                        {
                            StartCoroutine("trigger");
                        }
                    }
                }
            }
        }

        if (triggered)
        {
            transform.Rotate(new Vector3(0, 30, 0));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("Bullet") || other.gameObject.CompareTag("Laser") ||
            other.gameObject.CompareTag("Freezer") || other.gameObject.CompareTag("Killer"))
        {
            StartCoroutine("trigger");
        }
    }

    IEnumerator trigger()
    {
        triggered = true;
        yield return new WaitForSeconds(1f);

        //sound effect
        SEManagerScript.generalAudio.PlayOneShot(SEManagerScript.mineSound);

        GameObject myExplode = Instantiate(killerPlane, transform.position, transform.rotation);
        myExplode.GetComponent<Killer>().id = id;
        myExplode.GetComponent<Killer>().StartCoroutine("selfDestruct");
        Destroy(gameObject);
    }

    float distance(Vector3 ship1, Vector3 ship2)
    {
        return Mathf.Sqrt(Mathf.Pow((ship1.x - ship2.x), 2) +
            Mathf.Pow((ship1.z - ship2.z), 2));
    }
}
