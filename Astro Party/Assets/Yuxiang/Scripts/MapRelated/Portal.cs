using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Renderer rend;

    public GameObject pair;

    List<GameObject> waitList;
    float waitTime = 0.2f;

    public int offSet = 75;

    // Start is called before the first frame update
    void Start()
    {
        waitList = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ship") || collision.gameObject.CompareTag("Pilot") ||
            collision.gameObject.CompareTag("Bullet") || collision.gameObject.CompareTag("BouncyBullet"))
        {
            float angle = transform.rotation.ToEulerAngles().y;
            float pairAngle = pair.transform.rotation.ToEulerAngles().y;

            //offset of ship to portal
            if (! waitList.Contains(collision.gameObject))
            {
                waitList.Add(collision.gameObject);
                StartCoroutine("wait");

                Vector3 dif = collision.transform.position - transform.position;
                dif.y = collision.transform.position.y;

                collision.transform.position = Quaternion.AngleAxis(180 * (pairAngle - angle) / Mathf.PI, Vector3.up)
        * dif + pair.transform.position;

                if (collision.gameObject.CompareTag("Ship") ||
            collision.gameObject.CompareTag("Bullet") || collision.gameObject.CompareTag("BouncyBullet"))
                {
                    collision.transform.Rotate(new Vector3(0, (pairAngle - angle) * 180 / Mathf.PI + 180, 0));
                }
                else
                {
                    collision.transform.Rotate(new Vector3(0, 0, (pairAngle - angle) * 180 / Mathf.PI + 180));
                }

                //velocity for bullets
                if (collision.gameObject.CompareTag("Bullet") || collision.gameObject.CompareTag("BouncyBullet"))
                {
                    collision.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                    collision.gameObject.GetComponent<Rigidbody>().AddForce(1000 * collision.transform.forward,
                        ForceMode.VelocityChange);
                }

                //add a little more offset
                while (pairAngle < 0)
                {
                    pairAngle += 2 * Mathf.PI;
                }

                bool reverse = false;

                if (pairAngle < Mathf.PI)
                {
                    pairAngle = Mathf.PI / 2 - pairAngle;
                }
                else
                {
                    pairAngle -= Mathf.PI;
                    pairAngle = Mathf.PI / 2 - pairAngle;
                    reverse = true;
                }

                if (reverse)
                {
                    collision.transform.position -= new Vector3(Mathf.Cos(pairAngle) * offSet, 0, Mathf.Sin(pairAngle) * offSet);
                }
                else
                {
                    collision.transform.position += new Vector3(Mathf.Cos(pairAngle) * offSet, 0, Mathf.Sin(pairAngle) * offSet);
                }
            }
        }
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(waitTime);
        waitList.RemoveAt(waitList.Count - 1);
    }
}
