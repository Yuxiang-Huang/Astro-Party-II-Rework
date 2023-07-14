using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyBullet : MonoBehaviour
{
    public int id;
    public int team;

    Rigidbody Rb;
    int speed = 1000;

    bool attacked;

    public Renderer rend;
    public Material blue1;
    public Material red2;
    public Material yellow3;
    public Material cyan4;
    public Material green5;

    // Start is called before the first frame update
    void Start()
    {
        id = GetComponent<Identification>().id;
        team = GetComponent<Identification>().team;

        Rb = GetComponent<Rigidbody>();
        Rb.AddRelativeForce(new Vector3(0, 0, speed), ForceMode.VelocityChange);

        switch (id)
        {
            case 1:
                rend.material = blue1;
                break;
            case 2:
                rend.material = red2;
                break;
            case 3:
                rend.material = yellow3;
                break;
            case 4:
                rend.material = cyan4;
                break;
            case 5:
                rend.material = green5;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!attacked)
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
                    collision.gameObject.GetComponent<PilotPlayerController>().kill(id, team);
                }
                else
                {
                    collision.gameObject.GetComponent<BotPilotMove>().kill(id, team);
                }
            }

            if (collision.gameObject.CompareTag("Ship"))
            {
                collision.gameObject.GetComponent<MutualShip>().damage(id, team);
            }

            if (!collision.gameObject.CompareTag("Floor"))
            {
                bool destroy = true;

                if (collision.gameObject.GetComponent<BouncyBullet>() != null)
                {
                    if (collision.gameObject.GetComponent<BouncyBullet>().id == id)
                    {
                        destroy = false;
                    }
                }

                if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Breakable")
                    || collision.gameObject.CompareTag("Asteroid") || collision.gameObject.CompareTag("Portal"))
                {
                    destroy = false;
                }

                if (destroy)
                {
                    Destroy(gameObject);
                    attacked = true;
                }
            }
        }
    }
}
