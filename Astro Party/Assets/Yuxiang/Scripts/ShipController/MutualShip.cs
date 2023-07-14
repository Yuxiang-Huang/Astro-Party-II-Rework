using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MutualShip : MonoBehaviour
{
    public int id;
    public int team;

    public bool attacked;
    public string shootMode = "normal";
    public bool tripleShot;
    public bool hasShield;

    int speed = 500;
    int bulletDis = 50;
    public float bulletAnimationPos;
    public GameObject[] bulletAnimation;
    public float reloadTime;
    public int ammo;
    public int powerUpUsed;

    public float freezeTime;
    bool freezed;

    Rigidbody playerRb;
    AudioSource playerAudio;

    ScoreManager scoreManagerScript;
    GameManager gameManagerScript;
    PowerUpManager powerUpManagerScript;
    SEManager SEManagerScript;
    HighlightModeManager highlightModeManagerScript;

    public GameObject pilot;

    int speedBoostInt = 300;
    public GameObject jouster1;
    public GameObject jouster2;
    public GameObject sideCannons;
    public GameObject shield;
    public GameObject freezeCube;
    public GameObject crown;
    public Renderer crownRend;

    public Renderer rend;
    public Material blue1;
    public Material red2;
    public Material yellow3;
    public Material cyan4;
    public Material green5;

    public bool highlighed;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAudio = GetComponent<AudioSource>();

        if (shootMode != "normal")
        {
            foreach (GameObject curr in bulletAnimation)
            {
                curr.SetActive(false);
            }
        }

        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
        scoreManagerScript = GameObject.Find("Score Manager").GetComponent<ScoreManager>();
        powerUpManagerScript = GameObject.Find("PowerUp Manager").GetComponent<PowerUpManager>();
        SEManagerScript = GameObject.Find("SoundEffect Manager").GetComponent<SEManager>();
        highlightModeManagerScript = GameObject.Find("Highlight Manager").GetComponent<HighlightModeManager>();

        ammo = 3;

        //attached
        jouster1.SetActive(false);
        jouster2.SetActive(false);

        if (tripleShot)
        {
            sideCannons.SetActive(true);
        }
        else
        {
            sideCannons.SetActive(false);
        }

        if (hasShield)
        {
            shield.SetActive(true);
        }
        else
        {
            shield.SetActive(false);
        }

        freezeCube.SetActive(false);

        //setColor
        switch (id)
        {
            case 1:
                rend.material = blue1;
                crownRend.material = blue1;
                break;
            case 2:
                rend.material = red2;
                crownRend.material = red2;
                break;
            case 3:
                rend.material = yellow3;
                crownRend.material = yellow3;
                break;
            case 4:
                rend.material = cyan4;
                crownRend.material = cyan4;
                break;
            case 5:
                rend.material = green5;
                crownRend.material = green5;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //frozen
        if (freezeTime > 0)
        {
            //no velocity when frozen
            GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            freezeTime -= Time.deltaTime;
        }
        //freeze
        if (!freezed && freezeTime > 0)
        {
            if (GetComponent<PlayerController>() != null)
            {
                GetComponent<PlayerController>().shootDisable = true;
                playerRb.constraints = RigidbodyConstraints.FreezeAll;
            }
            else if (GetComponent<BotMove>() != null)
            {

                GetComponent<BotMove>().agent.enabled = false;
                GetComponent<BotMove>().disable = true;
            }
            else
            {
                GetComponent<BotMove1>().disable = true;
            }

            freezeCube.SetActive(true);
            freezed = true;
        }

        //unfreeze
        if (freezed && freezeTime <= 0)
        {
            if (GetComponent<PlayerController>() != null)
            {
                playerRb.constraints = RigidbodyConstraints.FreezeRotation;
                GetComponent<PlayerController>().shootDisable = false;
            }
            else
            {
                playerRb.constraints = RigidbodyConstraints.FreezeRotation;
                if (GetComponent<BotMove>() != null)
                {
                    GetComponent<BotMove>().agent.enabled = true;
                    GetComponent<BotMove>().disable = false;
                    GetComponent<BotMove>().traceTime = 0;
                }
                else
                {
                    GetComponent<BotMove1>().disable = false;
                }
            }

            freezeCube.SetActive(false);
            freezed = false;
        }

        //regenerate ammo
        if (reloadTime > 0)
        {
            reloadTime -= Time.deltaTime;
            reloadTime = Mathf.Max(reloadTime, 0);
            if (reloadTime == 0)
            {
                ammo++;
                ammo = Mathf.Min(ammo, 3);

                if (shootMode == "normal")
                {
                    bulletAnimation[ammo - 1].SetActive(true);
                }
            }
        }

        if (ammo < 3 && reloadTime == 0)
        {
            reloadTime = 2;
        }

        //bullet animation
        bulletAnimationPos += Time.deltaTime;

        for (int i = 0; i < bulletAnimation.Length; i++)
        {
            bulletAnimation[i].transform.position = new Vector3(transform.position.x +
                Mathf.Cos(bulletAnimationPos + i * 2 * Mathf.PI / 3) * bulletDis,
                transform.position.y, transform.position.z + Mathf.Sin(bulletAnimationPos + i * 2 * Mathf.PI / 3) * bulletDis);
        }

        //check for crown in highlight
        if (scoreManagerScript.gameMode == "highlight")
        {
            if (highlighed)
            {
                crown.SetActive(true);
                crown.transform.Rotate(new Vector3(0, 2, 0));
            }
        }
    }

    public void fire()
    {
        float angle = transform.rotation.ToEulerAngles().y;

        if (shootMode == "normal")
        {
            //ammo change
            if (ammo > 0)
            {
                ammo--;

                bulletAnimation[ammo].SetActive(false);

                if (reloadTime == 0)
                {
                    reloadTime = 2;
                }

                shoot(angle, powerUpManagerScript.bullet);

                //Sound effect
                playerAudio.PlayOneShot(SEManagerScript.bulletSound);
            }
        }
        else
        {
            if (shootMode == "Laser Beam")
            {
                //5000 is half the length of laserbeam
                GameObject myLaser = Instantiate(powerUpManagerScript.laser, transform.position +
                new Vector3((bulletDis + 1000) * Mathf.Sin(angle), transform.position.y, (bulletDis + 1000) * Mathf.Cos(angle)),
                transform.rotation);

                //setting the script varibles
                myLaser.GetComponent<Laser>().id = id;
                myLaser.GetComponent<Laser>().team = team;

                //Sound effect
                playerAudio.PlayOneShot(SEManagerScript.laserSound);

                //Freeze after using laser
                StartCoroutine("laserFreeze");
            }

            else if (shootMode == "Scatter Shot")
            {
                int numOfShots = 16;

                float ran = Random.Range(0, 360);

                transform.Rotate(0, ran, 0);

                for (int i = 0; i < numOfShots; i++)
                {
                    float angleNow = transform.rotation.ToEulerAngles().y;

                    GameObject myBullet = Instantiate(powerUpManagerScript.bullet, transform.position +
                new Vector3(bulletDis * Mathf.Sin(angleNow), 0, bulletDis * Mathf.Cos(angleNow)), transform.rotation);

                    transform.Rotate(0, 360 / numOfShots, 0);

                    //setting the script varibles
                    myBullet.GetComponent<Identification>().id = id;
                    myBullet.GetComponent<Identification>().team = team;

                    gameManagerScript.needToClear.Add(myBullet);
                }

                transform.Rotate(0, 360 - ran, 0);

                //Sound effect
                playerAudio.PlayOneShot(SEManagerScript.bulletSound);
            }

            else if (shootMode == "Freezer")
            {
                GameObject myFreezer = Instantiate(powerUpManagerScript.freezer, transform.position, transform.rotation);

                //Sound effect
                playerAudio.PlayOneShot(SEManagerScript.freezerSound);

                //setting the script varibles
                myFreezer.GetComponent<Freezer>().id = id;
                myFreezer.GetComponent<Freezer>().team = team;
            }

            else if (shootMode == "Proximity Mine")
            {
                GameObject myMine = Instantiate(powerUpManagerScript.mine, transform.position, transform.rotation);

                //setting the script varibles
                myMine.GetComponent<Mine>().id = id;
                myMine.GetComponent<Mine>().team = team;

                gameManagerScript.needToClear.Add(myMine);
            }

            else if (shootMode == "Bouncy Bullet")
            {
                shoot(angle, powerUpManagerScript.bouncyBullet);

                //Sound effect
                playerAudio.PlayOneShot(SEManagerScript.bulletSound);
            }

            else if (shootMode == "Jouster")
            {
                jouster1.SetActive(true);
                jouster2.SetActive(true);

                jouster1.GetComponent<Jouster>().health = 3;
                jouster2.GetComponent<Jouster>().health = 3;

                StartCoroutine("speedBoost");

                playerRb.AddRelativeForce(new Vector3(0, 0, speed * 30), ForceMode.Force);
            }

            //check for powerup usage
            powerUpUsed++;
            if (powerUpUsed == powerUpManagerScript.maxPowerUp)
            {
                shootMode = "normal";

                ammo = 3;

                foreach (GameObject curr in bulletAnimation)
                {
                    curr.SetActive(true);
                }

                powerUpUsed = 0;
            }
        }
    }

    //shoot for bullet, bouncy bullet and
    void shoot(float angle, GameObject projectile)
    {
        if (tripleShot)
        {
            GameObject sideBullet1 = Instantiate(projectile, transform.position +
        new Vector3(bulletDis * Mathf.Sin(angle + 1), 0, bulletDis * Mathf.Cos(angle + 1)),
        transform.rotation);

            //setting the script varibles
            sideBullet1.GetComponent<Identification>().id = id;
            if (gameManagerScript.suicidalBullet)
            {
                sideBullet1.GetComponent<Identification>().team = -1;
            }
            else
            {
                sideBullet1.GetComponent<Identification>().team = team;
            }

            GameObject sideBullet2 = Instantiate(projectile, transform.position +
        new Vector3(bulletDis * Mathf.Sin(angle - 1), 0, bulletDis * Mathf.Cos(angle - 1)),
        transform.rotation);

            //setting the script varibles
            sideBullet2.GetComponent<Identification>().id = id;
            if (gameManagerScript.suicidalBullet)
            {
                sideBullet2.GetComponent<Identification>().team = -1;
            }
            else
            {
                sideBullet2.GetComponent<Identification>().team = team;
            }

            //add to be destroyed after round ends
            gameManagerScript.needToClear.Add(sideBullet1);
            gameManagerScript.needToClear.Add(sideBullet2);
        }

        GameObject myBullet = Instantiate(projectile, transform.position +
        new Vector3(bulletDis * Mathf.Sin(angle), 0, bulletDis * Mathf.Cos(angle)),
        transform.rotation);

        //setting the script varibles
        myBullet.GetComponent<Identification>().id = id;
        if (gameManagerScript.suicidalBullet)
        {
            myBullet.GetComponent<Identification>().team = -1;
        }
        else
        {
            myBullet.GetComponent<Identification>().team = team;
        }
        gameManagerScript.needToClear.Add(myBullet);
    }
 
    IEnumerator laserFreeze()
    {
        //freeze a few
        playerRb.constraints = RigidbodyConstraints.FreezeAll;
        yield return new WaitForSeconds(0.3f);
        playerRb.constraints = RigidbodyConstraints.FreezeRotation;

        //opposite force not for bot
        if (GetComponent<PlayerController>() != null)
        {
            playerRb.AddRelativeForce(new Vector3(0, 0, -speed * 30), ForceMode.Force);
        }
    }

    IEnumerator speedBoost()
    {
        speed += speedBoostInt;

        //for player
        if (GetComponent<PlayerController>() != null)
        {
            GetComponent<PlayerController>().speed += speedBoostInt;
            GetComponent<PlayerController>().maxVelocity += speedBoostInt;

            yield return new WaitForSeconds(7f);

            GetComponent<PlayerController>().speed -= speedBoostInt;
            GetComponent<PlayerController>().maxVelocity -= speedBoostInt;
        }

        //for Bot
        if (GetComponent<BotMove>() != null)
        {
            GetComponent<NavMeshAgent>().speed += speedBoostInt;

            yield return new WaitForSeconds(7f);

            GetComponent<NavMeshAgent>().speed -= speedBoostInt;
        }

        //for Bot 1
        if (GetComponent<BotMove1>() != null)
        {
            GetComponent<BotMove1>().speed += speedBoostInt;
            GetComponent<BotMove1>().maxVelocity += speedBoostInt;

            yield return new WaitForSeconds(7f);

            GetComponent<BotMove1>().speed -= 100;
            GetComponent<BotMove1>().maxVelocity -= speedBoostInt;
        }
    }

    public void damage(int otherID, int otherTeam)
    {
        bool toKill = true;
        //Friendly Fire 
        if (!scoreManagerScript.friendlyFire)
        {
            if (otherTeam == team)
            {
                toKill = false;
            }
        }

        if (attacked)
        {
            toKill = false;
        }

        if (toKill && hasShield)
        {
            hasShield = false;
            shield.SetActive(false);
            toKill = false;
        }

        if (toKill)
        {
            attacked = true;

            //ship explode sound effect
            SEManagerScript.generalAudio.PlayOneShot(SEManagerScript.shipExplode);

            powerUpManagerScript.dropItem(this);

            if (scoreManagerScript.gameMode == "ship")
            {
                if (otherID == id)
                {
                    suicide();
                }
                else
                {
                    earnPoint(otherID);
                }
                Destroy(this.gameObject);
            }
            else if (scoreManagerScript.gameMode == "pilot" || scoreManagerScript.gameMode == "highlight")
            {
                spawnPilot();
            }

            //for highlight mode
            if (scoreManagerScript.gameMode == "highlight")
            {
                if (highlighed)
                {
                    highlightModeManagerScript.assign(otherID, this.transform.position, id == otherID || otherID == -1);
                }
            }
        }
    }

    void spawnPilot()
    {
        GameObject myPilot = Instantiate(pilot, transform.position, transform.rotation);
        myPilot.transform.Rotate(90, 0, 0);
        if (myPilot.GetComponent<PilotPlayerController>() != null)
        {
            myPilot.GetComponent<PilotPlayerController>().turn = GetComponent<PlayerController>().turn;
            myPilot.GetComponent<PilotPlayerController>().move = GetComponent<PlayerController>().shoot;
            myPilot.GetComponent<PilotPlayerController>().StartCoroutine("respawn");
            myPilot.GetComponent<PilotPlayerController>().team = team;
            myPilot.GetComponent<PilotPlayerController>().id = id;
        } else if (myPilot.GetComponent<BotPilotMove>() != null)
        {
            myPilot.GetComponent<BotPilotMove>().StartCoroutine("respawn");
            myPilot.GetComponent<BotPilotMove>().team = team;
            myPilot.GetComponent<BotPilotMove>().id = id;
        }

        gameManagerScript.inGameShips[team].Add(myPilot);

        Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Jouster"))
        {
            Jouster script = other.gameObject.GetComponent<Jouster>();
            damage(script.id, script.team);
            script.health = 0;
            other.gameObject.SetActive(false);
        }

        if (other.gameObject.CompareTag("PowerUp"))
        {
            string powerUpName = other.gameObject.GetComponent<PowerUp>().powerUpName;
            if (powerUpName == "Triple Shot")
            {
                tripleShot = true;
                sideCannons.SetActive(true);
            }
            else if (powerUpName == "Shield")
            {
                hasShield = true;
                shield.SetActive(true);
            }
            else
            {
                shootMode = powerUpName;

                powerUpUsed = 0;

                foreach (GameObject curr in bulletAnimation)
                {
                    curr.SetActive(false);
                }
            }
            gameManagerScript.inGameIndicators.Remove(other.gameObject);
            Destroy(other.gameObject);
        }

        //for highlight mode
        if (other.gameObject.CompareTag("Crown"))
        {
            highlighed = true;
            Destroy(other.transform.parent.gameObject);
            gameManagerScript.highlightModeManagerScript.inGameCrown = null;
        }
    }
}
