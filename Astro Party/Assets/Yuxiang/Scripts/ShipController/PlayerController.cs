using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int speed = 1000;
    public int maxVelocity = 500;
    float rotatingSpeed = 5f;
    bool rotating;

    public KeyCode turn;
    public KeyCode shoot;

    Rigidbody playerRb;

    public bool shootDisable;

    int dash;

    private void Start()
    {
        playerRb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(shoot) && !shootDisable)
        {
            GetComponent<MutualShip>().fire();
        }

        if (Input.GetKeyDown(turn))
        {
            rotating = true;
            if (dash > 0 && playerRb.constraints != RigidbodyConstraints.FreezeAll)
            {
                transform.Rotate(0, 90, 0);
                //change velocity
                playerRb.velocity = new Vector3(playerRb.velocity.z, playerRb.velocity.y, -playerRb.velocity.x);

                ////translate
                //float angle = transform.rotation.ToEulerAngles().y;
                //transform.position = new Vector3(transform.position.x + 100 * Mathf.Sin(angle),
                //    transform.position.y, transform.position.z + 100 * Mathf.Cos(angle));

                //add impulse
                playerRb.AddRelativeForce(new Vector3(0, 0, speed), ForceMode.Force);

                dash = 0;
            }
            else
            {
                StartCoroutine("dashCountDown");
            }
        }
        if (Input.GetKeyUp(turn))
        {
            rotating = false;
        }

        if (rotating && playerRb.constraints != RigidbodyConstraints.FreezeAll)
        {
            playerRb.freezeRotation = false;
            transform.Rotate(0, rotatingSpeed, 0);
            playerRb.freezeRotation = true;
        }

        playerRb.AddRelativeForce(new Vector3(0, 0, speed), ForceMode.Force);

        if (playerRb.velocity.magnitude > maxVelocity)
        {
            //Debug.Log(playerRb.velocity);
            playerRb.velocity = playerRb.velocity.normalized * maxVelocity;
        }
    }

    IEnumerator dashCountDown()
    {
        dash ++;
        yield return new WaitForSeconds(0.25f);
        dash --;
        dash = Mathf.Max(0, dash);
    }
}
